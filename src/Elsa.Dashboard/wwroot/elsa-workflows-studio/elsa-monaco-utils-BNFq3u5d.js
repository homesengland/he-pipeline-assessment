const E_TIMEOUT = new Error('timeout while waiting for mutex to become available');
const E_ALREADY_LOCKED = new Error('mutex already locked');
const E_CANCELED = new Error('request for lock canceled');

var __awaiter$2 = (undefined && undefined.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
class Semaphore {
    constructor(_maxConcurrency, _cancelError = E_CANCELED) {
        this._maxConcurrency = _maxConcurrency;
        this._cancelError = _cancelError;
        this._queue = [];
        this._waiters = [];
        if (_maxConcurrency <= 0) {
            throw new Error('semaphore must be initialized to a positive value');
        }
        this._value = _maxConcurrency;
    }
    acquire() {
        const locked = this.isLocked();
        const ticketPromise = new Promise((resolve, reject) => this._queue.push({ resolve, reject }));
        if (!locked)
            this._dispatch();
        return ticketPromise;
    }
    runExclusive(callback) {
        return __awaiter$2(this, void 0, void 0, function* () {
            const [value, release] = yield this.acquire();
            try {
                return yield callback(value);
            }
            finally {
                release();
            }
        });
    }
    waitForUnlock() {
        return __awaiter$2(this, void 0, void 0, function* () {
            if (!this.isLocked()) {
                return Promise.resolve();
            }
            const waitPromise = new Promise((resolve) => this._waiters.push({ resolve }));
            return waitPromise;
        });
    }
    isLocked() {
        return this._value <= 0;
    }
    /** @deprecated Deprecated in 0.3.0, will be removed in 0.4.0. Use runExclusive instead. */
    release() {
        if (this._maxConcurrency > 1) {
            throw new Error('this method is unavailable on semaphores with concurrency > 1; use the scoped release returned by acquire instead');
        }
        if (this._currentReleaser) {
            const releaser = this._currentReleaser;
            this._currentReleaser = undefined;
            releaser();
        }
    }
    cancel() {
        this._queue.forEach((ticket) => ticket.reject(this._cancelError));
        this._queue = [];
    }
    _dispatch() {
        const nextTicket = this._queue.shift();
        if (!nextTicket)
            return;
        let released = false;
        this._currentReleaser = () => {
            if (released)
                return;
            released = true;
            this._value++;
            this._resolveWaiters();
            this._dispatch();
        };
        nextTicket.resolve([this._value--, this._currentReleaser]);
    }
    _resolveWaiters() {
        this._waiters.forEach((waiter) => waiter.resolve());
        this._waiters = [];
    }
}

var __awaiter$1 = (undefined && undefined.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
class Mutex {
    constructor(cancelError) {
        this._semaphore = new Semaphore(1, cancelError);
    }
    acquire() {
        return __awaiter$1(this, void 0, void 0, function* () {
            const [, releaser] = yield this._semaphore.acquire();
            return releaser;
        });
    }
    runExclusive(callback) {
        return this._semaphore.runExclusive(() => callback());
    }
    isLocked() {
        return this._semaphore.isLocked();
    }
    waitForUnlock() {
        return this._semaphore.waitForUnlock();
    }
    /** @deprecated Deprecated in 0.3.0, will be removed in 0.4.0. Use runExclusive instead. */
    release() {
        this._semaphore.release();
    }
    cancel() {
        return this._semaphore.cancel();
    }
}

var __awaiter = (undefined && undefined.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
// eslint-disable-next-line @typescript-eslint/explicit-module-boundary-types
function withTimeout(sync, timeout, timeoutError = E_TIMEOUT) {
    return {
        acquire: () => new Promise((resolve, reject) => __awaiter(this, void 0, void 0, function* () {
            let isTimeout = false;
            const handle = setTimeout(() => {
                isTimeout = true;
                reject(timeoutError);
            }, timeout);
            try {
                const ticket = yield sync.acquire();
                if (isTimeout) {
                    const release = Array.isArray(ticket) ? ticket[1] : ticket;
                    release();
                }
                else {
                    clearTimeout(handle);
                    resolve(ticket);
                }
            }
            catch (e) {
                if (!isTimeout) {
                    clearTimeout(handle);
                    reject(e);
                }
            }
        })),
        runExclusive(callback) {
            return __awaiter(this, void 0, void 0, function* () {
                let release = () => undefined;
                try {
                    const ticket = yield this.acquire();
                    if (Array.isArray(ticket)) {
                        release = ticket[1];
                        return yield callback(ticket[0]);
                    }
                    else {
                        release = ticket;
                        return yield callback();
                    }
                }
                finally {
                    release();
                }
            });
        },
        /** @deprecated Deprecated in 0.3.0, will be removed in 0.4.0. Use runExclusive instead. */
        release() {
            sync.release();
        },
        cancel() {
            return sync.cancel();
        },
        waitForUnlock: () => sync.waitForUnlock(),
        isLocked: () => sync.isLocked(),
    };
}

// eslint-disable-next-lisne @typescript-eslint/explicit-module-boundary-types
function tryAcquire(sync, alreadyAcquiredError = E_ALREADY_LOCKED) {
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    return withTimeout(sync, 0, alreadyAcquiredError);
}

const win = window;
const require = win.require;
var EditorVariables = [];
let isInitialized;
const mutex = new Mutex();
async function initializeMonacoWorker(libPath) {
    return await mutex.runExclusive(async () => {
        if (isInitialized) {
            return win.monaco;
        }
        const origin = document.location.origin;
        const baseUrl = libPath.startsWith('http') ? libPath : `${origin}/${libPath}`;
        require.config({ paths: { 'vs': `${baseUrl}/vs` } });
        win.MonacoEnvironment = { getWorkerUrl: () => proxy };
        let proxy = URL.createObjectURL(new Blob([`
	self.MonacoEnvironment = {
		baseUrl: '${baseUrl}'
	};
	importScripts('${baseUrl}/vs/base/worker/workerMain.js');
`], { type: 'text/javascript' }));
        return new Promise(resolve => {
            require(["vs/editor/editor.main"], () => {
                isInitialized = true;
                registerLiquid(win.monaco);
                registerSql(win.monaco);
                resolve(win.monaco);
            });
        });
    });
}
function registerLiquid(monaco) {
    monaco.languages.register({ id: 'liquid' });
    monaco.languages.registerCompletionItemProvider('liquid', {
        provideCompletionItems: () => {
            const autocompleteProviderItems = [];
            const keywords = ['assign', 'capture', 'endcapture', 'increment', 'decrement',
                'if', 'else', 'elsif', 'endif', 'for', 'endfor', 'break',
                'continue', 'limit', 'offset', 'range', 'reversed', 'cols',
                'case', 'endcase', 'when', 'block', 'endblock', 'true', 'false',
                'in', 'unless', 'endunless', 'cycle', 'tablerow', 'endtablerow',
                'contains', 'startswith', 'endswith', 'comment', 'endcomment',
                'raw', 'endraw', 'editable', 'endentitylist', 'endentityview', 'endinclude',
                'endmarker', 'entitylist', 'entityview', 'forloop', 'image', 'include',
                'marker', 'outputcache', 'plugin', 'style', 'text', 'widget',
                'abs', 'append', 'at_least', 'at_most', 'capitalize', 'ceil', 'compact',
                'concat', 'date', 'default', 'divided_by', 'downcase', 'escape',
                'escape_once', 'first', 'floor', 'join', 'last', 'lstrip', 'map',
                'minus', 'modulo', 'newline_to_br', 'plus', 'prepend', 'remove',
                'remove_first', 'replace', 'replace_first', 'reverse', 'round',
                'rstrip', 'size', 'slice', 'sort', 'sort_natural', 'split', 'strip',
                'strip_html', 'strip_newlines', 'times', 'truncate', 'truncatewords',
                'uniq', 'upcase', 'url_decode', 'url_encode'];
            for (let i = 0; i < keywords.length; i++) {
                autocompleteProviderItems.push({ 'label': keywords[i], kind: monaco.languages.CompletionItemKind.Keyword });
            }
            return { suggestions: autocompleteProviderItems };
        }
    });
}
function registerSql(monaco) {
    monaco.languages.registerCompletionItemProvider('sql', {
        triggerCharacters: ["@"],
        provideCompletionItems: (model, position) => {
            const word = model.getWordUntilPosition(position);
            const autocompleteProviderItems = [];
            for (const varible of EditorVariables) {
                autocompleteProviderItems.push({
                    label: `${varible.variableName}: ${varible.type}`,
                    kind: monaco.languages.CompletionItemKind.Variable,
                    insertText: varible.variableName
                });
            }
            return { suggestions: autocompleteProviderItems };
        }
    });
}

export { EditorVariables as E, initializeMonacoWorker as i };
//# sourceMappingURL=elsa-monaco-utils-BNFq3u5d.js.map

//# sourceMappingURL=elsa-monaco-utils-BNFq3u5d.js.map
import { a as activityIconProvider } from './elsa-client-ecb85def.js';
import { e as eventBus } from './event-bus-6625fc04.js';
import { E as EventTypes, S as SyntaxNames } from './index-0f68dbd6.js';
import { h } from './index-ea213ee1.js';
import { h as htmlEncode, g as getOrCreateProperty, s as setActivityModelProperty, p as parseJson } from './utils-db96334c.js';
import { c as createCommonjsModule, g as getDefaultExportFromCjs } from './_commonjsHelpers-6cb8dacb.js';
import { c as cronstrue } from './cronstrue-37d55fa1.js';

class ConfirmDialogService {
  show(caption, message) {
    const context = { caption, message, promise: null };
    eventBus.emit(EventTypes.ShowConfirmDialog, this, context);
    return context.promise;
  }
  hide() {
    eventBus.emit(EventTypes.HideConfirmDialog, this);
  }
}
const confirmDialogService = new ConfirmDialogService();

class ToastNotificationService {
  show(title, message, autoCloseIn) {
    const options = { title, message, autoCloseIn };
    eventBus.emit(EventTypes.ShowToastNotification, this, options);
  }
  hide() {
    eventBus.emit(EventTypes.HideToastNotification, this);
  }
}
const toastNotificationService = new ToastNotificationService();

class FeaturesDataManager {
  constructor() {
    this.supportedFeatures = {
      workflowLayout: 'workflowLayout'
    };
    this.getFeatureList = () => {
      return Object.keys(this.features);
    };
    this.getUIFeatureList = () => {
      return Object.keys(this.features).filter(feature => this.features[feature].ui);
    };
    this.getFeatureConfig = (name) => {
      const value = localStorage.getItem(`elsa.properties.${name}`);
      const enabled = localStorage.getItem(`elsa.properties.${name}.enabled`);
      const feature = this.features[name];
      if (feature) {
        return Object.assign(Object.assign({}, feature), { value: value === null ? feature.defaultValue : value, 'enabled': enabled === null ? feature.enabled : enabled === 'true' });
      }
    };
    this.setFeatureConfig = (name, value) => {
      const feature = this.features[name];
      if (feature) {
        localStorage.setItem(`elsa.properties.${name}`, value);
      }
    };
    this.setEnableStatus = (name, value) => {
      const feature = this.features[name];
      if (feature) {
        localStorage.setItem(`elsa.properties.${name}.enabled`, `${value}`);
        console.log(`elsa.properties.${name}-enabled`, value);
      }
    };
  }
  initialize(elsaStudio) {
    if (this.initialized)
      return;
    this.elsaStudio = elsaStudio;
    this.initialized = true;
    this.features = elsaStudio.features || {};
  }
}
const featuresDataManager = new FeaturesDataManager();

class IfPlugin {
  constructor() {
    eventBus.on(EventTypes.ActivityDesignDisplaying, this.onActivityDesignDisplaying);
  }
  onActivityDesignDisplaying(context) {
    const activityModel = context.activityModel;
    if (activityModel.type !== 'If')
      return;
    const props = activityModel.properties || [];
    const condition = props.find(x => x.name == 'Condition') || { name: 'Condition', expressions: { 'Literal': '' }, syntax: 'Literal' };
    const expression = condition.expressions[condition.syntax] || '';
    const description = activityModel.description;
    const bodyText = htmlEncode(description && description.length > 0 ? description : expression);
    context.bodyDisplay = `<p>${bodyText}</p>`;
  }
}

var code$2 = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.regexpCode = exports.getEsmExportName = exports.getProperty = exports.safeStringify = exports.stringify = exports.strConcat = exports.addCodeArg = exports.str = exports._ = exports.nil = exports._Code = exports.Name = exports.IDENTIFIER = exports._CodeOrName = void 0;
// eslint-disable-next-line @typescript-eslint/no-extraneous-class
class _CodeOrName {
}
exports._CodeOrName = _CodeOrName;
exports.IDENTIFIER = /^[a-z$_][a-z$_0-9]*$/i;
class Name extends _CodeOrName {
    constructor(s) {
        super();
        if (!exports.IDENTIFIER.test(s))
            throw new Error("CodeGen: name must be a valid identifier");
        this.str = s;
    }
    toString() {
        return this.str;
    }
    emptyStr() {
        return false;
    }
    get names() {
        return { [this.str]: 1 };
    }
}
exports.Name = Name;
class _Code extends _CodeOrName {
    constructor(code) {
        super();
        this._items = typeof code === "string" ? [code] : code;
    }
    toString() {
        return this.str;
    }
    emptyStr() {
        if (this._items.length > 1)
            return false;
        const item = this._items[0];
        return item === "" || item === '""';
    }
    get str() {
        var _a;
        return ((_a = this._str) !== null && _a !== void 0 ? _a : (this._str = this._items.reduce((s, c) => `${s}${c}`, "")));
    }
    get names() {
        var _a;
        return ((_a = this._names) !== null && _a !== void 0 ? _a : (this._names = this._items.reduce((names, c) => {
            if (c instanceof Name)
                names[c.str] = (names[c.str] || 0) + 1;
            return names;
        }, {})));
    }
}
exports._Code = _Code;
exports.nil = new _Code("");
function _(strs, ...args) {
    const code = [strs[0]];
    let i = 0;
    while (i < args.length) {
        addCodeArg(code, args[i]);
        code.push(strs[++i]);
    }
    return new _Code(code);
}
exports._ = _;
const plus = new _Code("+");
function str(strs, ...args) {
    const expr = [safeStringify(strs[0])];
    let i = 0;
    while (i < args.length) {
        expr.push(plus);
        addCodeArg(expr, args[i]);
        expr.push(plus, safeStringify(strs[++i]));
    }
    optimize(expr);
    return new _Code(expr);
}
exports.str = str;
function addCodeArg(code, arg) {
    if (arg instanceof _Code)
        code.push(...arg._items);
    else if (arg instanceof Name)
        code.push(arg);
    else
        code.push(interpolate(arg));
}
exports.addCodeArg = addCodeArg;
function optimize(expr) {
    let i = 1;
    while (i < expr.length - 1) {
        if (expr[i] === plus) {
            const res = mergeExprItems(expr[i - 1], expr[i + 1]);
            if (res !== undefined) {
                expr.splice(i - 1, 3, res);
                continue;
            }
            expr[i++] = "+";
        }
        i++;
    }
}
function mergeExprItems(a, b) {
    if (b === '""')
        return a;
    if (a === '""')
        return b;
    if (typeof a == "string") {
        if (b instanceof Name || a[a.length - 1] !== '"')
            return;
        if (typeof b != "string")
            return `${a.slice(0, -1)}${b}"`;
        if (b[0] === '"')
            return a.slice(0, -1) + b.slice(1);
        return;
    }
    if (typeof b == "string" && b[0] === '"' && !(a instanceof Name))
        return `"${a}${b.slice(1)}`;
    return;
}
function strConcat(c1, c2) {
    return c2.emptyStr() ? c1 : c1.emptyStr() ? c2 : str `${c1}${c2}`;
}
exports.strConcat = strConcat;
// TODO do not allow arrays here
function interpolate(x) {
    return typeof x == "number" || typeof x == "boolean" || x === null
        ? x
        : safeStringify(Array.isArray(x) ? x.join(",") : x);
}
function stringify(x) {
    return new _Code(safeStringify(x));
}
exports.stringify = stringify;
function safeStringify(x) {
    return JSON.stringify(x)
        .replace(/\u2028/g, "\\u2028")
        .replace(/\u2029/g, "\\u2029");
}
exports.safeStringify = safeStringify;
function getProperty(key) {
    return typeof key == "string" && exports.IDENTIFIER.test(key) ? new _Code(`.${key}`) : _ `[${key}]`;
}
exports.getProperty = getProperty;
//Does best effort to format the name properly
function getEsmExportName(key) {
    if (typeof key == "string" && exports.IDENTIFIER.test(key)) {
        return new _Code(`${key}`);
    }
    throw new Error(`CodeGen: invalid export name: ${key}, use explicit $id name mapping`);
}
exports.getEsmExportName = getEsmExportName;
function regexpCode(rx) {
    return new _Code(rx.toString());
}
exports.regexpCode = regexpCode;
//# sourceMappingURL=code.js.map
});

const code$3 = /*@__PURE__*/getDefaultExportFromCjs(code$2);

var scope = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.ValueScope = exports.ValueScopeName = exports.Scope = exports.varKinds = exports.UsedValueState = void 0;

class ValueError extends Error {
    constructor(name) {
        super(`CodeGen: "code" for ${name} not defined`);
        this.value = name.value;
    }
}
var UsedValueState;
(function (UsedValueState) {
    UsedValueState[UsedValueState["Started"] = 0] = "Started";
    UsedValueState[UsedValueState["Completed"] = 1] = "Completed";
})(UsedValueState || (exports.UsedValueState = UsedValueState = {}));
exports.varKinds = {
    const: new code$2.Name("const"),
    let: new code$2.Name("let"),
    var: new code$2.Name("var"),
};
class Scope {
    constructor({ prefixes, parent } = {}) {
        this._names = {};
        this._prefixes = prefixes;
        this._parent = parent;
    }
    toName(nameOrPrefix) {
        return nameOrPrefix instanceof code$2.Name ? nameOrPrefix : this.name(nameOrPrefix);
    }
    name(prefix) {
        return new code$2.Name(this._newName(prefix));
    }
    _newName(prefix) {
        const ng = this._names[prefix] || this._nameGroup(prefix);
        return `${prefix}${ng.index++}`;
    }
    _nameGroup(prefix) {
        var _a, _b;
        if (((_b = (_a = this._parent) === null || _a === void 0 ? void 0 : _a._prefixes) === null || _b === void 0 ? void 0 : _b.has(prefix)) || (this._prefixes && !this._prefixes.has(prefix))) {
            throw new Error(`CodeGen: prefix "${prefix}" is not allowed in this scope`);
        }
        return (this._names[prefix] = { prefix, index: 0 });
    }
}
exports.Scope = Scope;
class ValueScopeName extends code$2.Name {
    constructor(prefix, nameStr) {
        super(nameStr);
        this.prefix = prefix;
    }
    setValue(value, { property, itemIndex }) {
        this.value = value;
        this.scopePath = (0, code$2._) `.${new code$2.Name(property)}[${itemIndex}]`;
    }
}
exports.ValueScopeName = ValueScopeName;
const line = (0, code$2._) `\n`;
class ValueScope extends Scope {
    constructor(opts) {
        super(opts);
        this._values = {};
        this._scope = opts.scope;
        this.opts = { ...opts, _n: opts.lines ? line : code$2.nil };
    }
    get() {
        return this._scope;
    }
    name(prefix) {
        return new ValueScopeName(prefix, this._newName(prefix));
    }
    value(nameOrPrefix, value) {
        var _a;
        if (value.ref === undefined)
            throw new Error("CodeGen: ref must be passed in value");
        const name = this.toName(nameOrPrefix);
        const { prefix } = name;
        const valueKey = (_a = value.key) !== null && _a !== void 0 ? _a : value.ref;
        let vs = this._values[prefix];
        if (vs) {
            const _name = vs.get(valueKey);
            if (_name)
                return _name;
        }
        else {
            vs = this._values[prefix] = new Map();
        }
        vs.set(valueKey, name);
        const s = this._scope[prefix] || (this._scope[prefix] = []);
        const itemIndex = s.length;
        s[itemIndex] = value.ref;
        name.setValue(value, { property: prefix, itemIndex });
        return name;
    }
    getValue(prefix, keyOrRef) {
        const vs = this._values[prefix];
        if (!vs)
            return;
        return vs.get(keyOrRef);
    }
    scopeRefs(scopeName, values = this._values) {
        return this._reduceValues(values, (name) => {
            if (name.scopePath === undefined)
                throw new Error(`CodeGen: name "${name}" has no value`);
            return (0, code$2._) `${scopeName}${name.scopePath}`;
        });
    }
    scopeCode(values = this._values, usedValues, getCode) {
        return this._reduceValues(values, (name) => {
            if (name.value === undefined)
                throw new Error(`CodeGen: name "${name}" has no value`);
            return name.value.code;
        }, usedValues, getCode);
    }
    _reduceValues(values, valueCode, usedValues = {}, getCode) {
        let code = code$2.nil;
        for (const prefix in values) {
            const vs = values[prefix];
            if (!vs)
                continue;
            const nameSet = (usedValues[prefix] = usedValues[prefix] || new Map());
            vs.forEach((name) => {
                if (nameSet.has(name))
                    return;
                nameSet.set(name, UsedValueState.Started);
                let c = valueCode(name);
                if (c) {
                    const def = this.opts.es5 ? exports.varKinds.var : exports.varKinds.const;
                    code = (0, code$2._) `${code}${def} ${name} = ${c};${this.opts._n}`;
                }
                else if ((c = getCode === null || getCode === void 0 ? void 0 : getCode(name))) {
                    code = (0, code$2._) `${code}${c}${this.opts._n}`;
                }
                else {
                    throw new ValueError(name);
                }
                nameSet.set(name, UsedValueState.Completed);
            });
        }
        return code;
    }
}
exports.ValueScope = ValueScope;
//# sourceMappingURL=scope.js.map
});

const scope$1 = /*@__PURE__*/getDefaultExportFromCjs(scope);

var codegen = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.or = exports.and = exports.not = exports.CodeGen = exports.operators = exports.varKinds = exports.ValueScopeName = exports.ValueScope = exports.Scope = exports.Name = exports.regexpCode = exports.stringify = exports.getProperty = exports.nil = exports.strConcat = exports.str = exports._ = void 0;


var code_2 = code$2;
Object.defineProperty(exports, "_", { enumerable: true, get: function () { return code_2._; } });
Object.defineProperty(exports, "str", { enumerable: true, get: function () { return code_2.str; } });
Object.defineProperty(exports, "strConcat", { enumerable: true, get: function () { return code_2.strConcat; } });
Object.defineProperty(exports, "nil", { enumerable: true, get: function () { return code_2.nil; } });
Object.defineProperty(exports, "getProperty", { enumerable: true, get: function () { return code_2.getProperty; } });
Object.defineProperty(exports, "stringify", { enumerable: true, get: function () { return code_2.stringify; } });
Object.defineProperty(exports, "regexpCode", { enumerable: true, get: function () { return code_2.regexpCode; } });
Object.defineProperty(exports, "Name", { enumerable: true, get: function () { return code_2.Name; } });
var scope_2 = scope;
Object.defineProperty(exports, "Scope", { enumerable: true, get: function () { return scope_2.Scope; } });
Object.defineProperty(exports, "ValueScope", { enumerable: true, get: function () { return scope_2.ValueScope; } });
Object.defineProperty(exports, "ValueScopeName", { enumerable: true, get: function () { return scope_2.ValueScopeName; } });
Object.defineProperty(exports, "varKinds", { enumerable: true, get: function () { return scope_2.varKinds; } });
exports.operators = {
    GT: new code$2._Code(">"),
    GTE: new code$2._Code(">="),
    LT: new code$2._Code("<"),
    LTE: new code$2._Code("<="),
    EQ: new code$2._Code("==="),
    NEQ: new code$2._Code("!=="),
    NOT: new code$2._Code("!"),
    OR: new code$2._Code("||"),
    AND: new code$2._Code("&&"),
    ADD: new code$2._Code("+"),
};
class Node {
    optimizeNodes() {
        return this;
    }
    optimizeNames(_names, _constants) {
        return this;
    }
}
class Def extends Node {
    constructor(varKind, name, rhs) {
        super();
        this.varKind = varKind;
        this.name = name;
        this.rhs = rhs;
    }
    render({ es5, _n }) {
        const varKind = es5 ? scope.varKinds.var : this.varKind;
        const rhs = this.rhs === undefined ? "" : ` = ${this.rhs}`;
        return `${varKind} ${this.name}${rhs};` + _n;
    }
    optimizeNames(names, constants) {
        if (!names[this.name.str])
            return;
        if (this.rhs)
            this.rhs = optimizeExpr(this.rhs, names, constants);
        return this;
    }
    get names() {
        return this.rhs instanceof code$2._CodeOrName ? this.rhs.names : {};
    }
}
class Assign extends Node {
    constructor(lhs, rhs, sideEffects) {
        super();
        this.lhs = lhs;
        this.rhs = rhs;
        this.sideEffects = sideEffects;
    }
    render({ _n }) {
        return `${this.lhs} = ${this.rhs};` + _n;
    }
    optimizeNames(names, constants) {
        if (this.lhs instanceof code$2.Name && !names[this.lhs.str] && !this.sideEffects)
            return;
        this.rhs = optimizeExpr(this.rhs, names, constants);
        return this;
    }
    get names() {
        const names = this.lhs instanceof code$2.Name ? {} : { ...this.lhs.names };
        return addExprNames(names, this.rhs);
    }
}
class AssignOp extends Assign {
    constructor(lhs, op, rhs, sideEffects) {
        super(lhs, rhs, sideEffects);
        this.op = op;
    }
    render({ _n }) {
        return `${this.lhs} ${this.op}= ${this.rhs};` + _n;
    }
}
class Label extends Node {
    constructor(label) {
        super();
        this.label = label;
        this.names = {};
    }
    render({ _n }) {
        return `${this.label}:` + _n;
    }
}
class Break extends Node {
    constructor(label) {
        super();
        this.label = label;
        this.names = {};
    }
    render({ _n }) {
        const label = this.label ? ` ${this.label}` : "";
        return `break${label};` + _n;
    }
}
class Throw extends Node {
    constructor(error) {
        super();
        this.error = error;
    }
    render({ _n }) {
        return `throw ${this.error};` + _n;
    }
    get names() {
        return this.error.names;
    }
}
class AnyCode extends Node {
    constructor(code) {
        super();
        this.code = code;
    }
    render({ _n }) {
        return `${this.code};` + _n;
    }
    optimizeNodes() {
        return `${this.code}` ? this : undefined;
    }
    optimizeNames(names, constants) {
        this.code = optimizeExpr(this.code, names, constants);
        return this;
    }
    get names() {
        return this.code instanceof code$2._CodeOrName ? this.code.names : {};
    }
}
class ParentNode extends Node {
    constructor(nodes = []) {
        super();
        this.nodes = nodes;
    }
    render(opts) {
        return this.nodes.reduce((code, n) => code + n.render(opts), "");
    }
    optimizeNodes() {
        const { nodes } = this;
        let i = nodes.length;
        while (i--) {
            const n = nodes[i].optimizeNodes();
            if (Array.isArray(n))
                nodes.splice(i, 1, ...n);
            else if (n)
                nodes[i] = n;
            else
                nodes.splice(i, 1);
        }
        return nodes.length > 0 ? this : undefined;
    }
    optimizeNames(names, constants) {
        const { nodes } = this;
        let i = nodes.length;
        while (i--) {
            // iterating backwards improves 1-pass optimization
            const n = nodes[i];
            if (n.optimizeNames(names, constants))
                continue;
            subtractNames(names, n.names);
            nodes.splice(i, 1);
        }
        return nodes.length > 0 ? this : undefined;
    }
    get names() {
        return this.nodes.reduce((names, n) => addNames(names, n.names), {});
    }
}
class BlockNode extends ParentNode {
    render(opts) {
        return "{" + opts._n + super.render(opts) + "}" + opts._n;
    }
}
class Root extends ParentNode {
}
class Else extends BlockNode {
}
Else.kind = "else";
class If extends BlockNode {
    constructor(condition, nodes) {
        super(nodes);
        this.condition = condition;
    }
    render(opts) {
        let code = `if(${this.condition})` + super.render(opts);
        if (this.else)
            code += "else " + this.else.render(opts);
        return code;
    }
    optimizeNodes() {
        super.optimizeNodes();
        const cond = this.condition;
        if (cond === true)
            return this.nodes; // else is ignored here
        let e = this.else;
        if (e) {
            const ns = e.optimizeNodes();
            e = this.else = Array.isArray(ns) ? new Else(ns) : ns;
        }
        if (e) {
            if (cond === false)
                return e instanceof If ? e : e.nodes;
            if (this.nodes.length)
                return this;
            return new If(not(cond), e instanceof If ? [e] : e.nodes);
        }
        if (cond === false || !this.nodes.length)
            return undefined;
        return this;
    }
    optimizeNames(names, constants) {
        var _a;
        this.else = (_a = this.else) === null || _a === void 0 ? void 0 : _a.optimizeNames(names, constants);
        if (!(super.optimizeNames(names, constants) || this.else))
            return;
        this.condition = optimizeExpr(this.condition, names, constants);
        return this;
    }
    get names() {
        const names = super.names;
        addExprNames(names, this.condition);
        if (this.else)
            addNames(names, this.else.names);
        return names;
    }
}
If.kind = "if";
class For extends BlockNode {
}
For.kind = "for";
class ForLoop extends For {
    constructor(iteration) {
        super();
        this.iteration = iteration;
    }
    render(opts) {
        return `for(${this.iteration})` + super.render(opts);
    }
    optimizeNames(names, constants) {
        if (!super.optimizeNames(names, constants))
            return;
        this.iteration = optimizeExpr(this.iteration, names, constants);
        return this;
    }
    get names() {
        return addNames(super.names, this.iteration.names);
    }
}
class ForRange extends For {
    constructor(varKind, name, from, to) {
        super();
        this.varKind = varKind;
        this.name = name;
        this.from = from;
        this.to = to;
    }
    render(opts) {
        const varKind = opts.es5 ? scope.varKinds.var : this.varKind;
        const { name, from, to } = this;
        return `for(${varKind} ${name}=${from}; ${name}<${to}; ${name}++)` + super.render(opts);
    }
    get names() {
        const names = addExprNames(super.names, this.from);
        return addExprNames(names, this.to);
    }
}
class ForIter extends For {
    constructor(loop, varKind, name, iterable) {
        super();
        this.loop = loop;
        this.varKind = varKind;
        this.name = name;
        this.iterable = iterable;
    }
    render(opts) {
        return `for(${this.varKind} ${this.name} ${this.loop} ${this.iterable})` + super.render(opts);
    }
    optimizeNames(names, constants) {
        if (!super.optimizeNames(names, constants))
            return;
        this.iterable = optimizeExpr(this.iterable, names, constants);
        return this;
    }
    get names() {
        return addNames(super.names, this.iterable.names);
    }
}
class Func extends BlockNode {
    constructor(name, args, async) {
        super();
        this.name = name;
        this.args = args;
        this.async = async;
    }
    render(opts) {
        const _async = this.async ? "async " : "";
        return `${_async}function ${this.name}(${this.args})` + super.render(opts);
    }
}
Func.kind = "func";
class Return extends ParentNode {
    render(opts) {
        return "return " + super.render(opts);
    }
}
Return.kind = "return";
class Try extends BlockNode {
    render(opts) {
        let code = "try" + super.render(opts);
        if (this.catch)
            code += this.catch.render(opts);
        if (this.finally)
            code += this.finally.render(opts);
        return code;
    }
    optimizeNodes() {
        var _a, _b;
        super.optimizeNodes();
        (_a = this.catch) === null || _a === void 0 ? void 0 : _a.optimizeNodes();
        (_b = this.finally) === null || _b === void 0 ? void 0 : _b.optimizeNodes();
        return this;
    }
    optimizeNames(names, constants) {
        var _a, _b;
        super.optimizeNames(names, constants);
        (_a = this.catch) === null || _a === void 0 ? void 0 : _a.optimizeNames(names, constants);
        (_b = this.finally) === null || _b === void 0 ? void 0 : _b.optimizeNames(names, constants);
        return this;
    }
    get names() {
        const names = super.names;
        if (this.catch)
            addNames(names, this.catch.names);
        if (this.finally)
            addNames(names, this.finally.names);
        return names;
    }
}
class Catch extends BlockNode {
    constructor(error) {
        super();
        this.error = error;
    }
    render(opts) {
        return `catch(${this.error})` + super.render(opts);
    }
}
Catch.kind = "catch";
class Finally extends BlockNode {
    render(opts) {
        return "finally" + super.render(opts);
    }
}
Finally.kind = "finally";
class CodeGen {
    constructor(extScope, opts = {}) {
        this._values = {};
        this._blockStarts = [];
        this._constants = {};
        this.opts = { ...opts, _n: opts.lines ? "\n" : "" };
        this._extScope = extScope;
        this._scope = new scope.Scope({ parent: extScope });
        this._nodes = [new Root()];
    }
    toString() {
        return this._root.render(this.opts);
    }
    // returns unique name in the internal scope
    name(prefix) {
        return this._scope.name(prefix);
    }
    // reserves unique name in the external scope
    scopeName(prefix) {
        return this._extScope.name(prefix);
    }
    // reserves unique name in the external scope and assigns value to it
    scopeValue(prefixOrName, value) {
        const name = this._extScope.value(prefixOrName, value);
        const vs = this._values[name.prefix] || (this._values[name.prefix] = new Set());
        vs.add(name);
        return name;
    }
    getScopeValue(prefix, keyOrRef) {
        return this._extScope.getValue(prefix, keyOrRef);
    }
    // return code that assigns values in the external scope to the names that are used internally
    // (same names that were returned by gen.scopeName or gen.scopeValue)
    scopeRefs(scopeName) {
        return this._extScope.scopeRefs(scopeName, this._values);
    }
    scopeCode() {
        return this._extScope.scopeCode(this._values);
    }
    _def(varKind, nameOrPrefix, rhs, constant) {
        const name = this._scope.toName(nameOrPrefix);
        if (rhs !== undefined && constant)
            this._constants[name.str] = rhs;
        this._leafNode(new Def(varKind, name, rhs));
        return name;
    }
    // `const` declaration (`var` in es5 mode)
    const(nameOrPrefix, rhs, _constant) {
        return this._def(scope.varKinds.const, nameOrPrefix, rhs, _constant);
    }
    // `let` declaration with optional assignment (`var` in es5 mode)
    let(nameOrPrefix, rhs, _constant) {
        return this._def(scope.varKinds.let, nameOrPrefix, rhs, _constant);
    }
    // `var` declaration with optional assignment
    var(nameOrPrefix, rhs, _constant) {
        return this._def(scope.varKinds.var, nameOrPrefix, rhs, _constant);
    }
    // assignment code
    assign(lhs, rhs, sideEffects) {
        return this._leafNode(new Assign(lhs, rhs, sideEffects));
    }
    // `+=` code
    add(lhs, rhs) {
        return this._leafNode(new AssignOp(lhs, exports.operators.ADD, rhs));
    }
    // appends passed SafeExpr to code or executes Block
    code(c) {
        if (typeof c == "function")
            c();
        else if (c !== code$2.nil)
            this._leafNode(new AnyCode(c));
        return this;
    }
    // returns code for object literal for the passed argument list of key-value pairs
    object(...keyValues) {
        const code = ["{"];
        for (const [key, value] of keyValues) {
            if (code.length > 1)
                code.push(",");
            code.push(key);
            if (key !== value || this.opts.es5) {
                code.push(":");
                (0, code$2.addCodeArg)(code, value);
            }
        }
        code.push("}");
        return new code$2._Code(code);
    }
    // `if` clause (or statement if `thenBody` and, optionally, `elseBody` are passed)
    if(condition, thenBody, elseBody) {
        this._blockNode(new If(condition));
        if (thenBody && elseBody) {
            this.code(thenBody).else().code(elseBody).endIf();
        }
        else if (thenBody) {
            this.code(thenBody).endIf();
        }
        else if (elseBody) {
            throw new Error('CodeGen: "else" body without "then" body');
        }
        return this;
    }
    // `else if` clause - invalid without `if` or after `else` clauses
    elseIf(condition) {
        return this._elseNode(new If(condition));
    }
    // `else` clause - only valid after `if` or `else if` clauses
    else() {
        return this._elseNode(new Else());
    }
    // end `if` statement (needed if gen.if was used only with condition)
    endIf() {
        return this._endBlockNode(If, Else);
    }
    _for(node, forBody) {
        this._blockNode(node);
        if (forBody)
            this.code(forBody).endFor();
        return this;
    }
    // a generic `for` clause (or statement if `forBody` is passed)
    for(iteration, forBody) {
        return this._for(new ForLoop(iteration), forBody);
    }
    // `for` statement for a range of values
    forRange(nameOrPrefix, from, to, forBody, varKind = this.opts.es5 ? scope.varKinds.var : scope.varKinds.let) {
        const name = this._scope.toName(nameOrPrefix);
        return this._for(new ForRange(varKind, name, from, to), () => forBody(name));
    }
    // `for-of` statement (in es5 mode replace with a normal for loop)
    forOf(nameOrPrefix, iterable, forBody, varKind = scope.varKinds.const) {
        const name = this._scope.toName(nameOrPrefix);
        if (this.opts.es5) {
            const arr = iterable instanceof code$2.Name ? iterable : this.var("_arr", iterable);
            return this.forRange("_i", 0, (0, code$2._) `${arr}.length`, (i) => {
                this.var(name, (0, code$2._) `${arr}[${i}]`);
                forBody(name);
            });
        }
        return this._for(new ForIter("of", varKind, name, iterable), () => forBody(name));
    }
    // `for-in` statement.
    // With option `ownProperties` replaced with a `for-of` loop for object keys
    forIn(nameOrPrefix, obj, forBody, varKind = this.opts.es5 ? scope.varKinds.var : scope.varKinds.const) {
        if (this.opts.ownProperties) {
            return this.forOf(nameOrPrefix, (0, code$2._) `Object.keys(${obj})`, forBody);
        }
        const name = this._scope.toName(nameOrPrefix);
        return this._for(new ForIter("in", varKind, name, obj), () => forBody(name));
    }
    // end `for` loop
    endFor() {
        return this._endBlockNode(For);
    }
    // `label` statement
    label(label) {
        return this._leafNode(new Label(label));
    }
    // `break` statement
    break(label) {
        return this._leafNode(new Break(label));
    }
    // `return` statement
    return(value) {
        const node = new Return();
        this._blockNode(node);
        this.code(value);
        if (node.nodes.length !== 1)
            throw new Error('CodeGen: "return" should have one node');
        return this._endBlockNode(Return);
    }
    // `try` statement
    try(tryBody, catchCode, finallyCode) {
        if (!catchCode && !finallyCode)
            throw new Error('CodeGen: "try" without "catch" and "finally"');
        const node = new Try();
        this._blockNode(node);
        this.code(tryBody);
        if (catchCode) {
            const error = this.name("e");
            this._currNode = node.catch = new Catch(error);
            catchCode(error);
        }
        if (finallyCode) {
            this._currNode = node.finally = new Finally();
            this.code(finallyCode);
        }
        return this._endBlockNode(Catch, Finally);
    }
    // `throw` statement
    throw(error) {
        return this._leafNode(new Throw(error));
    }
    // start self-balancing block
    block(body, nodeCount) {
        this._blockStarts.push(this._nodes.length);
        if (body)
            this.code(body).endBlock(nodeCount);
        return this;
    }
    // end the current self-balancing block
    endBlock(nodeCount) {
        const len = this._blockStarts.pop();
        if (len === undefined)
            throw new Error("CodeGen: not in self-balancing block");
        const toClose = this._nodes.length - len;
        if (toClose < 0 || (nodeCount !== undefined && toClose !== nodeCount)) {
            throw new Error(`CodeGen: wrong number of nodes: ${toClose} vs ${nodeCount} expected`);
        }
        this._nodes.length = len;
        return this;
    }
    // `function` heading (or definition if funcBody is passed)
    func(name, args = code$2.nil, async, funcBody) {
        this._blockNode(new Func(name, args, async));
        if (funcBody)
            this.code(funcBody).endFunc();
        return this;
    }
    // end function definition
    endFunc() {
        return this._endBlockNode(Func);
    }
    optimize(n = 1) {
        while (n-- > 0) {
            this._root.optimizeNodes();
            this._root.optimizeNames(this._root.names, this._constants);
        }
    }
    _leafNode(node) {
        this._currNode.nodes.push(node);
        return this;
    }
    _blockNode(node) {
        this._currNode.nodes.push(node);
        this._nodes.push(node);
    }
    _endBlockNode(N1, N2) {
        const n = this._currNode;
        if (n instanceof N1 || (N2 && n instanceof N2)) {
            this._nodes.pop();
            return this;
        }
        throw new Error(`CodeGen: not in block "${N2 ? `${N1.kind}/${N2.kind}` : N1.kind}"`);
    }
    _elseNode(node) {
        const n = this._currNode;
        if (!(n instanceof If)) {
            throw new Error('CodeGen: "else" without "if"');
        }
        this._currNode = n.else = node;
        return this;
    }
    get _root() {
        return this._nodes[0];
    }
    get _currNode() {
        const ns = this._nodes;
        return ns[ns.length - 1];
    }
    set _currNode(node) {
        const ns = this._nodes;
        ns[ns.length - 1] = node;
    }
}
exports.CodeGen = CodeGen;
function addNames(names, from) {
    for (const n in from)
        names[n] = (names[n] || 0) + (from[n] || 0);
    return names;
}
function addExprNames(names, from) {
    return from instanceof code$2._CodeOrName ? addNames(names, from.names) : names;
}
function optimizeExpr(expr, names, constants) {
    if (expr instanceof code$2.Name)
        return replaceName(expr);
    if (!canOptimize(expr))
        return expr;
    return new code$2._Code(expr._items.reduce((items, c) => {
        if (c instanceof code$2.Name)
            c = replaceName(c);
        if (c instanceof code$2._Code)
            items.push(...c._items);
        else
            items.push(c);
        return items;
    }, []));
    function replaceName(n) {
        const c = constants[n.str];
        if (c === undefined || names[n.str] !== 1)
            return n;
        delete names[n.str];
        return c;
    }
    function canOptimize(e) {
        return (e instanceof code$2._Code &&
            e._items.some((c) => c instanceof code$2.Name && names[c.str] === 1 && constants[c.str] !== undefined));
    }
}
function subtractNames(names, from) {
    for (const n in from)
        names[n] = (names[n] || 0) - (from[n] || 0);
}
function not(x) {
    return typeof x == "boolean" || typeof x == "number" || x === null ? !x : (0, code$2._) `!${par(x)}`;
}
exports.not = not;
const andCode = mappend(exports.operators.AND);
// boolean AND (&&) expression with the passed arguments
function and(...args) {
    return args.reduce(andCode);
}
exports.and = and;
const orCode = mappend(exports.operators.OR);
// boolean OR (||) expression with the passed arguments
function or(...args) {
    return args.reduce(orCode);
}
exports.or = or;
function mappend(op) {
    return (x, y) => (x === code$2.nil ? y : y === code$2.nil ? x : (0, code$2._) `${par(x)} ${op} ${par(y)}`);
}
function par(x) {
    return x instanceof code$2.Name ? x : (0, code$2._) `(${x})`;
}
//# sourceMappingURL=index.js.map
});

const index$8 = /*@__PURE__*/getDefaultExportFromCjs(codegen);

var util = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.checkStrictMode = exports.getErrorPath = exports.Type = exports.useFunc = exports.setEvaluated = exports.evaluatedPropsToName = exports.mergeEvaluated = exports.eachItem = exports.unescapeJsonPointer = exports.escapeJsonPointer = exports.escapeFragment = exports.unescapeFragment = exports.schemaRefOrVal = exports.schemaHasRulesButRef = exports.schemaHasRules = exports.checkUnknownRules = exports.alwaysValidSchema = exports.toHash = void 0;


// TODO refactor to use Set
function toHash(arr) {
    const hash = {};
    for (const item of arr)
        hash[item] = true;
    return hash;
}
exports.toHash = toHash;
function alwaysValidSchema(it, schema) {
    if (typeof schema == "boolean")
        return schema;
    if (Object.keys(schema).length === 0)
        return true;
    checkUnknownRules(it, schema);
    return !schemaHasRules(schema, it.self.RULES.all);
}
exports.alwaysValidSchema = alwaysValidSchema;
function checkUnknownRules(it, schema = it.schema) {
    const { opts, self } = it;
    if (!opts.strictSchema)
        return;
    if (typeof schema === "boolean")
        return;
    const rules = self.RULES.keywords;
    for (const key in schema) {
        if (!rules[key])
            checkStrictMode(it, `unknown keyword: "${key}"`);
    }
}
exports.checkUnknownRules = checkUnknownRules;
function schemaHasRules(schema, rules) {
    if (typeof schema == "boolean")
        return !schema;
    for (const key in schema)
        if (rules[key])
            return true;
    return false;
}
exports.schemaHasRules = schemaHasRules;
function schemaHasRulesButRef(schema, RULES) {
    if (typeof schema == "boolean")
        return !schema;
    for (const key in schema)
        if (key !== "$ref" && RULES.all[key])
            return true;
    return false;
}
exports.schemaHasRulesButRef = schemaHasRulesButRef;
function schemaRefOrVal({ topSchemaRef, schemaPath }, schema, keyword, $data) {
    if (!$data) {
        if (typeof schema == "number" || typeof schema == "boolean")
            return schema;
        if (typeof schema == "string")
            return (0, codegen._) `${schema}`;
    }
    return (0, codegen._) `${topSchemaRef}${schemaPath}${(0, codegen.getProperty)(keyword)}`;
}
exports.schemaRefOrVal = schemaRefOrVal;
function unescapeFragment(str) {
    return unescapeJsonPointer(decodeURIComponent(str));
}
exports.unescapeFragment = unescapeFragment;
function escapeFragment(str) {
    return encodeURIComponent(escapeJsonPointer(str));
}
exports.escapeFragment = escapeFragment;
function escapeJsonPointer(str) {
    if (typeof str == "number")
        return `${str}`;
    return str.replace(/~/g, "~0").replace(/\//g, "~1");
}
exports.escapeJsonPointer = escapeJsonPointer;
function unescapeJsonPointer(str) {
    return str.replace(/~1/g, "/").replace(/~0/g, "~");
}
exports.unescapeJsonPointer = unescapeJsonPointer;
function eachItem(xs, f) {
    if (Array.isArray(xs)) {
        for (const x of xs)
            f(x);
    }
    else {
        f(xs);
    }
}
exports.eachItem = eachItem;
function makeMergeEvaluated({ mergeNames, mergeToName, mergeValues, resultToName, }) {
    return (gen, from, to, toName) => {
        const res = to === undefined
            ? from
            : to instanceof codegen.Name
                ? (from instanceof codegen.Name ? mergeNames(gen, from, to) : mergeToName(gen, from, to), to)
                : from instanceof codegen.Name
                    ? (mergeToName(gen, to, from), from)
                    : mergeValues(from, to);
        return toName === codegen.Name && !(res instanceof codegen.Name) ? resultToName(gen, res) : res;
    };
}
exports.mergeEvaluated = {
    props: makeMergeEvaluated({
        mergeNames: (gen, from, to) => gen.if((0, codegen._) `${to} !== true && ${from} !== undefined`, () => {
            gen.if((0, codegen._) `${from} === true`, () => gen.assign(to, true), () => gen.assign(to, (0, codegen._) `${to} || {}`).code((0, codegen._) `Object.assign(${to}, ${from})`));
        }),
        mergeToName: (gen, from, to) => gen.if((0, codegen._) `${to} !== true`, () => {
            if (from === true) {
                gen.assign(to, true);
            }
            else {
                gen.assign(to, (0, codegen._) `${to} || {}`);
                setEvaluated(gen, to, from);
            }
        }),
        mergeValues: (from, to) => (from === true ? true : { ...from, ...to }),
        resultToName: evaluatedPropsToName,
    }),
    items: makeMergeEvaluated({
        mergeNames: (gen, from, to) => gen.if((0, codegen._) `${to} !== true && ${from} !== undefined`, () => gen.assign(to, (0, codegen._) `${from} === true ? true : ${to} > ${from} ? ${to} : ${from}`)),
        mergeToName: (gen, from, to) => gen.if((0, codegen._) `${to} !== true`, () => gen.assign(to, from === true ? true : (0, codegen._) `${to} > ${from} ? ${to} : ${from}`)),
        mergeValues: (from, to) => (from === true ? true : Math.max(from, to)),
        resultToName: (gen, items) => gen.var("items", items),
    }),
};
function evaluatedPropsToName(gen, ps) {
    if (ps === true)
        return gen.var("props", true);
    const props = gen.var("props", (0, codegen._) `{}`);
    if (ps !== undefined)
        setEvaluated(gen, props, ps);
    return props;
}
exports.evaluatedPropsToName = evaluatedPropsToName;
function setEvaluated(gen, props, ps) {
    Object.keys(ps).forEach((p) => gen.assign((0, codegen._) `${props}${(0, codegen.getProperty)(p)}`, true));
}
exports.setEvaluated = setEvaluated;
const snippets = {};
function useFunc(gen, f) {
    return gen.scopeValue("func", {
        ref: f,
        code: snippets[f.code] || (snippets[f.code] = new code$2._Code(f.code)),
    });
}
exports.useFunc = useFunc;
var Type;
(function (Type) {
    Type[Type["Num"] = 0] = "Num";
    Type[Type["Str"] = 1] = "Str";
})(Type || (exports.Type = Type = {}));
function getErrorPath(dataProp, dataPropType, jsPropertySyntax) {
    // let path
    if (dataProp instanceof codegen.Name) {
        const isNumber = dataPropType === Type.Num;
        return jsPropertySyntax
            ? isNumber
                ? (0, codegen._) `"[" + ${dataProp} + "]"`
                : (0, codegen._) `"['" + ${dataProp} + "']"`
            : isNumber
                ? (0, codegen._) `"/" + ${dataProp}`
                : (0, codegen._) `"/" + ${dataProp}.replace(/~/g, "~0").replace(/\\//g, "~1")`; // TODO maybe use global escapePointer
    }
    return jsPropertySyntax ? (0, codegen.getProperty)(dataProp).toString() : "/" + escapeJsonPointer(dataProp);
}
exports.getErrorPath = getErrorPath;
function checkStrictMode(it, msg, mode = it.opts.strictSchema) {
    if (!mode)
        return;
    msg = `strict mode: ${msg}`;
    if (mode === true)
        throw new Error(msg);
    it.self.logger.warn(msg);
}
exports.checkStrictMode = checkStrictMode;
//# sourceMappingURL=util.js.map
});

const util$1 = /*@__PURE__*/getDefaultExportFromCjs(util);

var names_1 = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });

const names = {
    // validation function arguments
    data: new codegen.Name("data"), // data passed to validation function
    // args passed from referencing schema
    valCxt: new codegen.Name("valCxt"), // validation/data context - should not be used directly, it is destructured to the names below
    instancePath: new codegen.Name("instancePath"),
    parentData: new codegen.Name("parentData"),
    parentDataProperty: new codegen.Name("parentDataProperty"),
    rootData: new codegen.Name("rootData"), // root data - same as the data passed to the first/top validation function
    dynamicAnchors: new codegen.Name("dynamicAnchors"), // used to support recursiveRef and dynamicRef
    // function scoped variables
    vErrors: new codegen.Name("vErrors"), // null or array of validation errors
    errors: new codegen.Name("errors"), // counter of validation errors
    this: new codegen.Name("this"),
    // "globals"
    self: new codegen.Name("self"),
    scope: new codegen.Name("scope"),
    // JTD serialize/parse name for JSON string and position
    json: new codegen.Name("json"),
    jsonPos: new codegen.Name("jsonPos"),
    jsonLen: new codegen.Name("jsonLen"),
    jsonPart: new codegen.Name("jsonPart"),
};
exports.default = names;
//# sourceMappingURL=names.js.map
});

const names = /*@__PURE__*/getDefaultExportFromCjs(names_1);

var errors = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.extendErrors = exports.resetErrorsCount = exports.reportExtraError = exports.reportError = exports.keyword$DataError = exports.keywordError = void 0;



exports.keywordError = {
    message: ({ keyword }) => (0, codegen.str) `must pass "${keyword}" keyword validation`,
};
exports.keyword$DataError = {
    message: ({ keyword, schemaType }) => schemaType
        ? (0, codegen.str) `"${keyword}" keyword must be ${schemaType} ($data)`
        : (0, codegen.str) `"${keyword}" keyword is invalid ($data)`,
};
function reportError(cxt, error = exports.keywordError, errorPaths, overrideAllErrors) {
    const { it } = cxt;
    const { gen, compositeRule, allErrors } = it;
    const errObj = errorObjectCode(cxt, error, errorPaths);
    if (overrideAllErrors !== null && overrideAllErrors !== void 0 ? overrideAllErrors : (compositeRule || allErrors)) {
        addError(gen, errObj);
    }
    else {
        returnErrors(it, (0, codegen._) `[${errObj}]`);
    }
}
exports.reportError = reportError;
function reportExtraError(cxt, error = exports.keywordError, errorPaths) {
    const { it } = cxt;
    const { gen, compositeRule, allErrors } = it;
    const errObj = errorObjectCode(cxt, error, errorPaths);
    addError(gen, errObj);
    if (!(compositeRule || allErrors)) {
        returnErrors(it, names_1.default.vErrors);
    }
}
exports.reportExtraError = reportExtraError;
function resetErrorsCount(gen, errsCount) {
    gen.assign(names_1.default.errors, errsCount);
    gen.if((0, codegen._) `${names_1.default.vErrors} !== null`, () => gen.if(errsCount, () => gen.assign((0, codegen._) `${names_1.default.vErrors}.length`, errsCount), () => gen.assign(names_1.default.vErrors, null)));
}
exports.resetErrorsCount = resetErrorsCount;
function extendErrors({ gen, keyword, schemaValue, data, errsCount, it, }) {
    /* istanbul ignore if */
    if (errsCount === undefined)
        throw new Error("ajv implementation error");
    const err = gen.name("err");
    gen.forRange("i", errsCount, names_1.default.errors, (i) => {
        gen.const(err, (0, codegen._) `${names_1.default.vErrors}[${i}]`);
        gen.if((0, codegen._) `${err}.instancePath === undefined`, () => gen.assign((0, codegen._) `${err}.instancePath`, (0, codegen.strConcat)(names_1.default.instancePath, it.errorPath)));
        gen.assign((0, codegen._) `${err}.schemaPath`, (0, codegen.str) `${it.errSchemaPath}/${keyword}`);
        if (it.opts.verbose) {
            gen.assign((0, codegen._) `${err}.schema`, schemaValue);
            gen.assign((0, codegen._) `${err}.data`, data);
        }
    });
}
exports.extendErrors = extendErrors;
function addError(gen, errObj) {
    const err = gen.const("err", errObj);
    gen.if((0, codegen._) `${names_1.default.vErrors} === null`, () => gen.assign(names_1.default.vErrors, (0, codegen._) `[${err}]`), (0, codegen._) `${names_1.default.vErrors}.push(${err})`);
    gen.code((0, codegen._) `${names_1.default.errors}++`);
}
function returnErrors(it, errs) {
    const { gen, validateName, schemaEnv } = it;
    if (schemaEnv.$async) {
        gen.throw((0, codegen._) `new ${it.ValidationError}(${errs})`);
    }
    else {
        gen.assign((0, codegen._) `${validateName}.errors`, errs);
        gen.return(false);
    }
}
const E = {
    keyword: new codegen.Name("keyword"),
    schemaPath: new codegen.Name("schemaPath"), // also used in JTD errors
    params: new codegen.Name("params"),
    propertyName: new codegen.Name("propertyName"),
    message: new codegen.Name("message"),
    schema: new codegen.Name("schema"),
    parentSchema: new codegen.Name("parentSchema"),
};
function errorObjectCode(cxt, error, errorPaths) {
    const { createErrors } = cxt.it;
    if (createErrors === false)
        return (0, codegen._) `{}`;
    return errorObject(cxt, error, errorPaths);
}
function errorObject(cxt, error, errorPaths = {}) {
    const { gen, it } = cxt;
    const keyValues = [
        errorInstancePath(it, errorPaths),
        errorSchemaPath(cxt, errorPaths),
    ];
    extraErrorProps(cxt, error, keyValues);
    return gen.object(...keyValues);
}
function errorInstancePath({ errorPath }, { instancePath }) {
    const instPath = instancePath
        ? (0, codegen.str) `${errorPath}${(0, util.getErrorPath)(instancePath, util.Type.Str)}`
        : errorPath;
    return [names_1.default.instancePath, (0, codegen.strConcat)(names_1.default.instancePath, instPath)];
}
function errorSchemaPath({ keyword, it: { errSchemaPath } }, { schemaPath, parentSchema }) {
    let schPath = parentSchema ? errSchemaPath : (0, codegen.str) `${errSchemaPath}/${keyword}`;
    if (schemaPath) {
        schPath = (0, codegen.str) `${schPath}${(0, util.getErrorPath)(schemaPath, util.Type.Str)}`;
    }
    return [E.schemaPath, schPath];
}
function extraErrorProps(cxt, { params, message }, keyValues) {
    const { keyword, data, schemaValue, it } = cxt;
    const { opts, propertyName, topSchemaRef, schemaPath } = it;
    keyValues.push([E.keyword, keyword], [E.params, typeof params == "function" ? params(cxt) : params || (0, codegen._) `{}`]);
    if (opts.messages) {
        keyValues.push([E.message, typeof message == "function" ? message(cxt) : message]);
    }
    if (opts.verbose) {
        keyValues.push([E.schema, schemaValue], [E.parentSchema, (0, codegen._) `${topSchemaRef}${schemaPath}`], [names_1.default.data, data]);
    }
    if (propertyName)
        keyValues.push([E.propertyName, propertyName]);
}
//# sourceMappingURL=errors.js.map
});

const errors$1 = /*@__PURE__*/getDefaultExportFromCjs(errors);

var boolSchema = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.boolOrEmptySchema = exports.topBoolOrEmptySchema = void 0;



const boolError = {
    message: "boolean schema is false",
};
function topBoolOrEmptySchema(it) {
    const { gen, schema, validateName } = it;
    if (schema === false) {
        falseSchemaError(it, false);
    }
    else if (typeof schema == "object" && schema.$async === true) {
        gen.return(names_1.default.data);
    }
    else {
        gen.assign((0, codegen._) `${validateName}.errors`, null);
        gen.return(true);
    }
}
exports.topBoolOrEmptySchema = topBoolOrEmptySchema;
function boolOrEmptySchema(it, valid) {
    const { gen, schema } = it;
    if (schema === false) {
        gen.var(valid, false); // TODO var
        falseSchemaError(it);
    }
    else {
        gen.var(valid, true); // TODO var
    }
}
exports.boolOrEmptySchema = boolOrEmptySchema;
function falseSchemaError(it, overrideAllErrors) {
    const { gen, data } = it;
    // TODO maybe some other interface should be used for non-keyword validation errors...
    const cxt = {
        gen,
        keyword: "false schema",
        data,
        schema: false,
        schemaCode: false,
        schemaValue: false,
        params: {},
        it,
    };
    (0, errors.reportError)(cxt, boolError, undefined, overrideAllErrors);
}
//# sourceMappingURL=boolSchema.js.map
});

const boolSchema$1 = /*@__PURE__*/getDefaultExportFromCjs(boolSchema);

var rules = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.getRules = exports.isJSONType = void 0;
const _jsonTypes = ["string", "number", "integer", "boolean", "null", "object", "array"];
const jsonTypes = new Set(_jsonTypes);
function isJSONType(x) {
    return typeof x == "string" && jsonTypes.has(x);
}
exports.isJSONType = isJSONType;
function getRules() {
    const groups = {
        number: { type: "number", rules: [] },
        string: { type: "string", rules: [] },
        array: { type: "array", rules: [] },
        object: { type: "object", rules: [] },
    };
    return {
        types: { ...groups, integer: true, boolean: true, null: true },
        rules: [{ rules: [] }, groups.number, groups.string, groups.array, groups.object],
        post: { rules: [] },
        all: {},
        keywords: {},
    };
}
exports.getRules = getRules;
//# sourceMappingURL=rules.js.map
});

const rules$1 = /*@__PURE__*/getDefaultExportFromCjs(rules);

var applicability = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.shouldUseRule = exports.shouldUseGroup = exports.schemaHasRulesForType = void 0;
function schemaHasRulesForType({ schema, self }, type) {
    const group = self.RULES.types[type];
    return group && group !== true && shouldUseGroup(schema, group);
}
exports.schemaHasRulesForType = schemaHasRulesForType;
function shouldUseGroup(schema, group) {
    return group.rules.some((rule) => shouldUseRule(schema, rule));
}
exports.shouldUseGroup = shouldUseGroup;
function shouldUseRule(schema, rule) {
    var _a;
    return (schema[rule.keyword] !== undefined ||
        ((_a = rule.definition.implements) === null || _a === void 0 ? void 0 : _a.some((kwd) => schema[kwd] !== undefined)));
}
exports.shouldUseRule = shouldUseRule;
//# sourceMappingURL=applicability.js.map
});

const applicability$1 = /*@__PURE__*/getDefaultExportFromCjs(applicability);

var dataType = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.reportTypeError = exports.checkDataTypes = exports.checkDataType = exports.coerceAndCheckDataType = exports.getJSONTypes = exports.getSchemaTypes = exports.DataType = void 0;





var DataType;
(function (DataType) {
    DataType[DataType["Correct"] = 0] = "Correct";
    DataType[DataType["Wrong"] = 1] = "Wrong";
})(DataType || (exports.DataType = DataType = {}));
function getSchemaTypes(schema) {
    const types = getJSONTypes(schema.type);
    const hasNull = types.includes("null");
    if (hasNull) {
        if (schema.nullable === false)
            throw new Error("type: null contradicts nullable: false");
    }
    else {
        if (!types.length && schema.nullable !== undefined) {
            throw new Error('"nullable" cannot be used without "type"');
        }
        if (schema.nullable === true)
            types.push("null");
    }
    return types;
}
exports.getSchemaTypes = getSchemaTypes;
// eslint-disable-next-line @typescript-eslint/no-redundant-type-constituents
function getJSONTypes(ts) {
    const types = Array.isArray(ts) ? ts : ts ? [ts] : [];
    if (types.every(rules.isJSONType))
        return types;
    throw new Error("type must be JSONType or JSONType[]: " + types.join(","));
}
exports.getJSONTypes = getJSONTypes;
function coerceAndCheckDataType(it, types) {
    const { gen, data, opts } = it;
    const coerceTo = coerceToTypes(types, opts.coerceTypes);
    const checkTypes = types.length > 0 &&
        !(coerceTo.length === 0 && types.length === 1 && (0, applicability.schemaHasRulesForType)(it, types[0]));
    if (checkTypes) {
        const wrongType = checkDataTypes(types, data, opts.strictNumbers, DataType.Wrong);
        gen.if(wrongType, () => {
            if (coerceTo.length)
                coerceData(it, types, coerceTo);
            else
                reportTypeError(it);
        });
    }
    return checkTypes;
}
exports.coerceAndCheckDataType = coerceAndCheckDataType;
const COERCIBLE = new Set(["string", "number", "integer", "boolean", "null"]);
function coerceToTypes(types, coerceTypes) {
    return coerceTypes
        ? types.filter((t) => COERCIBLE.has(t) || (coerceTypes === "array" && t === "array"))
        : [];
}
function coerceData(it, types, coerceTo) {
    const { gen, data, opts } = it;
    const dataType = gen.let("dataType", (0, codegen._) `typeof ${data}`);
    const coerced = gen.let("coerced", (0, codegen._) `undefined`);
    if (opts.coerceTypes === "array") {
        gen.if((0, codegen._) `${dataType} == 'object' && Array.isArray(${data}) && ${data}.length == 1`, () => gen
            .assign(data, (0, codegen._) `${data}[0]`)
            .assign(dataType, (0, codegen._) `typeof ${data}`)
            .if(checkDataTypes(types, data, opts.strictNumbers), () => gen.assign(coerced, data)));
    }
    gen.if((0, codegen._) `${coerced} !== undefined`);
    for (const t of coerceTo) {
        if (COERCIBLE.has(t) || (t === "array" && opts.coerceTypes === "array")) {
            coerceSpecificType(t);
        }
    }
    gen.else();
    reportTypeError(it);
    gen.endIf();
    gen.if((0, codegen._) `${coerced} !== undefined`, () => {
        gen.assign(data, coerced);
        assignParentData(it, coerced);
    });
    function coerceSpecificType(t) {
        switch (t) {
            case "string":
                gen
                    .elseIf((0, codegen._) `${dataType} == "number" || ${dataType} == "boolean"`)
                    .assign(coerced, (0, codegen._) `"" + ${data}`)
                    .elseIf((0, codegen._) `${data} === null`)
                    .assign(coerced, (0, codegen._) `""`);
                return;
            case "number":
                gen
                    .elseIf((0, codegen._) `${dataType} == "boolean" || ${data} === null
              || (${dataType} == "string" && ${data} && ${data} == +${data})`)
                    .assign(coerced, (0, codegen._) `+${data}`);
                return;
            case "integer":
                gen
                    .elseIf((0, codegen._) `${dataType} === "boolean" || ${data} === null
              || (${dataType} === "string" && ${data} && ${data} == +${data} && !(${data} % 1))`)
                    .assign(coerced, (0, codegen._) `+${data}`);
                return;
            case "boolean":
                gen
                    .elseIf((0, codegen._) `${data} === "false" || ${data} === 0 || ${data} === null`)
                    .assign(coerced, false)
                    .elseIf((0, codegen._) `${data} === "true" || ${data} === 1`)
                    .assign(coerced, true);
                return;
            case "null":
                gen.elseIf((0, codegen._) `${data} === "" || ${data} === 0 || ${data} === false`);
                gen.assign(coerced, null);
                return;
            case "array":
                gen
                    .elseIf((0, codegen._) `${dataType} === "string" || ${dataType} === "number"
              || ${dataType} === "boolean" || ${data} === null`)
                    .assign(coerced, (0, codegen._) `[${data}]`);
        }
    }
}
function assignParentData({ gen, parentData, parentDataProperty }, expr) {
    // TODO use gen.property
    gen.if((0, codegen._) `${parentData} !== undefined`, () => gen.assign((0, codegen._) `${parentData}[${parentDataProperty}]`, expr));
}
function checkDataType(dataType, data, strictNums, correct = DataType.Correct) {
    const EQ = correct === DataType.Correct ? codegen.operators.EQ : codegen.operators.NEQ;
    let cond;
    switch (dataType) {
        case "null":
            return (0, codegen._) `${data} ${EQ} null`;
        case "array":
            cond = (0, codegen._) `Array.isArray(${data})`;
            break;
        case "object":
            cond = (0, codegen._) `${data} && typeof ${data} == "object" && !Array.isArray(${data})`;
            break;
        case "integer":
            cond = numCond((0, codegen._) `!(${data} % 1) && !isNaN(${data})`);
            break;
        case "number":
            cond = numCond();
            break;
        default:
            return (0, codegen._) `typeof ${data} ${EQ} ${dataType}`;
    }
    return correct === DataType.Correct ? cond : (0, codegen.not)(cond);
    function numCond(_cond = codegen.nil) {
        return (0, codegen.and)((0, codegen._) `typeof ${data} == "number"`, _cond, strictNums ? (0, codegen._) `isFinite(${data})` : codegen.nil);
    }
}
exports.checkDataType = checkDataType;
function checkDataTypes(dataTypes, data, strictNums, correct) {
    if (dataTypes.length === 1) {
        return checkDataType(dataTypes[0], data, strictNums, correct);
    }
    let cond;
    const types = (0, util.toHash)(dataTypes);
    if (types.array && types.object) {
        const notObj = (0, codegen._) `typeof ${data} != "object"`;
        cond = types.null ? notObj : (0, codegen._) `!${data} || ${notObj}`;
        delete types.null;
        delete types.array;
        delete types.object;
    }
    else {
        cond = codegen.nil;
    }
    if (types.number)
        delete types.integer;
    for (const t in types)
        cond = (0, codegen.and)(cond, checkDataType(t, data, strictNums, correct));
    return cond;
}
exports.checkDataTypes = checkDataTypes;
const typeError = {
    message: ({ schema }) => `must be ${schema}`,
    params: ({ schema, schemaValue }) => typeof schema == "string" ? (0, codegen._) `{type: ${schema}}` : (0, codegen._) `{type: ${schemaValue}}`,
};
function reportTypeError(it) {
    const cxt = getTypeErrorContext(it);
    (0, errors.reportError)(cxt, typeError);
}
exports.reportTypeError = reportTypeError;
function getTypeErrorContext(it) {
    const { gen, data, schema } = it;
    const schemaCode = (0, util.schemaRefOrVal)(it, schema, "type");
    return {
        gen,
        keyword: "type",
        data,
        schema: schema.type,
        schemaCode,
        schemaValue: schemaCode,
        parentSchema: schema,
        params: {},
        it,
    };
}
//# sourceMappingURL=dataType.js.map
});

const dataType$1 = /*@__PURE__*/getDefaultExportFromCjs(dataType);

var defaults = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.assignDefaults = void 0;


function assignDefaults(it, ty) {
    const { properties, items } = it.schema;
    if (ty === "object" && properties) {
        for (const key in properties) {
            assignDefault(it, key, properties[key].default);
        }
    }
    else if (ty === "array" && Array.isArray(items)) {
        items.forEach((sch, i) => assignDefault(it, i, sch.default));
    }
}
exports.assignDefaults = assignDefaults;
function assignDefault(it, prop, defaultValue) {
    const { gen, compositeRule, data, opts } = it;
    if (defaultValue === undefined)
        return;
    const childData = (0, codegen._) `${data}${(0, codegen.getProperty)(prop)}`;
    if (compositeRule) {
        (0, util.checkStrictMode)(it, `default is ignored for: ${childData}`);
        return;
    }
    let condition = (0, codegen._) `${childData} === undefined`;
    if (opts.useDefaults === "empty") {
        condition = (0, codegen._) `${condition} || ${childData} === null || ${childData} === ""`;
    }
    // `${childData} === undefined` +
    // (opts.useDefaults === "empty" ? ` || ${childData} === null || ${childData} === ""` : "")
    gen.if(condition, (0, codegen._) `${childData} = ${(0, codegen.stringify)(defaultValue)}`);
}
//# sourceMappingURL=defaults.js.map
});

const defaults$1 = /*@__PURE__*/getDefaultExportFromCjs(defaults);

var code = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.validateUnion = exports.validateArray = exports.usePattern = exports.callValidateCode = exports.schemaProperties = exports.allSchemaProperties = exports.noPropertyInData = exports.propertyInData = exports.isOwnProperty = exports.hasPropFunc = exports.reportMissingProp = exports.checkMissingProp = exports.checkReportMissingProp = void 0;



const util_2 = util;
function checkReportMissingProp(cxt, prop) {
    const { gen, data, it } = cxt;
    gen.if(noPropertyInData(gen, data, prop, it.opts.ownProperties), () => {
        cxt.setParams({ missingProperty: (0, codegen._) `${prop}` }, true);
        cxt.error();
    });
}
exports.checkReportMissingProp = checkReportMissingProp;
function checkMissingProp({ gen, data, it: { opts } }, properties, missing) {
    return (0, codegen.or)(...properties.map((prop) => (0, codegen.and)(noPropertyInData(gen, data, prop, opts.ownProperties), (0, codegen._) `${missing} = ${prop}`)));
}
exports.checkMissingProp = checkMissingProp;
function reportMissingProp(cxt, missing) {
    cxt.setParams({ missingProperty: missing }, true);
    cxt.error();
}
exports.reportMissingProp = reportMissingProp;
function hasPropFunc(gen) {
    return gen.scopeValue("func", {
        // eslint-disable-next-line @typescript-eslint/unbound-method
        ref: Object.prototype.hasOwnProperty,
        code: (0, codegen._) `Object.prototype.hasOwnProperty`,
    });
}
exports.hasPropFunc = hasPropFunc;
function isOwnProperty(gen, data, property) {
    return (0, codegen._) `${hasPropFunc(gen)}.call(${data}, ${property})`;
}
exports.isOwnProperty = isOwnProperty;
function propertyInData(gen, data, property, ownProperties) {
    const cond = (0, codegen._) `${data}${(0, codegen.getProperty)(property)} !== undefined`;
    return ownProperties ? (0, codegen._) `${cond} && ${isOwnProperty(gen, data, property)}` : cond;
}
exports.propertyInData = propertyInData;
function noPropertyInData(gen, data, property, ownProperties) {
    const cond = (0, codegen._) `${data}${(0, codegen.getProperty)(property)} === undefined`;
    return ownProperties ? (0, codegen.or)(cond, (0, codegen.not)(isOwnProperty(gen, data, property))) : cond;
}
exports.noPropertyInData = noPropertyInData;
function allSchemaProperties(schemaMap) {
    return schemaMap ? Object.keys(schemaMap).filter((p) => p !== "__proto__") : [];
}
exports.allSchemaProperties = allSchemaProperties;
function schemaProperties(it, schemaMap) {
    return allSchemaProperties(schemaMap).filter((p) => !(0, util.alwaysValidSchema)(it, schemaMap[p]));
}
exports.schemaProperties = schemaProperties;
function callValidateCode({ schemaCode, data, it: { gen, topSchemaRef, schemaPath, errorPath }, it }, func, context, passSchema) {
    const dataAndSchema = passSchema ? (0, codegen._) `${schemaCode}, ${data}, ${topSchemaRef}${schemaPath}` : data;
    const valCxt = [
        [names_1.default.instancePath, (0, codegen.strConcat)(names_1.default.instancePath, errorPath)],
        [names_1.default.parentData, it.parentData],
        [names_1.default.parentDataProperty, it.parentDataProperty],
        [names_1.default.rootData, names_1.default.rootData],
    ];
    if (it.opts.dynamicRef)
        valCxt.push([names_1.default.dynamicAnchors, names_1.default.dynamicAnchors]);
    const args = (0, codegen._) `${dataAndSchema}, ${gen.object(...valCxt)}`;
    return context !== codegen.nil ? (0, codegen._) `${func}.call(${context}, ${args})` : (0, codegen._) `${func}(${args})`;
}
exports.callValidateCode = callValidateCode;
const newRegExp = (0, codegen._) `new RegExp`;
function usePattern({ gen, it: { opts } }, pattern) {
    const u = opts.unicodeRegExp ? "u" : "";
    const { regExp } = opts.code;
    const rx = regExp(pattern, u);
    return gen.scopeValue("pattern", {
        key: rx.toString(),
        ref: rx,
        code: (0, codegen._) `${regExp.code === "new RegExp" ? newRegExp : (0, util_2.useFunc)(gen, regExp)}(${pattern}, ${u})`,
    });
}
exports.usePattern = usePattern;
function validateArray(cxt) {
    const { gen, data, keyword, it } = cxt;
    const valid = gen.name("valid");
    if (it.allErrors) {
        const validArr = gen.let("valid", true);
        validateItems(() => gen.assign(validArr, false));
        return validArr;
    }
    gen.var(valid, true);
    validateItems(() => gen.break());
    return valid;
    function validateItems(notValid) {
        const len = gen.const("len", (0, codegen._) `${data}.length`);
        gen.forRange("i", 0, len, (i) => {
            cxt.subschema({
                keyword,
                dataProp: i,
                dataPropType: util.Type.Num,
            }, valid);
            gen.if((0, codegen.not)(valid), notValid);
        });
    }
}
exports.validateArray = validateArray;
function validateUnion(cxt) {
    const { gen, schema, keyword, it } = cxt;
    /* istanbul ignore if */
    if (!Array.isArray(schema))
        throw new Error("ajv implementation error");
    const alwaysValid = schema.some((sch) => (0, util.alwaysValidSchema)(it, sch));
    if (alwaysValid && !it.opts.unevaluated)
        return;
    const valid = gen.let("valid", false);
    const schValid = gen.name("_valid");
    gen.block(() => schema.forEach((_sch, i) => {
        const schCxt = cxt.subschema({
            keyword,
            schemaProp: i,
            compositeRule: true,
        }, schValid);
        gen.assign(valid, (0, codegen._) `${valid} || ${schValid}`);
        const merged = cxt.mergeValidEvaluated(schCxt, schValid);
        // can short-circuit if `unevaluatedProperties/Items` not supported (opts.unevaluated !== true)
        // or if all properties and items were evaluated (it.props === true && it.items === true)
        if (!merged)
            gen.if((0, codegen.not)(valid));
    }));
    cxt.result(valid, () => cxt.reset(), () => cxt.error(true));
}
exports.validateUnion = validateUnion;
//# sourceMappingURL=code.js.map
});

const code$1 = /*@__PURE__*/getDefaultExportFromCjs(code);

var keyword = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.validateKeywordUsage = exports.validSchemaType = exports.funcKeywordCode = exports.macroKeywordCode = void 0;




function macroKeywordCode(cxt, def) {
    const { gen, keyword, schema, parentSchema, it } = cxt;
    const macroSchema = def.macro.call(it.self, schema, parentSchema, it);
    const schemaRef = useKeyword(gen, keyword, macroSchema);
    if (it.opts.validateSchema !== false)
        it.self.validateSchema(macroSchema, true);
    const valid = gen.name("valid");
    cxt.subschema({
        schema: macroSchema,
        schemaPath: codegen.nil,
        errSchemaPath: `${it.errSchemaPath}/${keyword}`,
        topSchemaRef: schemaRef,
        compositeRule: true,
    }, valid);
    cxt.pass(valid, () => cxt.error(true));
}
exports.macroKeywordCode = macroKeywordCode;
function funcKeywordCode(cxt, def) {
    var _a;
    const { gen, keyword, schema, parentSchema, $data, it } = cxt;
    checkAsyncKeyword(it, def);
    const validate = !$data && def.compile ? def.compile.call(it.self, schema, parentSchema, it) : def.validate;
    const validateRef = useKeyword(gen, keyword, validate);
    const valid = gen.let("valid");
    cxt.block$data(valid, validateKeyword);
    cxt.ok((_a = def.valid) !== null && _a !== void 0 ? _a : valid);
    function validateKeyword() {
        if (def.errors === false) {
            assignValid();
            if (def.modifying)
                modifyData(cxt);
            reportErrs(() => cxt.error());
        }
        else {
            const ruleErrs = def.async ? validateAsync() : validateSync();
            if (def.modifying)
                modifyData(cxt);
            reportErrs(() => addErrs(cxt, ruleErrs));
        }
    }
    function validateAsync() {
        const ruleErrs = gen.let("ruleErrs", null);
        gen.try(() => assignValid((0, codegen._) `await `), (e) => gen.assign(valid, false).if((0, codegen._) `${e} instanceof ${it.ValidationError}`, () => gen.assign(ruleErrs, (0, codegen._) `${e}.errors`), () => gen.throw(e)));
        return ruleErrs;
    }
    function validateSync() {
        const validateErrs = (0, codegen._) `${validateRef}.errors`;
        gen.assign(validateErrs, null);
        assignValid(codegen.nil);
        return validateErrs;
    }
    function assignValid(_await = def.async ? (0, codegen._) `await ` : codegen.nil) {
        const passCxt = it.opts.passContext ? names_1.default.this : names_1.default.self;
        const passSchema = !(("compile" in def && !$data) || def.schema === false);
        gen.assign(valid, (0, codegen._) `${_await}${(0, code.callValidateCode)(cxt, validateRef, passCxt, passSchema)}`, def.modifying);
    }
    function reportErrs(errors) {
        var _a;
        gen.if((0, codegen.not)((_a = def.valid) !== null && _a !== void 0 ? _a : valid), errors);
    }
}
exports.funcKeywordCode = funcKeywordCode;
function modifyData(cxt) {
    const { gen, data, it } = cxt;
    gen.if(it.parentData, () => gen.assign(data, (0, codegen._) `${it.parentData}[${it.parentDataProperty}]`));
}
function addErrs(cxt, errs) {
    const { gen } = cxt;
    gen.if((0, codegen._) `Array.isArray(${errs})`, () => {
        gen
            .assign(names_1.default.vErrors, (0, codegen._) `${names_1.default.vErrors} === null ? ${errs} : ${names_1.default.vErrors}.concat(${errs})`)
            .assign(names_1.default.errors, (0, codegen._) `${names_1.default.vErrors}.length`);
        (0, errors.extendErrors)(cxt);
    }, () => cxt.error());
}
function checkAsyncKeyword({ schemaEnv }, def) {
    if (def.async && !schemaEnv.$async)
        throw new Error("async keyword in sync schema");
}
function useKeyword(gen, keyword, result) {
    if (result === undefined)
        throw new Error(`keyword "${keyword}" failed to compile`);
    return gen.scopeValue("keyword", typeof result == "function" ? { ref: result } : { ref: result, code: (0, codegen.stringify)(result) });
}
function validSchemaType(schema, schemaType, allowUndefined = false) {
    // TODO add tests
    return (!schemaType.length ||
        schemaType.some((st) => st === "array"
            ? Array.isArray(schema)
            : st === "object"
                ? schema && typeof schema == "object" && !Array.isArray(schema)
                : typeof schema == st || (allowUndefined && typeof schema == "undefined")));
}
exports.validSchemaType = validSchemaType;
function validateKeywordUsage({ schema, opts, self, errSchemaPath }, def, keyword) {
    /* istanbul ignore if */
    if (Array.isArray(def.keyword) ? !def.keyword.includes(keyword) : def.keyword !== keyword) {
        throw new Error("ajv implementation error");
    }
    const deps = def.dependencies;
    if (deps === null || deps === void 0 ? void 0 : deps.some((kwd) => !Object.prototype.hasOwnProperty.call(schema, kwd))) {
        throw new Error(`parent schema must have dependencies of ${keyword}: ${deps.join(",")}`);
    }
    if (def.validateSchema) {
        const valid = def.validateSchema(schema[keyword]);
        if (!valid) {
            const msg = `keyword "${keyword}" value is invalid at path "${errSchemaPath}": ` +
                self.errorsText(def.validateSchema.errors);
            if (opts.validateSchema === "log")
                self.logger.error(msg);
            else
                throw new Error(msg);
        }
    }
}
exports.validateKeywordUsage = validateKeywordUsage;
//# sourceMappingURL=keyword.js.map
});

const keyword$1 = /*@__PURE__*/getDefaultExportFromCjs(keyword);

var subschema = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.extendSubschemaMode = exports.extendSubschemaData = exports.getSubschema = void 0;


function getSubschema(it, { keyword, schemaProp, schema, schemaPath, errSchemaPath, topSchemaRef }) {
    if (keyword !== undefined && schema !== undefined) {
        throw new Error('both "keyword" and "schema" passed, only one allowed');
    }
    if (keyword !== undefined) {
        const sch = it.schema[keyword];
        return schemaProp === undefined
            ? {
                schema: sch,
                schemaPath: (0, codegen._) `${it.schemaPath}${(0, codegen.getProperty)(keyword)}`,
                errSchemaPath: `${it.errSchemaPath}/${keyword}`,
            }
            : {
                schema: sch[schemaProp],
                schemaPath: (0, codegen._) `${it.schemaPath}${(0, codegen.getProperty)(keyword)}${(0, codegen.getProperty)(schemaProp)}`,
                errSchemaPath: `${it.errSchemaPath}/${keyword}/${(0, util.escapeFragment)(schemaProp)}`,
            };
    }
    if (schema !== undefined) {
        if (schemaPath === undefined || errSchemaPath === undefined || topSchemaRef === undefined) {
            throw new Error('"schemaPath", "errSchemaPath" and "topSchemaRef" are required with "schema"');
        }
        return {
            schema,
            schemaPath,
            topSchemaRef,
            errSchemaPath,
        };
    }
    throw new Error('either "keyword" or "schema" must be passed');
}
exports.getSubschema = getSubschema;
function extendSubschemaData(subschema, it, { dataProp, dataPropType: dpType, data, dataTypes, propertyName }) {
    if (data !== undefined && dataProp !== undefined) {
        throw new Error('both "data" and "dataProp" passed, only one allowed');
    }
    const { gen } = it;
    if (dataProp !== undefined) {
        const { errorPath, dataPathArr, opts } = it;
        const nextData = gen.let("data", (0, codegen._) `${it.data}${(0, codegen.getProperty)(dataProp)}`, true);
        dataContextProps(nextData);
        subschema.errorPath = (0, codegen.str) `${errorPath}${(0, util.getErrorPath)(dataProp, dpType, opts.jsPropertySyntax)}`;
        subschema.parentDataProperty = (0, codegen._) `${dataProp}`;
        subschema.dataPathArr = [...dataPathArr, subschema.parentDataProperty];
    }
    if (data !== undefined) {
        const nextData = data instanceof codegen.Name ? data : gen.let("data", data, true); // replaceable if used once?
        dataContextProps(nextData);
        if (propertyName !== undefined)
            subschema.propertyName = propertyName;
        // TODO something is possibly wrong here with not changing parentDataProperty and not appending dataPathArr
    }
    if (dataTypes)
        subschema.dataTypes = dataTypes;
    function dataContextProps(_nextData) {
        subschema.data = _nextData;
        subschema.dataLevel = it.dataLevel + 1;
        subschema.dataTypes = [];
        it.definedProperties = new Set();
        subschema.parentData = it.data;
        subschema.dataNames = [...it.dataNames, _nextData];
    }
}
exports.extendSubschemaData = extendSubschemaData;
function extendSubschemaMode(subschema, { jtdDiscriminator, jtdMetadata, compositeRule, createErrors, allErrors }) {
    if (compositeRule !== undefined)
        subschema.compositeRule = compositeRule;
    if (createErrors !== undefined)
        subschema.createErrors = createErrors;
    if (allErrors !== undefined)
        subschema.allErrors = allErrors;
    subschema.jtdDiscriminator = jtdDiscriminator; // not inherited
    subschema.jtdMetadata = jtdMetadata; // not inherited
}
exports.extendSubschemaMode = extendSubschemaMode;
//# sourceMappingURL=subschema.js.map
});

const subschema$1 = /*@__PURE__*/getDefaultExportFromCjs(subschema);

'use strict';

// do not edit .js files directly - edit src/index.jst



var fastDeepEqual = function equal(a, b) {
  if (a === b) return true;

  if (a && b && typeof a == 'object' && typeof b == 'object') {
    if (a.constructor !== b.constructor) return false;

    var length, i, keys;
    if (Array.isArray(a)) {
      length = a.length;
      if (length != b.length) return false;
      for (i = length; i-- !== 0;)
        if (!equal(a[i], b[i])) return false;
      return true;
    }



    if (a.constructor === RegExp) return a.source === b.source && a.flags === b.flags;
    if (a.valueOf !== Object.prototype.valueOf) return a.valueOf() === b.valueOf();
    if (a.toString !== Object.prototype.toString) return a.toString() === b.toString();

    keys = Object.keys(a);
    length = keys.length;
    if (length !== Object.keys(b).length) return false;

    for (i = length; i-- !== 0;)
      if (!Object.prototype.hasOwnProperty.call(b, keys[i])) return false;

    for (i = length; i-- !== 0;) {
      var key = keys[i];

      if (!equal(a[key], b[key])) return false;
    }

    return true;
  }

  // true if both NaN, false otherwise
  return a!==a && b!==b;
};

var jsonSchemaTraverse = createCommonjsModule(function (module) {
'use strict';

var traverse = module.exports = function (schema, opts, cb) {
  // Legacy support for v0.3.1 and earlier.
  if (typeof opts == 'function') {
    cb = opts;
    opts = {};
  }

  cb = opts.cb || cb;
  var pre = (typeof cb == 'function') ? cb : cb.pre || function() {};
  var post = cb.post || function() {};

  _traverse(opts, pre, post, schema, '', schema);
};


traverse.keywords = {
  additionalItems: true,
  items: true,
  contains: true,
  additionalProperties: true,
  propertyNames: true,
  not: true,
  if: true,
  then: true,
  else: true
};

traverse.arrayKeywords = {
  items: true,
  allOf: true,
  anyOf: true,
  oneOf: true
};

traverse.propsKeywords = {
  $defs: true,
  definitions: true,
  properties: true,
  patternProperties: true,
  dependencies: true
};

traverse.skipKeywords = {
  default: true,
  enum: true,
  const: true,
  required: true,
  maximum: true,
  minimum: true,
  exclusiveMaximum: true,
  exclusiveMinimum: true,
  multipleOf: true,
  maxLength: true,
  minLength: true,
  pattern: true,
  format: true,
  maxItems: true,
  minItems: true,
  uniqueItems: true,
  maxProperties: true,
  minProperties: true
};


function _traverse(opts, pre, post, schema, jsonPtr, rootSchema, parentJsonPtr, parentKeyword, parentSchema, keyIndex) {
  if (schema && typeof schema == 'object' && !Array.isArray(schema)) {
    pre(schema, jsonPtr, rootSchema, parentJsonPtr, parentKeyword, parentSchema, keyIndex);
    for (var key in schema) {
      var sch = schema[key];
      if (Array.isArray(sch)) {
        if (key in traverse.arrayKeywords) {
          for (var i=0; i<sch.length; i++)
            _traverse(opts, pre, post, sch[i], jsonPtr + '/' + key + '/' + i, rootSchema, jsonPtr, key, schema, i);
        }
      } else if (key in traverse.propsKeywords) {
        if (sch && typeof sch == 'object') {
          for (var prop in sch)
            _traverse(opts, pre, post, sch[prop], jsonPtr + '/' + key + '/' + escapeJsonPtr(prop), rootSchema, jsonPtr, key, schema, prop);
        }
      } else if (key in traverse.keywords || (opts.allKeys && !(key in traverse.skipKeywords))) {
        _traverse(opts, pre, post, sch, jsonPtr + '/' + key, rootSchema, jsonPtr, key, schema);
      }
    }
    post(schema, jsonPtr, rootSchema, parentJsonPtr, parentKeyword, parentSchema, keyIndex);
  }
}


function escapeJsonPtr(str) {
  return str.replace(/~/g, '~0').replace(/\//g, '~1');
}
});

var resolve$1 = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.getSchemaRefs = exports.resolveUrl = exports.normalizeId = exports._getFullPath = exports.getFullPath = exports.inlineRef = void 0;



// TODO refactor to use keyword definitions
const SIMPLE_INLINED = new Set([
    "type",
    "format",
    "pattern",
    "maxLength",
    "minLength",
    "maxProperties",
    "minProperties",
    "maxItems",
    "minItems",
    "maximum",
    "minimum",
    "uniqueItems",
    "multipleOf",
    "required",
    "enum",
    "const",
]);
function inlineRef(schema, limit = true) {
    if (typeof schema == "boolean")
        return true;
    if (limit === true)
        return !hasRef(schema);
    if (!limit)
        return false;
    return countKeys(schema) <= limit;
}
exports.inlineRef = inlineRef;
const REF_KEYWORDS = new Set([
    "$ref",
    "$recursiveRef",
    "$recursiveAnchor",
    "$dynamicRef",
    "$dynamicAnchor",
]);
function hasRef(schema) {
    for (const key in schema) {
        if (REF_KEYWORDS.has(key))
            return true;
        const sch = schema[key];
        if (Array.isArray(sch) && sch.some(hasRef))
            return true;
        if (typeof sch == "object" && hasRef(sch))
            return true;
    }
    return false;
}
function countKeys(schema) {
    let count = 0;
    for (const key in schema) {
        if (key === "$ref")
            return Infinity;
        count++;
        if (SIMPLE_INLINED.has(key))
            continue;
        if (typeof schema[key] == "object") {
            (0, util.eachItem)(schema[key], (sch) => (count += countKeys(sch)));
        }
        if (count === Infinity)
            return Infinity;
    }
    return count;
}
function getFullPath(resolver, id = "", normalize) {
    if (normalize !== false)
        id = normalizeId(id);
    const p = resolver.parse(id);
    return _getFullPath(resolver, p);
}
exports.getFullPath = getFullPath;
function _getFullPath(resolver, p) {
    const serialized = resolver.serialize(p);
    return serialized.split("#")[0] + "#";
}
exports._getFullPath = _getFullPath;
const TRAILING_SLASH_HASH = /#\/?$/;
function normalizeId(id) {
    return id ? id.replace(TRAILING_SLASH_HASH, "") : "";
}
exports.normalizeId = normalizeId;
function resolveUrl(resolver, baseId, id) {
    id = normalizeId(id);
    return resolver.resolve(baseId, id);
}
exports.resolveUrl = resolveUrl;
const ANCHOR = /^[a-z_][-a-z0-9._]*$/i;
function getSchemaRefs(schema, baseId) {
    if (typeof schema == "boolean")
        return {};
    const { schemaId, uriResolver } = this.opts;
    const schId = normalizeId(schema[schemaId] || baseId);
    const baseIds = { "": schId };
    const pathPrefix = getFullPath(uriResolver, schId, false);
    const localRefs = {};
    const schemaRefs = new Set();
    jsonSchemaTraverse(schema, { allKeys: true }, (sch, jsonPtr, _, parentJsonPtr) => {
        if (parentJsonPtr === undefined)
            return;
        const fullPath = pathPrefix + jsonPtr;
        let innerBaseId = baseIds[parentJsonPtr];
        if (typeof sch[schemaId] == "string")
            innerBaseId = addRef.call(this, sch[schemaId]);
        addAnchor.call(this, sch.$anchor);
        addAnchor.call(this, sch.$dynamicAnchor);
        baseIds[jsonPtr] = innerBaseId;
        function addRef(ref) {
            // eslint-disable-next-line @typescript-eslint/unbound-method
            const _resolve = this.opts.uriResolver.resolve;
            ref = normalizeId(innerBaseId ? _resolve(innerBaseId, ref) : ref);
            if (schemaRefs.has(ref))
                throw ambiguos(ref);
            schemaRefs.add(ref);
            let schOrRef = this.refs[ref];
            if (typeof schOrRef == "string")
                schOrRef = this.refs[schOrRef];
            if (typeof schOrRef == "object") {
                checkAmbiguosRef(sch, schOrRef.schema, ref);
            }
            else if (ref !== normalizeId(fullPath)) {
                if (ref[0] === "#") {
                    checkAmbiguosRef(sch, localRefs[ref], ref);
                    localRefs[ref] = sch;
                }
                else {
                    this.refs[ref] = fullPath;
                }
            }
            return ref;
        }
        function addAnchor(anchor) {
            if (typeof anchor == "string") {
                if (!ANCHOR.test(anchor))
                    throw new Error(`invalid anchor "${anchor}"`);
                addRef.call(this, `#${anchor}`);
            }
        }
    });
    return localRefs;
    function checkAmbiguosRef(sch1, sch2, ref) {
        if (sch2 !== undefined && !fastDeepEqual(sch1, sch2))
            throw ambiguos(ref);
    }
    function ambiguos(ref) {
        return new Error(`reference "${ref}" resolves to more than one schema`);
    }
}
exports.getSchemaRefs = getSchemaRefs;
//# sourceMappingURL=resolve.js.map
});

const resolve$2 = /*@__PURE__*/getDefaultExportFromCjs(resolve$1);

var validate = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.getData = exports.KeywordCxt = exports.validateFunctionCode = void 0;



const dataType_2 = dataType;








// schema compilation - generates validation function, subschemaCode (below) is used for subschemas
function validateFunctionCode(it) {
    if (isSchemaObj(it)) {
        checkKeywords(it);
        if (schemaCxtHasRules(it)) {
            topSchemaObjCode(it);
            return;
        }
    }
    validateFunction(it, () => (0, boolSchema.topBoolOrEmptySchema)(it));
}
exports.validateFunctionCode = validateFunctionCode;
function validateFunction({ gen, validateName, schema, schemaEnv, opts }, body) {
    if (opts.code.es5) {
        gen.func(validateName, (0, codegen._) `${names_1.default.data}, ${names_1.default.valCxt}`, schemaEnv.$async, () => {
            gen.code((0, codegen._) `"use strict"; ${funcSourceUrl(schema, opts)}`);
            destructureValCxtES5(gen, opts);
            gen.code(body);
        });
    }
    else {
        gen.func(validateName, (0, codegen._) `${names_1.default.data}, ${destructureValCxt(opts)}`, schemaEnv.$async, () => gen.code(funcSourceUrl(schema, opts)).code(body));
    }
}
function destructureValCxt(opts) {
    return (0, codegen._) `{${names_1.default.instancePath}="", ${names_1.default.parentData}, ${names_1.default.parentDataProperty}, ${names_1.default.rootData}=${names_1.default.data}${opts.dynamicRef ? (0, codegen._) `, ${names_1.default.dynamicAnchors}={}` : codegen.nil}}={}`;
}
function destructureValCxtES5(gen, opts) {
    gen.if(names_1.default.valCxt, () => {
        gen.var(names_1.default.instancePath, (0, codegen._) `${names_1.default.valCxt}.${names_1.default.instancePath}`);
        gen.var(names_1.default.parentData, (0, codegen._) `${names_1.default.valCxt}.${names_1.default.parentData}`);
        gen.var(names_1.default.parentDataProperty, (0, codegen._) `${names_1.default.valCxt}.${names_1.default.parentDataProperty}`);
        gen.var(names_1.default.rootData, (0, codegen._) `${names_1.default.valCxt}.${names_1.default.rootData}`);
        if (opts.dynamicRef)
            gen.var(names_1.default.dynamicAnchors, (0, codegen._) `${names_1.default.valCxt}.${names_1.default.dynamicAnchors}`);
    }, () => {
        gen.var(names_1.default.instancePath, (0, codegen._) `""`);
        gen.var(names_1.default.parentData, (0, codegen._) `undefined`);
        gen.var(names_1.default.parentDataProperty, (0, codegen._) `undefined`);
        gen.var(names_1.default.rootData, names_1.default.data);
        if (opts.dynamicRef)
            gen.var(names_1.default.dynamicAnchors, (0, codegen._) `{}`);
    });
}
function topSchemaObjCode(it) {
    const { schema, opts, gen } = it;
    validateFunction(it, () => {
        if (opts.$comment && schema.$comment)
            commentKeyword(it);
        checkNoDefault(it);
        gen.let(names_1.default.vErrors, null);
        gen.let(names_1.default.errors, 0);
        if (opts.unevaluated)
            resetEvaluated(it);
        typeAndKeywords(it);
        returnResults(it);
    });
    return;
}
function resetEvaluated(it) {
    // TODO maybe some hook to execute it in the end to check whether props/items are Name, as in assignEvaluated
    const { gen, validateName } = it;
    it.evaluated = gen.const("evaluated", (0, codegen._) `${validateName}.evaluated`);
    gen.if((0, codegen._) `${it.evaluated}.dynamicProps`, () => gen.assign((0, codegen._) `${it.evaluated}.props`, (0, codegen._) `undefined`));
    gen.if((0, codegen._) `${it.evaluated}.dynamicItems`, () => gen.assign((0, codegen._) `${it.evaluated}.items`, (0, codegen._) `undefined`));
}
function funcSourceUrl(schema, opts) {
    const schId = typeof schema == "object" && schema[opts.schemaId];
    return schId && (opts.code.source || opts.code.process) ? (0, codegen._) `/*# sourceURL=${schId} */` : codegen.nil;
}
// schema compilation - this function is used recursively to generate code for sub-schemas
function subschemaCode(it, valid) {
    if (isSchemaObj(it)) {
        checkKeywords(it);
        if (schemaCxtHasRules(it)) {
            subSchemaObjCode(it, valid);
            return;
        }
    }
    (0, boolSchema.boolOrEmptySchema)(it, valid);
}
function schemaCxtHasRules({ schema, self }) {
    if (typeof schema == "boolean")
        return !schema;
    for (const key in schema)
        if (self.RULES.all[key])
            return true;
    return false;
}
function isSchemaObj(it) {
    return typeof it.schema != "boolean";
}
function subSchemaObjCode(it, valid) {
    const { schema, gen, opts } = it;
    if (opts.$comment && schema.$comment)
        commentKeyword(it);
    updateContext(it);
    checkAsyncSchema(it);
    const errsCount = gen.const("_errs", names_1.default.errors);
    typeAndKeywords(it, errsCount);
    // TODO var
    gen.var(valid, (0, codegen._) `${errsCount} === ${names_1.default.errors}`);
}
function checkKeywords(it) {
    (0, util.checkUnknownRules)(it);
    checkRefsAndKeywords(it);
}
function typeAndKeywords(it, errsCount) {
    if (it.opts.jtd)
        return schemaKeywords(it, [], false, errsCount);
    const types = (0, dataType.getSchemaTypes)(it.schema);
    const checkedTypes = (0, dataType.coerceAndCheckDataType)(it, types);
    schemaKeywords(it, types, !checkedTypes, errsCount);
}
function checkRefsAndKeywords(it) {
    const { schema, errSchemaPath, opts, self } = it;
    if (schema.$ref && opts.ignoreKeywordsWithRef && (0, util.schemaHasRulesButRef)(schema, self.RULES)) {
        self.logger.warn(`$ref: keywords ignored in schema at path "${errSchemaPath}"`);
    }
}
function checkNoDefault(it) {
    const { schema, opts } = it;
    if (schema.default !== undefined && opts.useDefaults && opts.strictSchema) {
        (0, util.checkStrictMode)(it, "default is ignored in the schema root");
    }
}
function updateContext(it) {
    const schId = it.schema[it.opts.schemaId];
    if (schId)
        it.baseId = (0, resolve$1.resolveUrl)(it.opts.uriResolver, it.baseId, schId);
}
function checkAsyncSchema(it) {
    if (it.schema.$async && !it.schemaEnv.$async)
        throw new Error("async schema in sync schema");
}
function commentKeyword({ gen, schemaEnv, schema, errSchemaPath, opts }) {
    const msg = schema.$comment;
    if (opts.$comment === true) {
        gen.code((0, codegen._) `${names_1.default.self}.logger.log(${msg})`);
    }
    else if (typeof opts.$comment == "function") {
        const schemaPath = (0, codegen.str) `${errSchemaPath}/$comment`;
        const rootName = gen.scopeValue("root", { ref: schemaEnv.root });
        gen.code((0, codegen._) `${names_1.default.self}.opts.$comment(${msg}, ${schemaPath}, ${rootName}.schema)`);
    }
}
function returnResults(it) {
    const { gen, schemaEnv, validateName, ValidationError, opts } = it;
    if (schemaEnv.$async) {
        // TODO assign unevaluated
        gen.if((0, codegen._) `${names_1.default.errors} === 0`, () => gen.return(names_1.default.data), () => gen.throw((0, codegen._) `new ${ValidationError}(${names_1.default.vErrors})`));
    }
    else {
        gen.assign((0, codegen._) `${validateName}.errors`, names_1.default.vErrors);
        if (opts.unevaluated)
            assignEvaluated(it);
        gen.return((0, codegen._) `${names_1.default.errors} === 0`);
    }
}
function assignEvaluated({ gen, evaluated, props, items }) {
    if (props instanceof codegen.Name)
        gen.assign((0, codegen._) `${evaluated}.props`, props);
    if (items instanceof codegen.Name)
        gen.assign((0, codegen._) `${evaluated}.items`, items);
}
function schemaKeywords(it, types, typeErrors, errsCount) {
    const { gen, schema, data, allErrors, opts, self } = it;
    const { RULES } = self;
    if (schema.$ref && (opts.ignoreKeywordsWithRef || !(0, util.schemaHasRulesButRef)(schema, RULES))) {
        gen.block(() => keywordCode(it, "$ref", RULES.all.$ref.definition)); // TODO typecast
        return;
    }
    if (!opts.jtd)
        checkStrictTypes(it, types);
    gen.block(() => {
        for (const group of RULES.rules)
            groupKeywords(group);
        groupKeywords(RULES.post);
    });
    function groupKeywords(group) {
        if (!(0, applicability.shouldUseGroup)(schema, group))
            return;
        if (group.type) {
            gen.if((0, dataType_2.checkDataType)(group.type, data, opts.strictNumbers));
            iterateKeywords(it, group);
            if (types.length === 1 && types[0] === group.type && typeErrors) {
                gen.else();
                (0, dataType_2.reportTypeError)(it);
            }
            gen.endIf();
        }
        else {
            iterateKeywords(it, group);
        }
        // TODO make it "ok" call?
        if (!allErrors)
            gen.if((0, codegen._) `${names_1.default.errors} === ${errsCount || 0}`);
    }
}
function iterateKeywords(it, group) {
    const { gen, schema, opts: { useDefaults }, } = it;
    if (useDefaults)
        (0, defaults.assignDefaults)(it, group.type);
    gen.block(() => {
        for (const rule of group.rules) {
            if ((0, applicability.shouldUseRule)(schema, rule)) {
                keywordCode(it, rule.keyword, rule.definition, group.type);
            }
        }
    });
}
function checkStrictTypes(it, types) {
    if (it.schemaEnv.meta || !it.opts.strictTypes)
        return;
    checkContextTypes(it, types);
    if (!it.opts.allowUnionTypes)
        checkMultipleTypes(it, types);
    checkKeywordTypes(it, it.dataTypes);
}
function checkContextTypes(it, types) {
    if (!types.length)
        return;
    if (!it.dataTypes.length) {
        it.dataTypes = types;
        return;
    }
    types.forEach((t) => {
        if (!includesType(it.dataTypes, t)) {
            strictTypesError(it, `type "${t}" not allowed by context "${it.dataTypes.join(",")}"`);
        }
    });
    narrowSchemaTypes(it, types);
}
function checkMultipleTypes(it, ts) {
    if (ts.length > 1 && !(ts.length === 2 && ts.includes("null"))) {
        strictTypesError(it, "use allowUnionTypes to allow union type keyword");
    }
}
function checkKeywordTypes(it, ts) {
    const rules = it.self.RULES.all;
    for (const keyword in rules) {
        const rule = rules[keyword];
        if (typeof rule == "object" && (0, applicability.shouldUseRule)(it.schema, rule)) {
            const { type } = rule.definition;
            if (type.length && !type.some((t) => hasApplicableType(ts, t))) {
                strictTypesError(it, `missing type "${type.join(",")}" for keyword "${keyword}"`);
            }
        }
    }
}
function hasApplicableType(schTs, kwdT) {
    return schTs.includes(kwdT) || (kwdT === "number" && schTs.includes("integer"));
}
function includesType(ts, t) {
    return ts.includes(t) || (t === "integer" && ts.includes("number"));
}
function narrowSchemaTypes(it, withTypes) {
    const ts = [];
    for (const t of it.dataTypes) {
        if (includesType(withTypes, t))
            ts.push(t);
        else if (withTypes.includes("integer") && t === "number")
            ts.push("integer");
    }
    it.dataTypes = ts;
}
function strictTypesError(it, msg) {
    const schemaPath = it.schemaEnv.baseId + it.errSchemaPath;
    msg += ` at "${schemaPath}" (strictTypes)`;
    (0, util.checkStrictMode)(it, msg, it.opts.strictTypes);
}
class KeywordCxt {
    constructor(it, def, keyword$1) {
        (0, keyword.validateKeywordUsage)(it, def, keyword$1);
        this.gen = it.gen;
        this.allErrors = it.allErrors;
        this.keyword = keyword$1;
        this.data = it.data;
        this.schema = it.schema[keyword$1];
        this.$data = def.$data && it.opts.$data && this.schema && this.schema.$data;
        this.schemaValue = (0, util.schemaRefOrVal)(it, this.schema, keyword$1, this.$data);
        this.schemaType = def.schemaType;
        this.parentSchema = it.schema;
        this.params = {};
        this.it = it;
        this.def = def;
        if (this.$data) {
            this.schemaCode = it.gen.const("vSchema", getData(this.$data, it));
        }
        else {
            this.schemaCode = this.schemaValue;
            if (!(0, keyword.validSchemaType)(this.schema, def.schemaType, def.allowUndefined)) {
                throw new Error(`${keyword$1} value must be ${JSON.stringify(def.schemaType)}`);
            }
        }
        if ("code" in def ? def.trackErrors : def.errors !== false) {
            this.errsCount = it.gen.const("_errs", names_1.default.errors);
        }
    }
    result(condition, successAction, failAction) {
        this.failResult((0, codegen.not)(condition), successAction, failAction);
    }
    failResult(condition, successAction, failAction) {
        this.gen.if(condition);
        if (failAction)
            failAction();
        else
            this.error();
        if (successAction) {
            this.gen.else();
            successAction();
            if (this.allErrors)
                this.gen.endIf();
        }
        else {
            if (this.allErrors)
                this.gen.endIf();
            else
                this.gen.else();
        }
    }
    pass(condition, failAction) {
        this.failResult((0, codegen.not)(condition), undefined, failAction);
    }
    fail(condition) {
        if (condition === undefined) {
            this.error();
            if (!this.allErrors)
                this.gen.if(false); // this branch will be removed by gen.optimize
            return;
        }
        this.gen.if(condition);
        this.error();
        if (this.allErrors)
            this.gen.endIf();
        else
            this.gen.else();
    }
    fail$data(condition) {
        if (!this.$data)
            return this.fail(condition);
        const { schemaCode } = this;
        this.fail((0, codegen._) `${schemaCode} !== undefined && (${(0, codegen.or)(this.invalid$data(), condition)})`);
    }
    error(append, errorParams, errorPaths) {
        if (errorParams) {
            this.setParams(errorParams);
            this._error(append, errorPaths);
            this.setParams({});
            return;
        }
        this._error(append, errorPaths);
    }
    _error(append, errorPaths) {
        ;
        (append ? errors.reportExtraError : errors.reportError)(this, this.def.error, errorPaths);
    }
    $dataError() {
        (0, errors.reportError)(this, this.def.$dataError || errors.keyword$DataError);
    }
    reset() {
        if (this.errsCount === undefined)
            throw new Error('add "trackErrors" to keyword definition');
        (0, errors.resetErrorsCount)(this.gen, this.errsCount);
    }
    ok(cond) {
        if (!this.allErrors)
            this.gen.if(cond);
    }
    setParams(obj, assign) {
        if (assign)
            Object.assign(this.params, obj);
        else
            this.params = obj;
    }
    block$data(valid, codeBlock, $dataValid = codegen.nil) {
        this.gen.block(() => {
            this.check$data(valid, $dataValid);
            codeBlock();
        });
    }
    check$data(valid = codegen.nil, $dataValid = codegen.nil) {
        if (!this.$data)
            return;
        const { gen, schemaCode, schemaType, def } = this;
        gen.if((0, codegen.or)((0, codegen._) `${schemaCode} === undefined`, $dataValid));
        if (valid !== codegen.nil)
            gen.assign(valid, true);
        if (schemaType.length || def.validateSchema) {
            gen.elseIf(this.invalid$data());
            this.$dataError();
            if (valid !== codegen.nil)
                gen.assign(valid, false);
        }
        gen.else();
    }
    invalid$data() {
        const { gen, schemaCode, schemaType, def, it } = this;
        return (0, codegen.or)(wrong$DataType(), invalid$DataSchema());
        function wrong$DataType() {
            if (schemaType.length) {
                /* istanbul ignore if */
                if (!(schemaCode instanceof codegen.Name))
                    throw new Error("ajv implementation error");
                const st = Array.isArray(schemaType) ? schemaType : [schemaType];
                return (0, codegen._) `${(0, dataType_2.checkDataTypes)(st, schemaCode, it.opts.strictNumbers, dataType_2.DataType.Wrong)}`;
            }
            return codegen.nil;
        }
        function invalid$DataSchema() {
            if (def.validateSchema) {
                const validateSchemaRef = gen.scopeValue("validate$data", { ref: def.validateSchema }); // TODO value.code for standalone
                return (0, codegen._) `!${validateSchemaRef}(${schemaCode})`;
            }
            return codegen.nil;
        }
    }
    subschema(appl, valid) {
        const subschema$1 = (0, subschema.getSubschema)(this.it, appl);
        (0, subschema.extendSubschemaData)(subschema$1, this.it, appl);
        (0, subschema.extendSubschemaMode)(subschema$1, appl);
        const nextContext = { ...this.it, ...subschema$1, items: undefined, props: undefined };
        subschemaCode(nextContext, valid);
        return nextContext;
    }
    mergeEvaluated(schemaCxt, toName) {
        const { it, gen } = this;
        if (!it.opts.unevaluated)
            return;
        if (it.props !== true && schemaCxt.props !== undefined) {
            it.props = util.mergeEvaluated.props(gen, schemaCxt.props, it.props, toName);
        }
        if (it.items !== true && schemaCxt.items !== undefined) {
            it.items = util.mergeEvaluated.items(gen, schemaCxt.items, it.items, toName);
        }
    }
    mergeValidEvaluated(schemaCxt, valid) {
        const { it, gen } = this;
        if (it.opts.unevaluated && (it.props !== true || it.items !== true)) {
            gen.if(valid, () => this.mergeEvaluated(schemaCxt, codegen.Name));
            return true;
        }
    }
}
exports.KeywordCxt = KeywordCxt;
function keywordCode(it, keyword$1, def, ruleType) {
    const cxt = new KeywordCxt(it, def, keyword$1);
    if ("code" in def) {
        def.code(cxt, ruleType);
    }
    else if (cxt.$data && def.validate) {
        (0, keyword.funcKeywordCode)(cxt, def);
    }
    else if ("macro" in def) {
        (0, keyword.macroKeywordCode)(cxt, def);
    }
    else if (def.compile || def.validate) {
        (0, keyword.funcKeywordCode)(cxt, def);
    }
}
const JSON_POINTER = /^\/(?:[^~]|~0|~1)*$/;
const RELATIVE_JSON_POINTER = /^([0-9]+)(#|\/(?:[^~]|~0|~1)*)?$/;
function getData($data, { dataLevel, dataNames, dataPathArr }) {
    let jsonPointer;
    let data;
    if ($data === "")
        return names_1.default.rootData;
    if ($data[0] === "/") {
        if (!JSON_POINTER.test($data))
            throw new Error(`Invalid JSON-pointer: ${$data}`);
        jsonPointer = $data;
        data = names_1.default.rootData;
    }
    else {
        const matches = RELATIVE_JSON_POINTER.exec($data);
        if (!matches)
            throw new Error(`Invalid JSON-pointer: ${$data}`);
        const up = +matches[1];
        jsonPointer = matches[2];
        if (jsonPointer === "#") {
            if (up >= dataLevel)
                throw new Error(errorMsg("property/index", up));
            return dataPathArr[dataLevel - up];
        }
        if (up > dataLevel)
            throw new Error(errorMsg("data", up));
        data = dataNames[dataLevel - up];
        if (!jsonPointer)
            return data;
    }
    let expr = data;
    const segments = jsonPointer.split("/");
    for (const segment of segments) {
        if (segment) {
            data = (0, codegen._) `${data}${(0, codegen.getProperty)((0, util.unescapeJsonPointer)(segment))}`;
            expr = (0, codegen._) `${expr} && ${data}`;
        }
    }
    return expr;
    function errorMsg(pointerType, up) {
        return `Cannot access ${pointerType} ${up} levels up, current level is ${dataLevel}`;
    }
}
exports.getData = getData;
//# sourceMappingURL=index.js.map
});

const index$7 = /*@__PURE__*/getDefaultExportFromCjs(validate);

var validation_error = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
class ValidationError extends Error {
    constructor(errors) {
        super("validation failed");
        this.errors = errors;
        this.ajv = this.validation = true;
    }
}
exports.default = ValidationError;
//# sourceMappingURL=validation_error.js.map
});

const validation_error$1 = /*@__PURE__*/getDefaultExportFromCjs(validation_error);

var ref_error = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });

class MissingRefError extends Error {
    constructor(resolver, baseId, ref, msg) {
        super(msg || `can't resolve reference ${ref} from id ${baseId}`);
        this.missingRef = (0, resolve$1.resolveUrl)(resolver, baseId, ref);
        this.missingSchema = (0, resolve$1.normalizeId)((0, resolve$1.getFullPath)(resolver, this.missingRef));
    }
}
exports.default = MissingRefError;
//# sourceMappingURL=ref_error.js.map
});

const ref_error$1 = /*@__PURE__*/getDefaultExportFromCjs(ref_error);

var compile = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.resolveSchema = exports.getCompilingSchema = exports.resolveRef = exports.compileSchema = exports.SchemaEnv = void 0;






class SchemaEnv {
    constructor(env) {
        var _a;
        this.refs = {};
        this.dynamicAnchors = {};
        let schema;
        if (typeof env.schema == "object")
            schema = env.schema;
        this.schema = env.schema;
        this.schemaId = env.schemaId;
        this.root = env.root || this;
        this.baseId = (_a = env.baseId) !== null && _a !== void 0 ? _a : (0, resolve$1.normalizeId)(schema === null || schema === void 0 ? void 0 : schema[env.schemaId || "$id"]);
        this.schemaPath = env.schemaPath;
        this.localRefs = env.localRefs;
        this.meta = env.meta;
        this.$async = schema === null || schema === void 0 ? void 0 : schema.$async;
        this.refs = {};
    }
}
exports.SchemaEnv = SchemaEnv;
// let codeSize = 0
// let nodeCount = 0
// Compiles schema in SchemaEnv
function compileSchema(sch) {
    // TODO refactor - remove compilations
    const _sch = getCompilingSchema.call(this, sch);
    if (_sch)
        return _sch;
    const rootId = (0, resolve$1.getFullPath)(this.opts.uriResolver, sch.root.baseId); // TODO if getFullPath removed 1 tests fails
    const { es5, lines } = this.opts.code;
    const { ownProperties } = this.opts;
    const gen = new codegen.CodeGen(this.scope, { es5, lines, ownProperties });
    let _ValidationError;
    if (sch.$async) {
        _ValidationError = gen.scopeValue("Error", {
            ref: validation_error.default,
            code: (0, codegen._) `require("ajv/dist/runtime/validation_error").default`,
        });
    }
    const validateName = gen.scopeName("validate");
    sch.validateName = validateName;
    const schemaCxt = {
        gen,
        allErrors: this.opts.allErrors,
        data: names_1.default.data,
        parentData: names_1.default.parentData,
        parentDataProperty: names_1.default.parentDataProperty,
        dataNames: [names_1.default.data],
        dataPathArr: [codegen.nil], // TODO can its length be used as dataLevel if nil is removed?
        dataLevel: 0,
        dataTypes: [],
        definedProperties: new Set(),
        topSchemaRef: gen.scopeValue("schema", this.opts.code.source === true
            ? { ref: sch.schema, code: (0, codegen.stringify)(sch.schema) }
            : { ref: sch.schema }),
        validateName,
        ValidationError: _ValidationError,
        schema: sch.schema,
        schemaEnv: sch,
        rootId,
        baseId: sch.baseId || rootId,
        schemaPath: codegen.nil,
        errSchemaPath: sch.schemaPath || (this.opts.jtd ? "" : "#"),
        errorPath: (0, codegen._) `""`,
        opts: this.opts,
        self: this,
    };
    let sourceCode;
    try {
        this._compilations.add(sch);
        (0, validate.validateFunctionCode)(schemaCxt);
        gen.optimize(this.opts.code.optimize);
        // gen.optimize(1)
        const validateCode = gen.toString();
        sourceCode = `${gen.scopeRefs(names_1.default.scope)}return ${validateCode}`;
        // console.log((codeSize += sourceCode.length), (nodeCount += gen.nodeCount))
        if (this.opts.code.process)
            sourceCode = this.opts.code.process(sourceCode, sch);
        // console.log("\n\n\n *** \n", sourceCode)
        const makeValidate = new Function(`${names_1.default.self}`, `${names_1.default.scope}`, sourceCode);
        const validate$1 = makeValidate(this, this.scope.get());
        this.scope.value(validateName, { ref: validate$1 });
        validate$1.errors = null;
        validate$1.schema = sch.schema;
        validate$1.schemaEnv = sch;
        if (sch.$async)
            validate$1.$async = true;
        if (this.opts.code.source === true) {
            validate$1.source = { validateName, validateCode, scopeValues: gen._values };
        }
        if (this.opts.unevaluated) {
            const { props, items } = schemaCxt;
            validate$1.evaluated = {
                props: props instanceof codegen.Name ? undefined : props,
                items: items instanceof codegen.Name ? undefined : items,
                dynamicProps: props instanceof codegen.Name,
                dynamicItems: items instanceof codegen.Name,
            };
            if (validate$1.source)
                validate$1.source.evaluated = (0, codegen.stringify)(validate$1.evaluated);
        }
        sch.validate = validate$1;
        return sch;
    }
    catch (e) {
        delete sch.validate;
        delete sch.validateName;
        if (sourceCode)
            this.logger.error("Error compiling schema, function code:", sourceCode);
        // console.log("\n\n\n *** \n", sourceCode, this.opts)
        throw e;
    }
    finally {
        this._compilations.delete(sch);
    }
}
exports.compileSchema = compileSchema;
function resolveRef(root, baseId, ref) {
    var _a;
    ref = (0, resolve$1.resolveUrl)(this.opts.uriResolver, baseId, ref);
    const schOrFunc = root.refs[ref];
    if (schOrFunc)
        return schOrFunc;
    let _sch = resolve.call(this, root, ref);
    if (_sch === undefined) {
        const schema = (_a = root.localRefs) === null || _a === void 0 ? void 0 : _a[ref]; // TODO maybe localRefs should hold SchemaEnv
        const { schemaId } = this.opts;
        if (schema)
            _sch = new SchemaEnv({ schema, schemaId, root, baseId });
    }
    if (_sch === undefined)
        return;
    return (root.refs[ref] = inlineOrCompile.call(this, _sch));
}
exports.resolveRef = resolveRef;
function inlineOrCompile(sch) {
    if ((0, resolve$1.inlineRef)(sch.schema, this.opts.inlineRefs))
        return sch.schema;
    return sch.validate ? sch : compileSchema.call(this, sch);
}
// Index of schema compilation in the currently compiled list
function getCompilingSchema(schEnv) {
    for (const sch of this._compilations) {
        if (sameSchemaEnv(sch, schEnv))
            return sch;
    }
}
exports.getCompilingSchema = getCompilingSchema;
function sameSchemaEnv(s1, s2) {
    return s1.schema === s2.schema && s1.root === s2.root && s1.baseId === s2.baseId;
}
// resolve and compile the references ($ref)
// TODO returns AnySchemaObject (if the schema can be inlined) or validation function
function resolve(root, // information about the root schema for the current schema
ref // reference to resolve
) {
    let sch;
    while (typeof (sch = this.refs[ref]) == "string")
        ref = sch;
    return sch || this.schemas[ref] || resolveSchema.call(this, root, ref);
}
// Resolve schema, its root and baseId
function resolveSchema(root, // root object with properties schema, refs TODO below SchemaEnv is assigned to it
ref // reference to resolve
) {
    const p = this.opts.uriResolver.parse(ref);
    const refPath = (0, resolve$1._getFullPath)(this.opts.uriResolver, p);
    let baseId = (0, resolve$1.getFullPath)(this.opts.uriResolver, root.baseId, undefined);
    // TODO `Object.keys(root.schema).length > 0` should not be needed - but removing breaks 2 tests
    if (Object.keys(root.schema).length > 0 && refPath === baseId) {
        return getJsonPointer.call(this, p, root);
    }
    const id = (0, resolve$1.normalizeId)(refPath);
    const schOrRef = this.refs[id] || this.schemas[id];
    if (typeof schOrRef == "string") {
        const sch = resolveSchema.call(this, root, schOrRef);
        if (typeof (sch === null || sch === void 0 ? void 0 : sch.schema) !== "object")
            return;
        return getJsonPointer.call(this, p, sch);
    }
    if (typeof (schOrRef === null || schOrRef === void 0 ? void 0 : schOrRef.schema) !== "object")
        return;
    if (!schOrRef.validate)
        compileSchema.call(this, schOrRef);
    if (id === (0, resolve$1.normalizeId)(ref)) {
        const { schema } = schOrRef;
        const { schemaId } = this.opts;
        const schId = schema[schemaId];
        if (schId)
            baseId = (0, resolve$1.resolveUrl)(this.opts.uriResolver, baseId, schId);
        return new SchemaEnv({ schema, schemaId, root, baseId });
    }
    return getJsonPointer.call(this, p, schOrRef);
}
exports.resolveSchema = resolveSchema;
const PREVENT_SCOPE_CHANGE = new Set([
    "properties",
    "patternProperties",
    "enum",
    "dependencies",
    "definitions",
]);
function getJsonPointer(parsedRef, { baseId, schema, root }) {
    var _a;
    if (((_a = parsedRef.fragment) === null || _a === void 0 ? void 0 : _a[0]) !== "/")
        return;
    for (const part of parsedRef.fragment.slice(1).split("/")) {
        if (typeof schema === "boolean")
            return;
        const partSchema = schema[(0, util.unescapeFragment)(part)];
        if (partSchema === undefined)
            return;
        schema = partSchema;
        // TODO PREVENT_SCOPE_CHANGE could be defined in keyword def?
        const schId = typeof schema === "object" && schema[this.opts.schemaId];
        if (!PREVENT_SCOPE_CHANGE.has(part) && schId) {
            baseId = (0, resolve$1.resolveUrl)(this.opts.uriResolver, baseId, schId);
        }
    }
    let env;
    if (typeof schema != "boolean" && schema.$ref && !(0, util.schemaHasRulesButRef)(schema, this.RULES)) {
        const $ref = (0, resolve$1.resolveUrl)(this.opts.uriResolver, baseId, schema.$ref);
        env = resolveSchema.call(this, root, $ref);
    }
    // even though resolution failed we need to return SchemaEnv to throw exception
    // so that compileAsync loads missing schema.
    const { schemaId } = this.opts;
    env = env || new SchemaEnv({ schema, schemaId, root, baseId });
    if (env.schema !== env.root.schema)
        return env;
    return undefined;
}
//# sourceMappingURL=index.js.map
});

const index$6 = /*@__PURE__*/getDefaultExportFromCjs(compile);

const $id$1 = "https://raw.githubusercontent.com/ajv-validator/ajv/master/lib/refs/data.json#";
const description = "Meta-schema for $data reference (JSON AnySchema extension proposal)";
const type$1 = "object";
const required$2 = [
	"$data"
];
const properties$3 = {
	$data: {
		type: "string",
		anyOf: [
			{
				format: "relative-json-pointer"
			},
			{
				format: "json-pointer"
			}
		]
	}
};
const additionalProperties$2 = false;
const $dataRefSchema = {
	$id: $id$1,
	description: description,
	type: type$1,
	required: required$2,
	properties: properties$3,
	additionalProperties: additionalProperties$2
};

'use strict';

const HEX$1 = {
  0: 0,
  1: 1,
  2: 2,
  3: 3,
  4: 4,
  5: 5,
  6: 6,
  7: 7,
  8: 8,
  9: 9,
  a: 10,
  A: 10,
  b: 11,
  B: 11,
  c: 12,
  C: 12,
  d: 13,
  D: 13,
  e: 14,
  E: 14,
  f: 15,
  F: 15
};

var scopedChars = {
  HEX: HEX$1
};

'use strict';

const { HEX } = scopedChars;

const IPV4_REG = /^(?:(?:25[0-5]|2[0-4]\d|1\d{2}|[1-9]\d|\d)\.){3}(?:25[0-5]|2[0-4]\d|1\d{2}|[1-9]\d|\d)$/u;

function normalizeIPv4$1 (host) {
  if (findToken(host, '.') < 3) { return { host, isIPV4: false } }
  const matches = host.match(IPV4_REG) || [];
  const [address] = matches;
  if (address) {
    return { host: stripLeadingZeros(address, '.'), isIPV4: true }
  } else {
    return { host, isIPV4: false }
  }
}

/**
 * @param {string[]} input
 * @param {boolean} [keepZero=false]
 * @returns {string|undefined}
 */
function stringArrayToHexStripped (input, keepZero = false) {
  let acc = '';
  let strip = true;
  for (const c of input) {
    if (HEX[c] === undefined) return undefined
    if (c !== '0' && strip === true) strip = false;
    if (!strip) acc += c;
  }
  if (keepZero && acc.length === 0) acc = '0';
  return acc
}

function getIPV6 (input) {
  let tokenCount = 0;
  const output = { error: false, address: '', zone: '' };
  const address = [];
  const buffer = [];
  let isZone = false;
  let endipv6Encountered = false;
  let endIpv6 = false;

  function consume () {
    if (buffer.length) {
      if (isZone === false) {
        const hex = stringArrayToHexStripped(buffer);
        if (hex !== undefined) {
          address.push(hex);
        } else {
          output.error = true;
          return false
        }
      }
      buffer.length = 0;
    }
    return true
  }

  for (let i = 0; i < input.length; i++) {
    const cursor = input[i];
    if (cursor === '[' || cursor === ']') { continue }
    if (cursor === ':') {
      if (endipv6Encountered === true) {
        endIpv6 = true;
      }
      if (!consume()) { break }
      tokenCount++;
      address.push(':');
      if (tokenCount > 7) {
        // not valid
        output.error = true;
        break
      }
      if (i - 1 >= 0 && input[i - 1] === ':') {
        endipv6Encountered = true;
      }
      continue
    } else if (cursor === '%') {
      if (!consume()) { break }
      // switch to zone detection
      isZone = true;
    } else {
      buffer.push(cursor);
      continue
    }
  }
  if (buffer.length) {
    if (isZone) {
      output.zone = buffer.join('');
    } else if (endIpv6) {
      address.push(buffer.join(''));
    } else {
      address.push(stringArrayToHexStripped(buffer));
    }
  }
  output.address = address.join('');
  return output
}

function normalizeIPv6$1 (host) {
  if (findToken(host, ':') < 2) { return { host, isIPV6: false } }
  const ipv6 = getIPV6(host);

  if (!ipv6.error) {
    let newHost = ipv6.address;
    let escapedHost = ipv6.address;
    if (ipv6.zone) {
      newHost += '%' + ipv6.zone;
      escapedHost += '%25' + ipv6.zone;
    }
    return { host: newHost, escapedHost, isIPV6: true }
  } else {
    return { host, isIPV6: false }
  }
}

function stripLeadingZeros (str, token) {
  let out = '';
  let skip = true;
  const l = str.length;
  for (let i = 0; i < l; i++) {
    const c = str[i];
    if (c === '0' && skip) {
      if ((i + 1 <= l && str[i + 1] === token) || i + 1 === l) {
        out += c;
        skip = false;
      }
    } else {
      if (c === token) {
        skip = true;
      } else {
        skip = false;
      }
      out += c;
    }
  }
  return out
}

function findToken (str, token) {
  let ind = 0;
  for (let i = 0; i < str.length; i++) {
    if (str[i] === token) ind++;
  }
  return ind
}

const RDS1 = /^\.\.?\//u;
const RDS2 = /^\/\.(?:\/|$)/u;
const RDS3 = /^\/\.\.(?:\/|$)/u;
const RDS5 = /^\/?(?:.|\n)*?(?=\/|$)/u;

function removeDotSegments$1 (input) {
  const output = [];

  while (input.length) {
    if (input.match(RDS1)) {
      input = input.replace(RDS1, '');
    } else if (input.match(RDS2)) {
      input = input.replace(RDS2, '/');
    } else if (input.match(RDS3)) {
      input = input.replace(RDS3, '/');
      output.pop();
    } else if (input === '.' || input === '..') {
      input = '';
    } else {
      const im = input.match(RDS5);
      if (im) {
        const s = im[0];
        input = input.slice(s.length);
        output.push(s);
      } else {
        throw new Error('Unexpected dot segment condition')
      }
    }
  }
  return output.join('')
}

function normalizeComponentEncoding$1 (components, esc) {
  const func = esc !== true ? escape : unescape;
  if (components.scheme !== undefined) {
    components.scheme = func(components.scheme);
  }
  if (components.userinfo !== undefined) {
    components.userinfo = func(components.userinfo);
  }
  if (components.host !== undefined) {
    components.host = func(components.host);
  }
  if (components.path !== undefined) {
    components.path = func(components.path);
  }
  if (components.query !== undefined) {
    components.query = func(components.query);
  }
  if (components.fragment !== undefined) {
    components.fragment = func(components.fragment);
  }
  return components
}

function recomposeAuthority$1 (components) {
  const uriTokens = [];

  if (components.userinfo !== undefined) {
    uriTokens.push(components.userinfo);
    uriTokens.push('@');
  }

  if (components.host !== undefined) {
    let host = unescape(components.host);
    const ipV4res = normalizeIPv4$1(host);

    if (ipV4res.isIPV4) {
      host = ipV4res.host;
    } else {
      const ipV6res = normalizeIPv6$1(ipV4res.host);
      if (ipV6res.isIPV6 === true) {
        host = `[${ipV6res.escapedHost}]`;
      } else {
        host = components.host;
      }
    }
    uriTokens.push(host);
  }

  if (typeof components.port === 'number' || typeof components.port === 'string') {
    uriTokens.push(':');
    uriTokens.push(String(components.port));
  }

  return uriTokens.length ? uriTokens.join('') : undefined
};

var utils = {
  recomposeAuthority: recomposeAuthority$1,
  normalizeComponentEncoding: normalizeComponentEncoding$1,
  removeDotSegments: removeDotSegments$1,
  normalizeIPv4: normalizeIPv4$1,
  normalizeIPv6: normalizeIPv6$1,
  stringArrayToHexStripped
};

'use strict';

const UUID_REG = /^[\da-f]{8}-[\da-f]{4}-[\da-f]{4}-[\da-f]{4}-[\da-f]{12}$/iu;
const URN_REG = /([\da-z][\d\-a-z]{0,31}):((?:[\w!$'()*+,\-.:;=@]|%[\da-f]{2})+)/iu;

function isSecure (wsComponents) {
  return typeof wsComponents.secure === 'boolean' ? wsComponents.secure : String(wsComponents.scheme).toLowerCase() === 'wss'
}

function httpParse (components) {
  if (!components.host) {
    components.error = components.error || 'HTTP URIs must have a host.';
  }

  return components
}

function httpSerialize (components) {
  const secure = String(components.scheme).toLowerCase() === 'https';

  // normalize the default port
  if (components.port === (secure ? 443 : 80) || components.port === '') {
    components.port = undefined;
  }

  // normalize the empty path
  if (!components.path) {
    components.path = '/';
  }

  // NOTE: We do not parse query strings for HTTP URIs
  // as WWW Form Url Encoded query strings are part of the HTML4+ spec,
  // and not the HTTP spec.

  return components
}

function wsParse (wsComponents) {
// indicate if the secure flag is set
  wsComponents.secure = isSecure(wsComponents);

  // construct resouce name
  wsComponents.resourceName = (wsComponents.path || '/') + (wsComponents.query ? '?' + wsComponents.query : '');
  wsComponents.path = undefined;
  wsComponents.query = undefined;

  return wsComponents
}

function wsSerialize (wsComponents) {
// normalize the default port
  if (wsComponents.port === (isSecure(wsComponents) ? 443 : 80) || wsComponents.port === '') {
    wsComponents.port = undefined;
  }

  // ensure scheme matches secure flag
  if (typeof wsComponents.secure === 'boolean') {
    wsComponents.scheme = (wsComponents.secure ? 'wss' : 'ws');
    wsComponents.secure = undefined;
  }

  // reconstruct path from resource name
  if (wsComponents.resourceName) {
    const [path, query] = wsComponents.resourceName.split('?');
    wsComponents.path = (path && path !== '/' ? path : undefined);
    wsComponents.query = query;
    wsComponents.resourceName = undefined;
  }

  // forbid fragment component
  wsComponents.fragment = undefined;

  return wsComponents
}

function urnParse (urnComponents, options) {
  if (!urnComponents.path) {
    urnComponents.error = 'URN can not be parsed';
    return urnComponents
  }
  const matches = urnComponents.path.match(URN_REG);
  if (matches) {
    const scheme = options.scheme || urnComponents.scheme || 'urn';
    urnComponents.nid = matches[1].toLowerCase();
    urnComponents.nss = matches[2];
    const urnScheme = `${scheme}:${options.nid || urnComponents.nid}`;
    const schemeHandler = SCHEMES[urnScheme];
    urnComponents.path = undefined;

    if (schemeHandler) {
      urnComponents = schemeHandler.parse(urnComponents, options);
    }
  } else {
    urnComponents.error = urnComponents.error || 'URN can not be parsed.';
  }

  return urnComponents
}

function urnSerialize (urnComponents, options) {
  const scheme = options.scheme || urnComponents.scheme || 'urn';
  const nid = urnComponents.nid.toLowerCase();
  const urnScheme = `${scheme}:${options.nid || nid}`;
  const schemeHandler = SCHEMES[urnScheme];

  if (schemeHandler) {
    urnComponents = schemeHandler.serialize(urnComponents, options);
  }

  const uriComponents = urnComponents;
  const nss = urnComponents.nss;
  uriComponents.path = `${nid || options.nid}:${nss}`;

  options.skipEscape = true;
  return uriComponents
}

function urnuuidParse (urnComponents, options) {
  const uuidComponents = urnComponents;
  uuidComponents.uuid = uuidComponents.nss;
  uuidComponents.nss = undefined;

  if (!options.tolerant && (!uuidComponents.uuid || !UUID_REG.test(uuidComponents.uuid))) {
    uuidComponents.error = uuidComponents.error || 'UUID is not valid.';
  }

  return uuidComponents
}

function urnuuidSerialize (uuidComponents) {
  const urnComponents = uuidComponents;
  // normalize UUID
  urnComponents.nss = (uuidComponents.uuid || '').toLowerCase();
  return urnComponents
}

const http = {
  scheme: 'http',
  domainHost: true,
  parse: httpParse,
  serialize: httpSerialize
};

const https = {
  scheme: 'https',
  domainHost: http.domainHost,
  parse: httpParse,
  serialize: httpSerialize
};

const ws = {
  scheme: 'ws',
  domainHost: true,
  parse: wsParse,
  serialize: wsSerialize
};

const wss = {
  scheme: 'wss',
  domainHost: ws.domainHost,
  parse: ws.parse,
  serialize: ws.serialize
};

const urn = {
  scheme: 'urn',
  parse: urnParse,
  serialize: urnSerialize,
  skipNormalize: true
};

const urnuuid = {
  scheme: 'urn:uuid',
  parse: urnuuidParse,
  serialize: urnuuidSerialize,
  skipNormalize: true
};

const SCHEMES = {
  http,
  https,
  ws,
  wss,
  urn,
  'urn:uuid': urnuuid
};

var schemes = SCHEMES;

'use strict';

const { normalizeIPv6, normalizeIPv4, removeDotSegments, recomposeAuthority, normalizeComponentEncoding } = utils;


function normalize (uri, options) {
  if (typeof uri === 'string') {
    uri = serialize(parse(uri, options), options);
  } else if (typeof uri === 'object') {
    uri = parse(serialize(uri, options), options);
  }
  return uri
}

function resolve (baseURI, relativeURI, options) {
  const schemelessOptions = Object.assign({ scheme: 'null' }, options);
  const resolved = resolveComponents(parse(baseURI, schemelessOptions), parse(relativeURI, schemelessOptions), schemelessOptions, true);
  return serialize(resolved, { ...schemelessOptions, skipEscape: true })
}

function resolveComponents (base, relative, options, skipNormalization) {
  const target = {};
  if (!skipNormalization) {
    base = parse(serialize(base, options), options); // normalize base components
    relative = parse(serialize(relative, options), options); // normalize relative components
  }
  options = options || {};

  if (!options.tolerant && relative.scheme) {
    target.scheme = relative.scheme;
    // target.authority = relative.authority;
    target.userinfo = relative.userinfo;
    target.host = relative.host;
    target.port = relative.port;
    target.path = removeDotSegments(relative.path || '');
    target.query = relative.query;
  } else {
    if (relative.userinfo !== undefined || relative.host !== undefined || relative.port !== undefined) {
      // target.authority = relative.authority;
      target.userinfo = relative.userinfo;
      target.host = relative.host;
      target.port = relative.port;
      target.path = removeDotSegments(relative.path || '');
      target.query = relative.query;
    } else {
      if (!relative.path) {
        target.path = base.path;
        if (relative.query !== undefined) {
          target.query = relative.query;
        } else {
          target.query = base.query;
        }
      } else {
        if (relative.path.charAt(0) === '/') {
          target.path = removeDotSegments(relative.path);
        } else {
          if ((base.userinfo !== undefined || base.host !== undefined || base.port !== undefined) && !base.path) {
            target.path = '/' + relative.path;
          } else if (!base.path) {
            target.path = relative.path;
          } else {
            target.path = base.path.slice(0, base.path.lastIndexOf('/') + 1) + relative.path;
          }
          target.path = removeDotSegments(target.path);
        }
        target.query = relative.query;
      }
      // target.authority = base.authority;
      target.userinfo = base.userinfo;
      target.host = base.host;
      target.port = base.port;
    }
    target.scheme = base.scheme;
  }

  target.fragment = relative.fragment;

  return target
}

function equal$1 (uriA, uriB, options) {
  if (typeof uriA === 'string') {
    uriA = unescape(uriA);
    uriA = serialize(normalizeComponentEncoding(parse(uriA, options), true), { ...options, skipEscape: true });
  } else if (typeof uriA === 'object') {
    uriA = serialize(normalizeComponentEncoding(uriA, true), { ...options, skipEscape: true });
  }

  if (typeof uriB === 'string') {
    uriB = unescape(uriB);
    uriB = serialize(normalizeComponentEncoding(parse(uriB, options), true), { ...options, skipEscape: true });
  } else if (typeof uriB === 'object') {
    uriB = serialize(normalizeComponentEncoding(uriB, true), { ...options, skipEscape: true });
  }

  return uriA.toLowerCase() === uriB.toLowerCase()
}

function serialize (cmpts, opts) {
  const components = {
    host: cmpts.host,
    scheme: cmpts.scheme,
    userinfo: cmpts.userinfo,
    port: cmpts.port,
    path: cmpts.path,
    query: cmpts.query,
    nid: cmpts.nid,
    nss: cmpts.nss,
    uuid: cmpts.uuid,
    fragment: cmpts.fragment,
    reference: cmpts.reference,
    resourceName: cmpts.resourceName,
    secure: cmpts.secure,
    error: ''
  };
  const options = Object.assign({}, opts);
  const uriTokens = [];

  // find scheme handler
  const schemeHandler = schemes[(options.scheme || components.scheme || '').toLowerCase()];

  // perform scheme specific serialization
  if (schemeHandler && schemeHandler.serialize) schemeHandler.serialize(components, options);

  if (components.path !== undefined) {
    if (!options.skipEscape) {
      components.path = escape(components.path);

      if (components.scheme !== undefined) {
        components.path = components.path.split('%3A').join(':');
      }
    } else {
      components.path = unescape(components.path);
    }
  }

  if (options.reference !== 'suffix' && components.scheme) {
    uriTokens.push(components.scheme, ':');
  }

  const authority = recomposeAuthority(components);
  if (authority !== undefined) {
    if (options.reference !== 'suffix') {
      uriTokens.push('//');
    }

    uriTokens.push(authority);

    if (components.path && components.path.charAt(0) !== '/') {
      uriTokens.push('/');
    }
  }
  if (components.path !== undefined) {
    let s = components.path;

    if (!options.absolutePath && (!schemeHandler || !schemeHandler.absolutePath)) {
      s = removeDotSegments(s);
    }

    if (authority === undefined) {
      s = s.replace(/^\/\//u, '/%2F'); // don't allow the path to start with "//"
    }

    uriTokens.push(s);
  }

  if (components.query !== undefined) {
    uriTokens.push('?', components.query);
  }

  if (components.fragment !== undefined) {
    uriTokens.push('#', components.fragment);
  }
  return uriTokens.join('')
}

const hexLookUp = Array.from({ length: 127 }, (_v, k) => /[^!"$&'()*+,\-.;=_`a-z{}~]/u.test(String.fromCharCode(k)));

function nonSimpleDomain (value) {
  let code = 0;
  for (let i = 0, len = value.length; i < len; ++i) {
    code = value.charCodeAt(i);
    if (code > 126 || hexLookUp[code]) {
      return true
    }
  }
  return false
}

const URI_PARSE = /^(?:([^#/:?]+):)?(?:\/\/((?:([^#/?@]*)@)?(\[[^#/?\]]+\]|[^#/:?]*)(?::(\d*))?))?([^#?]*)(?:\?([^#]*))?(?:#((?:.|[\n\r])*))?/u;

function parse (uri, opts) {
  const options = Object.assign({}, opts);
  const parsed = {
    scheme: undefined,
    userinfo: undefined,
    host: '',
    port: undefined,
    path: '',
    query: undefined,
    fragment: undefined
  };
  const gotEncoding = uri.indexOf('%') !== -1;
  let isIP = false;
  if (options.reference === 'suffix') uri = (options.scheme ? options.scheme + ':' : '') + '//' + uri;

  const matches = uri.match(URI_PARSE);

  if (matches) {
    // store each component
    parsed.scheme = matches[1];
    parsed.userinfo = matches[3];
    parsed.host = matches[4];
    parsed.port = parseInt(matches[5], 10);
    parsed.path = matches[6] || '';
    parsed.query = matches[7];
    parsed.fragment = matches[8];

    // fix port number
    if (isNaN(parsed.port)) {
      parsed.port = matches[5];
    }
    if (parsed.host) {
      const ipv4result = normalizeIPv4(parsed.host);
      if (ipv4result.isIPV4 === false) {
        const ipv6result = normalizeIPv6(ipv4result.host);
        parsed.host = ipv6result.host.toLowerCase();
        isIP = ipv6result.isIPV6;
      } else {
        parsed.host = ipv4result.host;
        isIP = true;
      }
    }
    if (parsed.scheme === undefined && parsed.userinfo === undefined && parsed.host === undefined && parsed.port === undefined && parsed.query === undefined && !parsed.path) {
      parsed.reference = 'same-document';
    } else if (parsed.scheme === undefined) {
      parsed.reference = 'relative';
    } else if (parsed.fragment === undefined) {
      parsed.reference = 'absolute';
    } else {
      parsed.reference = 'uri';
    }

    // check for reference errors
    if (options.reference && options.reference !== 'suffix' && options.reference !== parsed.reference) {
      parsed.error = parsed.error || 'URI is not a ' + options.reference + ' reference.';
    }

    // find scheme handler
    const schemeHandler = schemes[(options.scheme || parsed.scheme || '').toLowerCase()];

    // check if scheme can't handle IRIs
    if (!options.unicodeSupport && (!schemeHandler || !schemeHandler.unicodeSupport)) {
      // if host component is a domain name
      if (parsed.host && (options.domainHost || (schemeHandler && schemeHandler.domainHost)) && isIP === false && nonSimpleDomain(parsed.host)) {
        // convert Unicode IDN -> ASCII IDN
        try {
          parsed.host = URL.domainToASCII(parsed.host.toLowerCase());
        } catch (e) {
          parsed.error = parsed.error || "Host's domain name can not be converted to ASCII: " + e;
        }
      }
      // convert IRI -> URI
    }

    if (!schemeHandler || (schemeHandler && !schemeHandler.skipNormalize)) {
      if (gotEncoding && parsed.scheme !== undefined) {
        parsed.scheme = unescape(parsed.scheme);
      }
      if (gotEncoding && parsed.host !== undefined) {
        parsed.host = unescape(parsed.host);
      }
      if (parsed.path) {
        parsed.path = escape(unescape(parsed.path));
      }
      if (parsed.fragment) {
        parsed.fragment = encodeURI(decodeURIComponent(parsed.fragment));
      }
    }

    // perform scheme specific parsing
    if (schemeHandler && schemeHandler.parse) {
      schemeHandler.parse(parsed, options);
    }
  } else {
    parsed.error = parsed.error || 'URI can not be parsed.';
  }
  return parsed
}

const fastUri = {
  SCHEMES: schemes,
  normalize,
  resolve,
  resolveComponents,
  equal: equal$1,
  serialize,
  parse
};

var fastUri_1 = fastUri;
var _default = fastUri;
var fastUri_2 = fastUri;
fastUri_1.default = _default;
fastUri_1.fastUri = fastUri_2;

var uri_1 = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });

fastUri_1.code = 'require("ajv/dist/runtime/uri").default';
exports.default = fastUri_1;
//# sourceMappingURL=uri.js.map
});

const uri = /*@__PURE__*/getDefaultExportFromCjs(uri_1);

var core = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.CodeGen = exports.Name = exports.nil = exports.stringify = exports.str = exports._ = exports.KeywordCxt = void 0;

Object.defineProperty(exports, "KeywordCxt", { enumerable: true, get: function () { return validate.KeywordCxt; } });

Object.defineProperty(exports, "_", { enumerable: true, get: function () { return codegen._; } });
Object.defineProperty(exports, "str", { enumerable: true, get: function () { return codegen.str; } });
Object.defineProperty(exports, "stringify", { enumerable: true, get: function () { return codegen.stringify; } });
Object.defineProperty(exports, "nil", { enumerable: true, get: function () { return codegen.nil; } });
Object.defineProperty(exports, "Name", { enumerable: true, get: function () { return codegen.Name; } });
Object.defineProperty(exports, "CodeGen", { enumerable: true, get: function () { return codegen.CodeGen; } });




const codegen_2 = codegen;





const defaultRegExp = (str, flags) => new RegExp(str, flags);
defaultRegExp.code = "new RegExp";
const META_IGNORE_OPTIONS = ["removeAdditional", "useDefaults", "coerceTypes"];
const EXT_SCOPE_NAMES = new Set([
    "validate",
    "serialize",
    "parse",
    "wrapper",
    "root",
    "schema",
    "keyword",
    "pattern",
    "formats",
    "validate$data",
    "func",
    "obj",
    "Error",
]);
const removedOptions = {
    errorDataPath: "",
    format: "`validateFormats: false` can be used instead.",
    nullable: '"nullable" keyword is supported by default.',
    jsonPointers: "Deprecated jsPropertySyntax can be used instead.",
    extendRefs: "Deprecated ignoreKeywordsWithRef can be used instead.",
    missingRefs: "Pass empty schema with $id that should be ignored to ajv.addSchema.",
    processCode: "Use option `code: {process: (code, schemaEnv: object) => string}`",
    sourceCode: "Use option `code: {source: true}`",
    strictDefaults: "It is default now, see option `strict`.",
    strictKeywords: "It is default now, see option `strict`.",
    uniqueItems: '"uniqueItems" keyword is always validated.',
    unknownFormats: "Disable strict mode or pass `true` to `ajv.addFormat` (or `formats` option).",
    cache: "Map is used as cache, schema object as key.",
    serialize: "Map is used as cache, schema object as key.",
    ajvErrors: "It is default now.",
};
const deprecatedOptions = {
    ignoreKeywordsWithRef: "",
    jsPropertySyntax: "",
    unicode: '"minLength"/"maxLength" account for unicode characters by default.',
};
const MAX_EXPRESSION = 200;
// eslint-disable-next-line complexity
function requiredOptions(o) {
    var _a, _b, _c, _d, _e, _f, _g, _h, _j, _k, _l, _m, _o, _p, _q, _r, _s, _t, _u, _v, _w, _x, _y, _z, _0;
    const s = o.strict;
    const _optz = (_a = o.code) === null || _a === void 0 ? void 0 : _a.optimize;
    const optimize = _optz === true || _optz === undefined ? 1 : _optz || 0;
    const regExp = (_c = (_b = o.code) === null || _b === void 0 ? void 0 : _b.regExp) !== null && _c !== void 0 ? _c : defaultRegExp;
    const uriResolver = (_d = o.uriResolver) !== null && _d !== void 0 ? _d : uri_1.default;
    return {
        strictSchema: (_f = (_e = o.strictSchema) !== null && _e !== void 0 ? _e : s) !== null && _f !== void 0 ? _f : true,
        strictNumbers: (_h = (_g = o.strictNumbers) !== null && _g !== void 0 ? _g : s) !== null && _h !== void 0 ? _h : true,
        strictTypes: (_k = (_j = o.strictTypes) !== null && _j !== void 0 ? _j : s) !== null && _k !== void 0 ? _k : "log",
        strictTuples: (_m = (_l = o.strictTuples) !== null && _l !== void 0 ? _l : s) !== null && _m !== void 0 ? _m : "log",
        strictRequired: (_p = (_o = o.strictRequired) !== null && _o !== void 0 ? _o : s) !== null && _p !== void 0 ? _p : false,
        code: o.code ? { ...o.code, optimize, regExp } : { optimize, regExp },
        loopRequired: (_q = o.loopRequired) !== null && _q !== void 0 ? _q : MAX_EXPRESSION,
        loopEnum: (_r = o.loopEnum) !== null && _r !== void 0 ? _r : MAX_EXPRESSION,
        meta: (_s = o.meta) !== null && _s !== void 0 ? _s : true,
        messages: (_t = o.messages) !== null && _t !== void 0 ? _t : true,
        inlineRefs: (_u = o.inlineRefs) !== null && _u !== void 0 ? _u : true,
        schemaId: (_v = o.schemaId) !== null && _v !== void 0 ? _v : "$id",
        addUsedSchema: (_w = o.addUsedSchema) !== null && _w !== void 0 ? _w : true,
        validateSchema: (_x = o.validateSchema) !== null && _x !== void 0 ? _x : true,
        validateFormats: (_y = o.validateFormats) !== null && _y !== void 0 ? _y : true,
        unicodeRegExp: (_z = o.unicodeRegExp) !== null && _z !== void 0 ? _z : true,
        int32range: (_0 = o.int32range) !== null && _0 !== void 0 ? _0 : true,
        uriResolver: uriResolver,
    };
}
class Ajv {
    constructor(opts = {}) {
        this.schemas = {};
        this.refs = {};
        this.formats = {};
        this._compilations = new Set();
        this._loading = {};
        this._cache = new Map();
        opts = this.opts = { ...opts, ...requiredOptions(opts) };
        const { es5, lines } = this.opts.code;
        this.scope = new codegen_2.ValueScope({ scope: {}, prefixes: EXT_SCOPE_NAMES, es5, lines });
        this.logger = getLogger(opts.logger);
        const formatOpt = opts.validateFormats;
        opts.validateFormats = false;
        this.RULES = (0, rules.getRules)();
        checkOptions.call(this, removedOptions, opts, "NOT SUPPORTED");
        checkOptions.call(this, deprecatedOptions, opts, "DEPRECATED", "warn");
        this._metaOpts = getMetaSchemaOptions.call(this);
        if (opts.formats)
            addInitialFormats.call(this);
        this._addVocabularies();
        this._addDefaultMetaSchema();
        if (opts.keywords)
            addInitialKeywords.call(this, opts.keywords);
        if (typeof opts.meta == "object")
            this.addMetaSchema(opts.meta);
        addInitialSchemas.call(this);
        opts.validateFormats = formatOpt;
    }
    _addVocabularies() {
        this.addKeyword("$async");
    }
    _addDefaultMetaSchema() {
        const { $data, meta, schemaId } = this.opts;
        let _dataRefSchema = $dataRefSchema;
        if (schemaId === "id") {
            _dataRefSchema = { ...$dataRefSchema };
            _dataRefSchema.id = _dataRefSchema.$id;
            delete _dataRefSchema.$id;
        }
        if (meta && $data)
            this.addMetaSchema(_dataRefSchema, _dataRefSchema[schemaId], false);
    }
    defaultMeta() {
        const { meta, schemaId } = this.opts;
        return (this.opts.defaultMeta = typeof meta == "object" ? meta[schemaId] || meta : undefined);
    }
    validate(schemaKeyRef, // key, ref or schema object
    // eslint-disable-next-line @typescript-eslint/no-redundant-type-constituents
    data // to be validated
    ) {
        let v;
        if (typeof schemaKeyRef == "string") {
            v = this.getSchema(schemaKeyRef);
            if (!v)
                throw new Error(`no schema with key or ref "${schemaKeyRef}"`);
        }
        else {
            v = this.compile(schemaKeyRef);
        }
        const valid = v(data);
        if (!("$async" in v))
            this.errors = v.errors;
        return valid;
    }
    compile(schema, _meta) {
        const sch = this._addSchema(schema, _meta);
        return (sch.validate || this._compileSchemaEnv(sch));
    }
    compileAsync(schema, meta) {
        if (typeof this.opts.loadSchema != "function") {
            throw new Error("options.loadSchema should be a function");
        }
        const { loadSchema } = this.opts;
        return runCompileAsync.call(this, schema, meta);
        async function runCompileAsync(_schema, _meta) {
            await loadMetaSchema.call(this, _schema.$schema);
            const sch = this._addSchema(_schema, _meta);
            return sch.validate || _compileAsync.call(this, sch);
        }
        async function loadMetaSchema($ref) {
            if ($ref && !this.getSchema($ref)) {
                await runCompileAsync.call(this, { $ref }, true);
            }
        }
        async function _compileAsync(sch) {
            try {
                return this._compileSchemaEnv(sch);
            }
            catch (e) {
                if (!(e instanceof ref_error.default))
                    throw e;
                checkLoaded.call(this, e);
                await loadMissingSchema.call(this, e.missingSchema);
                return _compileAsync.call(this, sch);
            }
        }
        function checkLoaded({ missingSchema: ref, missingRef }) {
            if (this.refs[ref]) {
                throw new Error(`AnySchema ${ref} is loaded but ${missingRef} cannot be resolved`);
            }
        }
        async function loadMissingSchema(ref) {
            const _schema = await _loadSchema.call(this, ref);
            if (!this.refs[ref])
                await loadMetaSchema.call(this, _schema.$schema);
            if (!this.refs[ref])
                this.addSchema(_schema, ref, meta);
        }
        async function _loadSchema(ref) {
            const p = this._loading[ref];
            if (p)
                return p;
            try {
                return await (this._loading[ref] = loadSchema(ref));
            }
            finally {
                delete this._loading[ref];
            }
        }
    }
    // Adds schema to the instance
    addSchema(schema, // If array is passed, `key` will be ignored
    key, // Optional schema key. Can be passed to `validate` method instead of schema object or id/ref. One schema per instance can have empty `id` and `key`.
    _meta, // true if schema is a meta-schema. Used internally, addMetaSchema should be used instead.
    _validateSchema = this.opts.validateSchema // false to skip schema validation. Used internally, option validateSchema should be used instead.
    ) {
        if (Array.isArray(schema)) {
            for (const sch of schema)
                this.addSchema(sch, undefined, _meta, _validateSchema);
            return this;
        }
        let id;
        if (typeof schema === "object") {
            const { schemaId } = this.opts;
            id = schema[schemaId];
            if (id !== undefined && typeof id != "string") {
                throw new Error(`schema ${schemaId} must be string`);
            }
        }
        key = (0, resolve$1.normalizeId)(key || id);
        this._checkUnique(key);
        this.schemas[key] = this._addSchema(schema, _meta, key, _validateSchema, true);
        return this;
    }
    // Add schema that will be used to validate other schemas
    // options in META_IGNORE_OPTIONS are alway set to false
    addMetaSchema(schema, key, // schema key
    _validateSchema = this.opts.validateSchema // false to skip schema validation, can be used to override validateSchema option for meta-schema
    ) {
        this.addSchema(schema, key, true, _validateSchema);
        return this;
    }
    //  Validate schema against its meta-schema
    validateSchema(schema, throwOrLogError) {
        if (typeof schema == "boolean")
            return true;
        let $schema;
        $schema = schema.$schema;
        if ($schema !== undefined && typeof $schema != "string") {
            throw new Error("$schema must be a string");
        }
        $schema = $schema || this.opts.defaultMeta || this.defaultMeta();
        if (!$schema) {
            this.logger.warn("meta-schema not available");
            this.errors = null;
            return true;
        }
        const valid = this.validate($schema, schema);
        if (!valid && throwOrLogError) {
            const message = "schema is invalid: " + this.errorsText();
            if (this.opts.validateSchema === "log")
                this.logger.error(message);
            else
                throw new Error(message);
        }
        return valid;
    }
    // Get compiled schema by `key` or `ref`.
    // (`key` that was passed to `addSchema` or full schema reference - `schema.$id` or resolved id)
    getSchema(keyRef) {
        let sch;
        while (typeof (sch = getSchEnv.call(this, keyRef)) == "string")
            keyRef = sch;
        if (sch === undefined) {
            const { schemaId } = this.opts;
            const root = new compile.SchemaEnv({ schema: {}, schemaId });
            sch = compile.resolveSchema.call(this, root, keyRef);
            if (!sch)
                return;
            this.refs[keyRef] = sch;
        }
        return (sch.validate || this._compileSchemaEnv(sch));
    }
    // Remove cached schema(s).
    // If no parameter is passed all schemas but meta-schemas are removed.
    // If RegExp is passed all schemas with key/id matching pattern but meta-schemas are removed.
    // Even if schema is referenced by other schemas it still can be removed as other schemas have local references.
    removeSchema(schemaKeyRef) {
        if (schemaKeyRef instanceof RegExp) {
            this._removeAllSchemas(this.schemas, schemaKeyRef);
            this._removeAllSchemas(this.refs, schemaKeyRef);
            return this;
        }
        switch (typeof schemaKeyRef) {
            case "undefined":
                this._removeAllSchemas(this.schemas);
                this._removeAllSchemas(this.refs);
                this._cache.clear();
                return this;
            case "string": {
                const sch = getSchEnv.call(this, schemaKeyRef);
                if (typeof sch == "object")
                    this._cache.delete(sch.schema);
                delete this.schemas[schemaKeyRef];
                delete this.refs[schemaKeyRef];
                return this;
            }
            case "object": {
                const cacheKey = schemaKeyRef;
                this._cache.delete(cacheKey);
                let id = schemaKeyRef[this.opts.schemaId];
                if (id) {
                    id = (0, resolve$1.normalizeId)(id);
                    delete this.schemas[id];
                    delete this.refs[id];
                }
                return this;
            }
            default:
                throw new Error("ajv.removeSchema: invalid parameter");
        }
    }
    // add "vocabulary" - a collection of keywords
    addVocabulary(definitions) {
        for (const def of definitions)
            this.addKeyword(def);
        return this;
    }
    addKeyword(kwdOrDef, def // deprecated
    ) {
        let keyword;
        if (typeof kwdOrDef == "string") {
            keyword = kwdOrDef;
            if (typeof def == "object") {
                this.logger.warn("these parameters are deprecated, see docs for addKeyword");
                def.keyword = keyword;
            }
        }
        else if (typeof kwdOrDef == "object" && def === undefined) {
            def = kwdOrDef;
            keyword = def.keyword;
            if (Array.isArray(keyword) && !keyword.length) {
                throw new Error("addKeywords: keyword must be string or non-empty array");
            }
        }
        else {
            throw new Error("invalid addKeywords parameters");
        }
        checkKeyword.call(this, keyword, def);
        if (!def) {
            (0, util.eachItem)(keyword, (kwd) => addRule.call(this, kwd));
            return this;
        }
        keywordMetaschema.call(this, def);
        const definition = {
            ...def,
            type: (0, dataType.getJSONTypes)(def.type),
            schemaType: (0, dataType.getJSONTypes)(def.schemaType),
        };
        (0, util.eachItem)(keyword, definition.type.length === 0
            ? (k) => addRule.call(this, k, definition)
            : (k) => definition.type.forEach((t) => addRule.call(this, k, definition, t)));
        return this;
    }
    getKeyword(keyword) {
        const rule = this.RULES.all[keyword];
        return typeof rule == "object" ? rule.definition : !!rule;
    }
    // Remove keyword
    removeKeyword(keyword) {
        // TODO return type should be Ajv
        const { RULES } = this;
        delete RULES.keywords[keyword];
        delete RULES.all[keyword];
        for (const group of RULES.rules) {
            const i = group.rules.findIndex((rule) => rule.keyword === keyword);
            if (i >= 0)
                group.rules.splice(i, 1);
        }
        return this;
    }
    // Add format
    addFormat(name, format) {
        if (typeof format == "string")
            format = new RegExp(format);
        this.formats[name] = format;
        return this;
    }
    errorsText(errors = this.errors, // optional array of validation errors
    { separator = ", ", dataVar = "data" } = {} // optional options with properties `separator` and `dataVar`
    ) {
        if (!errors || errors.length === 0)
            return "No errors";
        return errors
            .map((e) => `${dataVar}${e.instancePath} ${e.message}`)
            .reduce((text, msg) => text + separator + msg);
    }
    $dataMetaSchema(metaSchema, keywordsJsonPointers) {
        const rules = this.RULES.all;
        metaSchema = JSON.parse(JSON.stringify(metaSchema));
        for (const jsonPointer of keywordsJsonPointers) {
            const segments = jsonPointer.split("/").slice(1); // first segment is an empty string
            let keywords = metaSchema;
            for (const seg of segments)
                keywords = keywords[seg];
            for (const key in rules) {
                const rule = rules[key];
                if (typeof rule != "object")
                    continue;
                const { $data } = rule.definition;
                const schema = keywords[key];
                if ($data && schema)
                    keywords[key] = schemaOrData(schema);
            }
        }
        return metaSchema;
    }
    _removeAllSchemas(schemas, regex) {
        for (const keyRef in schemas) {
            const sch = schemas[keyRef];
            if (!regex || regex.test(keyRef)) {
                if (typeof sch == "string") {
                    delete schemas[keyRef];
                }
                else if (sch && !sch.meta) {
                    this._cache.delete(sch.schema);
                    delete schemas[keyRef];
                }
            }
        }
    }
    _addSchema(schema, meta, baseId, validateSchema = this.opts.validateSchema, addSchema = this.opts.addUsedSchema) {
        let id;
        const { schemaId } = this.opts;
        if (typeof schema == "object") {
            id = schema[schemaId];
        }
        else {
            if (this.opts.jtd)
                throw new Error("schema must be object");
            else if (typeof schema != "boolean")
                throw new Error("schema must be object or boolean");
        }
        let sch = this._cache.get(schema);
        if (sch !== undefined)
            return sch;
        baseId = (0, resolve$1.normalizeId)(id || baseId);
        const localRefs = resolve$1.getSchemaRefs.call(this, schema, baseId);
        sch = new compile.SchemaEnv({ schema, schemaId, meta, baseId, localRefs });
        this._cache.set(sch.schema, sch);
        if (addSchema && !baseId.startsWith("#")) {
            // TODO atm it is allowed to overwrite schemas without id (instead of not adding them)
            if (baseId)
                this._checkUnique(baseId);
            this.refs[baseId] = sch;
        }
        if (validateSchema)
            this.validateSchema(schema, true);
        return sch;
    }
    _checkUnique(id) {
        if (this.schemas[id] || this.refs[id]) {
            throw new Error(`schema with key or id "${id}" already exists`);
        }
    }
    _compileSchemaEnv(sch) {
        if (sch.meta)
            this._compileMetaSchema(sch);
        else
            compile.compileSchema.call(this, sch);
        /* istanbul ignore if */
        if (!sch.validate)
            throw new Error("ajv implementation error");
        return sch.validate;
    }
    _compileMetaSchema(sch) {
        const currentOpts = this.opts;
        this.opts = this._metaOpts;
        try {
            compile.compileSchema.call(this, sch);
        }
        finally {
            this.opts = currentOpts;
        }
    }
}
Ajv.ValidationError = validation_error.default;
Ajv.MissingRefError = ref_error.default;
exports.default = Ajv;
function checkOptions(checkOpts, options, msg, log = "error") {
    for (const key in checkOpts) {
        const opt = key;
        if (opt in options)
            this.logger[log](`${msg}: option ${key}. ${checkOpts[opt]}`);
    }
}
function getSchEnv(keyRef) {
    keyRef = (0, resolve$1.normalizeId)(keyRef); // TODO tests fail without this line
    return this.schemas[keyRef] || this.refs[keyRef];
}
function addInitialSchemas() {
    const optsSchemas = this.opts.schemas;
    if (!optsSchemas)
        return;
    if (Array.isArray(optsSchemas))
        this.addSchema(optsSchemas);
    else
        for (const key in optsSchemas)
            this.addSchema(optsSchemas[key], key);
}
function addInitialFormats() {
    for (const name in this.opts.formats) {
        const format = this.opts.formats[name];
        if (format)
            this.addFormat(name, format);
    }
}
function addInitialKeywords(defs) {
    if (Array.isArray(defs)) {
        this.addVocabulary(defs);
        return;
    }
    this.logger.warn("keywords option as map is deprecated, pass array");
    for (const keyword in defs) {
        const def = defs[keyword];
        if (!def.keyword)
            def.keyword = keyword;
        this.addKeyword(def);
    }
}
function getMetaSchemaOptions() {
    const metaOpts = { ...this.opts };
    for (const opt of META_IGNORE_OPTIONS)
        delete metaOpts[opt];
    return metaOpts;
}
const noLogs = { log() { }, warn() { }, error() { } };
function getLogger(logger) {
    if (logger === false)
        return noLogs;
    if (logger === undefined)
        return console;
    if (logger.log && logger.warn && logger.error)
        return logger;
    throw new Error("logger must implement log, warn and error methods");
}
const KEYWORD_NAME = /^[a-z_$][a-z0-9_$:-]*$/i;
function checkKeyword(keyword, def) {
    const { RULES } = this;
    (0, util.eachItem)(keyword, (kwd) => {
        if (RULES.keywords[kwd])
            throw new Error(`Keyword ${kwd} is already defined`);
        if (!KEYWORD_NAME.test(kwd))
            throw new Error(`Keyword ${kwd} has invalid name`);
    });
    if (!def)
        return;
    if (def.$data && !("code" in def || "validate" in def)) {
        throw new Error('$data keyword must have "code" or "validate" function');
    }
}
function addRule(keyword, definition, dataType$1) {
    var _a;
    const post = definition === null || definition === void 0 ? void 0 : definition.post;
    if (dataType$1 && post)
        throw new Error('keyword with "post" flag cannot have "type"');
    const { RULES } = this;
    let ruleGroup = post ? RULES.post : RULES.rules.find(({ type: t }) => t === dataType$1);
    if (!ruleGroup) {
        ruleGroup = { type: dataType$1, rules: [] };
        RULES.rules.push(ruleGroup);
    }
    RULES.keywords[keyword] = true;
    if (!definition)
        return;
    const rule = {
        keyword,
        definition: {
            ...definition,
            type: (0, dataType.getJSONTypes)(definition.type),
            schemaType: (0, dataType.getJSONTypes)(definition.schemaType),
        },
    };
    if (definition.before)
        addBeforeRule.call(this, ruleGroup, rule, definition.before);
    else
        ruleGroup.rules.push(rule);
    RULES.all[keyword] = rule;
    (_a = definition.implements) === null || _a === void 0 ? void 0 : _a.forEach((kwd) => this.addKeyword(kwd));
}
function addBeforeRule(ruleGroup, rule, before) {
    const i = ruleGroup.rules.findIndex((_rule) => _rule.keyword === before);
    if (i >= 0) {
        ruleGroup.rules.splice(i, 0, rule);
    }
    else {
        ruleGroup.rules.push(rule);
        this.logger.warn(`rule ${before} is not defined`);
    }
}
function keywordMetaschema(def) {
    let { metaSchema } = def;
    if (metaSchema === undefined)
        return;
    if (def.$data && this.opts.$data)
        metaSchema = schemaOrData(metaSchema);
    def.validateSchema = this.compile(metaSchema, true);
}
const $dataRef = {
    $ref: "https://raw.githubusercontent.com/ajv-validator/ajv/master/lib/refs/data.json#",
};
function schemaOrData(schema) {
    return { anyOf: [schema, $dataRef] };
}
//# sourceMappingURL=core.js.map
});

const core$1 = /*@__PURE__*/getDefaultExportFromCjs(core);

var id = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const def = {
    keyword: "id",
    code() {
        throw new Error('NOT SUPPORTED: keyword "id", use "$id" for schema ID');
    },
};
exports.default = def;
//# sourceMappingURL=id.js.map
});

const id$1 = /*@__PURE__*/getDefaultExportFromCjs(id);

var ref = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.callRef = exports.getValidate = void 0;






const def = {
    keyword: "$ref",
    schemaType: "string",
    code(cxt) {
        const { gen, schema: $ref, it } = cxt;
        const { baseId, schemaEnv: env, validateName, opts, self } = it;
        const { root } = env;
        if (($ref === "#" || $ref === "#/") && baseId === root.baseId)
            return callRootRef();
        const schOrEnv = compile.resolveRef.call(self, root, baseId, $ref);
        if (schOrEnv === undefined)
            throw new ref_error.default(it.opts.uriResolver, baseId, $ref);
        if (schOrEnv instanceof compile.SchemaEnv)
            return callValidate(schOrEnv);
        return inlineRefSchema(schOrEnv);
        function callRootRef() {
            if (env === root)
                return callRef(cxt, validateName, env, env.$async);
            const rootName = gen.scopeValue("root", { ref: root });
            return callRef(cxt, (0, codegen._) `${rootName}.validate`, root, root.$async);
        }
        function callValidate(sch) {
            const v = getValidate(cxt, sch);
            callRef(cxt, v, sch, sch.$async);
        }
        function inlineRefSchema(sch) {
            const schName = gen.scopeValue("schema", opts.code.source === true ? { ref: sch, code: (0, codegen.stringify)(sch) } : { ref: sch });
            const valid = gen.name("valid");
            const schCxt = cxt.subschema({
                schema: sch,
                dataTypes: [],
                schemaPath: codegen.nil,
                topSchemaRef: schName,
                errSchemaPath: $ref,
            }, valid);
            cxt.mergeEvaluated(schCxt);
            cxt.ok(valid);
        }
    },
};
function getValidate(cxt, sch) {
    const { gen } = cxt;
    return sch.validate
        ? gen.scopeValue("validate", { ref: sch.validate })
        : (0, codegen._) `${gen.scopeValue("wrapper", { ref: sch })}.validate`;
}
exports.getValidate = getValidate;
function callRef(cxt, v, sch, $async) {
    const { gen, it } = cxt;
    const { allErrors, schemaEnv: env, opts } = it;
    const passCxt = opts.passContext ? names_1.default.this : codegen.nil;
    if ($async)
        callAsyncRef();
    else
        callSyncRef();
    function callAsyncRef() {
        if (!env.$async)
            throw new Error("async schema referenced by sync schema");
        const valid = gen.let("valid");
        gen.try(() => {
            gen.code((0, codegen._) `await ${(0, code.callValidateCode)(cxt, v, passCxt)}`);
            addEvaluatedFrom(v); // TODO will not work with async, it has to be returned with the result
            if (!allErrors)
                gen.assign(valid, true);
        }, (e) => {
            gen.if((0, codegen._) `!(${e} instanceof ${it.ValidationError})`, () => gen.throw(e));
            addErrorsFrom(e);
            if (!allErrors)
                gen.assign(valid, false);
        });
        cxt.ok(valid);
    }
    function callSyncRef() {
        cxt.result((0, code.callValidateCode)(cxt, v, passCxt), () => addEvaluatedFrom(v), () => addErrorsFrom(v));
    }
    function addErrorsFrom(source) {
        const errs = (0, codegen._) `${source}.errors`;
        gen.assign(names_1.default.vErrors, (0, codegen._) `${names_1.default.vErrors} === null ? ${errs} : ${names_1.default.vErrors}.concat(${errs})`); // TODO tagged
        gen.assign(names_1.default.errors, (0, codegen._) `${names_1.default.vErrors}.length`);
    }
    function addEvaluatedFrom(source) {
        var _a;
        if (!it.opts.unevaluated)
            return;
        const schEvaluated = (_a = sch === null || sch === void 0 ? void 0 : sch.validate) === null || _a === void 0 ? void 0 : _a.evaluated;
        // TODO refactor
        if (it.props !== true) {
            if (schEvaluated && !schEvaluated.dynamicProps) {
                if (schEvaluated.props !== undefined) {
                    it.props = util.mergeEvaluated.props(gen, schEvaluated.props, it.props);
                }
            }
            else {
                const props = gen.var("props", (0, codegen._) `${source}.evaluated.props`);
                it.props = util.mergeEvaluated.props(gen, props, it.props, codegen.Name);
            }
        }
        if (it.items !== true) {
            if (schEvaluated && !schEvaluated.dynamicItems) {
                if (schEvaluated.items !== undefined) {
                    it.items = util.mergeEvaluated.items(gen, schEvaluated.items, it.items);
                }
            }
            else {
                const items = gen.var("items", (0, codegen._) `${source}.evaluated.items`);
                it.items = util.mergeEvaluated.items(gen, items, it.items, codegen.Name);
            }
        }
    }
}
exports.callRef = callRef;
exports.default = def;
//# sourceMappingURL=ref.js.map
});

const ref$1 = /*@__PURE__*/getDefaultExportFromCjs(ref);

var core_1 = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });


const core = [
    "$schema",
    "$id",
    "$defs",
    "$vocabulary",
    { keyword: "$comment" },
    "definitions",
    id.default,
    ref.default,
];
exports.default = core;
//# sourceMappingURL=index.js.map
});

const index$5 = /*@__PURE__*/getDefaultExportFromCjs(core_1);

var limitNumber = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });

const ops = codegen.operators;
const KWDs = {
    maximum: { okStr: "<=", ok: ops.LTE, fail: ops.GT },
    minimum: { okStr: ">=", ok: ops.GTE, fail: ops.LT },
    exclusiveMaximum: { okStr: "<", ok: ops.LT, fail: ops.GTE },
    exclusiveMinimum: { okStr: ">", ok: ops.GT, fail: ops.LTE },
};
const error = {
    message: ({ keyword, schemaCode }) => (0, codegen.str) `must be ${KWDs[keyword].okStr} ${schemaCode}`,
    params: ({ keyword, schemaCode }) => (0, codegen._) `{comparison: ${KWDs[keyword].okStr}, limit: ${schemaCode}}`,
};
const def = {
    keyword: Object.keys(KWDs),
    type: "number",
    schemaType: "number",
    $data: true,
    error,
    code(cxt) {
        const { keyword, data, schemaCode } = cxt;
        cxt.fail$data((0, codegen._) `${data} ${KWDs[keyword].fail} ${schemaCode} || isNaN(${data})`);
    },
};
exports.default = def;
//# sourceMappingURL=limitNumber.js.map
});

const limitNumber$1 = /*@__PURE__*/getDefaultExportFromCjs(limitNumber);

var multipleOf = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });

const error = {
    message: ({ schemaCode }) => (0, codegen.str) `must be multiple of ${schemaCode}`,
    params: ({ schemaCode }) => (0, codegen._) `{multipleOf: ${schemaCode}}`,
};
const def = {
    keyword: "multipleOf",
    type: "number",
    schemaType: "number",
    $data: true,
    error,
    code(cxt) {
        const { gen, data, schemaCode, it } = cxt;
        // const bdt = bad$DataType(schemaCode, <string>def.schemaType, $data)
        const prec = it.opts.multipleOfPrecision;
        const res = gen.let("res");
        const invalid = prec
            ? (0, codegen._) `Math.abs(Math.round(${res}) - ${res}) > 1e-${prec}`
            : (0, codegen._) `${res} !== parseInt(${res})`;
        cxt.fail$data((0, codegen._) `(${schemaCode} === 0 || (${res} = ${data}/${schemaCode}, ${invalid}))`);
    },
};
exports.default = def;
//# sourceMappingURL=multipleOf.js.map
});

const multipleOf$1 = /*@__PURE__*/getDefaultExportFromCjs(multipleOf);

var ucs2length_1 = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
// https://mathiasbynens.be/notes/javascript-encoding
// https://github.com/bestiejs/punycode.js - punycode.ucs2.decode
function ucs2length(str) {
    const len = str.length;
    let length = 0;
    let pos = 0;
    let value;
    while (pos < len) {
        length++;
        value = str.charCodeAt(pos++);
        if (value >= 0xd800 && value <= 0xdbff && pos < len) {
            // high surrogate, and there is a next character
            value = str.charCodeAt(pos);
            if ((value & 0xfc00) === 0xdc00)
                pos++; // low surrogate
        }
    }
    return length;
}
exports.default = ucs2length;
ucs2length.code = 'require("ajv/dist/runtime/ucs2length").default';
//# sourceMappingURL=ucs2length.js.map
});

const ucs2length = /*@__PURE__*/getDefaultExportFromCjs(ucs2length_1);

var limitLength = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });



const error = {
    message({ keyword, schemaCode }) {
        const comp = keyword === "maxLength" ? "more" : "fewer";
        return (0, codegen.str) `must NOT have ${comp} than ${schemaCode} characters`;
    },
    params: ({ schemaCode }) => (0, codegen._) `{limit: ${schemaCode}}`,
};
const def = {
    keyword: ["maxLength", "minLength"],
    type: "string",
    schemaType: "number",
    $data: true,
    error,
    code(cxt) {
        const { keyword, data, schemaCode, it } = cxt;
        const op = keyword === "maxLength" ? codegen.operators.GT : codegen.operators.LT;
        const len = it.opts.unicode === false ? (0, codegen._) `${data}.length` : (0, codegen._) `${(0, util.useFunc)(cxt.gen, ucs2length_1.default)}(${data})`;
        cxt.fail$data((0, codegen._) `${len} ${op} ${schemaCode}`);
    },
};
exports.default = def;
//# sourceMappingURL=limitLength.js.map
});

const limitLength$1 = /*@__PURE__*/getDefaultExportFromCjs(limitLength);

var pattern = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });


const error = {
    message: ({ schemaCode }) => (0, codegen.str) `must match pattern "${schemaCode}"`,
    params: ({ schemaCode }) => (0, codegen._) `{pattern: ${schemaCode}}`,
};
const def = {
    keyword: "pattern",
    type: "string",
    schemaType: "string",
    $data: true,
    error,
    code(cxt) {
        const { data, $data, schema, schemaCode, it } = cxt;
        // TODO regexp should be wrapped in try/catchs
        const u = it.opts.unicodeRegExp ? "u" : "";
        const regExp = $data ? (0, codegen._) `(new RegExp(${schemaCode}, ${u}))` : (0, code.usePattern)(cxt, schema);
        cxt.fail$data((0, codegen._) `!${regExp}.test(${data})`);
    },
};
exports.default = def;
//# sourceMappingURL=pattern.js.map
});

const pattern$1 = /*@__PURE__*/getDefaultExportFromCjs(pattern);

var limitProperties = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });

const error = {
    message({ keyword, schemaCode }) {
        const comp = keyword === "maxProperties" ? "more" : "fewer";
        return (0, codegen.str) `must NOT have ${comp} than ${schemaCode} properties`;
    },
    params: ({ schemaCode }) => (0, codegen._) `{limit: ${schemaCode}}`,
};
const def = {
    keyword: ["maxProperties", "minProperties"],
    type: "object",
    schemaType: "number",
    $data: true,
    error,
    code(cxt) {
        const { keyword, data, schemaCode } = cxt;
        const op = keyword === "maxProperties" ? codegen.operators.GT : codegen.operators.LT;
        cxt.fail$data((0, codegen._) `Object.keys(${data}).length ${op} ${schemaCode}`);
    },
};
exports.default = def;
//# sourceMappingURL=limitProperties.js.map
});

const limitProperties$1 = /*@__PURE__*/getDefaultExportFromCjs(limitProperties);

var required = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });



const error = {
    message: ({ params: { missingProperty } }) => (0, codegen.str) `must have required property '${missingProperty}'`,
    params: ({ params: { missingProperty } }) => (0, codegen._) `{missingProperty: ${missingProperty}}`,
};
const def = {
    keyword: "required",
    type: "object",
    schemaType: "array",
    $data: true,
    error,
    code(cxt) {
        const { gen, schema, schemaCode, data, $data, it } = cxt;
        const { opts } = it;
        if (!$data && schema.length === 0)
            return;
        const useLoop = schema.length >= opts.loopRequired;
        if (it.allErrors)
            allErrorsMode();
        else
            exitOnErrorMode();
        if (opts.strictRequired) {
            const props = cxt.parentSchema.properties;
            const { definedProperties } = cxt.it;
            for (const requiredKey of schema) {
                if ((props === null || props === void 0 ? void 0 : props[requiredKey]) === undefined && !definedProperties.has(requiredKey)) {
                    const schemaPath = it.schemaEnv.baseId + it.errSchemaPath;
                    const msg = `required property "${requiredKey}" is not defined at "${schemaPath}" (strictRequired)`;
                    (0, util.checkStrictMode)(it, msg, it.opts.strictRequired);
                }
            }
        }
        function allErrorsMode() {
            if (useLoop || $data) {
                cxt.block$data(codegen.nil, loopAllRequired);
            }
            else {
                for (const prop of schema) {
                    (0, code.checkReportMissingProp)(cxt, prop);
                }
            }
        }
        function exitOnErrorMode() {
            const missing = gen.let("missing");
            if (useLoop || $data) {
                const valid = gen.let("valid", true);
                cxt.block$data(valid, () => loopUntilMissing(missing, valid));
                cxt.ok(valid);
            }
            else {
                gen.if((0, code.checkMissingProp)(cxt, schema, missing));
                (0, code.reportMissingProp)(cxt, missing);
                gen.else();
            }
        }
        function loopAllRequired() {
            gen.forOf("prop", schemaCode, (prop) => {
                cxt.setParams({ missingProperty: prop });
                gen.if((0, code.noPropertyInData)(gen, data, prop, opts.ownProperties), () => cxt.error());
            });
        }
        function loopUntilMissing(missing, valid) {
            cxt.setParams({ missingProperty: missing });
            gen.forOf(missing, schemaCode, () => {
                gen.assign(valid, (0, code.propertyInData)(gen, data, missing, opts.ownProperties));
                gen.if((0, codegen.not)(valid), () => {
                    cxt.error();
                    gen.break();
                });
            }, codegen.nil);
        }
    },
};
exports.default = def;
//# sourceMappingURL=required.js.map
});

const required$1 = /*@__PURE__*/getDefaultExportFromCjs(required);

var limitItems = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });

const error = {
    message({ keyword, schemaCode }) {
        const comp = keyword === "maxItems" ? "more" : "fewer";
        return (0, codegen.str) `must NOT have ${comp} than ${schemaCode} items`;
    },
    params: ({ schemaCode }) => (0, codegen._) `{limit: ${schemaCode}}`,
};
const def = {
    keyword: ["maxItems", "minItems"],
    type: "array",
    schemaType: "number",
    $data: true,
    error,
    code(cxt) {
        const { keyword, data, schemaCode } = cxt;
        const op = keyword === "maxItems" ? codegen.operators.GT : codegen.operators.LT;
        cxt.fail$data((0, codegen._) `${data}.length ${op} ${schemaCode}`);
    },
};
exports.default = def;
//# sourceMappingURL=limitItems.js.map
});

const limitItems$1 = /*@__PURE__*/getDefaultExportFromCjs(limitItems);

var equal_1 = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
// https://github.com/ajv-validator/ajv/issues/889

fastDeepEqual.code = 'require("ajv/dist/runtime/equal").default';
exports.default = fastDeepEqual;
//# sourceMappingURL=equal.js.map
});

const equal = /*@__PURE__*/getDefaultExportFromCjs(equal_1);

var uniqueItems = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });




const error = {
    message: ({ params: { i, j } }) => (0, codegen.str) `must NOT have duplicate items (items ## ${j} and ${i} are identical)`,
    params: ({ params: { i, j } }) => (0, codegen._) `{i: ${i}, j: ${j}}`,
};
const def = {
    keyword: "uniqueItems",
    type: "array",
    schemaType: "boolean",
    $data: true,
    error,
    code(cxt) {
        const { gen, data, $data, schema, parentSchema, schemaCode, it } = cxt;
        if (!$data && !schema)
            return;
        const valid = gen.let("valid");
        const itemTypes = parentSchema.items ? (0, dataType.getSchemaTypes)(parentSchema.items) : [];
        cxt.block$data(valid, validateUniqueItems, (0, codegen._) `${schemaCode} === false`);
        cxt.ok(valid);
        function validateUniqueItems() {
            const i = gen.let("i", (0, codegen._) `${data}.length`);
            const j = gen.let("j");
            cxt.setParams({ i, j });
            gen.assign(valid, true);
            gen.if((0, codegen._) `${i} > 1`, () => (canOptimize() ? loopN : loopN2)(i, j));
        }
        function canOptimize() {
            return itemTypes.length > 0 && !itemTypes.some((t) => t === "object" || t === "array");
        }
        function loopN(i, j) {
            const item = gen.name("item");
            const wrongType = (0, dataType.checkDataTypes)(itemTypes, item, it.opts.strictNumbers, dataType.DataType.Wrong);
            const indices = gen.const("indices", (0, codegen._) `{}`);
            gen.for((0, codegen._) `;${i}--;`, () => {
                gen.let(item, (0, codegen._) `${data}[${i}]`);
                gen.if(wrongType, (0, codegen._) `continue`);
                if (itemTypes.length > 1)
                    gen.if((0, codegen._) `typeof ${item} == "string"`, (0, codegen._) `${item} += "_"`);
                gen
                    .if((0, codegen._) `typeof ${indices}[${item}] == "number"`, () => {
                    gen.assign(j, (0, codegen._) `${indices}[${item}]`);
                    cxt.error();
                    gen.assign(valid, false).break();
                })
                    .code((0, codegen._) `${indices}[${item}] = ${i}`);
            });
        }
        function loopN2(i, j) {
            const eql = (0, util.useFunc)(gen, equal_1.default);
            const outer = gen.name("outer");
            gen.label(outer).for((0, codegen._) `;${i}--;`, () => gen.for((0, codegen._) `${j} = ${i}; ${j}--;`, () => gen.if((0, codegen._) `${eql}(${data}[${i}], ${data}[${j}])`, () => {
                cxt.error();
                gen.assign(valid, false).break(outer);
            })));
        }
    },
};
exports.default = def;
//# sourceMappingURL=uniqueItems.js.map
});

const uniqueItems$1 = /*@__PURE__*/getDefaultExportFromCjs(uniqueItems);

var _const = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });



const error = {
    message: "must be equal to constant",
    params: ({ schemaCode }) => (0, codegen._) `{allowedValue: ${schemaCode}}`,
};
const def = {
    keyword: "const",
    $data: true,
    error,
    code(cxt) {
        const { gen, data, $data, schemaCode, schema } = cxt;
        if ($data || (schema && typeof schema == "object")) {
            cxt.fail$data((0, codegen._) `!${(0, util.useFunc)(gen, equal_1.default)}(${data}, ${schemaCode})`);
        }
        else {
            cxt.fail((0, codegen._) `${schema} !== ${data}`);
        }
    },
};
exports.default = def;
//# sourceMappingURL=const.js.map
});

const _const$1 = /*@__PURE__*/getDefaultExportFromCjs(_const);

var _enum = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });



const error = {
    message: "must be equal to one of the allowed values",
    params: ({ schemaCode }) => (0, codegen._) `{allowedValues: ${schemaCode}}`,
};
const def = {
    keyword: "enum",
    schemaType: "array",
    $data: true,
    error,
    code(cxt) {
        const { gen, data, $data, schema, schemaCode, it } = cxt;
        if (!$data && schema.length === 0)
            throw new Error("enum must have non-empty array");
        const useLoop = schema.length >= it.opts.loopEnum;
        let eql;
        const getEql = () => (eql !== null && eql !== void 0 ? eql : (eql = (0, util.useFunc)(gen, equal_1.default)));
        let valid;
        if (useLoop || $data) {
            valid = gen.let("valid");
            cxt.block$data(valid, loopEnum);
        }
        else {
            /* istanbul ignore if */
            if (!Array.isArray(schema))
                throw new Error("ajv implementation error");
            const vSchema = gen.const("vSchema", schemaCode);
            valid = (0, codegen.or)(...schema.map((_x, i) => equalCode(vSchema, i)));
        }
        cxt.pass(valid);
        function loopEnum() {
            gen.assign(valid, false);
            gen.forOf("v", schemaCode, (v) => gen.if((0, codegen._) `${getEql()}(${data}, ${v})`, () => gen.assign(valid, true).break()));
        }
        function equalCode(vSchema, i) {
            const sch = schema[i];
            return typeof sch === "object" && sch !== null
                ? (0, codegen._) `${getEql()}(${data}, ${vSchema}[${i}])`
                : (0, codegen._) `${data} === ${sch}`;
        }
    },
};
exports.default = def;
//# sourceMappingURL=enum.js.map
});

const _enum$1 = /*@__PURE__*/getDefaultExportFromCjs(_enum);

var validation_1 = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });










const validation = [
    // number
    limitNumber.default,
    multipleOf.default,
    // string
    limitLength.default,
    pattern.default,
    // object
    limitProperties.default,
    required.default,
    // array
    limitItems.default,
    uniqueItems.default,
    // any
    { keyword: "type", schemaType: ["string", "array"] },
    { keyword: "nullable", schemaType: "boolean" },
    _const.default,
    _enum.default,
];
exports.default = validation;
//# sourceMappingURL=index.js.map
});

const index$4 = /*@__PURE__*/getDefaultExportFromCjs(validation_1);

var additionalItems = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.validateAdditionalItems = void 0;


const error = {
    message: ({ params: { len } }) => (0, codegen.str) `must NOT have more than ${len} items`,
    params: ({ params: { len } }) => (0, codegen._) `{limit: ${len}}`,
};
const def = {
    keyword: "additionalItems",
    type: "array",
    schemaType: ["boolean", "object"],
    before: "uniqueItems",
    error,
    code(cxt) {
        const { parentSchema, it } = cxt;
        const { items } = parentSchema;
        if (!Array.isArray(items)) {
            (0, util.checkStrictMode)(it, '"additionalItems" is ignored when "items" is not an array of schemas');
            return;
        }
        validateAdditionalItems(cxt, items);
    },
};
function validateAdditionalItems(cxt, items) {
    const { gen, schema, data, keyword, it } = cxt;
    it.items = true;
    const len = gen.const("len", (0, codegen._) `${data}.length`);
    if (schema === false) {
        cxt.setParams({ len: items.length });
        cxt.pass((0, codegen._) `${len} <= ${items.length}`);
    }
    else if (typeof schema == "object" && !(0, util.alwaysValidSchema)(it, schema)) {
        const valid = gen.var("valid", (0, codegen._) `${len} <= ${items.length}`); // TODO var
        gen.if((0, codegen.not)(valid), () => validateItems(valid));
        cxt.ok(valid);
    }
    function validateItems(valid) {
        gen.forRange("i", items.length, len, (i) => {
            cxt.subschema({ keyword, dataProp: i, dataPropType: util.Type.Num }, valid);
            if (!it.allErrors)
                gen.if((0, codegen.not)(valid), () => gen.break());
        });
    }
}
exports.validateAdditionalItems = validateAdditionalItems;
exports.default = def;
//# sourceMappingURL=additionalItems.js.map
});

const additionalItems$1 = /*@__PURE__*/getDefaultExportFromCjs(additionalItems);

var items = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.validateTuple = void 0;



const def = {
    keyword: "items",
    type: "array",
    schemaType: ["object", "array", "boolean"],
    before: "uniqueItems",
    code(cxt) {
        const { schema, it } = cxt;
        if (Array.isArray(schema))
            return validateTuple(cxt, "additionalItems", schema);
        it.items = true;
        if ((0, util.alwaysValidSchema)(it, schema))
            return;
        cxt.ok((0, code.validateArray)(cxt));
    },
};
function validateTuple(cxt, extraItems, schArr = cxt.schema) {
    const { gen, parentSchema, data, keyword, it } = cxt;
    checkStrictTuple(parentSchema);
    if (it.opts.unevaluated && schArr.length && it.items !== true) {
        it.items = util.mergeEvaluated.items(gen, schArr.length, it.items);
    }
    const valid = gen.name("valid");
    const len = gen.const("len", (0, codegen._) `${data}.length`);
    schArr.forEach((sch, i) => {
        if ((0, util.alwaysValidSchema)(it, sch))
            return;
        gen.if((0, codegen._) `${len} > ${i}`, () => cxt.subschema({
            keyword,
            schemaProp: i,
            dataProp: i,
        }, valid));
        cxt.ok(valid);
    });
    function checkStrictTuple(sch) {
        const { opts, errSchemaPath } = it;
        const l = schArr.length;
        const fullTuple = l === sch.minItems && (l === sch.maxItems || sch[extraItems] === false);
        if (opts.strictTuples && !fullTuple) {
            const msg = `"${keyword}" is ${l}-tuple, but minItems or maxItems/${extraItems} are not specified or different at path "${errSchemaPath}"`;
            (0, util.checkStrictMode)(it, msg, opts.strictTuples);
        }
    }
}
exports.validateTuple = validateTuple;
exports.default = def;
//# sourceMappingURL=items.js.map
});

const items$1 = /*@__PURE__*/getDefaultExportFromCjs(items);

var prefixItems = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });

const def = {
    keyword: "prefixItems",
    type: "array",
    schemaType: ["array"],
    before: "uniqueItems",
    code: (cxt) => (0, items.validateTuple)(cxt, "items"),
};
exports.default = def;
//# sourceMappingURL=prefixItems.js.map
});

const prefixItems$1 = /*@__PURE__*/getDefaultExportFromCjs(prefixItems);

var items2020 = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });




const error = {
    message: ({ params: { len } }) => (0, codegen.str) `must NOT have more than ${len} items`,
    params: ({ params: { len } }) => (0, codegen._) `{limit: ${len}}`,
};
const def = {
    keyword: "items",
    type: "array",
    schemaType: ["object", "boolean"],
    before: "uniqueItems",
    error,
    code(cxt) {
        const { schema, parentSchema, it } = cxt;
        const { prefixItems } = parentSchema;
        it.items = true;
        if ((0, util.alwaysValidSchema)(it, schema))
            return;
        if (prefixItems)
            (0, additionalItems.validateAdditionalItems)(cxt, prefixItems);
        else
            cxt.ok((0, code.validateArray)(cxt));
    },
};
exports.default = def;
//# sourceMappingURL=items2020.js.map
});

const items2020$1 = /*@__PURE__*/getDefaultExportFromCjs(items2020);

var contains = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });


const error = {
    message: ({ params: { min, max } }) => max === undefined
        ? (0, codegen.str) `must contain at least ${min} valid item(s)`
        : (0, codegen.str) `must contain at least ${min} and no more than ${max} valid item(s)`,
    params: ({ params: { min, max } }) => max === undefined ? (0, codegen._) `{minContains: ${min}}` : (0, codegen._) `{minContains: ${min}, maxContains: ${max}}`,
};
const def = {
    keyword: "contains",
    type: "array",
    schemaType: ["object", "boolean"],
    before: "uniqueItems",
    trackErrors: true,
    error,
    code(cxt) {
        const { gen, schema, parentSchema, data, it } = cxt;
        let min;
        let max;
        const { minContains, maxContains } = parentSchema;
        if (it.opts.next) {
            min = minContains === undefined ? 1 : minContains;
            max = maxContains;
        }
        else {
            min = 1;
        }
        const len = gen.const("len", (0, codegen._) `${data}.length`);
        cxt.setParams({ min, max });
        if (max === undefined && min === 0) {
            (0, util.checkStrictMode)(it, `"minContains" == 0 without "maxContains": "contains" keyword ignored`);
            return;
        }
        if (max !== undefined && min > max) {
            (0, util.checkStrictMode)(it, `"minContains" > "maxContains" is always invalid`);
            cxt.fail();
            return;
        }
        if ((0, util.alwaysValidSchema)(it, schema)) {
            let cond = (0, codegen._) `${len} >= ${min}`;
            if (max !== undefined)
                cond = (0, codegen._) `${cond} && ${len} <= ${max}`;
            cxt.pass(cond);
            return;
        }
        it.items = true;
        const valid = gen.name("valid");
        if (max === undefined && min === 1) {
            validateItems(valid, () => gen.if(valid, () => gen.break()));
        }
        else if (min === 0) {
            gen.let(valid, true);
            if (max !== undefined)
                gen.if((0, codegen._) `${data}.length > 0`, validateItemsWithCount);
        }
        else {
            gen.let(valid, false);
            validateItemsWithCount();
        }
        cxt.result(valid, () => cxt.reset());
        function validateItemsWithCount() {
            const schValid = gen.name("_valid");
            const count = gen.let("count", 0);
            validateItems(schValid, () => gen.if(schValid, () => checkLimits(count)));
        }
        function validateItems(_valid, block) {
            gen.forRange("i", 0, len, (i) => {
                cxt.subschema({
                    keyword: "contains",
                    dataProp: i,
                    dataPropType: util.Type.Num,
                    compositeRule: true,
                }, _valid);
                block();
            });
        }
        function checkLimits(count) {
            gen.code((0, codegen._) `${count}++`);
            if (max === undefined) {
                gen.if((0, codegen._) `${count} >= ${min}`, () => gen.assign(valid, true).break());
            }
            else {
                gen.if((0, codegen._) `${count} > ${max}`, () => gen.assign(valid, false).break());
                if (min === 1)
                    gen.assign(valid, true);
                else
                    gen.if((0, codegen._) `${count} >= ${min}`, () => gen.assign(valid, true));
            }
        }
    },
};
exports.default = def;
//# sourceMappingURL=contains.js.map
});

const contains$1 = /*@__PURE__*/getDefaultExportFromCjs(contains);

var dependencies = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.validateSchemaDeps = exports.validatePropertyDeps = exports.error = void 0;



exports.error = {
    message: ({ params: { property, depsCount, deps } }) => {
        const property_ies = depsCount === 1 ? "property" : "properties";
        return (0, codegen.str) `must have ${property_ies} ${deps} when property ${property} is present`;
    },
    params: ({ params: { property, depsCount, deps, missingProperty } }) => (0, codegen._) `{property: ${property},
    missingProperty: ${missingProperty},
    depsCount: ${depsCount},
    deps: ${deps}}`, // TODO change to reference
};
const def = {
    keyword: "dependencies",
    type: "object",
    schemaType: "object",
    error: exports.error,
    code(cxt) {
        const [propDeps, schDeps] = splitDependencies(cxt);
        validatePropertyDeps(cxt, propDeps);
        validateSchemaDeps(cxt, schDeps);
    },
};
function splitDependencies({ schema }) {
    const propertyDeps = {};
    const schemaDeps = {};
    for (const key in schema) {
        if (key === "__proto__")
            continue;
        const deps = Array.isArray(schema[key]) ? propertyDeps : schemaDeps;
        deps[key] = schema[key];
    }
    return [propertyDeps, schemaDeps];
}
function validatePropertyDeps(cxt, propertyDeps = cxt.schema) {
    const { gen, data, it } = cxt;
    if (Object.keys(propertyDeps).length === 0)
        return;
    const missing = gen.let("missing");
    for (const prop in propertyDeps) {
        const deps = propertyDeps[prop];
        if (deps.length === 0)
            continue;
        const hasProperty = (0, code.propertyInData)(gen, data, prop, it.opts.ownProperties);
        cxt.setParams({
            property: prop,
            depsCount: deps.length,
            deps: deps.join(", "),
        });
        if (it.allErrors) {
            gen.if(hasProperty, () => {
                for (const depProp of deps) {
                    (0, code.checkReportMissingProp)(cxt, depProp);
                }
            });
        }
        else {
            gen.if((0, codegen._) `${hasProperty} && (${(0, code.checkMissingProp)(cxt, deps, missing)})`);
            (0, code.reportMissingProp)(cxt, missing);
            gen.else();
        }
    }
}
exports.validatePropertyDeps = validatePropertyDeps;
function validateSchemaDeps(cxt, schemaDeps = cxt.schema) {
    const { gen, data, keyword, it } = cxt;
    const valid = gen.name("valid");
    for (const prop in schemaDeps) {
        if ((0, util.alwaysValidSchema)(it, schemaDeps[prop]))
            continue;
        gen.if((0, code.propertyInData)(gen, data, prop, it.opts.ownProperties), () => {
            const schCxt = cxt.subschema({ keyword, schemaProp: prop }, valid);
            cxt.mergeValidEvaluated(schCxt, valid);
        }, () => gen.var(valid, true) // TODO var
        );
        cxt.ok(valid);
    }
}
exports.validateSchemaDeps = validateSchemaDeps;
exports.default = def;
//# sourceMappingURL=dependencies.js.map
});

const dependencies$1 = /*@__PURE__*/getDefaultExportFromCjs(dependencies);

var propertyNames = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });


const error = {
    message: "property name must be valid",
    params: ({ params }) => (0, codegen._) `{propertyName: ${params.propertyName}}`,
};
const def = {
    keyword: "propertyNames",
    type: "object",
    schemaType: ["object", "boolean"],
    error,
    code(cxt) {
        const { gen, schema, data, it } = cxt;
        if ((0, util.alwaysValidSchema)(it, schema))
            return;
        const valid = gen.name("valid");
        gen.forIn("key", data, (key) => {
            cxt.setParams({ propertyName: key });
            cxt.subschema({
                keyword: "propertyNames",
                data: key,
                dataTypes: ["string"],
                propertyName: key,
                compositeRule: true,
            }, valid);
            gen.if((0, codegen.not)(valid), () => {
                cxt.error(true);
                if (!it.allErrors)
                    gen.break();
            });
        });
        cxt.ok(valid);
    },
};
exports.default = def;
//# sourceMappingURL=propertyNames.js.map
});

const propertyNames$1 = /*@__PURE__*/getDefaultExportFromCjs(propertyNames);

var additionalProperties = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });




const error = {
    message: "must NOT have additional properties",
    params: ({ params }) => (0, codegen._) `{additionalProperty: ${params.additionalProperty}}`,
};
const def = {
    keyword: "additionalProperties",
    type: ["object"],
    schemaType: ["boolean", "object"],
    allowUndefined: true,
    trackErrors: true,
    error,
    code(cxt) {
        const { gen, schema, parentSchema, data, errsCount, it } = cxt;
        /* istanbul ignore if */
        if (!errsCount)
            throw new Error("ajv implementation error");
        const { allErrors, opts } = it;
        it.props = true;
        if (opts.removeAdditional !== "all" && (0, util.alwaysValidSchema)(it, schema))
            return;
        const props = (0, code.allSchemaProperties)(parentSchema.properties);
        const patProps = (0, code.allSchemaProperties)(parentSchema.patternProperties);
        checkAdditionalProperties();
        cxt.ok((0, codegen._) `${errsCount} === ${names_1.default.errors}`);
        function checkAdditionalProperties() {
            gen.forIn("key", data, (key) => {
                if (!props.length && !patProps.length)
                    additionalPropertyCode(key);
                else
                    gen.if(isAdditional(key), () => additionalPropertyCode(key));
            });
        }
        function isAdditional(key) {
            let definedProp;
            if (props.length > 8) {
                // TODO maybe an option instead of hard-coded 8?
                const propsSchema = (0, util.schemaRefOrVal)(it, parentSchema.properties, "properties");
                definedProp = (0, code.isOwnProperty)(gen, propsSchema, key);
            }
            else if (props.length) {
                definedProp = (0, codegen.or)(...props.map((p) => (0, codegen._) `${key} === ${p}`));
            }
            else {
                definedProp = codegen.nil;
            }
            if (patProps.length) {
                definedProp = (0, codegen.or)(definedProp, ...patProps.map((p) => (0, codegen._) `${(0, code.usePattern)(cxt, p)}.test(${key})`));
            }
            return (0, codegen.not)(definedProp);
        }
        function deleteAdditional(key) {
            gen.code((0, codegen._) `delete ${data}[${key}]`);
        }
        function additionalPropertyCode(key) {
            if (opts.removeAdditional === "all" || (opts.removeAdditional && schema === false)) {
                deleteAdditional(key);
                return;
            }
            if (schema === false) {
                cxt.setParams({ additionalProperty: key });
                cxt.error();
                if (!allErrors)
                    gen.break();
                return;
            }
            if (typeof schema == "object" && !(0, util.alwaysValidSchema)(it, schema)) {
                const valid = gen.name("valid");
                if (opts.removeAdditional === "failing") {
                    applyAdditionalSchema(key, valid, false);
                    gen.if((0, codegen.not)(valid), () => {
                        cxt.reset();
                        deleteAdditional(key);
                    });
                }
                else {
                    applyAdditionalSchema(key, valid);
                    if (!allErrors)
                        gen.if((0, codegen.not)(valid), () => gen.break());
                }
            }
        }
        function applyAdditionalSchema(key, valid, errors) {
            const subschema = {
                keyword: "additionalProperties",
                dataProp: key,
                dataPropType: util.Type.Str,
            };
            if (errors === false) {
                Object.assign(subschema, {
                    compositeRule: true,
                    createErrors: false,
                    allErrors: false,
                });
            }
            cxt.subschema(subschema, valid);
        }
    },
};
exports.default = def;
//# sourceMappingURL=additionalProperties.js.map
});

const additionalProperties$1 = /*@__PURE__*/getDefaultExportFromCjs(additionalProperties);

var properties$1 = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });




const def = {
    keyword: "properties",
    type: "object",
    schemaType: "object",
    code(cxt) {
        const { gen, schema, parentSchema, data, it } = cxt;
        if (it.opts.removeAdditional === "all" && parentSchema.additionalProperties === undefined) {
            additionalProperties.default.code(new validate.KeywordCxt(it, additionalProperties.default, "additionalProperties"));
        }
        const allProps = (0, code.allSchemaProperties)(schema);
        for (const prop of allProps) {
            it.definedProperties.add(prop);
        }
        if (it.opts.unevaluated && allProps.length && it.props !== true) {
            it.props = util.mergeEvaluated.props(gen, (0, util.toHash)(allProps), it.props);
        }
        const properties = allProps.filter((p) => !(0, util.alwaysValidSchema)(it, schema[p]));
        if (properties.length === 0)
            return;
        const valid = gen.name("valid");
        for (const prop of properties) {
            if (hasDefault(prop)) {
                applyPropertySchema(prop);
            }
            else {
                gen.if((0, code.propertyInData)(gen, data, prop, it.opts.ownProperties));
                applyPropertySchema(prop);
                if (!it.allErrors)
                    gen.else().var(valid, true);
                gen.endIf();
            }
            cxt.it.definedProperties.add(prop);
            cxt.ok(valid);
        }
        function hasDefault(prop) {
            return it.opts.useDefaults && !it.compositeRule && schema[prop].default !== undefined;
        }
        function applyPropertySchema(prop) {
            cxt.subschema({
                keyword: "properties",
                schemaProp: prop,
                dataProp: prop,
            }, valid);
        }
    },
};
exports.default = def;
//# sourceMappingURL=properties.js.map
});

const properties$2 = /*@__PURE__*/getDefaultExportFromCjs(properties$1);

var patternProperties = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });



const util_2 = util;
const def = {
    keyword: "patternProperties",
    type: "object",
    schemaType: "object",
    code(cxt) {
        const { gen, schema, data, parentSchema, it } = cxt;
        const { opts } = it;
        const patterns = (0, code.allSchemaProperties)(schema);
        const alwaysValidPatterns = patterns.filter((p) => (0, util.alwaysValidSchema)(it, schema[p]));
        if (patterns.length === 0 ||
            (alwaysValidPatterns.length === patterns.length &&
                (!it.opts.unevaluated || it.props === true))) {
            return;
        }
        const checkProperties = opts.strictSchema && !opts.allowMatchingProperties && parentSchema.properties;
        const valid = gen.name("valid");
        if (it.props !== true && !(it.props instanceof codegen.Name)) {
            it.props = (0, util_2.evaluatedPropsToName)(gen, it.props);
        }
        const { props } = it;
        validatePatternProperties();
        function validatePatternProperties() {
            for (const pat of patterns) {
                if (checkProperties)
                    checkMatchingProperties(pat);
                if (it.allErrors) {
                    validateProperties(pat);
                }
                else {
                    gen.var(valid, true); // TODO var
                    validateProperties(pat);
                    gen.if(valid);
                }
            }
        }
        function checkMatchingProperties(pat) {
            for (const prop in checkProperties) {
                if (new RegExp(pat).test(prop)) {
                    (0, util.checkStrictMode)(it, `property ${prop} matches pattern ${pat} (use allowMatchingProperties)`);
                }
            }
        }
        function validateProperties(pat) {
            gen.forIn("key", data, (key) => {
                gen.if((0, codegen._) `${(0, code.usePattern)(cxt, pat)}.test(${key})`, () => {
                    const alwaysValid = alwaysValidPatterns.includes(pat);
                    if (!alwaysValid) {
                        cxt.subschema({
                            keyword: "patternProperties",
                            schemaProp: pat,
                            dataProp: key,
                            dataPropType: util_2.Type.Str,
                        }, valid);
                    }
                    if (it.opts.unevaluated && props !== true) {
                        gen.assign((0, codegen._) `${props}[${key}]`, true);
                    }
                    else if (!alwaysValid && !it.allErrors) {
                        // can short-circuit if `unevaluatedProperties` is not supported (opts.next === false)
                        // or if all properties were evaluated (props === true)
                        gen.if((0, codegen.not)(valid), () => gen.break());
                    }
                });
            });
        }
    },
};
exports.default = def;
//# sourceMappingURL=patternProperties.js.map
});

const patternProperties$1 = /*@__PURE__*/getDefaultExportFromCjs(patternProperties);

var not = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });

const def = {
    keyword: "not",
    schemaType: ["object", "boolean"],
    trackErrors: true,
    code(cxt) {
        const { gen, schema, it } = cxt;
        if ((0, util.alwaysValidSchema)(it, schema)) {
            cxt.fail();
            return;
        }
        const valid = gen.name("valid");
        cxt.subschema({
            keyword: "not",
            compositeRule: true,
            createErrors: false,
            allErrors: false,
        }, valid);
        cxt.failResult(valid, () => cxt.reset(), () => cxt.error());
    },
    error: { message: "must NOT be valid" },
};
exports.default = def;
//# sourceMappingURL=not.js.map
});

const not$1 = /*@__PURE__*/getDefaultExportFromCjs(not);

var anyOf = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });

const def = {
    keyword: "anyOf",
    schemaType: "array",
    trackErrors: true,
    code: code.validateUnion,
    error: { message: "must match a schema in anyOf" },
};
exports.default = def;
//# sourceMappingURL=anyOf.js.map
});

const anyOf$1 = /*@__PURE__*/getDefaultExportFromCjs(anyOf);

var oneOf = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });


const error = {
    message: "must match exactly one schema in oneOf",
    params: ({ params }) => (0, codegen._) `{passingSchemas: ${params.passing}}`,
};
const def = {
    keyword: "oneOf",
    schemaType: "array",
    trackErrors: true,
    error,
    code(cxt) {
        const { gen, schema, parentSchema, it } = cxt;
        /* istanbul ignore if */
        if (!Array.isArray(schema))
            throw new Error("ajv implementation error");
        if (it.opts.discriminator && parentSchema.discriminator)
            return;
        const schArr = schema;
        const valid = gen.let("valid", false);
        const passing = gen.let("passing", null);
        const schValid = gen.name("_valid");
        cxt.setParams({ passing });
        // TODO possibly fail straight away (with warning or exception) if there are two empty always valid schemas
        gen.block(validateOneOf);
        cxt.result(valid, () => cxt.reset(), () => cxt.error(true));
        function validateOneOf() {
            schArr.forEach((sch, i) => {
                let schCxt;
                if ((0, util.alwaysValidSchema)(it, sch)) {
                    gen.var(schValid, true);
                }
                else {
                    schCxt = cxt.subschema({
                        keyword: "oneOf",
                        schemaProp: i,
                        compositeRule: true,
                    }, schValid);
                }
                if (i > 0) {
                    gen
                        .if((0, codegen._) `${schValid} && ${valid}`)
                        .assign(valid, false)
                        .assign(passing, (0, codegen._) `[${passing}, ${i}]`)
                        .else();
                }
                gen.if(schValid, () => {
                    gen.assign(valid, true);
                    gen.assign(passing, i);
                    if (schCxt)
                        cxt.mergeEvaluated(schCxt, codegen.Name);
                });
            });
        }
    },
};
exports.default = def;
//# sourceMappingURL=oneOf.js.map
});

const oneOf$1 = /*@__PURE__*/getDefaultExportFromCjs(oneOf);

var allOf = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });

const def = {
    keyword: "allOf",
    schemaType: "array",
    code(cxt) {
        const { gen, schema, it } = cxt;
        /* istanbul ignore if */
        if (!Array.isArray(schema))
            throw new Error("ajv implementation error");
        const valid = gen.name("valid");
        schema.forEach((sch, i) => {
            if ((0, util.alwaysValidSchema)(it, sch))
                return;
            const schCxt = cxt.subschema({ keyword: "allOf", schemaProp: i }, valid);
            cxt.ok(valid);
            cxt.mergeEvaluated(schCxt);
        });
    },
};
exports.default = def;
//# sourceMappingURL=allOf.js.map
});

const allOf$1 = /*@__PURE__*/getDefaultExportFromCjs(allOf);

var _if = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });


const error = {
    message: ({ params }) => (0, codegen.str) `must match "${params.ifClause}" schema`,
    params: ({ params }) => (0, codegen._) `{failingKeyword: ${params.ifClause}}`,
};
const def = {
    keyword: "if",
    schemaType: ["object", "boolean"],
    trackErrors: true,
    error,
    code(cxt) {
        const { gen, parentSchema, it } = cxt;
        if (parentSchema.then === undefined && parentSchema.else === undefined) {
            (0, util.checkStrictMode)(it, '"if" without "then" and "else" is ignored');
        }
        const hasThen = hasSchema(it, "then");
        const hasElse = hasSchema(it, "else");
        if (!hasThen && !hasElse)
            return;
        const valid = gen.let("valid", true);
        const schValid = gen.name("_valid");
        validateIf();
        cxt.reset();
        if (hasThen && hasElse) {
            const ifClause = gen.let("ifClause");
            cxt.setParams({ ifClause });
            gen.if(schValid, validateClause("then", ifClause), validateClause("else", ifClause));
        }
        else if (hasThen) {
            gen.if(schValid, validateClause("then"));
        }
        else {
            gen.if((0, codegen.not)(schValid), validateClause("else"));
        }
        cxt.pass(valid, () => cxt.error(true));
        function validateIf() {
            const schCxt = cxt.subschema({
                keyword: "if",
                compositeRule: true,
                createErrors: false,
                allErrors: false,
            }, schValid);
            cxt.mergeEvaluated(schCxt);
        }
        function validateClause(keyword, ifClause) {
            return () => {
                const schCxt = cxt.subschema({ keyword }, schValid);
                gen.assign(valid, schValid);
                cxt.mergeValidEvaluated(schCxt, valid);
                if (ifClause)
                    gen.assign(ifClause, (0, codegen._) `${keyword}`);
                else
                    cxt.setParams({ ifClause: keyword });
            };
        }
    },
};
function hasSchema(it, keyword) {
    const schema = it.schema[keyword];
    return schema !== undefined && !(0, util.alwaysValidSchema)(it, schema);
}
exports.default = def;
//# sourceMappingURL=if.js.map
});

const _if$1 = /*@__PURE__*/getDefaultExportFromCjs(_if);

var thenElse = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });

const def = {
    keyword: ["then", "else"],
    schemaType: ["object", "boolean"],
    code({ keyword, parentSchema, it }) {
        if (parentSchema.if === undefined)
            (0, util.checkStrictMode)(it, `"${keyword}" without "if" is ignored`);
    },
};
exports.default = def;
//# sourceMappingURL=thenElse.js.map
});

const thenElse$1 = /*@__PURE__*/getDefaultExportFromCjs(thenElse);

var applicator = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
















function getApplicator(draft2020 = false) {
    const applicator = [
        // any
        not.default,
        anyOf.default,
        oneOf.default,
        allOf.default,
        _if.default,
        thenElse.default,
        // object
        propertyNames.default,
        additionalProperties.default,
        dependencies.default,
        properties$1.default,
        patternProperties.default,
    ];
    // array
    if (draft2020)
        applicator.push(prefixItems.default, items2020.default);
    else
        applicator.push(additionalItems.default, items.default);
    applicator.push(contains.default);
    return applicator;
}
exports.default = getApplicator;
//# sourceMappingURL=index.js.map
});

const index$3 = /*@__PURE__*/getDefaultExportFromCjs(applicator);

var format = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });

const error = {
    message: ({ schemaCode }) => (0, codegen.str) `must match format "${schemaCode}"`,
    params: ({ schemaCode }) => (0, codegen._) `{format: ${schemaCode}}`,
};
const def = {
    keyword: "format",
    type: ["number", "string"],
    schemaType: "string",
    $data: true,
    error,
    code(cxt, ruleType) {
        const { gen, data, $data, schema, schemaCode, it } = cxt;
        const { opts, errSchemaPath, schemaEnv, self } = it;
        if (!opts.validateFormats)
            return;
        if ($data)
            validate$DataFormat();
        else
            validateFormat();
        function validate$DataFormat() {
            const fmts = gen.scopeValue("formats", {
                ref: self.formats,
                code: opts.code.formats,
            });
            const fDef = gen.const("fDef", (0, codegen._) `${fmts}[${schemaCode}]`);
            const fType = gen.let("fType");
            const format = gen.let("format");
            // TODO simplify
            gen.if((0, codegen._) `typeof ${fDef} == "object" && !(${fDef} instanceof RegExp)`, () => gen.assign(fType, (0, codegen._) `${fDef}.type || "string"`).assign(format, (0, codegen._) `${fDef}.validate`), () => gen.assign(fType, (0, codegen._) `"string"`).assign(format, fDef));
            cxt.fail$data((0, codegen.or)(unknownFmt(), invalidFmt()));
            function unknownFmt() {
                if (opts.strictSchema === false)
                    return codegen.nil;
                return (0, codegen._) `${schemaCode} && !${format}`;
            }
            function invalidFmt() {
                const callFormat = schemaEnv.$async
                    ? (0, codegen._) `(${fDef}.async ? await ${format}(${data}) : ${format}(${data}))`
                    : (0, codegen._) `${format}(${data})`;
                const validData = (0, codegen._) `(typeof ${format} == "function" ? ${callFormat} : ${format}.test(${data}))`;
                return (0, codegen._) `${format} && ${format} !== true && ${fType} === ${ruleType} && !${validData}`;
            }
        }
        function validateFormat() {
            const formatDef = self.formats[schema];
            if (!formatDef) {
                unknownFormat();
                return;
            }
            if (formatDef === true)
                return;
            const [fmtType, format, fmtRef] = getFormat(formatDef);
            if (fmtType === ruleType)
                cxt.pass(validCondition());
            function unknownFormat() {
                if (opts.strictSchema === false) {
                    self.logger.warn(unknownMsg());
                    return;
                }
                throw new Error(unknownMsg());
                function unknownMsg() {
                    return `unknown format "${schema}" ignored in schema at path "${errSchemaPath}"`;
                }
            }
            function getFormat(fmtDef) {
                const code = fmtDef instanceof RegExp
                    ? (0, codegen.regexpCode)(fmtDef)
                    : opts.code.formats
                        ? (0, codegen._) `${opts.code.formats}${(0, codegen.getProperty)(schema)}`
                        : undefined;
                const fmt = gen.scopeValue("formats", { key: schema, ref: fmtDef, code });
                if (typeof fmtDef == "object" && !(fmtDef instanceof RegExp)) {
                    return [fmtDef.type || "string", fmtDef.validate, (0, codegen._) `${fmt}.validate`];
                }
                return ["string", fmtDef, fmt];
            }
            function validCondition() {
                if (typeof formatDef == "object" && !(formatDef instanceof RegExp) && formatDef.async) {
                    if (!schemaEnv.$async)
                        throw new Error("async format in sync schema");
                    return (0, codegen._) `await ${fmtRef}(${data})`;
                }
                return typeof format == "function" ? (0, codegen._) `${fmtRef}(${data})` : (0, codegen._) `${fmtRef}.test(${data})`;
            }
        }
    },
};
exports.default = def;
//# sourceMappingURL=format.js.map
});

const format$1 = /*@__PURE__*/getDefaultExportFromCjs(format);

var format_2 = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });

const format$1 = [format.default];
exports.default = format$1;
//# sourceMappingURL=index.js.map
});

const index$2 = /*@__PURE__*/getDefaultExportFromCjs(format_2);

var metadata = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.contentVocabulary = exports.metadataVocabulary = void 0;
exports.metadataVocabulary = [
    "title",
    "description",
    "default",
    "deprecated",
    "readOnly",
    "writeOnly",
    "examples",
];
exports.contentVocabulary = [
    "contentMediaType",
    "contentEncoding",
    "contentSchema",
];
//# sourceMappingURL=metadata.js.map
});

const metadata$1 = /*@__PURE__*/getDefaultExportFromCjs(metadata);

var draft7 = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });





const draft7Vocabularies = [
    core_1.default,
    validation_1.default,
    (0, applicator.default)(),
    format_2.default,
    metadata.metadataVocabulary,
    metadata.contentVocabulary,
];
exports.default = draft7Vocabularies;
//# sourceMappingURL=draft7.js.map
});

const draft7$1 = /*@__PURE__*/getDefaultExportFromCjs(draft7);

var types = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.DiscrError = void 0;
var DiscrError;
(function (DiscrError) {
    DiscrError["Tag"] = "tag";
    DiscrError["Mapping"] = "mapping";
})(DiscrError || (exports.DiscrError = DiscrError = {}));
//# sourceMappingURL=types.js.map
});

const types$1 = /*@__PURE__*/getDefaultExportFromCjs(types);

var discriminator = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });





const error = {
    message: ({ params: { discrError, tagName } }) => discrError === types.DiscrError.Tag
        ? `tag "${tagName}" must be string`
        : `value of tag "${tagName}" must be in oneOf`,
    params: ({ params: { discrError, tag, tagName } }) => (0, codegen._) `{error: ${discrError}, tag: ${tagName}, tagValue: ${tag}}`,
};
const def = {
    keyword: "discriminator",
    type: "object",
    schemaType: "object",
    error,
    code(cxt) {
        const { gen, data, schema, parentSchema, it } = cxt;
        const { oneOf } = parentSchema;
        if (!it.opts.discriminator) {
            throw new Error("discriminator: requires discriminator option");
        }
        const tagName = schema.propertyName;
        if (typeof tagName != "string")
            throw new Error("discriminator: requires propertyName");
        if (schema.mapping)
            throw new Error("discriminator: mapping is not supported");
        if (!oneOf)
            throw new Error("discriminator: requires oneOf keyword");
        const valid = gen.let("valid", false);
        const tag = gen.const("tag", (0, codegen._) `${data}${(0, codegen.getProperty)(tagName)}`);
        gen.if((0, codegen._) `typeof ${tag} == "string"`, () => validateMapping(), () => cxt.error(false, { discrError: types.DiscrError.Tag, tag, tagName }));
        cxt.ok(valid);
        function validateMapping() {
            const mapping = getMapping();
            gen.if(false);
            for (const tagValue in mapping) {
                gen.elseIf((0, codegen._) `${tag} === ${tagValue}`);
                gen.assign(valid, applyTagSchema(mapping[tagValue]));
            }
            gen.else();
            cxt.error(false, { discrError: types.DiscrError.Mapping, tag, tagName });
            gen.endIf();
        }
        function applyTagSchema(schemaProp) {
            const _valid = gen.name("valid");
            const schCxt = cxt.subschema({ keyword: "oneOf", schemaProp }, _valid);
            cxt.mergeEvaluated(schCxt, codegen.Name);
            return _valid;
        }
        function getMapping() {
            var _a;
            const oneOfMapping = {};
            const topRequired = hasRequired(parentSchema);
            let tagRequired = true;
            for (let i = 0; i < oneOf.length; i++) {
                let sch = oneOf[i];
                if ((sch === null || sch === void 0 ? void 0 : sch.$ref) && !(0, util.schemaHasRulesButRef)(sch, it.self.RULES)) {
                    const ref = sch.$ref;
                    sch = compile.resolveRef.call(it.self, it.schemaEnv.root, it.baseId, ref);
                    if (sch instanceof compile.SchemaEnv)
                        sch = sch.schema;
                    if (sch === undefined)
                        throw new ref_error.default(it.opts.uriResolver, it.baseId, ref);
                }
                const propSch = (_a = sch === null || sch === void 0 ? void 0 : sch.properties) === null || _a === void 0 ? void 0 : _a[tagName];
                if (typeof propSch != "object") {
                    throw new Error(`discriminator: oneOf subschemas (or referenced schemas) must have "properties/${tagName}"`);
                }
                tagRequired = tagRequired && (topRequired || hasRequired(sch));
                addMappings(propSch, i);
            }
            if (!tagRequired)
                throw new Error(`discriminator: "${tagName}" must be required`);
            return oneOfMapping;
            function hasRequired({ required }) {
                return Array.isArray(required) && required.includes(tagName);
            }
            function addMappings(sch, i) {
                if (sch.const) {
                    addMapping(sch.const, i);
                }
                else if (sch.enum) {
                    for (const tagValue of sch.enum) {
                        addMapping(tagValue, i);
                    }
                }
                else {
                    throw new Error(`discriminator: "properties/${tagName}" must have "const" or "enum"`);
                }
            }
            function addMapping(tagValue, i) {
                if (typeof tagValue != "string" || tagValue in oneOfMapping) {
                    throw new Error(`discriminator: "${tagName}" values must be unique strings`);
                }
                oneOfMapping[tagValue] = i;
            }
        }
    },
};
exports.default = def;
//# sourceMappingURL=index.js.map
});

const index$1 = /*@__PURE__*/getDefaultExportFromCjs(discriminator);

const $schema = "http://json-schema.org/draft-07/schema#";
const $id = "http://json-schema.org/draft-07/schema#";
const title = "Core schema meta-schema";
const definitions = {
	schemaArray: {
		type: "array",
		minItems: 1,
		items: {
			$ref: "#"
		}
	},
	nonNegativeInteger: {
		type: "integer",
		minimum: 0
	},
	nonNegativeIntegerDefault0: {
		allOf: [
			{
				$ref: "#/definitions/nonNegativeInteger"
			},
			{
				"default": 0
			}
		]
	},
	simpleTypes: {
		"enum": [
			"array",
			"boolean",
			"integer",
			"null",
			"number",
			"object",
			"string"
		]
	},
	stringArray: {
		type: "array",
		items: {
			type: "string"
		},
		uniqueItems: true,
		"default": [
		]
	}
};
const type = [
	"object",
	"boolean"
];
const properties = {
	$id: {
		type: "string",
		format: "uri-reference"
	},
	$schema: {
		type: "string",
		format: "uri"
	},
	$ref: {
		type: "string",
		format: "uri-reference"
	},
	$comment: {
		type: "string"
	},
	title: {
		type: "string"
	},
	description: {
		type: "string"
	},
	"default": true,
	readOnly: {
		type: "boolean",
		"default": false
	},
	examples: {
		type: "array",
		items: true
	},
	multipleOf: {
		type: "number",
		exclusiveMinimum: 0
	},
	maximum: {
		type: "number"
	},
	exclusiveMaximum: {
		type: "number"
	},
	minimum: {
		type: "number"
	},
	exclusiveMinimum: {
		type: "number"
	},
	maxLength: {
		$ref: "#/definitions/nonNegativeInteger"
	},
	minLength: {
		$ref: "#/definitions/nonNegativeIntegerDefault0"
	},
	pattern: {
		type: "string",
		format: "regex"
	},
	additionalItems: {
		$ref: "#"
	},
	items: {
		anyOf: [
			{
				$ref: "#"
			},
			{
				$ref: "#/definitions/schemaArray"
			}
		],
		"default": true
	},
	maxItems: {
		$ref: "#/definitions/nonNegativeInteger"
	},
	minItems: {
		$ref: "#/definitions/nonNegativeIntegerDefault0"
	},
	uniqueItems: {
		type: "boolean",
		"default": false
	},
	contains: {
		$ref: "#"
	},
	maxProperties: {
		$ref: "#/definitions/nonNegativeInteger"
	},
	minProperties: {
		$ref: "#/definitions/nonNegativeIntegerDefault0"
	},
	required: {
		$ref: "#/definitions/stringArray"
	},
	additionalProperties: {
		$ref: "#"
	},
	definitions: {
		type: "object",
		additionalProperties: {
			$ref: "#"
		},
		"default": {
		}
	},
	properties: {
		type: "object",
		additionalProperties: {
			$ref: "#"
		},
		"default": {
		}
	},
	patternProperties: {
		type: "object",
		additionalProperties: {
			$ref: "#"
		},
		propertyNames: {
			format: "regex"
		},
		"default": {
		}
	},
	dependencies: {
		type: "object",
		additionalProperties: {
			anyOf: [
				{
					$ref: "#"
				},
				{
					$ref: "#/definitions/stringArray"
				}
			]
		}
	},
	propertyNames: {
		$ref: "#"
	},
	"const": true,
	"enum": {
		type: "array",
		items: true,
		minItems: 1,
		uniqueItems: true
	},
	type: {
		anyOf: [
			{
				$ref: "#/definitions/simpleTypes"
			},
			{
				type: "array",
				items: {
					$ref: "#/definitions/simpleTypes"
				},
				minItems: 1,
				uniqueItems: true
			}
		]
	},
	format: {
		type: "string"
	},
	contentMediaType: {
		type: "string"
	},
	contentEncoding: {
		type: "string"
	},
	"if": {
		$ref: "#"
	},
	then: {
		$ref: "#"
	},
	"else": {
		$ref: "#"
	},
	allOf: {
		$ref: "#/definitions/schemaArray"
	},
	anyOf: {
		$ref: "#/definitions/schemaArray"
	},
	oneOf: {
		$ref: "#/definitions/schemaArray"
	},
	not: {
		$ref: "#"
	}
};
const draft7MetaSchema = {
	$schema: $schema,
	$id: $id,
	title: title,
	definitions: definitions,
	type: type,
	properties: properties,
	"default": true
};

var ajv = createCommonjsModule(function (module, exports) {
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.MissingRefError = exports.ValidationError = exports.CodeGen = exports.Name = exports.nil = exports.stringify = exports.str = exports._ = exports.KeywordCxt = exports.Ajv = void 0;




const META_SUPPORT_DATA = ["/properties"];
const META_SCHEMA_ID = "http://json-schema.org/draft-07/schema";
class Ajv extends core.default {
    _addVocabularies() {
        super._addVocabularies();
        draft7.default.forEach((v) => this.addVocabulary(v));
        if (this.opts.discriminator)
            this.addKeyword(discriminator.default);
    }
    _addDefaultMetaSchema() {
        super._addDefaultMetaSchema();
        if (!this.opts.meta)
            return;
        const metaSchema = this.opts.$data
            ? this.$dataMetaSchema(draft7MetaSchema, META_SUPPORT_DATA)
            : draft7MetaSchema;
        this.addMetaSchema(metaSchema, META_SCHEMA_ID, false);
        this.refs["http://json-schema.org/schema"] = META_SCHEMA_ID;
    }
    defaultMeta() {
        return (this.opts.defaultMeta =
            super.defaultMeta() || (this.getSchema(META_SCHEMA_ID) ? META_SCHEMA_ID : undefined));
    }
}
exports.Ajv = Ajv;
module.exports = exports = Ajv;
module.exports.Ajv = Ajv;
Object.defineProperty(exports, "__esModule", { value: true });
exports.default = Ajv;

Object.defineProperty(exports, "KeywordCxt", { enumerable: true, get: function () { return validate.KeywordCxt; } });

Object.defineProperty(exports, "_", { enumerable: true, get: function () { return codegen._; } });
Object.defineProperty(exports, "str", { enumerable: true, get: function () { return codegen.str; } });
Object.defineProperty(exports, "stringify", { enumerable: true, get: function () { return codegen.stringify; } });
Object.defineProperty(exports, "nil", { enumerable: true, get: function () { return codegen.nil; } });
Object.defineProperty(exports, "Name", { enumerable: true, get: function () { return codegen.Name; } });
Object.defineProperty(exports, "CodeGen", { enumerable: true, get: function () { return codegen.CodeGen; } });

Object.defineProperty(exports, "ValidationError", { enumerable: true, get: function () { return validation_error.default; } });

Object.defineProperty(exports, "MissingRefError", { enumerable: true, get: function () { return ref_error.default; } });
//# sourceMappingURL=ajv.js.map
});

const Ajv = /*@__PURE__*/getDefaultExportFromCjs(ajv);

/**
 * lodash 3.0.4 (Custom Build) <https://lodash.com/>
 * Build: `lodash modern modularize exports="npm" -o ./`
 * Copyright 2012-2015 The Dojo Foundation <http://dojofoundation.org/>
 * Based on Underscore.js 1.8.3 <http://underscorejs.org/LICENSE>
 * Copyright 2009-2015 Jeremy Ashkenas, DocumentCloud and Investigative Reporters & Editors
 * Available under MIT license <https://lodash.com/license>
 */

/** `Object#toString` result references. */
var arrayTag$2 = '[object Array]',
    funcTag$3 = '[object Function]';

/** Used to detect host constructors (Safari > 5). */
var reIsHostCtor$1 = /^\[object .+?Constructor\]$/;

/**
 * Checks if `value` is object-like.
 *
 * @private
 * @param {*} value The value to check.
 * @returns {boolean} Returns `true` if `value` is object-like, else `false`.
 */
function isObjectLike$4(value) {
  return !!value && typeof value == 'object';
}

/** Used for native method references. */
var objectProto$5 = Object.prototype;

/** Used to resolve the decompiled source of functions. */
var fnToString$1 = Function.prototype.toString;

/** Used to check objects for own properties. */
var hasOwnProperty$4 = objectProto$5.hasOwnProperty;

/**
 * Used to resolve the [`toStringTag`](http://ecma-international.org/ecma-262/6.0/#sec-object.prototype.tostring)
 * of values.
 */
var objToString$2 = objectProto$5.toString;

/** Used to detect if a method is native. */
var reIsNative$1 = RegExp('^' +
  fnToString$1.call(hasOwnProperty$4).replace(/[\\^$.*+?()[\]{}|]/g, '\\$&')
  .replace(/hasOwnProperty|(function).*?(?=\\\()| for .+?(?=\\\])/g, '$1.*?') + '$'
);

/* Native method references for those with the same name as other `lodash` methods. */
var nativeIsArray = getNative$1(Array, 'isArray');

/**
 * Used as the [maximum length](http://ecma-international.org/ecma-262/6.0/#sec-number.max_safe_integer)
 * of an array-like value.
 */
var MAX_SAFE_INTEGER$3 = 9007199254740991;

/**
 * Gets the native function at `key` of `object`.
 *
 * @private
 * @param {Object} object The object to query.
 * @param {string} key The key of the method to get.
 * @returns {*} Returns the function if it's native, else `undefined`.
 */
function getNative$1(object, key) {
  var value = object == null ? undefined : object[key];
  return isNative$1(value) ? value : undefined;
}

/**
 * Checks if `value` is a valid array-like length.
 *
 * **Note:** This function is based on [`ToLength`](http://ecma-international.org/ecma-262/6.0/#sec-tolength).
 *
 * @private
 * @param {*} value The value to check.
 * @returns {boolean} Returns `true` if `value` is a valid length, else `false`.
 */
function isLength$3(value) {
  return typeof value == 'number' && value > -1 && value % 1 == 0 && value <= MAX_SAFE_INTEGER$3;
}

/**
 * Checks if `value` is classified as an `Array` object.
 *
 * @static
 * @memberOf _
 * @category Lang
 * @param {*} value The value to check.
 * @returns {boolean} Returns `true` if `value` is correctly classified, else `false`.
 * @example
 *
 * _.isArray([1, 2, 3]);
 * // => true
 *
 * _.isArray(function() { return arguments; }());
 * // => false
 */
var isArray = nativeIsArray || function(value) {
  return isObjectLike$4(value) && isLength$3(value.length) && objToString$2.call(value) == arrayTag$2;
};

/**
 * Checks if `value` is classified as a `Function` object.
 *
 * @static
 * @memberOf _
 * @category Lang
 * @param {*} value The value to check.
 * @returns {boolean} Returns `true` if `value` is correctly classified, else `false`.
 * @example
 *
 * _.isFunction(_);
 * // => true
 *
 * _.isFunction(/abc/);
 * // => false
 */
function isFunction$2(value) {
  // The use of `Object#toString` avoids issues with the `typeof` operator
  // in older versions of Chrome and Safari which return 'function' for regexes
  // and Safari 8 equivalents which return 'object' for typed array constructors.
  return isObject$4(value) && objToString$2.call(value) == funcTag$3;
}

/**
 * Checks if `value` is the [language type](https://es5.github.io/#x8) of `Object`.
 * (e.g. arrays, functions, objects, regexes, `new Number(0)`, and `new String('')`)
 *
 * @static
 * @memberOf _
 * @category Lang
 * @param {*} value The value to check.
 * @returns {boolean} Returns `true` if `value` is an object, else `false`.
 * @example
 *
 * _.isObject({});
 * // => true
 *
 * _.isObject([1, 2, 3]);
 * // => true
 *
 * _.isObject(1);
 * // => false
 */
function isObject$4(value) {
  // Avoid a V8 JIT bug in Chrome 19-20.
  // See https://code.google.com/p/v8/issues/detail?id=2291 for more details.
  var type = typeof value;
  return !!value && (type == 'object' || type == 'function');
}

/**
 * Checks if `value` is a native function.
 *
 * @static
 * @memberOf _
 * @category Lang
 * @param {*} value The value to check.
 * @returns {boolean} Returns `true` if `value` is a native function, else `false`.
 * @example
 *
 * _.isNative(Array.prototype.push);
 * // => true
 *
 * _.isNative(_);
 * // => false
 */
function isNative$1(value) {
  if (value == null) {
    return false;
  }
  if (isFunction$2(value)) {
    return reIsNative$1.test(fnToString$1.call(value));
  }
  return isObjectLike$4(value) && reIsHostCtor$1.test(value);
}

var lodash_isarray = isArray;

/**
 * lodash 3.0.6 (Custom Build) <https://lodash.com/>
 * Build: `lodash modularize exports="npm" -o ./`
 * Copyright jQuery Foundation and other contributors <https://jquery.org/>
 * Released under MIT license <https://lodash.com/license>
 * Based on Underscore.js 1.8.3 <http://underscorejs.org/LICENSE>
 * Copyright Jeremy Ashkenas, DocumentCloud and Investigative Reporters & Editors
 */

/** Used as references for various `Number` constants. */
var MAX_SAFE_INTEGER$2 = 9007199254740991;

/** `Object#toString` result references. */
var argsTag$2 = '[object Arguments]',
    arrayTag$1 = '[object Array]',
    boolTag$1 = '[object Boolean]',
    dateTag$1 = '[object Date]',
    errorTag$1 = '[object Error]',
    funcTag$2 = '[object Function]',
    mapTag = '[object Map]',
    numberTag$1 = '[object Number]',
    objectTag$1 = '[object Object]',
    regexpTag$1 = '[object RegExp]',
    setTag = '[object Set]',
    stringTag$1 = '[object String]',
    weakMapTag = '[object WeakMap]';

var arrayBufferTag = '[object ArrayBuffer]',
    dataViewTag = '[object DataView]',
    float32Tag = '[object Float32Array]',
    float64Tag = '[object Float64Array]',
    int8Tag = '[object Int8Array]',
    int16Tag = '[object Int16Array]',
    int32Tag = '[object Int32Array]',
    uint8Tag = '[object Uint8Array]',
    uint8ClampedTag = '[object Uint8ClampedArray]',
    uint16Tag = '[object Uint16Array]',
    uint32Tag = '[object Uint32Array]';

/** Used to identify `toStringTag` values of typed arrays. */
var typedArrayTags = {};
typedArrayTags[float32Tag] = typedArrayTags[float64Tag] =
typedArrayTags[int8Tag] = typedArrayTags[int16Tag] =
typedArrayTags[int32Tag] = typedArrayTags[uint8Tag] =
typedArrayTags[uint8ClampedTag] = typedArrayTags[uint16Tag] =
typedArrayTags[uint32Tag] = true;
typedArrayTags[argsTag$2] = typedArrayTags[arrayTag$1] =
typedArrayTags[arrayBufferTag] = typedArrayTags[boolTag$1] =
typedArrayTags[dataViewTag] = typedArrayTags[dateTag$1] =
typedArrayTags[errorTag$1] = typedArrayTags[funcTag$2] =
typedArrayTags[mapTag] = typedArrayTags[numberTag$1] =
typedArrayTags[objectTag$1] = typedArrayTags[regexpTag$1] =
typedArrayTags[setTag] = typedArrayTags[stringTag$1] =
typedArrayTags[weakMapTag] = false;

/** Used for built-in method references. */
var objectProto$4 = Object.prototype;

/**
 * Used to resolve the [`toStringTag`](http://ecma-international.org/ecma-262/6.0/#sec-object.prototype.tostring)
 * of values.
 */
var objectToString$1 = objectProto$4.toString;

/**
 * Checks if `value` is a valid array-like length.
 *
 * **Note:** This function is loosely based on
 * [`ToLength`](http://ecma-international.org/ecma-262/6.0/#sec-tolength).
 *
 * @static
 * @memberOf _
 * @since 4.0.0
 * @category Lang
 * @param {*} value The value to check.
 * @returns {boolean} Returns `true` if `value` is a valid length,
 *  else `false`.
 * @example
 *
 * _.isLength(3);
 * // => true
 *
 * _.isLength(Number.MIN_VALUE);
 * // => false
 *
 * _.isLength(Infinity);
 * // => false
 *
 * _.isLength('3');
 * // => false
 */
function isLength$2(value) {
  return typeof value == 'number' &&
    value > -1 && value % 1 == 0 && value <= MAX_SAFE_INTEGER$2;
}

/**
 * Checks if `value` is object-like. A value is object-like if it's not `null`
 * and has a `typeof` result of "object".
 *
 * @static
 * @memberOf _
 * @since 4.0.0
 * @category Lang
 * @param {*} value The value to check.
 * @returns {boolean} Returns `true` if `value` is object-like, else `false`.
 * @example
 *
 * _.isObjectLike({});
 * // => true
 *
 * _.isObjectLike([1, 2, 3]);
 * // => true
 *
 * _.isObjectLike(_.noop);
 * // => false
 *
 * _.isObjectLike(null);
 * // => false
 */
function isObjectLike$3(value) {
  return !!value && typeof value == 'object';
}

/**
 * Checks if `value` is classified as a typed array.
 *
 * @static
 * @memberOf _
 * @since 3.0.0
 * @category Lang
 * @param {*} value The value to check.
 * @returns {boolean} Returns `true` if `value` is correctly classified,
 *  else `false`.
 * @example
 *
 * _.isTypedArray(new Uint8Array);
 * // => true
 *
 * _.isTypedArray([]);
 * // => false
 */
function isTypedArray(value) {
  return isObjectLike$3(value) &&
    isLength$2(value.length) && !!typedArrayTags[objectToString$1.call(value)];
}

var lodash_istypedarray = isTypedArray;

/**
 * lodash 3.9.1 (Custom Build) <https://lodash.com/>
 * Build: `lodash modern modularize exports="npm" -o ./`
 * Copyright 2012-2015 The Dojo Foundation <http://dojofoundation.org/>
 * Based on Underscore.js 1.8.3 <http://underscorejs.org/LICENSE>
 * Copyright 2009-2015 Jeremy Ashkenas, DocumentCloud and Investigative Reporters & Editors
 * Available under MIT license <https://lodash.com/license>
 */

/** `Object#toString` result references. */
var funcTag$1 = '[object Function]';

/** Used to detect host constructors (Safari > 5). */
var reIsHostCtor = /^\[object .+?Constructor\]$/;

/**
 * Checks if `value` is object-like.
 *
 * @private
 * @param {*} value The value to check.
 * @returns {boolean} Returns `true` if `value` is object-like, else `false`.
 */
function isObjectLike$2(value) {
  return !!value && typeof value == 'object';
}

/** Used for native method references. */
var objectProto$3 = Object.prototype;

/** Used to resolve the decompiled source of functions. */
var fnToString = Function.prototype.toString;

/** Used to check objects for own properties. */
var hasOwnProperty$3 = objectProto$3.hasOwnProperty;

/**
 * Used to resolve the [`toStringTag`](http://ecma-international.org/ecma-262/6.0/#sec-object.prototype.tostring)
 * of values.
 */
var objToString$1 = objectProto$3.toString;

/** Used to detect if a method is native. */
var reIsNative = RegExp('^' +
  fnToString.call(hasOwnProperty$3).replace(/[\\^$.*+?()[\]{}|]/g, '\\$&')
  .replace(/hasOwnProperty|(function).*?(?=\\\()| for .+?(?=\\\])/g, '$1.*?') + '$'
);

/**
 * Gets the native function at `key` of `object`.
 *
 * @private
 * @param {Object} object The object to query.
 * @param {string} key The key of the method to get.
 * @returns {*} Returns the function if it's native, else `undefined`.
 */
function getNative(object, key) {
  var value = object == null ? undefined : object[key];
  return isNative(value) ? value : undefined;
}

/**
 * Checks if `value` is classified as a `Function` object.
 *
 * @static
 * @memberOf _
 * @category Lang
 * @param {*} value The value to check.
 * @returns {boolean} Returns `true` if `value` is correctly classified, else `false`.
 * @example
 *
 * _.isFunction(_);
 * // => true
 *
 * _.isFunction(/abc/);
 * // => false
 */
function isFunction$1(value) {
  // The use of `Object#toString` avoids issues with the `typeof` operator
  // in older versions of Chrome and Safari which return 'function' for regexes
  // and Safari 8 equivalents which return 'object' for typed array constructors.
  return isObject$3(value) && objToString$1.call(value) == funcTag$1;
}

/**
 * Checks if `value` is the [language type](https://es5.github.io/#x8) of `Object`.
 * (e.g. arrays, functions, objects, regexes, `new Number(0)`, and `new String('')`)
 *
 * @static
 * @memberOf _
 * @category Lang
 * @param {*} value The value to check.
 * @returns {boolean} Returns `true` if `value` is an object, else `false`.
 * @example
 *
 * _.isObject({});
 * // => true
 *
 * _.isObject([1, 2, 3]);
 * // => true
 *
 * _.isObject(1);
 * // => false
 */
function isObject$3(value) {
  // Avoid a V8 JIT bug in Chrome 19-20.
  // See https://code.google.com/p/v8/issues/detail?id=2291 for more details.
  var type = typeof value;
  return !!value && (type == 'object' || type == 'function');
}

/**
 * Checks if `value` is a native function.
 *
 * @static
 * @memberOf _
 * @category Lang
 * @param {*} value The value to check.
 * @returns {boolean} Returns `true` if `value` is a native function, else `false`.
 * @example
 *
 * _.isNative(Array.prototype.push);
 * // => true
 *
 * _.isNative(_);
 * // => false
 */
function isNative(value) {
  if (value == null) {
    return false;
  }
  if (isFunction$1(value)) {
    return reIsNative.test(fnToString.call(value));
  }
  return isObjectLike$2(value) && reIsHostCtor.test(value);
}

var lodash__getnative = getNative;

/**
 * lodash (Custom Build) <https://lodash.com/>
 * Build: `lodash modularize exports="npm" -o ./`
 * Copyright jQuery Foundation and other contributors <https://jquery.org/>
 * Released under MIT license <https://lodash.com/license>
 * Based on Underscore.js 1.8.3 <http://underscorejs.org/LICENSE>
 * Copyright Jeremy Ashkenas, DocumentCloud and Investigative Reporters & Editors
 */

/** Used as references for various `Number` constants. */
var MAX_SAFE_INTEGER$1 = 9007199254740991;

/** `Object#toString` result references. */
var argsTag$1 = '[object Arguments]',
    funcTag = '[object Function]',
    genTag = '[object GeneratorFunction]';

/** Used for built-in method references. */
var objectProto$2 = Object.prototype;

/** Used to check objects for own properties. */
var hasOwnProperty$2 = objectProto$2.hasOwnProperty;

/**
 * Used to resolve the
 * [`toStringTag`](http://ecma-international.org/ecma-262/7.0/#sec-object.prototype.tostring)
 * of values.
 */
var objectToString = objectProto$2.toString;

/** Built-in value references. */
var propertyIsEnumerable = objectProto$2.propertyIsEnumerable;

/**
 * Checks if `value` is likely an `arguments` object.
 *
 * @static
 * @memberOf _
 * @since 0.1.0
 * @category Lang
 * @param {*} value The value to check.
 * @returns {boolean} Returns `true` if `value` is an `arguments` object,
 *  else `false`.
 * @example
 *
 * _.isArguments(function() { return arguments; }());
 * // => true
 *
 * _.isArguments([1, 2, 3]);
 * // => false
 */
function isArguments(value) {
  // Safari 8.1 makes `arguments.callee` enumerable in strict mode.
  return isArrayLikeObject(value) && hasOwnProperty$2.call(value, 'callee') &&
    (!propertyIsEnumerable.call(value, 'callee') || objectToString.call(value) == argsTag$1);
}

/**
 * Checks if `value` is array-like. A value is considered array-like if it's
 * not a function and has a `value.length` that's an integer greater than or
 * equal to `0` and less than or equal to `Number.MAX_SAFE_INTEGER`.
 *
 * @static
 * @memberOf _
 * @since 4.0.0
 * @category Lang
 * @param {*} value The value to check.
 * @returns {boolean} Returns `true` if `value` is array-like, else `false`.
 * @example
 *
 * _.isArrayLike([1, 2, 3]);
 * // => true
 *
 * _.isArrayLike(document.body.children);
 * // => true
 *
 * _.isArrayLike('abc');
 * // => true
 *
 * _.isArrayLike(_.noop);
 * // => false
 */
function isArrayLike$1(value) {
  return value != null && isLength$1(value.length) && !isFunction(value);
}

/**
 * This method is like `_.isArrayLike` except that it also checks if `value`
 * is an object.
 *
 * @static
 * @memberOf _
 * @since 4.0.0
 * @category Lang
 * @param {*} value The value to check.
 * @returns {boolean} Returns `true` if `value` is an array-like object,
 *  else `false`.
 * @example
 *
 * _.isArrayLikeObject([1, 2, 3]);
 * // => true
 *
 * _.isArrayLikeObject(document.body.children);
 * // => true
 *
 * _.isArrayLikeObject('abc');
 * // => false
 *
 * _.isArrayLikeObject(_.noop);
 * // => false
 */
function isArrayLikeObject(value) {
  return isObjectLike$1(value) && isArrayLike$1(value);
}

/**
 * Checks if `value` is classified as a `Function` object.
 *
 * @static
 * @memberOf _
 * @since 0.1.0
 * @category Lang
 * @param {*} value The value to check.
 * @returns {boolean} Returns `true` if `value` is a function, else `false`.
 * @example
 *
 * _.isFunction(_);
 * // => true
 *
 * _.isFunction(/abc/);
 * // => false
 */
function isFunction(value) {
  // The use of `Object#toString` avoids issues with the `typeof` operator
  // in Safari 8-9 which returns 'object' for typed array and other constructors.
  var tag = isObject$2(value) ? objectToString.call(value) : '';
  return tag == funcTag || tag == genTag;
}

/**
 * Checks if `value` is a valid array-like length.
 *
 * **Note:** This method is loosely based on
 * [`ToLength`](http://ecma-international.org/ecma-262/7.0/#sec-tolength).
 *
 * @static
 * @memberOf _
 * @since 4.0.0
 * @category Lang
 * @param {*} value The value to check.
 * @returns {boolean} Returns `true` if `value` is a valid length, else `false`.
 * @example
 *
 * _.isLength(3);
 * // => true
 *
 * _.isLength(Number.MIN_VALUE);
 * // => false
 *
 * _.isLength(Infinity);
 * // => false
 *
 * _.isLength('3');
 * // => false
 */
function isLength$1(value) {
  return typeof value == 'number' &&
    value > -1 && value % 1 == 0 && value <= MAX_SAFE_INTEGER$1;
}

/**
 * Checks if `value` is the
 * [language type](http://www.ecma-international.org/ecma-262/7.0/#sec-ecmascript-language-types)
 * of `Object`. (e.g. arrays, functions, objects, regexes, `new Number(0)`, and `new String('')`)
 *
 * @static
 * @memberOf _
 * @since 0.1.0
 * @category Lang
 * @param {*} value The value to check.
 * @returns {boolean} Returns `true` if `value` is an object, else `false`.
 * @example
 *
 * _.isObject({});
 * // => true
 *
 * _.isObject([1, 2, 3]);
 * // => true
 *
 * _.isObject(_.noop);
 * // => true
 *
 * _.isObject(null);
 * // => false
 */
function isObject$2(value) {
  var type = typeof value;
  return !!value && (type == 'object' || type == 'function');
}

/**
 * Checks if `value` is object-like. A value is object-like if it's not `null`
 * and has a `typeof` result of "object".
 *
 * @static
 * @memberOf _
 * @since 4.0.0
 * @category Lang
 * @param {*} value The value to check.
 * @returns {boolean} Returns `true` if `value` is object-like, else `false`.
 * @example
 *
 * _.isObjectLike({});
 * // => true
 *
 * _.isObjectLike([1, 2, 3]);
 * // => true
 *
 * _.isObjectLike(_.noop);
 * // => false
 *
 * _.isObjectLike(null);
 * // => false
 */
function isObjectLike$1(value) {
  return !!value && typeof value == 'object';
}

var lodash_isarguments = isArguments;

/**
 * lodash 3.1.2 (Custom Build) <https://lodash.com/>
 * Build: `lodash modern modularize exports="npm" -o ./`
 * Copyright 2012-2015 The Dojo Foundation <http://dojofoundation.org/>
 * Based on Underscore.js 1.8.3 <http://underscorejs.org/LICENSE>
 * Copyright 2009-2015 Jeremy Ashkenas, DocumentCloud and Investigative Reporters & Editors
 * Available under MIT license <https://lodash.com/license>
 */


/** Used to detect unsigned integer values. */
var reIsUint = /^\d+$/;

/** Used for native method references. */
var objectProto$1 = Object.prototype;

/** Used to check objects for own properties. */
var hasOwnProperty$1 = objectProto$1.hasOwnProperty;

/* Native method references for those with the same name as other `lodash` methods. */
var nativeKeys = lodash__getnative(Object, 'keys');

/**
 * Used as the [maximum length](http://ecma-international.org/ecma-262/6.0/#sec-number.max_safe_integer)
 * of an array-like value.
 */
var MAX_SAFE_INTEGER = 9007199254740991;

/**
 * The base implementation of `_.property` without support for deep paths.
 *
 * @private
 * @param {string} key The key of the property to get.
 * @returns {Function} Returns the new function.
 */
function baseProperty(key) {
  return function(object) {
    return object == null ? undefined : object[key];
  };
}

/**
 * Gets the "length" property value of `object`.
 *
 * **Note:** This function is used to avoid a [JIT bug](https://bugs.webkit.org/show_bug.cgi?id=142792)
 * that affects Safari on at least iOS 8.1-8.3 ARM64.
 *
 * @private
 * @param {Object} object The object to query.
 * @returns {*} Returns the "length" value.
 */
var getLength = baseProperty('length');

/**
 * Checks if `value` is array-like.
 *
 * @private
 * @param {*} value The value to check.
 * @returns {boolean} Returns `true` if `value` is array-like, else `false`.
 */
function isArrayLike(value) {
  return value != null && isLength(getLength(value));
}

/**
 * Checks if `value` is a valid array-like index.
 *
 * @private
 * @param {*} value The value to check.
 * @param {number} [length=MAX_SAFE_INTEGER] The upper bounds of a valid index.
 * @returns {boolean} Returns `true` if `value` is a valid index, else `false`.
 */
function isIndex(value, length) {
  value = (typeof value == 'number' || reIsUint.test(value)) ? +value : -1;
  length = length == null ? MAX_SAFE_INTEGER : length;
  return value > -1 && value % 1 == 0 && value < length;
}

/**
 * Checks if `value` is a valid array-like length.
 *
 * **Note:** This function is based on [`ToLength`](http://ecma-international.org/ecma-262/6.0/#sec-tolength).
 *
 * @private
 * @param {*} value The value to check.
 * @returns {boolean} Returns `true` if `value` is a valid length, else `false`.
 */
function isLength(value) {
  return typeof value == 'number' && value > -1 && value % 1 == 0 && value <= MAX_SAFE_INTEGER;
}

/**
 * A fallback implementation of `Object.keys` which creates an array of the
 * own enumerable property names of `object`.
 *
 * @private
 * @param {Object} object The object to query.
 * @returns {Array} Returns the array of property names.
 */
function shimKeys(object) {
  var props = keysIn(object),
      propsLength = props.length,
      length = propsLength && object.length;

  var allowIndexes = !!length && isLength(length) &&
    (lodash_isarray(object) || lodash_isarguments(object));

  var index = -1,
      result = [];

  while (++index < propsLength) {
    var key = props[index];
    if ((allowIndexes && isIndex(key, length)) || hasOwnProperty$1.call(object, key)) {
      result.push(key);
    }
  }
  return result;
}

/**
 * Checks if `value` is the [language type](https://es5.github.io/#x8) of `Object`.
 * (e.g. arrays, functions, objects, regexes, `new Number(0)`, and `new String('')`)
 *
 * @static
 * @memberOf _
 * @category Lang
 * @param {*} value The value to check.
 * @returns {boolean} Returns `true` if `value` is an object, else `false`.
 * @example
 *
 * _.isObject({});
 * // => true
 *
 * _.isObject([1, 2, 3]);
 * // => true
 *
 * _.isObject(1);
 * // => false
 */
function isObject$1(value) {
  // Avoid a V8 JIT bug in Chrome 19-20.
  // See https://code.google.com/p/v8/issues/detail?id=2291 for more details.
  var type = typeof value;
  return !!value && (type == 'object' || type == 'function');
}

/**
 * Creates an array of the own enumerable property names of `object`.
 *
 * **Note:** Non-object values are coerced to objects. See the
 * [ES spec](http://ecma-international.org/ecma-262/6.0/#sec-object.keys)
 * for more details.
 *
 * @static
 * @memberOf _
 * @category Object
 * @param {Object} object The object to query.
 * @returns {Array} Returns the array of property names.
 * @example
 *
 * function Foo() {
 *   this.a = 1;
 *   this.b = 2;
 * }
 *
 * Foo.prototype.c = 3;
 *
 * _.keys(new Foo);
 * // => ['a', 'b'] (iteration order is not guaranteed)
 *
 * _.keys('hi');
 * // => ['0', '1']
 */
var keys = !nativeKeys ? shimKeys : function(object) {
  var Ctor = object == null ? undefined : object.constructor;
  if ((typeof Ctor == 'function' && Ctor.prototype === object) ||
      (typeof object != 'function' && isArrayLike(object))) {
    return shimKeys(object);
  }
  return isObject$1(object) ? nativeKeys(object) : [];
};

/**
 * Creates an array of the own and inherited enumerable property names of `object`.
 *
 * **Note:** Non-object values are coerced to objects.
 *
 * @static
 * @memberOf _
 * @category Object
 * @param {Object} object The object to query.
 * @returns {Array} Returns the array of property names.
 * @example
 *
 * function Foo() {
 *   this.a = 1;
 *   this.b = 2;
 * }
 *
 * Foo.prototype.c = 3;
 *
 * _.keysIn(new Foo);
 * // => ['a', 'b', 'c'] (iteration order is not guaranteed)
 */
function keysIn(object) {
  if (object == null) {
    return [];
  }
  if (!isObject$1(object)) {
    object = Object(object);
  }
  var length = object.length;
  length = (length && isLength(length) &&
    (lodash_isarray(object) || lodash_isarguments(object)) && length) || 0;

  var Ctor = object.constructor,
      index = -1,
      isProto = typeof Ctor == 'function' && Ctor.prototype === object,
      result = Array(length),
      skipIndexes = length > 0;

  while (++index < length) {
    result[index] = (index + '');
  }
  for (var key in object) {
    if (!(skipIndexes && isIndex(key, length)) &&
        !(key == 'constructor' && (isProto || !hasOwnProperty$1.call(object, key)))) {
      result.push(key);
    }
  }
  return result;
}

var lodash_keys = keys;

/**
 * lodash 3.0.7 (Custom Build) <https://lodash.com/>
 * Build: `lodash modern modularize exports="npm" -o ./`
 * Copyright 2012-2015 The Dojo Foundation <http://dojofoundation.org/>
 * Based on Underscore.js 1.8.3 <http://underscorejs.org/LICENSE>
 * Copyright 2009-2015 Jeremy Ashkenas, DocumentCloud and Investigative Reporters & Editors
 * Available under MIT license <https://lodash.com/license>
 */


/** `Object#toString` result references. */
var argsTag = '[object Arguments]',
    arrayTag = '[object Array]',
    boolTag = '[object Boolean]',
    dateTag = '[object Date]',
    errorTag = '[object Error]',
    numberTag = '[object Number]',
    objectTag = '[object Object]',
    regexpTag = '[object RegExp]',
    stringTag = '[object String]';

/**
 * Checks if `value` is object-like.
 *
 * @private
 * @param {*} value The value to check.
 * @returns {boolean} Returns `true` if `value` is object-like, else `false`.
 */
function isObjectLike(value) {
  return !!value && typeof value == 'object';
}

/** Used for native method references. */
var objectProto = Object.prototype;

/** Used to check objects for own properties. */
var hasOwnProperty = objectProto.hasOwnProperty;

/**
 * Used to resolve the [`toStringTag`](https://people.mozilla.org/~jorendorff/es6-draft.html#sec-object.prototype.tostring)
 * of values.
 */
var objToString = objectProto.toString;

/**
 * A specialized version of `_.some` for arrays without support for callback
 * shorthands and `this` binding.
 *
 * @private
 * @param {Array} array The array to iterate over.
 * @param {Function} predicate The function invoked per iteration.
 * @returns {boolean} Returns `true` if any element passes the predicate check,
 *  else `false`.
 */
function arraySome(array, predicate) {
  var index = -1,
      length = array.length;

  while (++index < length) {
    if (predicate(array[index], index, array)) {
      return true;
    }
  }
  return false;
}

/**
 * The base implementation of `_.isEqual` without support for `this` binding
 * `customizer` functions.
 *
 * @private
 * @param {*} value The value to compare.
 * @param {*} other The other value to compare.
 * @param {Function} [customizer] The function to customize comparing values.
 * @param {boolean} [isLoose] Specify performing partial comparisons.
 * @param {Array} [stackA] Tracks traversed `value` objects.
 * @param {Array} [stackB] Tracks traversed `other` objects.
 * @returns {boolean} Returns `true` if the values are equivalent, else `false`.
 */
function baseIsEqual(value, other, customizer, isLoose, stackA, stackB) {
  if (value === other) {
    return true;
  }
  if (value == null || other == null || (!isObject(value) && !isObjectLike(other))) {
    return value !== value && other !== other;
  }
  return baseIsEqualDeep(value, other, baseIsEqual, customizer, isLoose, stackA, stackB);
}

/**
 * A specialized version of `baseIsEqual` for arrays and objects which performs
 * deep comparisons and tracks traversed objects enabling objects with circular
 * references to be compared.
 *
 * @private
 * @param {Object} object The object to compare.
 * @param {Object} other The other object to compare.
 * @param {Function} equalFunc The function to determine equivalents of values.
 * @param {Function} [customizer] The function to customize comparing objects.
 * @param {boolean} [isLoose] Specify performing partial comparisons.
 * @param {Array} [stackA=[]] Tracks traversed `value` objects.
 * @param {Array} [stackB=[]] Tracks traversed `other` objects.
 * @returns {boolean} Returns `true` if the objects are equivalent, else `false`.
 */
function baseIsEqualDeep(object, other, equalFunc, customizer, isLoose, stackA, stackB) {
  var objIsArr = lodash_isarray(object),
      othIsArr = lodash_isarray(other),
      objTag = arrayTag,
      othTag = arrayTag;

  if (!objIsArr) {
    objTag = objToString.call(object);
    if (objTag == argsTag) {
      objTag = objectTag;
    } else if (objTag != objectTag) {
      objIsArr = lodash_istypedarray(object);
    }
  }
  if (!othIsArr) {
    othTag = objToString.call(other);
    if (othTag == argsTag) {
      othTag = objectTag;
    } else if (othTag != objectTag) {
      othIsArr = lodash_istypedarray(other);
    }
  }
  var objIsObj = objTag == objectTag,
      othIsObj = othTag == objectTag,
      isSameTag = objTag == othTag;

  if (isSameTag && !(objIsArr || objIsObj)) {
    return equalByTag(object, other, objTag);
  }
  if (!isLoose) {
    var objIsWrapped = objIsObj && hasOwnProperty.call(object, '__wrapped__'),
        othIsWrapped = othIsObj && hasOwnProperty.call(other, '__wrapped__');

    if (objIsWrapped || othIsWrapped) {
      return equalFunc(objIsWrapped ? object.value() : object, othIsWrapped ? other.value() : other, customizer, isLoose, stackA, stackB);
    }
  }
  if (!isSameTag) {
    return false;
  }
  // Assume cyclic values are equal.
  // For more information on detecting circular references see https://es5.github.io/#JO.
  stackA || (stackA = []);
  stackB || (stackB = []);

  var length = stackA.length;
  while (length--) {
    if (stackA[length] == object) {
      return stackB[length] == other;
    }
  }
  // Add `object` and `other` to the stack of traversed objects.
  stackA.push(object);
  stackB.push(other);

  var result = (objIsArr ? equalArrays : equalObjects)(object, other, equalFunc, customizer, isLoose, stackA, stackB);

  stackA.pop();
  stackB.pop();

  return result;
}

/**
 * A specialized version of `baseIsEqualDeep` for arrays with support for
 * partial deep comparisons.
 *
 * @private
 * @param {Array} array The array to compare.
 * @param {Array} other The other array to compare.
 * @param {Function} equalFunc The function to determine equivalents of values.
 * @param {Function} [customizer] The function to customize comparing arrays.
 * @param {boolean} [isLoose] Specify performing partial comparisons.
 * @param {Array} [stackA] Tracks traversed `value` objects.
 * @param {Array} [stackB] Tracks traversed `other` objects.
 * @returns {boolean} Returns `true` if the arrays are equivalent, else `false`.
 */
function equalArrays(array, other, equalFunc, customizer, isLoose, stackA, stackB) {
  var index = -1,
      arrLength = array.length,
      othLength = other.length;

  if (arrLength != othLength && !(isLoose && othLength > arrLength)) {
    return false;
  }
  // Ignore non-index properties.
  while (++index < arrLength) {
    var arrValue = array[index],
        othValue = other[index],
        result = customizer ? customizer(isLoose ? othValue : arrValue, isLoose ? arrValue : othValue, index) : undefined;

    if (result !== undefined) {
      if (result) {
        continue;
      }
      return false;
    }
    // Recursively compare arrays (susceptible to call stack limits).
    if (isLoose) {
      if (!arraySome(other, function(othValue) {
            return arrValue === othValue || equalFunc(arrValue, othValue, customizer, isLoose, stackA, stackB);
          })) {
        return false;
      }
    } else if (!(arrValue === othValue || equalFunc(arrValue, othValue, customizer, isLoose, stackA, stackB))) {
      return false;
    }
  }
  return true;
}

/**
 * A specialized version of `baseIsEqualDeep` for comparing objects of
 * the same `toStringTag`.
 *
 * **Note:** This function only supports comparing values with tags of
 * `Boolean`, `Date`, `Error`, `Number`, `RegExp`, or `String`.
 *
 * @private
 * @param {Object} value The object to compare.
 * @param {Object} other The other object to compare.
 * @param {string} tag The `toStringTag` of the objects to compare.
 * @returns {boolean} Returns `true` if the objects are equivalent, else `false`.
 */
function equalByTag(object, other, tag) {
  switch (tag) {
    case boolTag:
    case dateTag:
      // Coerce dates and booleans to numbers, dates to milliseconds and booleans
      // to `1` or `0` treating invalid dates coerced to `NaN` as not equal.
      return +object == +other;

    case errorTag:
      return object.name == other.name && object.message == other.message;

    case numberTag:
      // Treat `NaN` vs. `NaN` as equal.
      return (object != +object)
        ? other != +other
        : object == +other;

    case regexpTag:
    case stringTag:
      // Coerce regexes to strings and treat strings primitives and string
      // objects as equal. See https://es5.github.io/#x15.10.6.4 for more details.
      return object == (other + '');
  }
  return false;
}

/**
 * A specialized version of `baseIsEqualDeep` for objects with support for
 * partial deep comparisons.
 *
 * @private
 * @param {Object} object The object to compare.
 * @param {Object} other The other object to compare.
 * @param {Function} equalFunc The function to determine equivalents of values.
 * @param {Function} [customizer] The function to customize comparing values.
 * @param {boolean} [isLoose] Specify performing partial comparisons.
 * @param {Array} [stackA] Tracks traversed `value` objects.
 * @param {Array} [stackB] Tracks traversed `other` objects.
 * @returns {boolean} Returns `true` if the objects are equivalent, else `false`.
 */
function equalObjects(object, other, equalFunc, customizer, isLoose, stackA, stackB) {
  var objProps = lodash_keys(object),
      objLength = objProps.length,
      othProps = lodash_keys(other),
      othLength = othProps.length;

  if (objLength != othLength && !isLoose) {
    return false;
  }
  var index = objLength;
  while (index--) {
    var key = objProps[index];
    if (!(isLoose ? key in other : hasOwnProperty.call(other, key))) {
      return false;
    }
  }
  var skipCtor = isLoose;
  while (++index < objLength) {
    key = objProps[index];
    var objValue = object[key],
        othValue = other[key],
        result = customizer ? customizer(isLoose ? othValue : objValue, isLoose? objValue : othValue, key) : undefined;

    // Recursively compare objects (susceptible to call stack limits).
    if (!(result === undefined ? equalFunc(objValue, othValue, customizer, isLoose, stackA, stackB) : result)) {
      return false;
    }
    skipCtor || (skipCtor = key == 'constructor');
  }
  if (!skipCtor) {
    var objCtor = object.constructor,
        othCtor = other.constructor;

    // Non `Object` object instances with different constructors are not equal.
    if (objCtor != othCtor &&
        ('constructor' in object && 'constructor' in other) &&
        !(typeof objCtor == 'function' && objCtor instanceof objCtor &&
          typeof othCtor == 'function' && othCtor instanceof othCtor)) {
      return false;
    }
  }
  return true;
}

/**
 * Checks if `value` is the [language type](https://es5.github.io/#x8) of `Object`.
 * (e.g. arrays, functions, objects, regexes, `new Number(0)`, and `new String('')`)
 *
 * @static
 * @memberOf _
 * @category Lang
 * @param {*} value The value to check.
 * @returns {boolean} Returns `true` if `value` is an object, else `false`.
 * @example
 *
 * _.isObject({});
 * // => true
 *
 * _.isObject([1, 2, 3]);
 * // => true
 *
 * _.isObject(1);
 * // => false
 */
function isObject(value) {
  // Avoid a V8 JIT bug in Chrome 19-20.
  // See https://code.google.com/p/v8/issues/detail?id=2291 for more details.
  var type = typeof value;
  return !!value && (type == 'object' || type == 'function');
}

var lodash__baseisequal = baseIsEqual;

/**
 * lodash 3.0.1 (Custom Build) <https://lodash.com/>
 * Build: `lodash modern modularize exports="npm" -o ./`
 * Copyright 2012-2015 The Dojo Foundation <http://dojofoundation.org/>
 * Based on Underscore.js 1.8.3 <http://underscorejs.org/LICENSE>
 * Copyright 2009-2015 Jeremy Ashkenas, DocumentCloud and Investigative Reporters & Editors
 * Available under MIT license <https://lodash.com/license>
 */

/**
 * A specialized version of `baseCallback` which only supports `this` binding
 * and specifying the number of arguments to provide to `func`.
 *
 * @private
 * @param {Function} func The function to bind.
 * @param {*} thisArg The `this` binding of `func`.
 * @param {number} [argCount] The number of arguments to provide to `func`.
 * @returns {Function} Returns the callback.
 */
function bindCallback(func, thisArg, argCount) {
  if (typeof func != 'function') {
    return identity;
  }
  if (thisArg === undefined) {
    return func;
  }
  switch (argCount) {
    case 1: return function(value) {
      return func.call(thisArg, value);
    };
    case 3: return function(value, index, collection) {
      return func.call(thisArg, value, index, collection);
    };
    case 4: return function(accumulator, value, index, collection) {
      return func.call(thisArg, accumulator, value, index, collection);
    };
    case 5: return function(value, other, key, object, source) {
      return func.call(thisArg, value, other, key, object, source);
    };
  }
  return function() {
    return func.apply(thisArg, arguments);
  };
}

/**
 * This method returns the first argument provided to it.
 *
 * @static
 * @memberOf _
 * @category Utility
 * @param {*} value Any value.
 * @returns {*} Returns `value`.
 * @example
 *
 * var object = { 'user': 'fred' };
 *
 * _.identity(object) === object;
 * // => true
 */
function identity(value) {
  return value;
}

var lodash__bindcallback = bindCallback;

/**
 * lodash 3.0.4 (Custom Build) <https://lodash.com/>
 * Build: `lodash modern modularize exports="npm" -o ./`
 * Copyright 2012-2015 The Dojo Foundation <http://dojofoundation.org/>
 * Based on Underscore.js 1.8.3 <http://underscorejs.org/LICENSE>
 * Copyright 2009-2015 Jeremy Ashkenas, DocumentCloud and Investigative Reporters & Editors
 * Available under MIT license <https://lodash.com/license>
 */


/**
 * Performs a deep comparison between two values to determine if they are
 * equivalent. If `customizer` is provided it is invoked to compare values.
 * If `customizer` returns `undefined` comparisons are handled by the method
 * instead. The `customizer` is bound to `thisArg` and invoked with three
 * arguments: (value, other [, index|key]).
 *
 * **Note:** This method supports comparing arrays, booleans, `Date` objects,
 * numbers, `Object` objects, regexes, and strings. Objects are compared by
 * their own, not inherited, enumerable properties. Functions and DOM nodes
 * are **not** supported. Provide a customizer function to extend support
 * for comparing other values.
 *
 * @static
 * @memberOf _
 * @alias eq
 * @category Lang
 * @param {*} value The value to compare.
 * @param {*} other The other value to compare.
 * @param {Function} [customizer] The function to customize value comparisons.
 * @param {*} [thisArg] The `this` binding of `customizer`.
 * @returns {boolean} Returns `true` if the values are equivalent, else `false`.
 * @example
 *
 * var object = { 'user': 'fred' };
 * var other = { 'user': 'fred' };
 *
 * object == other;
 * // => false
 *
 * _.isEqual(object, other);
 * // => true
 *
 * // using a customizer callback
 * var array = ['hello', 'goodbye'];
 * var other = ['hi', 'goodbye'];
 *
 * _.isEqual(array, other, function(value, other) {
 *   if (_.every([value, other], RegExp.prototype.test, /^h(?:i|ello)$/)) {
 *     return true;
 *   }
 * });
 * // => true
 */
function isEqual(value, other, customizer, thisArg) {
  customizer = typeof customizer == 'function' ? lodash__bindcallback(customizer, thisArg, 3) : undefined;
  var result = customizer ? customizer(value, other) : undefined;
  return  result === undefined ? lodash__baseisequal(value, other, customizer) : !!result;
}

var lodash_isequal = isEqual;

var dist = createCommonjsModule(function (module, exports) {
'use strict';

Object.defineProperty(exports, '__esModule', {
  value: true
});

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { 'default': obj }; }



var _lodashIsequal2 = _interopRequireDefault(lodash_isequal);

/*
 * Converts a JSON object to a JSON Schema
 * @param {any} json
 * @param {object} options
 * @returns {object} a json schema
*/
function convert(json) {
  var options = arguments.length <= 1 || arguments[1] === undefined ? {} : arguments[1];

  if (typeof json === 'function') {
    throw new TypeError('Can not convert a function');
  }

  if (json === undefined) {
    return {};
  }

  // primitives
  if (typeof json === 'string') {

    // TODO: date format
    return { type: 'string' };
  }

  if (typeof json === 'boolean') {
    return { type: 'boolean' };
  }

  if (typeof json === 'number') {
    if (Number.isInteger(json)) {
      return { type: 'integer' };
    } else {
      return { type: 'number' };
    }
  }

  if (json === null) {
    return { type: 'null' };
  }

  if (Array.isArray(json)) {
    var _ret = (function () {
      var schema = { type: 'array' };

      if (!json.length) {
        schema.items = {};
        return {
          v: schema
        };
      }

      var schemas = json.map(convert);

      // if all schemas are the same use that schema for items
      if (schemas.every(function (s) {
        return (0, _lodashIsequal2['default'])(s, schemas[0]);
      })) {
        schema.items = schemas[0];

        // if there are multiple schemas use oneOf
      } else {
          schema.items = { oneOf: unique(schemas) };
        }

      return {
        v: schema
      };
    })();

    if (typeof _ret === 'object') return _ret.v;
  }

  var schema = { type: 'object' };

  if (!Object.keys(json).length) {
    return schema;
  }

  schema.properties = Object.keys(json).reduce(function (properties, key) {
    properties[key] = convert(json[key]);
    return properties;
  }, {});

  return schema;
}

/*
 * Removes duplicates from array using isEqual comparator
 * @param {array}
 * @return {array}
*/
function unique() {
  var arr = arguments.length <= 0 || arguments[0] === undefined ? [] : arguments[0];

  return arr.reduce(function (result, item) {

    // result does not contain item
    if (!result.some(function (i) {
      return (0, _lodashIsequal2['default'])(i, item);
    })) {
      result.push(item);
    }
    return result;
  }, []);
}

exports.convert = convert;
});

const index = /*@__PURE__*/getDefaultExportFromCjs(dist);

class HttpEndpointPlugin {
  constructor() {
    eventBus.on(EventTypes.ActivityDesignDisplaying, this.onActivityDisplaying);
    eventBus.on(EventTypes.ActivityPluginUpdated, this.onActivityUpdated);
    eventBus.on(EventTypes.ActivityPluginValidating, this.onActivityValidating);
    eventBus.on(EventTypes.ComponentLoadingCustomButton, this.onComponentLoadingCustomButton);
    eventBus.on(EventTypes.ComponentCustomButtonClick, this.onComponentCustomButtonClick);
  }
  onActivityDisplaying(context) {
    const activityModel = context.activityModel;
    if (activityModel.type !== 'HttpEndpoint')
      return;
    const props = activityModel.properties || [];
    const path = props.find(x => x.name == 'Path') || { name: 'Path', expressions: { 'Literal': '', syntax: SyntaxNames.Literal } };
    const syntax = path.syntax || SyntaxNames.Literal;
    const bodyDisplay = htmlEncode(path.expressions[syntax]);
    context.bodyDisplay = `<p>${bodyDisplay}</p>`;
  }
  onComponentLoadingCustomButton(context) {
    if (context.activityType !== 'HttpEndpoint')
      return;
    if (context.component === 'elsa-script-property') {
      if (context.prop !== 'Schema')
        return;
      const label = 'Convert to Json Schema';
      context.data = { label };
    }
    if (context.component === 'elsa-workflow-definition-editor-screen') {
      const label = 'Use as Schema';
      context.data = { label };
    }
  }
  onComponentCustomButtonClick(context) {
    var _a;
    if (context.activityType !== 'HttpEndpoint')
      return;
    if (context.component === 'elsa-script-property') {
      if (context.prop !== 'Schema')
        return;
      window.open('https://www.convertsimple.com/convert-json-to-json-schema/');
    }
    if (context.component === 'elsa-workflow-definition-editor-screen') {
      const activityUpdatedContext = {
        activityModel: context.params[0],
        data: JSON.stringify(dist.convert((_a = context.params[1]) === null || _a === void 0 ? void 0 : _a.Body), null, 1)
      };
      eventBus.emit(EventTypes.ActivityPluginUpdated, this, activityUpdatedContext);
    }
  }
  onActivityUpdated(context) {
    const activityModel = context.activityModel;
    if (activityModel.type !== 'HttpEndpoint')
      return;
    const props = activityModel.properties || [];
    const prop = props.find(x => x.name == 'Schema') || { name: 'Schema', expressions: { 'Literal': '', syntax: SyntaxNames.Literal } };
    prop.expressions[SyntaxNames.Literal] = context.data;
    eventBus.emit(EventTypes.UpdateActivity, this, prop);
  }
  onActivityValidating(context) {
    if (context.activityType !== 'HttpEndpoint' || context.prop !== 'Schema')
      return;
    const jsonSchema = context.value;
    let isValid = true;
    if (jsonSchema == '')
      return;
    const ajv = new Ajv();
    let json;
    try {
      json = JSON.parse(jsonSchema);
    }
    catch (e) {
      isValid = false;
    }
    if (json != undefined) {
      try {
        const validate = ajv.compile(json);
        const errors = validate.errors;
        if (errors != null)
          isValid = false;
      }
      catch (e) {
        const err = e;
        isValid = false;
      }
    }
    context.isValidated = true;
    context.isValid = isValid;
    context.data = isValid ? 'Json is valid' : 'Json is invalid';
  }
}

class TimerPlugin {
  constructor() {
    eventBus.on(EventTypes.ActivityDesignDisplaying, this.onActivityDesignDisplaying);
  }
  onActivityDesignDisplaying(context) {
    const activityModel = context.activityModel;
    if (activityModel.type !== 'Timer')
      return;
    const props = activityModel.properties || [];
    const condition = props.find(x => x.name == 'Timeout') || { name: 'Timeout', expressions: { 'Literal': '' }, syntax: 'Literal' };
    const expression = htmlEncode(condition.expressions[condition.syntax] || '');
    context.bodyDisplay = `<p>${expression}</p>`;
  }
}

class WriteLinePlugin {
  constructor() {
    eventBus.on(EventTypes.ActivityDesignDisplaying, this.onActivityDesignDisplaying);
  }
  onActivityDesignDisplaying(context) {
    const activityModel = context.activityModel;
    if (activityModel.type !== 'WriteLine')
      return;
    const props = activityModel.properties || [];
    const condition = props.find(x => x.name == 'Text') || { name: 'Text', expressions: { 'Literal': '' }, syntax: SyntaxNames.Literal };
    const expression = condition.expressions[condition.syntax || 'Literal'] || '';
    context.bodyDisplay = `<p>${htmlEncode(expression)}</p>`;
  }
}

class SendEmailPlugin {
  constructor() {
    eventBus.on(EventTypes.ActivityDesignDisplaying, this.onActivityDesignDisplaying);
  }
  onActivityDesignDisplaying(context) {
    const activityModel = context.activityModel;
    if (activityModel.type !== 'SendEmail')
      return;
    const props = activityModel.properties || [];
    const to = props.find(x => x.name == 'To') || { expressions: { 'Json': '' }, syntax: SyntaxNames.Json };
    const expression = to.expressions[to.syntax || SyntaxNames.Json] || '';
    const description = activityModel.description;
    const bodyText = htmlEncode(description && description.length > 0 ? description : expression);
    context.bodyDisplay = `<p>To: ${bodyText}</p>`;
  }
}

class CheckListDriver {
  display(activity, property) {
    const prop = getOrCreateProperty(activity, property.name);
    return h("elsa-check-list-property", { activityModel: activity, propertyDescriptor: property, propertyModel: prop });
  }
  update(activity, property, form) {
  }
}

class CheckboxDriver {
  display(activity, property) {
    const prop = getOrCreateProperty(activity, property.name);
    return h("elsa-checkbox-property", { activityModel: activity, propertyDescriptor: property, propertyModel: prop });
  }
  update(activity, property, form) {
  }
}

class CodeEditorDriver {
  display(activity, property) {
    const prop = getOrCreateProperty(activity, property.name);
    const options = property.options;
    const editorHeight = this.getEditorHeight(options);
    const syntax = options === null || options === void 0 ? void 0 : options.syntax;
    return h("elsa-script-property", { activityModel: activity, propertyDescriptor: property, propertyModel: prop, "editor-height": editorHeight, syntax: syntax });
  }
  update(activity, property, form) {
    const value = form.get(property.name);
    setActivityModelProperty(activity, property.name, value, "Literal");
  }
  getEditorHeight(options) {
    const editorHeightName = (options === null || options === void 0 ? void 0 : options.editorHeight) || 'Default';
    switch (editorHeightName) {
      case 'Large':
        return '20em';
    }
    return '8em';
  }
}

class DropdownDriver {
  display(activity, property) {
    const prop = getOrCreateProperty(activity, property.name);
    return h("elsa-dropdown-property", { activityModel: activity, propertyDescriptor: property, propertyModel: prop });
  }
  update(activity, property, form) {
  }
}

class JsonDriver {
  display(activity, property) {
    const prop = getOrCreateProperty(activity, property.name);
    return h("elsa-json-property", { activityModel: activity, propertyDescriptor: property, propertyModel: prop });
  }
  update(activity, property, form) {
  }
}

class MultiTextDriver {
  display(activity, property, onUpdated) {
    const prop = getOrCreateProperty(activity, property.name);
    return h("elsa-multi-text-property", { activityModel: activity, propertyDescriptor: property, propertyModel: prop, onValueChange: onUpdated });
  }
  update(activity, property, form) {
  }
}

class MultilineDriver {
  display(activity, property) {
    const prop = getOrCreateProperty(activity, property.name);
    return h("elsa-multi-line-property", { activityModel: activity, propertyDescriptor: property, propertyModel: prop });
  }
  update(activity, property, form) {
  }
}

class NullPropertyDriver {
  display(activity, property) {
    return undefined;
  }
  update(activity, property, form) {
  }
}

class RadioListDriver {
  display(activity, property) {
    const prop = getOrCreateProperty(activity, property.name);
    return h("elsa-radio-list-property", { activityModel: activity, propertyDescriptor: property, propertyModel: prop });
  }
  update(activity, property, form) {
  }
}

class SingleLineDriver {
  display(activity, property) {
    const prop = getOrCreateProperty(activity, property.name);
    return h("elsa-single-line-property", { activityModel: activity, propertyDescriptor: property, propertyModel: prop });
  }
}

class SwitchCaseBuilderDriver {
  display(activity, property) {
    const prop = getOrCreateProperty(activity, property.name);
    return h("elsa-switch-cases-property", { activityModel: activity, propertyDescriptor: property, propertyModel: prop });
  }
  update(activity, property, form) {
  }
}

class DictionaryDriver {
  display(activity, property) {
    const prop = getOrCreateProperty(activity, property.name);
    return h("elsa-dictionary-property", { activityModel: activity, propertyDescriptor: property, propertyModel: prop });
  }
  update(activity, property, form) {
  }
}

class CronExpressionDriver {
  display(activity, property) {
    const prop = getOrCreateProperty(activity, property.name);
    return h("elsa-cron-expression-property", { activityModel: activity, propertyDescriptor: property, propertyModel: prop });
  }
  update(activity, property, form) {
  }
}

class DefaultDriversPlugin {
  constructor() {
    this.addDriver('single-line', () => new SingleLineDriver());
    this.addDriver('multi-line', () => new MultilineDriver());
    this.addDriver('json', () => new JsonDriver());
    this.addDriver('check-list', () => new CheckListDriver());
    this.addDriver('radio-list', () => new RadioListDriver());
    this.addDriver('checkbox', () => new CheckboxDriver());
    this.addDriver('dropdown', () => new DropdownDriver());
    this.addDriver('multi-text', () => new MultiTextDriver());
    this.addDriver('code-editor', () => new CodeEditorDriver());
    this.addDriver('switch-case-builder', () => new SwitchCaseBuilderDriver());
    this.addDriver('dictionary', () => new DictionaryDriver());
    this.addDriver('cron-expression', () => new CronExpressionDriver());
  }
  addDriver(controlType, c) {
    propertyDisplayManager.addDriver(controlType, c);
  }
}

class ActivityIconProviderPlugin {
  constructor() {
    eventBus.on(EventTypes.ActivityDescriptorDisplaying, this.onActivityDescriptorDisplaying);
    eventBus.on(EventTypes.ActivityDesignDisplaying, this.onActivityDesignDisplaying);
  }
  onActivityDescriptorDisplaying(context) {
    const descriptor = context.activityDescriptor;
    const iconEntry = activityIconProvider.getIcon(descriptor.type);
    if (iconEntry)
      context.activityIcon = iconEntry;
  }
  onActivityDesignDisplaying(context) {
    const activityModel = context.activityModel;
    const iconEntry = activityIconProvider.getIcon(activityModel.type);
    if (iconEntry)
      context.activityIcon = iconEntry;
  }
}

class SwitchPlugin {
  constructor() {
    eventBus.on(EventTypes.ActivityDesignDisplaying, this.onActivityDesignDisplaying);
  }
  onActivityDesignDisplaying(context) {
    const activityModel = context.activityModel;
    const activityDescriptor = context.activityDescriptor;
    const propertyDescriptors = activityDescriptor.inputProperties;
    const switchCaseProperties = propertyDescriptors.filter(x => x.uiHint == 'switch-case-builder');
    if (switchCaseProperties.length == 0)
      return;
    let outcomesHash = {};
    const syntax = 'Switch';
    for (const propertyDescriptor of switchCaseProperties) {
      const props = activityModel.properties || [];
      const casesProp = props.find(x => x.name == propertyDescriptor.name) || { expressions: { 'Switch': '' }, syntax: syntax };
      const expression = casesProp.expressions[syntax] || [];
      const cases = !!expression['$values'] ? expression['$values'] : Array.isArray(expression) ? expression : parseJson(expression) || [];
      for (const c of cases)
        outcomesHash[c.name] = true;
    }
    const outcomes = Object.keys(outcomesHash);
    context.outcomes = [...outcomes, 'Default', 'Done'];
  }
}

class WhilePlugin {
  constructor() {
    eventBus.on(EventTypes.ActivityDesignDisplaying, this.onActivityDesignDisplaying);
  }
  onActivityDesignDisplaying(context) {
    const activityModel = context.activityModel;
    if (activityModel.type !== 'While')
      return;
    const props = activityModel.properties || [];
    const condition = props.find(x => x.name == 'Condition') || { name: 'Condition', expressions: { 'Literal': '' }, syntax: 'Literal' };
    const expression = condition.expressions[condition.syntax] || '';
    const description = activityModel.description;
    const bodyText = htmlEncode(description && description.length > 0 ? description : expression);
    context.bodyDisplay = `<p>${bodyText}</p>`;
  }
}

class StartAtPlugin {
  constructor() {
    eventBus.on(EventTypes.ActivityDesignDisplaying, this.onActivityDesignDisplaying);
  }
  onActivityDesignDisplaying(context) {
    const activityModel = context.activityModel;
    if (activityModel.type !== 'StartAt')
      return;
    const props = activityModel.properties || [];
    const condition = props.find(x => x.name == 'Instant') || { name: 'Instant', expressions: { 'Literal': '' }, syntax: 'Literal' };
    const expression = htmlEncode(condition.expressions[condition.syntax] || '');
    context.bodyDisplay = `<p>${expression}</p>`;
  }
}

class CronPlugin {
  constructor() {
    eventBus.on(EventTypes.ActivityDesignDisplaying, this.onActivityDesignDisplaying);
  }
  onActivityDesignDisplaying(context) {
    const activityModel = context.activityModel;
    if (activityModel.type !== 'Cron')
      return;
    const props = activityModel.properties || [];
    const condition = props.find(x => x.name == 'CronExpression') || { name: 'CronExpression', expressions: { 'Literal': '' }, syntax: SyntaxNames.Literal };
    const expression = htmlEncode(condition.expressions[condition.syntax || 'Literal'] || '');
    const cronDescription = cronstrue.toString(expression, { throwExceptionOnParseError: false });
    context.bodyDisplay = `<p style="overflow: hidden;text-overflow: ellipsis;" title="${cronDescription}">${cronDescription}</p>`;
  }
}

class SignalReceivedPlugin {
  constructor() {
    eventBus.on(EventTypes.ActivityDesignDisplaying, this.onActivityDisplaying);
  }
  onActivityDisplaying(context) {
    const activityModel = context.activityModel;
    if (activityModel.type !== 'SignalReceived')
      return;
    const props = activityModel.properties || [];
    const signalName = props.find(x => x.name == 'Signal') || { name: 'Signal', expressions: { 'Literal': '', syntax: SyntaxNames.Literal } };
    const syntax = signalName.syntax || SyntaxNames.Literal;
    const bodyDisplay = htmlEncode(signalName.expressions[syntax]);
    context.bodyDisplay = `<p>${bodyDisplay}</p>`;
  }
}

class SendSignalPlugin {
  constructor() {
    eventBus.on(EventTypes.ActivityDesignDisplaying, this.onActivityDisplaying);
  }
  onActivityDisplaying(context) {
    const activityModel = context.activityModel;
    if (activityModel.type !== 'SendSignal')
      return;
    const props = activityModel.properties || [];
    const signalName = props.find(x => x.name == 'Signal') || { name: 'Signal', expressions: { 'Literal': '', syntax: SyntaxNames.Literal } };
    const syntax = signalName.syntax || SyntaxNames.Literal;
    const bodyDisplay = htmlEncode(signalName.expressions[syntax]);
    context.bodyDisplay = `<p>${bodyDisplay}</p>`;
  }
}

class StatePlugin {
  constructor() {
    eventBus.on(EventTypes.ActivityDesignDisplaying, this.onActivityDesignDisplaying);
  }
  onActivityDesignDisplaying(context) {
    const activityModel = context.activityModel;
    if (activityModel.type !== 'State')
      return;
    const props = activityModel.properties || [];
    const stateNameProp = props.find(x => x.name == 'StateName') || { name: 'Text', expressions: { 'Literal': '' }, syntax: SyntaxNames.Literal };
    context.displayName = htmlEncode(stateNameProp.expressions[stateNameProp.syntax || 'Literal'] || 'State');
  }
}

class SendHttpRequestPlugin {
  constructor() {
    this.onActivityEditorAppearing = (args) => {
      if (args.activityDescriptor.type != 'SendHttpRequest')
        return;
      document.querySelector('#ReadContent').addEventListener('change', this.updateUI);
      document.querySelector('#ResponseContentParserName').addEventListener('change', this.updateUI);
      this.updateUI();
    };
    this.onActivityEditorDisappearing = (args) => {
      if (args.activityDescriptor.type != 'SendHttpRequest')
        return;
      document.querySelector('#ReadContent').removeEventListener('change', this.updateUI);
      document.querySelector('#ResponseContentParserName').removeEventListener('change', this.updateUI);
    };
    this.updateUI = () => {
      const readContentCheckbox = document.querySelector('#ReadContent');
      const parserList = document.querySelector('#ResponseContentParserName');
      const responseContentParserListControl = document.querySelector('#ResponseContentParserNameControl');
      const responseContentTargetTypeControl = document.querySelector('#ResponseContentTargetTypeControl');
      const selectedParserName = parserList.value;
      responseContentParserListControl.classList.toggle('hidden', !readContentCheckbox.checked);
      responseContentTargetTypeControl.classList.toggle('hidden', (!readContentCheckbox.checked || selectedParserName != '.NET Type'));
    };
    eventBus.on(EventTypes.ActivityEditor.Appearing, this.onActivityEditorAppearing);
    eventBus.on(EventTypes.ActivityEditor.Disappearing, this.onActivityEditorDisappearing);
  }
}

class DynamicOutcomesPlugin {
  constructor() {
    this.onActivityDesignDisplaying = (context) => {
      const propValuesAsOutcomes = context.activityDescriptor.inputProperties.filter(prop => prop.considerValuesAsOutcomes);
      if (propValuesAsOutcomes.length > 0) {
        const props = context.activityModel.properties || [];
        const syntax = SyntaxNames.Json;
        let dynamicOutcomes = [];
        props
          .filter(prop => propValuesAsOutcomes.find(desc => desc.name == prop.name) != undefined)
          .forEach(prop => {
          const expression = prop.expressions[syntax] || [];
          const dynamicPropertyOutcomes = !!expression['$values'] ? expression['$values'] : Array.isArray(expression) ? expression : parseJson(expression) || [];
          dynamicOutcomes = [...dynamicOutcomes, ...dynamicPropertyOutcomes];
        });
        context.outcomes = [...dynamicOutcomes, ...context.outcomes];
      }
    };
    eventBus.on(EventTypes.ActivityDesignDisplaying, this.onActivityDesignDisplaying);
  }
}

class PluginManager {
  constructor() {
    this.pluginFactories = [];
    this.createPlugin = (pluginFactory) => pluginFactory(this.elsaStudio);
    this.pluginFactories = [
      () => new DefaultDriversPlugin(),
      () => new ActivityIconProviderPlugin(),
      () => new IfPlugin(),
      () => new WhilePlugin(),
      () => new SwitchPlugin(),
      () => new HttpEndpointPlugin(),
      () => new SendHttpRequestPlugin(),
      () => new TimerPlugin(),
      () => new StartAtPlugin(),
      () => new CronPlugin(),
      () => new SignalReceivedPlugin(),
      () => new SendSignalPlugin(),
      () => new WriteLinePlugin(),
      () => new StatePlugin(),
      () => new SendEmailPlugin(),
      () => new DynamicOutcomesPlugin()
    ];
  }
  initialize(elsaStudio) {
    if (this.initialized)
      return;
    this.elsaStudio = elsaStudio;
    for (const pluginType of this.pluginFactories) {
      this.createPlugin(pluginType);
    }
    this.initialized = true;
  }
  registerPlugins(pluginFactories) {
    for (const pluginFactory of pluginFactories) {
      this.registerPlugin(pluginFactory);
    }
  }
  registerPlugin(pluginType) {
    const factory = () => new pluginType(this.elsaStudio);
    this.registerPluginFactory(factory);
  }
  registerPluginFactory(pluginFactory) {
    this.pluginFactories.push(pluginFactory);
    if (this.initialized)
      this.createPlugin(pluginFactory);
  }
}
const pluginManager = new PluginManager();

class PropertyDisplayManager {
  constructor() {
    this.drivers = {};
  }
  initialize(elsaStudio) {
    if (this.initialized)
      return;
    this.elsaStudio = elsaStudio;
    this.initialized = true;
  }
  addDriver(controlType, driverFactory) {
    this.drivers[controlType] = driverFactory;
  }
  display(model, property, onUpdated) {
    const driver = this.getDriver(property.uiHint);
    return driver.display(model, property, onUpdated);
  }
  update(model, property, form) {
    const driver = this.getDriver(property.uiHint);
    const update = driver.update;
    if (!update)
      return;
    return update(model, property, form);
  }
  getDriver(type) {
    const driverFactory = this.drivers[type] || ((_) => new NullPropertyDriver());
    return driverFactory(this.elsaStudio);
  }
}
const propertyDisplayManager = new PropertyDisplayManager();

class MonacoEditorDialogService {
  constructor() {
    this.monacoEditor = null;
    this.monacoEditorDialog = null;
    this.valueSaved = null;
  }
  show(language, value, onChanged) {
    if (!this.monacoEditor || !this.monacoEditorDialog) {
      return;
    }
    this.currentValue = value;
    this.monacoEditor.language = language;
    this.monacoEditor.setValue(value);
    this.valueSaved = onChanged;
    this.monacoEditorDialog.show();
  }
  save() {
    if (this.valueSaved) {
      this.valueSaved(this.currentValue);
    }
  }
}
const monacoEditorDialogService = new MonacoEditorDialogService();

export { ConfirmDialogService as C, FeaturesDataManager as F, MonacoEditorDialogService as M, PluginManager as P, ToastNotificationService as T, PropertyDisplayManager as a, propertyDisplayManager as b, confirmDialogService as c, featuresDataManager as f, monacoEditorDialogService as m, pluginManager as p, toastNotificationService as t };

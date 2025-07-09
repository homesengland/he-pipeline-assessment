import { B as BUILD, c as consoleDevInfo, p as plt, w as win, H, d as doc, N as NAMESPACE, a as promiseResolve, b as bootstrapLazy } from './index-ea213ee1.js';
import { g as globalScripts } from './app-globals-0f993ce5.js';

/*
 Stencil Client Patch Browser v2.13.0 | MIT Licensed | https://stenciljs.com
 */
const getDynamicImportFunction = (namespace) => `__sc_import_${namespace.replace(/\s|-/g, '_')}`;
const patchBrowser = () => {
    // NOTE!! This fn cannot use async/await!
    if (BUILD.isDev && !BUILD.isTesting) {
        consoleDevInfo('Running in development mode.');
    }
    if (BUILD.cssVarShim) {
        // shim css vars
        plt.$cssShim$ = win.__cssshim;
    }
    if (BUILD.cloneNodeFix) {
        // opted-in to polyfill cloneNode() for slot polyfilled components
        patchCloneNodeFix(H.prototype);
    }
    if (BUILD.profile && !performance.mark) {
        // not all browsers support performance.mark/measure (Safari 10)
        performance.mark = performance.measure = () => {
            /*noop*/
        };
        performance.getEntriesByName = () => [];
    }
    // @ts-ignore
    const scriptElm = BUILD.scriptDataOpts || BUILD.safari10 || BUILD.dynamicImportShim
        ? Array.from(doc.querySelectorAll('script')).find((s) => new RegExp(`\/${NAMESPACE}(\\.esm)?\\.js($|\\?|#)`).test(s.src) ||
            s.getAttribute('data-stencil-namespace') === NAMESPACE)
        : null;
    const importMeta = import.meta.url;
    const opts = BUILD.scriptDataOpts ? scriptElm['data-opts'] || {} : {};
    if (BUILD.safari10 && 'onbeforeload' in scriptElm && !history.scrollRestoration /* IS_ESM_BUILD */) {
        // Safari < v11 support: This IF is true if it's Safari below v11.
        // This fn cannot use async/await since Safari didn't support it until v11,
        // however, Safari 10 did support modules. Safari 10 also didn't support "nomodule",
        // so both the ESM file and nomodule file would get downloaded. Only Safari
        // has 'onbeforeload' in the script, and "history.scrollRestoration" was added
        // to Safari in v11. Return a noop then() so the async/await ESM code doesn't continue.
        // IS_ESM_BUILD is replaced at build time so this check doesn't happen in systemjs builds.
        return {
            then() {
                /* promise noop */
            },
        };
    }
    if (!BUILD.safari10 && importMeta !== '') {
        opts.resourcesUrl = new URL('.', importMeta).href;
    }
    else if (BUILD.dynamicImportShim || BUILD.safari10) {
        opts.resourcesUrl = new URL('.', new URL(scriptElm.getAttribute('data-resources-url') || scriptElm.src, win.location.href)).href;
        if (BUILD.dynamicImportShim) {
            patchDynamicImport(opts.resourcesUrl, scriptElm);
        }
        if (BUILD.dynamicImportShim && !win.customElements) {
            // module support, but no custom elements support (Old Edge)
            // @ts-ignore
            return import(/* webpackChunkName: "polyfills-dom" */ './dom-c5ed0ba5.js').then(() => opts);
        }
    }
    return promiseResolve(opts);
};
const patchDynamicImport = (base, orgScriptElm) => {
    const importFunctionName = getDynamicImportFunction(NAMESPACE);
    try {
        // test if this browser supports dynamic imports
        // There is a caching issue in V8, that breaks using import() in Function
        // By generating a random string, we can workaround it
        // Check https://bugs.chromium.org/p/chromium/issues/detail?id=990810 for more info
        win[importFunctionName] = new Function('w', `return import(w);//${Math.random()}`);
    }
    catch (e) {
        // this shim is specifically for browsers that do support "esm" imports
        // however, they do NOT support "dynamic" imports
        // basically this code is for old Edge, v18 and below
        const moduleMap = new Map();
        win[importFunctionName] = (src) => {
            const url = new URL(src, base).href;
            let mod = moduleMap.get(url);
            if (!mod) {
                const script = doc.createElement('script');
                script.type = 'module';
                script.crossOrigin = orgScriptElm.crossOrigin;
                script.src = URL.createObjectURL(new Blob([`import * as m from '${url}'; window.${importFunctionName}.m = m;`], {
                    type: 'application/javascript',
                }));
                mod = new Promise((resolve) => {
                    script.onload = () => {
                        resolve(win[importFunctionName].m);
                        script.remove();
                    };
                });
                moduleMap.set(url, mod);
                doc.head.appendChild(script);
            }
            return mod;
        };
    }
};
const patchCloneNodeFix = (HTMLElementPrototype) => {
    const nativeCloneNodeFn = HTMLElementPrototype.cloneNode;
    HTMLElementPrototype.cloneNode = function (deep) {
        if (this.nodeName === 'TEMPLATE') {
            return nativeCloneNodeFn.call(this, deep);
        }
        const clonedNode = nativeCloneNodeFn.call(this, false);
        const srcChildNodes = this.childNodes;
        if (deep) {
            for (let i = 0; i < srcChildNodes.length; i++) {
                // Node.ATTRIBUTE_NODE === 2, and checking because IE11
                if (srcChildNodes[i].nodeType !== 2) {
                    clonedNode.appendChild(srcChildNodes[i].cloneNode(true));
                }
            }
        }
        return clonedNode;
    };
};

patchBrowser().then(options => {
  globalScripts();
  return bootstrapLazy(JSON.parse("[[\"elsa-studio-workflow-definitions-edit\",[[0,\"elsa-studio-workflow-definitions-edit\",{\"match\":[16]}]]],[\"elsa-workflow-instance-viewer-screen\",[[0,\"elsa-workflow-instance-viewer-screen\",{\"workflowInstanceId\":[1,\"workflow-instance-id\"],\"serverUrl\":[1,\"server-url\"],\"culture\":[1],\"workflowInstance\":[32],\"workflowBlueprint\":[32],\"workflowModel\":[32],\"selectedActivityId\":[32],\"activityStats\":[32],\"activityContextMenuState\":[32],\"getServerUrl\":[64]},[[8,\"click\",\"onWindowClicked\"]]]]],[\"elsa-studio-workflow-instances-view\",[[0,\"elsa-studio-workflow-instances-view\",{\"match\":[16]}]]],[\"elsa-credential-manager-items-list\",[[0,\"elsa-credential-manager-items-list\",{\"monacoLibPath\":[1,\"monaco-lib-path\"],\"culture\":[1],\"basePath\":[1,\"base-path\"],\"serverUrl\":[1,\"server-url\"]}]]],[\"elsa-studio-workflow-blueprint-view\",[[0,\"elsa-studio-workflow-blueprint-view\",{\"match\":[16]}]]],[\"elsa-studio-workflow-registry\",[[0,\"elsa-studio-workflow-registry\",{\"culture\":[1],\"basePath\":[1,\"base-path\"]}]]],[\"elsa-multi-text-property\",[[0,\"elsa-multi-text-property\",{\"activityModel\":[16],\"propertyDescriptor\":[16],\"propertyModel\":[16],\"serverUrl\":[1025,\"server-url\"],\"currentValue\":[32]}]]],[\"elsa-studio-dashboard\",[[0,\"elsa-studio-dashboard\",{\"culture\":[513],\"basePath\":[513,\"base-path\"]}]]],[\"elsa-studio-workflow-definitions-list\",[[0,\"elsa-studio-workflow-definitions-list\",{\"history\":[16],\"culture\":[1],\"basePath\":[1,\"base-path\"],\"serverUrl\":[1,\"server-url\"]}]]],[\"elsa-studio-webhook-definitions-list\",[[0,\"elsa-studio-webhook-definitions-list\",{\"culture\":[1],\"basePath\":[1,\"base-path\"]}]]],[\"elsa-studio-workflow-instances-list\",[[0,\"elsa-studio-workflow-instances-list\",{\"culture\":[1]}]]],[\"elsa-check-list-property\",[[0,\"elsa-check-list-property\",{\"activityModel\":[16],\"propertyDescriptor\":[16],\"propertyModel\":[16],\"serverUrl\":[1025,\"server-url\"],\"currentValue\":[32]}]]],[\"elsa-checkbox-property\",[[0,\"elsa-checkbox-property\",{\"activityModel\":[16],\"propertyDescriptor\":[16],\"propertyModel\":[16],\"isChecked\":[32]}]]],[\"elsa-cron-expression-property\",[[0,\"elsa-cron-expression-property\",{\"activityModel\":[16],\"propertyDescriptor\":[16],\"propertyModel\":[16],\"currentValue\":[32],\"valueDescription\":[32]}]]],[\"elsa-dictionary-property\",[[0,\"elsa-dictionary-property\",{\"propertyDescriptor\":[16],\"propertyModel\":[16],\"activityModel\":[16],\"serverUrl\":[1025,\"server-url\"],\"currentValue\":[32]}]]],[\"elsa-dropdown-property\",[[0,\"elsa-dropdown-property\",{\"activityModel\":[16],\"propertyDescriptor\":[16],\"propertyModel\":[16],\"serverUrl\":[1025,\"server-url\"],\"currentValue\":[32]}]]],[\"elsa-json-property\",[[0,\"elsa-json-property\",{\"activityModel\":[16],\"propertyDescriptor\":[16],\"propertyModel\":[16],\"currentValue\":[32]}]]],[\"elsa-multi-line-property\",[[0,\"elsa-multi-line-property\",{\"activityModel\":[16],\"propertyDescriptor\":[16],\"propertyModel\":[16],\"currentValue\":[32]}]]],[\"elsa-radio-list-property\",[[0,\"elsa-radio-list-property\",{\"activityModel\":[16],\"propertyDescriptor\":[16],\"propertyModel\":[16],\"serverUrl\":[1025,\"server-url\"],\"currentValue\":[32]}]]],[\"elsa-single-line-property\",[[0,\"elsa-single-line-property\",{\"activityModel\":[16],\"propertyDescriptor\":[16],\"propertyModel\":[16],\"currentValue\":[32]}]]],[\"elsa-activity-editor-panel\",[[0,\"elsa-activity-editor-panel\",{\"culture\":[1],\"workflowStorageDescriptors\":[32],\"activityModel\":[32],\"activityDescriptor\":[32],\"renderProps\":[32],\"updateCounter\":[32]}]]],[\"elsa-studio-root\",[[4,\"elsa-studio-root\",{\"serverUrl\":[513,\"server-url\"],\"monacoLibPath\":[513,\"monaco-lib-path\"],\"culture\":[513],\"basePath\":[513,\"base-path\"],\"useX6Graphs\":[516,\"use-x6-graphs\"],\"features\":[8],\"config\":[1],\"featuresConfig\":[32],\"addPlugins\":[64],\"addPlugin\":[64]},[[0,\"workflow-changed\",\"workflowChangedHandler\"]]]]],[\"elsa-studio-webhook-definitions-edit\",[[0,\"elsa-studio-webhook-definitions-edit\",{\"match\":[16]}]]],[\"elsa-switch-cases-property\",[[0,\"elsa-switch-cases-property\",{\"activityModel\":[16],\"propertyDescriptor\":[16],\"propertyModel\":[16],\"cases\":[32]}]]],[\"elsa-script-property\",[[0,\"elsa-script-property\",{\"activityModel\":[16],\"propertyDescriptor\":[16],\"propertyModel\":[16],\"editorHeight\":[513,\"editor-height\"],\"singleLineMode\":[516,\"single-line\"],\"syntax\":[1],\"serverUrl\":[1025,\"server-url\"],\"workflowDefinitionId\":[1025,\"workflow-definition-id\"],\"currentValue\":[32]}]]],[\"elsa-studio-home\",[[0,\"elsa-studio-home\",{\"culture\":[1],\"serverVersion\":[1,\"server-version\"]}]]],[\"elsa-oauth2-authorized\",[[0,\"elsa-oauth2-authorized\"]]],[\"stencil-async-content\",[[0,\"stencil-async-content\",{\"documentLocation\":[1,\"document-location\"],\"content\":[32]}]]],[\"stencil-route-title\",[[0,\"stencil-route-title\",{\"titleSuffix\":[1,\"title-suffix\"],\"pageTitle\":[1,\"page-title\"]}]]],[\"stencil-router-prompt\",[[0,\"stencil-router-prompt\",{\"when\":[4],\"message\":[1],\"history\":[16],\"unblock\":[32]}]]],[\"stencil-router-redirect\",[[0,\"stencil-router-redirect\",{\"history\":[16],\"root\":[1],\"url\":[1]}]]],[\"elsa-workflow-definition-editor-screen\",[[0,\"elsa-workflow-definition-editor-screen\",{\"workflowDefinitionId\":[513,\"workflow-definition-id\"],\"serverUrl\":[513,\"server-url\"],\"monacoLibPath\":[513,\"monaco-lib-path\"],\"features\":[1],\"culture\":[1],\"basePath\":[1,\"base-path\"],\"serverFeatures\":[16],\"history\":[16],\"workflowDefinition\":[32],\"workflowModel\":[32],\"publishing\":[32],\"unPublishing\":[32],\"unPublished\":[32],\"saving\":[32],\"saved\":[32],\"importing\":[32],\"imported\":[32],\"reverting\":[32],\"reverted\":[32],\"networkError\":[32],\"selectedActivityId\":[32],\"workflowDesignerMode\":[32],\"workflowTestActivityMessages\":[32],\"workflowInstance\":[32],\"workflowInstanceId\":[32],\"activityStats\":[32],\"layoutDirection\":[32],\"activityContextMenuState\":[32],\"activityContextMenuTestState\":[32],\"getServerUrl\":[64],\"getWorkflowDefinitionId\":[64],\"exportWorkflow\":[64],\"importWorkflow\":[64]},[[0,\"workflow-changed\",\"workflowChangedHandler\"],[8,\"click\",\"onWindowClicked\"]]]]],[\"elsa-workflow-blueprint-viewer-screen\",[[0,\"elsa-workflow-blueprint-viewer-screen\",{\"workflowDefinitionId\":[1,\"workflow-definition-id\"],\"serverUrl\":[1,\"server-url\"],\"culture\":[1],\"workflowBlueprint\":[32],\"workflowModel\":[32],\"getServerUrl\":[64]}]]],[\"elsa-workflow-registry-list-screen\",[[0,\"elsa-workflow-registry-list-screen\",{\"history\":[16],\"serverUrl\":[1,\"server-url\"],\"basePath\":[1,\"base-path\"],\"culture\":[1],\"currentProviderName\":[32],\"workflowProviders\":[32],\"workflowBlueprints\":[32],\"currentPage\":[32],\"currentPageSize\":[32],\"currentSearchTerm\":[32]}]]],[\"elsa-workflow-definitions-list-screen\",[[0,\"elsa-workflow-definitions-list-screen\",{\"history\":[16],\"serverUrl\":[1,\"server-url\"],\"culture\":[1],\"basePath\":[1,\"base-path\"],\"workflowDefinitions\":[32],\"currentPage\":[32],\"currentPageSize\":[32],\"currentSearchTerm\":[32],\"loadWorkflowDefinitions\":[64]}]]],[\"elsa-webhook-definitions-list-screen\",[[0,\"elsa-webhook-definitions-list-screen\",{\"history\":[16],\"serverUrl\":[1,\"server-url\"],\"basePath\":[1,\"base-path\"],\"culture\":[1],\"webhookDefinitions\":[32]}]]],[\"elsa-workflow-instance-list-screen\",[[0,\"elsa-workflow-instance-list-screen\",{\"history\":[16],\"serverUrl\":[1,\"server-url\"],\"basePath\":[1,\"base-path\"],\"workflowId\":[1,\"workflow-id\"],\"correlationId\":[1,\"correlation-id\"],\"workflowStatus\":[1,\"workflow-status\"],\"orderBy\":[1,\"order-by\"],\"culture\":[1],\"bulkActions\":[32],\"workflowBlueprints\":[32],\"workflowInstances\":[32],\"selectedWorkflowId\":[32],\"selectedCorrelationId\":[32],\"selectedWorkflowStatus\":[32],\"selectedOrderByState\":[32],\"selectedWorkflowInstanceIds\":[32],\"selectAllChecked\":[32],\"currentPage\":[32],\"currentPageSize\":[32],\"currentSearchTerm\":[32],\"getSelectedWorkflowInstanceIds\":[64],\"refresh\":[64]}]]],[\"elsa-credential-manager-list-screen\",[[0,\"elsa-credential-manager-list-screen\",{\"history\":[16],\"serverUrl\":[1,\"server-url\"],\"basePath\":[1,\"base-path\"],\"culture\":[1],\"secrets\":[32]}]]],[\"elsa-user-context-menu\",[[0,\"elsa-user-context-menu\",{\"serverUrl\":[513,\"serverurl\"],\"menuItemSelected\":[64]}]]],[\"elsa-webhook-definition-editor-screen\",[[0,\"elsa-webhook-definition-editor-screen\",{\"webhookDefinition\":[16],\"webhookId\":[513,\"webhook-definition-id\"],\"serverUrl\":[513,\"server-url\"],\"culture\":[1],\"history\":[16],\"webhookDefinitionInternal\":[32],\"saving\":[32],\"saved\":[32],\"networkError\":[32],\"getServerUrl\":[64],\"getWebhookId\":[64]}]]],[\"elsa-secret-editor-modal\",[[0,\"elsa-secret-editor-modal\",{\"monacoLibPath\":[1,\"monaco-lib-path\"],\"culture\":[1],\"serverUrl\":[1,\"server-url\"],\"secretModel\":[32],\"secretDescriptor\":[32],\"renderProps\":[32],\"preventClosing\":[32],\"updateCounter\":[32]}]]],[\"elsa-secrets-picker-modal\",[[0,\"elsa-secrets-picker-modal\",{\"selectedTrait\":[32],\"selectedCategory\":[32],\"searchText\":[32]}]]],[\"elsa-input-tags\",[[0,\"elsa-input-tags\",{\"fieldName\":[1,\"field-name\"],\"fieldId\":[1,\"field-id\"],\"placeHolder\":[1,\"place-holder\"],\"values\":[16],\"currentValues\":[32]}]]],[\"elsa-input-tags-dropdown\",[[0,\"elsa-input-tags-dropdown\",{\"fieldName\":[1,\"field-name\"],\"fieldId\":[1,\"field-id\"],\"placeHolder\":[1,\"place-holder\"],\"values\":[16],\"dropdownValues\":[16],\"currentValues\":[32]}]]],[\"stencil-route\",[[0,\"stencil-route\",{\"group\":[513],\"componentUpdated\":[16],\"match\":[1040],\"url\":[1],\"component\":[1],\"componentProps\":[16],\"exact\":[4],\"routeRender\":[16],\"scrollTopOffset\":[2,\"scroll-top-offset\"],\"routeViewsUpdated\":[16],\"location\":[16],\"history\":[16],\"historyType\":[1,\"history-type\"]}]]],[\"stencil-route-switch\",[[4,\"stencil-route-switch\",{\"group\":[513],\"scrollTopOffset\":[2,\"scroll-top-offset\"],\"location\":[16],\"routeViewsUpdated\":[16]}]]],[\"stencil-router\",[[4,\"stencil-router\",{\"root\":[1],\"historyType\":[1,\"history-type\"],\"titleSuffix\":[1,\"title-suffix\"],\"scrollTopOffset\":[2,\"scroll-top-offset\"],\"location\":[32],\"history\":[32]}]]],[\"elsa-workflow-instance-journal\",[[0,\"elsa-workflow-instance-journal\",{\"workflowInstanceId\":[1,\"workflow-instance-id\"],\"workflowInstance\":[16],\"serverUrl\":[1,\"server-url\"],\"activityDescriptors\":[16],\"workflowBlueprint\":[16],\"workflowModel\":[16],\"isVisible\":[32],\"records\":[32],\"filteredRecords\":[32],\"selectedRecordId\":[32],\"selectedActivityId\":[32],\"selectedTabId\":[32],\"selectActivityRecord\":[64]}]]],[\"elsa-version-history-panel\",[[0,\"elsa-version-history-panel\",{\"workflowDefinition\":[16],\"serverUrl\":[1,\"server-url\"],\"versions\":[32]}]]],[\"elsa-workflow-test-panel\",[[0,\"elsa-workflow-test-panel\",{\"workflowDefinition\":[16],\"workflowTestActivityId\":[1,\"workflow-test-activity-id\"],\"culture\":[1],\"serverUrl\":[1,\"server-url\"],\"selectedActivityId\":[1,\"selected-activity-id\"],\"hubConnection\":[32],\"workflowTestActivityMessages\":[32],\"workflowStarted\":[32]}]]],[\"elsa-control\",[[0,\"elsa-control\",{\"content\":[1]}]]],[\"elsa-activity-editor-modal\",[[0,\"elsa-activity-editor-modal\",{\"culture\":[1],\"workflowStorageDescriptors\":[32],\"activityModel\":[32],\"activityDescriptor\":[32],\"renderProps\":[32]}]]],[\"elsa-workflow-settings-modal\",[[0,\"elsa-workflow-settings-modal\",{\"serverUrl\":[513,\"server-url\"],\"workflowDefinition\":[16],\"workflowDefinitionInternal\":[32],\"selectedTab\":[32],\"newVariable\":[32]}]]],[\"elsa-activity-picker-modal\",[[0,\"elsa-activity-picker-modal\",{\"selectedTrait\":[32],\"selectedCategory\":[32],\"searchText\":[32]}]]],[\"elsa-webhook-definition-editor-notifications\",[[0,\"elsa-webhook-definition-editor-notifications\"]]],[\"elsa-workflow-blueprint-properties-panel\",[[0,\"elsa-workflow-blueprint-properties-panel\",{\"workflowId\":[1,\"workflow-id\"],\"culture\":[1],\"serverUrl\":[1,\"server-url\"],\"workflowBlueprint\":[32],\"publishedVersion\":[32]}]]],[\"elsa-workflow-properties-panel\",[[0,\"elsa-workflow-properties-panel\",{\"workflowDefinition\":[16],\"culture\":[1],\"serverUrl\":[1,\"server-url\"],\"publishedVersion\":[32],\"expanded\":[32]}]]],[\"elsa-workflow-publish-button\",[[0,\"elsa-workflow-publish-button\",{\"workflowDefinition\":[16],\"publishing\":[4],\"culture\":[1]},[[8,\"click\",\"onWindowClicked\"]]]]],[\"elsa-designer-panel\",[[0,\"elsa-designer-panel\",{\"culture\":[1],\"lastChangeTime\":[32]}]]],[\"elsa-toast-notification\",[[0,\"elsa-toast-notification\",{\"isVisible\":[32],\"title\":[32],\"message\":[32],\"show\":[64],\"hide\":[64]}]]],[\"elsa-workflow-fault-information\",[[0,\"elsa-workflow-fault-information\",{\"workflowFault\":[16],\"faultedAt\":[16]}]]],[\"elsa-workflow-performance-information\",[[0,\"elsa-workflow-performance-information\",{\"activityStats\":[16]}]]],[\"elsa-workflow-definition-editor-notifications\",[[0,\"elsa-workflow-definition-editor-notifications\"]]],[\"elsa-dropdown-button\",[[0,\"elsa-dropdown-button\",{\"text\":[1],\"icon\":[8],\"btnClass\":[1,\"btn-class\"],\"origin\":[2],\"items\":[16]},[[8,\"click\",\"onWindowClicked\"]]]]],[\"elsa-copy-button\",[[0,\"elsa-copy-button\",{\"value\":[1]}]]],[\"elsa-designer-tree\",[[0,\"elsa-designer-tree\",{\"model\":[16],\"selectedActivityIds\":[16],\"activityContextMenuButton\":[16],\"activityBorderColor\":[16],\"activityContextMenu\":[16],\"connectionContextMenu\":[16],\"activityContextTestMenu\":[16],\"mode\":[2],\"layoutDirection\":[1,\"layout-direction\"],\"enableMultipleConnectionsFromSingleSource\":[4,\"enable-multiple-connections\"],\"workflowModel\":[32],\"activityContextMenuState\":[32],\"connectionContextMenuState\":[32],\"activityContextMenuTestState\":[32],\"removeActivity\":[64],\"removeSelectedActivities\":[64],\"showActivityEditor\":[64]}]]],[\"elsa-pager\",[[0,\"elsa-pager\",{\"page\":[2],\"pageSize\":[2,\"page-size\"],\"totalCount\":[2,\"total-count\"],\"location\":[16],\"history\":[16],\"culture\":[1]}]]],[\"x6-designer\",[[0,\"x6-designer\",{\"model\":[16],\"selectedActivityIds\":[16],\"activityContextMenuButton\":[16],\"activityBorderColor\":[16],\"activityContextMenu\":[16],\"connectionContextMenu\":[16],\"activityContextTestMenu\":[16],\"mode\":[2],\"layoutDirection\":[1,\"layout-direction\"],\"enableMultipleConnectionsFromSingleSource\":[4,\"enable-multiple-connections\"],\"workflowModel\":[32],\"activityContextMenuState\":[32],\"connectionContextMenuState\":[32],\"activityContextMenuTestState\":[32],\"removeActivity\":[64],\"removeSelectedActivities\":[64],\"showActivityEditor\":[64],\"updateLayout\":[64]}]]],[\"elsa-flyout-panel\",[[4,\"elsa-flyout-panel\",{\"expandButtonPosition\":[2,\"expand-button-position\"],\"autoExpand\":[4,\"auto-expand\"],\"hidden\":[4],\"silent\":[4],\"updateCounter\":[2,\"update-counter\"],\"expanded\":[32],\"currentTab\":[32],\"selectTab\":[64]}]]],[\"elsa-tab-content\",[[4,\"elsa-tab-content\",{\"tab\":[1],\"active\":[4]}]]],[\"elsa-tab-header\",[[4,\"elsa-tab-header\",{\"tab\":[1],\"active\":[4]}]]],[\"elsa-monaco\",[[0,\"elsa-monaco\",{\"monacoLibPath\":[1,\"monaco-lib-path\"],\"editorHeight\":[513,\"editor-height\"],\"value\":[1],\"language\":[1],\"singleLineMode\":[516,\"single-line\"],\"padding\":[1],\"setValue\":[64],\"addJavaScriptLib\":[64]}]]],[\"elsa-expression-editor\",[[0,\"elsa-expression-editor\",{\"opensModal\":[4,\"opens-modal\"],\"language\":[1],\"expression\":[1],\"editorHeight\":[513,\"editor-height\"],\"singleLineMode\":[516,\"single-line\"],\"padding\":[1],\"context\":[16],\"serverUrl\":[1025,\"server-url\"],\"workflowDefinitionId\":[1025,\"workflow-definition-id\"],\"currentExpression\":[32],\"setExpression\":[64]}]]],[\"elsa-multi-expression-editor\",[[4,\"elsa-multi-expression-editor\",{\"label\":[1],\"fieldName\":[1,\"field-name\"],\"syntax\":[1],\"defaultSyntax\":[1,\"default-syntax\"],\"expressions\":[16],\"supportedSyntaxes\":[16],\"isReadOnly\":[4,\"is-read-only\"],\"editorHeight\":[513,\"editor-height\"],\"singleLineMode\":[516,\"single-line\"],\"context\":[16],\"selectedSyntax\":[32],\"currentValue\":[32]},[[8,\"click\",\"onWindowClicked\"]]]]],[\"elsa-property-editor\",[[4,\"elsa-property-editor\",{\"activityModel\":[16],\"propertyDescriptor\":[16],\"propertyModel\":[16],\"editorHeight\":[513,\"editor-height\"],\"singleLineMode\":[516,\"single-line\"],\"context\":[513],\"showLabel\":[4,\"show-label\"]}]]],[\"stencil-route-link\",[[4,\"stencil-route-link\",{\"url\":[1],\"urlMatch\":[1,\"url-match\"],\"activeClass\":[1,\"active-class\"],\"exact\":[4],\"strict\":[4],\"custom\":[1],\"anchorClass\":[1,\"anchor-class\"],\"anchorRole\":[1,\"anchor-role\"],\"anchorTitle\":[1,\"anchor-title\"],\"anchorTabIndex\":[1,\"anchor-tab-index\"],\"anchorId\":[1,\"anchor-id\"],\"history\":[16],\"location\":[16],\"root\":[1],\"ariaHaspopup\":[1,\"aria-haspopup\"],\"ariaPosinset\":[1,\"aria-posinset\"],\"ariaSetsize\":[2,\"aria-setsize\"],\"ariaLabel\":[1,\"aria-label\"],\"match\":[32]}]]],[\"elsa-modal-dialog\",[[4,\"elsa-modal-dialog\",{\"isVisible\":[32],\"show\":[64],\"hide\":[64]},[[8,\"keydown\",\"handleKeyDown\"]]]]],[\"elsa-confirm-dialog\",[[0,\"elsa-confirm-dialog\",{\"culture\":[1],\"caption\":[32],\"message\":[32],\"show\":[64],\"hide\":[64]}]]],[\"elsa-context-menu\",[[0,\"elsa-context-menu\",{\"history\":[16],\"menuItems\":[16]},[[8,\"click\",\"onWindowClicked\"]]]]],[\"context-consumer\",[[0,\"context-consumer\",{\"context\":[16],\"renderer\":[16],\"subscribe\":[16],\"unsubscribe\":[32]}]]]]"), options);
});

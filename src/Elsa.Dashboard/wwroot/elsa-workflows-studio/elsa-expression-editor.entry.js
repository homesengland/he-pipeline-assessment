import { r as registerInstance, a as createEvent, h } from './index-CL6j2ec2.js';
import { m as monacoEditorDialogService } from './index-fZDMH_YE.js';
import { T as Tunnel } from './workflow-editor-pBAZ9Py8.js';
import { i as iconProvider, I as IconName } from './icon-provider-BX6jwQM3.js';
import { b as createElsaClient } from './elsa-client-q6ob5JPZ.js';
import './event-bus-axQqcjdg.js';
import './index-D7wXd6HU.js';
import './events-CpKc8CLe.js';
import './utils-C0M_5Llz.js';
import './collection-B4sYCr2r.js';
import './_commonjsHelpers-Cf5sKic0.js';
import './cronstrue-BvVNjwUa.js';
import './index-C-8L13GY.js';
import './fetch-client-1OcjQcrw.js';

const ElsaExpressionEditor = class {
    constructor(hostRef) {
        registerInstance(this, hostRef);
        this.expressionChanged = createEvent(this, "expressionChanged", 7);
        this.opensModal = false;
        this.editorHeight = '6em';
        this.singleLineMode = false;
        this.onEditorClick = e => {
            e.preventDefault();
            monacoEditorDialogService.show(this.language, this.currentExpression, (val) => this.setExpression(val));
        };
    }
    expressionChangedHandler(newValue) {
        this.currentExpression = newValue;
    }
    async setExpression(value) {
        await this.monacoEditor.setValue(value);
    }
    async componentWillLoad() {
        this.currentExpression = this.expression;
    }
    async componentDidLoad() {
        const elsaClient = await createElsaClient(this.serverUrl);
        const libSource = await elsaClient.scriptingApi.getJavaScriptTypeDefinitions(this.workflowDefinitionId, this.context);
        const libUri = 'defaultLib:lib.es6.d.ts';
        await this.monacoEditor.addJavaScriptLib(libSource, libUri);
        if (monacoEditorDialogService.monacoEditor) {
            monacoEditorDialogService.monacoEditor.addJavaScriptLib(libSource, libUri);
        }
    }
    async onMonacoValueChanged(e) {
        this.currentExpression = e.value;
        await this.expressionChanged.emit(e.value);
    }
    render() {
        const language = this.language;
        const value = this.currentExpression;
        return (h("elsa-monaco", { key: 'd8f620e8ce1df50f185ebe5378fde113237deba5', value: value, language: language, "editor-height": this.editorHeight, "single-line": this.singleLineMode, padding: this.padding, onValueChanged: e => this.onMonacoValueChanged(e.detail), ref: el => (this.monacoEditor = el) }, this.opensModal &&
            h("button", { key: '0e54fc87427424285edf663f48cdcc7166a7be23', class: "elsa-absolute elsa-z-10", style: { left: "0.25rem", top: "0.35rem" }, onClick: this.onEditorClick }, iconProvider.getIcon(IconName.OpenInDialog))));
    }
    static get watchers() { return {
        "expression": ["expressionChangedHandler"]
    }; }
};
Tunnel.injectProps(ElsaExpressionEditor, ['serverUrl', 'workflowDefinitionId']);

export { ElsaExpressionEditor as elsa_expression_editor };
//# sourceMappingURL=elsa-expression-editor.entry.esm.js.map

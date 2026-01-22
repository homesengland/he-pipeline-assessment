import { r as registerInstance, e as createEvent, h } from './index-1542df5c.js';
import { m as monacoEditorDialogService } from './index-892f713d.js';
import { T as Tunnel } from './workflow-editor-eaa88887.js';
import { i as iconProvider, I as IconName } from './icon-provider-7aea6be9.js';
import { b as createElsaClient } from './elsa-client-8304c78c.js';
import './event-bus-5d6f3774.js';
import './index-1654a48d.js';
import './events-d0aab14a.js';
import './fetch-client-f0dc2a52.js';
import './utils-db96334c.js';
import './collection-ba09a015.js';
import './_commonjsHelpers-6cb8dacb.js';
import './cronstrue-37d55fa1.js';
import './index-2db7bf78.js';

const ElsaExpressionEditor = class {
  constructor(hostRef) {
    registerInstance(this, hostRef);
    this.expressionChanged = createEvent(this, "expressionChanged", 7);
    this.onEditorClick = e => {
      e.preventDefault();
      monacoEditorDialogService.show(this.language, this.currentExpression, (val) => this.setExpression(val));
    };
    this.opensModal = false;
    this.language = undefined;
    this.expression = undefined;
    this.editorHeight = '6em';
    this.singleLineMode = false;
    this.padding = undefined;
    this.context = undefined;
    this.serverUrl = undefined;
    this.workflowDefinitionId = undefined;
    this.currentExpression = undefined;
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
    return (h("elsa-monaco", { value: value, language: language, "editor-height": this.editorHeight, "single-line": this.singleLineMode, padding: this.padding, onValueChanged: e => this.onMonacoValueChanged(e.detail), ref: el => (this.monacoEditor = el) }, this.opensModal &&
      h("button", { class: "elsa-absolute elsa-z-10", style: { left: "0.25rem", top: "0.35rem" }, onClick: this.onEditorClick }, iconProvider.getIcon(IconName.OpenInDialog))));
  }
  static get watchers() { return {
    "expression": ["expressionChangedHandler"]
  }; }
};
Tunnel.injectProps(ElsaExpressionEditor, ['serverUrl', 'workflowDefinitionId']);

export { ElsaExpressionEditor as elsa_expression_editor };

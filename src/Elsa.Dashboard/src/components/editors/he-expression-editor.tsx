import { Component, EventEmitter, h, Method, Prop, State, Watch, Event, Listen } from '@stencil/core';
import { IntellisenseGatherer } from "../../functions/intellisenseGatherer.js";
import Tunnel from '../tunnel/workflow-editor';
import { IntellisenseContext, MonacoValueChangedArgs, HTMLElsaMonacoElement } from "../../models/elsa-interfaces";
import { Uri } from "../../constants/constants";

@Component({
  tag: 'he-expression-editor',
  shadow: false,
})
export class HEExpressionEditor {

  @Event() expressionChanged: EventEmitter<string>;
  @Prop() language: string;
  @Prop() expression: string;
  @Prop({ attribute: 'editor-height', reflect: true }) editorHeight: string = '6em';
  @Prop({ attribute: 'single-line', reflect: true }) singleLineMode: boolean = false;
  @Prop() padding: string;
  @Prop() context?: IntellisenseContext;
  @Prop({ mutable: true }) serverUrl: string;
  @Prop({ mutable: true }) workflowDefinitionId: string;
  @State() currentExpression?: string

  intellisenseGatherer: IntellisenseGatherer;
  monacoEditor: HTMLElsaMonacoElement;

  @Watch("expression")
  expressionChangedHandler(newValue: string) {
    this.currentExpression = newValue;
  }

  @Method()
  async setExpression(value: string) {
    await this.monacoEditor.setValue(value);
  }

  async componentWillLoad() {
    console.log("loading custom editor");
  }

  async componentDidLoad() {
    console.log("Tunnel Provider", Tunnel.Provider);
    console.log("Tunnel Consumer", Tunnel.Consumer);
    console.log("ServerUrl Next");
    console.log("ServerUrl", this.serverUrl);
    console.log("WorkflowDefinitionId", this.workflowDefinitionId);
    this.currentExpression = this.expression;
    this.intellisenseGatherer = new IntellisenseGatherer(this.serverUrl);
    const libSource = await this.intellisenseGatherer.getJavaScriptTypeDefinitions(this.workflowDefinitionId, this.context);
    const libUri = Uri.LibUri;
    await this.monacoEditor.addJavaScriptLib(libSource, libUri);
  }

  async onMonacoValueChanged(e: MonacoValueChangedArgs) {
    this.currentExpression = e.value;
    await this.expressionChanged.emit(e.value);
  }

  render() {
    const language = this.language;
    const value = this.currentExpression;

    return (
      <elsa-monaco value={value}
        language={language}
        editor-height={this.editorHeight}
        single-line={this.singleLineMode}
        padding={this.padding}
        onValueChanged={e => this.onMonacoValueChanged(e.detail)}
        ref={el => this.monacoEditor = el} />
    )
  }
}

Tunnel.injectProps(HEExpressionEditor, ['serverUrl', 'workflowDefinitionId']);

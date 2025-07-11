var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component, h, Method, Prop, State, Watch, Event } from '@stencil/core';
import state from '../../stores/store';
import { IntellisenseGatherer } from "../../utils/intellisenseGatherer";
import Tunnel from '../tunnel/workflow-editor';
import { Uri } from "../../constants/constants";
let HEExpressionEditor = class HEExpressionEditor {
    constructor() {
        this.editorHeight = '6em';
        this.singleLineMode = false;
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
        this.workflowDefinitionId = state.workflowDefinitionId;
        this.currentExpression = this.expression;
        this.intellisenseGatherer = new IntellisenseGatherer();
        let libSource = state.javaScriptTypeDefinitions;
        const libUri = Uri.LibUri;
        await this.monacoEditor.addJavaScriptLib(libSource, libUri);
    }
    async onMonacoValueChanged(e) {
        this.currentExpression = e.value;
        await this.expressionChanged.emit(e.value);
    }
    render() {
        const language = this.language;
        const value = this.currentExpression;
        return (h("he-monaco", { value: value, language: language, "editor-height": this.editorHeight, "single-line": this.singleLineMode, padding: this.padding, onValueChanged: e => this.onMonacoValueChanged(e.detail), ref: el => this.monacoEditor = el }));
    }
};
__decorate([
    Event()
], HEExpressionEditor.prototype, "expressionChanged", void 0);
__decorate([
    Prop()
], HEExpressionEditor.prototype, "language", void 0);
__decorate([
    Prop()
], HEExpressionEditor.prototype, "expression", void 0);
__decorate([
    Prop({ attribute: 'editor-height', reflect: true })
], HEExpressionEditor.prototype, "editorHeight", void 0);
__decorate([
    Prop({ attribute: 'single-line', reflect: true })
], HEExpressionEditor.prototype, "singleLineMode", void 0);
__decorate([
    Prop()
], HEExpressionEditor.prototype, "padding", void 0);
__decorate([
    Prop()
], HEExpressionEditor.prototype, "context", void 0);
__decorate([
    Prop({ mutable: true })
], HEExpressionEditor.prototype, "serverUrl", void 0);
__decorate([
    Prop({ mutable: true })
], HEExpressionEditor.prototype, "workflowDefinitionId", void 0);
__decorate([
    State()
], HEExpressionEditor.prototype, "currentExpression", void 0);
__decorate([
    Watch("expression")
], HEExpressionEditor.prototype, "expressionChangedHandler", null);
__decorate([
    Method()
], HEExpressionEditor.prototype, "setExpression", null);
HEExpressionEditor = __decorate([
    Component({
        tag: 'he-expression-editor',
        shadow: false,
    })
], HEExpressionEditor);
export { HEExpressionEditor };
Tunnel.injectProps(HEExpressionEditor, ['serverUrl', 'workflowDefinitionId']);

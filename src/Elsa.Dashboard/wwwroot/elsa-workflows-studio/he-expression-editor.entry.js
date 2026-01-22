import { h, r as registerInstance, e as createEvent } from './index-1542df5c.js';
import { s as state } from './store-40346019.js';
import { I as IntellisenseGatherer } from './intellisenseGatherer-b418d374.js';
import { U as Uri } from './constants-6ea82f24.js';
import './index-0d4e8807.js';
import './fetch-client-f0dc2a52.js';
import './event-bus-5d6f3774.js';
import './events-d0aab14a.js';
import './auth0-spa-js.production.esm-eb2e28a3.js';

const createProviderConsumer = (defaultState, consumerRender) => {
  let listeners = new Map();
  let currentState = defaultState;
  const updateListener = (fields, instance) => {
    if (Array.isArray(fields)) {
      [...fields].forEach(fieldName => {
        instance[fieldName] = currentState[fieldName];
      });
    }
    else {
      instance[fields] = Object.assign({}, currentState);
    }
  };
  const subscribe = (instance, propList) => {
    if (!listeners.has(instance)) {
      listeners.set(instance, propList);
      updateListener(propList, instance);
    }
    return () => {
      if (listeners.has(instance)) {
        listeners.delete(instance);
      }
    };
  };
  const Provider = ({ state }, children) => {
    currentState = state;
    listeners.forEach(updateListener);
    return children;
  };
  const Consumer = (props, children) => {
    // The casting on subscribe is to allow for crossover through the stencil compiler
    // In the future we should allow for generics in components.
    props = props;
    return consumerRender(subscribe, children[0]);
  };
  const injectProps = (Cstr, fieldList) => {
    const CstrPrototype = Cstr.prototype;
    const cstrConnectedCallback = CstrPrototype.connectedCallback;
    const cstrDisconnectedCallback = CstrPrototype.disconnectedCallback;
    CstrPrototype.connectedCallback = function () {
      subscribe(this, fieldList);
      if (cstrConnectedCallback) {
        return cstrConnectedCallback.call(this);
      }
    };
    CstrPrototype.disconnectedCallback = function () {
      listeners.delete(this);
      if (cstrDisconnectedCallback) {
        cstrDisconnectedCallback.call(this);
      }
    };
  };
  return {
    Provider,
    Consumer,
    injectProps
  };
};

const Tunnel = createProviderConsumer({
  workflowDefinitionId: null,
  serverUrl: null,
  serverFeatures: []
}, (subscribe, child) => (h("context-consumer", { subscribe: subscribe, renderer: child })));

const HEExpressionEditor = class {
  constructor(hostRef) {
    registerInstance(this, hostRef);
    this.expressionChanged = createEvent(this, "expressionChanged", 7);
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
  static get watchers() { return {
    "expression": ["expressionChangedHandler"]
  }; }
};
Tunnel.injectProps(HEExpressionEditor, ['serverUrl', 'workflowDefinitionId']);

export { HEExpressionEditor as he_expression_editor };

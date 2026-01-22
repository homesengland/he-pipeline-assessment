import { r as registerInstance, e as createEvent, h } from './index-1542df5c.js';
import { s as state } from './store-40346019.js';
import { I as IconProvider } from './icon-provider-7131deff.js';
import { H as HePropertyDisplayManager } from './display-manager-6d41e6e8.js';
import { f as filterPropertiesByType, p as parseJson, u as updateNestedActivitiesByDescriptors, d as createQuestionProperty } from './utils-89b7e981.js';
import { S as SyntaxNames } from './constants-6ea82f24.js';
import './index-0d4e8807.js';
import './index-912d1a21.js';

const QuestionProperty = class {
  constructor(hostRef) {
    registerInstance(this, hostRef);
    this.updateQuestionScreen = createEvent(this, "updateQuestionScreen", 7);
    this.syntaxMultiChoiceCount = 0;
    this.dataDictionaryGroup = [];
    this.activityModel = undefined;
    this.questionModel = undefined;
    this.iconProvider = new IconProvider();
    this.currentValue = undefined;
    this.nestedQuestionProperties = undefined;
    this.displayManager = new HePropertyDisplayManager();
  }
  async componentWillLoad() {
    this.getOrCreateQuestionProperties();
    this.dataDictionaryGroup = state.dictionaryGroups;
  }
  async componentWillRender() {
    this.getOrCreateQuestionProperties();
  }
  getOrCreateQuestionProperties() {
    const model = this.questionModel;
    this.questionModel.descriptor = filterPropertiesByType(this.questionModel.descriptor, this.questionModel.ActivityType.nameConstant);
    const propertyJson = model.value.expressions[SyntaxNames.QuestionList];
    if (propertyJson != null && propertyJson != undefined && parseJson(propertyJson).length > 0) {
      const nestedProperties = parseJson(propertyJson);
      this.nestedQuestionProperties = updateNestedActivitiesByDescriptors(this.questionModel.descriptor, nestedProperties, this.questionModel);
    }
    else {
      this.nestedQuestionProperties = this.createQuestionProperties();
    }
  }
  createQuestionProperties() {
    let propertyArray = [];
    const descriptor = this.questionModel.descriptor;
    descriptor.forEach(d => {
      var prop = createQuestionProperty(d, this.questionModel);
      propertyArray.push(prop);
    });
    return propertyArray;
  }
  onPropertyExpressionChange(event, property) {
    event = event;
    property = property;
    this.updateQuestionModel();
  }
  updateQuestionModel() {
    let nestedQuestionsJson = JSON.stringify(this.nestedQuestionProperties);
    this.questionModel.value.expressions[SyntaxNames.QuestionList] = nestedQuestionsJson;
    this.updateQuestionScreen.emit(JSON.stringify(this.questionModel));
  }
  render() {
    const displayManager = this.displayManager;
    const renderPropertyEditor = (property) => {
      var content = displayManager.displayNested(this.activityModel, property, this.onPropertyExpressionChange.bind(this));
      let id = property.descriptor.name + "Category";
      return (h("he-elsa-control", { id: id, content: content, class: "sm:elsa-col-span-6 hydrated" }));
    };
    return (h("div", { class: "elsa-grid elsa-grid-cols-1 elsa-gap-y-6 elsa-gap-x-4 sm:elsa-grid-cols-6" }, this.nestedQuestionProperties.map(renderPropertyEditor)));
  }
};

export { QuestionProperty as question_property };

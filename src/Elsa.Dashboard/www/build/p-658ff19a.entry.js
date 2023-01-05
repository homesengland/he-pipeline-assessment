import { h as e, r as s } from "./p-08a7e18e.js";

import { S as t, T as o, Q as i, I as n } from "./p-e68ae523.js";

class a {
  constructor(e, s, t) {
    this.nameConstant = e, this.displayName = s, this.description = t;
  }
}

const l = {
  Checkbox: new a("CheckboxQuestion", "Checkbox Question", "A question that provides a user with a number of options presented as checkboxes.  A user may multiple values, but you can restrict this by using the 'isSingle' property."),
  Currency: new a("CurrencyQuestion", "Currency Question", "A question that provides a user with a text box to enter a numeric value"),
  Date: new a("DateQuestion", "Date Question", "A question that provides a user with fields to enter a single date."),
  Radio: new a("RadioQuestion", "Radio Question", "A question that provides a user with a number of options presented as radio buttons.  A user may only select a single value."),
  Text: new a("TextQuestion", "Text Question", "A question that provides a user with a free form, one-line text box.")
};

class r {
  constructor(e) {
    this.questionList = new Array, this.questionList.push(...e);
  }
  displayOptions() {
    return this.questionList.map(this.renderOption);
  }
  renderOption(s) {
    return e("option", {
      value: s.nameConstant,
      "data-typeName": s.displayName
    }, s.displayName);
  }
}

const u = class {
  constructor(a) {
    s(this, a), this.supportedSyntaxes = [ t.JavaScript, t.Liquid ], this.syntaxMultiChoiceCount = 0, 
    this.renderChoiceEditor = (s, t) => e("div", {
      id: `${`question-${t}`}-id`,
      class: "accordion elsa-mb-4 elsa-rounded",
      onClick: this.onAccordionQuestionClick
    }, e("button", {
      type: "button elsa-mt-1 elsa-text-m elsa-text-gray-900"
    }, s.title, " "), e("button", {
      type: "button",
      onClick: e => this.onDeleteQuestionClick(e, s),
      class: "elsa-h-5 elsa-w-5 elsa-mx-auto elsa-outline-none focus:elsa-outline-none trashcan-icon",
      style: {
        float: "right"
      }
    }, e(o, {
      options: this.iconProvider.getOptions()
    })), e("p", {
      class: "elsa-mt-1 elsa-text-sm elsa-text-gray-900"
    }, s.questionTypeName), this.renderQuestionComponent(s)), this.activityModel = void 0, 
    this.propertyDescriptor = void 0, this.propertyModel = void 0, this.questionModel = new i, 
    this.iconProvider = new n, this.questionProvider = new r(Object.values(l));
  }
  getQuestion(e) {
    e.detail && this.updateQuestion(e.detail);
  }
  async componentWillLoad() {
    const e = this.propertyModel.expressions[t.Json];
    this.questionModel = function(e) {
      if (!e) return null;
      try {
        return JSON.parse(e);
      } catch (e) {
        console.warn(`Error parsing JSON: ${e}`);
      }
    }(e) || this.defaultActivityModel();
  }
  defaultActivityModel() {
    var e = new i;
    return e.questions = [], e;
  }
  updatePropertyModel() {
    this.propertyModel.expressions[t.Json] = JSON.stringify(this.questionModel);
  }
  updateQuestion(e) {
    let s = this.questionModel.questions.findIndex((s => s.id === e.id)), t = this.questionModel;
    t.questions[s] = e, this.questionModel.questions.map((s => s.id != e.id)), this.questionModel = Object.assign(Object.assign({}, this.questionModel), {
      questions: t.questions
    }), this.updatePropertyModel();
  }
  handleAddQuestion(e) {
    let s = e.currentTarget.value.trim(), t = e.currentTarget.selectedOptions[0].dataset.typename;
    if (null != s && "" != s) {
      this.onAddQuestion(s, t), e.currentTarget.selectedIndex = 0;
    }
  }
  onAddQuestion(e, s) {
    const t = `Question ${this.questionModel.questions.length + 1}`;
    const o = {
      id: `${this.questionModel.questions.length + 1}`,
      title: t,
      questionGuidance: "",
      questionText: "",
      displayComments: !1,
      questionHint: "",
      questionType: e,
      questionTypeName: s
    };
    this.questionModel = Object.assign(Object.assign({}, this.questionModel), {
      questions: [ ...this.questionModel.questions, o ]
    }), this.updatePropertyModel();
  }
  onDeleteQuestionClick(e, s) {
    e.stopPropagation(), this.questionModel = Object.assign(Object.assign({}, this.questionModel), {
      questions: this.questionModel.questions.filter((e => e != s))
    }), this.updatePropertyModel();
  }
  onAccordionQuestionClick(e) {
    let s = e.currentTarget;
    s.classList.toggle("active");
    let t = s.querySelector(".panel");
    "block" === t.style.display ? t.style.display = "none" : t.style.display = "block";
  }
  renderQuestions(e) {
    return e.questions.map(this.renderChoiceEditor);
  }
  renderQuestionComponent(s) {
    switch (s.questionType) {
     case "CheckboxQuestion":
      return e("elsa-checkbox-question", {
        onClick: e => e.stopPropagation(),
        class: "panel elsa-rounded",
        question: s
      });

     case "RadioQuestion":
      return e("elsa-radio-question", {
        onClick: e => e.stopPropagation(),
        class: "panel elsa-rounded",
        question: s
      });

     default:
      return e("elsa-question", {
        onClick: e => e.stopPropagation(),
        class: "panel elsa-rounded",
        question: s
      });
    }
  }
  renderQuestionField(s, t, o, i, n) {
    return e("div", null, e("div", {
      class: "elsa-mb-1 elsa-mt-2"
    }, e("div", {
      class: "elsa-flex"
    }, e("div", {
      class: "elsa-flex-1"
    }, e("label", {
      htmlFor: s,
      class: "elsa-block elsa-text-sm elsa-font-medium elsa-text-gray-700"
    }, t)))), e("input", {
      type: "text",
      id: s,
      name: s,
      value: o,
      onChange: e => n(e, i),
      class: "disabled:elsa-opacity-50 disabled:elsa-cursor-not-allowed focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300"
    }));
  }
  renderCheckboxField(s, t, o, i, n) {
    return e("div", null, e("div", {
      class: "elsa-mb-1 elsa-mt-2"
    }, e("div", {
      class: "elsa-flex"
    }, e("div", null, e("input", {
      id: s,
      name: s,
      type: "checkbox",
      checked: o,
      value: "true",
      onChange: e => n(e, i),
      class: "focus:elsa-ring-blue-500 elsa-h-8 elsa-w-8 elsa-text-blue-600 elsa-border-gray-300 elsa-rounded"
    })), e("div", null, e("label", {
      htmlFor: s,
      class: "elsa-block elsa-text-sm elsa-font-medium elsa-text-gray-700 elsa-p-1"
    }, t)))));
  }
  render() {
    return e("div", null, e("div", {
      class: "elsa-mb-1"
    }, e("div", {
      class: "elsa-flex"
    }, e("div", {
      class: "elsa-flex-1"
    }, e("label", {
      htmlFor: "Questions",
      class: "elsa-block elsa-text-sm elsa-font-medium elsa-text-gray-700"
    }, "List of questions")))), this.renderQuestions(this.questionModel), e("select", {
      id: "addQuestionDropdown",
      onChange: e => this.handleAddQuestion.bind(this)(e),
      name: "addQuestionDropdown",
      class: "elsa-mt-1 elsa-block focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-w-full elsa-shadow-sm sm:elsa-max-w-xs sm:elsa-text-sm elsa-border-gray-300 elsa-rounded-md"
    }, e("option", {
      value: ""
    }, "Add a Question..."), this.questionProvider.displayOptions()));
  }
};

export { u as elsa_question_screen }
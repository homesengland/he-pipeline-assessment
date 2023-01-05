import { h as e, r as s, c as t } from "./p-08a7e18e.js";

import { a as i, S as n, I as l, T as a } from "./p-e68ae523.js";

const PlusIcon = s => e("svg", {
  class: `-elsa-ml-1 elsa-mr-2 elsa-h-5 elsa-w-5 ${(null == s ? void 0 : s.color) ? `elsa-text-${s.color}-500` : ""} ${(null == s ? void 0 : s.hoverColor) ? `hover:elsa-text-${s.hoverColor}-500` : ""}`,
  width: "24",
  height: "24",
  viewBox: "0 0 24 24",
  "stroke-width": "2",
  stroke: "currentColor",
  fill: "transparent",
  "stroke-linecap": "round",
  "stroke-linejoin": "round"
}, e("path", {
  stroke: "none",
  d: "M0 0h24v24H0z"
}), e("line", {
  x1: "12",
  y1: "5",
  x2: "12",
  y2: "19"
}), e("line", {
  x1: "5",
  y1: "12",
  x2: "19",
  y2: "12"
}));

class o {
  constructor(e, s) {
    this.onTitleChanged = e => {
      let s = this.question;
      s.title = e.currentTarget.value.trim(), this.emitter.emit(s);
    }, this.onIdentifierChanged = e => {
      let s = this.question;
      s.id = e.currentTarget.value.trim(), this.emitter.emit(s);
    }, this.onQuestionChanged = e => {
      let s = this.question;
      s.questionText = e.currentTarget.value.trim(), this.emitter.emit(s);
    }, this.onGuidanceChanged = e => {
      let s = this.question;
      s.questionGuidance = e.currentTarget.value.trim(), this.emitter.emit(s);
    }, this.onHintChanged = e => {
      let s = this.question;
      s.questionHint = e.currentTarget.value.trim(), this.emitter.emit(s);
    }, this.onDisplayCommentsBox = e => {
      let s = this.question;
      console.log(e), console.log(e.target), console.log(e.currentTarget);
      const t = e.currentTarget;
      s.displayComments = t.checked, this.emitter.emit(s);
    }, this.question = e, this.emitter = s;
  }
}

class r extends o {
  constructor(e, s) {
    super(e, s), this.onAddChoiceClick = () => {
      const e = `Choice ${this.question.options.choices.length + 1}`, s = this.getChoice(e);
      let t = Object.assign(Object.assign({}, this.question.options), {
        choices: [ ...this.question.options.choices, s ]
      });
      this.question = Object.assign(Object.assign({}, this.question), {
        options: t
      }), this.assignOptions(t), this.emitter.emit(this.question);
    }, this.onChoiceNameChanged = (e, s) => {
      s.answer = e.currentTarget.value.trim(), this.emitter.emit(this.question);
    }, this.onDeleteChoiceClick = e => {
      //The below filter method has to convert and compare a stringified version, as the options property is mapped from the actual implmeneted property
      //i.e. Radio or Checkbox.  Because of this the objects are not equal in reference, and the filter would fail.
      let s = Object.assign(Object.assign({}, this.question.options), {
        choices: this.question.options.choices.filter((s => JSON.stringify(s) != JSON.stringify(e)))
      });
      this.question = Object.assign(Object.assign({}, this.question), {
        options: s
      }), this.assignOptions(s), this.emitter.emit(this.question);
    }, this.question.options || (this.question.options = new i);
  }
}

class d extends o {
  constructor(e, s) {
    super(e, s);
  }
}

class u extends r {
  constructor(e, s) {
    super(e, s), this.onCheckChanged = (e, s) => {
      const t = e.target;
      s.isSingle = t.checked, this.emitter.emit(this.question);
    };
  }
  getChoice(e) {
    return {
      answer: e,
      isSingle: !1
    };
  }
  assignOptions(e) {
    this.question.checkbox = e;
  }
}

class h extends r {
  constructor(e, s) {
    super(e, s), this.assignOptions = e => {
      this.question.radio = e;
    };
  }
  getChoice(e) {
    return {
      answer: e
    };
  }
}

const c = class {
  constructor(e) {
    s(this, e), this.updateQuestion = t(this, "updateQuestion", 7), this.supportedSyntaxes = [ n.JavaScript, n.Liquid ], 
    this.syntaxMultiChoiceCount = 0, this.question = void 0, this.iconProvider = new l;
  }
  async componentWillLoad() {
    this.question && !this.question.checkbox && (this.question.checkbox = new i), this.handler = new u(this.question, this.updateQuestion);
  }
  renderQuestionField(s, t, i, n) {
    return e("div", null, e("div", {
      class: "elsa-mb-1"
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
      value: i,
      onChange: e => {
        n.bind(this)(e);
      },
      class: "disabled:elsa-opacity-50 disabled:elsa-cursor-not-allowed focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300"
    }));
  }
  renderCheckboxField(s, t, i, n) {
    return e("div", null, e("div", {
      class: "elsa-mb-1 elsa-mt-2"
    }, e("div", {
      class: "elsa-flex"
    }, e("div", null, e("label", {
      htmlFor: s,
      class: "elsa-block elsa-text-sm elsa-font-medium elsa-text-gray-700 elsa-p-1"
    }, t)), e("div", null, e("input", {
      id: s,
      name: s,
      type: "checkbox",
      checked: i,
      value: "true",
      onChange: e => n.bind(this)(e),
      class: "focus:elsa-ring-blue-500 elsa-h-8 elsa-w-8 elsa-text-blue-600 elsa-border-gray-300 elsa-rounded"
    })))));
  }
  render() {
    const s = `question-${this.question.id}`;
    return e("div", null, this.renderQuestionField(`${s}-questionid`, "Identifier", this.question.id, this.handler.onIdentifierChanged), this.renderQuestionField(`${s}-title`, "Title", this.question.title, this.handler.onTitleChanged), this.renderQuestionField(`${s}-questionText`, "Question", this.question.questionText, this.handler.onQuestionChanged), this.renderQuestionField(`${s}-questionHint`, "Hint", this.question.questionHint, this.handler.onHintChanged), this.renderQuestionField(`${s}-questionGuidance`, "Guidance", this.question.questionGuidance, this.handler.onGuidanceChanged), this.renderCheckboxField(`${s}-displayCommentBox`, "Display Comments", this.question.displayComments, this.handler.onDisplayCommentsBox), e("div", null, e("table", {
      class: "elsa-min-w-full elsa-divide-y elsa-divide-gray-200"
    }, e("thead", {
      class: "elsa-bg-gray-50"
    }, e("tr", null, e("th", {
      class: "elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-10/12"
    }, "Answer"), e("th", {
      class: "elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-1/12"
    }, "IsSingle"), e("th", {
      class: "elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-1/12"
    }, " "))), e("tbody", null, this.question.checkbox.choices.map(((s, t) => {
      const i = `choice-${t}`;
      let n = s.isSingle;
      return e("tr", {
        key: `choice-${t}`
      }, e("td", {
        class: "elsa-py-2 elsa-pr-5"
      }, e("input", {
        type: "text",
        value: s.answer,
        onChange: e => this.handler.onChoiceNameChanged.bind(this)(e, s),
        class: "focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300"
      })), e("td", {
        class: "elsa-py-0"
      }, e("input", {
        id: `${i}-id`,
        name: "choice_input",
        type: "checkbox",
        checked: n,
        value: "true",
        onChange: e => this.handler.onCheckChanged.bind(this)(e, s),
        class: "focus:elsa-ring-blue-500 elsa-h-8 elsa-w-8 elsa-text-blue-600 elsa-border-gray-300 elsa-rounded"
      })), e("td", {
        class: "elsa-pt-1 elsa-pr-2 elsa-text-right"
      }, e("button", {
        type: "button",
        onClick: () => this.handler.onDeleteChoiceClick.bind(this)(s),
        class: "elsa-h-5 elsa-w-5 elsa-mx-auto elsa-outline-none focus:elsa-outline-none"
      }, e(a, {
        options: this.iconProvider.getOptions()
      }))));
    })))), e("button", {
      type: "button",
      onClick: () => this.handler.onAddChoiceClick.bind(this)(),
      class: "elsa-inline-flex elsa-items-center elsa-px-4 elsa-py-2 elsa-border elsa-border-transparent elsa-shadow-sm elsa-text-sm elsa-font-medium elsa-rounded-md elsa-text-white elsa-bg-blue-600 hover:elsa-bg-blue-700 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 elsa-mt-2"
    }, e(PlusIcon, {
      options: this.iconProvider.getOptions()
    }), "Add Choice")));
  }
}, m = class {
  constructor(e) {
    s(this, e), this.updateQuestion = t(this, "updateQuestion", 7), this.supportedSyntaxes = [ n.JavaScript, n.Liquid ], 
    this.syntaxMultiChoiceCount = 0, this.question = void 0, this.iconProvider = new l;
  }
  async componentWillLoad() {
    this.handler = new d(this.question, this.updateQuestion);
  }
  renderQuestionField(s, t, i, n) {
    return e("div", null, e("div", {
      class: "elsa-mb-1"
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
      value: i,
      onChange: e => {
        n.bind(this)(e);
      },
      class: "disabled:elsa-opacity-50 disabled:elsa-cursor-not-allowed focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300"
    }));
  }
  renderCheckboxField(s, t, i, n) {
    return e("div", null, e("div", {
      class: "elsa-mb-1 elsa-mt-2"
    }, e("div", {
      class: "elsa-flex"
    }, e("div", null, e("label", {
      htmlFor: s,
      class: "elsa-block elsa-text-sm elsa-font-medium elsa-text-gray-700 elsa-p-1"
    }, t)), e("div", null, e("input", {
      id: s,
      name: s,
      type: "checkbox",
      checked: i,
      value: "true",
      onChange: e => n.bind(this)(e),
      class: "focus:elsa-ring-blue-500 elsa-h-8 elsa-w-8 elsa-text-blue-600 elsa-border-gray-300 elsa-rounded"
    })))));
  }
  render() {
    const s = `question-${this.question.id}`;
    return e("div", null, this.renderQuestionField(`${s}-questionid`, "Identifier", this.question.id, this.handler.onIdentifierChanged), this.renderQuestionField(`${s}-title`, "Title", this.question.title, this.handler.onTitleChanged), this.renderQuestionField(`${s}-questionText`, "Question", this.question.questionText, this.handler.onQuestionChanged), this.renderQuestionField(`${s}-questionHint`, "Hint", this.question.questionHint, this.handler.onHintChanged), this.renderQuestionField(`${s}-questionGuidance`, "Guidance", this.question.questionGuidance, this.handler.onGuidanceChanged), this.renderCheckboxField(`${s}-displayCommentBox`, "Display Comments", this.question.displayComments, this.handler.onDisplayCommentsBox));
  }
}, g = class {
  constructor(e) {
    s(this, e), this.updateQuestion = t(this, "updateQuestion", 7), this.supportedSyntaxes = [ n.JavaScript, n.Liquid ], 
    this.syntaxMultiChoiceCount = 0, this.question = void 0, this.iconProvider = new l;
  }
  async componentWillLoad() {
    this.question && !this.question.radio && (this.question.radio = new i), this.handler = new h(this.question, this.updateQuestion);
  }
  renderQuestionField(s, t, i, n) {
    return e("div", null, e("div", {
      class: "elsa-mb-1"
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
      value: i,
      onChange: e => {
        n.bind(this)(e);
      },
      class: "disabled:elsa-opacity-50 disabled:elsa-cursor-not-allowed focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300"
    }));
  }
  renderCheckboxField(s, t, i, n) {
    return e("div", null, e("div", {
      class: "elsa-mb-1 elsa-mt-2"
    }, e("div", {
      class: "elsa-flex"
    }, e("div", null, e("label", {
      htmlFor: s,
      class: "elsa-block elsa-text-sm elsa-font-medium elsa-text-gray-700 elsa-p-1"
    }, t)), e("div", null, e("input", {
      id: s,
      name: s,
      type: "checkbox",
      checked: i,
      value: "true",
      onChange: e => n.bind(this)(e, this.question),
      class: "focus:elsa-ring-blue-500 elsa-h-8 elsa-w-8 elsa-text-blue-600 elsa-border-gray-300 elsa-rounded"
    })))));
  }
  render() {
    const s = `question-${this.question.id}`;
    return e("div", null, this.renderQuestionField(`${s}-questionid`, "Identifier", this.question.id, this.handler.onIdentifierChanged), this.renderQuestionField(`${s}-title`, "Title", this.question.title, this.handler.onTitleChanged), this.renderQuestionField(`${s}-questionText`, "Question", this.question.questionText, this.handler.onQuestionChanged), this.renderQuestionField(`${s}-questionHint`, "Hint", this.question.questionHint, this.handler.onHintChanged), this.renderQuestionField(`${s}-questionGuidance`, "Guidance", this.question.questionGuidance, this.handler.onGuidanceChanged), this.renderCheckboxField(`${s}-displayCommentBox`, "Display Comments", this.question.displayComments, this.handler.onDisplayCommentsBox), e("div", null, e("table", {
      class: "elsa-min-w-full elsa-divide-y elsa-divide-gray-200"
    }, e("thead", {
      class: "elsa-bg-gray-50"
    }, e("tr", null, e("th", {
      class: "elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-10/12"
    }, "Answer"), e("th", {
      class: "elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-1/12"
    }, " "))), e("tbody", null, this.question.radio.choices.map(((s, t) => e("tr", {
      key: `choice-${t}`
    }, e("td", {
      class: "elsa-py-2 elsa-pr-5"
    }, e("input", {
      type: "text",
      value: s.answer,
      onChange: e => this.handler.onChoiceNameChanged.bind(this)(e, s),
      class: "focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300"
    })), e("td", {
      class: "elsa-pt-1 elsa-pr-2 elsa-text-right"
    }, e("button", {
      type: "button",
      onClick: () => this.handler.onDeleteChoiceClick.bind(this)(s),
      class: "elsa-h-5 elsa-w-5 elsa-mx-auto elsa-outline-none focus:elsa-outline-none"
    }, e(a, {
      options: this.iconProvider.getOptions()
    })))))))), e("button", {
      type: "button",
      onClick: () => this.handler.onAddChoiceClick.bind(this)(),
      class: "elsa-inline-flex elsa-items-center elsa-px-4 elsa-py-2 elsa-border elsa-border-transparent elsa-shadow-sm elsa-text-sm elsa-font-medium elsa-rounded-md elsa-text-white elsa-bg-blue-600 hover:elsa-bg-blue-700 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 elsa-mt-2"
    }, e(PlusIcon, {
      options: this.iconProvider.getOptions()
    }), "Add Choice")));
  }
};

export { c as elsa_checkbox_question, m as elsa_question, g as elsa_radio_question }
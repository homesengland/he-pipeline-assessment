import { h as o } from "./p-08a7e18e.js";

const TrashCanIcon = e => o("svg", {
  class: `elsa-h-5 elsa-w-5 ${(null == e ? void 0 : e.color) ? `elsa-text-${e.color}-500` : ""} ${(null == e ? void 0 : e.hoverColor) ? `hover:elsa-text-${e.hoverColor}-500` : ""}`,
  width: "24",
  height: "24",
  viewBox: "0 0 24 24",
  "stroke-width": "2",
  stroke: "currentColor",
  fill: "transparent",
  "stroke-linecap": "round",
  "stroke-linejoin": "round"
}, o("polyline", {
  points: "3 6 5 6 21 6"
}), o("path", {
  d: "M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6m3 0V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2"
}), o("line", {
  x1: "10",
  y1: "11",
  x2: "10",
  y2: "17"
}), o("line", {
  x1: "14",
  y1: "11",
  x2: "14",
  y2: "17"
}));

class e {}

e.Literal = "Literal", e.JavaScript = "JavaScript", e.Liquid = "Liquid", e.Json = "Json";

///Question Options
class r {
  constructor() {
    this.choices = [];
  }
}

class l {
  constructor() {
    this.questions = [];
  }
}

var t, s;

!function(o) {
  o.Plus = "plus", o.TrashBinOutline = "trash-bin-outline";
}(t || (t = {})), function(o) {
  o.Blue = "blue", o.Gray = "gray", o.Green = "green", o.Red = "red", o.Default = "currentColor";
}(s || (s = {}));

class n {
  constructor() {
    this.map = {
      plus: e => o("svg", {
        class: `-elsa-ml-1 elsa-mr-2 elsa-h-5 elsa-w-5 ${(null == e ? void 0 : e.color) ? `elsa-text-${e.color}-500` : ""} ${(null == e ? void 0 : e.hoverColor) ? `hover:elsa-text-${e.hoverColor}-500` : ""}`,
        width: "24",
        height: "24",
        viewBox: "0 0 24 24",
        "stroke-width": "2",
        stroke: "currentColor",
        fill: "transparent",
        "stroke-linecap": "round",
        "stroke-linejoin": "round"
      }, o("path", {
        stroke: "none",
        d: "M0 0h24v24H0z"
      }), o("line", {
        x1: "12",
        y1: "5",
        x2: "12",
        y2: "19"
      }), o("line", {
        x1: "5",
        y1: "12",
        x2: "19",
        y2: "12"
      })),
      "trash-bin-outline": e => o("svg", {
        class: `elsa-h-5 elsa-w-5 ${(null == e ? void 0 : e.color) ? `elsa-text-${e.color}-500` : ""} ${(null == e ? void 0 : e.hoverColor) ? `hover:elsa-text-${e.hoverColor}-500` : ""}`,
        width: "24",
        height: "24",
        viewBox: "0 0 24 24",
        "stroke-width": "2",
        stroke: "currentColor",
        fill: "transparent",
        "stroke-linecap": "round",
        "stroke-linejoin": "round"
      }, o("polyline", {
        points: "3 6 5 6 21 6"
      }), o("path", {
        d: "M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6m3 0V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2"
      }), o("line", {
        x1: "10",
        y1: "11",
        x2: "10",
        y2: "17"
      }), o("line", {
        x1: "14",
        y1: "11",
        x2: "14",
        y2: "17"
      }))
    };
  }
  getIcon(o, e) {
    const r = this.map[o];
    if (r) return r(e);
  }
  getOptions(o) {
    return o;
  }
}

export { n as I, l as Q, e as S, TrashCanIcon as T, r as a }
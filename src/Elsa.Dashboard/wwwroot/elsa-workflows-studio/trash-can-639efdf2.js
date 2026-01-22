import { h } from './index-1542df5c.js';

const TrashCanIcon = (options) => {
  return (h("svg", { class: `elsa-h-5 elsa-w-5 ${(options === null || options === void 0 ? void 0 : options.color) ? `elsa-text-${options.color}-500` : ''} ${(options === null || options === void 0 ? void 0 : options.hoverColor) ? `hover:elsa-text-${options.hoverColor}-500` : ''}`, width: "24", height: "24", viewBox: "0 0 24 24", "stroke-width": "2", stroke: "currentColor", fill: "transparent", "stroke-linecap": "round", "stroke-linejoin": "round" },
    h("polyline", { points: "3 6 5 6 21 6" }),
    h("path", { d: "M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6m3 0V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2" }),
    h("line", { x1: "10", y1: "11", x2: "10", y2: "17" }),
    h("line", { x1: "14", y1: "11", x2: "14", y2: "17" })));
};

export { TrashCanIcon as T };

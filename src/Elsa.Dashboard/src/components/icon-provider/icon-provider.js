import { h } from '@stencil/core';
export var IconName;
(function (IconName) {
    IconName["Plus"] = "plus";
    IconName["TrashBinOutline"] = "trash-bin-outline";
})(IconName || (IconName = {}));
var IconColor;
(function (IconColor) {
    IconColor["Blue"] = "blue";
    IconColor["Gray"] = "gray";
    IconColor["Green"] = "green";
    IconColor["Red"] = "red";
    IconColor["Default"] = "currentColor";
})(IconColor || (IconColor = {}));
export class IconProvider {
    constructor() {
        this.map = {
            'plus': (options) => h("svg", { class: `-elsa-ml-1 elsa-mr-2 elsa-h-5 elsa-w-5 ${(options === null || options === void 0 ? void 0 : options.color) ? `elsa-text-${options.color}-500` : ''} ${(options === null || options === void 0 ? void 0 : options.hoverColor) ? `hover:elsa-text-${options.hoverColor}-500` : ''}`, width: "24", height: "24", viewBox: "0 0 24 24", "stroke-width": "2", stroke: "currentColor", fill: "transparent", "stroke-linecap": "round", "stroke-linejoin": "round" },
                h("path", { stroke: "none", d: "M0 0h24v24H0z" }),
                h("line", { x1: "12", y1: "5", x2: "12", y2: "19" }),
                h("line", { x1: "5", y1: "12", x2: "19", y2: "12" })),
            'trash-bin-outline': (options) => h("svg", { class: `elsa-h-5 elsa-w-5 ${(options === null || options === void 0 ? void 0 : options.color) ? `elsa-text-${options.color}-500` : ''} ${(options === null || options === void 0 ? void 0 : options.hoverColor) ? `hover:elsa-text-${options.hoverColor}-500` : ''}`, width: "24", height: "24", viewBox: "0 0 24 24", "stroke-width": "2", stroke: "currentColor", fill: "transparent", "stroke-linecap": "round", "stroke-linejoin": "round" },
                h("polyline", { points: "3 6 5 6 21 6" }),
                h("path", { d: "M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6m3 0V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2" }),
                h("line", { x1: "10", y1: "11", x2: "10", y2: "17" }),
                h("line", { x1: "14", y1: "11", x2: "14", y2: "17" }))
        };
    }
    getIcon(name, options) {
        const provider = this.map[name];
        if (!provider)
            return undefined;
        return provider(options);
    }
}

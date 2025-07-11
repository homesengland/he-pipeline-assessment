import { h } from '@stencil/core';
const ExpandIcon = (options) => {
    return (h("svg", { class: `elsa-h-5 elsa-w-5 ${(options === null || options === void 0 ? void 0 : options.color) ? `elsa-text-${options.color}-500` : ''} ${(options === null || options === void 0 ? void 0 : options.hoverColor) ? `hover:elsa-text-${options.hoverColor}-500` : ''}`, width: "24px", height: "24px", viewBox: "0 0 24 24", xmlns: "http://www.w3.org/2000/svg" },
        h("path", { fill: "none", stroke: "#000", "stroke-width": "2", d: "M10,14 L2,22 M1,15 L1,23 L9,23 M22,2 L14,10 M15,1 L23,1 L23,9" })));
};
export default ExpandIcon;

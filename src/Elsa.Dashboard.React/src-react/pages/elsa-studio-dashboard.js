import { jsx as _jsx, jsxs as _jsxs } from "react/jsx-runtime";
import { Component } from 'react';
class ElsaStudioDashboard extends Component {
    async componentWillLoad() {
    }
    render() {
        const logoPath = '/assets/logo.png';
        return (_jsxs("div", Object.assign({ className: "elsa-h-screen elsa-bg-gray-100" }, { children: [_jsx("nav", Object.assign({ className: "elsa-bg-gray-800" }, { children: _jsx("div", Object.assign({ className: "elsa-px-4 sm:elsa-px-6 lg:elsa-px-8" }, { children: _jsx("div", Object.assign({ className: "elsa-flex elsa-items-center elsa-justify-between elsa-h-16" }, { children: _jsxs("div", Object.assign({ className: "elsa-flex elsa-items-center" }, { children: [_jsx("div", Object.assign({ className: "elsa-flex-shrink-0" }, { children: _jsx("div", { children: _jsx("img", { className: "elsa-h-8 elsa-w-8", src: logoPath, alt: "Workflow" }, void 0) }, void 0) }), void 0), _jsx("div", Object.assign({ className: "hidden md:elsa-block" }, { children: _jsx("div", Object.assign({ className: "elsa-ml-10 elsa-flex elsa-items-baseline elsa-space-x-4" }, { children: "Elsa Dashboard React component" }), void 0) }), void 0)] }), void 0) }), void 0) }), void 0) }), void 0), _jsx("main", {}, void 0)] }), void 0));
    }
}
export default ElsaStudioDashboard;

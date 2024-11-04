import { jsx as _jsx, jsxs as _jsxs } from "react/jsx-runtime";
import { Component } from 'react';
class ElsaStudioDashboard extends Component {
    async componentWillLoad() {
    }
    render() {
        return (_jsx("div", { className: "elsa-h-screen elsa-bg-gray-100", children: _jsx("nav", { className: "elsa-bg-gray-800", children: _jsx("div", { className: "elsa-px-4 sm:elsa-px-6 lg:elsa-px-8", children: _jsx("div", { className: "elsa-flex elsa-items-center elsa-justify-between elsa-h-16", children: _jsxs("div", { className: "elsa-flex elsa-items-center", children: [_jsx("div", { className: "elsa-flex-shrink-0" }), _jsx("div", { className: "hidden md:elsa-block", children: _jsx("div", { className: "elsa-ml-10 elsa-flex elsa-items-baseline elsa-space-x-4", children: "Elsa Dashboard React component" }) })] }) }) }) }) }));
    }
}
export default ElsaStudioDashboard;

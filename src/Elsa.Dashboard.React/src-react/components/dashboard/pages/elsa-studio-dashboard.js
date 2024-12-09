import { jsx as _jsx, jsxs as _jsxs } from "react/jsx-runtime";
import { Component } from 'react';
import { NavLink, Routes, Route } from 'react-router-dom';
import ElsaStudioWorkflowDefinitionsList from './elsa-studio-workflow-definitions-list';
import ElsaStudioWorkflowInstancesList from './elsa-studio-workflow-instances-list';
class ElsaStudioDashboard extends Component {
    constructor(props) {
        super(props);
        this.dashboardMenu = {
            data: {
                menuItems: [
                    ['workflow-definitions', 'Workflow Definitions'],
                    ['workflow-instances', 'Workflow Instances'],
                ],
                routes: [
                    { path: 'workflow-definitions', element: _jsx(ElsaStudioWorkflowDefinitionsList, {}) },
                    { path: 'workflow-instances', element: _jsx(ElsaStudioWorkflowInstancesList, {}) },
                ]
            }
        };
        this.state = {
            basePath: ""
        };
    }
    render() {
        const logoPath = '/assets/logo.png';
        let basePath = this.state.basePath;
        let menuItems = (this.dashboardMenu.data != null ? this.dashboardMenu.data.menuItems : [])
            .map(([route, label]) => [route, label]);
        let routes = this.dashboardMenu.data != null ? this.dashboardMenu.data.routes : [];
        const renderFeatureMenuItem = (item, basePath) => {
            return (_jsx(NavLink, { to: `${basePath}/${item[0]}`, className: ({ isActive }) => (isActive ? "" : "elsa-text-gray-300 hover:elsa-bg-gray-700 hover:elsa-text-white elsa-px-3 elsa-py-2 elsa-rounded-md elsa-text-sm elsa-font-medium"), children: item[1] }));
        };
        const renderFeatureRoute = (path, element, basePath) => {
            return (_jsx(Route, { path: `${basePath}/${path}`, element: element }));
        };
        return (_jsxs("div", { className: "elsa-h-screen elsa-bg-gray-100", children: [_jsx("nav", { className: "elsa-bg-gray-800", children: _jsx("div", { className: "elsa-px-4 sm:elsa-px-6 lg:elsa-px-8", children: _jsx("div", { className: "elsa-flex elsa-items-center elsa-justify-between elsa-h-16", children: _jsxs("div", { className: "elsa-flex elsa-items-center", children: [_jsx("div", { className: "elsa-flex-shrink-0", children: _jsx("div", { children: _jsx("img", { className: "elsa-h-8 elsa-w-8", src: logoPath, alt: "Workflow" }) }) }), _jsx("div", { className: "hidden md:elsa-block", children: _jsx("div", { className: "elsa-ml-10 elsa-flex elsa-items-baseline elsa-space-x-4", children: menuItems.map((item) => renderFeatureMenuItem(item, basePath)) }) })] }) }) }) }), _jsx("main", { children: _jsx(Routes, { children: routes.map(({ path, element }) => renderFeatureRoute(path, element, basePath)) }) })] }));
    }
}
export default ElsaStudioDashboard;

import { jsx as _jsx } from "react/jsx-runtime";
import { Component } from "react";
import ElsaStudioDashboard from "./elsa-studio-dashboard";
class ElsaStudioRoot extends Component {
    render() {
        return (_jsx("div", { children: _jsx(ElsaStudioDashboard, { basePath: "" }) }));
    }
}
export default ElsaStudioRoot;

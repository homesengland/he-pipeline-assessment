import { Component } from "react";
import ElsaStudioDashboard from "./elsa-studio-dashboard";

class ElsaStudioRoot extends Component
{
  render() {
    return (
      <div>
        <ElsaStudioDashboard basePath= ""></ElsaStudioDashboard>
      </div>
    );
  }
}

export default ElsaStudioRoot;

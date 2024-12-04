import { Component } from 'react';
import { ConfigureDashboardMenuContext } from '../../../models/events';
import { NavLink, Routes, Route } from 'react-router-dom'
import ElsaStudioWorkflowDefinitionsList from './elsa-studio-workflow-definitions-list';
import ElsaStudioWorkflowInstancesList from './elsa-studio-workflow-instances-list';
class ElsaStudioDashboard extends Component<ElsaDashboardProps, ElsaDashboardState> {
  constructor(props) {
    super(props);
    this.state = {
      basePath: ""
    }
    
  }

  private dashboardMenu: ConfigureDashboardMenuContext = {
    data: {
      menuItems: [
        ['workflow-definitions', 'Workflow Definitions'],
        ['workflow-instances', 'Workflow Instances'],
      ],
      routes: [
        { path: 'workflow-definitions', element: <ElsaStudioWorkflowDefinitionsList /> },
        { path: 'workflow-instances', element: <ElsaStudioWorkflowInstancesList /> },
      ]
    }
  };

  render() {
    const logoPath = '/assets/logo.png';
    let basePath = this.state.basePath;
    let menuItems = (this.dashboardMenu.data != null ? this.dashboardMenu.data.menuItems : [])
      .map(([route, label]) => [route, label]);

    let routes = this.dashboardMenu.data != null ? this.dashboardMenu.data.routes : [];

    const renderFeatureMenuItem = (item, basePath) => {
      return (<NavLink to={`${basePath}/${item[0]}`}
        className={({ isActive }) => (isActive ? "" : "elsa-text-gray-300 hover:elsa-bg-gray-700 hover:elsa-text-white elsa-px-3 elsa-py-2 elsa-rounded-md elsa-text-sm elsa-font-medium")}
            >
        {item[1]}
      </NavLink>);
    }

    const renderFeatureRoute = (path: string, element: any, basePath: string) => {
      return (<Route path={`${basePath}/${path}`} element={element}/>)
    }

    return (
      <div className="elsa-h-screen elsa-bg-gray-100">
        <nav className="elsa-bg-gray-800">
          <div className="elsa-px-4 sm:elsa-px-6 lg:elsa-px-8">
            <div className="elsa-flex elsa-items-center elsa-justify-between elsa-h-16">
              <div className="elsa-flex elsa-items-center">
                <div className="elsa-flex-shrink-0">
                  <div >
                  <img className="elsa-h-8 elsa-w-8" src={logoPath}
                     alt="Workflow" />
                  </div>
                </div>
                <div className="hidden md:elsa-block">
                  <div className="elsa-ml-10 elsa-flex elsa-items-baseline elsa-space-x-4">
                    {menuItems.map((item) => renderFeatureMenuItem(item, basePath))}
                  </div>
                </div>
              </div>
            </div>
          </div>
        </nav>
        <main>
          <Routes>
            {routes.map(({ path, element }) => renderFeatureRoute(path, element, basePath))}
            </Routes>
        </main>
      </div>
    );
  }
}

export default ElsaStudioDashboard;

interface ElsaDashboardProps {
  basePath: string;
}

interface ElsaDashboardState {
  basePath: string;
}



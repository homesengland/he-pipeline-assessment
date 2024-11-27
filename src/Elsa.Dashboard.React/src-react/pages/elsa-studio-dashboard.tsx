import { Component } from 'react';
class ElsaStudioDashboard extends Component
{

  async componentWillLoad() {

  }

  render() {
    const logoPath = '/assets/logo.png';

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
                    Elsa Dashboard React component 
                  </div>
                </div>
              </div>
            </div>
          </div>
        </nav>
        <main>
        </main>
      </div>
    );
  }
}

export default ElsaStudioDashboard;



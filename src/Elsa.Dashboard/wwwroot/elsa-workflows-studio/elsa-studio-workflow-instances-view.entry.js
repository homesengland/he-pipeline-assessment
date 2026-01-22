import { r as registerInstance, h } from './index-1542df5c.js';

const ElsaStudioWorkflowInstancesView = class {
  constructor(hostRef) {
    registerInstance(this, hostRef);
    this.match = undefined;
  }
  componentWillLoad() {
    this.id = this.match.params.id;
  }
  render() {
    const id = this.id;
    return h("div", null, h("elsa-workflow-instance-viewer-screen", { workflowInstanceId: id }));
  }
};

export { ElsaStudioWorkflowInstancesView as elsa_studio_workflow_instances_view };

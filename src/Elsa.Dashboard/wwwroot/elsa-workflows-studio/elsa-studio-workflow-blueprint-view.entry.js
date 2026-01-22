import { r as registerInstance, h } from './index-1542df5c.js';

const ElsaStudioWorkflowBlueprintView = class {
  constructor(hostRef) {
    registerInstance(this, hostRef);
    this.match = undefined;
  }
  componentWillLoad() {
    this.id = this.match.params.id;
  }
  render() {
    const id = this.id;
    return h("div", null, h("elsa-workflow-blueprint-viewer-screen", { workflowDefinitionId: id }));
  }
};

export { ElsaStudioWorkflowBlueprintView as elsa_studio_workflow_blueprint_view };

import { r as registerInstance, h } from './index-CL6j2ec2.js';

const ElsaStudioWorkflowBlueprintView = class {
    constructor(hostRef) {
        registerInstance(this, hostRef);
    }
    componentWillLoad() {
        this.id = this.match.params.id;
    }
    render() {
        const id = this.id;
        return h("div", { key: 'b7948cdbf899b12800e92b4208c1bcab2d211668' }, h("elsa-workflow-blueprint-viewer-screen", { key: '1093b93b1b76a9533085693d05325c3033f63066', workflowDefinitionId: id }));
    }
};

export { ElsaStudioWorkflowBlueprintView as elsa_studio_workflow_blueprint_view };
//# sourceMappingURL=elsa-studio-workflow-blueprint-view.entry.esm.js.map

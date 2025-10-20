import { r as registerInstance, h } from './index-CL6j2ec2.js';

const ElsaStudioWorkflowInstancesView = class {
    constructor(hostRef) {
        registerInstance(this, hostRef);
    }
    componentWillLoad() {
        this.id = this.match.params.id;
    }
    render() {
        const id = this.id;
        return h("div", { key: 'b2b659c0c7bd67873482d93e5ae0f979599799fb' }, h("elsa-workflow-instance-viewer-screen", { key: '77d1be523ab6bb86ec05e384b8a1128243afff9a', workflowInstanceId: id }));
    }
};

export { ElsaStudioWorkflowInstancesView as elsa_studio_workflow_instances_view };
//# sourceMappingURL=elsa-studio-workflow-instances-view.entry.esm.js.map

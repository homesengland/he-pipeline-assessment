import { r as registerInstance, h } from './index-CL6j2ec2.js';

const ElsaStudioWorkflowDefinitionsEdit = class {
    constructor(hostRef) {
        registerInstance(this, hostRef);
    }
    componentWillLoad() {
        let id = this.match.params.id;
        if (!!id && id.toLowerCase() == 'new')
            id = null;
        this.id = id;
    }
    render() {
        const id = this.id;
        return h("div", { key: '10766db3bd599fd18a0a3994425a16ea682f96ca' }, h("elsa-workflow-definition-editor-screen", { key: '62402f32c7ff2df2ef014bd7ffd63d6fdecb1cd1', "workflow-definition-id": id }));
    }
};

export { ElsaStudioWorkflowDefinitionsEdit as elsa_studio_workflow_definitions_edit };
//# sourceMappingURL=elsa-studio-workflow-definitions-edit.entry.esm.js.map

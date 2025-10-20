import { r as registerInstance, h } from './index-CL6j2ec2.js';

const ElsaStudioWebhookDefinitionsEdit = class {
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
        return (h("div", { key: '6989e3023b6a17e70b384ea5dbab2e6403eec06b' }, h("elsa-webhook-definition-editor-screen", { key: 'ae6f682a01c11935c7a498c23ef606c7cffc998d', "webhook-definition-id": id })));
    }
};

export { ElsaStudioWebhookDefinitionsEdit as elsa_studio_webhook_definitions_edit };
//# sourceMappingURL=elsa-studio-webhook-definitions-edit.entry.esm.js.map

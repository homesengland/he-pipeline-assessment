import { r as registerInstance, h } from './index-ea213ee1.js';

let ElsaStudioWebhookDefinitionsEdit = class {
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
    return (h("div", null, h("elsa-webhook-definition-editor-screen", { "webhook-definition-id": id })));
  }
};

export { ElsaStudioWebhookDefinitionsEdit as elsa_studio_webhook_definitions_edit };

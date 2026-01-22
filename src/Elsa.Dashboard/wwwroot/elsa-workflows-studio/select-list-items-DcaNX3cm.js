import './index-cDkf_GGD.js';
import { b as createElsaClient } from './elsa-client-03-1ZsIW.js';

async function fetchRuntimeItems(serverUrl, options) {
    const elsaClient = await createElsaClient(serverUrl);
    return await elsaClient.designerApi.runtimeSelectItemsApi.get(options.runtimeSelectListProviderType, options.context || {});
}
async function getSelectListItems(serverUrl, propertyDescriptor) {
    const options = propertyDescriptor.options;
    let selectList;
    if (!!options && options.runtimeSelectListProviderType)
        selectList = await fetchRuntimeItems(serverUrl, options);
    else if (Array.isArray(options))
        selectList = {
            items: options,
            isFlagsEnum: false
        };
    else
        selectList = options;
    return selectList || { items: [], isFlagsEnum: false };
}

export { getSelectListItems as g };
//# sourceMappingURL=select-list-items-DcaNX3cm.js.map

//# sourceMappingURL=select-list-items-DcaNX3cm.js.map
import './index-c5018c3a.js';
import { b as createElsaClient } from './elsa-client-ecb85def.js';

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

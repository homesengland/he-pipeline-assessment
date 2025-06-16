import { ElsaClientService } from 'src/services/elsa-client';
import { ActivityPropertyDescriptor, RuntimeSelectListProviderSettings, SelectList, SelectListItem } from '../models';

async function fetchRuntimeItems(elsaClientService: ElsaClientService, serverUrl: string, options: RuntimeSelectListProviderSettings): Promise<SelectList> {
  const elsaClient = await elsaClientService.createElsaClient(serverUrl);
  return await elsaClient.designerApi.runtimeSelectItemsApi.get(options.runtimeSelectListProviderType, options.context || {});
}

export async function getSelectListItems(elsaClientService: ElsaClientService, serverUrl: string, propertyDescriptor: ActivityPropertyDescriptor): Promise<SelectList> {
  const options: any = propertyDescriptor.options;
  let selectList: SelectList;

  if (!!options && options.runtimeSelectListProviderType) selectList = await this.fetchRuntimeItems(serverUrl, options);
  else if (Array.isArray(options))
    selectList = {
      items: options,
      isFlagsEnum: false,
    };
  else selectList = options as SelectList;

  return selectList || { items: [], isFlagsEnum: false };
}

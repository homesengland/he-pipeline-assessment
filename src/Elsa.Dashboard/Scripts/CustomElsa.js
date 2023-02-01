import { GetCustomActivityProperties } from './Activities/GetCustomActivityProperties';
import { RegisterCustomPlugins } from './Plugins/CustomPlugins';
import { CustomPropertyUIHints } from '../Constants/CustomPropertyUiHints';
import { CustomComponentTags } from '../Constants/CustomComponentTags';
import { QuestionDriver } from './Drivers/QuestionPropertyDriver';
import { CustomSwitchDriver } from './Drivers/CustomSwitchPropertyDriver';
import { CustomTextDriver } from './Drivers/CustomTextPropertyDriver';
import { ConditionalTextListDriver } from './Drivers/ConditionalTextListPropertyDriver';


export async function InitCustomElsa() {
  const elsaStudioRoot = document.querySelector('elsa-studio-root');

  const customProperties = await GetCustomActivityProperties(elsaStudioRoot.serverUrl);

  console.log(customProperties);

  elsaStudioRoot.addEventListener('initializing', e => {
    var elsaStudio = e.detail;
    RegisterPlugins(elsaStudio);
    RegisterDrivers(elsaStudio);
  });

  function RegisterDrivers(elsaStudio) {
    elsaStudio.propertyDisplayManager.addDriver(CustomPropertyUIHints.QuestionScreenBuilder,
      () => new QuestionDriver(elsaStudio, CustomComponentTags.QuestionScreen));

    elsaStudio.propertyDisplayManager.addDriver(CustomPropertyUIHints.ConditionalTextListBuilder,
      () => new ConditionalTextListDriver(elsaStudio, CustomComponentTags.ConditionalTextList));

    elsaStudio.propertyDisplayManager.addDriver(CustomPropertyUIHints.CustomTextBuilder,
      () => new CustomTextDriver(elsaStudio, CustomComponentTags.CustomTextArea));

    elsaStudio.propertyDisplayManager.addDriver(CustomPropertyUIHints.CustomSwitchBuilder,
      () => new CustomSwitchDriver(elsaStudio, CustomComponentTags.CustomSwitch));
    
  }

  function RegisterPlugins(elsaStudio) {
    elsaStudio.pluginManager.registerPlugin(RegisterCustomPlugins);
  }

 }

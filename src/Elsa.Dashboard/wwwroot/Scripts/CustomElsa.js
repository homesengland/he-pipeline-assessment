import { GetCustomActivityProperties } from './Activities/GetCustomActivityProperties.js';
import { RegisterCustomPlugins } from './Plugins/CustomPlugins.js';
import { CustomPropertyUIHints } from './Constants/CustomPropertyUiHints.js';
import { CustomComponentTags } from './Constants/CustomComponentTags.js';
import { QuestionDriver } from './Drivers/QuestionPropertyDriver.js';
import { CustomSwitchDriver } from './Drivers/CustomSwitchPropertyDriver.js';
import { CustomTextDriver } from './Drivers/CustomTextPropertyDriver.js';
import { ConditionalTextListDriver } from './Drivers/ConditionalTextListPropertyDriver.js';


export async function InitCustomElsa(elsaStudioRoot, serverUrl) {
  const customProperties = await GetCustomActivityProperties(serverUrl);
  console.log(customProperties);

  elsaStudioRoot.addEventListener('initializing', e => {
    var elsaStudio = e.detail;
    RegisterPlugins(elsaStudio);
    RegisterDrivers(elsaStudio);
  });

  function RegisterDrivers(elsaStudio) {
    elsaStudio.propertyDisplayManager.addDriver(CustomPropertyUIHints.QuestionScreenBuilder,
      () => new QuestionDriver(elsaStudio, CustomComponentTags.QuestionScreen, customProperties[CustomPropertyUIHints.QuestionScreenBuilder]));

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

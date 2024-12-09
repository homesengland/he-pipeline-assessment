import { RegisterCustomPlugins } from './Plugins/CustomPlugins.js';
import { Auth0Plugin } from './Plugins/Auth0Plugin.js';
import { CustomPropertyUIHints } from './Constants/CustomPropertyUiHints.js';
import { CustomComponentTags } from './Constants/CustomComponentTags.js';
import { QuestionDriver } from './Drivers/QuestionPropertyDriver.js';
import { CustomSwitchDriver } from './Drivers/CustomSwitchPropertyDriver.js';
import { CustomTextDriver } from './Drivers/CustomTextPropertyDriver.js';
import { ConditionalTextListDriver } from './Drivers/ConditionalTextListPropertyDriver.js';
import { TextActivityDriver } from './Drivers/TextActivityPropertyDriver.js';
import { TextGroupDriver } from './Drivers/TextGroupDriver.js';

export function InitCustomElsa(elsaStudioRoot, customProperties, auth0ClientOptions) {

  elsaStudioRoot.addEventListener('initializing', e => {
    var elsaStudio = e.detail;
    elsaStudio.pluginManager.registerPluginFactory(() => new Auth0Plugin(auth0ClientOptions, elsaStudio))
    elsaStudio.pluginManager.registerPlugin(RegisterCustomPlugins);
    RegisterDrivers(elsaStudio, customProperties);
  });

  function RegisterDrivers(elsaStudio, customProperties) {
    elsaStudio.propertyDisplayManager.addDriver(CustomPropertyUIHints.QuestionScreenBuilder,
      () => new QuestionDriver(CustomComponentTags.QuestionScreen, customProperties));

    elsaStudio.propertyDisplayManager.addDriver(CustomPropertyUIHints.TextActivityBuilder,
      () => new TextActivityDriver(elsaStudio, CustomComponentTags.TextActivity));

    elsaStudio.propertyDisplayManager.addDriver(CustomPropertyUIHints.TextGroupBuilder,
      () => new TextGroupDriver(elsaStudio, CustomComponentTags.TextGroup));

    elsaStudio.propertyDisplayManager.addDriver(CustomPropertyUIHints.ConditionalTextListBuilder,
      () => new ConditionalTextListDriver(elsaStudio, CustomComponentTags.ConditionalTextList));

    elsaStudio.propertyDisplayManager.addDriver(CustomPropertyUIHints.CustomTextBuilder,
      () => new CustomTextDriver(elsaStudio, CustomComponentTags.CustomTextArea));

    elsaStudio.propertyDisplayManager.addDriver(CustomPropertyUIHints.CustomSwitchBuilder,
      () => new CustomSwitchDriver(elsaStudio, CustomComponentTags.CustomSwitch));
    
  }

 }

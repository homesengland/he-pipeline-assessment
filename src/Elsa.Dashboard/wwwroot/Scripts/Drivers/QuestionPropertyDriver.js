import { getOrCreateProperty } from '../Activities/GetOrCreateProperty.js'
import { PropertyDescriberHints } from '../Constants/CustomPropertyUiHints.js';

export function QuestionDriver(elementName, customProperties) {
  this.display = (activity, property) => {
    var prop = (0, getOrCreateProperty)(activity, property.name);
    var questionActivity = document.createElement(elementName);
    questionActivity.activityModel = activity;
    questionActivity.propertyDescriptor = property;
    questionActivity.propertyModel = prop;
    questionActivity.questionProperties = JSON.parse(customProperties[PropertyDescriberHints.QuestionScreenBuilder]);
    return questionActivity;
  }
}

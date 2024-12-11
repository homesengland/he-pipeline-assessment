import { getOrCreateProperty } from '../Activities/GetOrCreateProperty.js'

export function ConditionalTextListDriver(elsaStudio, elementName) {
  this.display = (activity, property) => {
    var prop = (0, getOrCreateProperty)(activity, property.name);
    var questionActivity = document.createElement(elementName);
    questionActivity.activityModel = activity;
    questionActivity.propertyDescriptor = property;
    questionActivity.propertyModel = prop;
    return questionActivity;
  }
}

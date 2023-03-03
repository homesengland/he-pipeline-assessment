import { getOrCreateProperty } from '../Activities/GetOrCreateProperty.js'

export function CustomSwitchDriver(elsaStudio, elementName) {
  this.display = (activity, property) => {
    console.log("Elsa Studio", elsaStudio);
    console.log("Property", property);
    console.log("Activity", activity);
    var prop = (0, getOrCreateProperty)(activity, property.name);
    var questionActivity = document.createElement(elementName);
    questionActivity.activityModel = activity;
    questionActivity.propertyDescriptor = property;
    questionActivity.propertyModel = prop;
    return questionActivity;
  }
}

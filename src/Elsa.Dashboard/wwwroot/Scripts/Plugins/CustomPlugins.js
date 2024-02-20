export function RegisterCustomPlugins(elsaStudio){
  const eventBus = elsaStudio.eventBus;

  eventBus.on('activity-design-displaying',
    eventContext => {
      DisableDefaultOutcomePaths(eventContext);
      EnableCustomSwitchOutcomePaths(eventContext);
    });

  //eventBus.on('dashboard.appearing',
  //  eventContext => {
  //    AddDataDictionaryTab(eventContext);
  //  });
}

const ActivitiesWithFilteredOutcomes = [
  "QuestionScreen",
  "CheckYourAnswersScreen"
];

const ActivitiesWithDoneOutcome = [
  "ConfirmationScreen"
];

const SwitchSyntax = 'Switch';
const DefaultOutcome = 'Default';
const DoneOutcome = 'Done'

//function AddDataDictionaryTab(eventContext) {
//  // Define a list of menu items to append.
//  const menuItems = [["data-dictionary", "Data Dictionary"]];

//  // Define a list of routes to register with the router.
//  const routes = [["data-dictionary", "data-dictionary-screen", true],
//    ["data-dictionary/group/new", "data-dictionary-group-create", true],
//    ["data-dictionary/group/:id", "data-dictionary-group-edit", true],
//    ["data-dictionary/group/:id/item/new", "data-dictionary-item-create", true],
//    ["data-dictionary/group/:id/item/:itemId", "data-dictionary-item-edit", true],
//  ];

//  // Append custom menu items to existing list of menu items.
//  eventContext.data.menuItems = [...eventContext.data.menuItems, ...menuItems];

//  // Append custom routes to existing list of routes.
//  eventContext.data.routes = [...eventContext.data.routes, ...routes];
//}

function DisableDefaultOutcomePaths(eventContext) {
  if (ActivitiesWithFilteredOutcomes.includes(eventContext.activityModel.type))
    eventContext.outcomes = eventContext.outcomes.filter(x => x !== DefaultOutcome && x !== DoneOutcome);
  else if (eventContext.activityModel.type == ActivitiesWithDoneOutcome) {
    eventContext.outcomes = eventContext.outcomes.filter(x => x == DoneOutcome);
  }
}

function EnableCustomSwitchOutcomePaths(eventContext) {
  const activityModel = eventContext.activityModel;
  const activityDescriptor = eventContext.activityDescriptor;
  const propertyDescriptors = activityDescriptor.inputProperties;
  const switchCaseProperties = propertyDescriptors.filter(x => x.uiHint == 'custom-switch-case-builder');
  if (switchCaseProperties.length == 0)
    return;
  let outcomesHash = {};
  const syntax = SwitchSyntax;
  for (const propertyDescriptor of switchCaseProperties) {
    const props = activityModel.properties || [];
    const casesProp = props.find(x => x.name == propertyDescriptor.name) || { expressions: { SwitchSyntax: '' }, syntax: syntax };
    const expression = casesProp.expressions[syntax] || [];
    const cases = !!expression['$values'] ? expression['$values'] : Array.isArray(expression) ? expression : parseJson(expression) || [];
    for (const c of cases)
      outcomesHash[c.name] = true;
  }
  const outcomes = Object.keys(outcomesHash);
  eventContext.outcomes = [...outcomes];
}

function parseJson(json) {
  if (!json)
    return null;
  try {
    return JSON.parse(json);
  }
  catch (e) {
    console.warn(`Error parsing JSON: ${e}`);
  }
  return undefined;
}


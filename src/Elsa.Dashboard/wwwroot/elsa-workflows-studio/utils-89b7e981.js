import { S as SyntaxNames } from './constants-6ea82f24.js';
import { v as v4 } from './index-912d1a21.js';

function newOptionNumber(options) {
  let highestValue = 0;
  if (options != null && options.length > 0) {
    highestValue = options.sort(function (a, b) {
      return a - b;
    }).pop();
  }
  highestValue = highestValue + 1;
  return highestValue.toString();
}
function newOptionLetter(options) {
  let highestValue = "A";
  if (options != null && options.length > 0) {
    highestValue = options.sort().pop();
    return incrementString(highestValue);
  }
  return highestValue;
}
function toLetter(num) {
  "use strict";
  var mod = num % 26, pow = num / 26 | 0, out = mod ? String.fromCharCode(64 + mod) : (--pow, 'Z');
  return pow ? toLetter(pow) + out : out;
}
;
function incrementString(value) {
  let carry = 1;
  let res = '';
  for (let i = value.length - 1; i >= 0; i--) {
    let char = value.toUpperCase().charCodeAt(i);
    char += carry;
    if (char > 90) {
      char = 65;
      carry = 1;
    }
    else {
      carry = 0;
    }
    res = String.fromCharCode(char) + res;
    if (!carry) {
      res = value.substring(0, i) + res;
      break;
    }
  }
  if (carry) {
    res = 'A' + res;
  }
  return res;
}
function getOrCreateProperty(activity, name, defaultExpression, defaultSyntax) {
  let property = activity.properties.find(x => x.name == name);
  if (!property) {
    const expressions = {};
    let syntax = defaultSyntax ? defaultSyntax() : undefined;
    if (!syntax)
      syntax = SyntaxNames.Literal;
    expressions[syntax] = defaultExpression ? defaultExpression() : undefined;
    property = { name: name, expressions: expressions, syntax: null };
    activity.properties.push(property);
  }
  return property;
}
function getOrCreateNestedProperty(activity, name, type, defaultExpression, defaultSyntax) {
  var property = getOrCreateProperty(activity, name, defaultExpression, defaultSyntax);
  property.type = type;
  return property;
}
function parseJson(json) {
  if (!json)
    return null;
  try {
    return JSON.parse(json);
  }
  catch (e) {
    //console.warn(`Error parsing JSON: ${e}`);
    //The Above never parses quite correctly,
    e = e;
  }
  return undefined;
}
function mapSyntaxToLanguage(syntax) {
  switch (syntax) {
    case SyntaxNames.Json:
      return 'json';
    case SyntaxNames.JavaScript:
      return 'javascript';
    case SyntaxNames.Liquid:
      return 'liquid';
    case SyntaxNames.SQL:
      return 'sql';
    case SyntaxNames.Literal:
    default:
      return 'plaintext';
  }
}
function filterPropertiesByType(questionProperties, questionType) {
  const propertiesJson = JSON.stringify(questionProperties);
  const properties = JSON.parse(propertiesJson);
  properties.forEach(p => {
    if (p.conditionalActivityTypes != null && p.conditionalActivityTypes.length > 0) {
      if (!p.conditionalActivityTypes.includes(questionType)) {
        p.displayInDesigner = false;
      }
    }
  });
  const filteredProperties = properties.filter(property => property.displayInDesigner == true);
  return filteredProperties;
}
function updateNestedActivitiesByDescriptors(descriptors, properties, questionModel) {
  let newProperties = [];
  descriptors.forEach(x => newProperties.push(createQuestionProperty(x, questionModel)));
  const result = newProperties.map(x => {
    const existingValue = properties.filter(y => x.descriptor.name == y.descriptor.name)[0];
    if (existingValue != null) {
      x.value = existingValue.value;
    }
    return x;
  });
  return result;
}
function createQuestionProperty(descriptor, questionModel) {
  let propertyValue = {
    syntax: descriptor.defaultSyntax,
    value: descriptor.expectedOutputType,
    name: descriptor.name,
    expressions: getExpressionMap(descriptor.supportedSyntaxes),
    type: ''
  };
  if (descriptor.name.toLowerCase() == 'id') {
    propertyValue.expressions[SyntaxNames.Literal] = questionModel.value.value;
  }
  let property = { value: propertyValue, descriptor: descriptor, };
  return property;
}
function getExpressionMap(syntaxes) {
  if (syntaxes.length > 0) {
    var value = {};
    syntaxes.forEach(s => {
      value[s] = "";
    });
    return value;
  }
  else {
    let value = {};
    //value[SyntaxNames.Literal] = '';
    return value;
  }
}
async function awaitElement(selector) {
  while (document.querySelector(selector) === null) {
    await new Promise(resolve => requestAnimationFrame(resolve));
  }
  return document.querySelector(selector);
}
function onUpdateCustomExpression(event, property, syntax, update) {
  property.expressions[syntax] = event.currentTarget.value.trim();
  update();
}
function getUniversalUniqueId() {
  return v4();
}
function getWorkflowDefinitionIdFromUrl() {
  var url_string = document.URL;
  var n = url_string.lastIndexOf('/');
  var workflowDef = url_string.substring(n + 1);
  return workflowDef !== null && workflowDef !== void 0 ? workflowDef : '';
}

export { newOptionNumber as a, getOrCreateProperty as b, getOrCreateNestedProperty as c, createQuestionProperty as d, filterPropertiesByType as f, getUniversalUniqueId as g, mapSyntaxToLanguage as m, newOptionLetter as n, parseJson as p, updateNestedActivitiesByDescriptors as u };

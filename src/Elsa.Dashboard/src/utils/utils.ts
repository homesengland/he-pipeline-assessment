import { SyntaxNames } from "../constants/constants";
import { HeActivityPropertyDescriptor, NestedActivityDefinitionProperty, NestedProperty, NestedPropertyModel } from "../models/custom-component-models";
import { ActivityDefinitionProperty, ActivityModel } from "../models/elsa-interfaces";
import { v4 as uuidv4 } from 'uuid';

export type Map<T> = {
  [key: string]: T
};

export function newOptionNumber(options: Array<number>) : string {
  let highestValue: number = 0;
  if (options != null && options.length > 0) {
    highestValue = options.sort(function (a, b) {
      return a - b;
    }).pop();
  }
  highestValue = highestValue + 1;
  return highestValue.toString();
}

export function newOptionLetter(options: Array<string>): string {
  let highestValue: string = "A";
  if (options != null && options.length > 0) {
    highestValue = options.sort().pop();
    return incrementString(highestValue);
  }
  return highestValue;
  
}

export function toLetter(num: number) {
  "use strict";
  var mod = num % 26,
    pow = num / 26 | 0,
    out = mod ? String.fromCharCode(64 + mod) : (--pow, 'Z');
  return pow ? toLetter(pow) + out : out;
};

function incrementString(value: string) : string {
  let carry: number = 1;
  let res: string = '';

  for (let i = value.length - 1; i >= 0; i--) {
    let char = value.toUpperCase().charCodeAt(i);
    char += carry;
    if (char > 90) {
      char = 65;
      carry = 1;
    } else {
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

export function getOrCreateProperty(activity: ActivityModel, name: string, defaultExpression?: () => string, defaultSyntax?: () => string): ActivityDefinitionProperty {
  let property: ActivityDefinitionProperty = activity.properties.find(x => x.name == name);

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

export function parseJson(json: string): any {
  if (!json)
    return null;

  try {
    return JSON.parse(json);
  } catch (e) {
    //console.warn(`Error parsing JSON: ${e}`);
    //The Above never parses quite correctly,
    e = e;
  }
  return undefined;
}

export function mapSyntaxToLanguage(syntax: string): any {
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

export function filterPropertiesByType(questionProperties: Array<HeActivityPropertyDescriptor>, questionType: string) {

  const propertiesJson: string = JSON.stringify(questionProperties);
  const properties: Array<HeActivityPropertyDescriptor> = JSON.parse(propertiesJson);


  properties.forEach(p => {

    if (p.conditionalActivityTypes != null && p.conditionalActivityTypes.length > 0) {
      if (!p.conditionalActivityTypes.includes(questionType)) {
        p.displayInDesigner = false;
      }
    }

  });
  const filteredProperties: Array<HeActivityPropertyDescriptor> = properties.filter(property => property.displayInDesigner == true);
  return filteredProperties;
}

export function updateNestedActivitiesByDescriptors(descriptors: Array<HeActivityPropertyDescriptor>, properties: Array<NestedProperty>, questionModel: NestedPropertyModel): Array<NestedProperty> {
  let newProperties: Array<NestedProperty> = [];
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

export function createQuestionProperty(descriptor: HeActivityPropertyDescriptor, questionModel: NestedPropertyModel): NestedProperty {

  let propertyValue: NestedActivityDefinitionProperty = {
    syntax: descriptor.defaultSyntax,
    value: descriptor.expectedOutputType,
    name: descriptor.name,
    expressions: getExpressionMap(descriptor.supportedSyntaxes),
    type: ''
  }
  if (descriptor.name.toLowerCase() == 'id') {
    propertyValue.expressions[SyntaxNames.Literal] = questionModel.value.value;
  }
  let property: NestedProperty = { value: propertyValue, descriptor: descriptor, }
  return property;
}

export function getExpressionMap(syntaxes: Array<string>): Map < string > {
  if(syntaxes.length > 0) {
  var value: Map<string> = {};
  syntaxes.forEach(s => {
    value[s] = "";
  })
  return value;
}
    else {
  let value: Map<string> = {};
  //value[SyntaxNames.Literal] = '';
  return value;
}
  }

export async function awaitElement(selector) {
  while (document.querySelector(selector) === null) {
    await new Promise(resolve => requestAnimationFrame(resolve))
  }
  return document.querySelector(selector);
}

export function onUpdateCustomExpression(event: CustomEvent<string>, property: NestedActivityDefinitionProperty, syntax: string, update: Function) {
  property.expressions[syntax] = (event.currentTarget as HTMLInputElement).value.trim();
  update();
}

export function getUniversalUniqueId() {
  return uuidv4();
}

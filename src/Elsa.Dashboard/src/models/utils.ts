import { SyntaxNames } from "../constants/Constants";
import { HeActivityPropertyDescriptor } from "./custom-component-models";
import { ActivityDefinitionProperty, ActivityModel } from "./elsa-interfaces";

export type Map<T> = {
  [key: string]: T
};

export function ToLetter(num: number) {
  "use strict";
  var mod = num % 26,
    pow = num / 26 | 0,
    out = mod ? String.fromCharCode(64 + mod) : (--pow, 'Z');
  return pow ? ToLetter(pow) + out : out;
};

export function getOrCreateProperty(activity: ActivityModel, name: string, defaultExpression?: () => string, defaultSyntax?: () => string): ActivityDefinitionProperty {
  let property: ActivityDefinitionProperty = activity.properties.find(x => x.name == name);

  if (!property) {
    const expressions = {};
    let syntax = defaultSyntax ? defaultSyntax() : undefined;

    if (!syntax)
      syntax = 'Literal';

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
    console.warn(`Error parsing JSON: ${e}`);
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
    if (p.conditionalActivityType != null && p.conditionalActivityType != questionType)
      p.displayInDesigner = false;
  });
  const filteredProperties: Array<HeActivityPropertyDescriptor> = properties.filter(property => property.displayInDesigner == true);
  return filteredProperties;
}

export async function awaitElement(selector) {
  while (document.querySelector(selector) === null) {
    await new Promise(resolve => requestAnimationFrame(resolve))
  }
  return document.querySelector(selector);
}

//async function fetchRuntimeItems(serverUrl: string, options: RuntimeSelectListProviderSettings): Promise<SelectList> {
//  //const elsaClient = await createElsaClient(serverUrl);
//  //return await elsaClient.designerApi.runtimeSelectItemsApi.get(options.runtimeSelectListProviderType, options.context || {});
//}

//export async function getSelectListItems(serverUrl: string, propertyDescriptor: ActivityPropertyDescriptor): Promise<SelectList> {
//  const options: any = propertyDescriptor.options;
//  let selectList: SelectList;

//  if (!!options && options.runtimeSelectListProviderType)
//    selectList = await fetchRuntimeItems(serverUrl, options);
//  else if (Array.isArray(options))
//    selectList = {
//      items: options,
//      isFlagsEnum: false
//    };
//  else
//    selectList = options as SelectList;

//  return selectList || { items: [], isFlagsEnum: false };
//}

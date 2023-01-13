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

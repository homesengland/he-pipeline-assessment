export function getOrCreateProperty(activity, name, defaultExpression, defaultSyntax) {
  var property = activity.properties.find(function (x) { return x.name == name; });
  if (!property) {
    var expressions = {};
    var syntax = defaultSyntax ? defaultSyntax() : undefined;
    if (!syntax)
      syntax = 'Literal';
    expressions[syntax] = defaultExpression ? defaultExpression() : undefined;
    property = { name: name, expressions: expressions, syntax: null };
    activity.properties.push(property);
  }
  return property;
}

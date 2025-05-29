// Supports hierarchical field names, e.g. 'foo.bar.baz`, which will map to e.g. { foo: { bar: { baz: ''} } }.

export interface SelectOption {
  value: string;
  text: string;
}

export class FormContext {
  constructor(model: any, updater: (model: any) => any) {
    this.model = model;
    this.updater = updater;
  }

  public model: any;
  public updater: (model: any) => any;
}

export function updateField<T>(context: FormContext, fieldName: string, value: T) {
  const fieldNameHierarchy = fieldName.split('.');
  let clone = { ...context.model };
  let current = clone;

  for (const name of fieldNameHierarchy.slice(0, fieldNameHierarchy.length - 1)) {
    if (!current[name]) current[name] = {};

    current = current[name];
  }

  const leafFieldName = fieldNameHierarchy[fieldNameHierarchy.length - 1];
  current[leafFieldName] = value;

  context.model = clone;
  context.updater(clone);
}

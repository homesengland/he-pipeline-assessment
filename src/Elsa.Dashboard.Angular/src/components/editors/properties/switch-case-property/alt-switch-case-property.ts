import { Component, computed, EventEmitter, model, OnChanges, OnInit, signal, SimpleChanges, ViewChild} from '@angular/core';
import { ActivityModel, SyntaxNames } from '../../../../models';
import { ActivityDefinitionProperty, ActivityPropertyDescriptor } from '../../../../models/domain';
import { PropertyEditor } from '../../property-editor/property-editor';
import { Map, mapSyntaxToLanguage, parseJson } from 'src/utils/utils';
import { SwitchCase } from './models';
import { HTMLElsaExpressionEditorElement, HTMLElsaMultiExpressionEditorElement, IntellisenseContext } from 'src/models/elsa-interfaces';
import { ActivityIconProvider } from 'src/services/activity-icon-provider';

@Component({
  selector: 'alt-switch-case-property',
  templateUrl: './alt-switch-case-property.html',
  standalone: false,
})
export class AltSwitchCaseProperty {
  activityModel = model<ActivityModel>();
  propertyDescriptor = model<ActivityPropertyDescriptor>();
  propertyModel = model<ActivityDefinitionProperty>();

  @ViewChild('multiExpressionEditor') multiExpressionEditor: HTMLElsaMultiExpressionEditorElement;
  @ViewChild('expressionEditor') expressionEditor: HTMLElsaExpressionEditorElement;

  cases = signal<Array<SwitchCase>>([]);

  //// Correctly assigning values to cases at declaration time
  //cases: Array<SwitchCase> = [
  //  {
  //    name: "Case 1",
  //    syntax: "JavaScript",
  //    expressions: {
  //      JavaScript: "x > 5",
  //      Liquid: "{{ x | plus: 5 }}"
  //    }
  //  },
  //  {
  //    name: "Case 2",
  //    syntax: "Liquid",
  //    expressions: {
  //      JavaScript: "y < 10",
  //      Liquid: "{{ y | minus: 10 }}"
  //    }
  //  }
  //];

  valueChange: EventEmitter<Array<any>>;

  supportedSyntaxes: Array<string> = [SyntaxNames.JavaScript, SyntaxNames.Liquid];
  syntaxSwitchCount: number = 0;


  /* COMMENTED OUT AS I MIGHT NOT NEED THIS AT THE MOMENT */
  //defaultSyntax = computed(() => this.propertyDescriptor()?.defaultSyntax || SyntaxNames.Literal);
  //isEncypted = model<boolean>(false);
  //currentValue = computed(() => this.propertyModel()?.expressions[this.defaultSyntax()] || '');
  // fieldId = computed(() => this.propertyDescriptor()?.name ?? 'default');
  //fieldName = computed(() => this.propertyDescriptor()?.name ?? 'default');
  //isReadOnly = computed(() => this.propertyDescriptor()?.isReadOnly ?? false);


  ///* Below code commented out as it causes problems when setting the following code line in the multi-expression-editor: context={context} */
  // context: IntellisenseContext = {
  //   activityTypeName: this.activityModel().type,
  //   propertyName: this.propertyDescriptor().name
  // };

  //expressions = computed(() => {
  //  const model = this.propertyModel();
  //  return model?.expressions ? { ...model.expressions } : {};
  //});

  json: any;
  activityIconProvider: any;

  constructor(activityIconProvider: ActivityIconProvider) {
    this.activityIconProvider = activityIconProvider;
    console.log('Setting property model', this.propertyModel());
  }

  async ngOnInit(): Promise<void> {
    // const propertyModel = this.propertyModel();
    const casesJson = this.propertyModel().expressions[SyntaxNames.Switch]
    let parsedCases = parseJson(casesJson);
     this.cases.set(parsedCases || []);
  }

  updatePropertyModel() {
    this.propertyModel().expressions[SyntaxNames.Switch] = JSON.stringify(this.cases);
    this.multiExpressionEditor.expressions[SyntaxNames.Json] = JSON.stringify(this.cases, null, 2);
  }

  onDefaultSyntaxValueChanged(e: CustomEvent) {
    this.cases = e.detail;
  }

  onAddCaseClick() {
    console.log("Adding Case")
    const caseName = `Case ${this.cases.length + 1}`;
    const newCase = { name: caseName, syntax: SyntaxNames.JavaScript, expressions: { [SyntaxNames.JavaScript]: 'test' } }
    console.log(newCase);
    this.cases.update(cases => [...cases, newCase]);
    //this.updatePropertyModel();
  }

  onTestClick() {
    console.log("This is a Test of the delete button");
  }

  onDeleteCaseClick(switchCase: SwitchCase) {
    console.log("Removing Case", switchCase);
    //Not Being called.
    this.cases.update(() => this.cases().filter(x => x != switchCase));
  }

  onCaseNameChanged(e: Event, switchCase: SwitchCase) {
    switchCase.name = (e.currentTarget as HTMLInputElement).value.trim();
    this.updatePropertyModel();
  }

  onCaseExpressionChanged(e: Event, switchCase: SwitchCase) {
    const detail = (e as CustomEvent<string>).detail;
    switchCase.expressions[switchCase.syntax] = detail;
    this.updatePropertyModel();
  }

  onCaseSyntaxChanged(e: Event, switchCase: SwitchCase, expressionEditor: HTMLElsaExpressionEditorElement) {
    const select = e.currentTarget as HTMLSelectElement;
    switchCase.syntax = select.value;
    expressionEditor.language = mapSyntaxToLanguage(switchCase.syntax);
    this.updatePropertyModel();
  }

  onMultiExpressionEditorValueChanged(e: Event) {
    const detail = (e as CustomEvent<string>).detail;
    const json = detail;
    const parsed = parseJson(json);

    if (!parsed)
      return;

    if (!Array.isArray(parsed))
      return;

    this.propertyModel().expressions[SyntaxNames.Switch] = json;
    this.cases.set(parsed);

  }
  
  onMultiExpressionEditorSyntaxChanged(e: Event) {
    this.syntaxSwitchCount++;
  }

  onMapSyntaxToLanguage(syntax: string): string {
    return mapSyntaxToLanguage(syntax);
  }

  getCases(): any {
    return this.cases().map((switchCase: SwitchCase, index: number) => {
      let expressionEditor = null;

      console.log("Index:", index);
      console.log("Name:", switchCase.name);
      console.log("Expressions:", switchCase.expressions);
      console.log("Expression:", switchCase.expressions[switchCase.syntax]);
      console.log("syntax:", switchCase.syntax);
      console.log("monacoLanguage:", mapSyntaxToLanguage(switchCase.syntax));

      return {
        index: index,
        expression: switchCase.expressions[switchCase.syntax],
        syntax: switchCase.syntax,
        monacoLanguage: mapSyntaxToLanguage(switchCase.syntax),
        name: switchCase.name,
        //onNameChanged: (e: Event) => this.onCaseNameChanged(e, switchCase),
        //onExpressionChanged: (e: CustomEvent<string>) => this.onCaseExpressionChanged(e, switchCase)
      };
    });
  }
}

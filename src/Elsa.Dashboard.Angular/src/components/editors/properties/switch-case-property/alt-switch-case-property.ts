import { Component, computed, EventEmitter, model, OnChanges, OnInit, signal, Signal, SimpleChanges, WritableSignal, ViewChild} from '@angular/core';
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


  @ViewChild('multiExpressionEditorRef') multiExpressionEditor: HTMLElsaMultiExpressionEditorElement;
  @ViewChild('expressionEditorRef') expressionEditor: HTMLElsaExpressionEditorElement;

  cases: WritableSignal<SwitchCase>[] = [];
  context: IntellisenseContext = { activityTypeName: '', propertyName: '' };
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
    console.log("On Init Switch Case");
    this.context.activityTypeName = this.activityModel().type;
    this.context.propertyName = this.propertyDescriptor().name;
    if (parsedCases != null) {
      this.cases = parsedCases.map((c: SwitchCase) => signal(c));
    }


    //this.context.update(x => ({ ...x, activityTypeName: this.activityModel().type }));
    //this.context.update(x => ({ ...x, propertyName: this.propertyDescriptor().name }));
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
    const newCaseSignal = signal<SwitchCase>({
      name: caseName,
      syntax: SyntaxNames.JavaScript,
      expressions: { [SyntaxNames.JavaScript]: '' }
    });
    this.cases.push(newCaseSignal);
    //this.updatePropertyModel();
  }

  onTestClick() {
    console.log("This is a Test of the delete button");
  }

  onDeleteCase(e: Event) {
    console.log("Delete Click Event", e);
  }

  onDeleteCaseClick(e: Event) {
    console.log("Delete Click Event", e);
    
  }

  onCaseSyntaxChanged(e: Event) {
    console.log("Syntax Changed", e);
  }
  onCaseExpressionChanged(e: string) {
    console.log("Expression Changed", e);
  }
  onCaseNameChanged(e: Event) {
    console.log("Name Changed", e);
  }

  //onCaseNameChanged(e: Event, switchCase: SwitchCase) {
  //  console.log("Name Changed Event", e);
  //  console.log("Switch Case", switchCase);
  //  switchCase.name = (e.currentTarget as HTMLInputElement).value.trim();
  //  this.updatePropertyModel();
  //}

  //onCaseExpressionChanged(e: Event, switchCase: SwitchCase) {
  //  const detail = (e as CustomEvent<string>).detail;
  //  switchCase.expressions[switchCase.syntax] = detail;
  //  this.updatePropertyModel();
  //}

  //onCaseSyntaxChanged(e: Event, switchCase: SwitchCase, expressionEditor: HTMLElsaExpressionEditorElement) {
  //  const select = e.currentTarget as HTMLSelectElement;
  //  switchCase.syntax = select.value;
  //  expressionEditor.language = mapSyntaxToLanguage(switchCase.syntax);
  //  this.updatePropertyModel();
  //}

  onMultiExpressionEditorValueChanged(e: Event) {
    const detail = (e as CustomEvent<string>).detail;
    const json = detail;
    const parsed = parseJson(json);

    if (!parsed)
      return;

    if (!Array.isArray(parsed))
      return;

    this.propertyModel().expressions[SyntaxNames.Switch] = json;
    //this.cases.set(parsed);

  }

  onMultiExpressionEditorSyntaxChanged(e: Event) {
    this.syntaxSwitchCount++;
  }

  onMapSyntaxToLanguage(syntax: string): string {
    return mapSyntaxToLanguage(syntax);
  }
}

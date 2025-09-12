import { Component, computed, EventEmitter, input, model, OnChanges, OnInit, signal, SimpleChanges, ViewChild} from '@angular/core';
import { ActivityModel, SyntaxNames } from '../../../../models';
import { ActivityDefinitionProperty, ActivityPropertyDescriptor } from '../../../../models/domain';
import { PropertyEditor } from '../../property-editor/property-editor';
import { Map, mapSyntaxToLanguage, parseJson } from 'src/utils/utils';
import { SwitchCase } from './models';
import { HTMLElsaExpressionEditorElement, HTMLElsaMultiExpressionEditorElement, IntellisenseContext } from 'src/models/elsa-interfaces';
import { ActivityIconProvider } from 'src/services/activity-icon-provider';

@Component({
  selector: 'switch-case-row',
  templateUrl: './switch-case-row.html',
  standalone: false,
})
export class SwitchCaseRow {
  //switchCase = model<SwitchCase>();
  switchCase = model<SwitchCase>();
  caseDelete = new EventEmitter<SwitchCase>();
  @ViewChild('expressionEditor') expressionEditor: HTMLElsaExpressionEditorElement;

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
  supportedSyntaxes: Array<string> = [SyntaxNames.JavaScript, SyntaxNames.Liquid];

  activityIconProvider: ActivityIconProvider;

  constructor(activityIconProvider: ActivityIconProvider) {
    this.activityIconProvider = activityIconProvider;
  }

  async ngOnInit(): Promise<void> {
    //this.switchCase.set(this.switchCaseElement);
    // const propertyModel = this.propertyModel();
  }

  onDeleteCaseClick(e: Event) {
    this.caseDelete.emit(this.switchCase)
  }

  onCaseExpressionChanged(e: Event) {
    console.log('Case expression changed', e);
  }

  onNameChanged(e: Event) {
    console.log('Case name changed', e);
  };

  onCaseSyntaxChanged(e: Event) {
    console.log('Case syntax changed', e);
  }


  onMapSyntaxToLanguage(syntax: string): string {
    return mapSyntaxToLanguage(syntax);
  }
}

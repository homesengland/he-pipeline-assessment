import { HttpClient } from '@angular/common/http';
import { Component, OnInit, viewChild, ViewChild } from '@angular/core';
import { Store } from '@ngrx/store';
import { selectMonacoLibPath, selectServerUrl } from '../../../state/selectors/app.state.selectors';
import { WorkflowDefinitionListScreen } from '../../../screens/workflow-definition-list/workflow-definition-list-screen/workflow-definition-list-screen';

@Component({
  selector: 'workflow-definitions-list',
  templateUrl: './workflow-definitions-list.html',
  styleUrls: ['./workflow-definitions-list.css'],
  standalone: false,
})
export class WorkflowDefinitionsList implements OnInit {
  serverUrl: string;
  monacoLibPath: string;
  title = 'Workflow Definitions';
  readonly WorkflowDefinitionListScreen = viewChild('WorkflowDefinitionListScreen');

  constructor(private http: HttpClient, private store: Store) {}
  ngOnInit(): void {
    this.store.select(selectServerUrl).subscribe(data => {
      this.serverUrl = data;
    });
    this.store.select(selectMonacoLibPath).subscribe(data => {
      this.monacoLibPath = data;
    });
  }
}

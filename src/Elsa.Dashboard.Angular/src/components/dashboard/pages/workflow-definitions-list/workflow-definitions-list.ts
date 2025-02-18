import { HttpClient } from '@angular/common/http';
import { Component, OnInit, ViewContainerRef, ViewChild } from '@angular/core';
import { Store } from '@ngrx/store';
import { selectMonacoLibPath, selectServerUrl } from '../../../state/selectors/app.state.selectors';
import { HelloWorldComponent } from '../../../hello-world-component';
import { activityIconProvider } from '../../../../services/activity-icon-provider';
import { WorkflowDashboard } from '../workflow-dashboard/workflow-dashboard';

@Component({
  selector: 'workflow-definitions-list',
  templateUrl: './workflow-definitions-list.html',
  styleUrls: ['./workflow-definitions-list.css'],
})
export class WorkflowDefinitionsList implements OnInit {
  serverUrl: string;
  monacoLibPath: string;
  userInput: string = '';

  @ViewChild('helloWorldContainer', { read: ViewContainerRef }) helloWorldContainer: ViewContainerRef;
  @ViewChild('workflowDashboardContainer', { read: ViewContainerRef }) workflowDashboardContainer: ViewContainerRef;

  constructor(private http: HttpClient, private store: Store) { }
  ngOnInit(): void {
    this.store.select(selectServerUrl).subscribe(data => {
      this.serverUrl = data;
    });
    this.store.select(selectMonacoLibPath).subscribe(data => {
      this.monacoLibPath = data
    });
    //this.viewContainer.createComponent(HelloWorldComponent);
    //this.viewContainer.createComponent(WorkflowDashboard);
  }

  onRadioChange(value: string): void {
    this.helloWorldContainer.clear();
    if (value === 'yes') {
      this.helloWorldContainer.createComponent(HelloWorldComponent);
    }
  }

  onInputChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    const value = input.value;
    this.workflowDashboardContainer.clear();
    if (value === 'TEST') {
      this.workflowDashboardContainer.createComponent(WorkflowDashboard);
    }
  }
}


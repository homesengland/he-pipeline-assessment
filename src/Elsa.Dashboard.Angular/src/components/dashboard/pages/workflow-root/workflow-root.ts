import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, Input, OnInit } from '@angular/core';
import { WorkflowDashboard} from '../workflow-dashboard/workflow-dashboard'
import { Store } from '@ngrx/store';
import { AppStateActionGroup } from '../../../state/actions/app.state.actions';

@Component({
  selector: 'workflow-root',
  templateUrl: './workflow-root.html',
  styleUrls: ['./workflow-root.css'],
  imports: [WorkflowDashboard]
})
export class WorkflowRoot implements OnInit {
  serverUrl: string;
  monacoLibPath: string;

  constructor(private http: HttpClient, el: ElementRef, private store: Store) {
    this.serverUrl = el.nativeElement.getAttribute('serverUrl');
    this.monacoLibPath = el.nativeElement.getAttribute('monacoLibPath');
    this.store.dispatch(AppStateActionGroup.setExternalState({ serverUrl: this.serverUrl, monacoLibPath: this.monacoLibPath }));

  }

  ngOnInit(): void {

  }

}


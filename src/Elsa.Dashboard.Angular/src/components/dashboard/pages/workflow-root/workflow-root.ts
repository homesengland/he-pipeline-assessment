import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, Input, OnInit } from '@angular/core';
import { WorkflowDashboard} from '../workflow-dashboard/workflow-dashboard'
import { Store } from '@ngrx/store';
import { AppStateActionGroup } from '../../../state/actions/app.state.actions';

@Component({
  selector: 'workflow-root',
  templateUrl: './workflow-root.html',
  styleUrls: ['./workflow-root.css'],
  standalone: false
})
export class WorkflowRoot implements OnInit {
  serverUrl: string;
  monacoLibPath: string;
  domain: string;
  clientId: string;
  audience: string;
  basePath: string;
  constructor(private http: HttpClient, el: ElementRef, private store: Store) {
    this.serverUrl = el.nativeElement.getAttribute('serverUrl');
    this.monacoLibPath = el.nativeElement.getAttribute('monacoLibPath');
    this.domain = el.nativeElement.getAttribute('domain');
    this.clientId = el.nativeElement.getAttribute('clientId');
    this.audience = el.nativeElement.getAttribute('audience');

    this.store.dispatch(AppStateActionGroup.setExternalState({ serverUrl: this.serverUrl, monacoLibPath: this.monacoLibPath, auth0Domain: this.domain, auth0ClientId: this.clientId, auth0Audience: this.audience, basePath: this.basePath }));
    
  }

  ngOnInit(): void {

  }

}


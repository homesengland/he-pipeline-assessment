import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { selectMonacoLibPath, selectServerUrl } from '../../../state/selectors/app.state.selectors';

@Component({
  selector: 'workflow-definitions-list',
  templateUrl: './workflow-definitions-list.html',
  styleUrls: ['./workflow-definitions-list.css'],
  standalone: false
})
export class WorkflowDefinitionsList implements OnInit {
  serverUrl: string;
  monacoLibPath: string;

  constructor(private http: HttpClient, private store: Store) {}
    ngOnInit(): void {
    this.store.select(selectServerUrl).subscribe(data => {
      this.serverUrl = data;
    });
    this.store.select(selectMonacoLibPath).subscribe(data => {
      this.monacoLibPath = data
    });
    }

}


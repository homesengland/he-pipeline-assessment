import { HttpClient } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'workflow-user-context-menu',
  templateUrl: './workflow-user-context-menu.html',
  styleUrls: ['./workflow-user-context-menu.css'],
})
export class WorkflowUserContextMenu implements OnInit {
  @Input() serverUrl: string;

  constructor(private http: HttpClient) {}
    ngOnInit(): void {

    }

}


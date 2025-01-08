import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'workflow-definitions-list',
  templateUrl: './workflow-definitions-list.html',
  styleUrls: ['./workflow-definitions-list.css'],
})
export class WorkflowDefinitionsList implements OnInit {

  constructor(private http: HttpClient) {}
    ngOnInit(): void {

    }

}


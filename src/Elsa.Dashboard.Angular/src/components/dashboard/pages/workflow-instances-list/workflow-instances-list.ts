import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'workflow-instances-list',
  templateUrl: './workflow-instances-list.html',
  styleUrls: ['./workflow-instances-list.css'],
})
export class WorkflowInstancesList implements OnInit {

  constructor(private http: HttpClient) {}
    ngOnInit(): void {

    }

}


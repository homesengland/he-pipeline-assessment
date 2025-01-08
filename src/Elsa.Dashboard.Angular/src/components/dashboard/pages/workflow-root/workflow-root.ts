import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';


@Component({
  selector: 'workflow-root',
  templateUrl: './workflow-root.html',
  styleUrls: ['./workflow-root.css'],
})
export class WorkflowRoot implements OnInit {

  constructor(private http: HttpClient) {}
    ngOnInit(): void {

    }

}


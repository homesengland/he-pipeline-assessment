import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'workflow-home',
  templateUrl: './workflow-home.html',
  styleUrls: ['./workflow-home.css'],
})
export class WorkflowHome implements OnInit {

  constructor(private http: HttpClient) {}
    ngOnInit(): void {

    }

}


import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'workflow-instances-view',
  templateUrl: './workflow-instances-view.html',
  styleUrls: ['./workflow-instances-view.css'],
  standalone: false
})
export class WorkflowInstancesView implements OnInit {

  constructor(private http: HttpClient) {}
    ngOnInit(): void {

    }

}


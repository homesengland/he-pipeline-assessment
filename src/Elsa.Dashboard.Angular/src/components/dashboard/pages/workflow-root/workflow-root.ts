import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { WorkflowDashboard} from '../workflow-dashboard/workflow-dashboard'

@Component({
  selector: 'workflow-root',
  templateUrl: './workflow-root.html',
  styleUrls: ['./workflow-root.css'],
  imports: [WorkflowDashboard]
})
export class WorkflowRoot implements OnInit {

  constructor(private http: HttpClient) {}
    ngOnInit(): void {

    }

}


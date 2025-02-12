import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { WorkflowInstanceListScreen } from '../../../screens/workflow-instance-list/workflow-instance-list-screen/workflow-instance-list-screen';

@Component({
  selector: 'workflow-instances-list',
  templateUrl: './workflow-instances-list.html',
  styleUrls: ['./workflow-instances-list.css'],
    standalone: false
})
export class WorkflowInstancesList implements OnInit {
  title = "Workflow Instances";
  constructor(private http: HttpClient) {}
    ngOnInit(): void {

    }

}


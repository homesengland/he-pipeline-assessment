import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { WorkflowInstanceViewerScreen } from 'src/components/screens/workflow-instance-viewer/workflow-instance-viewer-screen/workflow-instance-viewer-screen';

@Component({
  selector: 'workflow-instances-view',
  templateUrl: './workflow-instances-view.html',
  styleUrls: ['./workflow-instances-view.css'],
  standalone: true,
  imports: [WorkflowInstanceViewerScreen],
})
export class WorkflowInstancesView implements OnInit {
  private route: ActivatedRoute;
  instanceId: string | null;

  constructor(private http: HttpClient, route: ActivatedRoute) {
    this.route = route;
  }

  ngOnInit(): void {
    this.instanceId = this.route.snapshot.params['id'];
  }
}

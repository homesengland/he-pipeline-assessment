import { Input, Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'workflow-dashboard',
  templateUrl: './workflow-dashboard.html',
  styleUrls: ['./workflow-dashboard.css'],
  standalone: true,
  imports: [RouterModule],
})
export class WorkflowDashboard implements OnInit {
  @Input() basePath: string = '/';
  logoSrc = 'static/images/logo.png';
  menuItems: [string, string][] = [
    ['workflow-definitions', 'Workflow Definitions'],
    ['workflow-instances', 'Workflow Instances'],
    ['workflow-registry', 'Workflow Registry'],
  ];

  ngOnInit() {}
}

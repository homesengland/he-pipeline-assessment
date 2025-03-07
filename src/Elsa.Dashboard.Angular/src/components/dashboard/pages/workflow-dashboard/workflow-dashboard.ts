import { Input, Component, OnInit } from '@angular/core';

@Component({
  selector: 'workflow-dashboard',
  templateUrl: './workflow-dashboard.html',
  styleUrls: ['./workflow-dashboard.css'],
  standalone: false
})
export class WorkflowDashboard implements OnInit {
  @Input() basePath: string = "/";
  logoSrc = "static/images/logo.png";
  menuItems: [string, string][] = [
    ['workflow-definitions', 'Workflow Definitions'],
    ['workflow-instances', 'Workflow Instances'],
    ['workflow-registry', 'Workflow Registry'],
  ];
  placeholderItems: [string, string][] = [
    ['workflow-placeholder', 'Workflow Placeholder'],
  ];
  placeholderWorkflowId: string = '5e4506339c934e199a17ca7a2e44f874';

  ngOnInit() {

  }
}





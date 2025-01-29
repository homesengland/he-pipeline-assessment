import { Input, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, RouterOutlet } from '@angular/router';

@Component({
  selector: 'workflow-dashboard',
  templateUrl: './workflow-dashboard.html',
  styleUrls: ['./workflow-dashboard.css'],
  imports: [CommonModule, RouterModule, RouterOutlet]
})
export class WorkflowDashboard implements OnInit {
  @Input() basePath: string = "/";
  logoSrc = "static/images/logo.png";
  menuItems: [string, string][] = [
    ['workflow-definitions', 'Workflow Definitions'],
    ['workflow-instances', 'Workflow Instances'],
    ['workflow-registry', 'Workflow Registry'],
    ['workflow-placeholder', 'Workflow Placeholder']
  ];

  ngOnInit() {

  }
}





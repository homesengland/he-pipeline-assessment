import { Input, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'workflow-dashboard',
  templateUrl: './workflow-dashboard.html',
  styleUrls: ['./workflow-dashboard.css'],
  imports: [CommonModule, RouterModule]
})
export class WorkflowDashboard implements OnInit {
  @Input() basePath: string = '';

  logoPath = './assets/logo.png';
  menuItems: [string, string][] = [
    ['workflow-definitions', 'Workflow Definitions'],
    ['workflow-instances', 'Workflow Instances'],
    ['workflow-registry', 'Workflow Registry']
  ];

  ngOnInit() {

  }
}





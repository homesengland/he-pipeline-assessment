import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'workflow-definitions-view',
  templateUrl: './workflow-definitions-view.html',
  styleUrls: ['./workflow-definitions-view.css'],
  standalone: true,
})
export class WorkflowDefinitionsView implements OnInit {
  constructor(private http: HttpClient) {}
  ngOnInit(): void {}
}

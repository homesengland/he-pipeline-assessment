import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'workflow-registry',
  templateUrl: './workflow-registry.html',
  styleUrls: ['./workflow-registry.css'],
  standalone: true,
})
export class WorkflowRegistry implements OnInit {
  constructor(private http: HttpClient) {}
  ngOnInit(): void {}
}

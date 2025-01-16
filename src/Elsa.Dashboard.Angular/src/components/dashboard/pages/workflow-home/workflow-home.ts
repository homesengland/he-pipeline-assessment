import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'workflow-home',
  templateUrl: './workflow-home.html',
  styleUrls: ['./workflow-home.css'],
  imports: [CommonModule]
})
export class WorkflowHome implements OnInit {
  visualPath: string = "static/images/undraw_breaking_barriers_vnf3.svg";
  constructor(private http: HttpClient) {
  }
    ngOnInit(): void {

    }

}


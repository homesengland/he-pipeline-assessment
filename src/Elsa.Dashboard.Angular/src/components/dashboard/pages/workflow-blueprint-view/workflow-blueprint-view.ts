import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'workflow-blueprint-view',
  templateUrl: './workflow-blueprint-view.html',
  styleUrls: ['./workflow-blueprint-view.css'],
  standalone: false
})
export class WorkflowBlueprintView implements OnInit {

  constructor(private http: HttpClient) {}
    ngOnInit(): void {

    }

}


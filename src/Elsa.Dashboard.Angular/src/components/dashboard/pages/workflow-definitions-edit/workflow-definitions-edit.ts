import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'workflow-definitions-edit',
  templateUrl: './workflow-definitions-edit.html',
  standalone: false,
})
export class WorkflowDefinitionEdit implements OnInit {
  private route: ActivatedRoute;
  id: string | null;

  constructor(route: ActivatedRoute) {
    this.route = route;
  }

  ngOnInit(): void {
    this.id = this.route.snapshot.params['id'];
  }
}

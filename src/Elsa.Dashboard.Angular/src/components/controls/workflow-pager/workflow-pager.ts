import { HttpClient } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'workflow-pager',
  templateUrl: './workflow-pager.html',
  styleUrls: ['./workflow-pager.css'],
  standalone: false
})
export class WorkflowPager implements OnInit {
  @Input() page: number;
  @Input() pageSize: number;
  @Input() totalCount: number;
  @Input() location: Location;
  basePath: string;
  from: number;
  to: number;
  math: Math;

  constructor(private http: HttpClient) {
    this.math = Math;
  }

    ngOnInit(): void {
      this.basePath = !!this.location ? this.location.pathname : document.location.pathname;
      this.from = this.page * this.pageSize + 1;
      console.log(this.page);
      console.log(this.pageSize);
      console.log(this.totalCount);
      this.to = Math.min(this.page + this.pageSize - 1, this.totalCount);
    }

  onNavigateClick(e: Event, page: number) {
    const anchor = e.currentTarget as HTMLAnchorElement;

    e.preventDefault();
    this.navigate(`${anchor.pathname}${anchor.search}`, page);
  }

  navigate(path: string, page: number) {

  }

  getNavUrl = (page: number) => {
    return ``;
  };
}


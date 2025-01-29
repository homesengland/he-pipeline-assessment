import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { parseQuery, queryToString } from "../../../utils/utils";
import { Location } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import * as _ from 'lodash';

export interface PagerData {
  page: number;
  pageSize: number;
  totalCount: number;
}

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

  basePath: string;
  from: number;
  to: number;
  math: Math;
  chevronLeftHref: string;
  history: Array<string> = new Array<string>();

  @Output() paged = new EventEmitter<PagerData>();
  loadash: any;

  constructor(private http: HttpClient, private location: Location, private router: Router, private activatedRoute: ActivatedRoute) {
    this.math = Math;
    this.loadash = _;
  }

  ngOnInit(): void {
    this.basePath = "";
    }

  onNavigateClick(e: Event, page: number) {
    const anchor = e.currentTarget as HTMLAnchorElement;

    e.preventDefault();
    let query = parseQuery(anchor.search);
    this.navigate(`${anchor.pathname}${anchor.search}`, page);
  }

  navigate(path: string, page: number) {
     if (this.location) {
      this.location.replaceState(path);
      return;
    }
    else {
      this.paged.emit({ page, pageSize: this.pageSize, totalCount: this.totalCount });
    }
  }

  getNavUrl(page: number) {
    let pageSize = this.pageSize;
    let queryString = this.router.url.split('?')[1];
    const currentQuery = queryString != null ? parseQuery(queryString) : { page, pageSize };
    const query = { ...currentQuery, 'page': page };
    return `${this.basePath}?${queryToString(query)}`;
  };

  log(text: string, obj: any) {
    console.log(text, obj);
  }
}


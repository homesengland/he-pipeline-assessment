import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { parseQuery, queryToString } from "../../../utils/utils";
import { Location } from '@angular/common';
import { selectStoreConfig } from '../../state/selectors/app.state.selectors';
import * as _ from 'lodash';
import { Store } from '@ngrx/store';
import { Router } from '@angular/router';

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
  loadash: any;

  @Output() paged = new EventEmitter<PagerData>();

  constructor(private http: HttpClient, private location: Location, private router: Router, private store: Store) {
    this.math = Math;
    this.loadash = _;
  }

  ngOnInit(): void {
    this.store.select(selectStoreConfig).subscribe(data => {
      this.basePath = data.basePath ? data.basePath : "";
    });
  }

  onNavigateClick(e: Event, page: number) {
    const anchor = e.currentTarget as HTMLAnchorElement;

    e.preventDefault();
    this.navigate(`${anchor.pathname}${anchor.search}`, page);
  }

  navigate(path: string, page: number) {
    if (this.location) {
      this.location.go(path);
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
}


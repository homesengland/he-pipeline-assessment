import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, OnInit, Output, computed, input } from '@angular/core';
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
  readonly page = input<number>(undefined);
  readonly pageSize = input<number>(undefined);
  readonly totalCount = input<number>(undefined);

    basePath: string;
    from = computed(() => this.page() * this.pageSize() + 1)
    to = computed(() => Math.min(this.from() + this.pageSize() - 1, this.totalCount()));
    pageCount = computed(() => Math.round(((this.totalCount() - 1) / this.pageSize()) + 0.5));
    maxPageButtons = 10;
    fromPage = computed(()=> Math.max(0, ((this.page())) - this.maxPageButtons / 2));
    toPage = computed(() => Math.min(this.pageCount(), this.fromPage() + this.maxPageButtons));
    currentPages = computed(()=> _.range(this.fromPage(), this.toPage()))
  chevronLeftHref: string;
  history: Array<string> = new Array<string>();

  @Output() paged = new EventEmitter<PagerData>();

  constructor(private http: HttpClient, private location: Location, private router: Router, private store: Store) {
0  }

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
      this.paged.emit({ page, pageSize: this.pageSize(), totalCount: this.totalCount() });
    }
  }

  getNavUrl(page: number) {
    let pageSize = this.pageSize();
    let queryString = this.router.url.split('?')[1];
    const currentQuery = queryString != null ? parseQuery(queryString) : { page, pageSize };
    const query = { ...currentQuery, 'page': page };
    return `${this.basePath}?${queryToString(query)}`;
  };
}


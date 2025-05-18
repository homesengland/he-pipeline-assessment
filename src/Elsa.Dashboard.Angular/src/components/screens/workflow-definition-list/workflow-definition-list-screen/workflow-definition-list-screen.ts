import { HttpClient } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { Router, NavigationEnd, RouterLink } from '@angular/router';
import { Location } from '@angular/common';
import { Store } from '@ngrx/store';
import { ElsaClientService, ElsaClient } from '../../../../services/elsa-client';
import { OrderBy, PagedList, VersionOptions, WorkflowDefinitionSummary } from '../../../../models';
import { parseQuery } from '../../../../utils/utils';
import { selectServerUrl, selectStoreConfig } from '../../../state/selectors/app.state.selectors';
import { HTMLElsaConfirmDialogElement, MenuItem } from 'src/models/elsa-interfaces';
import { BrowserModule } from '@angular/platform-browser';
import { WorkflowContextMenu } from 'src/components/controls/workflow-context-menu/workflow-context-menu';
import { WorkflowPager } from 'src/components/controls/workflow-pager/workflow-pager';

@Component({
  selector: 'workflow-definition-list-screen',
  templateUrl: './workflow-definition-list-screen.html',
  styleUrls: ['./workflow-definition-list-screen.css'],
  standalone: true,
  imports: [WorkflowContextMenu, WorkflowPager, ReactiveFormsModule, RouterLink],
})
export class WorkflowDefinitionListScreen implements OnInit, OnDestroy {
  static readonly DEFAULT_PAGE_SIZE = 15;
  static readonly MIN_PAGE_SIZE = 5;
  static readonly MAX_PAGE_SIZE = 100;
  static readonly START_PAGE = 0;
  static readonly ORDER_BY_VALUES: Array<OrderBy> = [OrderBy.Finished, OrderBy.LastExecuted, OrderBy.Started];
  static readonly PAGE_SIZES: Array<number> = [5, 10, 15, 20, 30, 50, 100];

  serverUrl: string;
  basePath: string;

  searchForm: FormGroup = this.formBuilder.group({
    searchTerm: '',
  });

  workflowDefinitionsTableValues: PagedList<WorkflowDefinitionSummaryTableRow> = { items: [], page: 1, pageSize: 50, totalCount: 0 };
  workflowDefintions: PagedList<WorkflowDefinitionSummary> = { items: [], page: 1, pageSize: 50, totalCount: 0 };

  confirmDialogue: HTMLElsaConfirmDialogElement;

  currentSearchTerm: string;
  currentPage: number = 0;
  currentPageSize: number = WorkflowDefinitionListScreen.DEFAULT_PAGE_SIZE;
  totalCount: number = 0;
  latestOrPublishedVersionOptions: VersionOptions;

  private clearRouteChangedListeners: () => void;

  constructor(
    private http: HttpClient,
    private formBuilder: FormBuilder,
    private elsaClientService: ElsaClientService,
    private store: Store,
    private router: Router,
    private location: Location,
  ) {}

  async ngOnInit(): Promise<void> {
    this.clearRouteChangedListeners = this.location.onUrlChange(async (url, state) => {
      if (url.split('?')[0].endsWith('/workflow-definitions')) {
        let queryString = url.split('?')[1] ? url.split('?')[1] : '';
        this.applyQueryString(queryString);
        await this.loadWorkflowDefinitions();
      }
    });

    this.setVariablesFromAppState();

    if (this.router.navigated) {
      let queryString = this.router.url.split('?')[1];
      this.applyQueryString(queryString);
    }

    this.router.events.subscribe(async event => {
      if (event instanceof NavigationEnd) {
        let queryString = this.router.url.split('?')[1];
        this.applyQueryString(queryString);
        await this.loadWorkflowDefinitions();
      }
    });

    await this.loadWorkflowDefinitions();
  }

  ngOnDestroy(): void {
    if (this.clearRouteChangedListeners) {
      this.clearRouteChangedListeners();
    }
  }

  private setVariablesFromAppState(): void {
    this.store.select(selectStoreConfig).subscribe(data => {
      this.basePath = data.basePath ? data.basePath : '';
    });

    this.store.select(selectServerUrl).subscribe(data => {
      this.serverUrl = data;
    });
  }

  private applyQueryString(queryString?: string): void {
    const query = parseQuery(queryString);
    console.log('Parsed query:', query);
    this.currentPage = parseInt(query['page']) || WorkflowDefinitionListScreen.START_PAGE;

    this.currentPageSize = Math.max(
      Math.min(parseInt(query['pageSize']) || WorkflowDefinitionListScreen.DEFAULT_PAGE_SIZE, WorkflowDefinitionListScreen.MAX_PAGE_SIZE),
      WorkflowDefinitionListScreen.MIN_PAGE_SIZE,
    );

    console.log('Set currentPage:', this.currentPage);
    console.log('Set currentPageSize:', this.currentPageSize);
  }

  async onSearchSubmit(): Promise<void> {
    this.currentSearchTerm = this.searchForm.value.searchTerm;
    await this.loadWorkflowDefinitions();
  }

  private async loadWorkflowDefinitions() {
    const elsaClient = await this.createClient();
    this.currentPage = isNaN(this.currentPage) ? WorkflowDefinitionListScreen.START_PAGE : this.currentPage;
    this.currentPage = Math.max(this.currentPage, WorkflowDefinitionListScreen.START_PAGE);
    this.currentPageSize = isNaN(this.currentPageSize) ? WorkflowDefinitionListScreen.DEFAULT_PAGE_SIZE : this.currentPageSize;
    this.latestOrPublishedVersionOptions = { isLatestOrPublished: true };
    this.workflowDefintions = await elsaClient.workflowDefinitionsApi.list(this.currentPage, this.currentPageSize, this.latestOrPublishedVersionOptions, this.currentSearchTerm);
    this.totalCount = this.workflowDefintions.totalCount;
    this.workflowDefinitionsTableValues = this.setTableValues(this.workflowDefintions);
    const maxPage = Math.floor(this.workflowDefintions.totalCount / this.currentPageSize);

    if (this.currentPage > maxPage) {
      this.currentPage = maxPage;
      this.workflowDefintions = await elsaClient.workflowDefinitionsApi.list(this.currentPage, this.currentPageSize, this.latestOrPublishedVersionOptions, this.currentSearchTerm);
      this.workflowDefinitionsTableValues = this.setTableValues(this.workflowDefintions);
      this.totalCount = this.workflowDefintions.totalCount;
    }
  }

  createClient(): Promise<ElsaClient> {
    return this.elsaClientService.createElsaClient(this.serverUrl);
  }

  setTableValues(workflowDefinitions: PagedList<WorkflowDefinitionSummary>): PagedList<WorkflowDefinitionSummaryTableRow> {
    const latestWorkflowDefinition = workflowDefinitions.items.filter(item => item.isLatest);
    const publishedDefinitions = workflowDefinitions.items.filter(x => x.isPublished);

    let rows = latestWorkflowDefinition.map(item => {
      const latestVersionNumber = item.version;
      const { isPublished } = item;
      const publishedVersion: WorkflowDefinitionSummary = isPublished ? item : publishedDefinitions.find(x => x.definitionId == item.definitionId);
      const publishedVersionNumber = !!publishedVersion ? publishedVersion.version : '-';
      let workflowDisplayName = item.displayName;
      if (!workflowDisplayName || workflowDisplayName.trim().length == 0) workflowDisplayName = item.name;

      if (!workflowDisplayName || workflowDisplayName.trim().length == 0) workflowDisplayName = 'Untitled';

      const editUrl = `/workflow-definitions/${item.definitionId}`;
      const instancesUrl = `/workflow-instances`;
      const instancesUrlQueryParams = item.definitionId;
      const contextMenuItems: Array<MenuItem> = [
        { text: 'Edit', anchorUrl: `/workflow-definitions/${item.id}`, icon: 'static/images/instances-view-icon.svg' },
        {
          text: isPublished ? 'Unpublish' : 'Publish',
          clickHandler: isPublished ? e => this.onUnPublishClick(e, item) : e => this.onPublishClick(e, item),
          icon: isPublished ? 'static/images/unpublish-icon.svg' : 'static/images/publish-icon.svg',
        },
      ];

      return {
        WorkflowDefinitionSummary: item,
        workflowDefinitionId: item.definitionId,
        displayName: workflowDisplayName,
        version: latestVersionNumber,
        publishedVersion: publishedVersionNumber,
        isPublished: isPublished,
        editUrl: editUrl,
        instancesUrl: instancesUrl,
        instancesUrlQueryParams: instancesUrlQueryParams,
        contextMenuItems: contextMenuItems,
      };
    });
    return {
      items: rows,
      totalCount: workflowDefinitions.totalCount,
      page: workflowDefinitions.page,
      pageSize: workflowDefinitions.pageSize,
    };
  }

  onPaged = async (e: PagerData) => {
    console.log('PagerData:', e);
    this.currentPage = e.page;
    this.currentPageSize = e.pageSize;

    await this.router.navigate([], {
      queryParams: {
        page: this.currentPage,
        pageSize: this.currentPageSize,
      },
      queryParamsHandling: 'merge',
    });

    await this.loadWorkflowDefinitions();
  };

  async onPublishClick(e: Event, workflowDefinition: WorkflowDefinitionSummary) {
    const elsaClient = await this.createClient();
    await elsaClient.workflowDefinitionsApi.publish(workflowDefinition.definitionId);
    await this.loadWorkflowDefinitions();
  }

  async onUnPublishClick(e: Event, workflowDefinition: WorkflowDefinitionSummary) {
    const elsaClient = await this.createClient();
    await elsaClient.workflowDefinitionsApi.retract(workflowDefinition.definitionId);
    await this.loadWorkflowDefinitions();
  }
}

export interface WorkflowDefinitionSummaryTableRow {
  WorkflowDefinitionSummary: WorkflowDefinitionSummary;
  workflowDefinitionId: string;
  displayName: string;
  version: number | string;
  publishedVersion: number | string;
  isPublished: boolean;
  editUrl: string;
  instancesUrl: string;
  instancesUrlQueryParams: string;
  contextMenuItems: Array<MenuItem>;
}

export interface PagerData {
  page: number;
  pageSize: number;
  totalCount: number;
}

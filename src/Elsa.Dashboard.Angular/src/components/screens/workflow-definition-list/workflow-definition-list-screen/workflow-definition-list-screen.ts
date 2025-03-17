import { HttpClient } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { Location } from '@angular/common';
import { Store } from '@ngrx/store';
import { ElsaClientService, ElsaClient } from '../../../../services/elsa-client';
import { OrderBy, PagedList, VersionOptions, WorkflowDefinitionSummary } from '../../../../models';
import { parseQuery } from '../../../../utils/utils';
import { selectServerUrl, selectStoreConfig } from '../../../state/selectors/app.state.selectors';
import { HTMLElsaConfirmDialogElement } from 'src/Models/elsa-interfaces';

@Component({
  selector: 'workflow-definition-list-screen',
  templateUrl: './workflow-definition-list-screen.html',
  styleUrls: ['./workflow-definition-list-screen.css'],
  standalone: false,
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
    this.setVariablesFromAppState();

    this.clearRouteChangedListeners = this.location.onUrlChange(async (url, state) => {
      if (url.split('?')[0] != '/workflow-definitions') return;
      let queryString = url.split('?')[1] ? url.split('?')[1] : '';
      this.applyQueryString(queryString);
      await this.loadWorkflowDefinitions();
    });

    if (this.router.navigated) {
      let queryString = this.router.url.split('?')[1];
      this.applyQueryString(queryString);
    }
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
    this.currentPage = !!query['page'] ? parseInt(query['page']) : 0;
    this.currentPage = isNaN(this.currentPage) ? WorkflowDefinitionListScreen.START_PAGE : this.currentPage;
    this.currentPageSize = !!query['pageSize'] ? parseInt(query['pageSize']) : WorkflowDefinitionListScreen.DEFAULT_PAGE_SIZE;
    this.currentPageSize = isNaN(this.currentPageSize) ? WorkflowDefinitionListScreen.DEFAULT_PAGE_SIZE : this.currentPageSize;
    this.currentPageSize = Math.max(Math.min(this.currentPageSize, WorkflowDefinitionListScreen.MAX_PAGE_SIZE), WorkflowDefinitionListScreen.MIN_PAGE_SIZE);
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

  // ***** MODIFY FOR DEFINITIONS INSTEAD OF INSTANCES, COPIED FROM workflow-instance-list-screen.ts *****

  setTableValues(workflowInstances: PagedList<WorkflowDefinitionSummary>): PagedList<WorkflowDefinitionSummaryTableRow> {
    const latestWorkflowDefinition = workflowInstances.items.filter(item => item.isLatest);
    const publishedDefinitions = workflowInstances.items.filter(x => x.isPublished);

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
      };
    });
    return {
      items: rows,
      totalCount: workflowInstances.totalCount,
      page: workflowInstances.page,
      pageSize: workflowInstances.pageSize,
    };
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
}

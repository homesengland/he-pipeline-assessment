import { HttpClient } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { Location } from '@angular/common';
import { Store } from '@ngrx/store';
import { ElsaClientService, ElsaClient } from '../../../../services/elsa-client';
import { OrderBy, PagedList, VersionOptions, WorkflowDefinitionSummary } from '../../../../models';
import { parseQuery } from '../../../../utils/utils';
import { selectStoreConfig } from '../../../state/selectors/app.state.selectors';
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
    this.clearRouteChangedListeners = this.location.onUrlChange(async (url, state) => {
      let queryString = url.split('?')[1] ? url.split('?')[1] : '';
      this.applyQueryString(queryString);
      await this.loadWorkflowDefinitions();
    });

    this.setVariablesFromAppState();

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

    this.store.select(selectStoreConfig).subscribe(data => {
      this.basePath = data.basePath ? data.basePath : "";
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

    this.setSelectAllIndeterminateState();
  }

  createClient(): Promise<ElsaClient> {
    return this.elsaClientService.createElsaClient(this.serverUrl);
  }

  // ***** MODIFY FOR DEFINITIONS INSTEAD OF INSTANCES, COPIED FROM workflow-instance-list-screen.ts *****

//   setTableValues(workflowInstances: PagedList<WorkflowInstanceSummary>): PagedList<WorkflowInstanceSummaryTableRow> {
  
//       let rows = workflowInstances.items.map(item => {
//         const isSelected = this.selectedWorkflowInstanceIds.findIndex(x => x === item.id) >= 0;
//         const workflowBlueprint = this.workflowBlueprints.find(x => x.versionId == item.definitionVersionId) ?? {
//           name: 'Not Found',
//           displayName: '(Workflow definition not found)'
//         };
//         return {
//           workflowInstanceSummary: item,
//           isSelected: isSelected,
//           correlationId: !!item.correlationId ? item.correlationId : '',
//           blueprintViewUrl: `${this.basePath}/workflow-registry/${item.definitionId}`,
//           displayName: workflowBlueprint.displayName || workflowBlueprint.name || '(Untitled)',
//           instanceName: !item.name ? '' : item.name,
//           createdAt: moment(item.createdAt).format('DD-MM-YYYY HH:mm:ss'),
//           finishedAt: !!moment(item.finishedAt) ? moment(item.finishedAt).format('DD-MM-YYYY HH:mm:ss') : '-',
//           lastExecutedAt: !!moment(item.lastExecutedAt) ? moment(item.lastExecutedAt).format('DD-MM-YYYY HH:mm:ss') : '-',
//           faultedAt: !!moment(item.faultedAt) ? moment(item.faultedAt).format('DD-MM-YYYY HH:mm:ss') : '-',
//           instanceViewUrl: `/workflow-instances/${item.id}`,
//           correlationListViewUrl: `/workflow-registry/${item.definitionId}`,
//           contextMenuItems: [
//             { text: 'View', anchorUrl: `/workflow-instances/${item.id}`, icon: "static/images/instances-view-icon.svg" }]
//         }
//       });
//       return {
//         items: rows,
//         totalCount: workflowInstances.totalCount,
//         page: workflowInstances.page,
//         pageSize: workflowInstances.pageSize
//       }
//     }
// }

export interface WorkflowDefinitionSummaryTableRow {}

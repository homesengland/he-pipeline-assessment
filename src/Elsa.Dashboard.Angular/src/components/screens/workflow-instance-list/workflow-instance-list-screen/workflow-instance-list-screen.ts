import { HttpClient } from '@angular/common/http';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import {ElsaClientService, ElsaClient} from "../../../../services/elsa-client";
import {EventTypes, OrderBy, PagedList,  WorkflowBlueprintSummary, WorkflowInstanceSummary, WorkflowStatus} from "../../../../models";
import { Store } from '@ngrx/store';
import { selectBasePath, selectServerUrl } from '../../../state/selectors/app.state.selectors';
import { eventBus } from '../../../../services/event-bus';
import { confirmDialogService } from '../../../../services/confirm-dialog-service';
import { DropdownButtonItem, DropdownButtonOrigin } from "../../../controls/workflow-dropdown-button/models";
import * as moment from 'moment';
import { Map, parseQuery } from '../../../../utils/utils';
import * as collection from 'lodash/collection';
import {  Router } from '@angular/router';
import { Location } from '@angular/common';
import { MenuItem } from '../../../controls/workflow-context-menu/models';

@Component({
  selector: 'workflow-instance-list-screen',
  templateUrl: './workflow-instance-list-screen.html',
  styleUrls: ['./workflow-instance-list-screen.css'],
  standalone: false
})
export class WorkflowInstanceListScreen implements OnInit {
  static readonly DEFAULT_PAGE_SIZE = 15;
  static readonly MIN_PAGE_SIZE = 5;
  static readonly MAX_PAGE_SIZE = 100;
  static readonly START_PAGE = 0;
  serverUrl: string;
  basePath: string;
  searchForm: FormGroup = this.formBuilder.group({
  searchTerm: '',
  });

  @Input() workflowId?: string;
  @Input() correlationId?: string;
  @Input() workflowStatus?: WorkflowStatus;
  @Input() orderBy?: OrderBy = OrderBy.Started;

  workflowInstancesTableValues: PagedList<WorkflowInstanceSummaryTableRow> = { items: [], page: 1, pageSize: 50, totalCount: 0 };
  workflowInstances: PagedList<WorkflowInstanceSummary> = { items: [], page: 1, pageSize: 50, totalCount: 0 };
  orderByValues: Array<OrderBy> = [OrderBy.Finished, OrderBy.LastExecuted, OrderBy.Started];
  workflowBlueprints: Array<WorkflowBlueprintSummary> = [];
  selectedWorkflowId?: string;
  selectedCorrelationId?: string;
  selectedWorkflowStatus?: WorkflowStatus;
  selectedOrderByState?: OrderBy = OrderBy.Started;
  selectedWorkflowInstanceIds: Array<string> = [];
  selectAllChecked: boolean;
  dropdownButtonOrigin = DropdownButtonOrigin;
  statuses: Array<WorkflowStatus> = [null, WorkflowStatus.Running, WorkflowStatus.Suspended, WorkflowStatus.Finished, WorkflowStatus.Faulted, WorkflowStatus.Cancelled, WorkflowStatus.Idle];
  pageSizes: Array<number> = [5, 10, 15, 20, 30, 50, 100];
  statusesButtonItems: Array<DropdownButtonItem> = new Array<DropdownButtonItem>();
  workflowButtonItems: Array<DropdownButtonItem> = new Array<DropdownButtonItem>();
  pageSizeButtonItems: Array<DropdownButtonItem> = new Array<DropdownButtonItem>();
  orderByButtonItems: Array<DropdownButtonItem> = new Array<DropdownButtonItem>();
  contextMenuItems: Array<MenuItem> = new Array<MenuItem>();
  selectedWorkflowText: string;
  currentPageSizeText: string;
  selectedOrderByText: string;
  currentSearchTerm: string;
  currentPage: number = 0;
  currentPageSize: number = WorkflowInstanceListScreen.DEFAULT_PAGE_SIZE;
  totalCount: number = 0;

  @ViewChild('selectAllCheckbox') selectAllCheckboxEl; 
  
  bulkActions: Array<DropdownButtonItem>;
  unlistenRouteChanged: any;

  constructor(private http: HttpClient, private formBuilder: FormBuilder, private elsaClientService: ElsaClientService, private store: Store, private router: Router, private location: Location) {

    if (!!this.location) {
      this.location.onUrlChange(async(url, state) => {
        
        if (url.toLowerCase().endsWith('workflow-instances'))
          return;
        let queryString = url.split('?')[1];
        this.applyQueryString(queryString);
        await this.loadWorkflowInstances();
      });
    }
  }

  async ngOnInit(): Promise<void> {
    this.store.select(selectServerUrl).subscribe(data => {
      this.serverUrl = data;
    });

    this.store.select(selectBasePath).subscribe(data => {
      this.basePath = data ? data : "";
    });


    this.selectedWorkflowId = this.workflowId;
    this.selectedCorrelationId = this.correlationId;
    this.selectedWorkflowStatus = this.workflowStatus;
    this.selectedOrderByState = this.orderBy;
    this.selectedWorkflowInstanceIds = [];

    let queryString = this.router.url.split('?')[1];
    this.applyQueryString(queryString);

    await this.loadWorkflowBlueprints();
    await this.loadWorkflowInstances();


    let bulkActions = [{
      text: "Cancel",
      name: 'Cancel',
    }, {
      text: "Delete",
      name: 'Delete',
    }, {
      text: "Retry",
      name: 'Retry',
    }];

    await eventBus.emit(EventTypes.WorkflowInstanceBulkActionsLoading, this, { sender: this, bulkActions });

    this.bulkActions = bulkActions;

    this.statusesButtonItems = this.statuses.map(x => {
      const text = x ?? 'All';
      const item: DropdownButtonItem = { text: text, isSelected: x == this.selectedWorkflowStatus, value: x };

      item.url = this.buildFilterUrl(this.selectedWorkflowId, x, this.selectedOrderByState, null, this.selectedCorrelationId);

      return item;
    });

    let latestWorkflowBlueprints = this.getLatestWorkflowBlueprintVersions();

    this.workflowButtonItems = latestWorkflowBlueprints.map(x => {
      const displayName = !!x.displayName && x.displayName.length > 0 ? x.displayName : x.name || 'Untitled';
      const item: DropdownButtonItem = { text: displayName, value: x.id, isSelected: x.id == this.selectedWorkflowId };

      item.url = this.buildFilterUrl(x.id, this.selectedWorkflowStatus, this.selectedOrderByState, null, this.selectedCorrelationId);

      return item;
    });

    let allItem: DropdownButtonItem = { text: 'All', value: null, isSelected: !this.selectedWorkflowId };
    allItem.url = this.buildFilterUrl(null, this.selectedWorkflowStatus, this.selectedOrderByState, null, this.selectedCorrelationId);

    this.workflowButtonItems = [allItem, ...this.workflowButtonItems];
    let selectedWorkflow = latestWorkflowBlueprints.find(x => x.id == this.selectedWorkflowId);
    this.selectedWorkflowText = !this.selectedWorkflowId ? 'Workflow' : !!selectedWorkflow && (selectedWorkflow.name || selectedWorkflow.displayName) ? (selectedWorkflow.displayName || selectedWorkflow.name) : 'Untitled';

    this.pageSizeButtonItems = this.pageSizes.map(x => {
      const text = "" + x;
      const item: DropdownButtonItem = { text: text, isSelected: x == this.currentPageSize, value: x };

      item.url = this.buildFilterUrl(this.selectedWorkflowId, this.selectedWorkflowStatus, this.selectedOrderByState, x, this.selectedCorrelationId);

      return item;
    });
    this.currentPageSizeText = `Page size: ${this.currentPageSize}`;

    this.selectedOrderByText = !!this.selectedOrderByState ? `Sort by: ${this.selectedOrderByState}` : "Sort";
    this.orderByButtonItems = this.orderByValues.map(x => {
      const item: DropdownButtonItem = { text: x, value: x, isSelected: x == this.selectedOrderByState };

      item.url = this.buildFilterUrl(this.selectedWorkflowId, this.selectedWorkflowStatus, x, null, this.selectedCorrelationId);

      return item;
    });

  }

  private applyQueryString(queryString?: string) {
    const query = parseQuery(queryString);

    this.selectedWorkflowId = query['workflow'];
    this.correlationId = query['correlationId'];
    this.selectedWorkflowStatus = WorkflowStatus[query['status']];
    this.selectedOrderByState = OrderBy[query['orderBy']] ?? OrderBy.Started;
    this.currentPage = !!query['page'] ? parseInt(query['page']) : 0;
    this.currentPage = isNaN(this.currentPage) ? WorkflowInstanceListScreen.START_PAGE : this.currentPage;
    this.currentPageSize = !!query['pageSize'] ? parseInt(query['pageSize']) : WorkflowInstanceListScreen.DEFAULT_PAGE_SIZE;
    this.currentPageSize = isNaN(this.currentPageSize) ? WorkflowInstanceListScreen.DEFAULT_PAGE_SIZE : this.currentPageSize;
    this.currentPageSize = Math.max(Math.min(this.currentPageSize, WorkflowInstanceListScreen.MAX_PAGE_SIZE), WorkflowInstanceListScreen.MIN_PAGE_SIZE);
  }

  getLatestWorkflowBlueprintVersions(): Array<WorkflowBlueprintSummary> {
    const groups = collection.groupBy(this.workflowBlueprints, 'id');
    return collection.map(groups, x => collection.orderBy(x, 'version', 'desc').first());
  }

  handleWorkflowIdChanged = async(e: any) => {
    this.selectedWorkflowId = e.value;
    await this.loadWorkflowInstances();
}

  buildFilterUrl(workflowId?: string, workflowStatus?: WorkflowStatus, orderBy?: OrderBy, pageSize?: number, correlationId?: string) {
    const filters: Map<string> = {};

    if (!!correlationId)
      filters['correlationId'] = correlationId;

    if (!!workflowId)
      filters['workflow'] = workflowId;

    if (!!workflowStatus)
      filters['status'] = workflowStatus;

    if (!!orderBy)
      filters['orderBy'] = orderBy;

    if (!!this.currentPage)
      filters['page'] = this.currentPage.toString();

    let newPageSize = !!pageSize ? pageSize : this.currentPageSize;
    newPageSize = Math.max(Math.min(newPageSize, 100), WorkflowInstanceListScreen.MIN_PAGE_SIZE);
    filters['pageSize'] = newPageSize.toString();

    if (newPageSize != this.currentPageSize)
      filters['page'] = Math.floor(this.currentPage * this.currentPageSize / newPageSize).toString();

    const queryString = collection.map(filters, (v, k) => `${k}=${v}`).join('&');
    return `${this.basePath}/workflow-instances?${queryString}`
  }

  async onSearchSubmit(): Promise<void> {
    this.currentSearchTerm = this.searchForm.value.searchTerm;
    await this.loadWorkflowInstances();
  }

  private async loadWorkflowInstances() {
    this.currentPage = isNaN(this.currentPage) ? WorkflowInstanceListScreen.START_PAGE : this.currentPage;
    this.currentPage = Math.max(this.currentPage, WorkflowInstanceListScreen.START_PAGE);
    this.currentPageSize = isNaN(this.currentPageSize) ? WorkflowInstanceListScreen.DEFAULT_PAGE_SIZE : this.currentPageSize;
    const elsaClient = await this.createClient();
    this.workflowInstances = await elsaClient.workflowInstancesApi.list(this.currentPage, this.currentPageSize, this.selectedWorkflowId, this.selectedWorkflowStatus, this.selectedOrderByState, this.currentSearchTerm, this.correlationId);
    this.totalCount = this.workflowInstances.totalCount;
    this.workflowInstancesTableValues = this.setTableValues(this.workflowInstances);
    const maxPage = Math.floor(this.workflowInstances.totalCount / this.currentPageSize);

    if (this.currentPage > maxPage) {
      this.currentPage = maxPage;
      this.workflowInstances = await elsaClient.workflowInstancesApi.list(this.currentPage, this.currentPageSize, this.selectedWorkflowId, this.selectedWorkflowStatus, this.selectedOrderByState, this.currentSearchTerm, this.correlationId);
      this.workflowInstancesTableValues = this.setTableValues(this.workflowInstances);
      this.totalCount = this.workflowInstances.totalCount;
    }

    this.setSelectAllIndeterminateState();
    
  }

  private async loadWorkflowBlueprints() {
    const elsaClient = await this.createClient();
    this.workflowBlueprints = await elsaClient.workflowRegistryApi.listAll({ allVersions: true });
  }

  setSelectAllIndeterminateState = () => {
    if (this.selectAllCheckboxEl) {
      const selectedItems = this.workflowInstances.items.filter(item => this.selectedWorkflowInstanceIds.includes(item.id));
      this.selectAllCheckboxEl.indeterminate = selectedItems.length != 0 && selectedItems.length != this.workflowInstances.items.length;
    }
  }

  createClient() : Promise<ElsaClient> {
    return this.elsaClientService.createElsaClient(this.serverUrl);
  }

  getStatusColor(status: WorkflowStatus) {
    switch (status) {
      default:
      case WorkflowStatus.Idle:
        return "gray";
      case WorkflowStatus.Running:
        return "rose";
      case WorkflowStatus.Suspended:
        return "blue";
      case WorkflowStatus.Finished:
        return "green";
      case WorkflowStatus.Faulted:
        return "red";
      case WorkflowStatus.Cancelled:
        return "yellow";
    }
  }

  onWorkflowInstanceCheckChange(e:Event, workflowInstance: WorkflowInstanceSummary) {
    const checkBox = e.target as HTMLInputElement;
    const isChecked = checkBox.checked;

    if (isChecked)
      this.selectedWorkflowInstanceIds = [...this.selectedWorkflowInstanceIds, workflowInstance.id];
    else
      this.selectedWorkflowInstanceIds = this.selectedWorkflowInstanceIds.filter(x => x != workflowInstance.id);

    this.setSelectAllIndeterminateState();
  }

  getSelectAllState = () => {
    const { items } = this.workflowInstances;

    for (let i = 0; i < items.length; i++) {
      if (!this.selectedWorkflowInstanceIds.includes(items[i].id)) {
        return false;
      }
    }

    return true;
  }

  onSelectAllCheckChange(e: Event) {
    const checkBox = e.target as HTMLInputElement;
    const isChecked = checkBox.checked;

    this.selectAllChecked = isChecked;

    if (isChecked) {
      let itemsToAdd = [];
      this.workflowInstances.items.forEach(item => {
        if (!this.selectedWorkflowInstanceIds.includes(item.id)) {
          itemsToAdd.push(item.id);
        }
      });

      if (itemsToAdd.length > 0) {
        this.selectedWorkflowInstanceIds = this.selectedWorkflowInstanceIds.concat(itemsToAdd);
      }
    } else {
      const currentItems = this.workflowInstances.items.map(x => x.id);
      this.selectedWorkflowInstanceIds = this.selectedWorkflowInstanceIds.filter(item => {
        return !currentItems.includes(item);
      });
    }
    this.workflowInstancesTableValues = this.setTableValues(this.workflowInstances); 
  }

  setTableValues(workflowInstances: PagedList<WorkflowInstanceSummary>): PagedList<WorkflowInstanceSummaryTableRow> {

    let rows = workflowInstances.items.map(item => {
      const isSelected = this.selectedWorkflowInstanceIds.findIndex(x => x === item.id) >= 0;
      const workflowBlueprint = this.workflowBlueprints.find(x => x.versionId == item.definitionVersionId) ?? {
        name: 'Not Found',
        displayName: '(Workflow definition not found)'
      };
      return {
        workflowInstanceSummary: item,
        isSelected: isSelected,
        correlationId: !!item.correlationId ? item.correlationId : '',
        blueprintViewUrl: `${this.basePath}/workflow-registry/${item.definitionId}`,
        displayName: workflowBlueprint.displayName || workflowBlueprint.name || '(Untitled)',
        instanceName: !item.name ? '' : item.name,
        createdAt: moment(item.createdAt).format('DD-MM-YYYY HH:mm:ss'),
        finishedAt: !!moment(item.finishedAt) ? moment(item.finishedAt).format('DD-MM-YYYY HH:mm:ss') : '-',
        lastExecutedAt: !!moment(item.lastExecutedAt) ? moment(item.lastExecutedAt).format('DD-MM-YYYY HH:mm:ss') : '-',
        faultedAt: !!moment(item.faultedAt) ? moment(item.faultedAt).format('DD-MM-YYYY HH:mm:ss') : '-',
        instanceViewUrl: `/workflow-instances/${item.id}`,
        correlationListViewUrl: `/workflow-registry/${item.definitionId}`
      }
    });
    return {
      items: rows,
      totalCount: workflowInstances.totalCount,
      page: workflowInstances.page,
      pageSize: workflowInstances.pageSize
    }
  }

  onPaged = async (e: PagerData) => {
    this.currentPage = e.page;
    await this.loadWorkflowInstances();
  };

  handleWorkflowStatusChanged = async (e: any) => {
    this.selectedWorkflowStatus = e.value;
    //if (this.location) {
    //  this.location.replaceState(e.Url);
    //}
    await this.loadWorkflowInstances();
  }

  handlePageSizeChanged = async (e: any) => {
    this.currentPageSize = e.value;
    this.currentPageSize = isNaN(this.currentPageSize) ? WorkflowInstanceListScreen.DEFAULT_PAGE_SIZE : this.currentPageSize;
    this.currentPageSize = Math.max(Math.min(this.currentPageSize, WorkflowInstanceListScreen.MAX_PAGE_SIZE), WorkflowInstanceListScreen.MIN_PAGE_SIZE);
    this.currentPageSizeText = `Page size: ${this.currentPageSize}`;
    await this.loadWorkflowInstances();
  }

  handleOrderByChanged = async (e: any) => {
    this.selectedOrderByState = e.value;
    await this.loadWorkflowInstances();
    this.selectedOrderByText = !!this.selectedOrderByState ? `Sort by: ${this.selectedOrderByState}` : "Sort";
  }

  handleBulkActionsChanged = async (e: any) => {
    const action = e;

    switch (action.name) {
      case 'Cancel':
        await this.onBulkCancel();
        break;
      case 'Delete':
        await this.onBulkDelete();
        break;
      case 'Retry':
        await this.onBulkRetry();
        break;
      default:
        action.handler();
    }

    this.updateSelectAllChecked();
  }

  async onBulkCancel() {
    const result = await confirmDialogService.show('Cancel Selected Workflow Instances', 'Are you sure you wish to cancel all selected workflow instances?');

    if (!result)
      return;

    const elsaClient = await this.createClient();
    await elsaClient.workflowInstancesApi.bulkCancel({ workflowInstanceIds: this.selectedWorkflowInstanceIds });
    this.selectedWorkflowInstanceIds = [];
    await this.loadWorkflowInstances();
    this.currentPage = 0;
  }

  async onBulkDelete() {
    const result = await confirmDialogService.show('Delete Selected Workflow Instances', 'Are you sure you wish to permanently delete all selected workflow instances?');

    if (!result)
      return;

    const elsaClient = await this.createClient();
    await elsaClient.workflowInstancesApi.bulkDelete({ workflowInstanceIds: this.selectedWorkflowInstanceIds });
    this.selectedWorkflowInstanceIds = [];
    await this.loadWorkflowInstances();
    this.currentPage = 0;
  }

  async onBulkRetry() {
    const result = await confirmDialogService.show('Retry Selected Workflow Instances', 'Are you sure you wish to retry all selected workflow instances?');

    if (!result)
      return;

    const elsaClient = await this.createClient();
    await elsaClient.workflowInstancesApi.bulkRetry({ workflowInstanceIds: this.selectedWorkflowInstanceIds });
    this.selectedWorkflowInstanceIds = [];
    await this.loadWorkflowInstances();
    this.currentPage = 0;
  }

  updateSelectAllChecked() {
    if (this.workflowInstances.items.length == 0) {
      this.selectAllChecked = false;
      return;
    }

    this.selectAllChecked = this.workflowInstances.items.findIndex(x => this.selectedWorkflowInstanceIds.findIndex(id => id == x.id) < 0) < 0;
  }
}

export interface WorkflowInstanceSummaryTableRow {
  workflowInstanceSummary: WorkflowInstanceSummary;
  isSelected: boolean;
  correlationId: string;
  blueprintViewUrl: string;
  displayName: string;
  instanceName: string;
  createdAt: string;
  finishedAt: string;
  lastExecutedAt: string;
  faultedAt: string;
  instanceViewUrl: string;
  correlationListViewUrl: string;
}

export interface PagerData {
  page: number;
  pageSize: number;
  totalCount: number;
}




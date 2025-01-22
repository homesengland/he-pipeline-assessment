import { HttpClient } from '@angular/common/http';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import {ElsaClientService, ElsaClient} from "../../../../services/elsa-client";
import {EventTypes, OrderBy, PagedList, VersionOptions, WorkflowBlueprintSummary, WorkflowDefinitionSummary, WorkflowInstanceSummary, WorkflowStatus} from "../../../../models";
import { Store } from '@ngrx/store';
import { selectBasePath, selectServerUrl } from '../../../state/selectors/app.state.selectors';
import { NgFor } from '@angular/common';
import { eventBus } from '../../../../services/event-bus';
import { DropdownButtonItem, DropdownButtonOrigin } from "../../../controls/workflow-dropdown-button/models";
import * as moment from 'moment';
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
  workflowBlueprints: Array<WorkflowBlueprintSummary> = [];
  selectedWorkflowId?: string;
  selectedCorrelationId?: string;
  selectedWorkflowStatus?: WorkflowStatus;
  selectedOrderByState?: OrderBy = OrderBy.Started;
  selectedWorkflowInstanceIds: Array<string> = [];
  selectAllChecked: boolean;

  currentSearchTerm: string;
  currentPage: number = 0;
  currentPageSize: number = WorkflowInstanceListScreen.DEFAULT_PAGE_SIZE;
  totalCount: number = 0;

  @ViewChild('selectAllCheckbox') selectAllCheckboxEl; 
  
  bulkActions: Array<DropdownButtonItem>;

  constructor(private http: HttpClient, private formBuilder: FormBuilder, private elsaClientService: ElsaClientService, private store: Store) {
  }

  async ngOnInit(): Promise<void> {
      this.store.select(selectServerUrl).subscribe(data => {
      this.serverUrl = data;
      });

    this.store.select(selectBasePath).subscribe(data => {
      this.basePath = data;
    });


    this.selectedWorkflowId = this.workflowId;
    this.selectedCorrelationId = this.correlationId;
    this.selectedWorkflowStatus = this.workflowStatus;
    this.selectedOrderByState = this.orderBy;
    this.selectedWorkflowInstanceIds = [];
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

  onWorkflowInstanceCheckChange(e: Event, workflowInstance: WorkflowInstanceSummary) {
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

  onPaged = async (e: CustomEvent<PagerData>) => {
    this.currentPage = e.detail.page;
    await this.loadWorkflowInstances();
  };

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




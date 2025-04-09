import { Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild, ViewContainerRef } from '@angular/core';
import * as collection from 'lodash/collection';
import moment from 'moment';
import {
  ActivityBlueprint,
  ActivityDescriptor,
  PagedList,
  WorkflowBlueprint,
  WorkflowExecutionLogRecord,
  WorkflowInstance,
  WorkflowModel,
  WorkflowStatus,
} from '../../../../models';
import { activityIconProvider } from '../../../../services/activity-icon-provider';
import { ElsaClientService, ElsaClient } from '../../../../services/elsa-client';
import { clip, durationToString } from '../../../../utils/utils';
import { FlyoutPanelComponent } from '../../../shared/flyout-panel/flyout-panel.component';
import { CommonModule } from '@angular/common';
import { CopyButtonComponent } from '../../../shared/copy-button/copy-button.component';
import { WorkflowEditorNotificationsComponent } from '../../workflow-definition-editor/workflow-definition-editor-notifications/workflow-editor-notifications';
import { TabHeaderComponent } from '../../../shared/tab-header/tab-header.component';
import { TabContentComponent } from '../../../shared/tab-content/tab-content.component';

interface Tab {
  id: string;
  text: string;
  view: () => any;
}

@Component({
  selector: 'elsa-workflow-instance-journal',
  templateUrl: './workflow-instance-journal.html',
  standalone: true,
  imports: [CommonModule, CopyButtonComponent, WorkflowEditorNotificationsComponent, FlyoutPanelComponent, TabHeaderComponent, TabContentComponent],
})
export class WorkflowInstanceJournalComponent implements OnInit {
  @Input() workflowInstanceId: string;
  @Input() workflowInstance: WorkflowInstance;
  @Input() serverUrl: string;
  @Input() activityDescriptors: Array<ActivityDescriptor> = [];
  @Input() workflowBlueprint: WorkflowBlueprint;
  @Input() workflowModel: WorkflowModel;
  @Output() recordSelected = new EventEmitter<WorkflowExecutionLogRecord>();

  @ViewChild(FlyoutPanelComponent) flyoutPanel: FlyoutPanelComponent;
  @ViewChild('iconContainer', { read: ViewContainerRef }) iconContainer: ViewContainerRef;

  isVisible: boolean = true;
  records: PagedList<WorkflowExecutionLogRecord> = { items: [], totalCount: 0 };
  filteredRecords: Array<WorkflowExecutionLogRecord> = [];
  selectedRecordId?: string;
  selectedActivityId?: string;
  selectedTabId: string = 'journal';

  el: ElementRef;
  moment = moment;

  constructor(private elementRef: ElementRef, private elsaClientService: ElsaClientService) {
    this.el = elementRef;
  }

  ngOnInit() {
    this.workflowInstanceIdChangedHandler(this.workflowInstanceId);
  }

  ngOnChanges(changes) {
    if (changes.workflowInstanceId) {
      this.workflowInstanceIdChangedHandler(this.workflowInstanceId);
    }
  }

  async selectActivityRecord(activityId?: string) {
    const record = !!activityId ? this.filteredRecords.find(x => x.activityId == activityId) : null;
    this.selectActivityRecordInternal(record);
    await this.flyoutPanel.selectTab('journal', true);
  }

  async workflowInstanceIdChangedHandler(newValue: string) {
    const workflowInstanceId = newValue;
    const client = await this.createClient();

    if (workflowInstanceId && workflowInstanceId.length > 0) {
      try {
        this.records = await client.workflowExecutionLogApi.get(workflowInstanceId);
        this.filteredRecords = this.records.items.filter(x => x.eventName != 'Executing' && x.eventName != 'Resuming');
      } catch {
        console.warn('The specified workflow instance does not exist.');
      }
    }
  }

  selectActivityRecordInternal(record?: WorkflowExecutionLogRecord) {
    const activity = !!record ? this.workflowBlueprint.activities.find(x => x.id === record.activityId) : null;
    this.selectedRecordId = !!record ? record.id : null;
    this.selectedActivityId = activity != null ? (!!activity.parentId && activity.parentId != this.workflowBlueprint.id ? activity.parentId : activity.id) : null;
  }

  getActivityDisplayName(activityId: string, activityType: string): string {
    const activity = this.workflowBlueprint.activities.find(x => x.id === activityId);

    if (activity) {
      return activity.displayName || activity.name;
    }

    const descriptor = this.activityDescriptors.find(x => x.type === activityType);
    return descriptor?.displayName || descriptor?.type || '(Not Found): ' + activityType;
  }

  createClient(): Promise<ElsaClient> {
    return this.elsaClientService.createElsaClient(this.serverUrl);
  }

  getEventColor(eventName: string) {
    const map = {
      Executing: 'elsa-bg-blue-500',
      Executed: 'elsa-bg-green-500',
      Faulted: 'elsa-bg-rose-500',
      Warning: 'elsa-bg-yellow-500',
      Information: 'elsa-bg-blue-500',
    };

    return map[eventName] || 'elsa-bg-gray-500';
  }

  getStatusColor(status: WorkflowStatus) {
    switch (status) {
      default:
      case WorkflowStatus.Idle:
        return 'gray';
      case WorkflowStatus.Running:
        return 'rose';
      case WorkflowStatus.Suspended:
        return 'blue';
      case WorkflowStatus.Finished:
        return 'green';
      case WorkflowStatus.Faulted:
        return 'red';
      case WorkflowStatus.Cancelled:
        return 'yellow';
    }
  }

  onRecordClick(record: WorkflowExecutionLogRecord) {
    this.selectActivityRecordInternal(record);
    this.recordSelected.emit(record);
  }

  clip(element: HTMLElement) {
    clip(element);
  }

  renderGeneralTab() {
    return;
  }

  renderJournalTab() {
    return;
  }

  renderActivityStateTab() {
    return;
  }

  renderVariablesTab() {
    return;
  }

  calculateDuration(record: WorkflowExecutionLogRecord, index: number): string {
    try {
      // Find the previous record with the same activity ID
      const recordIndex = this.records.items.indexOf(this.filteredRecords[index]);

      if (recordIndex <= 0) {
        return '';
      }

      const previousRecordsReversed = this.records.items.slice(0, recordIndex).reverse();
      const prevRecordIndex = previousRecordsReversed.findIndex(e => e.activityId == record.activityId);

      if (prevRecordIndex < 0) {
        return '';
      }

      const actualIndex = recordIndex - (prevRecordIndex + 1);
      const prevRecord = this.records.items[actualIndex];

      if (!prevRecord || !prevRecord.timestamp) {
        return '';
      }

      const duration = moment.duration(moment(record.timestamp).diff(moment(prevRecord.timestamp)));
      return durationToString(duration);
    } catch (error) {
      console.error('Error calculating duration', error);
      return '';
    }
  }

  renderIcon(activityType: string) {
    if (this.iconContainer) {
      activityIconProvider.getIcon(activityType, this.iconContainer);
    } else {
      console.warn('Icon container is not available.');
    }
  }

  // For JSON stringification in template
  JSON = JSON;

  // For accessing activity icons in template
  activityIconProvider = activityIconProvider;

  // For filtering record data in template
  getFilteredRecordDataKeys(recordData: any): string[] {
    const wellKnownDataKeys = { State: true, Input: null, Outcomes: true, Exception: true };
    const keys = [];

    for (const key in recordData) {
      if (!recordData.hasOwnProperty(key)) continue;

      if (!!wellKnownDataKeys[key]) continue;

      const value = recordData[key];

      if (!value && value != 0) continue;

      keys.push(key);
    }

    return keys;
  }

  // For getting record data values in template
  getRecordDataValue(data: any, key: string): string {
    const value = data[key];

    if (typeof value == 'string') return value;
    else if (typeof value == 'object') return JSON.stringify(value, null, 1);
    else if (typeof value == 'undefined') return null;
    else return value.toString();
  }

  // For activity state tab
  getActivityStateJSON(): string {
    const activityModel = !!this.workflowModel && this.selectedActivityId ? this.workflowModel.activities.find(x => x.activityId === this.selectedActivityId) : null;

    if (!activityModel) return '';

    // Hide expressions field from properties so that we only display the evaluated value.
    const model = {
      ...activityModel,
      properties: activityModel.properties.map(x => ({ name: x.name, value: x.value })),
    };

    return JSON.stringify(model, null, 2);
  }
}

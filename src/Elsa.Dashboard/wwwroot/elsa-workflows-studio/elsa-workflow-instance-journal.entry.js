import { r as registerInstance, e as createEvent, h, l as Host } from './index-1542df5c.js';
import { c as collection } from './collection-ba09a015.js';
import { h as hooks } from './moment-fe70f3d6.js';
import { b as WorkflowStatus } from './index-1654a48d.js';
import { a as activityIconProvider, b as createElsaClient } from './elsa-client-8304c78c.js';
import { d as durationToString, e as clip } from './utils-db96334c.js';
import './_commonjsHelpers-6cb8dacb.js';
import './events-d0aab14a.js';
import './fetch-client-f0dc2a52.js';
import './event-bus-5d6f3774.js';

const ElsaWorkflowInstanceJournal = class {
  constructor(hostRef) {
    registerInstance(this, hostRef);
    this.recordSelected = createEvent(this, "recordSelected", 7);
    this.renderJournalTab = () => {
      const items = this.filteredRecords;
      const allItems = this.records.items;
      const activityDescriptors = this.activityDescriptors;
      const workflowBlueprint = this.workflowBlueprint;
      const activityBlueprints = workflowBlueprint.activities || [];
      const selectedRecordId = this.selectedRecordId;
      const renderRecord = (record, index) => {
        var prevItemReverseIndex = allItems
          .slice(0, allItems.indexOf(items[index]))
          .reverse()
          .findIndex((e) => {
          return (e.activityId == record.activityId);
        });
        const prevItem = allItems[allItems.indexOf(items[index]) - (prevItemReverseIndex + 1)];
        const currentTimestamp = hooks(record.timestamp);
        const prevTimestamp = hooks(prevItem.timestamp);
        const deltaTime = hooks.duration(currentTimestamp.diff(prevTimestamp));
        const activityType = record.activityType;
        const activityIcon = activityIconProvider.getIcon(activityType);
        const activityDescriptor = activityDescriptors.find(x => x.type === activityType) || {
          displayName: null,
          type: null
        };
        const activityBlueprint = activityBlueprints.find(x => x.id === record.activityId) || {
          name: null,
          displayName: null
        };
        const activityName = activityBlueprint.displayName || activityBlueprint.name || activityDescriptor.displayName || activityDescriptor.type || '(Not Found): ' + activityType;
        const eventName = record.eventName;
        const eventColor = this.getEventColor(eventName);
        const recordClass = record.id === selectedRecordId ? 'elsa-border-blue-600' : 'hover:elsa-bg-gray-100 elsa-border-transparent';
        const recordData = record.data || {};
        const filteredRecordData = {};
        const wellKnownDataKeys = { State: true, Input: null, Outcomes: true, Exception: true };
        for (const key in recordData) {
          if (!recordData.hasOwnProperty(key))
            continue;
          if (!!wellKnownDataKeys[key])
            continue;
          const value = recordData[key];
          if (!value && value != 0)
            continue;
          let valueText = null;
          if (typeof value == 'string')
            valueText = value;
          else if (typeof value == 'object')
            valueText = JSON.stringify(value, null, 1);
          else if (typeof value == 'undefined')
            valueText = null;
          else
            valueText = value.toString();
          filteredRecordData[key] = valueText;
        }
        const deltaTimeText = durationToString(deltaTime);
        const outcomes = !!recordData.Outcomes ? recordData.Outcomes || [] : [];
        const exception = !!recordData.Exception ? recordData.Exception : null;
        const renderExceptionMessage = (exception) => {
          return (h("div", null, h("div", { class: "elsa-mb-4" }, h("strong", { class: "elsa-block elsa-font-bold" }, exception.Type), exception.Message), !!exception.InnerException ?
            h("div", { class: "elsa-ml-4" }, renderExceptionMessage(exception.InnerException)) : undefined));
        };
        return (h("li", null, h("div", { onClick: () => this.onRecordClick(record), class: `${recordClass} elsa-border-2 elsa-cursor-pointer elsa-p-4 elsa-rounded` }, h("div", { class: "elsa-relative elsa-pb-10" }, h("div", { class: "elsa-flex elsa-absolute top-8 elsa-left-4 -elsa-ml-px elsa-h-full elsa-w-0.5" }, h("div", { class: "elsa-flex elsa-flex-1 elsa-items-center elsa-relative elsa-right-10" }, h("span", { class: "elsa-flex-1 elsa-text-sm elsa-text-gray-500 elsa-w-max elsa-bg-white elsa-p-1 elsa-ml-1 elsa-rounded-r" }, deltaTimeText))), h("div", { class: "elsa-relative elsa-flex elsa-space-x-3" }, h("div", null, h("span", { class: `elsa-h-8 elsa-w-8 elsa-rounded-full ${eventColor} elsa-flex elsa-items-center elsa-justify-center elsa-ring-8 elsa-ring-white elsa-mr-1`, innerHTML: activityIcon })), h("div", { class: "elsa-min-w-0 elsa-flex-1 elsa-pt-1.5 elsa-flex elsa-justify-between elsa-space-x-4" }, h("div", null, h("h3", { class: "elsa-text-lg elsa-leading-6 elsa-font-medium elsa-text-gray-900" }, activityName)), h("div", null, h("span", { class: "elsa-relative elsa-inline-flex elsa-items-center elsa-rounded-full elsa-border elsa-border-gray-300 elsa-px-3 elsa-py-0.5 elsa-text-sm" }, h("span", { class: "elsa-absolute elsa-flex-shrink-0 elsa-flex elsa-items-center elsa-justify-center" }, h("span", { class: `elsa-h-1.5 elsa-w-1.5 elsa-rounded-full ${eventColor}`, "aria-hidden": "true" })), h("span", { class: "elsa-ml-3.5 elsa-font-medium elsa-text-gray-900" }, eventName))), h("div", { class: "elsa-text-right elsa-text-sm elsa-whitespace-nowrap elsa-text-gray-500" }, h("span", null, currentTimestamp.format('DD-MM-YYYY HH:mm:ss'))))), h("div", { class: "elsa-ml-12 elsa-mt-2" }, h("dl", { class: "sm:elsa-divide-y sm:elsa-divide-gray-200" }, h("div", { class: "elsa-grid elsa-grid-cols-2 elsa-gap-x-4 elsa-gap-y-8 sm:elsa-grid-cols-2" }, h("div", { class: "sm:elsa-col-span-2" }, h("dt", { class: "elsa-text-sm elsa-font-medium elsa-text-gray-500" }, h("span", null, "Activity ID"), h("elsa-copy-button", { value: record.activityId })), h("dd", { class: "elsa-mt-1 elsa-text-sm elsa-text-gray-900 elsa-mb-2" }, record.activityId)), outcomes.length > 0 ? (h("div", { class: "sm:elsa-col-span-2" }, h("dt", { class: "elsa-text-sm elsa-font-medium elsa-text-gray-500" }, h("span", null, "Outcomes"), h("elsa-copy-button", { value: outcomes.join(', ') })), h("dd", { class: "elsa-mt-1 elsa-text-sm elsa-text-gray-900 elsa-mb-2" }, h("div", { class: "elsa-flex elsa-flex-col elsa-space-y-4 sm:elsa-space-y-0 sm:elsa-flex-row sm:elsa-space-x-4" }, outcomes.map(outcome => (h("span", { class: "elsa-inline-flex elsa-items-center elsa-px-3 elsa-py-0.5 elsa-rounded-full elsa-text-sm elsa-font-medium elsa-bg-blue-100 elsa-text-blue-800" }, outcome))))))) : undefined, !!record.message && !exception ? (h("div", { class: "sm:elsa-col-span-2" }, h("dt", { class: "elsa-text-sm elsa-font-medium elsa-text-gray-500" }, h("span", null, "Message"), h("elsa-copy-button", { value: record.message })), h("dd", { class: "elsa-mt-1 elsa-text-sm elsa-text-gray-900" }, record.message))) : undefined, !!exception ? ([h("div", { class: "sm:elsa-col-span-2" }, h("dt", { class: "elsa-text-sm elsa-font-medium elsa-text-gray-500" }, h("span", null, "Exception"), h("elsa-copy-button", { value: exception.Type + '\n' + exception.Message })), h("dd", { class: "elsa-mt-1 elsa-text-sm elsa-text-gray-900" }, renderExceptionMessage(exception))), h("div", { class: "sm:elsa-col-span-2" }, h("dt", { class: "elsa-text-sm elsa-font-medium elsa-text-gray-500" }, h("span", null, "Exception Details"), h("elsa-copy-button", { value: JSON.stringify(exception, null, 1) })), h("dd", { class: "elsa-mt-1 elsa-text-sm elsa-text-gray-900 elsa-overflow-x-auto" }, h("pre", { onClick: e => clip(e.currentTarget) }, JSON.stringify(exception, null, 1))))]) : undefined, collection.map(filteredRecordData, (v, k) => (h("div", { class: "sm:elsa-col-span-2" }, h("dt", { class: "elsa-text-sm elsa-font-medium elsa-text-gray-500 elsa-capitalize" }, h("span", null, k), h("elsa-copy-button", { value: v })), h("dd", { class: "elsa-mt-1 elsa-text-sm elsa-text-gray-900 elsa-mb-2 elsa-overflow-x-auto" }, h("pre", { onClick: e => clip(e.currentTarget) }, v))))))))))));
      };
      return (h("div", { class: "flow-root elsa-mt-4" }, h("ul", { class: "-elsa-mb-8" }, items.map(renderRecord))));
    };
    this.renderActivityStateTab = () => {
      const activityModel = !!this.workflowModel && this.selectedActivityId ? this.workflowModel.activities.find(x => x.activityId === this.selectedActivityId) : null;
      if (!activityModel)
        return h("p", { class: "elsa-mt-4" }, "No activity selected");
      // Hide expressions field from properties so that we only display the evaluated value.
      const model = Object.assign(Object.assign({}, activityModel), { properties: activityModel.properties.map(x => ({ name: x.name, value: x.value })) });
      return (h("div", { class: "elsa-mt-4" }, h("pre", null, JSON.stringify(model, null, 2))));
    };
    this.renderGeneralTab = () => {
      const { workflowInstance, workflowBlueprint } = this;
      const { finishedAt, lastExecutedAt, faultedAt } = workflowInstance;
      const format = 'DD-MM-YYYY HH:mm:ss';
      const eventColor = this.getStatusColor(workflowInstance.workflowStatus);
      return (h("dl", { class: "elsa-border-b elsa-border-gray-200 elsa-divide-y elsa-divide-gray-200" }, h("div", { class: "elsa-py-3 elsa-flex elsa-justify-between elsa-text-sm elsa-font-medium" }, h("dt", { class: "elsa-text-gray-500" }, "Workflow Name"), h("dd", { class: "elsa-text-gray-900" }, workflowBlueprint.name)), h("div", { class: "elsa-py-3 elsa-flex elsa-justify-between elsa-text-sm elsa-font-medium" }, h("dt", { class: "elsa-text-gray-500" }, "Instance Name"), h("dd", { class: "elsa-text-gray-900" }, workflowInstance.name || '-')), h("div", { class: "elsa-py-3 elsa-flex elsa-justify-between elsa-text-sm elsa-font-medium" }, h("dt", { class: "elsa-text-gray-500" }, "Id"), h("dd", { class: "elsa-text-gray-900" }, workflowInstance.id)), h("div", { class: "elsa-py-3 elsa-flex elsa-justify-between elsa-text-sm elsa-font-medium" }, h("dt", { class: "elsa-text-gray-500" }, "Correlation id"), h("dd", { class: "elsa-text-gray-900" }, workflowInstance.correlationId)), h("div", { class: "elsa-py-3 elsa-flex elsa-justify-between elsa-text-sm elsa-font-medium" }, h("dt", { class: "elsa-text-gray-500" }, "Version"), h("dd", { class: "elsa-text-gray-900 elsa-break-all" }, workflowInstance.version || '-')), h("div", { class: "elsa-py-3 elsa-flex elsa-justify-between elsa-text-sm elsa-font-medium" }, h("dt", { class: "elsa-text-gray-500" }, "Workflow Status"), h("dd", { class: "elsa-text-gray-900 elsa-break-all" }, h("span", { class: "elsa-relative elsa-inline-flex elsa-items-center elsa-rounded-full" }, h("span", { class: "elsa-flex-shrink-0 elsa-flex elsa-items-center elsa-justify-center" }, h("span", { class: `elsa-w-2-5 elsa-h-2-5 elsa-rounded-full ${eventColor}`, "aria-hidden": "true" })), h("span", { class: "elsa-ml-3.5" }, workflowInstance.workflowStatus || '-')))), h("div", { class: "elsa-py-3 elsa-flex elsa-justify-between elsa-text-sm elsa-font-medium" }, h("dt", { class: "elsa-text-gray-500" }, "Created"), h("dd", { class: "elsa-text-gray-900 elsa-break-all" }, hooks(workflowInstance.createdAt).format(format) || '-')), h("div", { class: "elsa-py-3 elsa-flex elsa-justify-between elsa-text-sm elsa-font-medium" }, h("dt", { class: "elsa-text-gray-500" }, "Finished"), h("dd", { class: "elsa-text-gray-900 elsa-break-all" }, finishedAt ? hooks(finishedAt).format(format) : '-')), h("div", { class: "elsa-py-3 elsa-flex elsa-justify-between elsa-text-sm elsa-font-medium" }, h("dt", { class: "elsa-text-gray-500" }, "Last Executed"), h("dd", { class: "elsa-text-gray-900 elsa-break-all" }, lastExecutedAt ? hooks(lastExecutedAt).format(format) : '-')), h("div", { class: "elsa-py-3 elsa-flex elsa-justify-between elsa-text-sm elsa-font-medium" }, h("dt", { class: "elsa-text-gray-500" }, "Faulted"), h("dd", { class: "elsa-text-gray-900 elsa-break-all" }, faultedAt ? hooks(faultedAt).format(format) : '-'))));
    };
    this.renderVariablesTab = () => {
      const { workflowInstance, workflowBlueprint } = this;
      const { variables } = workflowInstance;
      return (h("dl", { class: "elsa-border-b elsa-border-gray-200 elsa-divide-y elsa-divide-gray-200" }, h("div", { class: "elsa-py-3 elsa-text-sm elsa-font-medium" }, (variables === null || variables === void 0 ? void 0 : variables.data) ? h("pre", null, JSON.stringify(variables === null || variables === void 0 ? void 0 : variables.data, null, 2)) : '-')));
    };
    this.workflowInstanceId = undefined;
    this.workflowInstance = undefined;
    this.serverUrl = undefined;
    this.activityDescriptors = [];
    this.workflowBlueprint = undefined;
    this.workflowModel = undefined;
    this.isVisible = true;
    this.records = { items: [], totalCount: 0 };
    this.filteredRecords = [];
    this.selectedRecordId = undefined;
    this.selectedActivityId = undefined;
    this.selectedTabId = 'journal';
  }
  async selectActivityRecord(activityId) {
    const record = !!activityId ? this.filteredRecords.find(x => x.activityId == activityId) : null;
    this.selectActivityRecordInternal(record);
    await this.flyoutPanel.selectTab('journal', true);
  }
  async workflowInstanceIdChangedHandler(newValue) {
    const workflowInstanceId = newValue;
    const client = await createElsaClient(this.serverUrl);
    if (workflowInstanceId && workflowInstanceId.length > 0) {
      try {
        this.records = await client.workflowExecutionLogApi.get(workflowInstanceId);
        this.filteredRecords = this.records.items.filter(x => x.eventName != 'Executing' && x.eventName != 'Resuming');
      }
      catch (_a) {
        console.warn('The specified workflow instance does not exist.');
      }
    }
  }
  async componentWillLoad() {
    await this.workflowInstanceIdChangedHandler(this.workflowInstanceId);
  }
  selectActivityRecordInternal(record) {
    const activity = !!record ? this.workflowBlueprint.activities.find(x => x.id === record.activityId) : null;
    this.selectedRecordId = !!record ? record.id : null;
    this.selectedActivityId = activity != null ? !!activity.parentId && activity.parentId != this.workflowBlueprint.id ? activity.parentId : activity.id : null;
  }
  getEventColor(eventName) {
    const map = {
      'Executing': 'elsa-bg-blue-500',
      'Executed': 'elsa-bg-green-500',
      'Faulted': 'elsa-bg-rose-500',
      'Warning': 'elsa-bg-yellow-500',
      'Information': 'elsa-bg-blue-500',
    };
    return map[eventName] || 'elsa-bg-gray-500';
  }
  getStatusColor(status) {
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
  onRecordClick(record) {
    this.selectActivityRecordInternal(record);
    this.recordSelected.emit(record);
  }
  render() {
    return (h(Host, null, this.renderPanel(), h("elsa-workflow-definition-editor-notifications", null)));
  }
  renderPanel() {
    return (h("elsa-flyout-panel", { ref: el => this.flyoutPanel = el }, h("elsa-tab-header", { tab: "general", slot: "header" }, "General"), h("elsa-tab-content", { tab: "general", slot: "content" }, this.renderGeneralTab()), h("elsa-tab-header", { tab: "journal", slot: "header" }, "Journal"), h("elsa-tab-content", { tab: "journal", slot: "content" }, this.renderJournalTab()), h("elsa-tab-header", { tab: "activityState", slot: "header" }, "Activity State"), h("elsa-tab-content", { tab: "activityState", slot: "content" }, this.renderActivityStateTab()), h("elsa-tab-header", { tab: "variables", slot: "header" }, "Variables"), h("elsa-tab-content", { tab: "variables", slot: "content" }, this.renderVariablesTab())));
  }
  static get watchers() { return {
    "workflowInstanceId": ["workflowInstanceIdChangedHandler"]
  }; }
};

export { ElsaWorkflowInstanceJournal as elsa_workflow_instance_journal };

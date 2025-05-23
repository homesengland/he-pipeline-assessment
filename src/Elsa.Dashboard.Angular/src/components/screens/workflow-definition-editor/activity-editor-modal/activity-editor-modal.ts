import { Component, ElementRef, EventEmitter, HostListener, Input, OnInit, Output, ViewChild, viewChild } from '@angular/core';
import { eventBus } from '../../../../services/event-bus';
import {
  ActivityDescriptor,
  ActivityPropertyDescriptor,
  ActivityDesignDisplayContext,
  ActivityModel,
  ActivityTraits,
  ConnectionModel,
  EventTypes,
  WorkflowModel,
  WorkflowPersistenceBehavior,
} from '../../../../models';
import { ModalDialog } from 'src/components/shared/modal-dialog/modal-dialog';

export interface TabModel {
  tabName: string;
  renderContent: () => any;
}

export interface ActivityEditorRenderProps {
  activityDescriptor?: ActivityDescriptor;
  activityModel?: ActivityModel;
  propertyCategories?: Array<string>;
  defaultProperties?: Array<ActivityPropertyDescriptor>;
  tabs?: Array<TabModel>;
  selectedTabName?: string;
}

export interface ActivityEditorEventArgs {
  activityDescriptor: ActivityDescriptor;
  activityModel: ActivityModel;
}

export interface ActivityEditorAppearingEventArgs extends ActivityEditorEventArgs {
}

export interface ActivityEditorDisappearingEventArgs extends ActivityEditorEventArgs {
}

@Component({
  selector: 'activity-editor-modal',
  templateUrl: './activity-editor-modal.html',
  standalone: false,
})

export class ActivityEditorModal implements OnInit {
  @Output() close = new EventEmitter<void>();
  @ViewChild(ModalDialog) dialog;
  activityModel: ActivityModel;
  activityDescriptor: ActivityDescriptor;
  isModalVisible: boolean;


  ngOnInit(): void {
    this.isModalVisible = false;
    eventBus.on(EventTypes.ActivityEditor.Show, this.onShowActivityEditor);
  }

  connectedCallback() {
    eventBus.on(EventTypes.ActivityEditor.Show, this.onShowActivityEditor);
  }

  disconnectedCallback() {
    eventBus.detach(EventTypes.ActivityEditor.Show, this.onShowActivityEditor);
  }

  onShowActivityEditor = async (activity: ActivityModel) => {
    this.activityModel = JSON.parse(JSON.stringify(activity));
    //this.activityDescriptor = state.activityDescriptors.find(x => x.type == activity.type);
    //this.workflowStorageDescriptors = state.workflowStorageDescriptors;
    //this.formContext = new FormContext(this.activityModel, newValue => this.activityModel = newValue);
    //this.timestamp = new Date();
    //this.renderProps = {};

    await this.showModal();
  };


  closeModal() {
    this.dialog.hide();
  }

  showModal() {
    this.dialog.show();
  }

  onDialogShown = async () => {
    const args: ActivityEditorAppearingEventArgs = {
      activityModel: this.activityModel,
      activityDescriptor: this.activityDescriptor
    };

    await eventBus.emit(EventTypes.ActivityEditor.Appearing, this, args);
  };

  onDialogHidden = async () => {
    const args: ActivityEditorDisappearingEventArgs = {
      activityModel: this.activityModel,
      activityDescriptor: this.activityDescriptor,
    };

    await eventBus.emit(EventTypes.ActivityEditor.Disappearing, this, args);
  };

}
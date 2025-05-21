import { Component, ElementRef, EventEmitter, HostListener, Input, OnInit, Output, viewChild } from '@angular/core';
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

  export class ActivityEditorModal {
        activityModel: ActivityModel;
        activityDescriptor: ActivityDescriptor;
        dialog: HTMLDialogElement;


    connectedCallback() {
      console.log("Event Bus Modal Event Caught");
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
        await this.show();
      };

      show = async () => await this.dialog.showModal();

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
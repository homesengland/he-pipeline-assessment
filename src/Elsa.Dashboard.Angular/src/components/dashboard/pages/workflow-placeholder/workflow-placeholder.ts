import { Component, OnInit, signal, input } from '@angular/core';
import { ActivityModel, ActivityPropertyDescriptor, ActivityDefinitionProperty } from '../../../../models';
import { Store } from '@ngrx/store';
import { AppStateActionGroup } from '../../../../components/state/actions/app.state.actions';

@Component({
  selector: 'workflow-placeholder',
  templateUrl: './workflow-placeholder.html',
    styleUrls: ['./workflow-placeholder.css'],
    standalone: false
})

export class WorkflowPlaceholder implements OnInit {

    private store : Store;
    id = input<string>('5e4506339c934e199a17ca7a2e44f874');
    activityModel = signal<ActivityModel|null>(null);
    propertyDescriptor = signal<ActivityPropertyDescriptor|null>(null);
    propertyModel = signal<ActivityDefinitionProperty|null>(null);

    constructor(store: Store) {
      this.store = store;
    }

    async ngOnInit() {

      await this.store.dispatch(AppStateActionGroup.setWorkflowDefinitionId({
        //must pick a valid workflow definition id from the environment database.
        workflowDefinitionId: this.id()
          }));
                  //event to trigger the intellisense gatherer;
        const event = new Event("shown");
        window.dispatchEvent(event);
        this.activityModel.set(this.getSingleLineModel())
        this.propertyModel.set(this.getSingleLineDefinition())
        this.propertyDescriptor.set(this.getSingleLineDescriptor())
    }

    getSingleLineModel(): ActivityModel {
        const model: ActivityModel = {
            activityId: '123',
            type: "SingleLine",
        name: 'TestSingleLine',
        displayName: 'Test Single Line',
        description: 'A Stub activity to display a single line property',
        outcomes: ["Done"],
        properties: undefined,
        persistWorkflow: true,
        loadWorkflowContext: undefined,
        saveWorkflowContext: undefined,
        propertyStorageProviders: undefined,
        }
        return model;
    }

    getSingleLineDefinition(): ActivityDefinitionProperty {
        const model: ActivityDefinitionProperty = {
            syntax: undefined,
            value: "string",
            name: "Test",
            expressions: {
                "Literal": "1",
                "Javascript": "",
            },
            type: ""
        }
        return model;
    }

    getSingleLineDescriptor(): ActivityPropertyDescriptor {
        const model: ActivityPropertyDescriptor = {
            conditionalActivityTypes:  [],
            expectedOutputType: "string",
            hasNestedProperties: false,
            hasColletedProperties: false,
            name: "Id",
            type: "System.String",
            uiHint: "single-line",
            label: "Text",
            hint: "Test Hint",
            options: null,
            order: 0,
            defaultValue: null,
            supportedSyntaxes: ["Literal", "JavaScript"],
            isReadOnly: false,
            isBrowsable: true,
            isDesignerCritical: false,
            disableWorkflowProviderSelection: false,
            considerValuesAsOutcomes: false
        }
        return model;
    }
}

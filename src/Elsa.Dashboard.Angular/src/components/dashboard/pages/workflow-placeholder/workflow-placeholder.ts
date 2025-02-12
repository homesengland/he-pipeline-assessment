import { Component, OnInit, signal } from '@angular/core';
import { ActivityModel, ActivityPropertyDescriptor, ActivityDefinitionProperty } from '../../../../models';
import { SingleLineProperty } from '../../../editors/properties/single-line-property/single-line-property';

@Component({
  selector: 'workflow-placeholder',
  templateUrl: './workflow-placeholder.html',
    styleUrls: ['./workflow-placeholder.css'],
    standalone: false
})

export class WorkflowPlaceholder implements OnInit {

    activityModel = signal<ActivityModel>(null);
    propertyDescriptor = signal<ActivityPropertyDescriptor>(null);
    propertyModel = signal<ActivityDefinitionProperty>(null);

    ngOnInit(): void {
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
        properties: null,
        persistWorkflow: true,
        loadWorkflowContext: null,
        saveWorkflowContext: null,
        propertyStorageProviders: null,
        }
        return model;
    }

    getSingleLineDefinition(): ActivityDefinitionProperty {
        const model: ActivityDefinitionProperty = {
            syntax: null,
            value: "string",
            name: "Test",
            expressions: {
                "Literal": "1",
                "Javascript": null,
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
            nestedProperties: null,
            name: "Id",
            type: "System.String",
            uiHint: "single-line",
            label: "Text",
            hint: "Test Hint",
            options: null,
            category: null,
            order: 0,
            defaultValue: null,
            defaultSyntax: null,
            supportedSyntaxes: ["Literal", "JavaScript"],
            isReadOnly: false,
            isBrowsable: true,
            isDesignerCritical: false,
            defaultWorkflowStorageProvider: null,
            disableWorkflowProviderSelection: false,
            considerValuesAsOutcomes: false
        }
        return model;
    }
}

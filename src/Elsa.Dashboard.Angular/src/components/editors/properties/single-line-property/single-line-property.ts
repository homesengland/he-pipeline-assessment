import { Component, computed, Input, model, OnChanges, OnInit, signal, SimpleChanges } from "@angular/core";
import { ActivityDefinitionProperty, ActivityModel, ActivityPropertyDescriptor, SyntaxNames } from "../../../../models";

@Component({
    selector: 'single-line-property',
    templateUrl: './single-line-property.html',
})

export class SingleLineProperty implements OnInit, OnChanges {

    activityModel = model<ActivityModel>();
    propertyDescriptor = model<ActivityPropertyDescriptor>();
    propertyModel = model<ActivityDefinitionProperty>();
    defaultSyntax = computed(() => this.propertyDescriptor().defaultSyntax || SyntaxNames.Literal;
    isEncypted = model<boolean>(false);
    propertyName: string;
    fieldId: string;
    fieldName: string;
    isReadOnly: boolean;
    currentValue = computed(() => this.propertyModel().expressions[this.defaultSyntax()] || undefined);
    ngOnChanges(changes: SimpleChanges): void {

    }

    ngOnInit(): void {

        this.propertyName = this.propertyDescriptor().name;
        this.fieldName = this.propertyName;
        this.fieldId = this.propertyName;
        this.isReadOnly = this.propertyDescriptor().isReadOnly;

    }




    onChange(e: Event) {
        const input = e.currentTarget as HTMLInputElement;
        let expressions = this.propertyModel().expressions;
        expressions[this.defaultSyntax()] = input.value;
        this.propertyModel.update(x => ({ ...x, expressions: expressions }));
    }

    onFocus(e: Event) {
        if (this.isEncypted) {
            const input = e.currentTarget as HTMLInputElement;
            let expressions = this.propertyModel().expressions;
            expressions[this.defaultSyntax()] = input.value;
            this.propertyModel.update(x => ({ ...x, expressions: expressions }));
        }
    }

    onDefaultSyntaxValueChanged(e: CustomEvent) {
        //dont think we need this...
        //this.currentValue = e.detail;
    }
}

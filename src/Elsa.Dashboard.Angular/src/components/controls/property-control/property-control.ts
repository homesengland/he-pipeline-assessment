import { Component, ComponentFactoryResolver, Input, OnInit, Signal, ViewChild, ViewContainerRef } from '@angular/core';
import { ActivityDefinition, ActivityDefinitionProperty, ActivityModel, ActivityPropertyDescriptor } from 'src/models';
import { propertyDisplayManager } from 'src/services/property-display-manager';

@Component({
  selector: 'property-control',
  templateUrl: './property-control.html',
  styleUrls: ['./property-control.css'],
  standalone: false,
})
export class PropertyControl implements OnInit {
  @Input() activity: Signal<ActivityModel>;
  @Input() property: Signal<ActivityPropertyDescriptor>;
  @Input() onUpdated: () => void;
  @Input() isEncypted: boolean;

  @ViewChild('container', { read: ViewContainerRef, static: true }) container: ViewContainerRef;

  constructor() {}

  ngOnInit() {
    this.renderComponent();
  }

  renderComponent() {
    this.container.clear();
    const componentInfo = propertyDisplayManager.display(this.activity, this.property, this.onUpdated, this.isEncypted);

    const componentRef = this.container.createComponent(componentInfo.componentType);
    console.log('Component created:', componentRef);
    Object.keys(componentInfo.inputs).forEach(key => {
      componentRef.instance[key] = componentInfo.inputs[key];
    });
  }
}

import { Component, ComponentFactoryResolver, Input, OnInit, ViewChild, ViewContainerRef } from '@angular/core';
import { ActivityModel, ActivityPropertyDescriptor } from 'src/models';
import { PropertyDisplayDriver } from 'src/services/property-display-driver';

@Component({
  selector: 'property-renderer',
  templateUrl: './property-renderer.html',
  styleUrls: ['./property-renderer.css'],
  standalone: false,
})
export class PropertyRenderer implements OnInit {
  @Input() driver: PropertyDisplayDriver;
  @Input() activity: ActivityModel;
  @Input() property: ActivityPropertyDescriptor;
  @Input() onUpdated: () => void;
  @Input() isEncypted: boolean;

  @ViewChild('container', { read: ViewContainerRef, static: true }) container: ViewContainerRef;

  constructor() {}

  ngOnInit() {
    this.renderComponent();
  }

  renderComponent() {
    this.container.clear();

    const componentInfo = this.driver.display(this.activity, this.property, this.onUpdated, this.isEncypted);

    const componentRef = this.container.createComponent(componentInfo.componentType);
    Object.keys(componentInfo.inputs).forEach(key => {
      componentRef.instance[key] = componentInfo.inputs[key];
    });
  }
}

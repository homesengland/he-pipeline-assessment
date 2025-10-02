import { AfterViewInit, ModelSignal, Component, Input } from '@angular/core';
import { ISharedComponent } from './base-component';
import { BaseComponent } from './base-component';
import Sortable from 'sortablejs';
import { NestedActivityDefinitionProperty } from 'src/models/custom-component-models';

//export interface ISortableSharedComponent extends ISharedComponent {
//  container: HTMLElement;
//}

//@Component({
//  selector: 'sortable-component',
//  templateUrl: '',
//})
//export class SortableComponent extends BaseComponent implements AfterViewInit {
  //constructor(public override component: ModelSignal<ISortableSharedComponent>) {
  //  super(component);
  // }

  //ngAfterViewInit() {
  //  // Presuming this is the correct lifecycle hook to use instead of Stencil's componentDidLoad, confirm if this is correct
  //  const dragEventHandler = this.onDragActivity.bind(this);
  //  Sortable.create(this.component().container, {
  //    animation: 150,
  //    handle: '.sortablejs-custom-handle',
  //    ghostClass: 'dragTarget',
  //    onEnd(evt) {
  //      dragEventHandler(evt.oldIndex, evt.newIndex);
  //    },
  //  });
  //}

  //onDragActivity(oldIndex: number, newIndex: number) {
  //  const propertiesJson = JSON.stringify(this.component().properties); // Get component.properties and makes a copy of this converting from JSON object to string
  //  let propertiesClone: Array<NestedActivityDefinitionProperty> = JSON.parse(propertiesJson); // Parse the string back to an array of NestedActivityDefinitionProperty
  //  const activity = propertiesClone.splice(oldIndex, 1)[0]; // Removes the activity at oldIndex and stores it in activity variable
  //  propertiesClone.splice(newIndex, 0, activity); // Inserts the activity at newIndex
  //  this.component().properties = propertiesClone; // Assigns the modified array back to component.properties
  //  this.updatePropertyModel(); // Updates the property model to reflect the changes
  //}
//}

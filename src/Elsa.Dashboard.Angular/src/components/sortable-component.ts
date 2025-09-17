import { AfterViewInit, Component, Input, input } from '@angular/core';
import { BaseComponent, ISharedComponent } from './base-component';
import Sortable from 'sortablejs';
import { NestedActivityDefinitionProperty } from 'src/models/custom-component-models';

export interface ISortableSharedComponent extends ISharedComponent {
  container: HTMLElement;
}

@Component({
  selector: 'sortable-component',
  template: '',
})
export class SortableComponent extends BaseComponent implements AfterViewInit {
  @Input() override component!: ISortableSharedComponent;

  ngAfterViewInit() {
    const dragEventHandler = this.onDragActivity.bind(this);
    Sortable.create(this.component.container, {
      animation: 150,
      handle: '.sortablejs-custom-handle',
      ghostClass: 'dragTarget',
      onEnd(evt) {
        dragEventHandler(evt.oldIndex, evt.newIndex);
      },
    });
  }

  onDragActivity(oldIndex: number, newIndex: number) {
    const propertiesJson = JSON.stringify(this.component.properties);
    let propertiesClone: Array<NestedActivityDefinitionProperty> = JSON.parse(propertiesJson);
    const activity = propertiesClone.splice(oldIndex, 1)[0];
    propertiesClone.splice(newIndex, 0, activity);
    this.component.properties = propertiesClone;
    this.updatePropertyModel();
  }
}

import { Component, computed, model, OnChanges, OnInit, signal, SimpleChanges } from '@angular/core';
import { ActivityModel, SyntaxNames } from '../../../../models';
import { ActivityDefinitionProperty, ActivityPropertyDescriptor, PropertySettings, RuntimeSelectListProviderSettings, SelectList, SelectListItem } from '../../../../models/domain';
import { awaitElement } from 'src/utils/utils';
import { ElsaClientService } from 'src/services/elsa-client';
import { selectServerUrl } from 'src/store/selectors/app.state.selectors';
import { Store } from '@ngrx/store';
import { getSelectListItems } from 'src/utils/selected-list-items';

@Component({
  selector: 'drop-down-property',
  templateUrl: './drop-down-property.html',
  standalone: false,
})
export class DropDownProperty implements OnInit {
  activityModel = model<ActivityModel>();
  propertyDescriptor = model<ActivityPropertyDescriptor>();
  propertyModel = model<ActivityDefinitionProperty>();
  currentValue: string;
  defaultSyntax = computed(() => this.propertyDescriptor()?.defaultSyntax || SyntaxNames.Literal);

  fieldName = computed(() => this.propertyDescriptor()?.name || 'default');
  fieldId = computed(() => this.propertyDescriptor()?.name || 'default');
  selectList: SelectList = {
    items: [],
    isFlagsEnum: false,
  };
  serverUrl: string;

  constructor(private elsaClientService: ElsaClientService, private store: Store) {
    console.log('Setting property model', this.propertyModel());
  }

  async ngOnInit(): Promise<void> {
    this.setVariablesFromAppState();
    const defaultSyntax = this.propertyDescriptor()?.defaultSyntax || SyntaxNames.Literal;
    this.currentValue = this.propertyModel()?.expressions[defaultSyntax] || undefined;
    const dependsOnEvent =
      this.propertyDescriptor()?.options && 'context' in this.propertyDescriptor().options ? this.propertyDescriptor().options?.context?.dependsOnEvent : undefined;

    // Does this property have a dependency on another property?
    if (!!dependsOnEvent) {
      // Collect current property values of the activity.
      const initialDepsValue = {};

      for (const event of dependsOnEvent) {
        for (const value of this.activityModel().properties) {
          initialDepsValue[value.name] = value.expressions['Literal'];
        }

        // Listen for change events on the dependency dropdown list.
        const dependentInputElement = (await awaitElement('#' + event)) as HTMLSelectElement;
        dependentInputElement.addEventListener('change', this.reloadSelectListFromDeps);

        // Get the current value of the dependency dropdown list.
        initialDepsValue[event] = dependentInputElement.value;
      }

      const existingOptions = this.propertyDescriptor().options as RuntimeSelectListProviderSettings;
      // Load the list items from the backend.
      const options: RuntimeSelectListProviderSettings = {
        context: {
          ...existingOptions.context,
          depValues: initialDepsValue,
        },
        runtimeSelectListProviderType: existingOptions.runtimeSelectListProviderType,
      };
      this.selectList = await getSelectListItems(this.elsaClientService, this.serverUrl, { options: options } as ActivityPropertyDescriptor);

      if (this.currentValue == undefined) {
        const defaultValue = this.propertyDescriptor().defaultValue;
        if (defaultValue) {
          this.currentValue = defaultValue;
        } else {
          const firstOption: any = this.selectList.items[0];

          if (firstOption) {
            const optionIsObject = typeof firstOption == 'object';
            this.currentValue = optionIsObject ? firstOption.value : firstOption.toString();
          }
        }
      }
    } else {
      this.selectList = await getSelectListItems(this.elsaClientService, this.serverUrl, this.propertyDescriptor());

      if (this.currentValue == undefined) {
        const defaultValue = this.propertyDescriptor().defaultValue;
        if (defaultValue) {
          this.currentValue = defaultValue;
        } else {
          const firstOption: any = this.selectList.items[0];

          if (firstOption) {
            const optionIsObject = typeof firstOption == 'object';
            this.currentValue = optionIsObject ? firstOption.value : firstOption.toString();
          }
        }
      }
    }

    if (this.currentValue != undefined) {
      this.propertyModel().expressions[defaultSyntax] = this.currentValue;
    }
  }
  private reloadSelectListFromDeps = async (e: InputEvent) => {
    const depValues = {};
    const options = this.propertyDescriptor().options as RuntimeSelectListProviderSettings;

    for (const dependencyPropName of options.context.dependsOnValue) {
      const value = this.activityModel().properties.find(prop => {
        return prop.name == dependencyPropName;
      });
      depValues[dependencyPropName] = value.expressions['Literal'];
    }

    // Need to get the latest value of the component that just changed.
    // For this we need to get the value from the event.
    const currentTarget = e.currentTarget as HTMLSelectElement;
    depValues[currentTarget.id] = currentTarget.value;

    let newOptions: RuntimeSelectListProviderSettings = {
      context: {
        ...options.context,
        depValues: depValues,
      },
      runtimeSelectListProviderType: options.runtimeSelectListProviderType,
    };

    this.selectList = await getSelectListItems(this.elsaClientService, this.serverUrl, { options: newOptions } as ActivityPropertyDescriptor);

    let currentSelectList = await awaitElement('#' + this.propertyDescriptor.name);

    const defaultValue = this.propertyDescriptor().defaultValue;
    if (defaultValue) {
      this.currentValue = defaultValue;
    } else {
      const firstOption: any = this.selectList.items[0];
      if (firstOption) {
        const optionIsObject = typeof firstOption == 'object';
        this.currentValue = optionIsObject ? firstOption.value : firstOption.toString();
      }
    }

    // Dispatch change event so that dependent dropdown elements refresh.
    // Do this after the current component has re-rendered, otherwise the current value will be sent to the backend, which is outdated.
    requestAnimationFrame(() => {
      currentSelectList.dispatchEvent(new Event('change'));
    });
  };

  onChange(e: Event) {
    const input = e.currentTarget as HTMLTextAreaElement;
    const defaultSyntax = this.propertyDescriptor()?.defaultSyntax || SyntaxNames.Literal;
    let expressions = this.propertyModel().expressions;
    expressions[defaultSyntax] = input.value;

    this.propertyModel.update(x => ({ ...x, expressions: expressions }));
    this.updateActivityModel('Literal', input.value);
  }

  private updateActivityModel(syntax: string, value: string) {
    const updatedProperties = this.activityModel().properties.map(property =>
      property.name === this.propertyDescriptor().name
        ? {
            ...property,
            expressions: {
              ...property.expressions,
              [syntax]: value,
            },
            syntax: this.defaultSyntax(),
          }
        : property,
    );
    this.activityModel.update(model => ({
      ...model,
      properties: updatedProperties,
    }));
  }

  isObject(item: any) {
    typeof item == 'object';
  }

  private setVariablesFromAppState(): void {
    this.store.select(selectServerUrl).subscribe(data => {
      this.serverUrl = data;
    });
  }

  getSelectItems(): any {
    return this.selectList.items.map(item => {
      return {
        text: item.label,
        value: item.value,
      } as SelectListItem;
    });
  }
}

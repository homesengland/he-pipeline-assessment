import { ActivityDescriptor, ActivityDescriptorDisplayContext } from "../../../../models";
import '../../../../utils/utils';
export declare class ElsaActivityPickerModal {
  selectedTrait: number;
  selectedCategory: string;
  searchText: string;
  dialog: HTMLElsaModalDialogElement;
  categories: Array<string>;
  filteredActivityDescriptorDisplayContexts: Array<ActivityDescriptorDisplayContext>;
  connectedCallback(): void;
  disconnectedCallback(): void;
  onShowActivityPicker: () => Promise<void>;
  componentWillRender(): void;
  selectTrait(trait: number): void;
  selectCategory(category: string): void;
  onTraitClick(e: MouseEvent, trait: number): void;
  onCategoryClick(e: MouseEvent, category: string): void;
  onSearchTextChange(e: any): void;
  onCancelClick(): Promise<void>;
  onActivityClick(e: Event, activityDescriptor: ActivityDescriptor): Promise<void>;
  render(): any;
}

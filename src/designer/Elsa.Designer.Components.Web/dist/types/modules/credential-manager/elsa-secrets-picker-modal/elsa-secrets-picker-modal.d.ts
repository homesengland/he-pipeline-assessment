import { SecretDescriptor } from "../models/secret.model";
export declare class ElsaSecretsPickerModal {
  selectedTrait: number;
  selectedCategory: string;
  searchText: string;
  dialog: HTMLElsaModalDialogElement;
  categories: Array<string>;
  filteredSecretsDescriptorDisplayContexts: Array<any>;
  connectedCallback(): void;
  disconnectedCallback(): void;
  onShowSecretsPicker: () => Promise<void>;
  componentWillRender(): void;
  selectTrait(trait: number): void;
  selectCategory(category: string): void;
  onTraitClick(e: MouseEvent, trait: number): void;
  onCategoryClick(e: MouseEvent, category: string): void;
  onSearchTextChange(e: any): void;
  onCancelClick(): Promise<void>;
  onSecretClick(e: Event, secretDescriptor: SecretDescriptor): Promise<void>;
  render(): any;
}

import 'i18next-wc';
export declare class CredentialManagerItemsList {
  monacoLibPath: string;
  culture: string;
  basePath: string;
  serverUrl: string;
  private i18next;
  componentWillLoad(): Promise<void>;
  onNewClick(): Promise<void>;
  renderListScreen(): any;
  renderSecretPickerModal(): any;
  renderSecretPickerEditor(): any;
  render(): any;
}

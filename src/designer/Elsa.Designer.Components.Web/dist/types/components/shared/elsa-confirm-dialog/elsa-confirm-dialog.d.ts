export declare class ElsaConfirmDialog {
  caption: string;
  message: string;
  culture: string;
  private i18next;
  dialog: HTMLElsaModalDialogElement;
  fulfill: (value: (PromiseLike<boolean> | boolean)) => void;
  reject: () => void;
  show(caption: string, message: string): Promise<boolean>;
  hide(): Promise<void>;
  componentWillLoad(): Promise<void>;
  onDismissClick(): Promise<void>;
  onAcceptClick(): Promise<void>;
  render(): any;
}

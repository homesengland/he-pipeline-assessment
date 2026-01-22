export interface ToastNotificationOptions {
  autoCloseIn?: number;
  title?: string;
  message: string;
}
export declare class ElsaToastNotification {
  isVisible: boolean;
  title?: string;
  message?: string;
  toast: HTMLElement;
  show(options: ToastNotificationOptions): Promise<void>;
  hide(): Promise<void>;
  render(): any;
  renderToast(): any;
  renderTitle(): any;
}

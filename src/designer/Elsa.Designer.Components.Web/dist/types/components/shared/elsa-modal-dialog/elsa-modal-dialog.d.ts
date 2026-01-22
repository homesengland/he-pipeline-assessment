import { EventEmitter } from '../../../stencil-public-runtime';
export declare class ElsaModalDialog {
  shown: EventEmitter;
  hidden: EventEmitter;
  isVisible: boolean;
  overlay: HTMLElement;
  modal: HTMLElement;
  render(): any;
  show(animate?: boolean): Promise<void>;
  hide(animate?: boolean): Promise<void>;
  handleDefaultClose: () => Promise<void>;
  showInternal(animate: boolean): void;
  hideInternal(animate: boolean): void;
  handleKeyDown(e: KeyboardEvent): Promise<void>;
  renderModal(): any;
}

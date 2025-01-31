import {eventBus} from './event-bus';
import { EventTypes } from "../models";
import { ToastNotificationOptions } from "../models/toast";

export class ToastNotificationService {
  show(title: string, message: string, autoCloseIn?: number) {
    const options: ToastNotificationOptions = {title, message, autoCloseIn};
    eventBus.emit(EventTypes.ShowToastNotification, this, options);
  }

  hide() {
    eventBus.emit(EventTypes.HideToastNotification, this);
  }
}

export const toastNotificationService = new ToastNotificationService();

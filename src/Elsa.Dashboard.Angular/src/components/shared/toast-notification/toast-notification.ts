import { Component, OnInit, signal } from '@angular/core';
import { ToastNotificationOptions } from '../../../models/toast';
import { enter, leave, toggle } from 'el-transition';

@Component({
  selector: 'toast-notification',
  templateUrl: './toast-notification.html',
  styleUrls: ['./toast-notification.css'],
  standalone: true,
})
export class ToastNotification implements OnInit {
  isVisible = signal(false);
  title? = signal(null);
  message? = signal(null);
  toast: HTMLElement;

  constructor() {}
  ngOnInit(): void {}

  async show(options: ToastNotificationOptions) {
    this.isVisible.update(value => true);
    enter(this.toast);

    if (options.autoCloseIn) {
      setTimeout(async () => await this.hide(), options.autoCloseIn);
    }

    this.title.update(value => options.title);
    this.message.update(value => options.message);
  }

  async hide() {
    leave(this.toast).then(() => this.isVisible.update(value => false));
  }
}

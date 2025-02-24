import { Component, signal, ViewChild } from '@angular/core';

@Component({
    selector: 'confirm-dialog',
    templateUrl: './confirm-dialog.html',
    styleUrls: ['./confirm-dialog.css'],
    standalone: false
})
export class ConfirmDialog {
    caption? = signal(null);
    message? = signal(null);

    @ViewChild('dialog') dialog;

    fulfill: (value: (PromiseLike<boolean> | boolean)) => void;
    reject: () => void;

    async show(caption: string, message: string): Promise<boolean> {
        this.caption.set(caption);
        this.message.set(message);

        await this.dialog.show(true);

        return new Promise<boolean>((fulfill, reject) => {
            this.fulfill = fulfill;
            this.reject = reject;
        });
    }

    async hide() {
        await this.dialog.hide(true);
    }

    async onDismissClick() {
        this.fulfill(false);
        await this.hide();
    }

    async onAcceptClick() {
        this.fulfill(true);
        this.fulfill = null;
        this.reject = null;
        await this.hide();
    }
}


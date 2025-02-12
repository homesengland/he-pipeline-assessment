import { Component, ElementRef, EventEmitter, HostListener, Input, Output, signal, viewChild } from '@angular/core';
import { enter, leave } from 'el-transition';
import { eventBus } from '../../../services/event-bus';
import { EventTypes } from '../../../models';

@Component({
    selector: 'modal-dialog',
    templateUrl: './modal-dialog.html',
    styleUrls: ['./modal-dialog.css'],
    host: {
        'class': 'elsa-block',
        '[class.hidden]': '!isVisible'
    },
    standalone: false
})
export class ModalDialog {
    @Output() shown = new EventEmitter<boolean>();
    @Output() hidden = new EventEmitter<boolean>();
    @Input() isVisible = false;

    readonly overlay = viewChild.required<ElementRef>('overlay');
    readonly modal = viewChild.required<ElementRef>('modal');

    dialogWidth = signal('56em')

    async show(animate: boolean = true) {
        this.showInternal(animate);
    }

    async hide(animate: boolean = true) {
        await eventBus.emit(EventTypes.HideModalDialog);
        this.hideInternal(animate);
    }

    handleDefaultClose = async () => {
        await this.hide();
    }

    showInternal(animate: boolean) {
        this.isVisible = true;

        if (!animate) {
            this.overlay().nativeElement.style.opacity = "1";
            this.modal().nativeElement.style.opacity = "1";
        }

        enter(this.overlay().nativeElement);
        enter(this.modal().nativeElement).then(this.shown.observed ?? this.shown.emit);
    }

    hideInternal(animate: boolean) {
        if (!animate) {
            this.isVisible = false;
        }

        leave(this.overlay().nativeElement);
        leave(this.modal().nativeElement).then(() => {
            this.isVisible = false;
            this.hidden.emit();
        });
    }

    @HostListener('document:keydown', ['$event'])
    async handleKeyDown(event: KeyboardEvent) {
        if (this.isVisible && event.key === 'Escape') {
            await this.hide(true);
        }
    }
}

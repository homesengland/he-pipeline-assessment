
export interface IConfirmData {
    title: string;
    content: string;
    confirmButtonLabel: string;
    confirmButtonClass: string;
    closeButtonLabel: string;
    dialogWidth: string;
}

export class ModalConfirmData {
    public title: string;
    public content: string;
    public confirmButtonLabel: string;
    public confirmButtonClass: string;
    public closeButtonLabel: string;
    public dialogWidth: string;

    constructor(data?: IConfirmData) {
        if (data) {
            this.title = data.title || 'Confirm';
            this.content = data.content || 'confirm message?';
            this.confirmButtonLabel = data.confirmButtonLabel || 'Confirm';
            this.closeButtonLabel = data.closeButtonLabel || 'Close';
            this.confirmButtonClass = data.confirmButtonClass || '';
            this.dialogWidth = data.dialogWidth || '60%';
        } else {
            this.title = 'Confirm';
            this.content = 'confrim message?';
            this.confirmButtonLabel = 'Confirm';
            this.closeButtonLabel = 'Close';
            this.confirmButtonClass = '';
            this.dialogWidth = '60%';
        }
    }
}
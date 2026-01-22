export declare class MonacoEditorDialogService {
  monacoEditor: HTMLElsaMonacoElement;
  monacoEditorDialog: HTMLElsaModalDialogElement;
  currentValue: string;
  valueSaved: (val: string) => void;
  show(language: string, value: string, onChanged: (val: string) => void): void;
  save(): void;
}
export declare const monacoEditorDialogService: MonacoEditorDialogService;

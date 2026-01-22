export interface Monaco {
  editor: any;
  languages: any;
  KeyCode: any;
  Uri: any;
}
export interface EditorVariable {
  variableName: string;
  type: string;
}
export declare var EditorVariables: Array<EditorVariable>;
export declare function initializeMonacoWorker(libPath?: string): Promise<Monaco>;

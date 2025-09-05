export interface DiffEditorModel {
  code: string;
  language: string;
}
export interface EditorModel {
  value: string;
  language?: string;
  uri?: any;
}

export enum MarkerSeverity {
  Error = 8,
  Warning = 4,
  Info = 2,
  Hint = 1,
}

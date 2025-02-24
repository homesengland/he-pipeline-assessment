import { Map } from '../utils/utils'

//Outcome Screens

export interface ITextProperty {
  expressions?: Map<string>;
  syntax?: string;
}

export interface IOutcomeProperty {
  text: ITextProperty;
  condition: ITextProperty;
}

export interface Dictionary<T> {
  [Key: string]: T;
}

export interface DataDictionaryGroup {
  Id: number;
  Name: string;
  DataDictionaryList: Array<DataDictionary>;
  IsArchived: boolean;
}

export interface DataDictionary{
  Id: number;
  Name: string;
  LegacyName: string;
  IsArchived: boolean;
}

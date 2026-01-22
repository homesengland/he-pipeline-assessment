import { ActivityDefinition, ActivityDefinitionProperty, ActivityModel, ConnectionModel, WorkflowModel } from "../models";
import { Duration } from "moment";
declare global {
  interface Array<T> {
    distinct(): Array<T>;
    last(): T;
  }
}
export declare type Map<T> = {
  [key: string]: T;
};
export declare function format(first: string, middle: string, last: string): string;
export interface Array<T> {
  distinct(): Array<T>;
  last(): T;
  find<S extends T>(predicate: (this: void, value: T, index: number, obj: T[]) => value is S, thisArg?: any): S | undefined;
  find(predicate: (value: T, index: number, obj: T[]) => unknown, thisArg?: any): T | undefined;
  push(...items: T[]): number;
}
export declare function isNumeric(str: string): boolean;
export declare function getChildActivities(workflowModel: WorkflowModel, parentId?: string): ActivityModel[];
export declare function getInboundConnections(workflowModel: WorkflowModel, activityId: string): ConnectionModel[];
export declare function getOutboundConnections(workflowModel: WorkflowModel, activityId: string): ConnectionModel[];
export declare function removeActivity(workflowModel: WorkflowModel, activityId: string): WorkflowModel;
export declare function removeConnection(workflowModel: WorkflowModel, sourceId: string, outcome: string): WorkflowModel;
export declare function findActivity(workflowModel: WorkflowModel, activityId: string): ActivityModel;
export declare function addConnection(workflowModel: WorkflowModel, connection: ConnectionModel): {
  connections: ConnectionModel[];
  activities: ActivityModel[];
  persistenceBehavior?: import("../models").WorkflowPersistenceBehavior;
};
export declare function setActivityDefinitionProperty(activityDefinition: ActivityDefinition, name: string, expression: string, syntax: string): void;
export declare function setActivityModelProperty(activityModel: ActivityModel, name: string, expression: string, syntax: string): void;
export declare function setProperty(properties: Array<ActivityDefinitionProperty>, name: string, expression: string, syntax?: string): void;
export declare function getOrCreateProperty(activity: ActivityModel, name: string, defaultExpression?: () => string, defaultSyntax?: () => string): ActivityDefinitionProperty;
export declare function parseJson(json: string): any;
export declare function parseQuery(queryString?: string): any;
export declare function queryToString(query: any): string;
export declare function mapSyntaxToLanguage(syntax: string): any;
export declare function htmlToElement(html: string): Element;
export declare function htmlDecode(value: any): string;
export declare function htmlEncode(value: any): string;
export declare function durationToString(duration: Duration): string;
export declare function clip(el: any): void;
export declare function awaitElement(selector: any): Promise<any>;
export interface Hash<TValue> {
  [key: string]: TValue;
}
export declare const stripActivityNameSpace: (name: string) => string;

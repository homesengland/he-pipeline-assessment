import { CellView, Graph } from '@antv/x6';
import './ports';
export declare function createGraph(container: HTMLElement, interacting: CellView.Interacting, disableEvents: () => void, enableEvents: (emitWorkflowChanged: boolean) => void, disableEdit?: boolean): Graph;
export declare function addGraphEvents(graph: any, disableEvents: () => void, enableEvents: (emitWorkflowChanged: boolean) => void, disableEdit: boolean): void;
export declare function removeGraphEvents(graph: Graph): void;

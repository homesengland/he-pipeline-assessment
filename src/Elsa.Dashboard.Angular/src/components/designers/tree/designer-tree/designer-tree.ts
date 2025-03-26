import {
  AfterViewChecked,
  Component,
  computed,
  ElementRef,
  EventEmitter,
  input,
  Input,
  OnChanges,
  OnInit,
  Output,
  Signal,
  SimpleChanges,
  viewChild,
  ViewChild,
  ViewEncapsulation,
} from '@angular/core';
import { ActivityContextMenuState, LayoutDirection, WorkflowDesignerMode } from '../models';
import * as d3 from 'd3';
import dagreD3 from 'dagre-d3';
import { ActivityDesignDisplayContext, ActivityModel, ConnectionModel, EventTypes, WorkflowModel, WorkflowPersistenceBehavior } from 'src/models';
import { getChildActivities, removeConnection } from 'src/utils/utils';
import { eventBus } from 'src/services/event-bus';

@Component({
  selector: 'designer-tree',
  templateUrl: './designer-tree.html',
  styleUrls: ['./designer-tree.css'],
  host: { class: 'workflow-canvas elsa-flex-1 elsa-flex' },
  standalone: false,
  encapsulation: ViewEncapsulation.None,
})
export class DesignerTree implements AfterViewChecked, OnChanges {
  WorkflowDesignerMode: any;
  calc(arg0: number) {
    throw new Error('Method not implemented.');
  }
  @Input() layoutDirection: LayoutDirection = LayoutDirection.TopBottom;
  @Input() mode: WorkflowDesignerMode = WorkflowDesignerMode.Edit;
  model = input<WorkflowModel>({
    activities: [],
    connections: [],
    persistenceBehavior: WorkflowPersistenceBehavior.WorkflowBurst,
  });
  @Input() selectedActivityIds: Array<string> = [];
  @Input() activityContextMenuButton?: (activity: ActivityModel) => string;
  @Input() activityBorderColor?: (activity: ActivityModel) => string;
  @Input() activityContextMenu?: ActivityContextMenuState;
  @Input() connectionContextMenu?: ActivityContextMenuState;
  @Input() activityContextTestMenu?: ActivityContextMenuState;
  @Input() enableMultipleConnectionsFromSingleSource: boolean;
  @ViewChild('svgSvgElement') svg: ElementRef;
  @ViewChild('svgGElement') inner: ElementRef;

  el: HTMLElement;
  svgD3Selected: d3.Selection<SVGSVGElement, unknown, null, undefined>;
  innerD3Selected: d3.Selection<SVGGElement, unknown, null, undefined>;
  zoom: d3.ZoomBehavior<Element, unknown>;
  zoomParams: { x: number; y: number; scale: number; initialZoom: boolean } = { x: 0, y: 0, scale: 1, initialZoom: true };
  graph: dagreD3.graphlib.Graph = new dagreD3.graphlib.Graph().setGraph({});
  parentActivityId?: string;
  parentActivityOutcome?: string;
  addingActivity: boolean = false;
  activityDisplayContexts: Map<string, ActivityDesignDisplayContext> = null;
  oldActivityDisplayContexts: Map<string, ActivityDesignDisplayContext> = null;
  selectedActivities: Map<string, ActivityModel> = new Map<string, ActivityModel>();
  ignoreCopyPasteActivities: boolean = false;
  workflowModel: WorkflowModel;
  dagreD3Renderer: dagreD3.Render = new dagreD3.render();
  testWorkflowDesignerMode: WorkflowDesignerMode = WorkflowDesignerMode.Test;

  @Output() workflowChanged: EventEmitter<WorkflowModel> = new EventEmitter<WorkflowModel>();
  @Output() activitySelected: EventEmitter<ActivityModel> = new EventEmitter<ActivityModel>();
  @Output() activityDeselected: EventEmitter<ActivityModel> = new EventEmitter<ActivityModel>();
  @Output() activityContextMenuButtonClicked: EventEmitter<ActivityContextMenuState> = new EventEmitter<ActivityContextMenuState>();
  @Output() connectionContextMenuButtonClicked: EventEmitter<ActivityContextMenuState> = new EventEmitter<ActivityContextMenuState>();
  @Output() activityContextMenuButtonTestClicked: EventEmitter<ActivityContextMenuState> = new EventEmitter<ActivityContextMenuState>();

  activityContextMenuState: ActivityContextMenuState = {
    shown: false,
    x: 0,
    y: 0,
    activity: null,
    selectedActivities: new Map<string, ActivityModel>(),
  };

  connectionContextMenuState: ActivityContextMenuState = {
    shown: false,
    x: 0,
    y: 0,
    activity: null,
  };

  activityContextMenuTestState: ActivityContextMenuState = {
    shown: false,
    x: 0,
    y: 0,
    activity: null,
  };

  handleContextMenuChange(state: ActivityContextMenuState) {
    this.ignoreCopyPasteActivities = true;
    this.activityContextMenuState = state;
    this.activityContextMenuButtonClicked.emit(state);
  }

  handleConnectionContextMenuChange(state: ActivityContextMenuState) {
    this.connectionContextMenuState = state;
    this.connectionContextMenuButtonClicked.emit(state);
  }

  handleContextMenuTestChange(state: ActivityContextMenuState) {
    this.ignoreCopyPasteActivities = true;
    this.activityContextMenuTestState = state;
    this.activityContextMenuButtonTestClicked.emit(state);
  }

  constructor(private elRef: ElementRef) {
    this.el = elRef.nativeElement;
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['model'] && this.model()) {
      this.workflowModel = this.model();
      this.updateWorkflowModel(this.workflowModel, false);
      if (this.workflowModel.activities.length > 0) {
        this.svgD3Selected = d3.select(this.svg.nativeElement);
        this.innerD3Selected = d3.select(this.inner.nativeElement);
        this.safeRender();
      }
    }
  }

  ngAfterViewChecked(): void {
    //this.svgD3Selected = d3.select(this.svg);
    //this.innerD3Selected = d3.select(this.inner);
    //this.safeRender();
  }

  safeRender() {
    // Rebuild D3 model if component completed its initial load.
    if (!this.svgD3Selected) return;

    const rect = this.el.getBoundingClientRect();
    if (rect.height === 0 || rect.width === 0) {
      const observer = new ResizeObserver(entries => {
        for (let entry of entries) {
          const rect = entry.contentRect;
          if (rect.height > 0 && rect.width > 0) {
            this.renderTree();
            observer.unobserve(entry.target);
          }
        }
      });

      observer.observe(this.el);
    } else {
      this.renderTree();
    }
  }

  renderTree() {
    this.applyZoom();
    this.setEntities();
    this.renderNodes();
  }

  applyZoom() {
    this.zoom = d3.zoom().on('zoom', event => {
      const { transform } = event;
      this.innerD3Selected.attr('transform', transform);
      this.zoomParams = {
        x: transform.x,
        y: transform.y,
        scale: transform.k,
        initialZoom: this.zoomParams.initialZoom,
      };

      //fix for safari
      const isSafari = /^((?!chrome|android).)*safari/i.test(navigator.userAgent);
      if (isSafari) {
        d3.selectAll('g.label > * > * > div, g.label > * > div').style('transform', `scale(${transform.k})`);
      }
    });

    this.svgD3Selected.call(this.zoom);
  }

  setEntities() {
    this.graph = new dagreD3.graphlib.Graph().setGraph({ ranksep: 30 });
    (this.graph.graph() as any).rankdir = this.getLayoutDirection();

    const rootActivities = this.getRootActivities();

    // Start node.
    this.graph.setNode('start', {
      shape: 'rect',
      label:
        this.mode == WorkflowDesignerMode.Edit
          ? `<button class="elsa-px-6 elsa-py-3 elsa-border elsa-border-transparent elsa-text-base elsa-leading-6 elsa-font-medium elsa-rounded-md elsa-text-white elsa-bg-green-600 hover:elsa-bg-green-500 focus:elsa-outline-none focus:elsa-border-green-700 focus:elsa-shadow-outline-green active:elsa-bg-green-700 elsa-transition elsa-ease-in-out elsa-duration-150">Start</button>`
          : `<button class="elsa-px-6 elsa-py-3 elsa-border elsa-border-transparent elsa-text-base elsa-leading-6 elsa-font-medium elsa-rounded-md elsa-text-white elsa-bg-green-600 focus:elsa-outline-none elsa-cursor-default">Start</button>`,
      rx: 5,
      ry: 5,
      labelType: 'html',
      class: 'start',
    });

    // Connections between Start and root activities.
    rootActivities.forEach(activity => {
      this.graph.setEdge('start', `${activity.activityId}/start`, {
        arrowhead: 'undirected',
      });
      this.graph.setNode(`${activity.activityId}/start`, {
        shape: 'rect',
        activity,
        label: this.renderOutcomeButton(),
        labelType: 'html',
        class: 'add',
      });
      this.graph.setEdge(`${activity.activityId}/start`, activity.activityId, { arrowhead: 'undirected' });
    });

    // Connections between activities and their outcomes.
    const activityDisplayContexts = this.activityDisplayContexts || {};

    this.workflowModel.activities.forEach(activity => {
      this.graph.setNode(activity.activityId, this.createActivityOptions(activity));
      const displayContext = activityDisplayContexts[activity.activityId] || undefined;
      const outcomes = !!displayContext ? displayContext.outcomes : activity.outcomes || [];

      outcomes.forEach(outcome => {
        this.graph.setNode(`${activity.activityId}/${outcome}`, {
          shape: 'rect',
          outcome,
          activity,
          label: this.renderOutcomeButton(),
          labelType: 'html',
          class: 'add',
        });
        this.graph.setEdge(activity.activityId, `${activity.activityId}/${outcome}`, {
          label: `<p class="elsa-outcome elsa-mb-4 elsa-relative elsa-z-10 elsa-px-2.5 elsa-py-0.5 elsa-rounded-full elsa-text-xs elsa-font-medium elsa-leading-4 elsa-bg-gray-100 elsa-text-gray-800 elsa-capitalize elsa-cursor-default">${outcome}</p>`,
          labelpos: 'c',
          labelType: 'html',
          arrowhead: 'undirected',
        });
      });
    });

    this.workflowModel.connections.forEach(({ sourceId, targetId, outcome }) => {
      const sourceName = `${sourceId}/${outcome}`;

      if (!this.graph.hasNode(sourceName)) {
        console.warn(`No source node with ID '${sourceName}' exists.`);
        return;
      }

      this.graph.setEdge(sourceName, targetId, { arrowhead: 'undirected' });
    });
  }

  getRootActivities(): Array<ActivityModel> {
    return getChildActivities(this.workflowModel, null);
  }

  renderOutcomeButton() {
    const cssClass = this.mode == WorkflowDesignerMode.Edit ? 'hover:elsa-text-blue-500 elsa-cursor-pointer' : 'elsa-cursor-default';
    return `<svg class="elsa-h-8 elsa-w-8 elsa-text-gray-400 ${cssClass}" fill="none" viewBox="0 0 24 24" stroke="currentColor">
        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 9v3m0 0v3m0-3h3m-3 0H9m12 0a9 9 0 11-18 0 9 9 0 0118 0z" />
      </svg>`;
  }

  createActivityOptions(activity: ActivityModel) {
    return {
      shape: 'rect',
      label: this.renderActivity(activity),
      rx: 5,
      ry: 5,
      labelType: 'html',
      class: 'activity',
      activity,
    };
  }

  renderActivity(activity: ActivityModel) {
    const activityDisplayContexts = this.activityDisplayContexts || {};
    const displayContext = activityDisplayContexts[activity.activityId] || undefined;
    const activityContextMenuButton = !!this.activityContextMenuButton ? this.activityContextMenuButton(activity) : '';
    const activityBorderColor = !!this.activityBorderColor ? this.activityBorderColor(activity) : 'gray';
    const selectedColor = !!this.activityBorderColor ? activityBorderColor : 'blue';
    const cssClass = !!this.selectedActivities[activity.activityId]
      ? `elsa-border-${selectedColor}-600`
      : `elsa-border-${activityBorderColor}-200 hover:elsa-border-${selectedColor}-600`;
    const displayName = displayContext != undefined ? displayContext.displayName : activity.displayName;
    const typeName = activity.type;
    const expanded = displayContext && displayContext.expanded;

    return `<div id=${`activity-${activity.activityId}`}
  class="activity elsa-border-2 elsa-border-solid elsa-rounded elsa-bg-white elsa-text-left elsa-text-black elsa-text-lg elsa-select-none elsa-max-w-md elsa-shadow-sm elsa-relative ${cssClass}">
    <div class="elsa-p-3">
      <div class="elsa-flex elsa-justify-between elsa-space-x-4">
        <div class="elsa-flex-shrink-0">
        ${displayContext?.activityIcon || ''}
        </div>
        <div class="elsa-flex-1 elsa-font-medium elsa-leading-8 elsa-overflow-hidden">
          <p class="elsa-overflow-ellipsis elsa-text-base">${displayName}</p>
          ${typeName !== displayName ? `<p class="elsa-text-gray-400 elsa-text-sm">${typeName}</p>` : ''}
        </div>
        <div class="elsa--mt-2">
          <div class="context-menu-button-container">
            ${activityContextMenuButton}
          </div>
          <button type="button" class="expand elsa-ml-1 elsa-text-gray-400 elsa-rounded-full elsa-bg-transparent hover:elsa-text-gray-500 focus:elsa-outline-none focus:elsa-text-gray-500 focus:elsa-bg-gray-100 elsa-transition elsa-ease-in-out elsa-duration-150">
            ${
              !expanded
                ? `<svg xmlns="http://www.w3.org/2000/svg" class="elsa-w-6 elsa-h-6 elsa-text-gray-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 9l-7 7-7-7" />
            </svg>`
                : ''
            }
            ${
              expanded
                ? `<svg xmlns="http://www.w3.org/2000/svg" class="elsa-h-6 elsa-w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 15l7-7 7 7" />
            </svg>`
                : ''
            }
          </button>
        </div>
      </div>
      ${this.renderActivityBody(displayContext)}
      </div>
    </div>
    </div>`;
  }

  renderActivityBody(displayContext: ActivityDesignDisplayContext) {
    if (displayContext && displayContext.expanded) {
      return `<div class="elsa-border-t elsa-border-t-solid">
          <div class="elsa-p-4 elsa-text-gray-400 elsa-text-sm">
            <div class="elsa-mb-2">${!!displayContext?.bodyDisplay ? displayContext.bodyDisplay : ''}</div>
            <div>
              <span class="elsa-inline-flex elsa-items-center elsa-px-2.5 elsa-py-0.5 elsa-rounded-full elsa-text-xs elsa-font-medium elsa-bg-gray-100 elsa-text-gray-500">
                <svg class="-elsa-ml-0.5 elsa-mr-1.5 elsa-h-2 elsa-w-2 elsa-text-gray-400" fill="currentColor" viewBox="0 0 8 8">
                  <circle cx="4" cy="4" r="3" />
                </svg>
                ${displayContext != undefined ? displayContext.activityModel.activityId : ''}
              </span>
            </div>
          </div>
      </div>`;
    }
    return '';
  }

  renderNodes() {
    const prevTransform = this.innerD3Selected.attr('transform');
    const scaleAfter = this.zoomParams.scale;
    const root = d3.select(this.el);
    this.svgD3Selected.call(this.zoom.scaleTo, 1);
    this.dagreD3Renderer(this.innerD3Selected as any, this.graph as any);
    this.svgD3Selected.call(this.zoom.scaleTo, scaleAfter);
    this.innerD3Selected.attr('transform', prevTransform);

    if (this.zoomParams.initialZoom === true) {
      this.applyInitialZoom();
    }

    if (this.mode == WorkflowDesignerMode.Edit) {
      root.selectAll('.node.add').each((n: any) => {
        const node = this.graph.node(n) as any;

        d3.select(node.elem)
          .on('click', async e => {
            e.preventDefault();
            root.selectAll('.node.add svg').classed('elsa-text-green-400', false).classed('elsa-text-gray-400', true).classed('hover:elsa-text-blue-500', true);
            this.parentActivityId = node.activity.activityId;
            this.parentActivityOutcome = node.outcome;

            if (e.shiftKey) {
              d3.select(node.elem).select('svg').classed('elsa-text-green-400', true).classed('elsa-text-gray-400', false).classed('hover:elsa-text-blue-500', false);
              return;
            }

            if (this.mode !== WorkflowDesignerMode.Test) await this.showActivityPicker();
          })
          .on('mouseover', e => {
            if (e.shiftKey) d3.select(node.elem).select('svg').classed('elsa-text-green-400', true).classed('hover:elsa-text-blue-500', false);
          })
          .on('mouseout', e => {
            d3.select(node.elem).select('svg').classed('elsa-text-green-400', false).classed('hover:elsa-text-blue-500', true);
          })
          .on('contextmenu', e => {
            e.preventDefault();
            e.stopPropagation();
            this.parentActivityId = node.activity.activityId;
            this.parentActivityOutcome = node.outcome;
            this.handleConnectionContextMenuChange({ x: e.clientX, y: e.clientY, shown: true, activity: node.activity });
          });
      });

      root.selectAll('.node.start').each((n: any) => {
        const node = this.graph.node(n) as any;
        d3.select(node.elem).on('click', async e => {
          if (this.mode == WorkflowDesignerMode.Test) return;

          await this.showActivityPicker();
        });
      });

      root.selectAll('.edgePath').append(appendClickableEl).attr('class', 'label-clickable');

      function appendClickableEl() {
        const originalD = this.querySelector('.path').getAttribute('d');
        const newPath = document.createElementNS('http://www.w3.org/2000/svg', 'path');
        newPath.setAttribute('d', originalD);
        return this.appendChild(newPath);
      }

      root.selectAll('.edgePath').each((edg: any) => {
        const edge = this.graph.edge(edg) as any;
        d3.select(edge.elem).on('contextmenu', e => {
          e.preventDefault();
          const from = edg.v.split('/');
          const to = edg.w.split('/');
          const fromActivityId = from[0];
          const outcome = from[1] || to[1];
          this.removeConnection(fromActivityId, outcome);
        });
      });
    }

    root.selectAll('.node.activity').each((n: any) => {
      const node = this.graph.node(n) as any;
      const activity = node.activity;
      const activityId = activity.activityId;

      d3.select(node.elem)
        .select('.expand')
        .on('click', e => {
          const activityContext = this.activityDisplayContexts[activityId];

          if (activityContext) {
            activityContext.expanded = !activityContext.expanded;

            this.safeRender();
          }
        });

      d3.select(node.elem)
        .on('click', e => {
          // If a parent activity was selected to connect to:
          if (this.mode == WorkflowDesignerMode.Edit && this.parentActivityId && this.parentActivityOutcome) {
            this.addConnection(this.parentActivityId, activityId, this.parentActivityOutcome);
          } else {
            // When clicking an activity with shift:
            if (e.shiftKey) {
              if (!!this.selectedActivities[activityId]) delete this.selectedActivities[activityId];
              else {
                this.selectedActivities[activityId] = activity;
                this.activitySelected.emit(activity);
              }
            }
            // When clicking an activity:
            else {
              if (!!this.selectedActivities[activityId]) delete this.selectedActivities[activityId];
              else {
                for (const key in this.selectedActivities) {
                  this.activityDeselected.emit(this.selectedActivities[key]);
                }
                this.selectedActivities = new Map<string, ActivityModel>();
                this.selectedActivities[activityId] = activity;
                this.activitySelected.emit(activity);
              }
            }
          }
        })
        .on('dblclick', async e => {
          e.stopPropagation();
          if (this.mode == WorkflowDesignerMode.Edit) {
            await this.showActivityEditor(activity, true);
            if (!this.selectedActivities[activityId]) {
              for (const key in this.selectedActivities) {
                this.activityDeselected.emit(this.selectedActivities[key]);
              }
              this.selectedActivities = new Map<string, ActivityModel>();
              this.selectedActivities[activityId] = activity;
              this.activitySelected.emit(activity);
            }
          }
        });

      if (this.mode == WorkflowDesignerMode.Edit || this.mode == WorkflowDesignerMode.Instance) {
        d3.select(node.elem)
          .select('.context-menu-button-container button')
          .on('click', evt => {
            evt.stopPropagation();
            this.handleContextMenuChange({
              x: evt.clientX,
              y: evt.clientY,
              shown: true,
              activity: node.activity,
              selectedActivities: this.selectedActivities,
            });
          });
      } else if (this.mode == WorkflowDesignerMode.Test) {
        d3.select(node.elem)
          .select('.context-menu-button-container button')
          .on('click', evt => {
            evt.stopPropagation();
            this.handleContextMenuTestChange({ x: evt.clientX, y: evt.clientY, shown: true, activity: node.activity });
          });
      }
    });
  }

  async showActivityEditor(activity: ActivityModel, animate: boolean) {
    await this.showActivityEditorInternal(activity, animate);
  }

  async showActivityEditorInternal(activity: ActivityModel, animate: boolean) {
    await eventBus.emit(EventTypes.ActivityEditor.Show, this, activity, animate);
  }

  addConnection(sourceActivityId: string, targetActivityId: string, outcome: string) {
    const workflowModel = { ...this.workflowModel };
    const newConnection: ConnectionModel = { sourceId: sourceActivityId, targetId: targetActivityId, outcome: outcome };
    let connections = workflowModel.connections;

    if (!this.enableMultipleConnectionsFromSingleSource) connections = [...workflowModel.connections.filter(x => !(x.sourceId === sourceActivityId && x.outcome === outcome))];

    workflowModel.connections = [...connections, newConnection];
    this.updateWorkflowModel(workflowModel);
    this.parentActivityId = null;
    this.parentActivityOutcome = null;
  }

  removeConnection(sourceId: string, outcome: string) {
    let workflowModel = { ...this.workflowModel };
    workflowModel = removeConnection(workflowModel, sourceId, outcome);
    this.updateWorkflowModel(workflowModel);
  }

  async showActivityPicker() {
    await eventBus.emit(EventTypes.ShowActivityPicker);
  }

  applyInitialZoom() {
    const { width: widthSvg }: { width: number } = this.svgD3Selected.node().getBBox();
    const middleScreen: number = this.svgD3Selected.node().clientWidth / 2;
    const nodeStartTransform: string = d3.select('.node.start').attr('transform');
    const nodeStartTranslateX: number = parseInt(nodeStartTransform.replace(/translate|((\)|\())/g, '').split(',')[0]);

    const zoomParamsScale: number = 1;
    const zoomParamsY: number = 50;
    const zoomParamsX: number = middleScreen - (widthSvg - (widthSvg - nodeStartTranslateX));

    this.zoom.scaleTo(this.svgD3Selected, zoomParamsScale);
    this.zoom.translateTo(this.svgD3Selected, zoomParamsX, zoomParamsY);

    this.svgD3Selected.call(this.zoom.transform, d3.zoomIdentity.scale(zoomParamsScale).translate(zoomParamsX, zoomParamsY));

    this.zoomParams.initialZoom = false;
  }

  updateWorkflowModel(model: WorkflowModel, emitEvent: boolean = true): WorkflowModel {
    const cleanedModel = this.cleanWorkflowModel(model);
    this.oldActivityDisplayContexts = this.activityDisplayContexts;
    this.activityDisplayContexts = null;

    if (emitEvent) this.workflowChanged.emit(cleanedModel);
    return cleanedModel;
  }

  cleanWorkflowModel(model: WorkflowModel): WorkflowModel {
    // Detect duplicate activities and throw.
    const activityIds = model.activities.map(x => x.activityId);
    const count = ids => ids.reduce((a, b) => ({ ...a, [b]: (a[b] || 0) + 1 }), {});
    const duplicates = dict => Object.keys(dict).filter(a => dict[a] > 1);
    const duplicateIds = duplicates(count(activityIds));

    if (duplicateIds.length > 0) {
      console.error(duplicateIds);
      throw Error(`Found duplicate activities. Throwing for now until we find the root cause.`);
    }

    model.connections = model.connections.filter(connection => {
      const sourceId = connection.sourceId;
      const targetId = connection.targetId;
      const sourceExists = model.activities.findIndex(x => x.activityId == sourceId) != null;
      const targetExists = model.activities.findIndex(x => x.activityId == targetId) != null;

      if (!sourceExists) connection.sourceId = null;

      if (!targetExists) connection.targetId = null;

      return !!connection.sourceId || !!connection.targetId;
    });

    return model;
  }

  private getLayoutDirection = () => {
    switch (this.layoutDirection) {
      case LayoutDirection.BottomTop:
        return 'BT';
      case LayoutDirection.LeftRight:
        return 'LR';
      case LayoutDirection.RightLeft:
        return 'RL';
      case LayoutDirection.TopBottom:
      default:
        return 'TB';
    }
  };
}

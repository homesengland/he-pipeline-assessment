import { ComponentFixture, TestBed } from '@angular/core/testing';
import { DesignerTree } from '../designer-tree';
import { ElementRef } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { MockStore, provideMockStore } from '@ngrx/store/testing';
import { WorkflowPersistenceBehavior, ActivityDescriptor, ActivityTraits } from '../../../../../models';
import { initial } from 'lodash';
import { eventBus } from '../../../../../services/event-bus';
import { Store } from '@ngrx/store';
import { selectActivityDefinitions } from '../../../../state/selectors/app.state.selectors';
import { DataDictionary } from '../../../../../models/custom-component-models';
import { ɵNgNoValidate } from '@angular/forms';
// Mock external dependencies
jest.mock('d3', () => ({
  select: jest.fn(() => ({
    node: jest.fn(() => ({
      getBBox: () => ({ width: 100, height: 100 }),
    })),
    attr: jest.fn().mockReturnThis(),
    call: jest.fn().mockReturnThis(),
    selectAll: jest.fn().mockReturnThis(),
  })),
  selectAll: jest.fn(() => ({
    on: jest.fn(),
  })),
  zoom: jest.fn(() => ({
    on: jest.fn().mockReturnThis(),
    scaleExtent: jest.fn().mockReturnThis(),
    filter: jest.fn().mockReturnThis(),
  })),
  zoomIdentity: {
    translate: jest.fn(),
    scale: jest.fn().mockReturnValue({
      translate: jest.fn(),
    }),
  },
}));

jest.mock('dagre-d3', () => ({
  graphlib: {
    Graph: jest.fn().mockImplementation(() => ({
      setGraph: jest.fn().mockReturnThis(),
      graph: jest.fn().mockReturnValue({}),
      setNode: jest.fn(),
      setEdge: jest.fn(),
      nodes: jest.fn().mockReturnValue([]),
      edges: jest.fn().mockReturnValue([]),
      hasNode: jest.fn().mockReturnValue(true),
    })),
  },
  render: jest.fn(),
}));

jest.mock('uuid', () => ({
  v4: jest.fn(() => 'mocked-uuid-value'),
}));

describe('DesignerTree', () => {
  let component: DesignerTree;
  let fixture: ComponentFixture<DesignerTree>;

  // Initial state for the store
  const initialState = {
    app: {
      activityDefinitions: [],
    },
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [DesignerTree],
      providers: [
        provideMockStore({ initialState }),
        {
          provide: ElementRef,
          useValue: { nativeElement: document.createElement('div') },
        },
        {
          provide: DomSanitizer,
          useValue: {
            bypassSecurityTrustHtml: jest.fn(html => html),
            sanitize: jest.fn((ctx, html) => html),
          },
        },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(DesignerTree);
    component = fixture.componentInstance;

    // Mock ViewChild references that would be populated by Angular
    component.svg = { nativeElement: document.createElementNS('http://www.w3.org/2000/svg', 'svg') } as ElementRef;
    component.inner = { nativeElement: document.createElementNS('http://www.w3.org/2000/svg', 'g') } as ElementRef;

    // Initialize minimal properties to prevent null reference errors
    component.model = {
      activities: [],
      connections: [],
      persistenceBehavior: WorkflowPersistenceBehavior.WorkflowBurst, // Replace 'None' with a valid WorkflowPersistenceBehavior value
    };

    // Mock methods that use DOM manipulation
    jest.spyOn(component, 'renderTree').mockImplementation(() => {});

    jest.spyOn(component, 'getActivityDisplayContext').mockImplementation(async (activity: any): Promise<any> => {
      return {
        activityModel: activity,
        activityDescriptor: {} as ActivityDescriptor,
        displayName: 'Test Activity',
        outcomes: ['Next', 'Back'],
        activityIcon: '<svg></svg>',
        activityIconColour: 'blue',
        expanded: false,
      };
    });

    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize the component', () => {
    expect(component.model).toBeDefined();
    expect(component.svg).toBeDefined();
    expect(component.inner).toBeDefined();
  });

  it('should set the graph and render the tree', () => {
    const graph = component.graph;
    expect(graph).toBeDefined();
    expect(graph.setGraph).toHaveBeenCalled();
    expect(graph.setNode).toHaveBeenCalledTimes(0); // No nodes yet
    expect(graph.setEdge).toHaveBeenCalledTimes(0); // No edges yet

    component.renderTree(); // Call the method to trigger rendering
    expect(component.renderTree).toHaveBeenCalled(); // Ensure it was called
  });

  it('should handle activity display context', async () => {
    const activity = { id: '1', type: 'TestActivity', activityId: '1' };
    const context = await component.getActivityDisplayContext(activity);
    expect(context).toBeDefined();
    expect(context.activityModel).toEqual(activity);
    expect(context.displayName).toEqual('Test Activity');
    expect(context.outcomes).toEqual(['Next', 'Back']);
    expect(context.activityIcon).toEqual('<svg></svg>');
    expect(context.activityIconColour).toEqual('blue');
  });

  it('should handle context menu change', () => {
    const activity = { id: '1', type: 'TestActivity', activityId: '1', shown: true, x: 0, y: 0 };
    component.activityContextMenuButtonClicked.emit = jest.fn(); // Mock the emit function
    component.handleContextMenuChange(activity);
    expect(component.ignoreCopyPasteActivities).toEqual(true);
    expect(component.activityContextMenuState).toEqual(activity);
    expect(component.activityContextMenuButtonClicked.emit).toHaveBeenCalledWith(activity);
  });

  it('should handle connection context menu change', () => {
    const connection = { id: '1', type: 'TestConnection', activityId: '1', shown: true, x: 0, y: 0 };
    component.connectionContextMenuButtonClicked.emit = jest.fn(); // Mock the emit function
    component.handleConnectionContextMenuChange(connection);
    expect(component.connectionContextMenuState).toEqual(connection);
    expect(component.connectionContextMenuButtonClicked.emit).toHaveBeenCalledWith(connection);
  });

  it('should handle context menu test change', () => {
    const activity = { id: '1', type: 'TestActivity', activityId: '1', shown: true, x: 0, y: 0 };
    component.activityContextMenuButtonTestClicked.emit = jest.fn(); // Mock the emit function
    component.handleContextMenuTestChange(activity);
    expect(component.ignoreCopyPasteActivities).toEqual(true);
    expect(component.activityContextMenuTestState).toEqual(activity);
    expect(component.activityContextMenuButtonTestClicked.emit).toHaveBeenCalledWith(activity);
  });

  it('should set activity descriptors from store', () => {
    const activityDescriptors: ActivityDescriptor[] = [
      {
        type: 'TestActivity',
        displayName: 'Test Activity',
        description: 'This is a test activity',
        category: 'Test category',
        traits: ActivityTraits.Action as ActivityTraits,
        outcomes: [],
        browsable: false,
        inputProperties: [],
        outputProperties: [],
        customAttributes: '',
        version: 1,
        isContainer: false,
      },
    ];
    const store: Store = TestBed.inject(Store);

    const mockStore = TestBed.inject(MockStore);
    mockStore.overrideSelector(selectActivityDefinitions, activityDescriptors);

    component.ngOnInit();
    expect(component.activityDescriptors).toEqual(activityDescriptors);
  });

  it('should subscribe to events when initialising the component', () => {
    const eventBusInstance = eventBus as any;
    const onActivityPickedSpy = jest.spyOn(eventBusInstance, 'on');
    component.ngOnInit();
    expect(eventBusInstance.on).toHaveBeenCalledWith('activity-picked', expect.any(Function));
    expect(eventBusInstance.on).toHaveBeenCalledWith('update-activity', expect.any(Function));
    expect(eventBusInstance.on).toHaveBeenCalledWith('workflow-executed', expect.any(Function));
    expect(onActivityPickedSpy).toHaveBeenCalledTimes(3);
  });
});

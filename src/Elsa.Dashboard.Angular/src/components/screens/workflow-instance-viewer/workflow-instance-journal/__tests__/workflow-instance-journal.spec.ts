import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ElementRef, NgModule } from '@angular/core';
import { WorkflowInstanceJournalComponent } from '../workflow-instance-journal';
import { ElsaClientService } from '../../../../../services/elsa-client';
import { WorkflowStatus } from '../../../../../models';
import moment from 'moment';

describe('WorkflowInstanceJournalComponent', () => {
  let component: WorkflowInstanceJournalComponent;
  let fixture: ComponentFixture<WorkflowInstanceJournalComponent>;
  let mockElsaClientService: jest.Mocked<ElsaClientService>;
  let mockElementRef: Partial<ElementRef>;

  const mockWorkflowInstance = {
    id: 'test-id',
    workflowStatus: WorkflowStatus.Running,
    variables: { data: {} },
  };

  const mockWorkflowBlueprint = {
    id: 'blueprint-id',
    activities: [{ id: 'activity-1', name: 'Test Activity', displayName: 'Test Display Name' }],
  };

  const mockActivityDescriptors = [{ type: 'test-type', displayName: 'Test Type' }];

  beforeEach(async () => {
    mockElsaClientService = {
      createElsaClient: jest.fn(),
    } as unknown as jest.Mocked<ElsaClientService>;

    mockElementRef = { nativeElement: document.createElement('div') };

    mockElsaClientService.createElsaClient.mockResolvedValue({
      workflowExecutionLogApi: {
        get: () => Promise.resolve({ items: [], totalCount: 0 }),
      },
    } as any);

    @NgModule({
      declarations: [WorkflowInstanceJournalComponent],
      exports: [WorkflowInstanceJournalComponent],
    })
    class TestModule {}

    await TestBed.configureTestingModule({
      imports: [TestModule],
      providers: [
        { provide: ElsaClientService, useValue: mockElsaClientService },
        { provide: ElementRef, useValue: mockElementRef },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(WorkflowInstanceJournalComponent);
    component = fixture.componentInstance;

    // Set up required inputs
    component.workflowInstance = mockWorkflowInstance as any;
    component.workflowBlueprint = mockWorkflowBlueprint as any;
    component.activityDescriptors = mockActivityDescriptors as any;
  });

  it('should create component', () => {
    expect(component).toBeTruthy();
  });

  describe('getActivityDisplayName', () => {
    it('should return activity display name when activity exists in blueprint', () => {
      const result = component.getActivityDisplayName('activity-1', 'test-type');
      expect(result).toBe('Test Display Name');
    });

    it('should return activity type display name when activity not found in blueprint', () => {
      const result = component.getActivityDisplayName('non-existent', 'test-type');
      expect(result).toBe('Test Type');
    });

    it('should return not found message for unknown activity type', () => {
      const result = component.getActivityDisplayName('non-existent', 'unknown-type');
      expect(result).toBe('(Not Found): unknown-type');
    });
  });

  describe('getEventColor', () => {
    it('should return correct color for known events', () => {
      expect(component.getEventColor('Executing')).toBe('elsa-bg-blue-500');
      expect(component.getEventColor('Executed')).toBe('elsa-bg-green-500');
      expect(component.getEventColor('Faulted')).toBe('elsa-bg-rose-500');
    });

    it('should return default color for unknown events', () => {
      expect(component.getEventColor('Unknown')).toBe('elsa-bg-gray-500');
    });
  });

  describe('getStatusColor', () => {
    it('should return correct colors for different workflow statuses', () => {
      expect(component.getStatusColor(WorkflowStatus.Idle)).toBe('gray');
      expect(component.getStatusColor(WorkflowStatus.Running)).toBe('rose');
      expect(component.getStatusColor(WorkflowStatus.Finished)).toBe('green');
      expect(component.getStatusColor(WorkflowStatus.Faulted)).toBe('red');
    });
  });

  describe('calculateDuration', () => {
    it('should return empty string for first record', () => {
      const record = { timestamp: new Date().toISOString() };
      const result = component.calculateDuration(record as any, 0);
      expect(result).toBe('');
    });

    it('should calculate duration between records', () => {
      const now = moment();
      component.records = {
        items: [
          {
            activityId: 'act1',
            timestamp: new Date(now.subtract(1, 'minute').toISOString()),
            id: '',
            workflowInstanceId: '',
            activityType: '',
            eventName: '',
          },
          {
            activityId: 'act1',
            timestamp: new Date(now.toISOString()),
            id: '',
            workflowInstanceId: '',
            activityType: '',
            eventName: '',
          },
        ],
        totalCount: 2,
      };
      component.filteredRecords = component.records.items;

      const result = component.calculateDuration(component.records.items[1] as any, 1);
      expect(result).toBeTruthy();
    });
  });

  describe('getFilteredRecordDataKeys', () => {
    it('should filter out well-known keys', () => {
      const recordData = {
        State: true,
        Input: null,
        Outcomes: true,
        CustomKey: 'value',
      };
      const result = component.getFilteredRecordDataKeys(recordData);
      expect(result).toEqual(['CustomKey']);
    });

    it('should handle empty record data', () => {
      const result = component.getFilteredRecordDataKeys({});
      expect(result).toEqual([]);
    });
  });

  describe('getRecordDataValue', () => {
    it('should handle string values', () => {
      expect(component.getRecordDataValue({ key: 'test' }, 'key')).toBe('test');
    });

    it('should handle object values', () => {
      const obj = { nested: 'value' };
      expect(component.getRecordDataValue({ key: obj }, 'key')).toBe(JSON.stringify(obj, null, 1));
    });

    it('should handle undefined values', () => {
      expect(component.getRecordDataValue({ key: undefined }, 'key')).toBeNull();
    });
  });

  describe('selectActivityRecord', () => {
    it('should select activity record and switch to journal tab', async () => {
      component.flyoutPanel = { selectTab: jest.fn() } as any;
      component.filteredRecords = [{ activityId: 'test-activity', id: 'record-1' }] as any;

      await component.selectActivityRecord('test-activity');

      expect(component.selectedRecordId).toBe('record-1');
      expect(component.flyoutPanel.selectTab).toHaveBeenCalledWith('journal', true);
    });
  });
});

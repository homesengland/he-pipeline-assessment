import { ComponentFixture, TestBed } from '@angular/core/testing';
import { WorkflowEditorNotificationsComponent } from '../workflow-editor-notifications';
import { eventBus } from '../../../../../services/event-bus';
import { EventTypes, WorkflowDefinition } from '../../../../../models';
import { toastNotificationService } from '../../../../../services/toast-notification-service';

jest.mock('../../../../../services/event-bus', () => ({
  eventBus: {
    on: jest.fn(),
    detach: jest.fn()
  }
}));

jest.mock('../../../../../services/toast-notification-service', () => ({
  toastNotificationService: {
    show: jest.fn()
  }
}));

describe('WorkflowEditorNotificationsComponent', () => {
  let component: WorkflowEditorNotificationsComponent;
  let fixture: ComponentFixture<WorkflowEditorNotificationsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [WorkflowEditorNotificationsComponent],
      providers: [
        { provide: eventBus, useValue: eventBus },
        { provide: toastNotificationService, useValue: toastNotificationService }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(WorkflowEditorNotificationsComponent);
    component = fixture.componentInstance;
  });

  it('should attach event handlers on init', () => {
    component.ngOnInit();

    expect(eventBus.on).toHaveBeenCalledWith(EventTypes.WorkflowPublished, component.onWorkflowPublished);
    expect(eventBus.on).toHaveBeenCalledWith(EventTypes.WorkflowRetracted, component.onWorkflowRetracted);
    expect(eventBus.on).toHaveBeenCalledWith(EventTypes.WorkflowImported, component.onWorkflowImported);
    expect(eventBus.on).toHaveBeenCalledWith(EventTypes.ClipboardPermissionDenied, component.onClipboardPermissionsDenied);
    expect(eventBus.on).toHaveBeenCalledWith(EventTypes.ClipboardCopied, component.onClipboardCopied);
  });

  it('should detach event handlers on destroy', () => {
    component.ngOnDestroy();

    expect(eventBus.detach).toHaveBeenCalledWith(EventTypes.WorkflowPublished, component.onWorkflowPublished);
    expect(eventBus.detach).toHaveBeenCalledWith(EventTypes.WorkflowRetracted, component.onWorkflowRetracted);
    expect(eventBus.detach).toHaveBeenCalledWith(EventTypes.WorkflowImported, component.onWorkflowImported);
    expect(eventBus.detach).toHaveBeenCalledWith(EventTypes.ClipboardPermissionDenied, component.onClipboardPermissionsDenied);
    expect(eventBus.detach).toHaveBeenCalledWith(EventTypes.ClipboardCopied, component.onClipboardCopied);
  });

  it('should show notification on workflow published', () => {
    const workflowDefinition: WorkflowDefinition = { version: 1 } as WorkflowDefinition;
    component.onWorkflowPublished(workflowDefinition);

    expect(toastNotificationService.show).toHaveBeenCalledWith('Workflow Published', 'Workflow successfully published at version 1.', 1500);
  });

  it('should show notification on workflow retracted', () => {
    const workflowDefinition: WorkflowDefinition = { version: 1 } as WorkflowDefinition;
    component.onWorkflowRetracted(workflowDefinition);

    expect(toastNotificationService.show).toHaveBeenCalledWith('Workflow Retracted', 'Workflow successfully retracted at version 1.', 1500);
  });

  it('should show notification on workflow imported', () => {
    component.onWorkflowImported();

    expect(toastNotificationService.show).toHaveBeenCalledWith('Workflow Imported', 'Workflow successfully imported.', 1500);
  });

  it('should show notification on clipboard permission denied', () => {
    component.onClipboardPermissionsDenied();

    expect(toastNotificationService.show).toHaveBeenCalledWith('Clipboard Error', 'Clipboard permission denied.', 1500);
  });

  it('should show notification on clipboard copied', () => {
    component.onClipboardCopied('Title', 'Body');

    expect(toastNotificationService.show).toHaveBeenCalledWith('Title', 'Body', 1500);
  });

  it('should show default notification on clipboard copied with no parameters', () => {
    component.onClipboardCopied('', '');

    expect(toastNotificationService.show).toHaveBeenCalledWith('Copy to Clipboard', 'Activities successfully copied to Clipboard.', 1500);
  });
});

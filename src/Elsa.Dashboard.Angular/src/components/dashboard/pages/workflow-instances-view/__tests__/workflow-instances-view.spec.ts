import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { WorkflowInstancesView } from '../workflow-instances-view';

describe('WorkflowInstancesView', () => {
  let component: WorkflowInstancesView;
  let fixture: ComponentFixture<WorkflowInstancesView>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [WorkflowInstancesView],
      imports: [HttpClientTestingModule],
    }).compileComponents();

    fixture = TestBed.createComponent(WorkflowInstancesView);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

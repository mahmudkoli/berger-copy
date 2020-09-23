import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkflowConfigurationAddComponent } from './workflow-configuration-add.component';

describe('WorkflowConfigurationAddComponent', () => {
  let component: WorkflowConfigurationAddComponent;
  let fixture: ComponentFixture<WorkflowConfigurationAddComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorkflowConfigurationAddComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkflowConfigurationAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

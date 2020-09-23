import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkflowConfigurationListComponent } from './workflow-configuration-list.component';

describe('WorkflowConfigurationListComponent', () => {
  let component: WorkflowConfigurationListComponent;
  let fixture: ComponentFixture<WorkflowConfigurationListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorkflowConfigurationListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkflowConfigurationListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

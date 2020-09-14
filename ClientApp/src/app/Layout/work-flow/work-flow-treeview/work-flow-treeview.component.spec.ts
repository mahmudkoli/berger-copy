import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkFlowTreeviewComponent } from './work-flow-treeview.component';

describe('WorkFlowTreeviewComponent', () => {
  let component: WorkFlowTreeviewComponent;
  let fixture: ComponentFixture<WorkFlowTreeviewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorkFlowTreeviewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkFlowTreeviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { JourneyPlanListLineManagerComponent } from './journey-plan-list-line-manager.component';

describe('JourneyPlanListLineManagerComponent', () => {
  let component: JourneyPlanListLineManagerComponent;
  let fixture: ComponentFixture<JourneyPlanListLineManagerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ JourneyPlanListLineManagerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(JourneyPlanListLineManagerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

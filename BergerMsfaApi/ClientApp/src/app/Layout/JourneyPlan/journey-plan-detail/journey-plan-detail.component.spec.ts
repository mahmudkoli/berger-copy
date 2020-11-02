import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { JourneyPlanDetailComponent } from './journey-plan-detail.component';

describe('JourneyPlanDetailComponent', () => {
  let component: JourneyPlanDetailComponent;
  let fixture: ComponentFixture<JourneyPlanDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ JourneyPlanDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(JourneyPlanDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

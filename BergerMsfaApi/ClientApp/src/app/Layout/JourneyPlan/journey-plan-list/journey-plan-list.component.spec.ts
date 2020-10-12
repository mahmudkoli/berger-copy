import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { JourneyPlanListComponent } from './journey-plan-list.component';

describe('JourneyPlanListComponent', () => {
  let component: JourneyPlanListComponent;
  let fixture: ComponentFixture<JourneyPlanListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ JourneyPlanListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(JourneyPlanListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { JourneyPlanAddComponent } from './journey-plan-add.component';

describe('JourneyPlanAddComponent', () => {
  let component: JourneyPlanAddComponent;
  let fixture: ComponentFixture<JourneyPlanAddComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ JourneyPlanAddComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(JourneyPlanAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

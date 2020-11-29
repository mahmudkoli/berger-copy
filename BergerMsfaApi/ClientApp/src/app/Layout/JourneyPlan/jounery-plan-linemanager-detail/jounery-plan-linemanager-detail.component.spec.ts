import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { JouneryPlanLinemanagerDetailComponent } from './jounery-plan-linemanager-detail.component';

describe('JouneryPlanLinemanagerDetailComponent', () => {
  let component: JouneryPlanLinemanagerDetailComponent;
  let fixture: ComponentFixture<JouneryPlanLinemanagerDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ JouneryPlanLinemanagerDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(JouneryPlanLinemanagerDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ColorBankInstallationPlanVsActualComponent } from './color-bank-installation-plan-vs-actual.component';

describe('ColorBankInstallationPlanVsActualComponent', () => {
  let component: ColorBankInstallationPlanVsActualComponent;
  let fixture: ComponentFixture<ColorBankInstallationPlanVsActualComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ColorBankInstallationPlanVsActualComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ColorBankInstallationPlanVsActualComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

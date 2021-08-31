import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ColorBankTargetSetupComponent } from './color-bank-target-setup.component';

describe('ColorBankTargetSetupComponent', () => {
  let component: ColorBankTargetSetupComponent;
  let fixture: ComponentFixture<ColorBankTargetSetupComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ColorBankTargetSetupComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ColorBankTargetSetupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

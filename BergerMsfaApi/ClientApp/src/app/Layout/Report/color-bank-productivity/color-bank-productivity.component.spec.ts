import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ColorBankProductivityComponent } from './color-bank-productivity.component';

describe('ColorBankProductivityComponent', () => {
  let component: ColorBankProductivityComponent;
  let fixture: ComponentFixture<ColorBankProductivityComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ColorBankProductivityComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ColorBankProductivityComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SchemedetailAddComponent } from './schemedetail-add.component';

describe('SchemedetailAddComponent', () => {
  let component: SchemedetailAddComponent;
  let fixture: ComponentFixture<SchemedetailAddComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SchemedetailAddComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SchemedetailAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

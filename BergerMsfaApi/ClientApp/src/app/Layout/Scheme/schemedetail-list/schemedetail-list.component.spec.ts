import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SchemedetailListComponent } from './schemedetail-list.component';

describe('SchemedetailListComponent', () => {
  let component: SchemedetailListComponent;
  let fixture: ComponentFixture<SchemedetailListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SchemedetailListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SchemedetailListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

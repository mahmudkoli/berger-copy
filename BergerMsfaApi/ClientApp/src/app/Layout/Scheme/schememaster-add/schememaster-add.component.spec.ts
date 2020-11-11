import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SchememasterAddComponent } from './schememaster-add.component';

describe('SchememasterAddComponent', () => {
  let component: SchememasterAddComponent;
  let fixture: ComponentFixture<SchememasterAddComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SchememasterAddComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SchememasterAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SchememasterListComponent } from './schememaster-list.component';

describe('SchememasterListComponent', () => {
  let component: SchememasterListComponent;
  let fixture: ComponentFixture<SchememasterListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SchememasterListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SchememasterListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

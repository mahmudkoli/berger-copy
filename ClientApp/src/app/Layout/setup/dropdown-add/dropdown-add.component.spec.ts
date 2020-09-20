import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DropdownAddComponent } from './dropdown-add.component';

describe('DropdownAddComponent', () => {
  let component: DropdownAddComponent;
  let fixture: ComponentFixture<DropdownAddComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DropdownAddComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DropdownAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

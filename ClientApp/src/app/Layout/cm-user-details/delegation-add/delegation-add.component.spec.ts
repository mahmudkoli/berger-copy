import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DelegationAddComponent } from './delegation-add.component';

describe('DelegationAddComponent', () => {
  let component: DelegationAddComponent;
  let fixture: ComponentFixture<DelegationAddComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DelegationAddComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DelegationAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

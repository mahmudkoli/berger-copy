import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalMenuActivityComponent } from './modal-menu-activity.component';

describe('ModalMenuActivityComponent', () => {
  let component: ModalMenuActivityComponent;
  let fixture: ComponentFixture<ModalMenuActivityComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModalMenuActivityComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModalMenuActivityComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

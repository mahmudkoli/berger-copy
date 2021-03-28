import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalPainterCallDetailsComponent } from './modal-painter-call-details.component';

describe('ModalPainterCallDetailsComponent', () => {
  let component: ModalPainterCallDetailsComponent;
  let fixture: ComponentFixture<ModalPainterCallDetailsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ModalPainterCallDetailsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ModalPainterCallDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

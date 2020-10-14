import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PainterRegisDetailComponent } from './painter-regis-detail.component';

describe('PainterRegisDetailComponent', () => {
  let component: PainterRegisDetailComponent;
  let fixture: ComponentFixture<PainterRegisDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PainterRegisDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PainterRegisDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

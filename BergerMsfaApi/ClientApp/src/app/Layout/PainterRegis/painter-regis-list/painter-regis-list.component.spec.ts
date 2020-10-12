import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PainterRegisListComponent } from './painter-regis-list.component';

describe('PainterRegisListComponent', () => {
  let component: PainterRegisListComponent;
  let fixture: ComponentFixture<PainterRegisListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PainterRegisListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PainterRegisListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FocusdealerAddComponent } from './focusdealer-add.component';

describe('FocusdealerAddComponent', () => {
  let component: FocusdealerAddComponent;
  let fixture: ComponentFixture<FocusdealerAddComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FocusdealerAddComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FocusdealerAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

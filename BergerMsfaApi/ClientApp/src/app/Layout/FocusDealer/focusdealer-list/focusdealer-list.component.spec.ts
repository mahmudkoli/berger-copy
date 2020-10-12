import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FocusdealerListComponent } from './focusdealer-list.component';

describe('FocusdealerListComponent', () => {
  let component: FocusdealerListComponent;
  let fixture: ComponentFixture<FocusdealerListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FocusdealerListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FocusdealerListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TintingmachineListComponent } from './tintingmachine-list.component';

describe('TintingmachineListComponent', () => {
  let component: TintingmachineListComponent;
  let fixture: ComponentFixture<TintingmachineListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TintingmachineListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TintingmachineListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

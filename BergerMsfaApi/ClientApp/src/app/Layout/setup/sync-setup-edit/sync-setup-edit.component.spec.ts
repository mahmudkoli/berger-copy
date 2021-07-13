import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SyncSetupEditComponent } from './sync-setup-edit.component';

describe('SyncSetupEditComponent', () => {
  let component: SyncSetupEditComponent;
  let fixture: ComponentFixture<SyncSetupEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SyncSetupEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SyncSetupEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

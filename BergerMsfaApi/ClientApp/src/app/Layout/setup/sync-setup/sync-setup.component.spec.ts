import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SyncSetupComponent } from './sync-setup.component';

describe('SyncSetupComponent', () => {
  let component: SyncSetupComponent;
  let fixture: ComponentFixture<SyncSetupComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SyncSetupComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SyncSetupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

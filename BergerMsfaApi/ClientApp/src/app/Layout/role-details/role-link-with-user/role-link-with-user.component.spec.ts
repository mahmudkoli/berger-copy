import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RoleLinkWithUserComponent } from './role-link-with-user.component';

describe('RoleLinkWithUserComponent', () => {
  let component: RoleLinkWithUserComponent;
  let fixture: ComponentFixture<RoleLinkWithUserComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RoleLinkWithUserComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RoleLinkWithUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

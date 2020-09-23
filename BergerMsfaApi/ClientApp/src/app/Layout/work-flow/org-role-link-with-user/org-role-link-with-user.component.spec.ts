import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OrgRoleLinkWithUserComponent } from './org-role-link-with-user.component';

describe('OrgRoleLinkWithUserComponent', () => {
  let component: OrgRoleLinkWithUserComponent;
  let fixture: ComponentFixture<OrgRoleLinkWithUserComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OrgRoleLinkWithUserComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OrgRoleLinkWithUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

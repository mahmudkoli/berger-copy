import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OrganizationRoleEditComponent } from './organization-role-edit.component';

describe('OrganizationRoleEditComponent', () => {
    let component: OrganizationRoleEditComponent;
    let fixture: ComponentFixture<OrganizationRoleEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OrganizationRoleEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
      fixture = TestBed.createComponent(OrganizationRoleEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

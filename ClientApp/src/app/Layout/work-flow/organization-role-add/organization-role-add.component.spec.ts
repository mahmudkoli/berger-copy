import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OrganizationRoleAddComponent } from './organization-role-add.component';

describe('OrganizationRoleAddComponent', () => {
  let component: OrganizationRoleAddComponent;
  let fixture: ComponentFixture<OrganizationRoleAddComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OrganizationRoleAddComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OrganizationRoleAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OrganizationRoleListComponent } from './organization-role-list.component';

describe('OrganizationRoleListComponent', () => {
  let component: OrganizationRoleListComponent;
  let fixture: ComponentFixture<OrganizationRoleListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OrganizationRoleListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OrganizationRoleListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

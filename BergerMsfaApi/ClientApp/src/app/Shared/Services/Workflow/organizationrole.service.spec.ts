import { TestBed } from '@angular/core/testing';

import { OrganizationRoleService } from './organizationrole.service';

describe('OrganizationroleService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
      const service: OrganizationRoleService = TestBed.get(OrganizationRoleService);
    expect(service).toBeTruthy();
  });
});

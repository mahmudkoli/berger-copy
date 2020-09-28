import { TestBed } from '@angular/core/testing';

import { DynamicDropdownService } from './dynamic-dropdown.service';

describe('DynamicDropdownService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: DynamicDropdownService = TestBed.get(DynamicDropdownService);
    expect(service).toBeTruthy();
  });
});

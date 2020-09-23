import { TestBed } from '@angular/core/testing';

import { PosmProductService } from './posmproduct.service';

describe('PosmProductService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
      const service: PosmProductService = TestBed.get(PosmProductService);
    expect(service).toBeTruthy();
  });
});

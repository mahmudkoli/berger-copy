import { TestBed } from '@angular/core/testing';

import { DealeropeningService } from './dealeropening.service';

describe('DealeropeningService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: DealeropeningService = TestBed.get(DealeropeningService);
    expect(service).toBeTruthy();
  });
});

import { TestBed } from '@angular/core/testing';

import { CollectionentryService } from './collectionentry.service';

describe('CollectionentryService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: CollectionentryService = TestBed.get(CollectionentryService);
    expect(service).toBeTruthy();
  });
});

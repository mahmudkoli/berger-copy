import { TestBed } from '@angular/core/testing';

import { SyncSetupService } from './sync-setup.service';

describe('SyncSetupService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: SyncSetupService = TestBed.get(SyncSetupService);
    expect(service).toBeTruthy();
  });
});

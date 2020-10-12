import { TestBed } from '@angular/core/testing';

import { FocusdealerService } from './focusdealer.service';

describe('FocusdealerService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: FocusdealerService = TestBed.get(FocusdealerService);
    expect(service).toBeTruthy();
  });
});

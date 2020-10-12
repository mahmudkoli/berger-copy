import { TestBed } from '@angular/core/testing';

import { JourneyPlanService } from './journey-plan.service';

describe('JourneyPlanService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: JourneyPlanService = TestBed.get(JourneyPlanService);
    expect(service).toBeTruthy();
  });
});

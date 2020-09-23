import { TestBed } from '@angular/core/testing';

import { WorkflowconfigurationService } from './workflowconfiguration.service';

describe('WorkflowconfigurationService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: WorkflowconfigurationService = TestBed.get(WorkflowconfigurationService);
    expect(service).toBeTruthy();
  });
});

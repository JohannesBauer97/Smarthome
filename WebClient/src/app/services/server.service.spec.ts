import { TestBed, inject } from '@angular/core/testing';

import { LedService } from './led.service';

describe('LedService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [LedService]
    });
  });

  it('should be created', inject([LedService], (service: LedService) => {
    expect(service).toBeTruthy();
  }));
});

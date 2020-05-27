import { TestBed } from '@angular/core/testing';

import { TemperatureApiService } from './temperature-api.service';

describe('TemperatureApiService', () => {
  let service: TemperatureApiService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TemperatureApiService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ShardingService } from './sharding.service';
import { API_SHARDING } from '../../../../core/helpers/routes/api-endpoints';

describe('ShardingService', () => {
  let service: ShardingService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [ShardingService]
    });
    service = TestBed.inject(ShardingService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should fetch all shardings', () => {
    const mockResponse = { 
      data: [], 
      isSuccess: true, 
      isFailure: false, 
      error: { code: '', description: '', type: 0 } // Provide an empty ApiError object
    };
    service.getAll().subscribe(res => {
      expect(res).toEqual(mockResponse);
    });
    const req = httpMock.expectOne(API_SHARDING);
    expect(req.request.method).toBe('GET');
    req.flush(mockResponse);
  });
});

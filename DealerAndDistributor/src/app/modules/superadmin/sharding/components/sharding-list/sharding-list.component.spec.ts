import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ShardingListComponent } from './sharding-list.component';
import { ShardingService } from '../../service/sharding.service';
import { of } from 'rxjs';
import { Sharding } from '../../models/sharding.model';

describe('ShardingListComponent', () => {
  let component: ShardingListComponent;
  let fixture: ComponentFixture<ShardingListComponent>;
  let shardingServiceSpy: jasmine.SpyObj<ShardingService>;

  beforeEach(async () => {
    shardingServiceSpy = jasmine.createSpyObj('ShardingService', ['getAll']);
    await TestBed.configureTestingModule({
      declarations: [ShardingListComponent],
      providers: [
        { provide: ShardingService, useValue: shardingServiceSpy }
      ]
    }).compileComponents();
    fixture = TestBed.createComponent(ShardingListComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load shardings on init', () => {
    const mockData: Sharding[] = [
      { name: 'Default Database', databaseName: null, connectionName: 'DefaultConnection', databaseType: 'SqlServer' }
    ];
    shardingServiceSpy.getAll.and.returnValue(of({ data: mockData, isSuccess: true, isFailure: false, error: null }));
    component.ngOnInit();
    expect(component.shardings()).toEqual(mockData);
  });
});

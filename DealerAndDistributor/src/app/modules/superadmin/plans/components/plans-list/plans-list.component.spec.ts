import { TestBed } from '@angular/core/testing';
import { PlansListComponent } from './plans-list.component';
import { of } from 'rxjs';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { PlansService } from '../../service/plans.service';

describe('PlansListComponent', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PlansListComponent],
      providers: [
        {
          provide: PlansService,
          useValue: { list: () => of({ isSuccess: true, isFailure: false, error: { code: '', description: '', type: 0 }, data: [] }) }
        }
      ],
      schemas: [NO_ERRORS_SCHEMA]
    }).compileComponents();
  });

  it('should create', () => {
    const fixture = TestBed.createComponent(PlansListComponent);
    const component = fixture.componentInstance;
    fixture.detectChanges();
    expect(component).toBeTruthy();
  });
});

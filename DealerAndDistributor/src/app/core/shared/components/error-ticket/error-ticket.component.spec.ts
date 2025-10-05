import { TestBed } from '@angular/core/testing';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ErrorTicketComponent } from './error-ticket.component';

describe('ErrorTicketComponent', () => {
  it('should create', () => {
    const dialogRefSpy = jasmine.createSpyObj('MatDialogRef', ['close']);
    TestBed.configureTestingModule({
      providers: [
        { provide: MatDialogRef, useValue: dialogRefSpy },
        { provide: MAT_DIALOG_DATA, useValue: { payload: { message: 'x', url: '/', method: 'GET', userAgent: 'ua', timestamp: new Date().toISOString() } } },
      ]
    });
    const fixture = TestBed.createComponent(ErrorTicketComponent);
    expect(fixture.componentInstance).toBeTruthy();
  });
});

import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { UpdateTicketDialogComponent } from './update-ticket-dialog.component';

describe('UpdateTicketDialogComponent', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideHttpClient(),
        { provide: MatDialogRef, useValue: { close: () => {} } },
        { provide: MAT_DIALOG_DATA, useValue: { id: 'T-1' } },
      ]
    });
  });

  it('should create', () => {
    const fixture = TestBed.createComponent(UpdateTicketDialogComponent);
    const comp = fixture.componentInstance;
    expect(comp).toBeTruthy();
  });
});

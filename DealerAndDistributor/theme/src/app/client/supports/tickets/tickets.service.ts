import { Injectable } from '@angular/core';
import { Observable, of, throwError } from 'rxjs';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Tickets } from './tickets.model';
import { catchError, map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class TicketsService {
  private readonly API_URL = 'assets/data/tickets.json';

  constructor(private httpClient: HttpClient) {}

  /** GET: Fetch all tickets */
  getTickets(): Observable<Tickets[]> {
    return this.httpClient
      .get<Tickets[]>(this.API_URL)
      .pipe(catchError(this.handleError));
  }

  /** POST: Add a new ticket */
  addTicket(ticket: Tickets): Observable<Tickets> {
    // Add the new ticket to the data array
    return of(ticket).pipe(
      map(() => ticket), // Return the added ticket
      catchError(this.handleError)
    );

    // API call to add the ticket
    // return this.httpClient.post<Tickets>(this.API_URL, ticket).pipe(
    //   map(() => ticket), // Return the added ticket
    //   catchError(this.handleError)
    // );
  }

  /** PUT: Update an existing ticket */
  updateTicket(ticket: Tickets): Observable<Tickets> {
    // Update the ticket in the data array
    return of(ticket).pipe(
      map(() => ticket), // Return the updated ticket
      catchError(this.handleError)
    );

    // API call to update the ticket
    // return this.httpClient.put<Tickets>(`${this.API_URL}`, ticket).pipe(
    //   map(() => ticket), // Return the updated ticket
    //   catchError(this.handleError)
    // );
  }

  /** DELETE: Remove a ticket by ID */
  deleteTicket(id: number): Observable<number> {
    // Update the ticket in the data array
    return of(id).pipe(
      map(() => id), // Return the ID of the deleted ticket
      catchError(this.handleError)
    );

    // API call to delete the ticket
    // return this.httpClient.delete<void>(`${this.API_URL}`).pipe(
    //   map(() => id), // Return the ID of the deleted ticket
    //   catchError(this.handleError)
    // );
  }

  /** Handle HTTP errors */
  private handleError(error: HttpErrorResponse): Observable<never> {
    console.error('An error occurred:', error.message);
    return throwError(
      () => new Error('Something went wrong; please try again later.')
    );
  }
}

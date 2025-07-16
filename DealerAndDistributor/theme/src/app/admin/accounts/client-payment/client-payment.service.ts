import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of, throwError } from 'rxjs';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { ClientPayment } from './client-payment.model';

@Injectable({
  providedIn: 'root',
})
export class ClientPaymentService {
  private readonly API_URL = 'assets/data/clientPayment.json';
  dataChange: BehaviorSubject<ClientPayment[]> = new BehaviorSubject<
    ClientPayment[]
  >([]);

  constructor(private httpClient: HttpClient) {}

  /** GET: Fetch all payments */
  getAllPayments(): Observable<ClientPayment[]> {
    return this.httpClient
      .get<ClientPayment[]>(this.API_URL)
      .pipe(catchError(this.handleError));
  }

  /** POST: Add a new client payment */
  addPayment(clientPayment: ClientPayment): Observable<ClientPayment> {
    // Add the new payment to the data array
    return of(clientPayment).pipe(
      map((response) => {
        return response;
      }),
      catchError(this.handleError)
    );

    // API call to add the client payment
    // return this.httpClient.post<ClientPayment>(this.API_URL, clientPayment).pipe(
    //   map(() => {
    //     return clientPayment; // return newly added client payment
    //   }),
    //   catchError(this.handleError)
    // );
  }

  /** PUT: Update an existing client payment */
  updatePayment(clientPayment: ClientPayment): Observable<ClientPayment> {
    // Update the client payment in the data array
    return of(clientPayment).pipe(
      map((response) => {
        return response;
      }),
      catchError(this.handleError)
    );

    // API call to update the client payment
    // return this.httpClient.put<ClientPayment>(`${this.API_URL}`, clientPayment).pipe(
    //   map(() => {
    //     return clientPayment; // return updated client payment
    //   }),
    //   catchError(this.handleError)
    // );
  }

  /** DELETE: Remove a client payment by ID */
  deletePayment(id: number): Observable<number> {
    // Update the client payment in the data array
    return of(id).pipe(
      map((response) => {
        return id;
      }),
      catchError(this.handleError)
    );

    // API call to delete the client payment
    // return this.httpClient.delete<void>(`${this.API_URL}`).pipe(
    //   map(() => {
    //     return id; // return the ID of the deleted client payment
    //   }),
    //   catchError(this.handleError)
    // );
  }

  /** Handle Http operation that failed */
  private handleError(error: HttpErrorResponse): Observable<never> {
    console.error('An error occurred:', error.message);
    return throwError(
      () => new Error('Something went wrong; please try again later.')
    );
  }
}

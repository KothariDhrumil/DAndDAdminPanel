import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of, throwError } from 'rxjs';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { Estimates } from './estimates.model';

@Injectable({
  providedIn: 'root',
})
export class EstimatesService {
  private readonly API_URL = 'assets/data/estimates.json';
  dataChange: BehaviorSubject<Estimates[]> = new BehaviorSubject<Estimates[]>(
    []
  );

  constructor(private httpClient: HttpClient) {}

  /** GET: Fetch all estimates */
  getAllEstimates(): Observable<Estimates[]> {
    return this.httpClient
      .get<Estimates[]>(this.API_URL)
      .pipe(catchError(this.handleError));
  }

  /** POST: Add a new estimate */
  addEstimate(estimates: Estimates): Observable<Estimates> {
    // Add the new estimate to the data array
    return of(estimates).pipe(
      map((response) => {
        return response; // return the newly added estimate
      }),
      catchError(this.handleError)
    );

    // API call to add the estimate
    // return this.httpClient.post<Estimates>(this.API_URL, estimates).pipe(
    //   map(() => {
    //     return estimates; // return the newly added estimate
    //   }),
    //   catchError(this.handleError)
    // );
  }

  /** PUT: Update an existing estimate */
  updateEstimate(estimates: Estimates): Observable<Estimates> {
    // Update the estimate in the data array
    return of(estimates).pipe(
      map((response) => {
        return response; // return the updated estimate
      }),
      catchError(this.handleError)
    );

    // API call to update the estimate
    // return this.httpClient.put<Estimates>(`${this.API_URL}`, estimates).pipe(
    //   map(() => {
    //     return estimates; // return the updated estimate
    //   }),
    //   catchError(this.handleError)
    // );
  }

  /** DELETE: Remove an estimate by ID */
  deleteEstimate(id: number): Observable<number> {
    // Update the estimate in the data array
    return of(id).pipe(
      map((response) => {
        return id; // return the ID of the deleted estimate
      }),
      catchError(this.handleError)
    );

    // API call to delete the estimate
    // return this.httpClient.delete<void>(`${this.API_URL}`).pipe(
    //   map(() => {
    //     return id; // return the ID of the deleted estimate
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

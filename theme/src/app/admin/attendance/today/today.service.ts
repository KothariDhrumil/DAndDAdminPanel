import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of, throwError } from 'rxjs';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { Today } from './today.model';

@Injectable({
  providedIn: 'root',
})
export class TodayService {
  private readonly API_URL = 'assets/data/today.json';
  private dataChange: BehaviorSubject<Today[]> = new BehaviorSubject<Today[]>(
    []
  );

  constructor(private httpClient: HttpClient) {}

  get data(): Today[] {
    return this.dataChange.value;
  }

  /** GET: Fetch all today's data */
  getAllTodays(): Observable<Today[]> {
    return this.httpClient.get<Today[]>(this.API_URL).pipe(
      map((todays) => {
        this.dataChange.next(todays); // Update the data change observable
        return todays;
      }),
      catchError(this.handleError)
    );
  }

  /** POST: Add a new today's data */
  addToday(today: Today): Observable<Today> {
    // Add the new data to the data array
    return of(today).pipe(
      map((response) => {
        return response;
      }),
      catchError(this.handleError)
    );

    // API call to add today's data
    // return this.httpClient.post<Today>(this.API_URL, today).pipe(
    //   map(() => {
    //     return today; // Return the newly added today's data
    //   }),
    //   catchError(this.handleError)
    // );
  }

  /** PUT: Update an existing today's data */
  updateToday(today: Today): Observable<Today> {
    // Update the data in the data array
    return of(today).pipe(
      map((response) => {
        return response;
      }),
      catchError(this.handleError)
    );

    // API call to update today's data
    // return this.httpClient.put<Today>(`${this.API_URL}`, today).pipe(
    //   map(() => {
    //     return today; // Return the updated today's data
    //   }),
    //   catchError(this.handleError)
    // );
  }

  /** DELETE: Remove today's data by ID */
  deleteToday(id: number): Observable<number> {
    // Simulate deleting by returning the ID
    return of(id).pipe(
      map((response) => {
        return id; // Return the ID of the deleted data
      }),
      catchError(this.handleError)
    );

    // API call to delete today's data
    // return this.httpClient.delete<void>(`${this.API_URL}`).pipe(
    //   map(() => {
    //     return id; // Return the ID of the deleted data
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

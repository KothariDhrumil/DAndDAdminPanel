import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of, throwError } from 'rxjs';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { AllHoliday } from './all-holidays.model';

@Injectable({
  providedIn: 'root',
})
export class HolidayService {
  private readonly API_URL = 'assets/data/holidays.json';
  private dataChange: BehaviorSubject<AllHoliday[]> = new BehaviorSubject<
    AllHoliday[]
  >([]);

  constructor(private httpClient: HttpClient) {}

  get data(): AllHoliday[] {
    return this.dataChange.value;
  }

  /** GET: Fetch all holidays */
  getAllHolidays(): Observable<AllHoliday[]> {
    return this.httpClient.get<AllHoliday[]>(this.API_URL).pipe(
      map((holidays) => {
        this.dataChange.next(holidays); // Update the data change observable
        return holidays;
      }),
      catchError(this.handleError)
    );

    // Local mock data
    // return of([{ id: 1, name: 'New Year', date: '2025-01-01' }]);

    // API call
    // return this.httpClient.get<AllHoliday[]>(this.API_URL).pipe(
    //   map((holidays) => {
    //     this.dataChange.next(holidays);
    //     return holidays;
    //   }),
    //   catchError(this.handleError)
    // );
  }

  /** POST: Add a new holiday */
  addHoliday(holiday: AllHoliday): Observable<AllHoliday> {
    // Local mock data
    return of(holiday).pipe(
      map((response) => response),
      catchError(this.handleError)
    );

    // API call
    // return this.httpClient.post<AllHoliday>(this.API_URL, holiday).pipe(
    //   map((response) => response),
    //   catchError(this.handleError)
    // );
  }

  /** PUT: Update an existing holiday */
  updateHoliday(holiday: AllHoliday): Observable<AllHoliday> {
    // Local mock data
    return of(holiday).pipe(
      map((response) => response),
      catchError(this.handleError)
    );

    // API call
    // return this.httpClient.put<AllHoliday>(`${this.API_URL}`, holiday).pipe(
    //   map((response) => response),
    //   catchError(this.handleError)
    // );
  }

  /** DELETE: Remove a holiday by ID */
  deleteHoliday(id: number): Observable<number> {
    // Local mock data
    return of(id).pipe(
      map((response) => id),
      catchError(this.handleError)
    );

    // API call
    // return this.httpClient.delete<void>(`${this.API_URL}`).pipe(
    //   map((response) => id),
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

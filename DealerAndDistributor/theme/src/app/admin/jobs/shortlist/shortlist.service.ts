import { Injectable } from '@angular/core';
import { Observable, of, throwError } from 'rxjs';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { Shortlist } from './shortlist.model';

@Injectable({
  providedIn: 'root',
})
export class ShortlistService {
  private readonly API_URL = 'assets/data/shortlist.json';

  constructor(private httpClient: HttpClient) {}

  /** GET: Fetch all shortlists */
  getAllShortlists(): Observable<Shortlist[]> {
    return this.httpClient
      .get<Shortlist[]>(this.API_URL)
      .pipe(catchError(this.handleError));

    // Local mock data
    // return of([
    //   { id: 1, candidateName: 'John Doe', jobTitle: 'Software Developer' },
    //   { id: 2, candidateName: 'Jane Smith', jobTitle: 'UI Designer' }
    // ]);

    // API call
    // return this.httpClient.get<Shortlist[]>(this.API_URL).pipe(
    //   catchError(this.handleError)
    // );
  }

  /** POST: Add a new shortlist */
  addShortlist(shortlist: Shortlist): Observable<Shortlist> {
    // Local mock data
    return of(shortlist).pipe(
      map((response) => response),
      catchError(this.handleError)
    );

    // API call
    // return this.httpClient.post<Shortlist>(this.API_URL, shortlist).pipe(
    //   map(() => shortlist),
    //   catchError(this.handleError)
    // );
  }

  /** PUT: Update an existing shortlist */
  updateShortlist(shortlist: Shortlist): Observable<Shortlist> {
    // Local mock data
    return of(shortlist).pipe(
      map((response) => response),
      catchError(this.handleError)
    );

    // API call
    // return this.httpClient.put<Shortlist>(this.API_URL, shortlist).pipe(
    //   map(() => shortlist),
    //   catchError(this.handleError)
    // );
  }

  /** DELETE: Remove a shortlist by ID */
  deleteShortlist(id: number): Observable<number> {
    // Local mock data
    return of(id).pipe(
      map((response) => id),
      catchError(this.handleError)
    );

    // API call
    // return this.httpClient.delete<void>(`${this.API_URL}`).pipe(
    //   map(() => id),
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

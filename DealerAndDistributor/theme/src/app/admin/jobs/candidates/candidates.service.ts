import { Injectable } from '@angular/core';
import { Observable, of, throwError } from 'rxjs';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { Candidates } from './candidates.model';

@Injectable({
  providedIn: 'root',
})
export class CandidatesService {
  private readonly API_URL = 'assets/data/candidates.json';

  constructor(private httpClient: HttpClient) {}

  /** GET: Fetch all candidates */
  getAllCandidates(): Observable<Candidates[]> {
    return this.httpClient
      .get<Candidates[]>(this.API_URL)
      .pipe(catchError(this.handleError));

    // Local mock data
    // return of([
    //   { id: 1, name: 'John Doe', position: 'Software Developer' },
    //   { id: 2, name: 'Jane Smith', position: 'UI/UX Designer' }
    // ]);

    // API call
    // return this.httpClient.get<Candidates[]>(this.API_URL).pipe(
    //   catchError(this.handleError)
    // );
  }

  /** POST: Add a new candidate */
  addCandidate(candidate: Candidates): Observable<Candidates> {
    // Local mock data
    return of(candidate).pipe(
      map((response) => response),
      catchError(this.handleError)
    );

    // API call
    // return this.httpClient.post<Candidates>(this.API_URL, candidate).pipe(
    //   map(() => candidate),
    //   catchError(this.handleError)
    // );
  }

  /** PUT: Update an existing candidate */
  updateCandidate(candidate: Candidates): Observable<Candidates> {
    // Local mock data
    return of(candidate).pipe(
      map((response) => response),
      catchError(this.handleError)
    );

    // API call
    // return this.httpClient.put<Candidates>(`${this.API_URL}`, candidate).pipe(
    //   map(() => candidate),
    //   catchError(this.handleError)
    // );
  }

  /** DELETE: Remove a candidate by ID */
  deleteCandidate(id: number): Observable<number> {
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

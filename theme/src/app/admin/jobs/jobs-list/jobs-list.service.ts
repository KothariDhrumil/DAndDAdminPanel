import { Injectable } from '@angular/core';
import { Observable, of, throwError } from 'rxjs';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { JobsList } from './jobs-list.model';

@Injectable({
  providedIn: 'root',
})
export class JobsListService {
  private readonly API_URL = 'assets/data/jobs-list.json';

  constructor(private httpClient: HttpClient) {}

  /** GET: Fetch all jobs lists */
  getAllJobsLists(): Observable<JobsList[]> {
    return this.httpClient
      .get<JobsList[]>(this.API_URL)
      .pipe(catchError(this.handleError));

    // Local mock data
    // return of([
    //   { id: 1, title: 'Software Engineer', description: 'Develop software solutions' },
    //   { id: 2, title: 'UI/UX Designer', description: 'Design intuitive user interfaces' }
    // ]);

    // API call
    // return this.httpClient.get<JobsList[]>(this.API_URL).pipe(
    //   catchError(this.handleError)
    // );
  }

  /** POST: Add a new job */
  addJobsList(jobsList: JobsList): Observable<JobsList> {
    // Local mock data
    return of(jobsList).pipe(
      map((response) => response),
      catchError(this.handleError)
    );

    // API call
    // return this.httpClient.post<JobsList>(this.API_URL, jobsList).pipe(
    //   map(() => jobsList),
    //   catchError(this.handleError)
    // );
  }

  /** PUT: Update an existing job */
  updateJobsList(jobsList: JobsList): Observable<JobsList> {
    // Local mock data
    return of(jobsList).pipe(
      map((response) => response),
      catchError(this.handleError)
    );

    // API call
    // return this.httpClient.put<JobsList>(`${this.API_URL}`, jobsList).pipe(
    //   map(() => jobsList),
    //   catchError(this.handleError)
    // );
  }

  /** DELETE: Remove a job by ID */
  deleteJobsList(id: number): Observable<number> {
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

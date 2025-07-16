import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of, throwError } from 'rxjs';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { LeaveReport } from './leave-report.model';

@Injectable({
  providedIn: 'root',
})
export class LeaveReportService {
  private readonly API_URL = 'assets/data/leave-report.json';
  dataChange: BehaviorSubject<LeaveReport[]> = new BehaviorSubject<
    LeaveReport[]
  >([]);

  constructor(private httpClient: HttpClient) {}

  /** GET: Fetch all leave reports */
  getAllLeaveReports(): Observable<LeaveReport[]> {
    return this.httpClient
      .get<LeaveReport[]>(this.API_URL)
      .pipe(catchError(this.handleError));
  }

  /** POST: Add a new leave report */
  addLeaveReport(leaveReport: LeaveReport): Observable<LeaveReport> {
    // Add the new leave report to the data array
    return of(leaveReport).pipe(
      map((response) => {
        return response; // return the newly added leave report
      }),
      catchError(this.handleError)
    );

    // API call to add the leave report
    // return this.httpClient
    //   .post<LeaveReport>(this.API_URL, leaveReport)
    //   .pipe(
    //     map(() => {
    //       return leaveReport; // return the newly added leave report
    //     }),
    //     catchError(this.handleError)
    //   );
  }

  /** PUT: Update an existing leave report */
  updateLeaveReport(leaveReport: LeaveReport): Observable<LeaveReport> {
    // Update the leave report in the data array
    return of(leaveReport).pipe(
      map((response) => {
        return response; // return the updated leave report
      }),
      catchError(this.handleError)
    );

    // API call to update the leave report
    // return this.httpClient.put<LeaveReport>(this.API_URL, leaveReport).pipe(
    //   map(() => {
    //     return leaveReport; // return the updated leave report
    //   }),
    //   catchError(this.handleError)
    // );
  }

  /** DELETE: Remove a leave report by ID */
  deleteLeaveReport(id: number): Observable<number> {
    // Update the leave report in the data array
    return of(id).pipe(
      map((response) => {
        return id; // return the ID of the deleted leave report
      }),
      catchError(this.handleError)
    );

    // API call to delete the leave report
    // return this.httpClient.delete<void>(`${this.API_URL}`).pipe(
    //   map(() => {
    //     return id; // return the ID of the deleted leave report
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

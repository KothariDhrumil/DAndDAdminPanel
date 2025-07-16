import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of, throwError } from 'rxjs';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { LeaveBalance } from './leave-balance.model';

@Injectable({
  providedIn: 'root',
})
export class LeaveBalanceService {
  private readonly API_URL = 'assets/data/leave-balance.json';
  private dataChange: BehaviorSubject<LeaveBalance[]> = new BehaviorSubject<
    LeaveBalance[]
  >([]);

  constructor(private httpClient: HttpClient) {}

  get data(): LeaveBalance[] {
    return this.dataChange.value;
  }

  /** GET: Fetch all leave balances */
  getAllLeaveBalances(): Observable<LeaveBalance[]> {
    return this.httpClient
      .get<LeaveBalance[]>(this.API_URL)
      .pipe(catchError(this.handleError));
  }

  /** POST: Add a new leave balance */
  addLeaveBalance(leaveBalance: LeaveBalance): Observable<LeaveBalance> {
    // Add the new leave balance to the data array
    return of(leaveBalance).pipe(
      map((response) => {
        return response; // return the newly added leave balance
      }),
      catchError(this.handleError)
    );

    // API call to add the leave balance
    // return this.httpClient.post<LeaveBalance>(this.API_URL, leaveBalance).pipe(
    //   map((response) => {
    //     return response; // return the newly added leave balance
    //   }),
    //   catchError(this.handleError)
    // );
  }

  /** PUT: Update an existing leave balance */
  updateLeaveBalance(leaveBalance: LeaveBalance): Observable<LeaveBalance> {
    // Update the leave balance in the data array
    return of(leaveBalance).pipe(
      map((response) => {
        return response; // return the updated leave balance
      }),
      catchError(this.handleError)
    );

    // API call to update the leave balance
    // return this.httpClient.put<LeaveBalance>(`${this.API_URL}`, leaveBalance).pipe(
    //   map((response) => {
    //     return response; // return the updated leave balance
    //   }),
    //   catchError(this.handleError)
    // );
  }

  /** DELETE: Remove a leave balance by ID */
  deleteLeaveBalance(id: number): Observable<number> {
    // Simulate deletion by returning the ID
    return of(id).pipe(
      map((response) => {
        return id; // return the ID of the deleted leave balance
      }),
      catchError(this.handleError)
    );

    // API call to delete the leave balance
    // return this.httpClient.delete<void>(`${this.API_URL}`).pipe(
    //   map((response) => {
    //     return id; // return the ID of the deleted leave balance
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

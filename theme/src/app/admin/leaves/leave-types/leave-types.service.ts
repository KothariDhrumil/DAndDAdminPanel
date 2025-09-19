import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of, throwError } from 'rxjs';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { LeaveTypes } from './leave-types.model';

@Injectable({
  providedIn: 'root',
})
export class LeaveTypesService {
  private readonly API_URL = 'assets/data/leave-types.json';
  private dataChange: BehaviorSubject<LeaveTypes[]> = new BehaviorSubject<
    LeaveTypes[]
  >([]);

  constructor(private httpClient: HttpClient) {}

  get data(): LeaveTypes[] {
    return this.dataChange.value;
  }

  /** GET: Fetch all leave types */
  getAllLeaveTypes(): Observable<LeaveTypes[]> {
    return this.httpClient.get<LeaveTypes[]>(this.API_URL).pipe(
      map((leaveTypes) => {
        this.dataChange.next(leaveTypes); // Update the data change observable
        return leaveTypes;
      }),
      catchError(this.handleError)
    );

    // API call code
    // return this.httpClient.get<LeaveTypes[]>(this.API_URL).pipe(
    //   map((leaveTypes) => {
    //     this.dataChange.next(leaveTypes); // Update the data change observable
    //     return leaveTypes;
    //   }),
    //   catchError(this.handleError)
    // );
  }

  /** POST: Add a new leave type */
  addLeaveType(leaveType: LeaveTypes): Observable<LeaveTypes> {
    // Add the new leave type to the data array
    return of(leaveType).pipe(
      map((response) => {
        return response;
      }),
      catchError(this.handleError)
    );

    // API call code
    // return this.httpClient.post<LeaveTypes>(this.API_URL, leaveType).pipe(
    //   map(() => {
    //     return leaveType; // Return the newly added leave type
    //   }),
    //   catchError(this.handleError)
    // );
  }

  /** PUT: Update an existing leave type */
  updateLeaveType(leaveType: LeaveTypes): Observable<LeaveTypes> {
    // Update the leave type in the data array
    return of(leaveType).pipe(
      map((response) => {
        return response;
      }),
      catchError(this.handleError)
    );

    // API call code
    // return this.httpClient.put<LeaveTypes>(`${this.API_URL}`, leaveType).pipe(
    //   map(() => {
    //     return leaveType; // Return the updated leave type
    //   }),
    //   catchError(this.handleError)
    // );
  }

  /** DELETE: Remove a leave type by ID */
  deleteLeaveType(id: number): Observable<number> {
    // Delete the leave type by ID in the data array
    return of(id).pipe(
      map((response) => {
        return id; // Return the ID of the deleted leave type
      }),
      catchError(this.handleError)
    );

    // API call code
    // return this.httpClient.delete<void>(`${this.API_URL}`).pipe(
    //   map(() => {
    //     return id; // Return the ID of the deleted leave type
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

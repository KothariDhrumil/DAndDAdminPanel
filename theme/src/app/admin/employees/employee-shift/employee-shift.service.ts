import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of, throwError } from 'rxjs';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { EmployeeShift } from './employee-shift.model';

@Injectable({
  providedIn: 'root',
})
export class EmployeeShiftService {
  private readonly API_URL = 'assets/data/employee-shift.json';
  dataChange: BehaviorSubject<EmployeeShift[]> = new BehaviorSubject<
    EmployeeShift[]
  >([]);
  dialogData!: EmployeeShift;

  constructor(private httpClient: HttpClient) {}

  get data(): EmployeeShift[] {
    return this.dataChange.value;
  }

  getDialogData(): EmployeeShift {
    return this.dialogData;
  }

  /** CRUD METHODS */

  /** GET: Fetch employee shifts */
  getEmployeeShifts(): Observable<EmployeeShift[]> {
    return this.httpClient.get<EmployeeShift[]>(this.API_URL).pipe(
      map((data) => {
        this.dataChange.next(data);
        return data;
      }),
      catchError(this.handleError)
    );
  }

  /** POST: Add a new employee shift */
  addEmployeeShift(employeeShift: EmployeeShift): Observable<EmployeeShift> {
    // Simulate adding the new employee shift
    return of(employeeShift).pipe(
      map((response) => {
        this.dialogData = employeeShift;
        return employeeShift;
      }),
      catchError(this.handleError)
    );

    // API call to add the employee shift
    // return this.httpClient.post<EmployeeShift>(this.API_URL, employeeShift).pipe(
    //   map((response) => {
    //     this.dialogData = employeeShift;
    //     return employeeShift;
    //   }),
    //   catchError(this.handleError)
    // );
  }

  /** PUT: Update an existing employee shift */
  updateEmployeeShift(employeeShift: EmployeeShift): Observable<EmployeeShift> {
    // Simulate updating the employee shift
    return of(employeeShift).pipe(
      map((response) => {
        this.dialogData = employeeShift;
        return employeeShift;
      }),
      catchError(this.handleError)
    );

    // API call to update the employee shift
    // return this.httpClient.put<EmployeeShift>(`${this.API_URL}`, employeeShift).pipe(
    //   map((response) => {
    //     this.dialogData = employeeShift;
    //     return employeeShift;
    //   }),
    //   catchError(this.handleError)
    // );
  }

  /** DELETE: Remove an employee shift by ID */
  deleteEmployeeShift(id: number): Observable<number> {
    // Simulate deleting the employee shift by returning the ID
    return of(id).pipe(
      map(() => {
        return id;
      }),
      catchError(this.handleError)
    );

    // API call to delete the employee shift
    // return this.httpClient.delete<void>(`${this.API_URL}`).pipe(
    //   map(() => {
    //     return id;
    //   }),
    //   catchError(this.handleError)
    // );
  }

  /** GET: Filter shifts by employee ID */
  getShiftsByEmployeeId(employeeId: number): Observable<EmployeeShift[]> {
    return this.httpClient
      .get<EmployeeShift[]>(`${this.API_URL}?employeeId=${employeeId}`)
      .pipe(
        map((data) => {
          return data;
        }),
        catchError(this.handleError)
      );
  }

  /** GET: Filter shifts by date range */
  getShiftsByDateRange(
    startDate: Date,
    endDate: Date
  ): Observable<EmployeeShift[]> {
    return this.httpClient
      .get<EmployeeShift[]>(
        `${
          this.API_URL
        }?startDate=${startDate.toISOString()}&endDate=${endDate.toISOString()}`
      )
      .pipe(
        map((data) => {
          return data;
        }),
        catchError(this.handleError)
      );
  }

  /** POST: Mark shift as checked in */
  checkInShift(shiftId: number, checkInTime: Date): Observable<EmployeeShift> {
    return this.httpClient
      .post<EmployeeShift>(`${this.API_URL}/checkin`, {
        shiftId,
        checkInTime: checkInTime.toISOString(),
      })
      .pipe(
        map((data) => {
          return data;
        }),
        catchError(this.handleError)
      );
  }

  /** POST: Mark shift as checked out */
  checkOutShift(
    shiftId: number,
    checkOutTime: Date
  ): Observable<EmployeeShift> {
    return this.httpClient
      .post<EmployeeShift>(`${this.API_URL}/checkout`, {
        shiftId,
        checkOutTime: checkOutTime.toISOString(),
      })
      .pipe(
        map((data) => {
          return data;
        }),
        catchError(this.handleError)
      );
  }

  /** Handle Http operation that failed */
  private handleError(error: HttpErrorResponse) {
    console.error('An error occurred:', error.message);
    return throwError(
      () => new Error('Something went wrong; please try again later.')
    );
  }
}

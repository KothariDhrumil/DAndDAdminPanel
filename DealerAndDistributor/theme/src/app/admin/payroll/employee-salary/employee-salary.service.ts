import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, of, throwError, map } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { EmployeeSalary } from './employee-salary.model';

@Injectable({
  providedIn: 'root',
})
export class EmployeeSalaryService {
  private readonly API_URL = 'assets/data/employee-salary.json';

  constructor(private httpClient: HttpClient) {}

  /** GET: Fetch all employee salaries */
  getAllEmployeeSalaries(): Observable<EmployeeSalary[]> {
    return this.httpClient
      .get<EmployeeSalary[]>(this.API_URL)
      .pipe(catchError(this.handleError));

    // API call code
    // return this.httpClient.get<EmployeeSalary[]>(this.API_URL).pipe(
    //   catchError(this.handleError)
    // );
  }

  /** POST: Add a new employee salary */
  addEmployeeSalary(
    employeeSalary: EmployeeSalary
  ): Observable<EmployeeSalary> {
    // Add the new employee salary to the data array
    return of(employeeSalary).pipe(
      map((response) => {
        return response;
      }),
      catchError(this.handleError)
    );

    // API call code
    // return this.httpClient
    //   .post<EmployeeSalary>(this.API_URL, employeeSalary)
    //   .pipe(
    //     map(() => {
    //       return employeeSalary; // Return the newly added employee salary
    //     }),
    //     catchError(this.handleError)
    //   );
  }

  /** PUT: Update an existing employee salary */
  updateEmployeeSalary(
    employeeSalary: EmployeeSalary
  ): Observable<EmployeeSalary> {
    // Update the employee salary in the data array
    return of(employeeSalary).pipe(
      map((response) => {
        return response;
      }),
      catchError(this.handleError)
    );

    // API call code
    // return this.httpClient
    //   .put<EmployeeSalary>(`${this.API_URL}`, employeeSalary) // Ensure to use the correct endpoint as per your API
    //   .pipe(
    //     map(() => {
    //       return employeeSalary; // Return the updated employee salary
    //     }),
    //     catchError(this.handleError)
    //   );
  }

  /** DELETE: Remove an employee salary by ID */
  deleteEmployeeSalary(id: number): Observable<number> {
    // Remove the employee salary by ID from the data array
    return of(id).pipe(
      map((response) => {
        return id; // Return the ID of the deleted employee salary
      }),
      catchError(this.handleError)
    );

    // API call code
    // return this.httpClient.delete<void>(`${this.API_URL}`).pipe(
    //   map(() => {
    //     return id; // Return the ID of the deleted employee salary
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

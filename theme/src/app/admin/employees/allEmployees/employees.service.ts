import { Injectable } from '@angular/core';
import { Observable, of, throwError } from 'rxjs';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { Employees } from './employees.model';

@Injectable({
  providedIn: 'root',
})
export class EmployeesService {
  private readonly API_URL = 'assets/data/employees.json';

  constructor(private httpClient: HttpClient) {}

  /** GET: Fetch all employees */
  getAllEmployees(): Observable<Employees[]> {
    return this.httpClient
      .get<Employees[]>(this.API_URL)
      .pipe(catchError(this.handleError));
  }

  /** POST: Add a new employee */
  addEmployee(employee: Employees): Observable<Employees> {
    // Simulate adding the new employee
    return of(employee).pipe(
      map((response) => {
        return response; // Return the newly added employee
      }),
      catchError(this.handleError)
    );

    // API call to add the employee
    // return this.httpClient.post<Employees>(this.API_URL, employee).pipe(
    //   map(() => {
    //     return employee; // Return the newly added employee
    //   }),
    //   catchError(this.handleError)
    // );
  }

  /** PUT: Update an existing employee */
  updateEmployee(employee: Employees): Observable<Employees> {
    // Simulate updating the employee
    return of(employee).pipe(
      map((response) => {
        return response; // Return the updated employee
      }),
      catchError(this.handleError)
    );

    // API call to update the employee
    // return this.httpClient.put<Employees>(`${this.API_URL}`, employee).pipe(
    //   map(() => {
    //     return employee; // Return the updated employee
    //   }),
    //   catchError(this.handleError)
    // );
  }

  /** DELETE: Remove an employee by ID */
  deleteEmployee(id: number): Observable<number> {
    // Simulate deleting the employee by returning the ID
    return of(id).pipe(
      map((response) => {
        return id; // Return the ID of the deleted employee
      }),
      catchError(this.handleError)
    );

    // API call to delete the employee
    // return this.httpClient.delete<void>(`${this.API_URL}`).pipe(
    //   map(() => {
    //     return id; // Return the ID of the deleted employee
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

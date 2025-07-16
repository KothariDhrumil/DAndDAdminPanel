import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of, throwError } from 'rxjs';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { Department } from './department.model';

@Injectable({
  providedIn: 'root',
})
export class DepartmentService {
  private readonly API_URL = 'assets/data/department.json';
  dataChange: BehaviorSubject<Department[]> = new BehaviorSubject<Department[]>(
    []
  );

  constructor(private httpClient: HttpClient) {}

  /** GET: Fetch all departments */
  getAllDepartments(): Observable<Department[]> {
    return this.httpClient.get<Department[]>(this.API_URL).pipe(
      map((departments) => {
        this.dataChange.next(departments); // Update the data change observable
        return departments;
      }),
      catchError(this.handleError)
    );
  }

  /** POST: Add a new department */
  addDepartment(department: Department): Observable<Department> {
    // Simulate adding the new department
    return of(department).pipe(
      map((response) => {
        return response; // Return the newly added department
      }),
      catchError(this.handleError)
    );

    // API call to add the department
    // return this.httpClient.post<Department>(this.API_URL, department).pipe(
    //   map((response) => {
    //     return department; // Return the department from the API
    //   }),
    //   catchError(this.handleError)
    // );
  }

  /** PUT: Update an existing department */
  updateDepartment(department: Department): Observable<Department> {
    // Simulate updating the department
    return of(department).pipe(
      map((response) => {
        return response; // Return the updated department
      }),
      catchError(this.handleError)
    );

    // API call to update the department
    // return this.httpClient.put<Department>(`${this.API_URL}`, department).pipe(
    //   map((response) => {
    //     return department; // Return the department from the API
    //   }),
    //   catchError(this.handleError)
    // );
  }

  /** DELETE: Remove a department by ID */
  deleteDepartment(id: number): Observable<number> {
    // Simulate deleting the department by returning the ID
    return of(id).pipe(
      map((response) => {
        return id; // Return the ID of the deleted department
      }),
      catchError(this.handleError)
    );

    // API call to delete the department
    // return this.httpClient.delete<void>(`${this.API_URL}`).pipe(
    //   map(() => {
    //     return id; // Return the ID of the deleted department
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

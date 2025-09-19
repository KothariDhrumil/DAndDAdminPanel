import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of, throwError } from 'rxjs';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { Leads } from './leads.model';

@Injectable({
  providedIn: 'root',
})
export class LeadsService {
  private readonly API_URL = 'assets/data/leads.json';
  dataChange: BehaviorSubject<Leads[]> = new BehaviorSubject<Leads[]>([]);

  constructor(private httpClient: HttpClient) {}

  /** GET: Fetch all leads */
  getAllLeads(): Observable<Leads[]> {
    return this.httpClient
      .get<Leads[]>(this.API_URL)
      .pipe(catchError(this.handleError));
  }

  /** POST: Add a new lead */
  addLeads(leads: Leads): Observable<Leads> {
    // Add the new lead to the data array
    return of(leads).pipe(
      map((response) => {
        return response; // Return the newly added lead
      }),
      catchError(this.handleError)
    );

    // API call to add the lead
    // return this.httpClient.post<Leads>(this.API_URL, leads).pipe(
    //   map((response) => {
    //     return response; // Return the newly added lead
    //   }),
    //   catchError(this.handleError)
    // );
  }

  /** PUT: Update an existing lead */
  updateLeads(leads: Leads): Observable<Leads> {
    // Update the lead in the data array
    return of(leads).pipe(
      map((response) => {
        return response; // Return the updated lead
      }),
      catchError(this.handleError)
    );

    // API call to update the lead
    // return this.httpClient.put<Leads>(`${this.API_URL}`, leads).pipe(
    //   map((response) => {
    //     return response; // Return the updated lead
    //   }),
    //   catchError(this.handleError)
    // );
  }

  /** DELETE: Remove a lead by ID */
  deleteLeads(id: number): Observable<number> {
    // Remove the lead in the data array
    return of(id).pipe(
      map((response) => {
        return id; // Return the ID of the deleted lead
      }),
      catchError(this.handleError)
    );

    // API call to delete the lead
    // return this.httpClient.delete<void>(`${this.API_URL}`).pipe(
    //   map((response) => {
    //     return id; // Return the ID of the deleted lead
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

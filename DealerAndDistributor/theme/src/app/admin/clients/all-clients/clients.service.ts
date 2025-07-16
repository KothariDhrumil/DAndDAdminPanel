import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of, throwError } from 'rxjs';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { Clients } from './clients.model';

@Injectable({
  providedIn: 'root',
})
export class ClientsService {
  private readonly API_URL = 'assets/data/clients.json';
  private dataChange: BehaviorSubject<Clients[]> = new BehaviorSubject<
    Clients[]
  >([]);

  constructor(private httpClient: HttpClient) {}

  /** GET: Fetch all clients */
  getAllClients(): Observable<Clients[]> {
    return this.httpClient.get<Clients[]>(this.API_URL).pipe(
      map((clients) => {
        this.dataChange.next(clients); // Update the data change observable
        return clients;
      }),
      catchError(this.handleError)
    );
  }

  /** POST: Add a new client */
  addClient(client: Clients): Observable<Clients> {
    // Simulate adding a new client
    return of(client).pipe(
      map((response) => {
        return response; // Return the newly added client
      }),
      catchError(this.handleError)
    );

    // API call to add a client
    // return this.httpClient.post<Clients>(this.API_URL, client).pipe(
    //   map(() => {
    //     return client; // Return the newly added client
    //   }),
    //   catchError(this.handleError)
    // );
  }

  /** PUT: Update an existing client */
  updateClient(client: Clients): Observable<Clients> {
    // Simulate updating a client
    return of(client).pipe(
      map((response) => {
        return response; // Return the updated client
      }),
      catchError(this.handleError)
    );

    // API call to update the client
    // return this.httpClient.put<Clients>(`${this.API_URL}`, client).pipe(
    //   map(() => {
    //     return client; // Return the updated client
    //   }),
    //   catchError(this.handleError)
    // );
  }

  /** DELETE: Remove a client by ID */
  deleteClient(id: number): Observable<number> {
    // Simulate deleting a client
    return of(id).pipe(
      map((response) => {
        return id; // Return the ID of the deleted client
      }),
      catchError(this.handleError)
    );

    // API call to delete a client
    // return this.httpClient.delete<void>(`${this.API_URL}`).pipe(
    //   map(() => {
    //     return id; // Return the ID of the deleted client
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

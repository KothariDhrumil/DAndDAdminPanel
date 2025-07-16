import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of, throwError } from 'rxjs';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { Trainers } from './trainers.model';

@Injectable({
  providedIn: 'root',
})
export class TrainersService {
  private readonly API_URL = 'assets/data/trainers.json';

  dataChange: BehaviorSubject<Trainers[]> = new BehaviorSubject<Trainers[]>([]);
  dialogData!: Trainers;

  constructor(private httpClient: HttpClient) {}

  get data(): Trainers[] {
    return this.dataChange.value;
  }

  getDialogData(): Trainers {
    return this.dialogData;
  }

  /** CRUD METHODS */

  /** GET: Fetch trainers */
  getTrainers(): Observable<Trainers[]> {
    return this.httpClient.get<Trainers[]>(this.API_URL).pipe(
      map((data) => {
        this.dataChange.next(data);
        return data;
      }),
      catchError(this.handleError)
    );
  }

  /** POST: Add a new trainer */
  addTrainer(trainer: Trainers): Observable<Trainers> {
    // Add the new trainer to the data array
    return of(trainer).pipe(
      map((response) => {
        this.dialogData = trainer;
        return response;
      }),
      catchError(this.handleError)
    );

    // API call to add the trainer
    // return this.httpClient.post<Trainers>(this.API_URL, trainer).pipe(
    //   map(() => {
    //     this.dialogData = trainer;
    //     return trainer;
    //   }),
    //   catchError(this.handleError)
    // );
  }

  /** PUT: Update an existing trainer */
  updateTrainer(trainer: Trainers): Observable<Trainers> {
    // Update the trainer in the data array
    return of(trainer).pipe(
      map((response) => {
        this.dialogData = trainer;
        return response;
      }),
      catchError(this.handleError)
    );

    // API call to update the trainer
    // return this.httpClient.put<Trainers>(`${this.API_URL}`, trainer).pipe(
    //   map(() => {
    //     this.dialogData = trainer;
    //     return trainer;
    //   }),
    //   catchError(this.handleError)
    // );
  }

  /** DELETE: Remove a trainer by ID */
  deleteTrainer(id: number): Observable<number> {
    // Simulate deletion of the trainer
    return of(id).pipe(
      map((response) => {
        return id; // return the ID of the deleted trainer
      }),
      catchError(this.handleError)
    );

    // API call to delete the trainer
    // return this.httpClient.delete<void>(`${this.API_URL}`).pipe(
    //   map(() => {
    //     return id;
    //   }),
    //   catchError(this.handleError)
    // );
  }

  /** GET: Search trainers by name */
  searchTrainersByName(name: string): Observable<Trainers[]> {
    // Simulate a search operation
    return of([]).pipe(
      map(() => {
        return []; // return empty list (simulate search)
      }),
      catchError(this.handleError)
    );

    // API call to search trainers by name
    // return this.httpClient.get<Trainers[]>(`${this.API_URL}?name=${name}`).pipe(
    //   map((data) => {
    //     return data;
    //   }),
    //   catchError(this.handleError)
    // );
  }

  /** GET: Filter trainers by specialization */
  getTrainersBySpecialization(specialization: string): Observable<Trainers[]> {
    // Simulate fetching filtered trainers
    return of([]).pipe(
      map(() => {
        return []; // return an empty list (simulate filtering)
      }),
      catchError(this.handleError)
    );

    // API call to filter trainers by specialization
    // return this.httpClient
    //   .get<Trainers[]>(`${this.API_URL}?specialization=${specialization}`)
    //   .pipe(
    //     map((data) => {
    //       return data;
    //     }),
    //     catchError(this.handleError)
    //   );
  }

  /** GET: Get available trainers */
  getAvailableTrainers(startDate: Date, endDate: Date): Observable<Trainers[]> {
    // Simulate fetching available trainers based on dates
    return of([]).pipe(
      map(() => {
        return []; // return empty list (simulate availability)
      }),
      catchError(this.handleError)
    );

    // API call to get available trainers
    // return this.httpClient
    //   .get<Trainers[]>(
    //     `${this.API_URL}?availability_startDate_lte=${startDate.toISOString()}&availability_endDate_gte=${endDate.toISOString()}`
    //   )
    //   .pipe(
    //     map((data) => {
    //       return data;
    //     }),
    //     catchError(this.handleError)
    //   );
  }

  /** GET: Get trainer details by ID */
  getTrainerById(id: number): Observable<Trainers> {
    // Simulate fetching a trainer by ID
    return of({} as Trainers).pipe(
      map(() => {
        return {} as Trainers; // return an empty trainer object
      }),
      catchError(this.handleError)
    );

    // API call to get trainer by ID
    // return this.httpClient.get<Trainers>(`${this.API_URL}/${id}`).pipe(
    //   map((data) => {
    //     return data;
    //   }),
    //   catchError(this.handleError)
    // );
  }

  /** POST: Assign trainer to a training */
  assignTrainerToTraining(
    trainerId: number,
    trainingId: number
  ): Observable<any> {
    // Simulate assigning trainer to a training
    return of({ trainerId, trainingId }).pipe(
      map((data) => {
        return data; // return assignment info
      }),
      catchError(this.handleError)
    );

    // API call to assign trainer to training
    // return this.httpClient
    //   .post<any>(`${this.API_URL}/assign`, {
    //     trainerId,
    //     trainingId,
    //   })
    //   .pipe(
    //     map((data) => {
    //       return data;
    //     }),
    //     catchError(this.handleError)
    //   );
  }

  /** Handle Http operation that failed */
  private handleError(error: HttpErrorResponse): Observable<never> {
    console.error('An error occurred:', error.message);
    return throwError(
      () => new Error('Something went wrong; please try again later.')
    );
  }
}

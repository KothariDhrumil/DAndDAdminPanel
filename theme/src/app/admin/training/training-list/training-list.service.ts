import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of, throwError } from 'rxjs';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { TrainingList } from './training-list.model';

export interface Employee {
  name: string;
  avatar: string;
}

@Injectable({
  providedIn: 'root',
})
export class TrainingListService {
  private readonly API_URL = 'assets/data/training-list.json';

  dataChange: BehaviorSubject<TrainingList[]> = new BehaviorSubject<
    TrainingList[]
  >([]);
  dialogData!: TrainingList;

  constructor(private httpClient: HttpClient) {}

  get data(): TrainingList[] {
    return this.dataChange.value;
  }

  getDialogData(): TrainingList {
    return this.dialogData;
  }

  /** CRUD METHODS */

  /** GET: Fetch training lists */
  getTrainingLists(): Observable<TrainingList[]> {
    // Mock API data
    return this.httpClient.get<TrainingList[]>(this.API_URL).pipe(
      map((data) => {
        this.dataChange.next(data);
        return data;
      }),
      catchError(this.handleError)
    );

    // Local mock response
    // return of([{ id: 1, name: 'Training A' }, { id: 2, name: 'Training B' }]);
  }

  /** POST: Add a new training list */
  addTrainingList(trainingList: TrainingList): Observable<TrainingList> {
    // Local mock response
    return of(trainingList).pipe(
      map(() => {
        this.dialogData = trainingList;
        return trainingList;
      }),
      catchError(this.handleError)
    );

    // API call to add training list
    // return this.httpClient.post<TrainingList>(this.API_URL, trainingList).pipe(
    //   map(() => {
    //     this.dialogData = trainingList;
    //     return trainingList;
    //   }),
    //   catchError(this.handleError)
    // );
  }

  /** PUT: Update an existing training list */
  updateTrainingList(trainingList: TrainingList): Observable<TrainingList> {
    // Local mock response
    return of(trainingList).pipe(
      map(() => {
        this.dialogData = trainingList;
        return trainingList;
      }),
      catchError(this.handleError)
    );

    // API call to update training list
    // return this.httpClient.put<TrainingList>(`${this.API_URL}`, trainingList).pipe(
    //   map(() => {
    //     this.dialogData = trainingList;
    //     return trainingList;
    //   }),
    //   catchError(this.handleError)
    // );
  }

  /** DELETE: Remove a training list by ID */
  deleteTrainingList(id: number): Observable<number> {
    // Local mock response
    return of(id).pipe(
      map(() => {
        return id;
      }),
      catchError(this.handleError)
    );

    // API call to delete training list
    // return this.httpClient.delete<void>(`${this.API_URL}`).pipe(
    //   map(() => {
    //     return id;
    //   }),
    //   catchError(this.handleError)
    // );
  }

  /** GET: Filter training lists by status */
  getTrainingListsByStatus(status: string): Observable<TrainingList[]> {
    // Local mock response
    return of([]).pipe(
      map((data) => {
        return data;
      }),
      catchError(this.handleError)
    );

    // API call to filter by status
    // return this.httpClient.get<TrainingList[]>(`${this.API_URL}?status=${status}`).pipe(
    //   map((data) => {
    //     return data;
    //   }),
    //   catchError(this.handleError)
    // );
  }

  /** GET: Filter training lists by date range */
  getTrainingListsByDateRange(
    startDate: Date,
    endDate: Date
  ): Observable<TrainingList[]> {
    // Local mock response
    return of([]).pipe(
      map((data) => {
        return data;
      }),
      catchError(this.handleError)
    );

    // API call to filter by date range
    // return this.httpClient.get<TrainingList[]>(`${this.API_URL}?startDate=${startDate.toISOString()}&endDate=${endDate.toISOString()}`).pipe(
    //   map((data) => {
    //     return data;
    //   }),
    //   catchError(this.handleError)
    // );
  }

  /** GET: Search training lists by trainer name */
  searchTrainingListsByTrainerName(
    trainerName: string
  ): Observable<TrainingList[]> {
    // Local mock response
    return of([]).pipe(
      map((data) => {
        return data;
      }),
      catchError(this.handleError)
    );

    // API call to search by trainer name
    // return this.httpClient.get<TrainingList[]>(`${this.API_URL}?trainerName=${trainerName}`).pipe(
    //   map((data) => {
    //     return data;
    //   }),
    //   catchError(this.handleError)
    // );
  }

  /** GET: Get upcoming training lists */
  getUpcomingTrainingLists(): Observable<TrainingList[]> {
    const today = new Date().toISOString();
    // Local mock response
    return of([]).pipe(
      map((data) => {
        return data;
      }),
      catchError(this.handleError)
    );

    // API call to get upcoming training lists
    // return this.httpClient.get<TrainingList[]>(`${this.API_URL}?startDate_gte=${today}`).pipe(
    //   map((data) => {
    //     return data;
    //   }),
    //   catchError(this.handleError)
    // );
  }

  /** POST: Register for a training list */
  registerForTraining(trainingId: number, employeeId: number): Observable<any> {
    // Local mock response
    return of({ success: true }).pipe(
      map((data) => {
        return data;
      }),
      catchError(this.handleError)
    );

    // API call to register
    // return this.httpClient.post<any>(`${this.API_URL}/register`, { trainingId, employeeId }).pipe(
    //   map((data) => {
    //     return data;
    //   }),
    //   catchError(this.handleError)
    // );
  }

  // Method to fetch all employees from the JSON file
  getEmployees(): Observable<Employee[]> {
    return this.httpClient.get<TrainingList[]>(this.API_URL).pipe(
      map((trainings) => {
        let employees: Employee[] = [];
        trainings.forEach((training) => {
          employees = [...employees, ...training.employee];
        });
        return employees;
      })
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

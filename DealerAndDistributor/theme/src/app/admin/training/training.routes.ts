import { Route } from '@angular/router';
import { Page404Component } from 'app/authentication/page404/page404.component';
import { TrainingListComponent } from './training-list/training-list.component';
import { TrainersComponent } from './trainers/trainers.component';
import { TrainingTypeComponent } from './training-type/training-type.component';

export const TRAINING_ROUTE: Route[] = [
  {
    path: 'training-list',
    component: TrainingListComponent,
  },
  {
    path: 'trainers',
    component: TrainersComponent,
  },
  {
    path: 'training-type',
    component: TrainingTypeComponent,
  },
  { path: '**', component: Page404Component },
];

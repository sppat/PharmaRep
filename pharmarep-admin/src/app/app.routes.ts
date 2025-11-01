import { Routes } from '@angular/router';
import {UsersList} from './users-list/users-list';

export const routes: Routes = [
  {
    path: 'users',
    component: UsersList
  }
];

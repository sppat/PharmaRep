import {Routes} from '@angular/router';
import {UsersList} from './users-list/users-list';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: '/users',
  },
  {
    path: 'users',
    component: UsersList
  }
];

import { Routes } from '@angular/router';
import { UserListComponent } from './user-list/user-list.component';
import { EventListComponent } from './event-list/event-list.component';
import { UserComponent } from './user-list/user/user.component';

export const routes: Routes = [
  { path: 'users', component: UserListComponent },
  { path: 'users/:id', component: UserComponent },
  { path: 'events', component: EventListComponent },
];

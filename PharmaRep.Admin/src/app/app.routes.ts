import { Routes } from '@angular/router';
import { UserListComponent } from './user-list/user-list.component';
import { EventListComponent } from './event-list/event-list.component';

export const routes: Routes = [
  { path: 'users', component: UserListComponent },
  { path: 'events', component: EventListComponent },
];

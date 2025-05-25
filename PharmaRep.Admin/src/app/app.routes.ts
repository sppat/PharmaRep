import { Routes } from '@angular/router';
import { UserListComponent } from './user-list/user-list.component';
import { EventListComponent } from './event-list/event-list.component';
import { UserFormComponent } from './user-list/user-form/user-form.component';

export const routes: Routes = [
  { path: 'users', component: UserListComponent },
  { path: 'users/:id', component: UserFormComponent },
  { path: 'events', component: EventListComponent },
];

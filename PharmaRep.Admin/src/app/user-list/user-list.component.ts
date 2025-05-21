import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { User } from '../models/user';
import { MatIconModule } from '@angular/material/icon';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-user-list',
  imports: [CommonModule, MatIconModule, RouterModule],
  templateUrl: './user-list.component.html',
  styleUrl: './user-list.component.css',
})
export class UserListComponent {
  users: User[] = [
    {
      id: 1,
      firstName: 'John',
      lastName: 'Doe',
      email: 'john@doe.com',
      roles: [{ id: 2, name: 'Doctor' }],
    },
    {
      id: 2,
      firstName: 'John',
      lastName: 'Foo',
      email: 'john@foo.com',
      roles: [{ id: 2, name: 'Medical Representative' }],
    },
  ];
}

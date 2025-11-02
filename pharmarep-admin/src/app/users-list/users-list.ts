import {Component} from '@angular/core';

@Component({
  selector: 'app-users-list',
  imports: [],
  templateUrl: './users-list.html',
  styleUrl: './users-list.css',
})
export class UsersList {
  users = [
    {
      "firstName": "Alice",
      "lastName": "Johnson",
      "email": "alice@johnson.com"
    },
    {
      "firstName": "Bob",
      "lastName": "Smith",
      "email": "bob@smith.com"
    },
    {
      "firstName": "Charlie",
      "lastName": "Brown",
      "email": "charlie@brown.com"
    },
    {
      "firstName": "Diana",
      "lastName": "Miller",
      "email": "diana@miller.com"
    },
    {
      "firstName": "Ethan",
      "lastName": "Davis",
      "email": "ethan@davis.com"
    },
    {
      "firstName": "Fiona",
      "lastName": "Wilson",
      "email": "fiona@wilson.com"
    },
    {
      "firstName": "George",
      "lastName": "Taylor",
      "email": "george@taylor.com"
    },
    {
      "firstName": "Hannah",
      "lastName": "Moore",
      "email": "hannah@moore.com"
    },
    {
      "firstName": "Ian",
      "lastName": "Anderson",
      "email": "ian@anderson.com"
    },
    {
      "firstName": "Julia",
      "lastName": "Thomas",
      "email": "julia@thomas.com"
    }
  ];
}

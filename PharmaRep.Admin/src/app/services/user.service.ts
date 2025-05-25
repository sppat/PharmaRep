import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment.development';
import PaginatedResponse from '../models/paginatedResponse';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private readonly usersApiUrl =
    environment.pharmaRepApiUrl + '/identity/users';

  constructor(private http: HttpClient) {}

  getUsers(): Observable<PaginatedResponse<User>> {
    let headers = {
      Authorization:
        'Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIwYzhkNWMxZi1iODk3LTQ4YTEtYjlmZC00NWI2M2QzMjlkMjYiLCJlbWFpbCI6InMucGF0ZXJha2lzQG91dGxvb2suY29tLmdyIiwicm9sZSI6IkFkbWluIiwibmJmIjoxNzQ4MTY3MTQ3LCJleHAiOjE3NDg1OTkxNDcsImlhdCI6MTc0ODE2NzE0NywiaXNzIjoiUGhhcm1hUmVwIiwiYXVkIjoiUGhhcm1hUmVwIn0.kA7TRAm0pMdsVkvpftOxQZa7DFvMkomWL2h6vzEUJQr04f8TGXn-Ry33U6mYhfo238OcxI-SI0c84opY6A7jBA',
    };
    return this.http.get<PaginatedResponse<User>>(this.usersApiUrl, {
      headers: headers,
    });
  }
}

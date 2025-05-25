import { Component } from '@angular/core';
import { User } from '../../models/user';

@Component({
  selector: 'app-user',
  imports: [],
  templateUrl: './user-form.component.html',
  styleUrl: './user-form.component.css'
})
export class UserFormComponent {
  private user: User | undefined;
}

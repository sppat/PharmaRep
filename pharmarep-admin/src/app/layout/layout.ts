import { Component } from '@angular/core';
import {NavBar} from '../nav-bar/nav-bar';

@Component({
  selector: 'app-layout',
  imports: [
    NavBar
  ],
  templateUrl: './layout.html',
  styleUrl: './layout.css',
})
export class Layout {

}

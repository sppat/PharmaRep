import {Component} from '@angular/core';
import {NavBar} from '../nav-bar/nav-bar';
import {RouterOutlet} from '@angular/router';

@Component({
  selector: 'app-layout',
  imports: [
    NavBar,
    RouterOutlet,
    RouterOutlet
  ],
  templateUrl: './layout.html',
  styleUrl: './layout.css',
})
export class Layout {

}

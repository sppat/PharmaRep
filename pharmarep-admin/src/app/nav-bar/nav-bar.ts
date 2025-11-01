import { Component } from '@angular/core';
import {NavBarItem} from './nav-bar-item/nav-bar-item';

@Component({
  selector: 'app-nav-bar',
  imports: [
    NavBarItem
  ],
  templateUrl: './nav-bar.html',
  styleUrl: './nav-bar.css',
})
export class NavBar {

}

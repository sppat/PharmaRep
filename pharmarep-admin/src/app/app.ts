import { Component, signal } from '@angular/core';
import {Layout} from './layout/layout';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  styleUrl: './app.css',
  imports: [
    Layout
  ]
})
export class App {}

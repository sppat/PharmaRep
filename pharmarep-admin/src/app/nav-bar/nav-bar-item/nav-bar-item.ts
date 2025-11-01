import {Component, Input} from '@angular/core';

@Component({
  selector: 'app-nav-bar-item',
  imports: [],
  templateUrl: './nav-bar-item.html',
  styleUrl: './nav-bar-item.css',
})
export class NavBarItem {
  @Input({ required: true }) image!: string;
  @Input({ required: true }) alt!: string;

  get imagePath() {
    return "/menuIcons/" + this.image;
  }
}

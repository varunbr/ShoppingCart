import { Component, OnInit } from '@angular/core';
import { UtilityService } from '../services/utility.service';

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  styleUrls: ['./about.component.css'],
})
export class AboutComponent implements OnInit {
  step = 0;

  constructor(utility: UtilityService) {
    utility.setTitle('About');
  }

  ngOnInit(): void {}
}

import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-spinner',
  templateUrl: './spinner.component.html',
  styleUrls: ['./spinner.component.css'],
})
export class SpinnerComponent implements OnInit {
  @Input() diameter: number = 100;
  @Input() overlay: boolean = true;
  @Input() color: string = 'accent';
  constructor() {}

  ngOnInit(): void {}
}

import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-single-select',
  templateUrl: './single-select.component.html',
  styleUrls: ['./single-select.component.css'],
})
export class SingleSelectComponent implements OnInit {
  @Input() name: string = '';
  @Input() values: { name: string; value: string }[];
  @Input() value: string;
  @Output() valueChange: EventEmitter<string> = new EventEmitter<string>();
  constructor() {}

  ngOnInit(): void {}

  onChange = () => {
    this.valueChange.emit(this.value);
  };
}

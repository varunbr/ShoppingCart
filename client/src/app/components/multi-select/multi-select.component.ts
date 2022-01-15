import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-multi-select',
  templateUrl: './multi-select.component.html',
  styleUrls: ['./multi-select.component.css'],
})
export class MultiSelectComponent implements OnInit {
  @Input() name: string = '';
  @Input() values: string = '';
  _value: string[];
  get value(): string {
    return this._value?.join(',');
  }
  @Input() set value(inpValue: string) {
    this._value = inpValue?.split(',');
  }
  @Output() valueChange: EventEmitter<string> = new EventEmitter<string>();
  constructor() {}

  ngOnInit(): void {}

  onChange = () => {
    this.valueChange.emit(this._value.join(','));
  };
}

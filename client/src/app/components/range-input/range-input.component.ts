import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-range-input',
  templateUrl: './range-input.component.html',
  styleUrls: ['./range-input.component.css'],
})
export class RangeInputComponent implements OnInit {
  @Input() prefix: string = '';
  @Input() postfix: string = '';
  @Input() name: string = '';
  @Input() range: number[];
  _value: string;
  get value(): string {
    return this._value;
  }
  @Input() set value(inpValue: string) {
    this._value = inpValue;
    let split = inpValue?.split('-');
    this.from = split?.[0] ? parseInt(split[0]) : undefined;
    this.to = split?.[1] ? parseInt(split[1]) : undefined;
  }
  @Output() valueChange: EventEmitter<string> = new EventEmitter<string>();
  from: number;
  to: number;
  values: SelectItem[] = [];
  constructor() {}

  ngOnInit(): void {
    if (this.range) {
      this.values = this.range.map(
        (e) => new SelectItem(e, this.prefix, this.postfix)
      );
    }
    if (this.value) {
      let split = this.value.split('-');
      if (split[0]) this.from = parseInt(split[0]);
      if (split[1]) this.to = parseInt(split[1]);
    }

    this.updateFlag();
  }

  onChange = () => {
    if (this.from == null && this.to == null) {
      this._value = '';
    } else {
      this._value = `${this.from == null ? '' : this.from}-${
        this.to == null ? '' : this.to
      }`.trim();
    }
    this.valueChange.emit(this._value);
    this.updateFlag();
  };

  updateFlag() {
    this.values.forEach((e) => {
      if (e.value < this.from) {
        e.flag = -1;
      } else if (e.value > this.to) {
        e.flag = 1;
      } else {
        e.flag = 0;
      }
    });
  }
}
class SelectItem {
  name: string;
  value: number;
  flag: number;

  constructor(value: number, prefix: string, postfix: string) {
    this.name = `${prefix} ${value} ${postfix}`.trim();
    this.value = value;
    this.flag = 0;
  }
}

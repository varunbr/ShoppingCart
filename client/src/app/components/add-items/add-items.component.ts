import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { MatChipInputEvent } from '@angular/material/chips';

@Component({
  selector: 'app-add-items',
  templateUrl: './add-items.component.html',
  styleUrls: ['./add-items.component.css'],
})
export class AddItemsComponent implements OnInit {
  @Input() name: string = '';

  values: string[] = [];
  get value(): string {
    return this.values?.join(',');
  }
  @Input() set value(inpValue: string) {
    if (inpValue) this.values = inpValue?.split(',');
    else this, (this.values = []);
  }
  @Output() valueChange: EventEmitter<string> = new EventEmitter<string>();
  readonly separatorKeysCodes = [ENTER, COMMA] as const;

  constructor() {}

  ngOnInit(): void {}

  add(event: MatChipInputEvent) {
    const value = (event.value || '').trim();
    if (value) {
      this.values.push(value);
    }
    event.chipInput!.clear();
    this.valueChange.emit(this.values?.join(','));
  }

  remove(index: number) {
    this.values.splice(index, 1);
    this.valueChange.emit(this.values?.join(','));
  }
}

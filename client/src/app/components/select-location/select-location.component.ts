import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MatChipInputEvent } from '@angular/material/chips';

@Component({
  selector: 'app-select-location',
  templateUrl: './select-location.component.html',
  styleUrls: ['./select-location.component.css'],
})
export class SelectLocationComponent implements OnInit {
  get value(): string {
    return this.getLocations();
  }
  @Input() set value(inpValue: string) {
    this.setLocations(inpValue);
  }
  @Output() valueChange: EventEmitter<string> = new EventEmitter<string>();
  locations: { name: string; type: string }[] = [];
  locationType = 'Area';
  readonly separatorKeysCodes = [ENTER, COMMA] as const;

  constructor() {}

  ngOnInit(): void {}

  setLocations(values: string) {
    this.locations = [];
    if (!values) return;
    for (let value of values.split(',')) {
      let items = value.split('-');
      this.locations = [...this.locations, { name: items[0], type: items[1] }];
    }
  }

  getLocations() {
    let values: string[] = [];
    for (let item of this.locations) {
      values = [...values, `${item.name.trim()}-${item.type}`];
    }
    return values.join(',');
  }

  addLocation(event: MatChipInputEvent) {
    const value = (event.value || '').trim();
    if (value) {
      this.locations = [
        ...this.locations,
        { name: value, type: this.locationType },
      ];
      this.onChange();
    }
    event.chipInput!.clear();
  }

  removeLocation(index: number) {
    this.locations.splice(index, 1);
    this.onChange();
  }

  onChange = () => {
    this.valueChange.emit(this.value);
  };
}

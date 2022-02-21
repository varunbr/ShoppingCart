import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

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
  locationName: string;
  locationType = 'Area';

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

  addLocation() {
    if (this.locationName && this.locationType) {
      this.locations = [
        ...this.locations,
        { name: this.locationName, type: this.locationType },
      ];
      this.locationName = '';
      this.locationType = 'Area';
      this.onChange();
    }
  }

  removeLocation(index: number) {
    this.locations.splice(index, 1);
    this.onChange();
  }

  onChange = () => {
    this.valueChange.emit(this.value);
  };
}

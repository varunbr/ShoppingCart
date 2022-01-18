import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.css'],
})
export class PaginationComponent implements OnInit {
  selected: number;
  range: number[] = [];

  @Input() listSize: number = 5;
  _totalPages: number;
  @Input() set totalPages(value: number) {
    this._totalPages = value;
    this.setPagination();
  }
  get page(): number {
    return this.selected;
  }
  @Input() set page(value: number) {
    this.selected = value;
    this.setPagination();
  }
  @Output() pageChange: EventEmitter<number> = new EventEmitter<number>();

  constructor() {}

  ngOnInit(): void {}

  getRange = (): number[] => {
    if (!this._totalPages) return [this.selected];

    if (this._totalPages <= this.listSize) {
      return Array.from({ length: this._totalPages }, (_, i) => i + 1);
    }
    if (this.selected - Math.floor(this.listSize / 2) <= 0) {
      return Array.from({ length: this.listSize }, (_, i) => i + 1);
    } else if (
      this.selected + Math.floor(this.listSize / 2) >
      this._totalPages
    ) {
      let start = this._totalPages - this.listSize;
      return Array.from({ length: this.listSize }, (_, i) => i + start + 1);
    }
    let start = this.selected - Math.floor(this.listSize / 2);
    return Array.from({ length: this.listSize }, (_, i) => i + start);
  };

  setPagination() {
    this.range = this.getRange();
  }

  onClick(page: number) {
    this.selected = page;
    this.setPagination();
    this.pageChange.emit(page);
  }
}

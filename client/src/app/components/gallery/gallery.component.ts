import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';

@Component({
  selector: 'app-gallery',
  templateUrl: './gallery.component.html',
  styleUrls: ['./gallery.component.css'],
})
export class GalleryComponent implements OnInit {
  selected: number;
  _urls: string[];
  get urls() {
    return this._urls;
  }
  @Input() set urls(value: string[]) {
    this._urls = value;
    this.selected = 0;
  }
  currentX = 0;
  start: boolean;
  end: boolean;

  constructor() {}

  ngOnInit(): void {}

  @ViewChild('content', { static: true }) content: ElementRef<HTMLElement>;

  onPan(event) {
    this.scrollToTarget(this.currentX - event.deltaX);
  }

  onPanEnd() {
    this.currentX = this.content.nativeElement.scrollLeft;
  }

  scrollToTarget(target: number, scrollSmooth = false) {
    this.content.nativeElement.scrollTo({
      left: target,
      behavior: scrollSmooth ? 'smooth' : 'auto',
    });
  }

  scrollToSelected() {
    let element = this.content.nativeElement;
    let boxLength = element.scrollWidth / this.urls.length;
    let target =
      boxLength * 0.5 + this.selected * boxLength - element.clientWidth * 0.5;
    this.scrollToTarget(target, true);
    this.currentX = target;
  }

  selectImage(index: number) {
    if (index >= 0 && index < this.urls.length) {
      this.selected = index;
      this.scrollToSelected();
    }
  }
}

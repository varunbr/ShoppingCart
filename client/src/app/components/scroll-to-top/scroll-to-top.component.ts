import { Component, HostListener, OnInit } from '@angular/core';

@Component({
  selector: 'app-scroll-to-top',
  templateUrl: './scroll-to-top.component.html',
  styleUrls: ['./scroll-to-top.component.css'],
})
export class ScrollToTopComponent implements OnInit {
  toScroll: boolean;
  topPosToStartShowing = 500;
  current = 0;
  scrollTimeOut = setTimeout(() => {}, 0);
  scrollStop = setTimeout(() => {}, 0);
  mouseIn = false;

  constructor() {}

  ngOnInit(): void {}

  @HostListener('window:scroll')
  checkScroll() {
    const scrollPosition =
      window.pageYOffset ||
      document.documentElement.scrollTop ||
      document.body.scrollTop ||
      0;

    this.scrollTimeOut = setTimeout(() => {
      this.current = scrollPosition;
    }, 1500);

    if (
      scrollPosition >= this.topPosToStartShowing &&
      Math.abs(this.current - scrollPosition) > 300
    ) {
      this.toScroll = true;
      clearTimeout(this.scrollStop);
      this.scrollStop = setTimeout(() => {
        if (!this.mouseIn) this.toScroll = false;
        this.current = scrollPosition;
      }, 2500);
    } else if (scrollPosition < this.topPosToStartShowing) {
      this.toScroll = false;
    }
  }

  onMouseIn() {
    this.mouseIn = true;
    clearTimeout(this.scrollStop);
  }

  onMouseOut() {
    this.mouseIn = false;
    const scrollPosition =
      window.pageYOffset ||
      document.documentElement.scrollTop ||
      document.body.scrollTop ||
      0;
    this.scrollStop = setTimeout(() => {
      this.toScroll = false;
      this.current = scrollPosition;
    }, 2500);
  }

  goToTop() {
    window.scroll({
      top: 0,
      left: 0,
      behavior: 'smooth',
    });
  }
}

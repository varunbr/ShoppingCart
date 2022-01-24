import {
  Component,
  ElementRef,
  Input,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';
import { Subscription } from 'rxjs';
import { MediaService } from 'src/app/services/media.service';

@Component({
  selector: 'app-scroll-content',
  templateUrl: './scroll-content.component.html',
  styleUrls: ['./scroll-content.component.css'],
})
export class ScrollContentComponent implements OnInit, OnDestroy {
  @Input() title: string;
  @Input() viewMoreLink: string;
  @Input() viewMoreParams: string;
  currentX = 0;
  start: boolean;
  end: boolean;
  subscription: Subscription;
  constructor(private media: MediaService) {}

  @ViewChild('content', { static: true }) content: ElementRef<HTMLElement>;
  ngOnInit(): void {
    this.subscription = this.media.mediaChange$.subscribe((response) => {
      this.setArrows(this.currentX);
    });
  }
  ngOnDestroy(): void {
    this.subscription?.unsubscribe();
  }

  onPan(event) {
    this.scrollToTarget(this.currentX - event.deltaX);
  }

  onPanEnd() {
    this.currentX = this.content.nativeElement.scrollLeft;
  }

  scroll(right = true): void {
    let element = this.content.nativeElement;
    let target = right
      ? element.scrollLeft + element.offsetWidth * 0.8
      : element.scrollLeft - element.offsetWidth * 0.8;
    this.scrollToTarget(target, true);
    this.currentX = target;
  }

  scrollToTarget(target: number, scrollSmooth = false) {
    this.setArrows(target);
    this.content.nativeElement.scrollTo({
      left: target,
      behavior: scrollSmooth ? 'smooth' : 'auto',
    });
  }

  setArrows(target: number) {
    let element = this.content.nativeElement;
    this.start = element.clientWidth === element.scrollWidth || target <= 0;
    this.end =
      element.clientWidth === element.scrollWidth ||
      target >= element.scrollWidth - element.clientWidth;
  }
}

import { Component, Input, OnInit } from '@angular/core';
import {
  NgxGalleryAnimation,
  NgxGalleryImage,
  NgxGalleryImageSize,
  NgxGalleryOptions,
} from '@kolkov/ngx-gallery';

@Component({
  selector: 'app-gallery',
  templateUrl: './gallery.component.html',
  styleUrls: ['./gallery.component.css'],
})
export class GalleryComponent implements OnInit {
  @Input() urls: NgxGalleryImage[];
  galleryOptions: NgxGalleryOptions[];

  constructor() {}

  ngOnInit(): void {
    this.setGalleryOptions();
  }

  setGalleryOptions() {
    this.galleryOptions = [
      {
        width: '100%',
        height: 'calc(100vh - 190px)',
        arrowPrevIcon: 'fa fa-chevron-left',
        arrowNextIcon: 'fa fa-chevron-right',
        imageAnimation: NgxGalleryAnimation.Fade,
        imageSwipe: true,
        imageSize: NgxGalleryImageSize.Contain,
        thumbnailsColumns: 4,
        thumbnailsSwipe: true,
        thumbnailSize: NgxGalleryImageSize.Contain,
        thumbnailsArrowsAutoHide: true,
        previewSwipe: true,
        previewCloseOnClick: true,
      },
      //md
      {
        breakpoint: 959,
        height: '400px',
      },
      //sm
      {
        breakpoint: 599,
        height: '300px',
        thumbnails: false,
        preview: false,
      },
    ];
  }
}

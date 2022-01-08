import { Injectable } from '@angular/core';
import { MediaChange, MediaObserver } from '@angular/flex-layout';
import { distinctUntilChanged, map, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class MediaService {
  public mediaChange$: Observable<string[]>;
  constructor(private mediaObserver: MediaObserver) {
    const getAlias = (MediaChange: MediaChange[]) => {
      return MediaChange[0].mqAlias;
    };
    this.mediaChange$ = this.mediaObserver.asObservable().pipe(
      distinctUntilChanged(
        (x: MediaChange[], y: MediaChange[]) => getAlias(x) === getAlias(y)
      ),
      map((change) => {
        let items: string[] = [];
        change.forEach((item) => items.push(item.mqAlias));
        return items;
      })
    );
  }
}

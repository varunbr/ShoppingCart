import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'toIntArray',
})
export class ToIntArrayPipe implements PipeTransform {
  transform(value: string): number[] {
    return value.split(',').map((e) => parseInt(e));
  }
}

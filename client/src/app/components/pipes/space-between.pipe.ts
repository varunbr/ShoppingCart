import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'spaceBetween',
})
export class SpaceBetweenPipe implements PipeTransform {
  transform(value: string): string {
    return value.replace(/([A-Z])/g, ' $1').trim();
  }
}

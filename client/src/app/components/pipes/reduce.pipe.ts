import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'reduce',
})
export class ReducePipe implements PipeTransform {
  transform(value: string): string {
    if (value.length <= 25) return value;
    let result = '';
    for (let item of value.split(' ')) {
      if (result.length + item.length <= 25) {
        result = `${result} ${item}`;
      } else {
        return `${result}...`.trim();
      }
    }
    return result.trim();
  }
}

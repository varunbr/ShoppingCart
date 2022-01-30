import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'reduce',
})
export class ReducePipe implements PipeTransform {
  transform(value: string, length: number = 25): string {
    if (value.length <= length) return value;
    let result = '';
    for (let item of value.split(' ')) {
      if (result.length + item.length <= length) {
        result = `${result} ${item}`;
      } else {
        return `${result}...`.trim();
      }
    }
    return result.trim();
  }
}

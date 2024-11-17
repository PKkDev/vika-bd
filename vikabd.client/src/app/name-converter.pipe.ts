import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'nameConverterPipe'
})
export class NameConverterPipe implements PipeTransform {

  transform(value: string): string {
    if (value.toLocaleLowerCase().includes('семья'))
      return 'Придёте на мой'
    return 'Придёшь на мой';
  }
  
}

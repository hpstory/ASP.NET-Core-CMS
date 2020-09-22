import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'contextSplit'
})
export class ContextSplitPipe implements PipeTransform {

  transform(value: string): string {
    value = value.replace(/(<[^>]+>)/g, "").replace(/\s/g, "");
    return value;
  }

}

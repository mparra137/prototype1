import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'cpf'
})
export class CpfPipe implements PipeTransform {

  transform(value: any, args?: any): any {
    var valueTransformed:string = value;
    valueTransformed = valueTransformed.padStart(11, '0').substring(0,11).replace(/[^0-9]/g, "").replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, "$1.$2.$3-$4");   
    
    return valueTransformed;
  }

}

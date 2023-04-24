import { AbstractControl, ValidatorFn, ValidationErrors } from "@angular/forms";

export function CPFValidator(): ValidatorFn {
    return (control: AbstractControl) : ValidationErrors | null => {
        const value = control.value;
      
        if (!value) return null;
       
        let cpf: string = value;
        let cpfArray: Array<string> = cpf.split('');
        
        let lastNumber: number = parseInt(cpfArray.pop()!);
        let priorLastNumber: number = parseInt(cpfArray.pop()!);        

        let multiplier:number = 10;
        let total: number = 0;
        cpfArray.forEach(item => {      
            total += +item * multiplier;
            multiplier -= 1;
        });
        
        let firstCheckValue: number = (total * 10) % 11;
        firstCheckValue = firstCheckValue === 10 ? 0: firstCheckValue;   

        if (firstCheckValue !== priorLastNumber){   
            return {invalidCPF: true};
        } else {
            cpfArray.push(priorLastNumber.toString());      

            multiplier = 11;
            total = 0;
            cpfArray.forEach(item => {      
                total += +item * multiplier;
                multiplier -= 1;
            });
            
            let secCheckValue: number = (total * 10) % 11
            secCheckValue = secCheckValue === 10 ? 0 : secCheckValue;
            
            return (secCheckValue !== lastNumber) ? {invalidCPF: true} : null;    
            
        }

        return null;
    }

}

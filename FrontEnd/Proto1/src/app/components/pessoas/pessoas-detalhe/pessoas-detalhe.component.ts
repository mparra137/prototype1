import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, FormBuilder, Validators, AbstractControl } from '@angular/forms';
import { Person } from '@app/model/Person';
import { CepService } from '@app/services/cep.service';
import { Cep } from '@app/model/cep';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { PersonService } from '@app/services/person.service';
import { CPFValidator } from '@app/helper/CustomValidator';
import { Router } from '@angular/router';

@Component({
  selector: 'app-pessoa-detalhe',
  templateUrl: './pessoas-detalhe.component.html',
  styleUrls: ['./pessoas-detalhe.component.scss']
})
export class PessoasDetalheComponent implements OnInit {
  public form!: FormGroup;

  constructor(private personService: PersonService, private fb: FormBuilder, private cepService: CepService, private toastr: ToastrService, private spinner: NgxSpinnerService, private router: Router ) { }

  ngOnInit(): void {
    this.initForm();
  }

  private initForm(): void{
    this.form = this.fb.group({
      id: [0],
      nome: ['', Validators.required],
      cpf: ['', {validators: [Validators.required, CPFValidator()], updateOn: 'blur'}],
      cep: ['', Validators.required],
      endereco: [''],
      numero: ['', Validators.required],
      bairro: [''],
      cidade: [''],
      uf: [''],
      complemento: ['']
    })

    this.form.get('uf')?.disable();    
  }
  
  public get f(): any{
    return this.form.controls;
  }

  public submitForm(): void{
    let person: Person = {...this.form.value};
    this.spinner.show();
    this.personService.post(person).subscribe({
      next: (result: Person) => {
        this.toastr.success("Novo cliente inserido", "Sucesso");  
        this.router.navigateByUrl("/pessoas/lista");      
      },
      error: (error: any) => {
        console.error(error);
        this.toastr.error("Não foi possível inserir novo cliente", "Erro")
      }
    }).add(() => this.spinner.hide());
  }  
  
  public checkPostalCode(): void { 
    this.spinner.show();
    let cep: string = this.f.cep.value;
    this.cepService.getAdress(cep).subscribe({
      next: (result: any) => {
        if (!result.erro){
          this.f.endereco.setValue(result.logradouro);
          this.f.bairro.setValue(result.bairro);
          this.f.cidade.setValue(result.localidade);
          this.f.uf.setValue(result.uf);
        } else {
          this.toastr.error("CEP inválido", "Erro")
        }                                      
      },
      error: (error: any) => {
        console.error(error);
        this.toastr.error("Não foi possível obter o endereço", "Erro")
      }
    }).add(() => this.spinner.hide());    
  }

  public cssValidator(formControl: FormControl, controlName: string | null = ""): any{  
    if (controlName === 'cpf')
    {
      return {'is-invalid' : formControl?.errors && formControl?.touched, 'is-valid': !formControl?.errors && formControl.touched}   
    }  
    return {'is-invalid' : formControl?.errors && formControl?.touched}   
  }

}

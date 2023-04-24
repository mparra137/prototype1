import { Component, OnInit } from '@angular/core';
import { Person } from '@app/model/Person';
import { PersonService } from '@app/services/person.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-pessoas-lista',
  templateUrl: './pessoas-lista.component.html',
  styleUrls: ['./pessoas-lista.component.scss']
})
export class PessoasListaComponent implements OnInit {

  public persons: Person[] = [];

  constructor(private personService: PersonService, private spinner: NgxSpinnerService, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.loadList();
  }

  public loadList(): void{
    this.spinner.show();
    this.personService.getList().subscribe({
      next: (returnedData: Person[]) => {
        this.persons = returnedData;
      },
      error: (error: any) => {
        console.log(error);
        this.toastr.error("Ocorreu Erro ao tentar obter os dados", "Erro");
      }
    }).add(() => this.spinner.hide());
  }

}

import { Component, OnInit, TemplateRef } from '@angular/core';
import { Person } from '@app/model/Person';
import { PersonService } from '@app/services/person.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { Subject, debounceTime, distinctUntilChanged } from 'rxjs';
import { HttpResponse } from '@angular/common/http';

@Component({
  selector: 'app-pessoas-lista',
  templateUrl: './pessoas-lista.component.html',
  styleUrls: ['./pessoas-lista.component.scss']
})
export class PessoasListaComponent implements OnInit {
  private modalRef?: BsModalRef;
  public pessoaAux: any = {};
  public persons: Person[] = [];
  public term?: string;

  public searchTermChanged: Subject<string> = new Subject();

  constructor(private personService: PersonService, private spinner: NgxSpinnerService, private toastr: ToastrService, private router: Router, private modalService: BsModalService) { }

  ngOnInit(): void {
    this.loadList();
    this.createFilterSubject();
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

  public filterData(): void{    
    if (this.term)
      this.searchTermChanged.next(this.term); 
  }

  public createFilterSubject(): void{
    if (!this.searchTermChanged.observed){
      this.searchTermChanged.pipe(debounceTime(1000), distinctUntilChanged()).subscribe(data => {        
        this.spinner.show();
        this.personService.getList(data).subscribe({
          next: (returnedData: Person[]) => {
            this.persons = returnedData;            
          },
          error: (error: any) => {
            console.error(error);
            this.toastr.error("Não foi possível executar a filtragem dos dados", "Erro");
          }
        }).add(() => this.spinner.hide());
      })
    }
  }

  public personDetail(id: number): void {
    this.router.navigate(["/pessoas/detalhe/",id]);
  }

  public deletePerson(id: number): void{
    this.personService.delete(id).subscribe({
      next: (deleteResult: any) => {
        this.toastr.success("Pessoa excluída", "Sucesso");
        this.loadList();
      },
      error: (error: any) => {
        console.error(error);
        this.toastr.error("Não foi possível excluir", "Erro");
      }
    });
  }

  public openModalDelete(modalRef: TemplateRef<any>, event: Event, pessoa: Person): void{
    event.stopPropagation();
    this.pessoaAux = { id: pessoa.id, nome: pessoa.nome};
    this.modalRef = this.modalService.show(modalRef, {class: "modal-md"});
  }

  public confirmDelete(): void{
    this.deletePerson(this.pessoaAux.id);
    this.modalRef?.hide();
  }

  public declineDelete(): void{
    this.modalRef?.hide();
  }

  public exportPDF(): void{
    this.personService.pdfReport(this.term).subscribe({
        next: (result: HttpResponse<Blob>) => {          
          let fileName = this.getFileName(result);
          let binaryData: Blob[] = [];
          binaryData.push(result.body as any);
          let downloadLink = document.createElement("a");
          downloadLink.href = window.URL.createObjectURL(new Blob(binaryData, {type: 'blob'}));
          downloadLink.setAttribute('download', fileName);
          document.body.append(downloadLink);
          downloadLink.click();
        },
        error: (error : any) => {
          console.error(error);
          this.toastr.error("Não foi possível obter o arquivo", "Erro");
        }        
    });
  }

  private getFileName(response: HttpResponse<Blob>): string{
    let fileName: string = "";
    try{
      const contentDisposition: string = response.headers.get("content-disposition") ?? "";      
      const regEx = /(?:filename=)(.+)(?:;)/g;      
      fileName = regEx.exec(contentDisposition)![1] ?? "";            
    } catch(e){
      fileName = 'file.pdf';
    }
    return fileName;
  }

}

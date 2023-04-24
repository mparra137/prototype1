import { Injectable } from '@angular/core';
import { Cep } from '@app/model/cep';
import { HttpClient } from '@angular/common/http';
import { Observable, take } from 'rxjs';

@Injectable()
export class CepService {

  constructor(private http: HttpClient) { }

  public getAdress(cep: string) : Observable<any>{
    return this.http.get<any>(`https://viacep.com.br/ws/${cep}/json/`).pipe(take(1));
  }

}

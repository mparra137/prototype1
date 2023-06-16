import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Person } from '@app/model/Person';
import { Observable, take } from 'rxjs';
import { environment } from 'src/environments/environment';
import { query } from '@angular/animations';

@Injectable({
  providedIn: 'root'
})
export class PersonService {
  private baseUrl: string = environment.apiURL +'/api/pessoa';
  
  constructor(private http: HttpClient) { }

  public getList(searchTerm?: string): Observable<Person[]>{
    let queryParams: HttpParams = new HttpParams();    
    if (searchTerm){      
      queryParams = queryParams.set("searchTerm", searchTerm);      
    }    

    return this.http.get<Person[]>(`${this.baseUrl}`, {params: queryParams}).pipe(take(1));
  }

  public getById(id: number): Observable<Person>{
    return this.http.get<Person>(`${this.baseUrl}/${id}`).pipe(take(1));
  }

  public post(person: Person) : Observable<Person>{    
    return this.http.post<Person>(`${this.baseUrl}/save`, person).pipe(take(1));
  }

  public searchByCPF(cpf: string) : Observable<any> {
    return this.http.get(`${this.baseUrl}/checkcpf/${cpf}`).pipe(take(1));
  }

  public delete(id: number) : Observable<any> {
    return this.http.delete<Person>(`${this.baseUrl}/${id}`).pipe(take(1));
  }

  public pdfReport(searchTerm?: string): Observable<HttpResponse<any>>{
    let queryParams: HttpParams = new HttpParams();    
    if (searchTerm){      
      queryParams = queryParams.set("searchTerm", searchTerm);      
    }  
    return this.http.get(`${this.baseUrl}/report/all`, {responseType: 'arraybuffer', observe: 'response', params: queryParams}).pipe(take(1));
  }


}

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Person } from '@app/model/Person';
import { Observable, take } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PersonService {
  private baseUrl: string = environment.apiURL +'/api/pessoa';
  

  constructor(private http: HttpClient) { }

  public getList(): Observable<Person[]>{
    return this.http.get<Person[]>(`${this.baseUrl}/all`).pipe(take(1));
  }

  public getById(id: number): Observable<Person>{
    return this.http.get<Person>(`${this.baseUrl}/${id}`).pipe(take(1));
  }

  public post(person: Person) : Observable<Person>{
    console.log(person);
    return this.http.post<Person>(`${this.baseUrl}/save`, person).pipe(take(1));
  }

}

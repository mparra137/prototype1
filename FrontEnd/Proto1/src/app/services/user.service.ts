import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { User } from '@app/model/Identity/user';
import { Observable, take } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Roles } from '@app/model/Identity/Roles';
import { RoleUpdate } from '@app/model/Identity/RoleUpdate';
import { query } from '@angular/animations';

@Injectable()
export class UserService {
  private baseUrl: string = environment.apiURL +'/api/user';
  private baseUrlForRoles: string = environment.apiURL + '/api/roles';

  constructor(private http: HttpClient) { }

  public getUsers() : Observable<User[]>{
    return this.http.get<User[]>(this.baseUrl).pipe(take(1));
  }  

  public getUserData(id: number) : Observable<User> {
    return this.http.get<User>(`${this.baseUrl}/${id}`).pipe(take(1));
  }

  public getRoles(): Observable<Roles[]>{
    return this.http.get<Roles[]>(`${this.baseUrlForRoles}`).pipe(take(1));
  }

  public postUserRoles(userName: string, roles: RoleUpdate[]): Observable<any> {
    let params: HttpParams = new HttpParams();
    params = params.append("userName", userName);

    return this.http.post<RoleUpdate>(`${this.baseUrl}/assignListOfRoles`, roles, {params}).pipe(take(1));
  }

  public removeRole(userName: string, roleName: string) : Observable<any>{
    let queryParams: HttpParams = new HttpParams();
    queryParams = queryParams.append("userName", userName);
    queryParams = queryParams.append("roleName", roleName);

    console.log(queryParams);

    return this.http.post(`${this.baseUrl}/removeRole`, null, {params: queryParams}).pipe(take(1));
  }


}

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, ReplaySubject, Subject, map, subscribeOn, take } from 'rxjs';
import { LoggedInUser } from '@app/model/Identity/loggedInUser';
import { LoginData } from '@app/model/Identity/loginData';
import { environment } from 'src/environments/environment';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private baseUrl: string = environment.apiURL + "/api/user";
  public signedInUser = new ReplaySubject<LoggedInUser>(1);
  public currentUser$ = this.signedInUser.asObservable();

  constructor(private http: HttpClient, private router: Router) { }

  public executeSignIn(login: LoginData) : Observable<void>{
    return this.http.post<LoggedInUser>(`${this.baseUrl}/login`, login).pipe(take(1), map(
      (result: LoggedInUser) => {
        this.storeLoginData(result);
      }
    ));
  }

  public storeLoginData(loginData: LoggedInUser): void {
    if (loginData){
      localStorage.setItem("userData", JSON.stringify(loginData));      
      this.signedInUser.next(loginData);
    }
  }

  public executeSignOut(): void{
    localStorage.removeItem("userData");
    this.signedInUser.next(null as any);      
    this.router.navigateByUrl("/user/login");
  }

  public checkToken(): Observable<any>{
    return this.http.get(`${this.baseUrl}/checktoken`).pipe(take(1));
  }
}

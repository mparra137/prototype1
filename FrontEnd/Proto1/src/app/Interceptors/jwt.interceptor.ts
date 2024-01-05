import { Injectable } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpResponse,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, catchError, take } from 'rxjs';
import { AccountService } from '@app/services/account.service';
import { LoggedInUser } from '@app/model/Identity/loggedInUser';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  constructor(private accountService: AccountService, private route: Router) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    this.accountService.currentUser$.pipe(take(1)).subscribe((user: LoggedInUser) => {
      if (user){
        request = request.clone({
          setHeaders: {Authorization: "Bearer " + user.token}
        })
      }      
    });

    return next.handle(request);
    /*.pipe(
      catchError((error:any, caught: Observable<any>) => {
        console.error('Interceptor catch: ', error);

        //this.route.navigate(["/user/login"]);

        throw new Error(error);                    
      })
    );
    */
  }
}

import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, take } from 'rxjs';
import { AccountService } from '@app/services/account.service';
import { LoggedInUser } from '@app/model/Identity/loggedInUser';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  constructor(private accountService: AccountService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    this.accountService.currentUser$.pipe(take(1)).subscribe((user: LoggedInUser) => {
      if (user){
        request = request.clone({
          setHeaders: {Authorization: "Bearer " + user.token}
        })
      }      
    });

    return next.handle(request);
  }
}

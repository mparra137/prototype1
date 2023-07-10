import { Component } from '@angular/core';
import { AccountService } from './services/account.service';
import { LoggedInUser } from './model/Identity/loggedInUser';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'Proto1';

  constructor(private accountService: AccountService){}

  ngOnInit(){
    this.setCurrentUser();
  }

  private setCurrentUser(): void{
    let user: LoggedInUser;
    if (localStorage.getItem('userData')){
      user = JSON.parse(localStorage.getItem('userData')!);
    } else {
      user = null as any
    }

    this.accountService.storeLoginData(user);
  }
}

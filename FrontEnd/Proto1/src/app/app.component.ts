import { Component } from '@angular/core';
import { AccountService } from './services/account.service';
import { LoggedInUser } from './model/Identity/loggedInUser';
import { Route, Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'Proto1';

  constructor(private accountService: AccountService, private router: Router){}

  ngOnInit(){
    this.setCurrentUser();
  }

  private setCurrentUser(): void{
    console.log("App Component fired");
    let user: LoggedInUser;
    if (localStorage.getItem('userData')){
      user = JSON.parse(localStorage.getItem('userData')!);      
    } else {
      user = null as any
    }    

    if (user){
      this.accountService.storeLoginData(user);      

      //Se possui token gravado, pode ser que já esteja expirado/não seja mais válido
      //Abaixo faz uma requisição simples para garantir autenticação
      this.accountService.checkToken().subscribe({
        next: (message: any) => {
          //console.log('Mensagem checktoken: ',message);
        },
        error: (err: any) => {
          console.error('Error checktoken: ',err.status);
          if (err.status = 401){
            this.accountService.executeSignOut();
            this.router.navigate(["/user/login"]);
          }
        }
      });
    } else {
      this.router.navigate(["/user/login"]);
    }
  }
}

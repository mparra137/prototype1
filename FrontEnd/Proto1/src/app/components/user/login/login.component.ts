import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LoggedInUser } from '@app/model/Identity/loggedInUser';
import { AccountService } from '@app/services/account.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  public loginModel = {userName :  "", password : ""};

  constructor(private accountService: AccountService, private toastr: ToastrService, private router: Router) { }

  ngOnInit(): void {
  }

  public submitLogin(): void{
    this.accountService.executeSignIn(this.loginModel).subscribe({
      next: () => {
        this.toastr.success("Login efetuado");
        this.router.navigate(["/home"]);
      },
      error: (error: any) => {
        console.error(error);
        this.toastr.error("Não foi possível realizar o login", "Erro")
      }
    });
  }

}

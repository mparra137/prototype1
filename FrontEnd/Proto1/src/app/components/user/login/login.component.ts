import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LoggedInUser } from '@app/model/Identity/loggedInUser';
import { AccountService } from '@app/services/account.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  public loginModel = {userName :  "", password : ""};

  constructor(private accountService: AccountService, private toastr: ToastrService, private router: Router, private spinner: NgxSpinnerService) { }

  ngOnInit(): void {
  }

  public submitLogin(): void{
    this.spinner.show();
    this.accountService.executeSignIn(this.loginModel).subscribe({
      next: () => {
        //this.toastr.success("Login efetuado");
        this.router.navigate(["/home"]);
      },
      error: (error: any) => {
        console.error(error);
        if (error.status == 401){
          this.toastr.error(error.error, "Erro");
        } else {
          this.toastr.error("Não foi possível realizar o login", "Erro");
        }
      }
    }).add(() => this.spinner.hide());
  }

}

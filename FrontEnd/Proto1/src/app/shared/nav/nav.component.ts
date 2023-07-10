import { Component, OnInit } from '@angular/core';
import { AccountService } from '@app/services/account.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent implements OnInit {
  isCollapsed = true;
  constructor(private accountService: AccountService, private router: Router) { }

  ngOnInit(): void {
  }

  public signOut(): void{
    this.accountService.executeSignOut();
  }

  public showNav() : boolean {    
    return this.router.url !== "/user/login";
  }

}

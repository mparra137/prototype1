import { Component, OnInit } from '@angular/core';
import { User } from '@app/model/Identity/user';
import { UserService } from '@app/services/user.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss']
})
export class UserListComponent implements OnInit {
  public users!: User[];
  
  constructor(private userService: UserService, private spinner: NgxSpinnerService, private router: Router) { }

  ngOnInit(): void {
    this.LoadList();
  }

  public LoadList(): void {
    this.spinner.show();
    this.userService.getUsers().subscribe({
      next: (userlist: User[]) => {
        this.users = userlist;
      },
      error: (err: any) => {
        console.log(err);
      }
    }).add(() => {this.spinner.hide()});
  }

  public UserDetails(id: number) : void{
    this.router.navigate(["/user/user-detail/", id]);
  }

}

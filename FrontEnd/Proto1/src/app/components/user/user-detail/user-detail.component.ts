import { Component, OnInit, TemplateRef, ChangeDetectorRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ActivatedRoute, Router } from '@angular/router';
import { User } from '@app/model/Identity/user';
import { UserService } from '@app/services/user.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { Subscription, combineLatest, forkJoin } from 'rxjs';
import { Roles } from '@app/model/Identity/Roles';
import { RoleUpdate } from '@app/model/Identity/RoleUpdate';
import { FormControl, FormBuilder, FormGroup, FormArray } from '@angular/forms';

@Component({
  selector: 'app-user-detail',
  templateUrl: './user-detail.component.html',
  styleUrls: ['./user-detail.component.scss']
})
export class UserDetailComponent implements OnInit {
  public userId: number = 0;
  public userData: User = {} as User;
  public modalRef?: BsModalRef;
  public subscriptions: Subscription[] = [];
  public rolesList: Roles[] = {} as Roles[];
  public form!: FormGroup;

  constructor(private route: ActivatedRoute, 
              private router: Router, 
              private userService: UserService, 
              private toastr: ToastrService, 
              private spinner: NgxSpinnerService, 
              private modalService: BsModalService, 
              private changeDetection: ChangeDetectorRef,
              private fb: FormBuilder
              ) { }

  ngOnInit(): void {
    this.loadUserInfo();   
    this.CreateForm();     
  }

  public loadUserInfo() : void{
    this.userId = +this.route.snapshot.paramMap.get('id')!;
    this.userService.getUserData(this.userId).subscribe({
      next: (user: User) => {
        this.userData = user;
      },
      error: (err : any) => {
        console.error(err);
        this.toastr.error("Não foi possível obter os dados","Erro");
      }
    });
  }

  public CreateForm(): void{
    this.form = this.fb.group({
      roles: this.fb.array([])
    })
  }

  public CreateRoleLine(role: RoleUpdate): FormGroup{
    return this.fb.group({
      selected: [role.selected],
      roleName: [role.roleName]
    });
  }

  public ReturnToList() : void{
    this.router.navigate(["user/user-list"]);
  }

  public hasRoles(): boolean {
    return Object.keys(this.userData?.roles).length > 0;
  }

  public RemoveRole(index: number): void {
    this.spinner.show();
    let roleName = this.userData.roles.at(index)!;
    this.userService.removeRole(this.userData.userName, roleName).subscribe({
      next: () => {
        this.toastr.success("Removido");
        this.userData.roles.splice(index, 1); 
        //let removed = this.userData.roles.splice(index, 1); 
        //console.log(removed[0]);
      },
      error: (error: any) => {
        console.error(error);
        this.toastr.error("Erro ao remover a role", "Erro");
      } 
    }).add(() => this.spinner.hide());    
  }


  public OpenModal(template: TemplateRef<any>){
    const combine = forkJoin([this.modalService.onShow, this.modalService.onShow, this.modalService.onHide, this.modalService.onHidden]).subscribe(() => this.changeDetection.markForCheck());

    this.subscriptions.push(
      this.modalService.onShow.subscribe(() => {
        console.log('OnShow - Mostrando');
        this.spinner.show();
        this.roles.clear();

        this.userService.getRoles().subscribe({
          next: (roles: Roles[]) => {
            let arrayRoles: RoleUpdate[] = [];
            roles.forEach((role: Roles) => {
              console.log(role);
              
              //arrayRoles.push({selected: this.userData.roles.includes(role.name), roleName: role.name});

              this.roles.push(this.CreateRoleLine({selected: this.userData.roles.includes(role.name), roleName: role.name}));
            });
                     
          },
          error: (err: any) => {
            this.toastr.error("Não foi possível obter a lista de roles", "Erro");
          } 
        }).add(() => this.spinner.hide());
      })
    );


    this.subscriptions.push(
      this.modalService.onShown.subscribe(() => {
        console.log('OnShown - Mostrado');        
      })
    );

    this.subscriptions.push(
      this.modalService.onHide.subscribe((reason: string | any) => {
        console.log(`OnHide has ben fired - ${reason} - ${reason.id}`);
      })
    );

    this.subscriptions.push(
      this.modalService.onHidden.subscribe((reason: string | any) => {
        console.log(`onHidden has ben fired - ${reason} - ${reason.id}`);
        this.unsubscribe();
      })
    );

    this.subscriptions.push(combine);

    this.modalRef = this.modalService.show(template);
  }

  public closeModal(){
    this.modalRef?.hide();
  }

  public SaveSelectedRoles(): void{
    console.log(this.form.value.roles);

    let roles: RoleUpdate[] = this.form.value.roles;
    //let ArraySelectedRoles: RoleUpdate[] = [];
    //roles.forEach((role: RoleUpdate) => {
    //  if (role.selected) {
    //    ArraySelectedRoles.push(role);
    //  }
    //});

    //console.log('Roles Selecionadas: ', ArraySelectedRoles);

    this.spinner.show();
    this.userService.postUserRoles(this.userData.userName, roles).subscribe({
      next: () => {
        this.toastr.success("Roles do usuário atualizadas", "Sucesso");
        this.loadUserInfo();
      },
      error: (error: any) => {
        this.toastr.error("Ocorreu erro ao tentar alterar as Roles do usuário", "Erro");
      }
    }).add(() => this.spinner.hide());

    this.modalRef?.hide();
  }

  unsubscribe(){
    this.subscriptions.forEach((subscription: Subscription) => {
      subscription.unsubscribe();
    });
    this.subscriptions = [];
  }

  public get roles() : FormArray{
    return this.form.get('roles') as FormArray;
  }


}

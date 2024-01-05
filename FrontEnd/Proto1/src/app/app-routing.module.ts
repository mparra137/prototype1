import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PessoasComponent } from './components/pessoas/pessoas.component';
import { PessoasListaComponent } from './components/pessoas/pessoas-lista/pessoas-lista.component';
import { PessoasDetalheComponent } from './components/pessoas/pessoas-detalhe/pessoas-detalhe.component';
import { HomeComponent } from './components/home/home.component';
import { UserComponent } from './components/user/user.component';
import { LoginComponent } from './components/user/login/login.component';
import { UserListComponent } from './components/user/user-list/user-list.component';
import { UserDetailComponent } from './components/user/user-detail/user-detail.component';

const routes: Routes = [
  {path: "home", component: HomeComponent},  
  {path: "pessoas", redirectTo: "pessoas/lista", pathMatch: 'full'},
  {path: "pessoas", component: PessoasComponent, children: [    
    {path: "detalhe", component: PessoasDetalheComponent},
    {path: "detalhe/:id", component: PessoasDetalheComponent},
    {path: "lista", component: PessoasListaComponent},
  ]},  
  {path: "user", component: UserComponent, children: [
    {path: "login", component: LoginComponent},
    {path: "user-list", component: UserListComponent},
    {path: "user-detail/:id", component: UserDetailComponent}
  ]},
  {path: "", component: HomeComponent},
  {path: "**", component: HomeComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

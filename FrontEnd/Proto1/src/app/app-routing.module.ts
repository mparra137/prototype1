import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PessoasComponent } from './components/pessoas/pessoas.component';
import { PessoasListaComponent } from './components/pessoas/pessoas-lista/pessoas-lista.component';
import { PessoasDetalheComponent } from './components/pessoas/pessoas-detalhe/pessoas-detalhe.component';
import { HomeComponent } from './components/home/home.component';

const routes: Routes = [
  {path: "home", component: HomeComponent},  
  {path: "pessoas", redirectTo: "pessoas/lista", pathMatch: 'full'},
  {path: "pessoas", component: PessoasComponent, children: [    
    {path: "detalhe", component: PessoasDetalheComponent},
    {path: "detalhe/:id", component: PessoasDetalheComponent},
    {path: "lista", component: PessoasListaComponent},
  ]},  
  
  {path: "", component: HomeComponent},
  {path: "**", component: HomeComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

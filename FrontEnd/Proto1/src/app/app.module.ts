import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

//ngx-bootstrap
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { ModalModule } from 'ngx-bootstrap/modal';
import { ToastrModule } from 'ngx-toastr';
import { NgxSpinnerModule } from 'ngx-spinner';
import { PaginationModule } from 'ngx-bootstrap/pagination';

//ngx-mask
import { NgxMaskModule } from 'ngx-mask';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { PessoasComponent } from './components/pessoas/pessoas.component';
import { NavComponent } from './shared/nav/nav.component';
import { PessoasDetalheComponent } from './components/pessoas/pessoas-detalhe/pessoas-detalhe.component';
import { PessoasListaComponent } from './components/pessoas/pessoas-lista/pessoas-lista.component';
import { HomeComponent } from './components/home/home.component';

//Pipe
import { CpfPipe } from './helper/CpfPipe.pipe';

//Services
import { CepService } from './services/cep.service';
import { UserService } from './services/user.service';

import { UserComponent } from './components/user/user.component';
import { LoginComponent } from './components/user/login/login.component';
import { JwtInterceptor } from './Interceptors/jwt.interceptor';
import { UserListComponent } from './components/user/user-list/user-list.component';
import { UserDetailComponent } from './components/user/user-detail/user-detail.component';




@NgModule({
  declarations: [
    AppComponent,
    PessoasComponent,
    NavComponent,
    PessoasDetalheComponent,
    PessoasListaComponent,
    HomeComponent,
    CpfPipe,
    UserComponent,
    LoginComponent,
    UserListComponent,
    UserDetailComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    CollapseModule.forRoot(),
    ToastrModule.forRoot({
      timeOut: 3000,
      positionClass: 'toast-bottom-right',
      preventDuplicates: true,
      progressBar: true
    }),
    NgxSpinnerModule.forRoot(),
    NgxMaskModule.forRoot(),
    ModalModule.forRoot(),
    PaginationModule.forRoot()
  ],
  providers: [CepService, UserService, {provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true}],
  bootstrap: [AppComponent]
})
export class AppModule { 
}

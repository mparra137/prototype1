import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

//ngx-bootstrap
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { ToastrModule } from 'ngx-toastr';
import { NgxSpinnerModule } from 'ngx-spinner';

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

@NgModule({
  declarations: [
    AppComponent,
    PessoasComponent,
    NavComponent,
    PessoasDetalheComponent,
    PessoasListaComponent,
    HomeComponent,
    CpfPipe
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
    NgxMaskModule.forRoot()
  ],
  providers: [CepService],
  bootstrap: [AppComponent]
})
export class AppModule { 
}

import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AcceptedCardComponent } from './accepted-card/accepted-card.component';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ErrorCardComponent } from './error-card/error-card.component';
import { MainCardComponent } from './main-card/main-card.component';

@NgModule({
  declarations: [
    AppComponent,
    MainCardComponent,
    AcceptedCardComponent,
    ErrorCardComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule
  ],
  providers: [
    {
      provide: 'BASE_URL',
      useValue: 'https://www.vika-birthday.ru/api'
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

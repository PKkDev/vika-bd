import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AcceptedCardComponent } from './accepted-card/accepted-card.component';
import { ErrorCardComponent } from './error-card/error-card.component';
import { MainCardComponent } from './main-card/main-card.component';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'додик'
  },
  {
    path: 'error/:key',
    component: ErrorCardComponent
  },
  {
    path: 'accepted/:key',
    component: AcceptedCardComponent
  },
  {
    path: ':key',
    component: MainCardComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule { }

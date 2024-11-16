import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainCardComponent } from './main-card/main-card.component';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'додик'
  },
  {
    path: ':name',
    component: MainCardComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

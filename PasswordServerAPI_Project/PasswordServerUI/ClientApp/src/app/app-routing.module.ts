import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';


export const appRoutes: Routes = [
  { path: '', redirectTo: '/', pathMatch: 'full' },
  { path: '**', redirectTo: '/' }
];

@NgModule({
  imports: [
    RouterModule.forRoot(appRoutes)
],
exports: [
    RouterModule
]
})
export class AppRoutingModule { }

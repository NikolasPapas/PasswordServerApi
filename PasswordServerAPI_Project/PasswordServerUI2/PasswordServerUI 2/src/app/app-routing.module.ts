import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
// import { ApplicationDataComponent } from "./ui/application/listing/listing-item/applicationData.component";

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
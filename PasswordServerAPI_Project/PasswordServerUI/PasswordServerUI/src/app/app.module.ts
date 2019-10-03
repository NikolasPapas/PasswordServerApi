import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { ApplicationModule } from './ui/application.module';
import { CoreServiceModule } from './core/services/core-service.module';
import { CommonModule } from '@angular/common';  
import { CommonUiModule } from './common/types/common-ui.module';

@NgModule({
  
  imports: [
    CommonUiModule,
    BrowserModule,
    AppRoutingModule,
    TranslateModule.forRoot(),
    CoreServiceModule.forRoot(),
    HttpClientModule,
    //UI
    ApplicationModule,
  ],
  declarations: [
    AppComponent,
  ],
  providers: [

  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

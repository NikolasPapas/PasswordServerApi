import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { AppComponent } from './app.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { ApplicationModule } from './ui/application.module';
import { CoreServiceModule } from './core/services/core-service.module';
import { CommonUiModule } from './common/types/common-ui.module';
import { CommonFormsModule } from './common/forms/common-forms.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HeaderTokenInterceptor } from './core/services/header-token-interceptor';

@NgModule({
  
  imports: [
    CommonUiModule,
		CommonFormsModule,
    BrowserModule,
    BrowserAnimationsModule,
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
    { provide: HTTP_INTERCEPTORS, useClass: HeaderTokenInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

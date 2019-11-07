import { BrowserModule } from '@angular/platform-browser';
import { NgModule, LOCALE_ID } from '@angular/core';
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
import { NotificationBarModule } from './common/notification-bar/notification-bar.module';
import { MAT_DATE_LOCALE } from '@angular/material';

@NgModule({
  
  imports: [
    CoreServiceModule.forRoot(),
    CommonUiModule,
		CommonFormsModule,
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    TranslateModule.forRoot(),
    HttpClientModule,
    //UI
    NotificationBarModule,
    ApplicationModule,
  ],
  declarations: [
    AppComponent,
  ],
  providers: [
    { provide: LOCALE_ID, useValue: "el" },
    { provide: MAT_DATE_LOCALE, useValue: "el" },
    { provide: HTTP_INTERCEPTORS, useClass: HeaderTokenInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

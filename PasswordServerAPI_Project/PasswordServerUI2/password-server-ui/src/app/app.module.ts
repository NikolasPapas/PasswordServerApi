import { BrowserModule } from '@angular/platform-browser';
import { NgModule, LOCALE_ID } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CoreServiceModule } from './services/core-service.module';
import { CommonUiModule } from './common/types/common-ui.module';
import { CommonFormsModule } from './common/forms/common-forms.module';
import { TranslateModule } from '@ngx-translate/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NotificationBarModule } from './common/notification-bar/notification-bar.module';
import { MAT_DATE_LOCALE, MAT_BOTTOM_SHEET_DEFAULT_OPTIONS } from '@angular/material';
import { HeaderTokenInterceptor } from './services/header-token-interceptor';
import { ApplicationModule } from './ui/application.module';

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
    { provide: HTTP_INTERCEPTORS, useClass: HeaderTokenInterceptor, multi: true },
    { provide: MAT_BOTTOM_SHEET_DEFAULT_OPTIONS, useValue: {hasBackdrop: true}}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

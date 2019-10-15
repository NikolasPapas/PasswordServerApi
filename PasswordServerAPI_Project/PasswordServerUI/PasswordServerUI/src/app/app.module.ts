import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { ApplicationModule } from './ui/application.module';
import { CoreServiceModule } from './core/services/core-service.module';
import { CommonUiModule } from './common/types/common-ui.module';
import { CommonFormsModule } from './common/forms/common-forms.module';

@NgModule({
  
  imports: [
    CommonUiModule,
		CommonFormsModule,
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

import { NgModule } from '@angular/core';
import { ApplicationComponent } from './application.component';
import { LoginComponent } from './login/login.component';
import { CommonUiModule } from '../common/types/common-ui.module';
import { CommonFormsModule } from '../common/forms/common-forms.module';
import { EditorComponent } from './main-panel/editor/editor.component';

@NgModule({
	imports: [
        CommonUiModule,
        CommonFormsModule,
    ],
    declarations: [
        ApplicationComponent,
        LoginComponent,
        EditorComponent,
    ],
    exports: [
        ApplicationComponent,
    ],
	entryComponents: [
	],
})
export class ApplicationModule { }


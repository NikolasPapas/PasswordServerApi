import { NgModule } from '@angular/core';
import { ApplicationComponent } from './application.component';
import { LoginComponent } from './login/login.component';
import { CommonUiModule } from '../common/types/common-ui.module';
import { CommonFormsModule } from '../common/forms/common-forms.module';
import { EditorComponent } from './main-panel/editor/editor.component';
import { EditorExpansionPanelComponent } from './main-panel/editor/editor-expansion-panel/editor-expansion-panel.component';
import { RequestEditorComponent } from './main-panel/editor/request-editor/request-editor.component';
import { PasswordEditorComponent } from './main-panel/editor/request-editor/password-editor/password-editor.component';
import { AccountEditorComponent } from './main-panel/editor/request-editor/account-editor/account-editor.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

@NgModule({
	imports: [
        CommonUiModule,
        CommonFormsModule,
        BrowserAnimationsModule,
    ],
    declarations: [
        ApplicationComponent,
        LoginComponent,
        EditorComponent,
        EditorExpansionPanelComponent,
        RequestEditorComponent,
        AccountEditorComponent,
        PasswordEditorComponent,

    ],
    exports: [
        ApplicationComponent,
    ],
	entryComponents: [
	],
})
export class ApplicationModule { }


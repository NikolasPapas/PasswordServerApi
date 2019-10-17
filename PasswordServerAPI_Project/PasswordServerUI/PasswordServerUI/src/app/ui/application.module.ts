import { NgModule } from '@angular/core';
import { ApplicationComponent } from './application.component';
import { LoginComponent } from './login/login.component';
import { CommonUiModule } from '../common/types/common-ui.module';
import { CommonFormsModule } from '../common/forms/common-forms.module';
import { EditorComponent } from './main-panel/editor/editor.component';
import { EditorExpansionPanelComponent } from './main-panel/editor/editor-expansion-panel/editor-expansion-panel.component';
import { RequestEditorComponent } from './main-panel/editor/request-editor/request-editor.component';

@NgModule({
	imports: [
        CommonUiModule,
        CommonFormsModule,
    ],
    declarations: [
        ApplicationComponent,
        LoginComponent,
        EditorComponent,
        EditorExpansionPanelComponent,
        RequestEditorComponent,
    ],
    exports: [
        ApplicationComponent,
    ],
	entryComponents: [
	],
})
export class ApplicationModule { }


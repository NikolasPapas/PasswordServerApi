import { NgModule } from '@angular/core';
import { ApplicationComponent } from './application.component';
import { LoginComponent } from './login/login.component';
import { CommonUiModule } from '../common/types/common-ui.module';
import { CommonFormsModule } from '../common/forms/common-forms.module';
import { EditorComponent } from './main-panel/editor/editor.component';
import { ActionExpansionPanelComponent } from './main-panel/editor/action-expansion-panel/action-expansion-panel.component';
import { RequestEditorComponent } from './main-panel/editor/request-editor/request-editor.component';
import { PasswordEditorComponent } from './main-panel/editor/request-editor/password-editor/password-editor.component';
import { AccountEditorComponent } from './main-panel/editor/request-editor/account-editor/account-editor.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { TokenSectionPanel } from './main-panel/editor/token-section-panel/token-section-panel.component';
import { GoogleMap } from './main-panel/google-map/google-map.component';
import { AgmCoreModule } from '@agm/core';


@NgModule({
    imports: [
        CommonUiModule,
        CommonFormsModule,
        BrowserAnimationsModule,
        AgmCoreModule.forRoot({ apiKey: 'AIzaSyCF21kad4kMsy3SLGsDDFz__HO-K6Csmxs' }),
    ],
    declarations: [
        ApplicationComponent,
        GoogleMap,
        LoginComponent,
        EditorComponent,
        ActionExpansionPanelComponent,
        RequestEditorComponent,
        TokenSectionPanel,
        AccountEditorComponent,
        PasswordEditorComponent
    ],
    exports: [
        ApplicationComponent,
        GoogleMap,
    ],
    entryComponents: [
    ],
})
export class ApplicationModule { }


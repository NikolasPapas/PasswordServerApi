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
import { AgmCoreModule } from '@agm/core';
import { BottomSheet } from './common/bottom-sheet/bottom-sheet.component';
import { GoogleMap } from './common/google-map/google-map.component';
import { NoteTabComponent } from './main-panel/editor/note-panel/note-tab.component';
import { RouterModule } from '@angular/router';
import { AccountService } from '../services/account-action.service';
import { NoteService } from '../services/note-servece';


@NgModule({
    imports: [
        CommonUiModule,
        CommonFormsModule,
        BrowserAnimationsModule,
		RouterModule,
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
        PasswordEditorComponent,
        BottomSheet,
        NoteTabComponent,
    ],
    exports: [
        ApplicationComponent,
        GoogleMap,
    ],
    entryComponents: [
        BottomSheet,
    ],
    providers: [
        AccountService,
        NoteService
	],
})
export class ApplicationModule { }


import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { MaterialModule } from '../material/material.module';
import { NgxFloatButtonModule } from 'ngx-float-button';
import { AngularEditorModule } from '@kolkov/angular-editor';

@NgModule({
	imports: [
		CommonModule,
		MaterialModule,
		TranslateModule,
		NgxFloatButtonModule,
		AngularEditorModule,
	],
	exports: [
		CommonModule,
		MaterialModule,
		TranslateModule,
		NgxFloatButtonModule,
		AngularEditorModule,
	]
})
export class CommonUiModule { }

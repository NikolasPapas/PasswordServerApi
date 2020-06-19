import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { MaterialModule } from '../material/material.module';
import { AngularEditorModule } from '@kolkov/angular-editor';

@NgModule({
	imports: [
		CommonModule,
		MaterialModule,
		TranslateModule,
		AngularEditorModule,
	],
	exports: [
		CommonModule,
		MaterialModule,
		TranslateModule,
		AngularEditorModule,
	]
})
export class CommonUiModule { }

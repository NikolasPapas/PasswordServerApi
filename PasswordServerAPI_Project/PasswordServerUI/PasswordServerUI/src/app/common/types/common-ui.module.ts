import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { MaterialModule } from '../material/material.module';

@NgModule({
	imports: [
		CommonModule,
		MaterialModule,
		TranslateModule
	],
	exports: [
		CommonModule,
		MaterialModule,
		TranslateModule
	]
})
export class CommonUiModule { }

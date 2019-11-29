import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { MaterialModule } from '../material/material.module';
import { NgxFloatButtonModule } from 'ngx-float-button';

@NgModule({
	imports: [
		CommonModule,
		MaterialModule,
		TranslateModule,
		NgxFloatButtonModule,
	],
	exports: [
		CommonModule,
		MaterialModule,
		TranslateModule,
		NgxFloatButtonModule,
	]
})
export class CommonUiModule { }

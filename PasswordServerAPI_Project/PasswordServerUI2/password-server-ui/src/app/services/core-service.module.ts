import { ModuleWithProviders, NgModule, Optional, SkipSelf } from '@angular/core';
import { ConfigurationService } from './configuration.service';
import { HttpPostService } from './http-post.service';
import { MetaData } from './urlBuilder';
import { AccountService } from './account-action.service';
import { FileSaveService } from './file-save.service';
import { UiNotificationService } from './ui-notification.service';
import { NoteService } from './note-servece';

//
//
// This is shared module that provides all the services. Its imported only once on the AppModule.
//
//

@NgModule({
})
export class CoreServiceModule {
	constructor(@Optional() @SkipSelf() parentModule: CoreServiceModule) {
		if (parentModule) {
			throw new Error(
				'CoreModule is already loaded. Import it in the AppModule only');
		}
	}
	static forRoot(): ModuleWithProviders {
		return {
			ngModule: CoreServiceModule,
			providers: [
				UiNotificationService,
				HttpPostService,
				MetaData,
				ConfigurationService,
				FileSaveService,
				AccountService,
				NoteService,
			],
		};
	}
}

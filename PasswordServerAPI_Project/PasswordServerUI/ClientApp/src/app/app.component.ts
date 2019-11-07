import { Component } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { Greek } from './common/language/gr';
import { ConfigurationService } from './core/services/configuration.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {
  
  title = 'Password Server Web App';

  constructor(
    private translate: TranslateService,
    public configurationService: ConfigurationService
  ) {

  }

  ngOnInit() {
    // Language Configuration
    this.translate.setDefaultLang('gr');
    this.translate.use('gr');
    this.translate.setTranslation('gr', Greek);

    //this.configurationService.init();
  }
}

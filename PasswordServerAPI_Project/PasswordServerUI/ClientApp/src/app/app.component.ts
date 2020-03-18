import { Component } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { ConfigurationService } from './services/configuration.service';
import { Greek } from './common/language/gr';
import { OverlayContainer } from '@angular/cdk/overlay';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {


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

  title = 'password-server-ui';
}

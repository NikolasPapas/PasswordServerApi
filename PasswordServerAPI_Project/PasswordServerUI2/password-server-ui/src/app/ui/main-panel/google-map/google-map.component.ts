import { BaseComponent } from 'src/app/common/base/base.component';
import { OnInit, Component } from '@angular/core';
import { ConfigurationService } from 'src/app/services/configuration.service';


@Component({
    selector: 'app-google-map',
    templateUrl: './google-map.component.html',
    styleUrls: ['./google-map.component.scss'],
})
export class GoogleMap extends BaseComponent implements OnInit {

    constructor(
        public configurationService: ConfigurationService,
    ) {
        super();
    }

    ngOnInit(): void {

    }

    title = 'My first AGM project';
    lat = 51.678418;
    lng = 7.809007;
}

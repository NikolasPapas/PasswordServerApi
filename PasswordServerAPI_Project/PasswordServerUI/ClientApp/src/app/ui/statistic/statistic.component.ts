import { Component, Input, OnInit, ViewEncapsulation, Output, EventEmitter } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { BaseComponent } from '../../common/base/base.component';
import { ConfigurationService } from '../../core/services/configuration.service';
import { StatisticModel } from './statistic.model';

@Component({
    selector: 'app-statistic',
    templateUrl: './statistic.component.html',
    styleUrls: ['./statistic.component.scss'],
    encapsulation: ViewEncapsulation.None,
})
export class StatisticComponent extends BaseComponent implements OnInit {

    statisticTable: FormGroup;

    public xHeight: number;

    constructor(
        private configurationService: ConfigurationService,
    ) {
        super();
    }

    ngOnInit() {
        this.statisticTable = new StatisticModel().fromModel().buildForm();

        for (let i = 1; i <= 100; i++) {
            (this.statisticTable.get('xValues').value as Array<string>).push(this.makeid(1));
            (this.statisticTable.get('yValues').value as number[]).push(Math.random());
        }


    }

    makeid(length) {
        var result = '';
        var characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';
        var charactersLength = characters.length;
        for (var i = 0; i < length; i++) {
            result += characters.charAt(Math.floor(Math.random() * charactersLength));
        }
        return result;
    }

    GetColSpan(xIndex: number): number {
        let maxValue = Math.max(...this.statisticTable.get('yValues').value);
        let value = ((this.statisticTable.get('yValues').value[xIndex] * 100) / maxValue);
        return value;
    }

    GetProgressSpan(xIndex: number): number {
        let maxValue = Math.max(...this.statisticTable.get('yValues').value);
        let value = ((this.statisticTable.get('yValues').value[xIndex] * 100) / maxValue);
        return Math.round(value);
    }

    GetMaxValue() {
        return Math.max(...this.statisticTable.get('yValues').value)
    }

    private befor: number = 0;
    getColor(xIndex: number): string {
        if (this.befor < xIndex) {
            this.befor = xIndex + 1;
            return 'lightblue';
        } else {
            this.befor = xIndex;
            return '#DDBDF1';
        }

    }

}



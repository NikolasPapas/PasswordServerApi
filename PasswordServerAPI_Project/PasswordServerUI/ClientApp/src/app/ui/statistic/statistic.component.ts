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

    // public table: StatisticItem[] = [];

    public xTable: Array<Array<string>> = [];


    constructor() {
        super();
    }

    ngOnInit() {
        // this.statisticTable = new StatisticModel().fromModel().buildForm();

        // for (let i = 1; i <= 100; i++) {
        //     (this.statisticTable.get('xValues').value as Array<string>).push(this.makeId(1));
        //     (this.statisticTable.get('yValues').value as number[]).push(Math.random());
        // }
        // this.table = this.GetStatisticTable();

       // this.cycleStatistics();
    }

    // makeId(length: number) {
    //     var result = '';
    //     var characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';
    //     var charactersLength = characters.length;
    //     for (var i = 0; i < length; i++) {
    //         result += characters.charAt(Math.floor(Math.random() * charactersLength));
    //     }
    //     return result;
    // }

    // GetColSpan(xIndex: number): number {
    //     let maxValue = Math.max(...this.statisticTable.get('yValues').value);
    //     let value = ((this.statisticTable.get('yValues').value[xIndex] * 100) / maxValue);
    //     return value;
    // }

    // GetReverseColSpan(xIndex: number): number {
    //     let maxValue = Math.max(...this.statisticTable.get('yValues').value);
    //     let value = ((this.statisticTable.get('yValues').value[xIndex] * 100) / maxValue);
    //     return 100 - value;
    // }

    // GetMaxValue() {
    //     return Math.max(...this.statisticTable.get('yValues').value)
    // }


    // GetStatisticTable() {
    //     let table2: StatisticItem[] = [];

    //     this.statisticTable.get('xValues').value.forEach((xValue, xIndex) => {

    //         let item: StatisticItem = new StatisticItem();
    //         item.xValue = xValue;
    //         let color = this.getColor(xIndex);
    //         item.yArray = [];
    //         let yMainIndex = 0;
    //         for (let yIndex = 1; yIndex < this.GetColSpan(xIndex); yIndex++) {
    //             item.yArray.push(color);
    //             yMainIndex = yIndex;
    //         }

    //         for (let yIndex = yMainIndex; yIndex < 99; yIndex++) {
    //             item.yArray.push('');
    //         }
    //         table2.push(item);
    //     });
    //     return table2;
    // }

    // private befor: number = 0;
    // getColor(xIndex: number): string {
    //     if (this.befor < xIndex) {
    //         this.befor = xIndex + 1;
    //         return 'lightblue';
    //     } else {
    //         this.befor = xIndex;
    //         return '#DDBDF1';
    //     }
    // }

    // cycleStatistics() {
    //     for (let xIndex = 1; xIndex < 100; xIndex++) {
    //         this.xTable[xIndex] = [];
    //     }

    //     //  0 |
    //     //    |0
    //     for (let xIndex = 1; xIndex < 100; xIndex++) {
    //         for (let yIndex = 1; yIndex < 100; yIndex++) {
    //             if (xIndex < 50 && yIndex < 50)                 //  b |
    //                 this.xTable[xIndex][yIndex] = 'blue';       //    |

    //             if (xIndex > 50 && yIndex > 50)                 //    |
    //                 this.xTable[xIndex][yIndex] = 'purple';     //    | p

    //             if (xIndex < 50 && yIndex > 50)                 //    | r
    //                 this.xTable[xIndex][yIndex] = 'red';        //    |

    //             if (xIndex > 50 && yIndex < 50)                 //    |
    //                 this.xTable[xIndex][yIndex] = 'yellow';     //  y | 


    //             if (xIndex < 50 && yIndex < 50 && (yIndex > xIndex))
    //                 this.xTable[xIndex][yIndex] = 'pink';

    //             if (xIndex < 50 && yIndex > 50 && (yIndex + xIndex < 100))
    //                 this.xTable[xIndex][yIndex] = '#2db59c';

    //             if (xIndex > 50 && yIndex > 50 && (yIndex < xIndex))
    //                 this.xTable[xIndex][yIndex] = 'green';

    //             if (xIndex > 50 && yIndex < 50 && (yIndex + xIndex < 100))
    //                 this.xTable[xIndex][yIndex] = '#7469ab';


    //             if (xIndex < 50 && yIndex < 50 && xIndex + yIndex < 35)
    //                 this.xTable[xIndex][yIndex] = '';

    //             if (xIndex < 50 && yIndex > 50 && yIndex - xIndex > 65)
    //                 this.xTable[xIndex][yIndex] = '';

    //             if (xIndex > 50 && yIndex < 50 && xIndex - yIndex > 65)
    //                 this.xTable[xIndex][yIndex] = '';


    //             if (xIndex > 50 && yIndex > 50 && xIndex + yIndex > 165)
    //                 this.xTable[xIndex][yIndex] = '';



    //             // if (xIndex == yIndex)                           //  b |
    //             //     this.xTable[xIndex][yIndex] = 'black';      //    | b

    //             // if (xIndex + yIndex == 100)                     //    | b
    //             //     this.xTable[xIndex][yIndex] = 'black';      //  b | 

    //             // if (xIndex + yIndex == 50)                      //  b | 
    //             //     this.xTable[xIndex][yIndex] = 'black';      //    | 

    //             // if (xIndex - yIndex == -50)                     //    | b 
    //             //     this.xTable[xIndex][yIndex] = 'black';      //    | 

    //             // if (xIndex - yIndex == 50)                      //    |  
    //             //     this.xTable[xIndex][yIndex] = 'black';      //  b | 

    //             // if (xIndex + yIndex == 150)                     //    | 
    //             //     this.xTable[xIndex][yIndex] = 'black';      //    | b
    //         }

    //         this.xTable[50][xIndex] = 'black'           // ___
    //         this.xTable[xIndex][50] = 'black'           //  |
    //     }


    // }
 }


// export class StatisticItem {
//     xValue: string
//     yArray: string[];
// }

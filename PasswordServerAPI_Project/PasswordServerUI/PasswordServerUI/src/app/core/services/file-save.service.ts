import { HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable()
export class FileSaveService {
    constructor(
    ) { }

    saveBlob(response: HttpResponse<any>) {
        if (window.navigator.msSaveBlob)
            window.navigator.msSaveBlob(response.body, this.getFileName(response));
        else {
            var a;
            a = document.createElement("a");
            a.href = window.URL.createObjectURL(response.body);
            a.download = "Report";//this.getFileName(response);
            document.body.appendChild(a);
            a.click();
            document.body.removeChild(a);
        }
    }

    private getFileName(response: HttpResponse<any>) {
        return response.headers.get('content-disposition').split(';')[1].split('filename')[1].split('=')[1].trim();
    }

    handleError(error: any) {
        const reader = new FileReader();
        reader.addEventListener('loadend', (e) => {
            const jsonResponse = JSON.parse(e.srcElement['result']);
        });
        reader.readAsText(error.value.body);
    }
} 
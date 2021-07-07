import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class FileHandlerService {
  constructor() {}

  downloadExcel(blobResponse: any, filename: string) {
    this.download(
      blobResponse,
      filename,
      'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
    );
  }

  download(blobResponse: any, filename: string, contentType: string) {
    const ieEDGE = navigator.userAgent.match(/Edge/g);
    const ie = navigator.userAgent.match(/.NET/g); // IE 11+
    const oldIE = navigator.userAgent.match(/MSIE/g);

    const blob = new window.Blob([blobResponse], { type: contentType });

    if (ie || oldIE || ieEDGE) {
      window.navigator.msSaveBlob(blob, filename);
    } else {
      const element = document.createElement('a');
      element.href = URL.createObjectURL(blob);
      element.download = filename;
      document.body.appendChild(element);
      element.click();
    }
  }
}

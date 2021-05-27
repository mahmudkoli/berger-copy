import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FileItem, FileUploader } from 'ng2-file-upload';
import { FileHandlerService } from 'src/app/Shared/Services/file-handler/file-handler.service';

@Component({
  selector: 'app-file-uploader',
  templateUrl: './file-uploader.component.html',
  styleUrls: ['./file-uploader.component.css'],
})
export class FileUploaderComponent implements OnInit {
  @Input() uploader: FileUploader;
  public hasBaseDropZoneOver = false;
  public hasAnotherDropZoneOver = false;
  public uploadMessage = 'Upload in progress ... ';
  public errorMessage = '';
  public title = '';
  public status = false;
  constructor(
    private activeModal: NgbActiveModal,
    private fileHandlerService: FileHandlerService
  ) {}

  ngOnInit() {
    this.uploader.uploadAll();
    this.uploader.onCompleteAll = () => {};
    this.uploader.onSuccessItem = (
      item: any,
      response: any,
      status: number,
      headers: any
    ): any => {
      if (response) {
        const responseObj = JSON.parse(response);
        this.status = responseObj.status;
        this.errorMessage = responseObj.message;
        this.uploadMessage = 'Upload completed.';
        if (responseObj.file) {
          const binary = atob(responseObj.file);
          const array = new Uint8Array(binary.length);
          for (let i = 0; i < binary.length; i++) {
            array[i] = binary.charCodeAt(i);
          }
          this.fileHandlerService.downloadExcel(
            array,
            responseObj.fileName + '.xlsx'
          );
        }
      }
    };
    this.uploader.onErrorItem = (
      item: FileItem,
      response: string,
      status: number,
      headers: any
    ): any => {
      if (response) {
        const responseObj = JSON.parse(response);
        this.uploadMessage = 'Upload failed.';
        this.errorMessage = responseObj.message;
        item.progress = 100;
      }
    };
  }

  public fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  public fileOverAnother(e: any): void {
    this.hasAnotherDropZoneOver = e;
  }
  close() {
    this.uploader.clearQueue();
    this.activeModal.close('close');
  }
}

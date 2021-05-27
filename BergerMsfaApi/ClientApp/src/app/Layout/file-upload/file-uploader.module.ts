import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { NgbModalModule } from '@ng-bootstrap/ng-bootstrap';
import { FileUploadModule } from 'ng2-file-upload';
import { FileUploaderComponent } from './file-uploader/file-uploader.component';



@NgModule({
imports: [NgbModalModule,FileUploadModule,CommonModule ],
declarations: [FileUploaderComponent],
entryComponents: [FileUploaderComponent],
exports:[FileUploaderComponent,FileUploadModule ]
})
export class FileUploaderModule { }

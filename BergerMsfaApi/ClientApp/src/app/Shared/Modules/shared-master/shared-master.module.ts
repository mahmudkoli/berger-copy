import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PageTitleComponent } from 'src/app/Layout/LayoutComponent/Components/page-title/page-title.component';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { AlertModule } from '../alert/alert.module';
import { PTableModule } from '../p-table/p-table.module';
import { ImageUploaderComponent } from 'src/app/Shared/Modules/image-uploader/image-upload.component';

@NgModule({
  declarations: [
    PageTitleComponent,
    ImageUploaderComponent
  ],
  imports: [
    CommonModule,
    AngularFontAwesomeModule,
    AlertModule,
    PTableModule
  ],
  exports:[PageTitleComponent,
    ImageUploaderComponent, 
    AlertModule,
    PTableModule
  ]
})
export class SharedMasterModule { }

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PageTitleComponent } from 'src/app/Layout/LayoutComponent/Components/page-title/page-title.component';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { AlertModule } from '../alert/alert.module';
import { PTableModule } from '../p-table/p-table.module';
import { ImageUploaderComponent } from 'src/app/Shared/Modules/image-uploader/image-upload.component';
//import { TableModule } from 'primeng-lts/table'
import { TableModule } from 'primeng/table'
import { ButtonModule } from 'primeng-lts/button'

@NgModule({
  declarations: [
    PageTitleComponent,
    ImageUploaderComponent
  ],
  imports: [
    CommonModule,
    AngularFontAwesomeModule,
    AlertModule,
    PTableModule,
    TableModule,
    ButtonModule
  ],
  exports:[PageTitleComponent,
     ImageUploaderComponent, 
      AlertModule,
      PTableModule,
    
      ButtonModule,
      TableModule,
  ]
})
export class SharedMasterModule { }

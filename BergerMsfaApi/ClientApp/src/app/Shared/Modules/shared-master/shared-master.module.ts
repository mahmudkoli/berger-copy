import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FullCalendarModule } from '@fullcalendar/angular';
import dayGridPlugin from '@fullcalendar/daygrid';
import interactionPlugin from '@fullcalendar/interaction';
import timeGridPlugin from '@fullcalendar/timegrid';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { ButtonModule } from 'primeng-lts/button';
import { PaginatorModule } from 'primeng/paginator';
import { TableModule } from 'primeng/table';
//import { TableModule } from 'primeng-lts/table'
import { ToggleButtonModule } from 'primeng/togglebutton';
import { FileUploaderModule } from 'src/app/Layout/file-upload/file-uploader.module';
import { PageTitleComponent } from 'src/app/Layout/LayoutComponent/Components/page-title/page-title.component';
import { ImageUploaderComponent } from 'src/app/Shared/Modules/image-uploader/image-upload.component';
import { NumberFormatColorDirective } from '../../Directive/number-format-color.directive';
import { AlertModule } from '../alert/alert.module';
import { ImageViewerModule } from '../image-viewer/image-viewer.module';
import { PTableModule } from '../p-table/p-table.module';
import { SearchOptionModule } from '../search-option/search-option.module';

FullCalendarModule.registerPlugins([
    dayGridPlugin,
    timeGridPlugin,
    interactionPlugin
  ])



@NgModule({
    declarations: [
        PageTitleComponent,
        ImageUploaderComponent,
        NumberFormatColorDirective
    ],
    imports: [
        CommonModule,
        AngularFontAwesomeModule,
        AlertModule,
        PTableModule,
        TableModule,
        PaginatorModule,
        ButtonModule,
        ToggleButtonModule,
        FullCalendarModule,
        SearchOptionModule,
        FileUploaderModule,
        ImageViewerModule
    ],
    exports: [PageTitleComponent,
        ImageUploaderComponent,
        AlertModule,
        PTableModule,
        PaginatorModule,
        ButtonModule,
        TableModule,
        ToggleButtonModule,
        FullCalendarModule,
        SearchOptionModule,
        FileUploaderModule,
        NumberFormatColorDirective,
        ImageViewerModule
    ],
})
export class SharedMasterModule { }

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';

import { PTableComponent } from './p-table.component';

import { MakeDraggable, MakeDroppable, Draggable } from './drag-drop-service/drag.n.drop';
import { PDFService } from './service/pdf.service';
import { ExcelService } from './service/excel.service';
import { PrintService } from './service/print.service';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';


@NgModule({
    imports: [CommonModule, RouterModule,FormsModule,NgbModule],
    declarations: [PTableComponent,MakeDraggable, MakeDroppable, Draggable],
    exports: [PTableComponent],
    providers:[PDFService,ExcelService,PrintService]
})
export class PTableModule {}

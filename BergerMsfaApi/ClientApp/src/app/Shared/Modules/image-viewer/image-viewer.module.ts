import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ImageWithDeleteButtonComponent } from './image-with-delete-button/image-with-delete-button.component';



@NgModule({
  declarations: [ ImageWithDeleteButtonComponent],
  imports: [
    CommonModule
  ],
  exports:[ImageWithDeleteButtonComponent]
})
export class ImageViewerModule { }

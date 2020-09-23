import { NgModule } from '@angular/core';
import { UniqueFilterByPipe } from './unique-filter.pipe';

@NgModule({
  declarations: [
    UniqueFilterByPipe
  ],
  imports: [
  ],
  // providers: [
  //   UniqueFilterByPipe
  // ],
  exports: [
    UniqueFilterByPipe
  ]
})
export class PipeSharedModule { }

import { NgModule } from '@angular/core';
import { UniqueFilterByPipe } from './unique-filter.pipe';
import { StatusPipe } from './status-filter.pipe';

@NgModule({
  declarations: [
        UniqueFilterByPipe,
        StatusPipe
  ],
  imports: [
  ],
  // providers: [
  //   UniqueFilterByPipe
  // ],
  exports: [
      UniqueFilterByPipe,
      StatusPipe,

  ]
})
export class PipeSharedModule { }

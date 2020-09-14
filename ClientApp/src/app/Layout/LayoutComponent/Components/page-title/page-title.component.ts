import {Component, Input, Output, EventEmitter} from '@angular/core';

@Component({
  selector: 'app-page-title',
  templateUrl: './page-title.component.html',
})
export class PageTitleComponent {

  @Input() heading;
  @Input() subheading;
  @Input() icon;
  @Output() routeToCreateNew = new EventEmitter();

  createNew(): void {
    // console.log(path);
    this.routeToCreateNew.emit();
	}
}

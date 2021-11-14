import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-image-with-delete-button',
  templateUrl: './image-with-delete-button.component.html',
  styleUrls: ['./image-with-delete-button.component.css'],
})
export class ImageWithDeleteButtonComponent implements OnInit {
  constructor() {}
  @Input() imageUrl = '';
  @Output() deleteEvenet = new EventEmitter<string>();
  @Input() refObject: any;
  ngOnInit() {}

  delete() {
    this.deleteEvenet.next(this.refObject);
  }
}

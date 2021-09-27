import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ImageWithDeleteButtonComponent } from './image-with-delete-button.component';

describe('ImageWithDeleteButtonComponent', () => {
  let component: ImageWithDeleteButtonComponent;
  let fixture: ComponentFixture<ImageWithDeleteButtonComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ImageWithDeleteButtonComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ImageWithDeleteButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

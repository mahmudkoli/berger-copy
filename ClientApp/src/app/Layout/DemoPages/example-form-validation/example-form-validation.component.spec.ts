import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ExampleFormValidationComponent } from './example-form-validation.component';

describe('ExampleFormValidationComponent', () => {
  let component: ExampleFormValidationComponent;
  let fixture: ComponentFixture<ExampleFormValidationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ExampleFormValidationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ExampleFormValidationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

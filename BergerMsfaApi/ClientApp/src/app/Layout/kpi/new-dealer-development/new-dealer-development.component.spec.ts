import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewDealerDevelopmentComponent } from './new-dealer-development.component';

describe('NewDealerDevelopmentComponent', () => {
  let component: NewDealerDevelopmentComponent;
  let fixture: ComponentFixture<NewDealerDevelopmentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewDealerDevelopmentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewDealerDevelopmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

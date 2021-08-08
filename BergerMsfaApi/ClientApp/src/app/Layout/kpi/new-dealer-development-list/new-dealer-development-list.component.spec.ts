import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewDealerDevelopmentListComponent } from './new-dealer-development-list.component';

describe('NewDealerDevelopmentListComponent', () => {
  let component: NewDealerDevelopmentListComponent;
  let fixture: ComponentFixture<NewDealerDevelopmentListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewDealerDevelopmentListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewDealerDevelopmentListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DealerSalesCallEditComponent } from './dealer-sales-call-edit.component';

describe('DealerSalesCallEditComponent', () => {
  let component: DealerSalesCallEditComponent;
  let fixture: ComponentFixture<DealerSalesCallEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DealerSalesCallEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DealerSalesCallEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

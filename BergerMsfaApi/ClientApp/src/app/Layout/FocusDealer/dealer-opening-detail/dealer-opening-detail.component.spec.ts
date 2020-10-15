import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DealerOpeningDetailComponent } from './dealer-opening-detail.component';

describe('DealerOpeningDetailComponent', () => {
  let component: DealerOpeningDetailComponent;
  let fixture: ComponentFixture<DealerOpeningDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DealerOpeningDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DealerOpeningDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DealerOpeningListComponent } from './dealer-opening-list.component';

describe('DealerOpeningListComponent', () => {
  let component: DealerOpeningListComponent;
  let fixture: ComponentFixture<DealerOpeningListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DealerOpeningListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DealerOpeningListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

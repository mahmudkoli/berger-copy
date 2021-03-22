import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BrandInfoLogDetailsComponent } from './brand-info-log-details.component';

describe('BrandInfoLogDetailsComponent', () => {
  let component: BrandInfoLogDetailsComponent;
  let fixture: ComponentFixture<BrandInfoLogDetailsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BrandInfoLogDetailsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BrandInfoLogDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

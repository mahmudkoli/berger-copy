import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmailConfigForDealerOppeningComponent } from './email-config-for-dealer-oppening.component';

describe('EmailConfigForDealerOppeningComponent', () => {
  let component: EmailConfigForDealerOppeningComponent;
  let fixture: ComponentFixture<EmailConfigForDealerOppeningComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmailConfigForDealerOppeningComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmailConfigForDealerOppeningComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

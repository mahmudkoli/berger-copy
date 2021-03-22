import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmailConfigAddComponent } from './email-config-add.component';

describe('EmailConfigAddComponent', () => {
  let component: EmailConfigAddComponent;
  let fixture: ComponentFixture<EmailConfigAddComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmailConfigAddComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmailConfigAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

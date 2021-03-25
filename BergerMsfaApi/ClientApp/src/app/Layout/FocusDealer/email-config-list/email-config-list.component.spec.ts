import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmailConfigListComponent } from './email-config-list.component';

describe('EmailConfigListComponent', () => {
  let component: EmailConfigListComponent;
  let fixture: ComponentFixture<EmailConfigListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmailConfigListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmailConfigListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UserInfoInsertComponent } from './user-info-insert.component';

describe('UserInfoInsertComponent', () => {
  let component: UserInfoInsertComponent;
  let fixture: ComponentFixture<UserInfoInsertComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UserInfoInsertComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserInfoInsertComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

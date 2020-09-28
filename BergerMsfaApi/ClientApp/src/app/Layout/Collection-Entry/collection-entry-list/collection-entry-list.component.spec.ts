import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CollectionEntryListComponent } from './collection-entry-list.component';

describe('CollectionEntryListComponent', () => {
  let component: CollectionEntryListComponent;
  let fixture: ComponentFixture<CollectionEntryListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CollectionEntryListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CollectionEntryListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

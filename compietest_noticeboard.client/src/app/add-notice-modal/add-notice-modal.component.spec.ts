import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddNoticeModalComponent } from './add-notice-modal.component';

describe('AddNoticeModalComponent', () => {
  let component: AddNoticeModalComponent;
  let fixture: ComponentFixture<AddNoticeModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AddNoticeModalComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddNoticeModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

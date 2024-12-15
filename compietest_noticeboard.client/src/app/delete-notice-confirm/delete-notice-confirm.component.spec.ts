import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeleteNoticeConfirmComponent } from './delete-notice-confirm.component';

describe('DeleteNoticeConfirmComponent', () => {
  let component: DeleteNoticeConfirmComponent;
  let fixture: ComponentFixture<DeleteNoticeConfirmComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [DeleteNoticeConfirmComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DeleteNoticeConfirmComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

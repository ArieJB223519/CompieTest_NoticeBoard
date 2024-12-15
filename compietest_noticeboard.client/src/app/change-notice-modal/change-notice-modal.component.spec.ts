import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChangeNoticeModalComponent } from './change-notice-modal.component';

describe('ChangeNoticeComponent', () => {
  let component: ChangeNoticeModalComponent;
  let fixture: ComponentFixture<ChangeNoticeModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ChangeNoticeModalComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ChangeNoticeModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

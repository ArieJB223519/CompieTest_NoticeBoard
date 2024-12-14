import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChangeNoticeComponent } from './change-notice.component';

describe('ChangeNoticeComponent', () => {
  let component: ChangeNoticeComponent;
  let fixture: ComponentFixture<ChangeNoticeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ChangeNoticeComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ChangeNoticeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

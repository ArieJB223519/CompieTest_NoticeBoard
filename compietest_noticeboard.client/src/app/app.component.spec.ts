import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AppComponent } from './app.component';

describe('AppComponent', () => {
  let component: AppComponent;
  let fixture: ComponentFixture<AppComponent>;
  let httpMock: HttpTestingController;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AppComponent],
      imports: [HttpClientTestingModule]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AppComponent);
    component = fixture.componentInstance;
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should create the app', () => {
    expect(component).toBeTruthy();
  });

  it('should retrieve weather forecasts from the server', () => {
    const mockNoticeBoards = [
      { title: 'ku-ku', content: 'fdsdf', createDate: '2024-12-13', updateDate: '2024-12-13' },
      { title: 'ku-ku-1', content: 'fdsdf-1', createDate: '2024-12-13', updateDate: '2024-12-13' }
    ];

    component.ngOnInit();

    const req = httpMock.expectOne('/noticeBoard');
    expect(req.request.method).toEqual('GET');
    req.flush(mockNoticeBoards);

    expect(component.noticeBoards).toEqual(mockNoticeBoards);
  });
});

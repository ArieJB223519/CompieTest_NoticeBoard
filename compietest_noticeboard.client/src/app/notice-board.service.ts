import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface NoticeBoard {
  id: number;
  title: string;
  content: string;
  createDate: string;
  updateDate: string;
}

@Injectable({
  providedIn: 'root'
})
export class NoticeBoardService {
  constructor(private http: HttpClient) { }

  getNoticeBoardsAll(): Observable<NoticeBoard[]> {
    return this.http.get<NoticeBoard[]>('/noticeBoard');
  }

  addNotice(item: NoticeBoard): Observable<NoticeBoard> {
    return this.http.post<NoticeBoard>('/noticeBoard', item);
  }
}

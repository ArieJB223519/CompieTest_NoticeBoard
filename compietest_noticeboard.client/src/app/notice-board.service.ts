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
  private baseUrl = 'https://127.0.0.1:57476/noticeBoard';
  constructor(private http: HttpClient) { }

  updateNotice(item: NoticeBoard): Observable<NoticeBoard> {
    return this.http.put<NoticeBoard>(this.baseUrl + "/" + item.id, item);
  }

  addNotice(item: NoticeBoard): Observable<NoticeBoard> {
    return this.http.post<NoticeBoard>(this.baseUrl, item);
  }

  getNoticeBoardsAll(): Observable<NoticeBoard[]> {
    return this.http.get<NoticeBoard[]>(this.baseUrl);
  }
}

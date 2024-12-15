import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AddNoticeModalComponent } from './add-notice-modal/add-notice-modal.component';
import { ChangeNoticeModalComponent } from './change-notice-modal/change-notice-modal.component';
import { NoticeBoardService, NoticeBoard } from './notice-board.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  standalone: true,
  imports: [CommonModule]
})
export class AppComponent implements OnInit {
  public noticeBoards: NoticeBoard[] = [];

  constructor(private noticeBoardService: NoticeBoardService, private dialog: MatDialog) { }

  ngOnInit() {
    console.log('ngOnInit called');
    this.getNoticeBoards();
  }

  getNoticeBoards() {
    this.noticeBoardService.getNoticeBoardsAll().subscribe(
      (result: NoticeBoard[]) => {
        this.noticeBoards = result;
      },
      (error: any) => {
        console.error(error);
      }
    );
  }

  openChangeNoticeModal(notice: NoticeBoard): void {
    const dialogRef = this.dialog.open(ChangeNoticeModalComponent, {
      width: '250px',
      data: notice
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.updateNotice(result);
      }
    });
  }

  updateNotice(updatedNotice: NoticeBoard): void {
    this.noticeBoardService.updateNotice(updatedNotice).subscribe(
      response => {
        console.log('Notice updated successfully', response);
        this.getNoticeBoards(); 
      },
      error => {
        console.error('Error updating notice', error);
      }
    );
  }

  openAddNoticeModal() {
    const dialogRef = this.dialog.open(AddNoticeModalComponent);

    dialogRef.afterClosed().subscribe((result: NoticeBoard) => {
      if (result) {
        this.noticeBoards.push(result);
      }
    });
  }
}

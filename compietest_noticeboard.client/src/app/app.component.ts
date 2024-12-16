import { Component, OnInit, ChangeDetectorRef, AfterViewInit, ElementRef, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AddNoticeModalComponent } from './add-notice-modal/add-notice-modal.component';
import { ChangeNoticeModalComponent } from './change-notice-modal/change-notice-modal.component';
import { DeleteNoticeConfirmComponent } from './delete-notice-confirm/delete-notice-confirm.component';
import { NoticeBoardService, NoticeBoard } from './notice-board.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  standalone: true,
  imports: [CommonModule]
})
export class AppComponent implements OnInit, AfterViewInit {
  public noticeBoards: NoticeBoard[] = [];
  public filteredNoticeBoards: NoticeBoard[] = [];
  public searchQuery: string = '';

  @ViewChild('searchInput') searchInputElement!: ElementRef;

  constructor(
    private noticeBoardService: NoticeBoardService,
    private dialog: MatDialog,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit() {
    console.log('ngOnInit called');
    this.getNoticeBoards();
  }

  ngAfterViewInit() {
    this.searchInputElement.nativeElement.focus();
  }

  getNoticeBoards() {
    this.noticeBoardService.getNoticeBoardsAll().subscribe(
      (result: NoticeBoard[]) => {
        this.noticeBoards = result;
        this.filteredNoticeBoards = result;
        this.cdr.detectChanges();
      },
      (error: any) => {
        console.error(error);
      }
    );
  }

  openChangeNoticeModal(notice: NoticeBoard): void {
    const dialogRef = this.dialog.open(ChangeNoticeModalComponent, {
      data: notice,
      width: "800px"
    });

    dialogRef.afterClosed().subscribe(
      (result: NoticeBoard[]) => {
        if (result) {
          this.noticeBoards = result;
          this.applyFilter(this.searchQuery);
          this.cdr.detectChanges();
        }
      }
    );
  }

  openAddNoticeModal() {
    const dialogRef = this.dialog.open(AddNoticeModalComponent, {
      width: "800px"
    });

    dialogRef.afterClosed().subscribe(
      (result: NoticeBoard[]) => {
        if (result) {
          this.noticeBoards = result;
          this.applyFilter(this.searchQuery);
          this.cdr.detectChanges();
        }
      }
    );
  }

  openDeleteNoticeModal(id: number): void {
    const dialogRef = this.dialog.open(DeleteNoticeConfirmComponent, {
      data: { id: id },
      width: "800px"
    });

    dialogRef.componentInstance.noticeId = id;

    dialogRef.afterClosed().subscribe(
      (result: boolean) => {
        if (result) {
          this.noticeBoards = this.noticeBoards.filter(notice => notice.id !== id);
          this.applyFilter(this.searchQuery);
          this.cdr.detectChanges();
        }
      }
    );
  }

  applyFilter(query: string) {
    this.searchQuery = query;

    if (query.length >= 3) {
      this.filteredNoticeBoards = this.noticeBoards.filter(item => item.title.toLowerCase().includes(query.toLowerCase()) || item.content.toLowerCase().includes(query.toLowerCase()));
    } else {
      this.filteredNoticeBoards = this.noticeBoards;
    }

    this.cdr.detectChanges();
  }

  handleKeyDown(event: KeyboardEvent) {
    if (event.key === 'Enter') {
      var searchSpan = (<HTMLInputElement>event.target).value;
      this.applyFilter(searchSpan);
    }
  }
}

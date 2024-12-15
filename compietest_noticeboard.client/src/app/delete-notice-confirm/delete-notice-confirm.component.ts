import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { NoticeBoardService, NoticeBoard } from '../notice-board.service';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-delete-notice-confirm',
  templateUrl: './delete-notice-confirm.component.html',
  styleUrl: './delete-notice-confirm.component.css',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule
  ]
})
export class DeleteNoticeConfirmComponent {
  noticeId: number;
  isLoading = false;

  constructor(
    public dialogRef: MatDialogRef<DeleteNoticeConfirmComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private noticeBoardService: NoticeBoardService
  ) {
    this.noticeId = data.id;
  }

  onCancel(): void {
    this.dialogRef.close();
  }

  onConfirm(): void {
    this.isLoading = true;

    this.noticeBoardService.deleteNotice(this.noticeId).subscribe(
      response => {
        console.log('Item deleted successfully with HHTP method DELETE', response);
        this.dialogRef.close(response);
      },
      error => {
        console.error('Error deleting item', error);
      }
    ).add(() => {
      this.isLoading = false;
    });
  }
}

import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { NoticeBoardService, NoticeBoard } from '../notice-board.service';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-change-notice-modal',
  templateUrl: './change-notice-modal.component.html',
  styleUrls: ['./change-notice-modal.component.css'],
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

export class ChangeNoticeModalComponent {
  noticeBoardForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private noticeBoardService: NoticeBoardService,
    private dialogRef: MatDialogRef<ChangeNoticeModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.noticeBoardForm = this.fb.group({
      id: [data.id],
      title: [data.title, Validators.required],
      content: [data.content, Validators.required],
      createDate: [data.createDate, Validators.required],
      updateDate: [new Date().toISOString(), Validators.required]
    });
  }

  onCancel(): void {
    this.dialogRef.close();
  }

  onSubmit(): void {
    if (this.noticeBoardForm.valid) {
      const updatedItem: NoticeBoard = this.noticeBoardForm.value;
      this.noticeBoardService.updateNotice(updatedItem).subscribe(
        response => {
          console.log('Item updated successfully', response);
          this.dialogRef.close(response);
        },
        error => {
          console.error('Error updating item', error);
        }
      );
    }
  }
}

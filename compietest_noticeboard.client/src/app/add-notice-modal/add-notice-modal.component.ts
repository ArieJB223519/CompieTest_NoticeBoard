import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatDialogRef, MatDialogModule } from '@angular/material/dialog';
import { NoticeBoardService, NoticeBoard } from '../notice-board.service';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-add-notice-modal',
  templateUrl: './add-notice-modal.component.html',
  styleUrls: ['./add-notice-modal.component.css'],
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

export class AddNoticeModalComponent {
  noticeBoardForm: FormGroup;
  selectedFile: File | null = null;

  constructor(
    private fb: FormBuilder,
    private noticeBoardService: NoticeBoardService,
    private dialogRef: MatDialogRef<AddNoticeModalComponent>
  ) {
    this.noticeBoardForm = this.fb.group({
      id: 0,
      title: ['', Validators.required],
      content: ['', Validators.required],

      createDate: [new Date().toISOString().split('T')[0], Validators.required],
      updateDate: [new Date().toISOString().split('T')[0], Validators.required]
    });
  }

  onFileSelected(event: any): void {
    this.selectedFile = event.target.files[0] || null;
  }
  onCancel(): void {
    this.dialogRef.close();
  }

  onSubmit(): void {
    if (this.noticeBoardForm.valid && this.selectedFile) {
      const formData = new FormData();
      formData.append('title', this.noticeBoardForm.get('title')?.value);
      formData.append('content', this.noticeBoardForm.get('content')?.value);
      formData.append('image', this.selectedFile);

      this.noticeBoardService.addNotice(formData).subscribe(
        response => {
          console.log('Item added successfully', response);
          this.dialogRef.close(response);
        },
        error => {
          console.error('Error adding item', error);
        }
      );
    }
  

  

    /*
    if (this.noticeBoardForm.valid) {
      const newItem: NoticeBoard = this.noticeBoardForm.value;
      this.noticeBoardService.addNotice(newItem).subscribe(
        response => {
          console.log('Item added successfully', response);
          this.dialogRef.close(response);
        },
        error => {
          console.error('Error adding item', error);
        }
      );
    }
    */
  }
}

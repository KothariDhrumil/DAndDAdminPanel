import {
  MAT_DIALOG_DATA,
  MatDialogRef,
  MatDialogContent,
  MatDialogClose,
} from '@angular/material/dialog';
import { Component, Inject } from '@angular/core';
import {
  UntypedFormControl,
  Validators,
  UntypedFormGroup,
  UntypedFormBuilder,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { formatDate } from '@angular/common';
import { MyTasksService } from '../../my-tasks.service';
import { MyTasks } from '../../my-tasks.model';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatOptionModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

export interface DialogData {
  id: number;
  action: string;
  myTasks: MyTasks;
}

@Component({
    selector: 'app-my-tasks-form',
    templateUrl: './form-dialog.component.html',
    styleUrls: ['./form-dialog.component.scss'],
    imports: [
        MatButtonModule,
        MatIconModule,
        MatDialogContent,
        FormsModule,
        ReactiveFormsModule,
        MatFormFieldModule,
        MatInputModule,
        MatSelectModule,
        MatOptionModule,
        MatDatepickerModule,
        MatDialogClose,
    ]
})
export class MyTasksFormComponent {
  action: string;
  dialogTitle: string;
  myTasksForm: UntypedFormGroup;
  myTasks: MyTasks;

  constructor(
    public dialogRef: MatDialogRef<MyTasksFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    public myTasksService: MyTasksService,
    private fb: UntypedFormBuilder
  ) {
    // Set the defaults
    this.action = data.action;
    this.dialogTitle =
      this.action === 'edit' ? data.myTasks.taskNo : 'New Task';
    this.myTasks =
      this.action === 'edit' ? data.myTasks : new MyTasks({} as MyTasks);

    // Create the form
    this.myTasksForm = this.createMyTasksForm();
  }

  // Create the form with validation
  private createMyTasksForm(): UntypedFormGroup {
    return this.fb.group({
      id: [this.myTasks.id],
      taskNo: [this.myTasks.taskNo, Validators.required],
      project: [this.myTasks.project, Validators.required],
      client: [this.myTasks.client, Validators.required],
      status: [this.myTasks.status, Validators.required],
      priority: [this.myTasks.priority, Validators.required],
      type: [this.myTasks.type, Validators.required],
      executor: [this.myTasks.executor, Validators.required],
      date: [
        formatDate(this.myTasks.date, 'yyyy-MM-dd', 'en'),
        [Validators.required],
      ],
      details: [this.myTasks.details, Validators.required],
    });
  }

  // Display error messages for validation
  getErrorMessage(controlName: string): string {
    const control = this.myTasksForm.get(controlName);
    if (control?.hasError('required')) {
      return 'Required field';
    }
    return '';
  }

  // Submit the form
  submit(): void {
    if (this.myTasksForm.valid) {
      const taskData = this.myTasksForm.getRawValue();
      if (this.action === 'edit') {
        this.myTasksService.updateMyTasks(taskData).subscribe({
          next: (response) => this.dialogRef.close(response),
          error: (error) => console.error('Update Error:', error),
        });
      } else {
        this.myTasksService.addMyTasks(taskData).subscribe({
          next: (response) => this.dialogRef.close(response),
          error: (error) => console.error('Add Error:', error),
        });
      }
    }
  }

  // Close the dialog without making changes
  onNoClick(): void {
    this.dialogRef.close();
  }

  // Confirm adding or editing of the task
  public confirmAdd(): void {
    this.submit();
  }
}

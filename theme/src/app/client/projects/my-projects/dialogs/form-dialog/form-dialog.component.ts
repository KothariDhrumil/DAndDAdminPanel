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
import { MyProjectsService } from '../../my-projects.service';
import { MyProjects } from '../../my-projects.model';
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
  myProjects: MyProjects;
}

@Component({
    selector: 'app-my-projects-form',
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
export class MyProjectsFormComponent {
  action: string;
  dialogTitle: string;
  myProjectsForm: UntypedFormGroup;
  myProjects: MyProjects;

  constructor(
    public dialogRef: MatDialogRef<MyProjectsFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    public myProjectsService: MyProjectsService,
    private fb: UntypedFormBuilder
  ) {
    // Set the defaults based on action (new or edit)
    this.action = data.action;
    this.dialogTitle =
      this.action === 'edit' ? data.myProjects.pName : 'New Project';
    this.myProjects =
      this.action === 'edit'
        ? data.myProjects
        : new MyProjects({} as MyProjects);

    // Initialize the form
    this.myProjectsForm = this.createMyProjectsForm();
  }

  // Create the form structure with validation
  private createMyProjectsForm(): UntypedFormGroup {
    return this.fb.group({
      id: [this.myProjects.id],
      pName: [this.myProjects.pName, Validators.required],
      type: [this.myProjects.type, Validators.required],
      open_task: [this.myProjects.open_task],
      last_modify: [
        formatDate(this.myProjects.last_modify, 'yyyy-MM-dd', 'en'),
        [Validators.required],
      ],
      create_date: [
        formatDate(this.myProjects.create_date, 'yyyy-MM-dd', 'en'),
        [Validators.required],
      ],
      status: [this.myProjects.status, Validators.required],
      lead_name: [this.myProjects.lead_name, Validators.required],
    });
  }

  // Handle error messages for form fields
  getErrorMessage(controlName: string): string {
    const control = this.myProjectsForm.get(controlName);
    if (control?.hasError('required')) {
      return 'Required field';
    }
    return '';
  }

  // Handle form submission
  submit(): void {
    if (this.myProjectsForm.valid) {
      const projectData = this.myProjectsForm.getRawValue();
      if (this.action === 'edit') {
        this.myProjectsService.updateMyProjects(projectData).subscribe({
          next: (response) => this.dialogRef.close(response),
          error: (error) => console.error('Update Error:', error),
        });
      } else {
        this.myProjectsService.addMyProjects(projectData).subscribe({
          next: (response) => this.dialogRef.close(response),
          error: (error) => console.error('Add Error:', error),
        });
      }
    }
  }

  // Close the dialog without any actions
  onNoClick(): void {
    this.dialogRef.close();
  }

  // Trigger form submission upon confirmation
  public confirmAdd(): void {
    this.submit();
  }
}

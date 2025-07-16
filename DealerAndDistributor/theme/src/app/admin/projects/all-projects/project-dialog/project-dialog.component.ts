import { Component, Inject } from '@angular/core';
import {
  UntypedFormBuilder,
  Validators,
  UntypedFormGroup,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';

import { MatSnackBar } from '@angular/material/snack-bar';
import {
  MatDialogRef,
  MAT_DIALOG_DATA,
  MatDialogContent,
  MatDialogClose,
} from '@angular/material/dialog';

import {
  Project,
  ProjectStatus,
  ProjectPriority,
  ProjectType,
} from '../core/project.model';
import { ProjectService } from '../core/project.service';
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
  title: string;
  project: Project;
}

@Component({
    selector: 'app-project-dialog',
    templateUrl: './project-dialog.component.html',
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
export class ProjectDialogComponent {
  public project: Project;
  public dialogTitle: string;
  public projectForm: UntypedFormGroup;
  public statusChoices: typeof ProjectStatus;
  public priorityChoices: typeof ProjectPriority;
  public projectType: typeof ProjectType;

  constructor(
    private formBuilder: UntypedFormBuilder,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    public dialogRef: MatDialogRef<ProjectDialogComponent>,
    private snackBar: MatSnackBar,
    private projectService: ProjectService
  ) {
    this.dialogTitle = data.title;
    this.project = data.project;
    this.statusChoices = ProjectStatus;
    this.priorityChoices = ProjectPriority;
    this.projectType = ProjectType;

    const nonWhiteSpaceRegExp = new RegExp('\\S');

    this.projectForm = this.formBuilder.group({
      name: [
        this.project?.name,
        [Validators.required, Validators.pattern(nonWhiteSpaceRegExp)],
      ],
      status: [
        this.project ? this.project.status : this.statusChoices.NEWPROJECTS,
      ],
      description: [this.project?.description],
      deadline: [this.project?.deadline],
      priority: [
        this.project ? this.project.priority : this.priorityChoices.MEDIUM,
      ],
      open_task: [this.project?.open_task],
      type: [this.project ? this.project.type : this.projectType.WEB],
      created: [this.project?.created],
      team_leader: [this.project?.team_leader],
      progress: [this.project?.progress],
    });
  }

  public submit(): void {
    console.log('save');
    if (!this.projectForm.valid) {
      return;
    }
    if (this.project) {
      // update project object with form values
      Object.assign(this.project, this.projectForm.value);
      this.projectService.updateObject(this.project);
      this.snackBar.open('Project updated Successfully...!!!', '', {
        duration: 2000,
        verticalPosition: 'bottom',
        horizontalPosition: 'center',
        panelClass: 'black',
      });

      this.dialogRef.close();
    } else {
      this.projectService.createOject(this.projectForm.value);
      this.snackBar.open('Project created Successfully...!!!', '', {
        duration: 2000,
        verticalPosition: 'bottom',
        horizontalPosition: 'center',
        panelClass: 'black',
      });

      this.dialogRef.close();
    }
  }
  onNoClick(): void {
    this.dialogRef.close();
  }
}


import { Component } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BreadcrumbComponent } from '@shared/components/breadcrumb/breadcrumb.component';

@Component({
    selector: 'app-leave-settings',
    imports: [
        BreadcrumbComponent,
        MatCardModule,
        MatFormFieldModule,
        MatInputModule,
        MatSelectModule,
        MatDatepickerModule,
        MatCheckboxModule,
        MatButtonModule,
        ReactiveFormsModule
    ],
    templateUrl: './leave-settings.component.html',
    styleUrl: './leave-settings.component.scss'
})
export class LeaveSettingsComponent {
  leaveForm: FormGroup;

  constructor() {
    this.leaveForm = new FormGroup({
      leavePolicyName: new FormControl('', Validators.required),
      leaveType: new FormControl('', Validators.required),
      leaveDuration: new FormControl('', Validators.required),
      carryForwardLimit: new FormControl(0, [
        Validators.required,
        Validators.min(0),
      ]),
      leaveQuota: new FormControl(12, [Validators.required, Validators.min(1)]),
      department: new FormControl('', Validators.required),
      approvalWorkflow: new FormControl('Manager'),
      leaveEncashment: new FormControl(false),
      halfDayOption: new FormControl(false),
      notes: new FormControl(''),
    });
  }

  onSubmit() {
    if (this.leaveForm.valid) {
      console.log(this.leaveForm.value);
    } else {
      console.log('Form is invalid');
    }
  }
}

import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Sharding } from '../../models/sharding.model';
import { MatSelectModule } from "@angular/material/select";

@Component({
  selector: 'app-sharding-dialog',
  templateUrl: './sharding-dialog.component.html',
  styleUrls: ['./sharding-dialog.component.scss'],
  imports: [MatSelectModule,FormsModule,ReactiveFormsModule],
})
export class ShardingDialogComponent {
  form: FormGroup;
  isEdit: boolean;

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<ShardingDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { sharding?: Sharding }
  ) {
    this.isEdit = !!data.sharding;
    this.form = this.fb.group({
      name: [data.sharding?.name || '', Validators.required],
      databaseName: [data.sharding?.databaseName || '', Validators.required],
      connectionName: [data.sharding?.connectionName || '', Validators.required],
      databaseType: [data.sharding?.databaseType || '', Validators.required]
    });
  }

  submit(): void {
    if (this.form.valid) {
      this.dialogRef.close(this.form.value);
    }
  }

  cancel(): void {
    this.dialogRef.close();
  }
}

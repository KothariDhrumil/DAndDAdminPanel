import { ChangeDetectionStrategy, Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';

import { MatInputModule } from "@angular/material/input";
import { MatSelectModule } from "@angular/material/select";
import { MatFormFieldModule } from '@angular/material/form-field';
import { ShardingRequest } from '../../models/sharding.model';

@Component({
    selector: 'app-sharding-form-dialog',
    templateUrl: './form-dialog.component.html',
    styleUrls: ['./form-dialog.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
    imports: [MatInputModule, MatSelectModule, MatFormFieldModule, FormsModule, ReactiveFormsModule],
})
export class ShardingFormDialogComponent {
    form: FormGroup;
    action: 'add' | 'edit';

    constructor(
        private fb: FormBuilder,
        private dialogRef: MatDialogRef<ShardingFormDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: { item?: ShardingRequest; action: 'add' | 'edit' }
    ) {
        this.action = data.action;
        this.form = this.fb.group({
            name: [data.item?.name || '', Validators.required],
            databaseName: [data.item?.databaseName || '', Validators.required],
            connectionName: [data.item?.connectionName || '', Validators.required],
            databaseType: [data.item?.databaseType || 'SqlServer', Validators.required],
        });
    }

    submit() {
        if (this.form.valid) {
            this.dialogRef.close(this.form.value);
        }
    }

    cancel() {
        this.dialogRef.close();
    }
}

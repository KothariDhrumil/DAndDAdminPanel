import { Component, Inject } from '@angular/core';
import { signal } from '@angular/core';
import { ShardingService } from '../../service/sharding.service';
import { FormBuilder, FormGroup, FormControl, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogClose, MatDialogContent, MatDialogRef } from '@angular/material/dialog';
import { DbDetailsResponse, Sharding } from '../../models/sharding.model';
import { ApiResponse } from '../../../../../core/models/interface/ApiResponse';

import { MatSelectModule } from "@angular/material/select";
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from "@angular/material/icon";
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';

@Component({
    selector: 'app-sharding-dialog',
    templateUrl: './sharding-dialog.component.html',
    styleUrls: ['./sharding-dialog.component.scss'],
    imports: [
        MatButtonModule,
        MatIconModule,
        MatDialogContent,
        FormsModule,
        ReactiveFormsModule,
        MatFormFieldModule,
        MatSelectModule,
        MatInputModule,
        MatDialogClose,
    ],
})
export class ShardingDialogComponent {
    form: FormGroup;
    isEdit: boolean;
    errorMessage = signal<string>('');
    connectionNames = signal<string[]>([]);
    databaseTypes = signal<string[]>([]);

    constructor(
        private fb: FormBuilder,
        private dialogRef: MatDialogRef<ShardingDialogComponent>,
        private shardingService: ShardingService,
        @Inject(MAT_DIALOG_DATA) public data: { item?: Sharding }
    ) {
        this.isEdit = !!data.item;
        this.form = new FormGroup({
            name: new FormControl(data.item?.name ?? '', [Validators.required]),
            databaseName: new FormControl(data.item?.databaseName ?? '', [Validators.required]),
            connectionName: new FormControl(data.item?.connectionName ?? '', [Validators.required]),
            databaseType: new FormControl(data.item?.databaseType ?? '', [Validators.required])
        });
        this.errorMessage = signal<string>('');
        this.loadDbDetails();
    }

    loadDbDetails(): void {
        this.shardingService.getDbDetails().subscribe({
            next: (res: ApiResponse<DbDetailsResponse>) => {
                if (res.isSuccess && res.data) {
                    this.connectionNames.set(res.data.allPossibleConnectionNames ?? []);
                    this.databaseTypes.set(res.data.possibleDatabaseTypes ?? []);
                }
            },
            error: () => {
                this.connectionNames.set([]);
                this.databaseTypes.set([]);
            }
        });
    }

    submit(): void {
        if (this.form.valid) {
            const payload = this.form.value;
            const handleResponse = (res: any) => {
                if (res?.isSuccess) {
                    this.dialogRef.close(res);
                } else {
                    this.errorMessage.set(res?.error?.description || 'An error occurred.');
                }
            };
            if (this.isEdit) {
                this.shardingService.update(payload).subscribe({
                    next: handleResponse,
                    error: (err) => {
                        this.errorMessage.set(err?.error?.description || 'Internal server error.');
                    }
                });
            } else {
                this.shardingService.create(payload).subscribe({
                    next: handleResponse,
                    error: (err) => {
                        this.errorMessage.set(err?.error?.description || 'Internal server error.');
                    }
                });
            }
        }
    }

    cancel(): void {
        this.dialogRef.close();
    }
}

import {
  MAT_DIALOG_DATA,
  MatDialogRef,
  MatDialogContent,
  MatDialogClose,
} from '@angular/material/dialog';
import { Component, Inject } from '@angular/core';
import {
  UntypedFormGroup,
  UntypedFormBuilder,
  Validators,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { ClientsService } from '../../clients.service';
import { Clients } from '../../clients.model';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';

export interface DialogData {
  id: number;
  action: string;
  clients: Clients;
}

@Component({
    selector: 'app-all-clients-form',
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
        MatDialogClose,
    ]
})
export class AllClientsFormComponent {
  action: string;
  dialogTitle: string;
  clientForm: UntypedFormGroup;
  clients: Clients;

  constructor(
    public dialogRef: MatDialogRef<AllClientsFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    public clientService: ClientsService,
    private fb: UntypedFormBuilder
  ) {
    // Set the defaults
    this.action = data.action;
    this.clients =
      this.action === 'edit' ? data.clients : new Clients({} as Clients);
    this.dialogTitle =
      this.action === 'edit' ? `Edit ${this.clients.name}` : 'New Client';
    this.clientForm = this.createClientForm();
  }

  // Create the form with validation
  private createClientForm(): UntypedFormGroup {
    return this.fb.group({
      id: [this.clients.id],
      img: [this.clients.img],
      name: [this.clients.name, [Validators.required]], // Name is required
      email: [this.clients.email, [Validators.required, Validators.email]], // Email validation
      mobile: [
        this.clients.mobile,
        [Validators.required, Validators.pattern(/^\d+$/)],
      ], // Mobile validation for numbers only
      company_name: [this.clients.company_name, [Validators.required]],
      currency: [this.clients.currency, [Validators.required]],
      billing_method: [this.clients.billing_method, [Validators.required]],
      contact_person: [this.clients.contact_person, [Validators.required]],
      contact_phone: [this.clients.contact_phone, [Validators.required]],
      status: [this.clients.status],
      contract_document: [this.clients.contract_document],
      notes: [this.clients.notes],
      address: [this.clients.address],
    });
  }

  // Generalized error message method
  getErrorMessage(controlName: string): string {
    const control = this.clientForm.get(controlName);
    if (control?.hasError('required')) {
      return 'This field is required';
    }
    if (control?.hasError('email')) {
      return 'Please enter a valid email';
    }
    if (control?.hasError('pattern')) {
      return 'Invalid format';
    }
    return '';
  }

  // Handle form submission
  submit(): void {
    if (this.clientForm.valid) {
      const clientData = this.clientForm.getRawValue();
      if (this.action === 'edit') {
        this.clientService.updateClient(clientData).subscribe({
          next: (response) => {
            this.dialogRef.close(response);
          },
          error: (error) => {
            console.error('Update Error:', error);
            // Optionally show an error message to the user
          },
        });
      } else {
        this.clientService.addClient(clientData).subscribe({
          next: (response) => {
            this.dialogRef.close(response);
          },
          error: (error) => {
            console.error('Add Error:', error);
            // Optionally show an error message to the user
          },
        });
      }
    }
  }

  onNoClick(): void {
    this.dialogRef.close();
  }
}

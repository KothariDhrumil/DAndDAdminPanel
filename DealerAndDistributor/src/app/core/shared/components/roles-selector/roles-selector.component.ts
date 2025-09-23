import { ChangeDetectionStrategy, Component, Input, OnInit, forwardRef, signal, computed, effect } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ControlValueAccessor, NG_VALUE_ACCESSOR, ReactiveFormsModule } from '@angular/forms';
import { MatListModule } from '@angular/material/list';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { RolesSelectorService } from './service/roles-selector.service';
import { RoleOption } from './models/role-option.model';

@Component({
    selector: 'app-roles-selector',
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule, MatListModule, MatCheckboxModule, MatProgressSpinnerModule],
    templateUrl: './roles-selector.component.html',
    styleUrls: ['./roles-selector.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => RolesSelectorComponent),
            multi: true
        }
    ]
})
export class RolesSelectorComponent implements ControlValueAccessor, OnInit {
    @Input() label = 'Roles';
    @Input() roleTypes: number[] = [80];
    @Input() disabled = false;

    readonly options = signal<RoleOption[]>([]);
    readonly loading = signal<boolean>(false);
    readonly error = signal<string | null>(null);

    private onChange: (value: number[]) => void = () => { };
    private onTouched: () => void = () => { };
    private _value = signal<number[]>([]);

    constructor(private readonly rolesService: RolesSelectorService) { }

    ngOnInit(): void {
        this.fetch();
    }

    private fetch(): void {
        this.loading.set(true);
        this.error.set(null);
        this.rolesService.getRolesByType(this.roleTypes).subscribe({
            next: (opts) => {
                this.options.set(opts);
                this.syncSelectionWithOptions();
                this.loading.set(false);
            },
            error: () => {
                this.error.set('Failed to load roles');
                this.loading.set(false);
            }
        });
    }

    // CVA
    writeValue(value: number[] | null): void {
        const v = Array.isArray(value) ? value.map(n => Number(n)) : [];
        this._value.set(v);
        this.syncSelectionWithOptions();
    }
    registerOnChange(fn: (value: number[]) => void): void { this.onChange = fn; }
    registerOnTouched(fn: () => void): void { this.onTouched = fn; }
    setDisabledState(isDisabled: boolean): void { this.disabled = isDisabled; }

    isSelected(id: number): boolean {
        return this._value().includes(id);
    }

    toggle(id: number): void {
        if (this.disabled) return;
        const current = new Set(this._value());
        if (current.has(id)) current.delete(id); else current.add(id);
        const next = Array.from(current.values());
        this._value.set(next);
        this.onChange(next);
        this.onTouched();
    }

    private syncSelectionWithOptions(): void {
            // If options not yet loaded, do not mutate selection
            const opts = this.options();
            if (!opts || opts.length === 0) return;
            // Remove any selections that no longer exist in options
            const ids = new Set(opts.map(o => o.id));
            const current = this._value();
            const filtered = current.filter(v => ids.has(v));
            if (filtered.length !== current.length) {
                this._value.set(filtered);
                this.onChange(filtered);
            }
    }
}

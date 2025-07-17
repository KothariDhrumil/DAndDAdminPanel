import { Component, ChangeDetectionStrategy } from '@angular/core';

@Component({
  selector: 'app-product',
  template: `<h2>Product Module Works!</h2>`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProductComponent {}

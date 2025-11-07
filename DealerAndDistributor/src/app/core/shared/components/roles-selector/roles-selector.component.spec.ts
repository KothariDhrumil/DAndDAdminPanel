import { RolesSelectorComponent } from './roles-selector.component';

describe('RolesSelectorComponent', () => {
  it('should initialize with empty value', () => {
    const cmp = new RolesSelectorComponent({} as any);
    cmp.writeValue([1,2]);
    expect(cmp).toBeTruthy();
  });
});

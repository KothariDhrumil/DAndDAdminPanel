export class Department {
  id: number;
  img: string;
  department_name: string;
  hod: string;
  phone: string;
  email: string;
  employee_capacity: string;
  establishedYear: string;
  totalEmployees: string;

  constructor(department: Department) {
    this.id = department.id || this.getRandomID();
    this.img = department.img || 'assets/images/user/user1.jpg';
    this.department_name = department.department_name || '';
    this.hod = department.hod || '';
    this.phone = department.phone || '';
    this.email = department.email || '';
    this.employee_capacity = department.employee_capacity || '';
    this.establishedYear = department.establishedYear || '';
    this.totalEmployees = department.totalEmployees || '';
  }

  public getRandomID(): number {
    const S4 = () => {
      return ((1 + Math.random()) * 0x10000) | 0;
    };
    return S4() + S4();
  }
}

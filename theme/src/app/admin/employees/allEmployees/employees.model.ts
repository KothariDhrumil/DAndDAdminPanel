import { formatDate } from '@angular/common';

export class Employees {
  id: number;
  img: string;
  name: string;
  email: string;
  birthDate: string;
  role: string;
  mobile: string;
  department: string;
  degree: string;
  gender: string;
  address: string;
  joiningDate: string;
  salary: number;
  lastPromotionDate: string;
  employeeStatus: string;
  workLocation: string;

  constructor(employees: Partial<Employees>) {
    this.id = employees.id || this.getRandomID();
    this.img = employees.img || 'assets/images/user/user1.jpg';
    this.name = employees.name || '';
    this.email = employees.email || '';
    this.birthDate = formatDate(new Date(), 'yyyy-MM-dd', 'en') || '';
    this.role = employees.role || '';
    this.mobile = employees.mobile || '';
    this.department = employees.department || '';
    this.degree = employees.degree || '';
    this.gender = employees.gender || '';
    this.address = employees.address || '';
    this.joiningDate = employees.joiningDate || '';
    this.salary = employees.salary || 0;
    this.lastPromotionDate = employees.lastPromotionDate || '';
    this.employeeStatus = employees.employeeStatus || 'Active';
    this.workLocation = employees.workLocation || '';
  }

  public getRandomID(): number {
    const S4 = () => {
      return ((1 + Math.random()) * 0x10000) | 0;
    };
    return S4() + S4();
  }
}

import { formatDate } from '@angular/common';

export class EmployeeShift {
  shiftId: number;
  employeeId: number;
  img: string;
  employeeName: string;
  shiftStartTime: string;
  shiftEndTime: string;
  shiftType: string;
  shiftDate: string;
  breakStartTime: string;
  breakEndTime: string;
  totalShiftHours: number;
  shiftStatus: string;
  shiftDescription: string;
  shiftAssignedBy: string;
  overtimeHours: number;
  shiftCategory: string;
  createdDate: string;
  lastModifiedDate: string;

  constructor(shiftData: Partial<EmployeeShift> = {}) {
    this.shiftId = shiftData.shiftId || this.getRandomID();
    this.employeeId = shiftData.employeeId || 0;
    this.img = shiftData.img || 'assets/images/user/user1.jpg';
    this.employeeName = shiftData.employeeName || '';
    this.shiftStartTime = shiftData.shiftStartTime || '';
    this.shiftEndTime = shiftData.shiftEndTime || '';
    this.shiftType = shiftData.shiftType || 'Day';
    this.shiftDate =
      shiftData.shiftDate || formatDate(new Date(), 'yyyy-MM-dd', 'en');
    this.breakStartTime = shiftData.breakStartTime || '';
    this.breakEndTime = shiftData.breakEndTime || '';
    this.totalShiftHours = shiftData.totalShiftHours || 8;
    this.shiftStatus = shiftData.shiftStatus || 'Scheduled';
    this.shiftDescription = shiftData.shiftDescription || 'Regular shift';
    this.shiftAssignedBy = shiftData.shiftAssignedBy || '';
    this.overtimeHours = shiftData.overtimeHours || 0;
    this.shiftCategory = shiftData.shiftCategory || 'Regular';
    this.createdDate =
      shiftData.createdDate ||
      formatDate(new Date(), "yyyy-MM-dd'T'HH:mm:ss", 'en');
    this.lastModifiedDate =
      shiftData.lastModifiedDate ||
      formatDate(new Date(), "yyyy-MM-dd'T'HH:mm:ss", 'en');
  }

  public getRandomID(): number {
    return Math.floor(Math.random() * 1000000);
  }
}

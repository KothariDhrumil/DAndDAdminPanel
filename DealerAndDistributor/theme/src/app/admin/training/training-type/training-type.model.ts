import { formatDate } from '@angular/common';

export class TrainingType {
  trainingTypeId: number;
  trainingTypeName: string;
  description: string;
  category: string;
  duration: string;
  deliveryMethod: string;
  targetAudience: string;
  status: string;
  createdDate: string;
  updatedDate: string;
  createdBy: string;
  updatedBy: string;
  isMandatory: string;
  cost: number;
  certification: string;

  constructor(trainingTypeData: Partial<TrainingType> = {}) {
    this.trainingTypeId = trainingTypeData.trainingTypeId || this.generateId();
    this.trainingTypeName = trainingTypeData.trainingTypeName || '';
    this.description = trainingTypeData.description || '';
    this.category = trainingTypeData.category || 'General';
    this.duration = trainingTypeData.duration || 'Not Specified';
    this.deliveryMethod = trainingTypeData.deliveryMethod || 'Online';
    this.targetAudience = trainingTypeData.targetAudience || 'All';
    this.status = trainingTypeData.status || 'Inactive';
    this.createdDate =
      trainingTypeData.createdDate ||
      formatDate(new Date(), 'yyyy-MM-ddTHH:mm:ss', 'en');
    this.updatedDate =
      trainingTypeData.updatedDate ||
      formatDate(new Date(), 'yyyy-MM-ddTHH:mm:ss', 'en');
    this.createdBy = trainingTypeData.createdBy || 'Unknown';
    this.updatedBy = trainingTypeData.updatedBy || 'Unknown';
    this.isMandatory = trainingTypeData.isMandatory || 'No';
    this.cost = trainingTypeData.cost || 0;
    this.certification = trainingTypeData.certification || 'No';
  }

  private generateId(): number {
    return Math.floor(Math.random() * 1000) + 1;
  }
}

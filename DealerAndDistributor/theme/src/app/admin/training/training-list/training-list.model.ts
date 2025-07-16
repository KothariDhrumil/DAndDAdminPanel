import { formatDate } from '@angular/common';

export class TrainingList {
  id: number;
  trainingType: string;
  trainer: string;
  employee: { name: string; avatar: string }[];
  timeDuration: string;
  description: string;
  cost: string;
  status: string;
  trainingDate: string;
  certification: string;
  department: string;
  durationHours: number;
  targetAudience: string;
  prerequisites: string;
  trainerContact: string;
  completionDate: string;

  constructor(training: TrainingList) {
    this.id = training.id || this.getRandomID();
    this.trainingType = training.trainingType || '';
    this.trainer = training.trainer || '';
    this.employee = training.employee || [];
    this.timeDuration = training.timeDuration || '';
    this.description = training.description || '';
    this.cost = training.cost || '';
    this.status = training.status || '';
    this.trainingDate =
      training.trainingDate || formatDate(new Date(), 'yyyy-MM-dd', 'en');
    this.certification = training.certification || '';
    this.department = training.department || '';
    this.durationHours = training.durationHours || 0;
    this.targetAudience = training.targetAudience || '';
    this.prerequisites = training.prerequisites || '';
    this.trainerContact = training.trainerContact || '';
    this.completionDate =
      training.completionDate || formatDate(new Date(), 'yyyy-MM-dd', 'en');
  }

  public getRandomID(): number {
    const S4 = () => {
      return ((1 + Math.random()) * 0x10000) | 0;
    };
    return S4() + S4();
  }
}

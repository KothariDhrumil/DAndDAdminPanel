import { formatDate } from '@angular/common';

export class Trainers {
  trainer_id: number;
  img: string;
  name: string;
  email: string;
  phone_number: string;
  hire_date: string;
  specialization: string;
  technical_skills: string[];
  certifications: string[];
  training_experience: number;
  industry_experience: number;
  training_area: string;
  status: string;
  location: string;
  languages_spoken: string[];
  training_format: string;

  constructor(trainer: Partial<Trainers>) {
    this.trainer_id = trainer.trainer_id || this.getRandomID();
    this.img = trainer.img || 'assets/images/user/user1.jpg'; // Default image
    this.name = trainer.name || '';
    this.email = trainer.email || '';
    this.phone_number = trainer.phone_number || '';
    this.hire_date =
      trainer.hire_date || formatDate(new Date(), 'yyyy-MM-dd', 'en');
    this.specialization = trainer.specialization || '';
    this.technical_skills = trainer.technical_skills || [];
    this.certifications = trainer.certifications || [];
    this.training_experience = trainer.training_experience || 0;
    this.industry_experience = trainer.industry_experience || 0;
    this.training_area = trainer.training_area || '';
    this.status = trainer.status || 'Active'; // Default to 'Active'
    this.location = trainer.location || '';
    this.languages_spoken = trainer.languages_spoken || [];
    this.training_format = trainer.training_format || 'In-person'; // Default to 'In-person'
  }

  public getRandomID(): number {
    const S4 = () => {
      return ((1 + Math.random()) * 0x10000) | 0;
    };
    return S4() + S4();
  }
}

import { Role } from "./role";

export class LoggedInUser {
  id: number;
  img: string;
  username: string = ''; // Initialize the 'username' property
  password: string;
  firstName: string;
  lastName: string;
  role: Role;
  token: string;

  constructor() {
    // Initialize other properties if needed
    this.id = 0;
    this.img = '';
    this.password = '';
    this.firstName = '';
    this.lastName = '';
    this.role = Role.Client;
    this.token = '';
  }
}

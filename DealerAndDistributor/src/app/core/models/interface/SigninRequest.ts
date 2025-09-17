// Request model for sign-in

export interface SigninRequest {
  phoneNumber: string;
  password: string;
  otpEnabled: boolean;
  email: string;
}

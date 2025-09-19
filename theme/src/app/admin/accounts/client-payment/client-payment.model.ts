export class ClientPayment {
  payment_id: number;
  client_id: string;
  client_name: string;
  invoice_id: string;
  payment_date: string;
  payment_amount: string;
  payment_method: string;
  transaction_id: string;
  payment_status: string;
  currency: string;
  description: string;
  created_at: string;
  updated_at: string;

  constructor(clientPayment: Partial<ClientPayment>) {
    this.payment_id = clientPayment.payment_id || this.getRandomID();
    this.client_id = clientPayment.client_id || '';
    this.client_name = clientPayment.client_name || '';
    this.invoice_id = clientPayment.invoice_id || '';
    this.payment_date = clientPayment.payment_date || '';
    this.payment_amount = clientPayment.payment_amount || '';
    this.payment_method = clientPayment.payment_method || '';
    this.transaction_id = clientPayment.transaction_id || '';
    this.payment_status = clientPayment.payment_status || '';
    this.currency = clientPayment.currency || '';
    this.description = clientPayment.description || '';
    this.created_at = clientPayment.created_at || '';
    this.updated_at = clientPayment.updated_at || '';
  }

  private getRandomID(): number {
    const S4 = () => {
      return ((1 + Math.random()) * 0x10000) | 0;
    };
    return S4() + S4();
  }
}

import { BaseModal, BaseContext } from '../base/modal';

export class Transaction extends BaseModal {
  date: Date;
  from: string;
  to: string;
  amount: number;
  type: string;
  description: string;
}

export class TransactionContext extends BaseContext {
  balance: number;
}

import { BaseRequest } from './base-request';
import { Account } from '../../account-model';

export interface AccountActionRequest extends BaseRequest {
    account: Account;
}


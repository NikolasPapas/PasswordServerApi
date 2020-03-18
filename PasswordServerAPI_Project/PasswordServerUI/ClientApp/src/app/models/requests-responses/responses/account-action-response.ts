import { BaseResponse } from './base-response';
import { Account } from '../../account-model';

export interface AccountActionResponse extends BaseResponse {
  accounts: Account[];    
}
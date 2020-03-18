import { BaseResponse } from './base-response';
import { Password } from '../../password-model';

export interface PasswordActionResponse extends BaseResponse {
  passwords: Password[];    
}


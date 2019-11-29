import { BaseRequest } from './base-request';
import { Password } from '../../password-model';

export interface PasswordActionRequest extends BaseRequest {
    password:Password;
}

import { TokenRequestActionEnum } from '../../enums/token-request-action';
import { LoginToken } from '../../login-token';

export interface TokenActionRequest {
    token: LoginToken;
    action: TokenRequestActionEnum;
}
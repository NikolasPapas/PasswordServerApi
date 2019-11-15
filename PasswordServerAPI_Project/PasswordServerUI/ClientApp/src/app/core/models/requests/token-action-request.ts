import { BaseRequest } from "./base-request";
import { LoginToken } from "../LoginToken";
import { TokenRequestActionEnum } from "../enums/token-request-action";

export interface TokenActionRequest {
    token:LoginToken;
    action:TokenRequestActionEnum;
}
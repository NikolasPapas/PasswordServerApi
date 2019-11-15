import { Guid } from "../../common/types/guid";

export interface LoginToken{
    LoginTokenId:Guid;
    userId:Guid;
    token:string;
    userAgent:string;
    lastLogIn:Date;
}

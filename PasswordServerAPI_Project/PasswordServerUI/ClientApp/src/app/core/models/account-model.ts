import { Guid } from "../../common/types/guid";
import { Password } from "./password-model";
import { Sex } from "./enums/sex-enum";

export class Account{
    accountId:Guid;
    firstName: string;
    lastName:string;
    userName:string;
    email:string;
    sex: Sex;
    lastLogIn:string;


    password:string;
    role:string;
    //currentToken:string;
    passwords:Password[]
}

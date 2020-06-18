
import { Password } from "./password-model";
import { Sex } from "./enums/sex-enum";
import { Guid } from '../common/types/guid';

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
    passwords:Password[]
}

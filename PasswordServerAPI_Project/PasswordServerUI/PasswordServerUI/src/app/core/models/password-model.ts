import { Guid } from "../../common/types/guid";
import { Sensitivity } from "./enums/sensitivity";
import { Strength } from "./enums/strength";

export interface Password {
    passwordId: Guid;
    name: string;
    userName: string;
    password: string;
    logInLink: string;
    sensitivity: Sensitivity;
    strength: Strength;
}

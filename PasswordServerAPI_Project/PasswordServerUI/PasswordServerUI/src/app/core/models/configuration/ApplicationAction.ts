import { ApplicationValidationMode } from "../../enums/application-validation-mode";
import { Guid } from "../../../common/types/guid";

export interface ApplicationAction {
    id: Guid;
    name: string;
    controllerPath: string;
    needsComment: boolean;
    tooltip: string;
    sendApplicationData: boolean;
    validationMode: ApplicationValidationMode;
    refreshAfterAction: boolean;
    collapseApplication: boolean;
    icon: string;
    needsConfirmation: boolean;
}

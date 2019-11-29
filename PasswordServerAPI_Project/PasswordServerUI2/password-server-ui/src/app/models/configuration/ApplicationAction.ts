import { Guid } from 'src/app/common/types/guid';
import { ApplicationValidationMode } from '../enums/application-validation-mode';

export interface ApplicationAction {
    id: Guid;
    name: string;
    controllerPath: string;
    controllerSend:string;
    needsComment: boolean;
    toolTipText: string;
    sendApplicationData: boolean;
    validationMode: ApplicationValidationMode;
    refreshAfterAction: boolean;
    collapseApplication: boolean;
    icon: string;
    needsConfirmation: boolean;
    color:string
}
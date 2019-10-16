import { Guid } from "../../../common/types/guid";

export interface BaseRequest {
    accountId : Guid;
    actionId : Guid;
}
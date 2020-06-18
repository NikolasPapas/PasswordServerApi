import { ApplicationAction } from "./ApplicationAction";

export interface ConfigurationResponse {
    actions:ApplicationAction[];
    token: string;

}
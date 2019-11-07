import { ApplicationAction } from "./ApplicationAction";

export interface ConfigurationResponse {
   // profiles: Profile[];
    actions:ApplicationAction[];
    token: string;

}
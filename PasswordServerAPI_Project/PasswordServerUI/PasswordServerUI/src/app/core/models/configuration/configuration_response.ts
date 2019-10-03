import { Profile } from "./profiles";

export interface ConfigurationResponse {
    profiles: Profile[];
    token: string;

}
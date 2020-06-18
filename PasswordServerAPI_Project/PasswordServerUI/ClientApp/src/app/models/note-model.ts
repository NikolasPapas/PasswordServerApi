import { Sensitivity } from "./enums/sensitivity";
import { Strength } from "./enums/strength";
import { Guid } from '../common/types/guid';

export interface Note {
    noteId: Guid;
    userId: Guid;
    note: string;
    lastEdit: Date;
}

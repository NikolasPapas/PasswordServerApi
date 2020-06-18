import { BaseRequest } from './base-request';
import { Note } from '../../note-model';

export interface NoteActionRequest extends BaseRequest {
    note:Note;
}

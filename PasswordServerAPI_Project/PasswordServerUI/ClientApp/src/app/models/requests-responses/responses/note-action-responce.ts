import { BaseResponse } from './base-response';
import { Note } from '../../note-model';

export interface NoteActionResponse extends BaseResponse {
  notes: Note[];    
}


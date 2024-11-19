import { Pipe, PipeTransform } from '@angular/core';
import { TaskStatus } from '../enums/task-status.enum';

@Pipe({
  name: 'taskStatusToString'
})
export class TaskStatusToStringPipe implements PipeTransform {
  transform(value: number, enumType: typeof TaskStatus): string {
    return enumType[value];
  }
}

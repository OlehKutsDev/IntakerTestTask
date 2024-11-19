import { Component, EventEmitter, Input, Output } from '@angular/core';
import { TaskStatus } from 'src/app/enums/task-status.enum';
import { TaskModel } from 'src/app/models/task.model';

@Component({
  selector: 'app-task-list',
  templateUrl: './task-list.component.html',
  styleUrls: ['./task-list.component.css']
})
export class TaskListComponent {
  @Input() tasks!: TaskModel[];
  @Output() statusUpdated: EventEmitter<number> = new EventEmitter<number>();

  TaskStatus = TaskStatus;
  desiredTaskStatus = TaskStatus.InProgress;

  onUpdateStatus(id: number) {
    this.statusUpdated.emit(id);
  }
}

import { Component } from '@angular/core';
import { TaskService } from './services/task.service';
import { TaskModel } from './models/task.model';
import { TaskStatus } from './enums/task-status.enum';

/* TaskService execution example */

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  tasks!: TaskModel[];

  ngOnInit() {
    this.getTasks();
  }
  
  constructor(private taskService: TaskService) {}

  onStatusUpdated(id: number) {
    this.updateTaskStatus(id);
  }

  getTasks() {
    this.taskService.getTasks().subscribe(tasks => this.tasks = tasks);
  }

  updateTaskStatus(id: number) {
    this.taskService.updateTaskStatus({
      id: id,
      status: TaskStatus.InProgress
    }).subscribe(_ => {
      alert(`Updated Task status for Id = ${id}`);
      this.getTasks();
    });
  }

  createTask() {
    this.taskService.createTask({
      name: 'Test Task',
      description: 'This is a test task.',
      assignedTo: 'Oleh Kuts',
      status: TaskStatus.NotStarted
    }).subscribe(id => { 
      alert(`Task created with Id = ${id}`);
      this.getTasks();
    });
  }
}

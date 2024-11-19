import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { TaskModel } from '../models/task.model';
import { Observable } from 'rxjs';
import { CreateTaskRequestModel } from '../requests/create-task-request.model';
import { UpdateTaskStatusRequestModel } from '../requests/update-task-status-request.model';

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  private baseUrl = 'http://localhost:5187/task';

  constructor(private http: HttpClient) {}

  createTask(task: CreateTaskRequestModel): Observable<number> {
    return this.http.post<number>(`${this.baseUrl}`, task);
  }

  updateTaskStatus(request: UpdateTaskStatusRequestModel): Observable<TaskModel> {
    return this.http.put<TaskModel>(`${this.baseUrl}/${request.id}/status`, request);
  }

  getTasks(): Observable<TaskModel[]> {
    return this.http.get<TaskModel[]>(`${this.baseUrl}`);
  }
}

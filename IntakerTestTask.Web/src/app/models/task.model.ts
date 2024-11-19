import { TaskStatus } from "../enums/task-status.enum";

export class TaskModel {
    id!: number;
    name!: string;
    description!: string;
    status!: TaskStatus;
    assignedTo?: string;
}
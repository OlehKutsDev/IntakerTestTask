import { TaskStatus } from "../enums/task-status.enum";

export class CreateTaskRequestModel {
    name!: string;
    description!: string;
    status!: TaskStatus;
    assignedTo?: string;
}
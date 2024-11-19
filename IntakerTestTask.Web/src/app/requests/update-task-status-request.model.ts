import { TaskStatus } from "../enums/task-status.enum";

export class UpdateTaskStatusRequestModel {
    id!: number;
    status!: TaskStatus;
}
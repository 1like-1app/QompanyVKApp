import {Meeting} from "./meeting"

export class Employee {
    id: number;
    vKId: string;
    firstName: string;
    lastName: string;
    employeeMeetings: Meeting[];
}
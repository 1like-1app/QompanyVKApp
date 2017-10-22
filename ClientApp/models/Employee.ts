import {Meeting} from "./Meeting"

export class Employee {
    id: number;
    vKId: string;
    firstName: string;
    lastName: string;
    employeeMeetings: Meeting[];
}
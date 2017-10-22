import {Meeting} from "./Meeting"

export class Employee {
    id: number;
    vkId: string;
    photo: string;
    firstName: string;
    lastName: string;
    employeeMeetings: Meeting[];
    meetings: Meeting[];
}
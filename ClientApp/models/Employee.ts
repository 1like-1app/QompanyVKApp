import {Meeting} from "./meeting"

export interface Employee {
    Id: number;
    VKId: string;
    FirstName: string;
    LastName: string;
    EmployeeMeetings: Meeting[];

}
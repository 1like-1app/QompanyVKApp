import {MeetingRoom} from "./MeetingRoom";
import {Employee} from "./Employee";

export interface Meeting {
    Id: number;
    MeetingRoom: MeetingRoom;
    StartTime: Date;
    EndTime: Date;
    EmployeeMeetings: Employee[];
}
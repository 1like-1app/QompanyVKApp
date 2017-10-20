import {MeetingRoom} from "./MeetingRoom";
import {Employee} from "./Employee";

export interface Meeting {
    id: number;
    meetingRoom: MeetingRoom;
    startTime: Date;
    endTime: Date;
    employeeMeetings: Employee[];
}
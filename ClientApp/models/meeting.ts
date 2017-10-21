import {MeetingRoom} from "./MeetingRoom";
import {Employee} from "./Employee";

export class Meeting {
    id: number;
    theme: string;
    meetingRoom: MeetingRoom;
    startTime: Date;
    endTime: Date;
    employeeMeetings: Employee[];
}
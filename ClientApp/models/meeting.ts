import {MeetingRoom} from "./MeetingRoom";
import {Employee} from "./Employee";

export class Meeting {
    id: number;
    date: string;
    title: string;
    theme: string;
    meetingRoom: MeetingRoom;
    startTime: Date;
    endTime: Date;
    employeeMeetings: Employee[] = [];

    public constructor() {
        this.startTime = new Date();
        this.startTime.setHours(this.startTime.getHours() + 2);
        this.endTime = this.startTime;
        this.endTime.setMinutes(this.endTime.getMinutes() + 30);
    }
}
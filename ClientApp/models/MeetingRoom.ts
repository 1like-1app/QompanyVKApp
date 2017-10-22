import { Meeting } from "./Meeting";

export class MeetingRoom {
    id: number;
    name: string;
    floor: number;
    capacity: number;
    meetings : Meeting[] = []
}
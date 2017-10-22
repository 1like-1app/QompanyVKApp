import Vue from 'vue';
import { Meeting } from "../../models/Meeting"
import { Component, Watch } from 'vue-property-decorator';
import { Employee } from '../../models/Employee';
import { MeetingRoom } from '../../models/MeetingRoom';

@Component
export default class BookingForm extends Vue {
    date: Date = new Date();
    meeting: Meeting = new Meeting()
    meetings: Meeting[] = [];
    employees: Employee[] = [];
    checkedEmployees = [];
    selected: string = '';
    rooms: MeetingRoom[] = [];
    duration: Date[] = [new Date((new Date()).setHours((new Date).getHours() + 2)), new Date((new Date()).setHours((new Date).getHours() + 3))];


    @Watch('date', { immediate: true, deep: true })
    dateOnPropertyChanged(value: Date, oldValue: Date) {
        console.log(JSON.stringify(value));        
        this.getRoomsForMeeting();
    }

    @Watch('duration', { immediate: true, deep: true })
    meetingOnPropertyChanged(value: Date[], oldValue: Date[]) {
        console.log(JSON.stringify(value));
        this.getRoomsForMeeting();
    }

    getRoomsForMeeting() {
        let query = 'api/MeetingRooms/GetSatisfyingRooms/' + this.duration[0].toISOString() + "/" + this.duration[1].toISOString();
        //console.log(query);
        fetch(query)
            .then(response => response.json() as Promise<MeetingRoom[]>)
            .then(data => {
                this.rooms = data;
            })
            .then(x => {
                if (this.rooms.length)
                    this.selected = this.rooms[0].name;
                else
                    this.selected = "К сожалению, все переговорные в это время заняты, попробуйте выбрать другое время";
            });
    }

    checkboxToggle(id: number) {
        let b = this.meeting.employeeMeetings.filter(e => e.id == id);
        if (b.length) {
            this.meeting.employeeMeetings = this.meeting.employeeMeetings.filter(e => e.id != b[0].id);
        }
        else {
            this.meeting.employeeMeetings.push(this.employees.filter(e => e.id == id)[0]);
        }
    }

    deleteMember(id: number) {
        let b = this.meeting.employeeMeetings.filter(e => e.id == id);
        if (b.length) {
            this.meeting.employeeMeetings = this.meeting.employeeMeetings.filter(e => e.id != b[0].id);
            this.checkedEmployees = this.checkedEmployees.filter(e => e != id);
        }
    }

    onSubmit(submitEvent: any) {
        this.meeting.startTime = this.duration[0];
        this.meeting.endTime = this.duration[1];
        this.meeting.meetingRoom = this.rooms.filter(r=> r.name === this.selected )[0];
        if (submitEvent) submitEvent.preventDefault();

        fetch('api/meetings', {
            method: 'post',
            body: JSON.stringify(this.meeting),
            headers: new Headers({
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            })
        })
    }

    mounted() {
        fetch('api/Meetings/GetMeetings')
            .then(response => response.json() as Promise<Meeting[]>)
            .then(data => {
                this.meetings = data;
                for (var i = 0; i < this.meetings.length; ++i) {
                    this.meetings[i].date = new Date(this.meetings[i].startTime).toISOString().split('T')[0].replace(/-/g, "/");
                    this.meetings[i].title = this.meetings[i].theme;
                }
            });
        fetch('api/Employees/GetEmployees')
            .then(response => response.json() as Promise<Employee[]>)
            .then(data => {
                this.employees = data;
            });
    }

}
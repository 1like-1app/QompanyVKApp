import Vue from 'vue';
import { Meeting } from "../../models/Meeting"
import { Component, Watch } from 'vue-property-decorator';
import { Employee } from '../../models/Employee';
import { MeetingRoom } from '../../models/MeetingRoom';

@Component
export default class BookingForm extends Vue {
    date: string = new Date().toISOString().split('T')[0];
    meeting: Meeting = new Meeting()
    meetings: Meeting[] = [];
    employees: Employee[] = [];
    checkedEmployees = [];
    rooms: MeetingRoom[] = [];

    @Watch('date', { immediate: true, deep: true })
    dateOnPropertyChanged(value: string, oldValue: string) {
        console.log(value + oldValue);
    }

    @Watch('meeting', { immediate: true, deep: true })
    meetingOnPropertyChanged(value: Meeting, oldValue: Meeting) {
        console.log(JSON.stringify(value));
        if (typeof value.startTime === "string" && typeof value.endTime === "string") {
            let startTime = new Date(this.date.toString() + 'T' + value.startTime);
            let endTime = new Date(this.date.toString() + 'T' + value.endTime);
            let query = 'api/MeetingRooms/GetSatisfyingRooms/' + startTime.toISOString() + "/" + endTime.toISOString();
            console.log(query);
            alert(query);
            fetch(query)
                .then(response => response.json() as Promise<MeetingRoom[]>)
                .then(data => {
                    this.rooms = data;
                });
        }
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
        this.meeting.startTime = new Date(this.date.toString() + 'T' + this.meeting.startTime.toString());
        this.meeting.endTime = new Date(this.date.toString() + 'T' + this.meeting.endTime.toString());
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

import Vue from 'vue';
import { Meeting } from "../../models/meeting"
import { Component } from 'vue-property-decorator';
import { Employee } from '../../models/Employee';


@Component
export default class BookingForm extends Vue {
    date: string = new Date().toISOString().split('T')[0];
    meeting: Meeting = new Meeting();
    meetings: Meeting[] = [];
    employees: Employee[] = [];

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

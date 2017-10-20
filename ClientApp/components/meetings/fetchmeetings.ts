import Vue from 'vue';
import {Meeting} from '../../models/meeting'
import { Component } from 'vue-property-decorator';

@Component
export default class FetchMeetingsComponent extends Vue {
    meetings: Meeting[] = [];

    mounted() {
        fetch('api/Meetings/GetMeetings')
            .then(response => response.json() as Promise<Meeting[]>)
            .then(data => {
                this.meetings = data;
            });
    }
}
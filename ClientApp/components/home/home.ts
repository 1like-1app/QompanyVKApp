import Vue from 'vue';
import { Meeting } from "../../models/meeting"
import { Component, Watch } from 'vue-property-decorator';
import { Employee } from '../../models/Employee';
import { Group } from '../../models/Group';
import { MeetingRoom } from '../../models/MeetingRoom';

declare var VK: any;


@Component
export default class BookingForm extends Vue {
    /**
     *
     */
    
    key: string = "2ac16aae8e4c27d2a2dcc77d95a5c1e13226f48f5a6ad3de7eca8f261b9a45688ab6d1b91bd3259cfb79d";
    date: Date = new Date();
    meeting: Meeting = new Meeting()
    meetings: Meeting[] = [];
    employees: Employee[] = [];
    checkedEmployees = [];
    init: boolean = false;
    currentGroup: string = "";
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
        let query = 'api/MeetingRooms/GetSatisfyingRooms/' + this.duration[0].toISOString() + "/" + this.duration[1].toISOString() + "/" + this.currentGroup;
        //console.log(query);
        fetch(query)
            .then(response => response.json() as Promise<MeetingRoom[]>)
            .then(data => {
                this.rooms = data;
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
    // callApi() {
    //     var self = this;
    //     VK.init(function () {
    //         var parts = document.location.search.substr(1).split("&");
    //         var flashVars = {},
    //             curr;
    //         for (var i = 0; i < parts.length; i++) {
    //             curr = parts[i].split('=');     // записываем в массив flashVars значения. Например: flashVars['viewer_id'] = 1;

    //             flashVars[curr[0]] = curr[1];
    //         }
    //         console.log(flashVars);
    //         alert(flashVars['access_token']);
    //         // VK.addCallback('onGroupSettingsChanged', function f(bytes, newkey){ 
    //         //     self.key = newkey; 
    //         //     console.log(newkey);
    //         // });
    //         // VK.callMethod("showAllowMessagesFromCommunityBox");            
    //         // VK.addCallback('onAllowMessagesFromCommunity', function f() {
    //         VK.api(
    //             "messages.send", { user_id: 12520313, access_token: '40099039dc84bbba636fff7051de15181a2da99a439ae66557e5646a64f36d75faebb023eb4cc7e2d86f6', message: 'Yeaaahhh!' },
    //             function (data) {
    //                 alert(JSON.stringify(data));
    //             }
    //         )
    //         //});
    //         VK.addCallback('onAllowMessagesFromCommunityCancel', function f() {
    //             alert("Vse ploho!");
    //         });
    //     }, function () { }, '5.68');



    // }
    deleteMember(id: number) {

        let b = this.meeting.employeeMeetings.filter(e => e.id == id);
        if (b.length) {
            this.meeting.employeeMeetings = this.meeting.employeeMeetings.filter(e => e.id != b[0].id);
            this.checkedEmployees = this.checkedEmployees.filter(e => e != id);
        }
    }
    getEmployeesData()
    {
        fetch('api/Employees/GetEmployeesBygroupid/' + this.currentGroup)
        .then(response => response.json() as Promise<Employee[]>)
        .then(data => {
            this.employees = data;
        });

        this.getRoomsForMeeting();        
    }
    onSubmit(submitEvent: any) {
     
        this.meeting.startTime = this.duration[0];
        this.meeting.endTime = this.duration[1];
        this.meeting.meetingRoom = this.rooms.filter(r => r.name === this.selected)[0];
        if (submitEvent) submitEvent.preventDefault();
        var data = { meeting: this.meeting, groupId: this.currentGroup, notifyEmployees: true }
        fetch('api/meetings', {
            method: 'post',
            body: JSON.stringify(data),
            headers: new Headers({
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            })
        })
    }
    updated() {

        // 
        if (!this.init) {
            
            this.init = true;
            var self = this;
            VK.init(function () {
                var parts = document.location.search.substr(1).split("&");
                var flashVars = {},
                    curr;
                for (var i = 0; i < parts.length; i++) {
                    curr = parts[i].split('=');     // записываем в массив flashVars значения. Например: flashVars['viewer_id'] = 1;

                    flashVars[curr[0]] = curr[1];
                }
                self.currentGroup = flashVars['group_id'];
                console.log(flashVars);
                if (flashVars['api_settings'] == 0) {
                    VK.addCallback('onGroupSettingsChanged', function f(bytes, newkey) {
                        console.log(newkey);
                        fetch('api/groups', {
                            method: 'post',
                            body: JSON.stringify(<Group>{
                                AccessToken: newkey,
                                VKId: flashVars['group_id']
                            }),
                            headers: new Headers({
                                'Accept': 'application/json',
                                'Content-Type': 'application/json'
                            })
                        })
                    });
                    VK.callMethod("showGroupSettingsBox", 4096);
                }
                VK.callMethod("showAllowMessagesFromCommunityBox");

            });
        }


        // VK.api(
        //     "users.get", { },
        //     function (data) {
        //         alert(JSON.stringify(data.response));
        //     });
    }
    mounted() {

        fetch('api/Meetings/GetMeetings/' + this.currentGroup)
            .then(response => response.json() as Promise<Meeting[]>)
            .then(data => {
                this.meetings = data;
                for (var i = 0; i < this.meetings.length; ++i) {
                    this.meetings[i].date = new Date(this.meetings[i].startTime).toISOString().split('T')[0].replace(/-/g, "/");
                    this.meetings[i].title = this.meetings[i].theme;
                }
            });
        fetch('api/Employees/GetEmployeesBygroupid/' + this.currentGroup)
            .then(response => response.json() as Promise<Employee[]>)
            .then(data => {
                this.employees = data;
            });
    }

}
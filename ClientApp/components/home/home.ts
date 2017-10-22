import Vue from 'vue';
import { Meeting } from "../../models/meeting"
import { Component } from 'vue-property-decorator';
import { Employee } from '../../models/Employee';
import { Group } from '../../models/Group';

declare var VK: any;


@Component
export default class BookingForm extends Vue {
    key: string = "2ac16aae8e4c27d2a2dcc77d95a5c1e13226f48f5a6ad3de7eca8f261b9a45688ab6d1b91bd3259cfb79d";
    date: string = new Date().toISOString().split('T')[0];
    meeting: Meeting = new Meeting();
    meetings: Meeting[] = [];
    employees: Employee[] = [];
    checkedEmployees = [];
    init: boolean = false;
    currentGroup: string = "";

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
    updated() {

        // 
        if (!this.init) {
            this.init = true;
            VK.init(function () {
                var parts = document.location.search.substr(1).split("&");
                var flashVars = {},
                    curr;
                for (var i = 0; i < parts.length; i++) {
                    curr = parts[i].split('=');     // записываем в массив flashVars значения. Например: flashVars['viewer_id'] = 1;

                    flashVars[curr[0]] = curr[1];
                }
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
                    VK.callMethod("showAllMessagesFromCommunityBox")
                }

            });
        }


        // VK.api(
        //     "users.get", { },
        //     function (data) {
        //         alert(JSON.stringify(data.response));
        //     });
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

import Vue from 'vue';
import { Component } from 'vue-property-decorator';

@Component
export default class Close extends Vue {
    collapsee: string = "";
    change(){
        //console.log("Ну хотя бы тут");
        this.collapsee = "collapse";
    }
}

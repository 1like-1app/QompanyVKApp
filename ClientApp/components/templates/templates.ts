import Vue from 'vue';
import VueFormWizard from 'vue-form-wizard';
import 'vue-form-wizard/dist/vue-form-wizard.min.css';
import {FormWizard, TabContent} from 'vue-form-wizard';
import 'vue-form-wizard/dist/vue-form-wizard.min.css';


Vue.use(VueFormWizard)
new Vue({
 el: '#Template',
 methods: {
  onComplete: function(){
      alert('Yay. Done!');
   }
  }
})
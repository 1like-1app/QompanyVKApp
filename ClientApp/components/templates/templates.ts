import Vue from 'vue';
import VueFormWizard from 'vue-form-wizard';
import 'vue-form-wizard/dist/vue-form-wizard.min.css';
import { FormWizard, TabContent } from 'vue-form-wizard';
import { Component, Emit } from 'vue-property-decorator';
import 'vue-form-wizard/dist/vue-form-wizard.min.css';

Vue.use(VueFormWizard);

@Component
export default class Template extends Vue {
  type: string ='';
  firstname: string = '';
  lastname: string ='';
  patronomic: string = '';
  age: Number;

  @Emit()
  onComplete() {
    alert('Отправлено администраторам!!!');
  }
}
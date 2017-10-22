import './css/site.css';
import 'bootstrap';
import Vue from 'vue';
import VueRouter from 'vue-router';


import './css/element-ui.css'
import {TimePicker, DatePicker} from 'element-ui';
import lang from 'element-ui/lib/locale/lang/ru-RU'
import locale from 'element-ui/lib/locale'

import 'vue-event-calendar/dist/style.css'
import vueEventCalendar from 'vue-event-calendar'
Vue.use(vueEventCalendar, {weekStartOn: 1, locale: 'ru'})

locale.use(lang);
Vue.use(TimePicker);
Vue.use(DatePicker);

Vue.config.devtools = true //enable Vue dev tools in browser //TODO: Change at prod

Vue.use(VueRouter);

const routes = [
    { path: '/', component: require('./components/home/home.vue.html') },
    { path: '/templates', component: require('./components/templates/templates.vue.html') },
    { path: '/fetchdata', component: require('./components/fetchdata/fetchdata.vue.html') },
    { path: '/meetings', component: require('./components/meetings/fetchmeetings.vue.html') }
];

new Vue({
    el: '#app-root',
    router: new VueRouter({ mode: 'history', routes: routes }),
    render: h => h(require('./components/app/app.vue.html'))
});

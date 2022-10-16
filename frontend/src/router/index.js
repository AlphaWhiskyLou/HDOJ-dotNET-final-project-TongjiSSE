import { createRouter, createWebHistory } from 'vue-router'
import Home from '../views/Main.vue'


const routes = [
  {
    path: '/',
    name: 'Login',
    component: () => import('../views/Login.vue'),
  },
  {
    path: '/hdoj',
    name: 'hdoj',
    redirect: '/hdoj/bank',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () => import(/* webpackChunkName: "about" */ '../views/Main.vue'),
    children: [{
        path: '/hdoj/bank',
        name:'Bank',
        //component: () => import('../views/mainPage.vue'),
        component: () => import('../views/Bank.vue'),
      },
      {
        path:'/hdoj/manage',
        name: 'Manage',
        component: () => import('../views/Manage.vue'),
      },
      {
        path: '/hdoj/bank/q/:id',
        name:'Question',
        //component: () => import('../views/mainPage.vue'),
        component: () => import('../views/Question.vue'),
      },
      {
        path: '/hdoj/material',
        //component: () => import('../views/mainPage.vue'),
        component: () => import('../views/Material.vue'),
      },
      {
        path: '/hdoj/online',
        //component: () => import('../views/mainPage.vue'),
        component: () => import('../views/Online.vue'),
      },
      {
        path: '/hdoj/calculator',
        //component: () => import('../views/mainPage.vue'),
        component: () => import('../views/Calculator.vue'),
      },
      {
        path: '/hdoj/myInfo',
        //component: () => import('../views/mainPage.vue'),
        component: () => import('../views/MyInfo.vue'),
      },
      {
        path: '/hdoj/alterInfo',
        //component: () => import('../views/mainPage.vue'),
        component: () => import('../views/alterMyInfo.vue'),
      },
      {
        path: '/hdoj/personalCloudDisk',
        component: () => import('../views/PersonalCloudDisk.vue'),
      },
      {
        path: '/acSaber',
        name: 'ACSaber',
        redirect: '/trainMode',
        component: ()=>import('../views/ACSaber/ACSaber.vue'),
        children:[
          {
            path: '/trainMode',
            name: 'TrainMode',
            component: () => import('../views/ACSaber/TrainMode.vue')
          },
          {
            path: '/friendMode',
            name: 'FriendMode',
            component: () => import('../views/ACSaber/FriendMode.vue'),
          },
          {
            path: '/questionList',
            name: 'QuestionList',
            component: () => import('../views/ACSaber/QuestionList.vue')
          },
          {
            path: '/ladderMode',
            name: 'LadderMode',
            component: () => import('../views/ACSaber/LadderMode.vue')
          },
          {
            path: '/questionForFight',
            name: 'DoQuestion',
            component: () => import('../views/ACSaber/QuestionAc.vue')
          },
          {
            path: '/rankList',
            name: 'RankList',
            component: () => import('../views/ACSaber/RankList.vue')
          },
          {
            path: '/chat',
            name: 'Chat',
            component: () => import('../components/Chat.vue')
          },
        ]
      },
      {
        path: '/hdoj/onlineedu',
        name:'OnlineEdu',
        component:()=>import('../views/OnlineEdu')
      },
      {
        path: '/hdoj/order',
        name:'CourseOrder',
        component: ()=>import('../views/order')
      }

    ]

  },
  {
    path: '/404',
    name: '404',
    component: () => import('../views/Error_404.vue')
  },
  {
    path: '/:pathMatch(.*)',
    redirect: '/404'
  }

]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

export default router

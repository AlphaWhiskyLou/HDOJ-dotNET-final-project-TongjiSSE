import { createStore } from 'vuex'

export default createStore({
  state: {
    userName: window.sessionStorage.getItem("user"),
    userType: window.sessionStorage.getItem("type"),
  },
  mutations: {
  },
  actions: {
  },
  modules: {
  }
})

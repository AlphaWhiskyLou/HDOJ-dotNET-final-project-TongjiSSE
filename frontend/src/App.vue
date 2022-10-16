<template>
  <div id="app">
    <router-view/>
    <div class="home" v-if="this.$route.path!='/'&&this.$route.path!='/404'&&this.$route.path!='/hdoj/order'&&this.$route.path!='/hdoj/bank'">
<!--      <el-menu-->
<!--          :default-active="activeIndex"-->
<!--          class="el-menu-demo"-->
<!--          mode="horizontal"-->
<!--          background-color="#545c64"-->
<!--          text-color="#fff"-->
<!--          active-text-color="#ffd04b">-->
<!--        <el-menu-item style="position:absolute;right:15%; color:black" index="1">-->
<!--          <el-button type="text" @click="dialogTableVisible = true">Chatting</el-button>-->
<!--        </el-menu-item>-->
<!--      </el-menu>-->
<!--      <div v-if="dialogTableVisible">-->
<!--        <el-dialog-->
<!--            v-model="dialogTableVisible"-->
<!--            width="1020px">-->
<!--          <div slot="title" class="title">-->
<!--            <img class="chat fs-gui-window-heading-icon-img" src="-->
<!--    https://cdn.acwing.com/media/file_system/file/application/icon/2220490481555590627-128_UQONKkk.png-->
<!--" alt="图标">-->
<!--          </div>-->
<!--&lt;!&ndash;          <div>&ndash;&gt;-->
<!--&lt;!&ndash;            <Chat/>&ndash;&gt;-->
<!--&lt;!&ndash;          </div>&ndash;&gt;-->
<!--        </el-dialog>-->
<!--      </div>-->
    </div>

  </div>
</template>

<script>

export default {
  name: 'App',
  components: {

  },
  data() {
    return {
      dialogTableVisible: false,
      activeIndex:0
    }
  },
}

</script>

<style scoped>
.title {
  height: 40px;
}


.chat {
  height: 46px;
  width: 46px;
  float: left;
}

</style>

<style>

#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;

}

#nav {
  padding: 30px;
}

#nav a {
  font-weight: bold;
  color: #2c3e50;
}

#nav a.router-link-exact-active {
  color: #42b983;
}
.home{

}

</style>

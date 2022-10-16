<template>
  <div class="ACSaber-menu">
    <el-container style="width: 100%; height: 100%">
      <el-aside style="height: 100%">
        <div style="height: 100%">
          <SideMenu style="height: 100%"></SideMenu>
        </div>
      </el-aside>
      <el-main ><router-view ></router-view></el-main>
    </el-container>
  </div>

</template>

<script>
import SideMenu from "./SideMenu";

export default {
  name: "ACSaber",
  components: {
    SideMenu
  }

}
</script>

<style scoped>
.ACSaber-menu{
  background-image: url(https://cdn.acwing.com/static/web/img/file_system/file/application/ac_saber/menu.jpg);
  background-repeat: no-repeat;
}
.background-img{
  position:absolute;
  left:0;
  top:0;
  z-index:-1;

}
/deep/ .el-main{
  height: 100vh;
  --el-main-padding: 0px;
}

</style>

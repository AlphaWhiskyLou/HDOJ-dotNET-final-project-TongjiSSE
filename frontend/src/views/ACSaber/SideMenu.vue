<template>
  <div>
    <el-menu
        default-active="activeIndex"
        class="side-menu"
        :router="true"
        :collapse="isCollapse"
        active-text-color="#ffd04b"
        text-color="#fff"
        background-color="#545c64"
    >
      <div class="fold-wrapper">
        <el-icon
            v-if="isCollapse"
            title="展开"
            @click="isCollapse=false"
        >
          <list style="width: 1em; height: 1em; margin-right: 8px" />
        </el-icon>
        <el-icon
            class="el-icon-s-fold"
            v-else
            title="收缩"
            @click="isCollapse=true"
        >
        </el-icon>
      </div>
      <el-menu-item index="0" :route="{name: 'TrainMode'}">
        <el-icon><set-up/></el-icon>
        <span slot="title">训练模式</span>
      </el-menu-item>
      <el-menu-item index="1" :route="{name:'LadderMode'}">
        <el-icon><opportunity/></el-icon>
        <span slot="title">天梯模式</span>
      </el-menu-item>
      <el-menu-item index="2" :route="{name:'FriendMode'}">
        <el-icon><avatar/></el-icon>
        <span slot="title">好友对战</span>
      </el-menu-item>
    </el-menu>
  </div>
</template>

<script>
import {
  Document,
  Menu as IconMenu,
  Location,
  Setting, SetUp, Avatar, Opportunity,
} from '@element-plus/icons-vue'
import {List} from "@element-plus/icons-vue";
export default {
  name: "SideMenu",
  components: {Opportunity, Avatar, SetUp, List},
  data(){
    return {
      isCollapse:false,
    }
  },
  watch:{
    //监听收缩状态变化，存储在sessionStorage中，保证页面刷新时仍然保存设置的装填
    isCollapse(newValue) {
      sessionStorage.setItem('isCollapse',newValue)
    }
  },
  created() {
    this.isCollapse=sessionStorage.getItem('isCollapse')==='true'
  },
  computed:{


  },
}
</script>

<style   scoped>

.side-menu{
  height: 100vh;
  width: 30vh;

}
</style>
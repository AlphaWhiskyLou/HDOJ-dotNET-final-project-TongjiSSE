<template>
  <div class="home">
    <el-menu
        :default-active="activeIndex"
        class="el-menu-demo"
        mode="horizontal"
        @select="handleSelect"
        background-color="#545c64"
        text-color="#fff"
        active-text-color="#ffd04b">
      <el-menu-item style="padding-left:15% " index="1">HDOJ</el-menu-item>
      <el-menu-item index="2"><router-link to="/hdoj/bank">题库</router-link></el-menu-item>
      <el-menu-item index="3"><router-link to="/hdoj/material">资料</router-link></el-menu-item>
      <el-menu-item index="4"><router-link to="/hdoj/online">在线编译</router-link></el-menu-item>
      <el-menu-item index="5"><router-link to="/hdoj/calculator">计算器</router-link></el-menu-item>

      <el-menu-item index="8" style='position: absolute;right:25%'><router-link to="/">返回登录页面</router-link></el-menu-item>

    </el-menu>

    <router-view/>
  </div>
</template>

<script>
export default {
  data() {
    return {
      headImgSrc: 'https://tva2.sinaimg.cn/large/9bd9b167ly1fzjxyujrpaj20b40b40ta.jpg',
      activeIndex: '2',
      userType: window.sessionStorage.getItem('type') === '0'
    };
  },
  methods: {
    handleClick() {
      alert('button click');
    },
    handleSelect(key, keyPath) {
      console.log(key, keyPath);
    }
  }
}
</script>

<style>

</style>

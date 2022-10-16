<template>
  <div style="text-align: center">
    <div class="background">
      <img :src="imgSrc" class="imgSrc"/>
    </div>
    <div class="wrap-box">
      <div style="font-size:80px" font-family="Microsoft YaHei">
        HDOJ
      </div>
      <div style="font-size:25px" >
        黄渡编程学习小组
      </div>
      <div class="login">
        <el-tabs v-model="activeName" @tab-click="handleClick">
          <el-tab-pane label="登录" name="first">
            <el-form :model="ruleForm" :rules="rules" ref="ruleForm" label-width="100px" class="demo-ruleForm">
              <el-form-item label="用户名" prop="name">
                <el-input v-model="ruleForm.name"></el-input>
              </el-form-item>
              <el-form-item label="密码" prop="pass">
                <el-input type="password" v-model="ruleForm.pass" auto-complete="off"></el-input>
              </el-form-item>
              <el-form-item>
                <el-button type="primary" @click="submitForm('ruleForm')">登录</el-button>
                <el-button @click="resetForm('ruleForm')">重置</el-button>
              </el-form-item>
            </el-form>
          </el-tab-pane>
          <el-tab-pane label="注册" name="second">
            <register></register>
          </el-tab-pane>
        </el-tabs>
      </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";
import Register from "@/views/Register";
import Cookies from "js-cookie";

export default {
  name: "Login",
  data(){
    var validatePass = (rule, value, callback) => {
      if (value === '') {
        callback(new Error('请输入密码'));
      } else {
        if (this.ruleForm.checkPass !== '') {
          this.$refs.ruleForm.validateField('checkPass');
        }
        callback();
      }
    };
    return {
      adminUrl: "https://localhost:7163/",
      imgSrc: require('@/assets/login.png'),//'./assets/logo.png',
      activeName: 'first',
      ruleForm: {
        name: '',
        pass: '',
        checkPass: '',
      },
      EduloginForm: {
        username: '15887928791',
        password: '123456'
      },
      rules: {
        name: [
          { required: true, message: '请输入您的用户名', trigger: 'blur' }
        ],
        pass: [
          { required: true, validator: validatePass, trigger: 'blur' }
        ]
      },

    };
  },
  methods: {
    //选项卡切换
    handleClick(tab, event) {
    },
    //重置表单
    resetForm(formName) {
      this.$refs[formName].resetFields();
    },
    //提交表单
    submitForm(formName) {
      this.loading= true;
      var param = {
        "username": this.ruleForm.name,
        "password": this.ruleForm.pass
      }
      this.$refs[formName].validate((valid) => {
        if (valid) {
          axios.post(
              "/user/login", param
              )
              .then(response => {
                if (response.data.code === 200) {
                  console.log(response,'ssss')
                  let s = JSON.stringify(response.data);
                  localStorage.setItem('token',s);
                  this.$alert('登录成功！')
                  // console.log('response==',response)
                  let type = response.data.type
                  console.log(type,'here is type')
                  if (type) {
                    this.$router.replace({path: '/hdoj/bank'});
                  }
                  else {
                    //this.$router.replace({path: '/hdoj/manage'});
                    window.location.href = this.adminUrl;
                  }
                  window.sessionStorage.setItem("id", this.ruleForm.name);
                  window.sessionStorage.setItem("type", type);
                  console.log('登录成功');
                }
                else {
                  this.$alert('登录失败!', '密码或用户名错误!');
                  console.log('登录失败');
                  return false;
                }
              })
        } else {
          console.log('登录失败');
          return false;
        }
      });
    },
  },
  components: { Register},
}
</script>

<style scoped>
body {
  margin: 40%;
  padding: 0;
}
.wrap-box{
  width: 40%;
  left: 30%;
  height: 40%;
  padding-top: 5%;
  background-color: rgba(255,255,255,0.6);
  position: relative;
}
.login {
  width: 400px;
  margin:  auto;
  text-align: center;
}

#app {
  font-family: 'Avenir', Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 0;

}
.el-tabs__item {
  text-align: center;
  width: 60px;
}

.demo-ruleForm .el-form-item__label{
  color:#3491FA;
  font-weight:bold;
  font-size:15px
}
.background {
  width: 100%;
  height: 100%;
  padding: 0;
  margin: 0;
  /**宽高100%是为了图片铺满屏幕 */
  z-index: -1;
  position: fixed;
  background-size: 100% 100%;
}
.imgSrc{
  height: 100%;
  width: 100%;
}
</style>

<template>
  <el-form :model="ruleForm" :rules="rules" ref="ruleForm" label-width="100px" class="demo-ruleForm">
    <el-form-item label="用户名" prop="name">
      <el-input v-model="ruleForm.name"></el-input>
    </el-form-item>
    <el-form-item label="密码" prop="pass">
      <el-input type="password" v-model="ruleForm.pass" auto-complete="off"></el-input>
    </el-form-item>
    <el-form-item label="确认密码" prop="checkPass">
      <el-input type="password" v-model="ruleForm.checkPass" auto-complete="off"></el-input>
    </el-form-item>
    <el-form-item label="邮箱" prop="email">
      <el-input type="email" v-model="ruleForm.email" auto-complete="off"></el-input>
    </el-form-item>
    <el-form-item label="大学名称" prop="university">
      <el-input type="university" v-model="ruleForm.university" auto-complete="off"></el-input>
    </el-form-item>
    <el-form-item label="年龄" prop="age">
      <el-input type="age" v-model="ruleForm.age" auto-complete="off"></el-input>
    </el-form-item>
    <el-form-item>
      <el-button type="primary" @click="submitForm('ruleForm')">注册</el-button>
      <el-button @click="resetForm('ruleForm')">重置</el-button>
    </el-form-item>
  </el-form>
</template>

<script>
import axios from "axios";

export default {
  name: "Register",
  data() {
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
    var validatePass2 = (rule, value, callback) => {
      if (value === '') {
        callback(new Error('请再次输入密码'));
      } else if (value !== this.ruleForm.pass) {
        callback(new Error('两次输入密码不一致!'));
      } else {
        callback();
      }
    };
    return {
      activeName: 'second',
      ruleForm: {
        name: '',
        pass: '',
        checkPass: '',
        university: '',
        email: '',
        age: '',
      },
      regFormEdu: {
        username: '',
        password: '',
        confirmPassword: ''
      },
      rules: {
        name: [
          {required: true, message: '请输入您的用户名', trigger: 'blur'},
        ],
        pass: [
          {required: true, validator: validatePass, trigger: 'blur'}
        ],
        checkPass: [
          {required: true, validator: validatePass2, trigger: 'blur'}
        ],
        email: [
          {required: true, message: '请输入您的电子邮箱', trigger: 'blur'},
        ],
        university: [
          {required: true, message: '请输入您的大学', trigger: 'blur'},
        ],
        age: [
          {required: true, message: '请输入您的年龄', trigger: 'blur'},
          {min: 0, max: 200, message: '请输入您的正确年龄', trigger: 'blur'}
        ]
      }
    };
  },
  methods: {
    submitForm(formName) {
      this.$refs[formName].validate((valid) => {
        if (valid) {
          //formdata.append("userPassword",this.ruleForm.pass);
          var param = {
            "username": this.ruleForm.name,
            "password": this.ruleForm.pass,
            "email": this.ruleForm.email,
            "university": this.ruleForm.university,
            "age": this.ruleForm.age
          }
          axios.post(
              "/user/register/", param
          )
              //.get("http://106.14.25.240:81/user/register/"+this.ruleForm.name+"/"+this.ruleForm.pass)//localStorage.getItem('token'))
              .then(response => {
                if (response.data.code === 200) {
                  console.log(response)
                  var s = JSON.stringify(response.data.data);
                  localStorage.setItem('token', s);
                  this.$alert('注册成功！');
                  this.$router.replace({path: '/hdoj/bank'});
                  console.log('注册成功');
                } else {
                  this.$alert('注册失败！');
                  console.log('注册失败');
                  return false;
                }
              })
        } else {
          console.log('注册失败');
          return false;
        }
      });
    },
    resetForm(formName) {
      this.$refs[formName].resetFields();
    },
  }
}
</script>

<style scoped>

</style>

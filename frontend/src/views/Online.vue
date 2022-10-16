<template>
  <el-dialog title="输入参数" v-model="dialogVisible">
    <el-input v-model="input" autosize type="textarea" style="width: 70%" placeholder="请输入要测试的参数">
    </el-input>
    <div slot="footer" class="dialog-footer" style="padding-top: 30px">
      <el-button @click="dialogVisible = false">取 消</el-button>
      <el-button type="primary" @click="runCode">确 定</el-button>
    </div>
  </el-dialog>
  <el-header>
    <el-row justify="center">
      <el-button type="primary" @click="dialogVisible=true" >编译运行</el-button>
    </el-row>
  </el-header>
  <el-main >
    <el-row justify="center">
      <MonacoEditor center ref="monacoEditor" :myArgs="myArgs"/>
    </el-row>
  </el-main>
</template>

<script>
import Submit from "../views/Submit.vue"
import MonacoEditor from "../components/MonacoEditor";
import axios from "axios";
export default {
  components:{
    Submit,
    MonacoEditor,
  },
  data(){
    return{
      dialogVisible: false,
      input:"",

    }
  },
  methods:{
    runCode () {
      this.$refs.monacoEditor.ArgsIn=this.input.replaceAll("\r", "\\r").replaceAll("\n", "\\n");
      this.$refs.monacoEditor.getCompile();// 调用子组件的提交
      // console.log(this.code)
      // this.$http.post("/question/submitTestCase", {
      //   "code": this.code,
      //   'input': this.input,
      //   'questionId': this.id
      // }).then(res => {
      //   console.log(res.data.data)
      //   alert(res.data.data)
      // }).catch(err => {
      //   console.log(err)
      // })
      this.dialogVisible = false
    },

  },
}
</script>

<style>

/*.el-main {*/
/*  background-color: lightgrey;*/
/*}*/

/*.el-footer {*/
/*  background-color: lightgrey;*/
/*}*/
</style>

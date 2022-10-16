<template>
  <div class="common-layout">
    <el-dialog style="width:650px;margin:auto;margin-top:10px;margin-down:30px;top:-150px;" v-model="dialogFormVisible">
      <el-card style="width:650px;margin:auto;margin-top:20px;margin-down:30px;">
        <el-card-content style="text-align: left">
          <h1>修改个⼈信息</h1>
          <div class="course-title">昵称：
            <el-input class="mb-4" v-model="this.nameAlter" placeholder="请输⼊修改后的内容" outline />
          </div>
          <div class="course-title">年龄：
            <el-input class="mb-4" v-model="this.ageAlter" placeholder="请输⼊修改后的内容" outline />
          </div>
          <div class="course-title">邮箱：
            <el-input class="mb-4" v-model="this.emailAlter" placeholder="请输⼊修改后的内容" outline />
          </div>
          <div class="course-title">学校：
            <el-input class="mb-4" v-model="this.schoolAlter" placeholder="请输⼊修改后的内容" outline />
          </div>
          <div class="course-title">个⼈介绍：
            <el-input class="mb-4" type="textarea" :autosize="{ minRows: 5}" v-model="this.contentAlter"
              placeholder="请输⼊修改后的内容" outline />
          </div>
          <el-button @click="submit" color="#e0e5df" style="color: rgb(40,40,40)">确认修改</el-button>
        </el-card-content>
      </el-card>
    </el-dialog>
    <el-dialog v-model="changePasswordVisible">
      <el-card>
        <el-card-head>
          <h1>修改密码</h1>
        </el-card-head>
        <el-card-content style="text-align: left">
          <div class="course-title">原始密码：
            <el-input type="password" v-model="this.oldPassword" placeholder="请输⼊原始密码" show-password/>
          </div>
          <div class="course-title">新密码：
            <el-input type="password" v-model="this.newPassword" placeholder="请输⼊修改后的密码" show-password/>
          </div>
          <el-button @click="submitChangePassword" color="#e0e5df" style="color: rgb(40,40,40)">确认修改</el-button>
        </el-card-content>
      </el-card>
    </el-dialog>
    <el-container>
      <el-aside>
        <img :src="headImgSrc" width="220" height="220" style="border-radius:50%;margin-top:50%">
        <div style="margin-top:-85px;">
          <h2>{{name}}</h2>
        </div>
        <!-- <hr style="margin-top:-40px;"> -->
      </el-aside>
      <el-main style="margin-top: 4%">
        <div class="personalInfoPanel">
          <el-row style="height:40px">
            <el-col :span="17">
              <h2>个⼈资料</h2>
            </el-col>
            <el-col :span="3">
              <el-button @click="jumpPage" style="width:130px;height:40px; border-radius:10px;">编辑个⼈资料</el-button>
            </el-col>
            <el-col :span="3">
              <el-button @click="changePassword" style="width:130px;height:40px; border-radius:10px;">修改密码</el-button>
            </el-col>
            <el-col :span="1">
              <el-button @click="deleteAccount"  style="background-color: #e2777a;color: white">注销</el-button>
            </el-col>
          </el-row>
          <br>
          <el-col id="info">
            <div class="personalInfoTitle">账号
            </div>
            <div class="personalInfoContent">
              {{this.id}}
            </div>
            <div class="personalInfoTitle">｜
            </div>
          </el-col>
          <el-col id="info">
            <div class="personalInfoTitle">昵称
            </div>
            <div class="personalInfoContent">
              {{this.name}}
            </div>
            <div class="personalInfoTitle">｜
            </div>
          </el-col>
          <el-col id="info">
            <div class="personalInfoTitle">年龄
            </div>
            <div class="personalInfoContent">
              {{this.age}}
            </div>
            <div class="personalInfoTitle">｜
            </div>
          </el-col>
          <el-col id="info">
            <div class="personalInfoTitle">邮箱
            </div>
            <div class="personalInfoContent">
              {{this.email}}
            </div>
            <div class="personalInfoTitle">｜
            </div>
          </el-col>
          <!-- <el-col id="info">-->
          <!-- <div class="personalInfoTitle">所在地-->
          <!-- </div>-->
          <!-- <div class="personalInfoContent">-->
          <!-- {{this.area}}-->
          <!-- </div>-->
          <!-- <div class="personalInfoTitle">｜-->
          <!-- </div>-->
          <!-- </el-col>-->
          <el-col id="info">
            <div class="personalInfoTitle">学校
            </div>
            <div class="personalInfoContent">
              {{this.school}}
            </div>
            <div class="personalInfoTitle">｜
            </div>
          </el-col>
          <p>.</p>
          <el-col id="info">
            <div class="personalInfoTitle">
              个⼈介绍
            </div>
            <div class="personalInfoContent" style="white-space: pre-line">
              {{this.content}}
            </div>
          </el-col>
        </div>
        <div class="question" style="margin-top:140px;">
          <hr style="margin-top:-15px;">
          <h2 style="text-align:left;margin-top:20px;">提交记录</h2>
          <el-table @row-click="openDetails" :data="tableData"
            style="width: 100%; padding-left: 13%;padding-right: 5%;border-radius: 20px;"
            :row-class-name="tableRowClassName" :default-sort="{prop:'submitTime',order:'ascending'}">
            <el-table-column label="id" align="center" prop="id" v-if="false" />
            <el-table-column prop="state" label="状态" sortable width="130">
              <template #default="scope">
                <span v-if="scope.row.state == 1" style="color: lightgreen;">pass</span>
                <span v-else-if="scope.row.state == 0" style="color: lightcoral;">failed</span>
                <span v-else>failed</span>
              </template>
            </el-table-column>
            <el-table-column prop="name" label="题⽬" sortable width="350">
            </el-table-column>
            <el-table-column prop="passingRate" label="通过率" sortable width="100">
            </el-table-column>
            <el-table-column prop="difficulty" label="难度" sortable width="100">
              <template #default="scope">
                <span v-if="scope.row.difficulty == '简单'" style="color: lightgreen;">{{scope.row.difficulty}}</span>
                <span v-else-if="scope.row.difficulty == '中等'"
                  style="color: lightsalmon;">{{scope.row.difficulty}}</span>
                <span v-else-if="scope.row.difficulty == '困难'"
                  style="color: lightcoral;">{{scope.row.difficulty}}</span>
                <span v-else>{{scope.row.difficulty}}</span>
              </template>
            </el-table-column>
            <el-table-column prop="language" label="编程语⾔" sortable width="120">
            </el-table-column>
            <el-table-column prop="submitTime" label="提交时间" sortable width="130">
            </el-table-column>
          </el-table>
        </div>
        <hr style="margin-top:-15px;">

      </el-main>
    </el-container>


    <!-- </div>
 </div> -->
  </div>
</template>
<script>
import Modal from './alterMyInfo.vue'
import axios from 'axios'
import {ElMessageBox} from "element-plus";


export default {
  components: {
    Modal,
    // 'v-course-order':()=>import('./order.vue')
  },
  data () {
    return {
      headImgSrc: 'https://tva2.sinaimg.cn/large/9bd9b167ly1fzjxyujrpaj20b40b40ta.jpg',
      dialogFormVisible: false,
      changePasswordVisible:false,
      oldPassword:"",
      newPassword:"",
      form: {
        name: '',
        region: '',
        date1: '',
        date2: '',
        delivery: false,
        type: [],
        resource: '',
        desc: ''
      },
      formLabelWidth: '120px',
      title: '弹窗标题',
      show: true,
      showAvatar: false,
      currentID: '',
      nameAlter: 'Jackie',
      ageAlter: '21',
      areaAlter: 'SHANGHAI HUANGDU',
      contentAlter: '较强的动⼿潜⼒。良好协调沟通潜⼒，适应⼒强，反应快⽤⼼灵活，爱创新！两年的会计经历锻炼了我细⼼的准则。',
      schoolAlter: '⻩渡理⼯',
      emailAlter: '1@gmail.com',
      id: '2821166215',
      name: 'Jackie',
      age: '21',
      area: 'SHANGHAI HUANGDU',
      content: '较强的动⼿潜⼒。良好协调沟通潜⼒，适应⼒强，反应快⽤⼼灵活，爱创新！两年的会计经历锻炼了我细⼼的准则。',
      school: '⻩渡理⼯',
      email: '1@gmail.com',
      tags: [
        { name: '标签⼀', type: '' },
        { name: '标签⼆', type: 'info' },
      ],
      difficultyOptions: [{
        value: '困难',
        label: '困难'
      }, {
        value: '中等',
        label: '中等'
      }, {
        value: '简单',
        label: '简单'
      }],
      difficulty: '',
      stateOptions: [{
        value: '已解答√',
        label: '已解答√'
      }, {
        value: '未尝试(--`)',
        label: '未尝试(--`)'
      }, {
        value: '在做了ing',
        label: '在做了ing'
      }],
      state: '',
      input: '',
      tableData: [{
        id: '1',
        result: '未完成',
        name: '⼤整数加法',
        submitTime: '2022.01.07',
        passingRate: '50%',
        difficulty: '简单',
      }, {
        id: '1',
        result: '未完成',
        name: '⼤整数加法',
        solution: '1',
        passingRate: '50%',
        difficulty: '中等',
        submitTime: '2022.01.07',
      }, {
        id: '1',
        result: '未完成',
        name: '⼤整数加法',
        solution: '1',
        passingRate: '50%',
        difficulty: '困难',
        submitTime: '2022.01.07',
      }],
    }
  },
  methods: {
    deleteAccount(){
      ElMessageBox.confirm(
          '是否要注销',
          '警告',
          {
            confirmButtonText: '是',
            cancelButtonText: '否',
            type: 'warning',
          }
      ).then(() => {
          axios({
            url:'/account/deleteAccount',
            method:'DELETE',
            params:{
              userId: this.currentID,
            }
          }).then(response => {
            console.log(response);
            var TCode=response.data.code;
            var TMessage=response.data.message;
            console.log(TCode);
            console.log(TMessage);
            if(TCode===200){
              this.$message({
                showClose: true,
                message: TMessage,
                type: 'success',})
              this.$router.push('/')
              window.localStorage.removeItem("token");
            }else{
              this.$message({
                showClose: true,
                message: TMessage,
                type: 'error',})
            }

          })
      }).catch(() => {
            this.$message({
              showClose: true,
              message: '注销取消！',
              type: 'info',})
          })
    },
    changePassword(){
      this.changePasswordVisible=true;
    },
    updateHeadImg () {
      if (this.currentID % 4 == 1) {
        this.headImgSrc = 'https://gimg2.baidu.com/image_search/src=http%3A%2F%2Fup.enterdesk.com%2F2021%2Fedpic%2F0b%2F17%2F04%2F0b1704a9741f4e7ddd07939877dd3590_1.jpg&refer=http%3A%2F%2Fup.enterdesk.com&app=2002&size=f9999,10000&q=a80&n=0&g=0n&fmt=jpeg?sec=1644142234&t=4f1fa0c40c07873cb747d04474178241'
      }
      else if (this.currentID % 4 == 2) {
        this.headImgSrc = 'https://gimg2.baidu.com/image_search/src=http%3A%2F%2Fup.enterdesk.com%2Fedpic%2F92%2F4a%2F72%2F924a726144487f372ac97057dbb01e81.jpg&refer=http%3A%2F%2Fup.enterdesk.com&app=2002&size=f9999,10000&q=a80&n=0&g=0n&fmt=jpeg?sec=1644142269&t=289155d2b0f39b19ed108a8b0eb24f8d'
      } else if (this.currentID % 4 == 3) {
        this.headImgSrc = 'https://gimg2.baidu.com/image_search/src=http%3A%2F%2Fb-ssl.duitang.com%2Fuploads%2Fitem%2F201704%2F05%2F20170405213655_uSEiT.jpeg&refer=http%3A%2F%2Fb-ssl.duitang.com&app=2002&size=f9999,10000&q=a80&n=0&g=0n&fmt=jpeg?sec=1644142269&t=0f75b074995a35b8c4e818116dd79fa8'
      }
    },
    jumpPage () {
      //alert(this.dialogFormVisible)
      this.dialogFormVisible = true
      // alert(this.dialogFormVisible)
      //window.location.href="/hdoj/alterInfo";
    },
    hideModal () {
      // 取消弹窗回调
      this.show = false
    },
    submitChangePassword(){
      if(this.oldPassword===""||this.newPassword===""){
        this.$message.error("输入值不能为空！！！")
      }
      axios({
        url:'/account/changePassword',
        method:'POST',
        data:{
          oldPassword : this.oldPassword,
          newPassword: this.newPassword,
          userId: this.currentID,
        }
      }).then(response => {
        console.log(response);
        console.log(response.data);
        var r_code=response.data.code;
        if(r_code===200){
          this.$message.success("修改密码成功!")
          this.changePasswordVisible=false;
        }else{
          this.$message.error("修改密码失败!原始密码错误!")
        }
      })
    },
    submit () {
      var param = {
        "userId": this.currentID,
        "username": this.nameAlter,
        "age": this.ageAlter,
        "university": this.schoolAlter,
        "email": this.emailAlter,
        "profile": this.contentAlter
      }
      //alert("!")
      // 确认弹窗回调
      axios.post(
        "/account/updateInfo", param
      ).then(res => {
      })
      let _this = this
      setTimeout(function () {
        _this.refreshP()
      }, 600)
      this.dialogFormVisible = false
    },
    updateTmpInfo () {
      this.nameAlter = this.name
      this.ageAlter = this.age
      this.contentAlter = this.content
      this.emailAlter = this.email
      this.schoolAlter = this.school
    },
    firstUploadPersonalInfo () {
      // let userLogin
      // this.id = userLogin.userId
      // this.name = userLogin.username
      // this.age = userLogin.age
      // this.area = userLogin.area
      // this.content = userLogin.profile
      // this.email = userLogin.email
      // this.school = userLogin.university
    },
    refreshP () {
      this.getCurrentId()
      axios.post(
        "/account/accountInfo?userId=" + this.currentID
      ).then(res => {
        console.log(res)
        this.id = res.data.data.user.userId
        this.name = res.data.data.user.username
        this.age = res.data.data.user.age
        this.area = res.data.data.user.area
        this.content = res.data.data.user.profile
        this.email = res.data.data.user.email
        this.school = res.data.data.user.university
        this.updateTmpInfo()
        this.tableData.splice(0, this.tableData.length)
        for (var i = 0; i < res.data.data.submissionCode.length; i++) {
          this.tableData.push(res.data.data.submissionCode[i])
        }
        for (var i = 0; i < res.data.data.questionList.length; i++) {
          this.tableData[i].questionId = res.data.data.questionList[i].questionId
          this.tableData[i].difficulty = res.data.data.questionList[i].difficulty
          this.tableData[i].name = res.data.data.questionList[i].name
          if (res.data.data.questionList[i].submission == 0)
            this.tableData[i].passingRate = "--"
          else
            this.tableData[i].passingRate = res.data.data.questionList[i].pass / res.data.data.questionList[i].submission
        }
        this.$forceUpdate()
      })
    },
    openDetails (row) {
      window.localStorage.setItem("questionId", row.questionId)
      this.$router.push("/hdoj/bank/q/" + row.questionId)
    },
    getCurrentId () {
      var userInfo = JSON.parse(localStorage.getItem('token'))
      console.log(userInfo)
      this.currentID = userInfo.id
    },
    // refreshUserInfo(){
    // axios.post(
    // "/account/accountInfo"
    // ).then(res => {
    // this.tableData.splice(0,this.tableData.length)
    // console.log(res)
    // // for(var i = 0;i<res.data.submissionCode.length();i++)
    // // {
    // // this.tableData.push()
    // // res.data.submissionCode
    // //
    // // }
    //
    // for(var i = 0;i<res.data.questionNames.length;i++)
    // {
    // this.tableData[i].name = res.data.questionNames[i]
    // }
    // this.$forceUpdate()
    // })
    // }
  },
  created() {
    // this.$refs.getList();
  },
  mounted () {
    this.getCurrentId()
    this.updateTmpInfo()
    this.refreshP()
    this.updateHeadImg()
    // axios.post(
    // "/account/accountInfo"
    // ).then(res => {
    // console.log(res)
    // // for(var i = 0;i<res.data.submissionCode.length();i++)
    // // {
    // // this.tableData.push()
    // // res.data.submissionCode
    // //
    // // }
    // for(var i = 0;i<res.data.questionNames.length();i++)
    // {
    // this.tableData[i].name = res.data.questionNames
    // }
    // this.$forceUpdate()
    //})
  }
}
</script>
<style scoped>
.photo {
  margin-bottom: 20px;
  height: 200px;
  margin-right: 80px;
}
#info {
  text-align: left;
  height: 40px;
}
#p1 {
  height: 140px;
}
.el-header {
  background-color: #b3c0d1;
  color: var(--el-text-color-primary);
  text-align: left;
  line-height: 60px;
}
.el-aside {
  /* background-color: #d3dce6; */
  color: var(--el-text-color-primary);
  text-align: center;
  line-height: 100px;
}
.el-main {
  background-color: #e9eef3;
  color: var(--el-text-color-primary);
  text-align: center;
  line-height: 20px;
  height: 600px;
  width: 200px;
  border-radius: 30px;
  margin: 20px;
}
.personalInfoPanel {
}
/* body > .el-container {
 margin-bottom: 50px;
} */
.el-row {
  margin-bottom: 20px;
  height: 90px;
  text-align: left;
  /* &:last-child {
 margin-bottom: 0;
 } */
}
.el-col {
  border-radius: 4px;
  /* height :20px; */
  /* font-size: 20px; */
  /* line-height: 20px; */
}
.bg-purple {
  text-align: left;
}
.bg-purple-light {
  /* background: #e5e9f2; */
  text-align: right;
}
/* .grid-content {
 border-radius: 4px;
 min-height: 36px;
} */
/* .el-container:nth-child(5) .el-aside,
.el-container:nth-child(6) .el-aside {
 line-height: 260px;
}
.el-container:nth-child(7) .el-aside {
 line-height: 320px;
} */
.personalInfoTitle {
  width: 180px;
  font-size: 16px;
  line-height: 20px;
  flex-grow: 0;
  flex-shrink: 0;
  flex-basis: auto;
  display: inline;
  color: darkgrey;
  margin-right: 6px;
  margin-left: 6px;
}
.personalInfoContent {
  font-size: 16px;
  line-height: 20px;
  overflow-x: hidden;
  overflow-y: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
  flex-grow: 1;
  flex-shrink: 1;
  flex-basis: auto;
  min-width: 0px;
  color: black;
  display: inline;
}
</style>

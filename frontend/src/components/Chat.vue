<template>
  <div class="main">
    <div class="main-up">
<!--      <div v-if="curUserName==''">-->
<!--        <el-button type="primary" @click="openDialog('登录')">登录</el-button>-->
<!--        <el-button type="primary" @click="openDialog('注册')">注册</el-button>-->
<!--      </div>-->
<!--      <div v-else>-->
<!--        {{ this.curUserName }}-->
<!--        <el-button type="primary" @click="loginOut">退出登录</el-button>-->
<!--      </div>-->
      <img :src="'data:image/png;base64,'+selfAvatar" style="width: 40px;height: 40px" alt="头像">
      <el-tag  style="margin-left: 10px;">{{curUserName}}</el-tag>
    </div>
    <div class="main_down">
      <div class="left">
        <div class="left_up">
          <div class="label_title">
            我的好友
          </div>
          <div :class="curSessionId==item.id?'box_select':'box'" v-for="item in sessionList_already" :key="item.id">
            <div class="box_left" @click="startSession(item.id)" title="点击开始聊天">
              <img :src="'data:image/png;base64,'+selfAvatar" style="width: 40px;height: 40px" alt="头像">
<!--              <img :src="'data:image/png;base64,'+item.avatar" style="width: 40px;height: 40px" alt="头像">-->
              <div style="display:inline-block;line-height: 20px">
                <el-tag type="success" style="margin-left: 10px;">
                {{ item.listName }}
                </el-tag>
              </div>
            </div>
            <div class="right_left">
              <div class="right_left_count" title="未读消息">

                <el-tag type="danger">
                  {{ item.unReadCount }}
                </el-tag>
              </div>
              <div class="right_left_del">
                <el-tag type="warning" class="el-icon-close" @click="delSession(item.id)" title="关闭会话">关闭</el-tag>
              </div>
            </div>
          </div>
        </div>
        <div class="left_down">
          <div class="label_title">
            可能认识的人
          </div>
          <div v-for="item in sessionList_not" :key="item.id" class="box" @click="createSession(item.id,item.name)">
            <div class="box_left" title="点击进行对话">
              <img :src="'data:image/png;base64,'+selfAvatar" style="width: 30px;height: 30px" alt="头像">
<!--              <img :src="'data:image/png;base64,'+item.avatar" style="width: 30px;height: 30px" alt="头像">-->
              <el-tag type="info" style="margin-left: 10px;">
                {{ item.name }}
              </el-tag>
            </div>
          </div>
        </div>
      </div>
      <div class="right">
        <div class="up" id="msg_end">
          <div v-for="(item,i) in list" :key="i" :class="item.fromUserId===curUserId?'msg_right':'msg_left'">
            <div>
              <div class="msg_up">
                <img :src="'data:image/png;base64,'+selfAvatar" style="width: 23px;height: 23px" alt="头像">

<!--                <img :src="'data:image/png;base64,'+item.fromUserAvatar" style="width: 23px;height: 23px" alt="头像">-->
                <span>
                  {{ item.fromUserName }}
                </span>
              </div>
              <div :class="item.fromUserId===curUserId?'msg_right_down':'msg_left_down'">
                {{ item.content }}
              </div>
            </div>
          </div>

        </div>
        <div class="down">
          <el-input
              type="textarea"
              :rows="9"
              placeholder="请输入内容，回车发送!"
              @keyup.enter.native="sendMsg"
              v-model="textarea">
          </el-input>
        </div>
      </div>
    </div>


    <el-dialog
        v-model='dialogVisible'
        :title="dialogTitle"
        width="30%">

      <template #footer>
        <el-input v-model="loginName" placeholder="请输入用户名..."></el-input>
        <div class="dialog-footer">
          <el-button @click="dialogVisible = false">取 消</el-button>
          <el-button type="primary" @click="loginOrRegister">确 定</el-button>
        </div>
      </template>
    </el-dialog>

  </div>
</template>

<script>
import axios from 'axios';
import * as ElementPlusIconsVue from '@element-plus/icons-vue'

export default {
  name: 'Chat',
  components: {},
  data() {
    return {
      dialogVisible: false,
      loginName: "",
      dialogTitle: "",
      // curUserId: "",
      // curUserName: "",
      curSessionId: "",
      sessionList_already: [],
      list: [],
      textarea: "",
      sessionList_not: [],
      fromUser:{
        id:'',
        username:'',
        avatar:'',
      }
    }
  },
  computed:{
    curUserName(){
      return  JSON.parse(window.localStorage.getItem("token")).username
    },
    selfAvatar() {
      return JSON.parse(window.localStorage.getItem("token")).avatar
    },
    curUserId(){
      return    JSON.parse(window.localStorage.getItem("token")).id
    },
    curUserAvatar(){
      return    JSON.parse(window.localStorage.getItem("token")).avatar
    }
  },
  created() {// 页面创建生命周期函数

  },
  mounted() {
    this.startSession(99999999)
    this.getSessionListNot()
    this.sessionListAlready() },
  updated() {
    // 解决每次发消息到最后面
    this.$nextTick(() => {
      var elmnt = document.getElementById("msg_end");
      elmnt.scrollTop = elmnt.scrollHeight;
    })
  },
  destroyed: function () {
    // this.wesocketonclose();
  },
  methods: {
    initWebsocket: function (userId, sessionId) {
      this.websock = new WebSocket("ws://101.132.172.189:2001/websocket/" + userId + "/" + sessionId);
      this.websock.onopen = this.wesocketonopen;
      this.websock.onerror = this.wesocketonerror;
      this.websock.onmessage = this.wesocketonmessage;
      this.websock.onclose = this.wesocketonclose;
    },
    wesocketonopen: function () {
      console.log("Websocket连接成功")
    },
    wesocketonerror: function (e) {
      console.log("Websocket连接发生错误", e)
    },
    wesocketonmessage: function (e) {
      let data = JSON.parse(e.data);
      if (data instanceof Array) {
        //列表数据
        this.sessionList_already = data
      } else {
        // 消息数据
        this.list.push(data)
      }
    },
    wesocketonclose: function (e) {
      if (this.curUserId != null) {
        if (this.curSessionId != null) {
          this.initWebsocket(this.curUserId, this.curSessionId)
        } else {
          this.initWebsocket(this.curUserId, 99999999)
        }
      }
      console.log("connection closed", e);
      console.log(e);
    },
    // 根据
    //消息发送
    sendMsg() {
      if (this.curSessionId == '') {
        return this.$message.error("请登录后开始聊天!")
      }
      let data = {
        "fromUserId": this.curUserId,
        "fromUserName": this.curUserName,
        'fromUserAvatar':this.curUserAvatar,
        "content": this.textarea
      }
      console.log('this.list==',this.list)
      if (this.list == null) {
        return this.$message.error("请选择左边的其中一位朋友开始聊天!")
      }
      this.list.push(data)
      this.websock.send(this.textarea)
      this.textarea = ''
    },

    //登录 or 注册
    loginOrRegister() {
      let thus = this
      if (this.dialogTitle == "注册") {
        axios.get('http://101.132.172.189:2001/register?name=' + this.loginName)
            .then(function (response) {
              if (response.data.code == -1) {
                return thus.$message.error(response.data.errDesc);
              }
              thus.$message.success("注册成功");
              thus.dialogVisible = false;
            })
            .catch(function (error) {
              console.log(error);
            })
      } else if (this.dialogTitle == "登录") {
        axios.get('http://101.132.172.189:2001/login?name=' + this.loginName)
            .then(function (response) {
              console.log(response.data);
              if (response.data.code == -1) {
                return thus.$message.error(response.data.errDesc);
              }
              thus.curUserId = response.data.data.id;
              thus.curUserName = response.data.data.name;
              thus.$message.success("登录成功");
              thus.getSessionListNot()
              thus.sessionListAlready()
              thus.startSession(99999999)
              thus.dialogVisible = false;
            })
            .catch(function (error) {
              console.log(error);
            })
      }

    },
    openDialog(openName) {
      this.dialogTitle = openName;
      this.dialogVisible = true;
    },
    // 退出登录
    loginOut() {
      let thus = this;
      axios.get("http://101.132.172.189:2001/loginOut?name=" + this.curUserName)
          .then(function (response) {
            if (response.data.code == -1) {
              return thus.$message.error(response.data.errDesc);
            }
            thus.curUserId = '';
            thus.curUserName = '';
            thus.$message.success("退出登录成功");
            thus.sessionList_not = []
            thus.sessionList_already = []
          })
          .catch(function (error) {
            console.log(error);
          })
    },
    //开始会话
    startSession(sessionId) {
      // console.log(this.websock);
      if (this.websock != undefined) {
        this.websock.close()
      }
      this.curSessionId = sessionId;
      this.initWebsocket(this.curUserId, sessionId)
      this.msgList(sessionId)
    },
    msgList(sessionId) {
      let thus = this
      axios.get("http://101.132.172.189:2001/msgList?sessionId=" + sessionId)
          .then(function (response) {
            if (response.data.code == -1) {
              return thus.$message.error(response.data.errDesc);
            }
            console.log(response)
            thus.list = response.data.data
            console.log('list==',response.data.data)
            thus.sessionListAlready()
          })
          .catch(function (error) {
            console.log(error);
          })
    },

    //删除会话
    delSession(sessionId) {
      let thus = this
      axios.get('http://101.132.172.189:2001/delSession?sessionId=' + sessionId)
          .then(function (response) {
            if (response.data.code == -1) {
              return thus.$message.error(response.data.errDesc);
            }
            thus.getSessionListNot()
            thus.sessionListAlready()
          })
          .catch(function (error) {
            console.log(error);
          })
    },
    getSessionListNot() {
      let thus = this
      axios.get('http://101.132.172.189:2001/sessionList/not?id=' + this.curUserId)
          .then(function (response) {
            if (response.data.code == -1) {
              return thus.$message.error(response.data.errDesc);
            }
            thus.sessionList_not = response.data.data
            console.log('thus.sessionList_not==',thus.sessionList_not)
            console.log('response.data.data==',response.data.data)

          })
          .catch(function (error) {
            console.log(error);
          })
    },
    sessionListAlready() {
      let thus = this
      axios.get('http://101.132.172.189:2001/sessionList/already?id=' + this.curUserId)
          .then(function (response) {
            if (response.data.code == -1) {
              return thus.$message.error(response.data.errDesc);
            }
            thus.sessionList_already = response.data.data
            console.log('thus.sessionList_already==',thus.sessionList_already)
          })
          .catch(function (error) {
            console.log(error);
          })
    },
    createSession(toUseId, toUserName) {
      let thus = this
      axios.get('http://101.132.172.189:2001/createSession?id=' + this.curUserId + "&toUserId=" + toUseId + "&toUserName=" + toUserName)
          .then(function (response) {
            if (response.data.code == -1) {
              return thus.$message.error(response.data.errDesc);
            }
            thus.getSessionListNot()
            thus.sessionListAlready()
          })
          .catch(function (error) {
            console.log(error);
          })
    }
  },


}
</script>
<style>
.main {
  width: 980px;
  height: 790px;
  margin-top: 0px;
  background-color: rgb(245, 245, 245);
}

.main-up {
  width: 980px;
  height: 40px;
  display: flex;
  align-items: center;
}

.main_down {
  width: 980px;
  height: 750px;
  display: flex;
  border: 1px #d3d3d3 solid;
}

.left {
  width: 300px;
  height: 750px;
  border-right: 1px #d3d3d3 solid;
}

.left_up {
  width: 300px;
  height: 450px;
  overflow-y: auto;
}

.left_down {
  width: 300px;
  height: 300px;
  overflow-y: auto;
}

.right {
  width: 680px;
  height: 750px;
}

.label_title {
  width: 282px;
  height: 25px;
  background-color: #f8f8f8;
  font-weight: 600;
  padding: 8px;

  display: inline-block;
  color: #07C160;
  font-size: 1vw;
  cursor: pointer;
  text-align: center;
  line-height: 5vh;
}

.box {
  width: 250px;
  height: 30px;
  padding: 10px 25px 10px 25px;
  display: flex;
  justify-self: flex-end;
  align-items: center;
}

.box:hover {
  background-color: gainsboro;
  cursor: pointer;
}

.box_select {
  width: 250px;
  height: 30px;

  padding: 10px 25px 10px 25px;
  display: flex;
  justify-self: flex-end;
  background-color: gainsboro;
}

.box_left {
  width: 230px;
  height: 30px;
  display: flex;
  align-items: center;
}

.right_left {
  width: 50px;
  height: 30px;
  line-height: 35px;
  display: flex;
  justify-content: space-between;
  /* border: 1px red solid; */
}

.right_left_del {
  width: 20px;
  padding-left: 10px;
}

.up {
  width: 680px;
  height: 550px;
  overflow-y: scroll;
  overflow-x: hidden;
  /* padding-bottom: 40px; */
  border-bottom: 1px #d3d3d3 solid;
}

.msg_left {
  width: 675px;
  margin-top: 5px;
}

.msg_right {
  width: 660px;
  margin-top: 5px;
  text-align: right;
}

.msg_up {
  height: 25px;
  padding-right: 20px;
  padding-left: 20px;
}

.msg_right_down {
  display: inline-block;
  padding: 0 10px;
  max-width: 80%;
  background-color: #b2e281;
  border-radius: 4px;
  line-height: 30px;
}

.msg_left_down {
  display: inline-block;
  padding: 0 10px;
  max-width: 80%;
  background-color: white;
  border-radius: 4px;
  line-height: 30px;
}

.down {
  width: 680px;
  height: 200px;
  /* border: 1px red solid; */
}

</style>

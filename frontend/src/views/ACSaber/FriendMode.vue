<template>
  <div>
    <div class="ac-saber-menu-ladder">
      <div class="ac-saber-menu-ladder-head">
        <div class="ac-saber-menu-ladder-head-timer">
         好友模式
        </div>
      </div>
      <div class="ac-saber-menu-ladder-body">
        <div class="ac-saber-menu-ladder-body-left">
          <div class="ac-saber-menu-ladder-body-left-scoreboard">
            <div class="ac-saber-menu-ladder-body-scoreboard-rank" @click="this.$router.replace('/acSaber/rankList')">
              排名 {{ selfInfo.rank }}
            </div>
            <img :src="'data:image/png;base64,'+selfAvatar" class="ac-saber-menu-ladder-body-scoreboard-photo" alt="头像">
            <div class="ac-saber-menu-ladder-body-scoreboard-username">
              <span>  {{ selfUsername }} </span>
            </div>
            <div class="ac-saber-menu-ladder-body-scoreboard-footer">
              <div class="ac-saber-menu-ladder-body-scoreboard-footer-text">
                <div>
                  <!--                <span>总场数: 10</span>-->
                  <span>总场数: {{ selfInfo.totalCount }}</span>
                </div>
                <div>
                  <span>胜场数: {{ selfInfo.passCount }}</span>
                </div>
              </div>
            </div>
          </div>
        </div>

        <div class="ac-saber-menu-ladder-body-middle">
          <img src="https://cdn.acwing.com/static/web/img/file_system/file/application/ac_saber/ladder/vs.png"
               class="img-responsive" alt="VS">
        </div>
        <div class="ac-saber-menu-ladder-body-right">
          <div class="ac-saber-menu-ladder-body-right-scoreboard">
            <div class="ac-saber-menu-ladder-body-right-scoreboard-rank"
                 @click="this.$router.replace('/acSaber/rankList')">
              <!--            排名 {{ 229 }}-->
              {{ opponentRank }}
            </div>
            <img :src="'data:image/png;base64,'+opponentAvatar" class="ac-saber-menu-ladder-body-right-scoreboard-photo"
                 alt="头像">

            <div class="ac-saber-menu-ladder-body-right-scoreboard-username">
              <!--            <span>saber</span>-->
              <span>{{ opponentUsername }}</span>
            </div>
            <div class="ac-saber-menu-ladder-body-right-scoreboard-footer">
              <div class="ac-saber-menu-ladder-body-right-scoreboard-footer-text">
                <div>
                  <!--                <span>总场数: 14</span>-->
                  <span>{{ opponentTotalCount }}</span>
                </div>
                <div>
                  <!--                <span>胜场数: 12</span>-->
                  <span>{{ opponentPassCount }}</span>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
      <div class="ac-saber-menu-ladder-footer">
        <div class="ac-saber-menu-ladder-footer-start" @click="copyLink()" v-show="!isMatching&&isMatchingButtonShow">
          邀请好友
        </div>
      </div>

    </div>
    <div v-if="questionDialogVisible">
      <el-dialog
          v-model="questionDialogVisible"
          title="竞技模式"
          width="100%"
          @opened="open"
          :destroy-on-close="true"
      >

        <div class='iconfont.icon-time' v-if='timeShow'>
          <div class='iconfont.icon-time'>倒计时 {{ minutes }}:{{ secondsCom }}</div>
        </div>

        <template #footer>
          <div>
            <QuestionAc @sendWinMessage="sendWinMessage" :id="questionId"></QuestionAc>
          </div>

        </template>
      </el-dialog>
    </div>
  </div>
</template>

<script>

import axios from "axios";
import QuestionAc from "./QuestionAc";


//test
import {ElMessage} from 'element-plus'

export default {
  name: "LadderMode",
  components: {
    QuestionAc,
  },
  data() {
    return {
      questionId:'',
      minutes: 15,
      seconds: 10,
      timeShow: false,
      answer: '',
      time: 20,
      chatMessage: {
        sender: "",
        type: "",
        data: "",
      },
      newScoreInput: 0,
      isMatching: false,
      isMatchingButtonShow: true,
      ws: '',
      opponentInfo: {
        userId: '',
        username: '',
        avatar: '',
        score: '',
        rank: '',
        totalCount: '',
        passCount: '',
      },
      questions: '',
      selfInfo: {
        userId: '',
        username: '',
        avatar: '',
        score: '',
        rank: '',
        totalCount: '',
        passCount: '',
      },
      questionDialogVisible: false,
    }
  },

  created() {
    this.getSelfInfo()
    this.initRankInfo()
  },
  mounted() {
    this.initWebsocket()

  },
  computed: {
    secondsCom() {
      return this.seconds < 10 ? '0' + this.seconds : this.seconds
    },
    userId() {
      return JSON.parse(window.localStorage.getItem("token")).id
    },
    selfUsername() {
      return JSON.parse(window.localStorage.getItem("token")).username
    },
    selfAvatar() {
      return JSON.parse(window.localStorage.getItem("token")).avatar
    },
    opponentId() {
      return this.opponentInfo.userId == '' ? '???' : this.opponentInfo.userId
    },
    opponentTotalCount() {
      return '总场数: ' + (this.opponentInfo.totalCount === undefined || this.opponentInfo.totalCount === '' ? '???' : this.opponentInfo.totalCount)
    },
    opponentPassCount() {
      return '胜场数: ' + (this.opponentInfo.passCount === undefined || this.opponentInfo.passCount === '' ? '???' : this.opponentInfo.passCount)
    },
    opponentRank() {
      return '排名 ' + (this.opponentInfo.rank === undefined || this.opponentInfo.rank === '' ? '???' : this.opponentInfo.rank)
    },
    opponentUsername() {
      return (this.opponentInfo.username === undefined || this.opponentInfo.username === '' ? '???' : this.opponentInfo.username)
    },
    opponentAvatar() {
      return (this.opponentInfo.avatar === undefined || this.opponentInfo.avatar === '' ? this.selfAvatar : this.opponentInfo.avatar)
    }
  },
  methods: {
    // 点击邀请好友获得链接功能
    // 参考实现: vue点击复制链接功能两种方法实现 https://blog.csdn.net/qq_45094682/article/details/120504518
    copyLink() {
      console.log( 'window.location==',window.location.href)
      var val = window.location.href+'?opponentId=' + this.userId
      // var val = 'http://localhost:8080/acSaber/friendMode?opponentId=' + this.userId
      console.log(val, '复制链接')
      let url = val // 后台接口返回的链接地址
      let inputNode = document.createElement('input')  // 创建input
      inputNode.value = url // 赋值给 input 值
      document.body.appendChild(inputNode) // 插入进去
      inputNode.select() // 选择对象
      document.execCommand('Copy') // 原生调用执行浏览器复制命令
      inputNode.className = 'oInput'
      inputNode.style.display = 'none' // 隐藏
      this.$message.success('复制邀请链接成功,快去让你的好友点击进行游戏对战吧！')
    },
    // 计时器报时结束时被调用
    showend() {
      this.$message.error('很遗憾，时间结束，您和对手均未能成功AC，游戏结束，战绩将不被记录！')
      let thus = this
      setTimeout(function () {
            thus.$nextTick(() => {
              thus.questionDialogVisible = false
              thus.isMatchingButtonShow = true
              thus.isMatching = false
              // 刷新个人信息
              thus.getSelfInfo()
              // 更新对手信息为未知
              thus.initOpponentInfo()
            })
          }
          , 3000
      )
    },
    // 计时器增加时间函数
    add() {
      var _this = this
      var time = window.setInterval(function () {
        if (_this.seconds === 0 && _this.minutes !== 0) {
          _this.seconds = 59
          _this.minutes -= 1
        } else if (_this.minutes === 0 && _this.seconds === 0) {
          _this.showend()
          _this.seconds = 0
          _this.timeShow = false
          window.clearInterval(time)
        } else {
          _this.seconds -= 1
        }
      }, 1000)
    },
    // 初始化rank信息
    initRankInfo() {
      let formData = new FormData();
      formData.append('userId', this.userId)
      axios.post('/solo/insertSoloUser', formData)
          .then((res) => {
            console.log(res)
          })
          .catch((err) => {
            console.log(err)
          })
    },
    getSelfInfo() {
      let formData = new FormData();
      formData.append('userId', this.userId)
      axios.post('/solo/findUserSoloInfo', formData)
          .then((res) => {
            console.log('getSelfInfo', res)
            if (res.data.code == 200) {
              this.selfInfo.rank = res.data.data.user.ranking
              this.selfInfo.totalCount = res.data.data.user.totalCount
              this.selfInfo.passCount = res.data.data.user.passCount
            } else {
              this.$message.error('个人排名加载异常 :' + res.data.message)
            }
          })
          .catch((error) => {
            console.log(error)
          })
    },
    getOpponentInfoByRoute() {
      if (this.$route.query.opponentId !== undefined) {
        this.opponentInfo.userId = this.$route.query.opponentId;
        console.log('通过url获取到了对手id')
        console.log(this.opponentInfo.userId)
        this.matchUserSpecified(this.opponentInfo.userId)
      }
    },
    // 发送websocket请求 同好友建立对战连接
    matchUserSpecified(id) {
      console.log("this.userId==", this.userId)
      this.chatMessage.sender = this.userId;
      this.chatMessage.type = "MATCH_USER_Specified";
      this.chatMessage.data = id
      console.log("用户: " + this.userId + "开始同指定选手" + id + "匹配....");
      this.ws.send(JSON.stringify(this.chatMessage))
      this.isMatching = true;
    },
    initOpponentInfo() {
      this.opponentInfo.userId = undefined
      this.opponentInfo.avatar =undefined
      this.opponentInfo.rank=undefined
      this.opponentInfo.score=undefined
      this.opponentInfo.totalCount=undefined
      this.opponentInfo.passCount=undefined
      this.opponentInfo.username=undefined
    },
    getOpponentInfo() {
      let formData = new FormData();
      formData.append('userId',this.opponentInfo.userId)
      axios.post('/solo/findUserSoloInfo', formData)
          .then((res) => {
            console.log('getOpponentInfo', res)
            if (res.data.code==200) {
              this.opponentInfo.username = res.data.data.username
              this.opponentInfo.avatar = res.data.data.avatar
              this.opponentInfo.rank = res.data.data.user.ranking
              this.opponentInfo.totalCount = res.data.data.user.totalCount
              this.opponentInfo.passCount = res.data.data.user.passCount
            } else {
              this.$message.error('对手信息加载异常 :' + res.message)
            }
          })
          .catch((error) => {
            console.log(error)
          })
    },
    initWebsocket() {
      let thus = this;
      thus.ws = new WebSocket("ws://101.132.172.189:1998/game/match/" + thus.userId);
      // thus.ws = new WebSocket("ws://localhost:1998/game/match/" + thus.userId);
      thus.ws.onopen = function (e) {
        console.log("websocket 已打开 userId: " + thus.userId);
        thus.addUser()
        thus.getOpponentInfoByRoute()
      }
      thus.ws.onmessage = function (msg) {
        console.log(thus.userId + "收到服务端消息: " + msg.data)
        let message = JSON.parse(msg.data)
        if (message.chatMessage.type == 'MATCH_USER_Specified') {
          thus.$message.success('匹配对手成功! 3s后即将进入对战')
          thus.isMatchingButtonShow = false
          thus.opponentInfo.userId = message.chatMessage.data.opponentInfo.userId;
          thus.questions = message.chatMessage.data.questions
          thus.questionId=message.chatMessage.data.questions[0].questionId

          thus.getOpponentInfo()
          // thus.selfInfo = message.chatMessage.data.selfInfo
          console.log("message==", message)
          console.log("message.chatMessage.data==", message.chatMessage.data)
          console.log('this.opponentInfo.userId==', thus.opponentInfo.userId)
          axios.post(
              "/account/accountInfo?userId=" + thus.opponentInfo.userId
          ).then(res => {
            console.log("call accountInfo==", res)
            thus.opponentInfo.username = res.data.data.user.username
            thus.opponentInfo.avatar = res.data.data.user.avatar
            console.log('thus.opponentInfo.username==', thus.opponentInfo.username)
          })
          setTimeout(function () {
                let thus_ = thus
                thus.$nextTick(() => {
                  // 打开做题窗口
                  thus_.questionDialogVisible = true
                  // 计时器开始倒计时
                  thus_.timeShow = true
                  thus_.seconds = 10
                  thus_.minutes = 15
                  thus_.add();
                })
              }
              , 3000
          )
        } else if (message.chatMessage.type == 'PLAY_GAME') {
          if (message.chatMessage.data.userId == thus.opponentInfo.userId) {
            ElMessage.error('很遗憾，对手已经率先AC！游戏结束')
            setTimeout(function () {
                  let thus_ = thus
                  thus.$nextTick(() => {
                    thus_.questionDialogVisible = false
                    thus_.isMatchingButtonShow = true
                    thus_.isMatching = false
                    // 令计时器停止
                    thus_.timeShow = false
                    thus_.seconds = 10
                    thus_.minutes = 15
                    // 刷新个人信息
                    thus_.getSelfInfo()
                    // 更新对手信息为未知
                    thus_.initOpponentInfo()
                  })
                }
                , 3000
            )
          }
        }
      }
      thus.ws.onclose = function (e) {
        console.log("websocket 已关闭 userId: " + thus.userId)
      }
      thus.ws.onerror = function (e) {
        console.log("websocket 发生了错误 userId : " + thus.userId, " 错误原因为 " + e.reason)
      }
    },
    randomId() {
      //这里需要改成题目总数！
      // var id = (parseInt(this.id) + (Math.floor(Math.random() * 30 + 1))) % 30
      var id = 1;
      return id;
    },
    openQuestion() {
      var id = this.randomId();
      window.localStorage.setItem("questionIdForLadderMode", id);
      this.$router.push("/hdoj/questionIdForLadderMode/q/" + id);
    },
    disconnect() {
      this.ws.close()
    },

// 用户加入
    addUser() {
      console.log("this.userId==", this.userId)
      this.chatMessage.sender = this.userId;
      this.chatMessage.type = "ADD_USER";
      console.log("用户: " + this.userId + " 开始加入....");
      this.ws.send(JSON.stringify(this.chatMessage))
    }
    ,
//随机匹配
    matchUser() {
      console.log("this.userId==", this.userId)
      this.chatMessage.sender = this.userId;
      this.chatMessage.type = "MATCH_USER";
      console.log("用户: " + this.userId + "开始匹配....");
      this.ws.send(JSON.stringify(this.chatMessage))
      this.isMatching = true;
    },

// 游戏中
    userInPlay() {
      console.log("this.userId==", this.userId)
      this.chatMessage.sender = this.userId;
      this.chatMessage.type = "PLAY_GAME";
      this.chatMessage.data = this.newScoreInput
      console.log("用户：" + this.userId + "更新分数为" + this.chatMessage.data);
      this.ws.send(JSON.stringify(this.chatMessage))
    },

    // 推送输赢消息
    sendWinMessage() {
      console.log('call sendWinMessage')
      console.log("this.userId==", this.userId)
      this.chatMessage.sender = this.userId;
      this.chatMessage.type = "PLAY_GAME";
      this.chatMessage.data = 1
      console.log("用户：" + this.userId + "更新分数为" + this.chatMessage.data);
      this.ws.send(JSON.stringify(this.chatMessage))
      ElMessage.success('恭喜你，成功AC，竞技成功！');
      // 令计时器停止
      this.timeShow = false
      this.seconds = 10
      this.minutes = 15
      let thus = this
      setTimeout(function () {
            thus.$nextTick(() => {
              thus.questionDialogVisible = false
              thus.isMatchingButtonShow = true
              thus.isMatching = false
              // 刷新个人信息
              thus.getSelfInfo()
              // 更新对手信息为未知
              thus.initOpponentInfo()
            })
          }
          , 3000
      )
    },
// 游戏结束
    gameover() {
      console.log("this.userId==", this.userId)
      this.chatMessage.sender = this.userId;
      this.chatMessage.type = "GAME_OVER";
      console.log("用户:" + this.userId + "结束游戏")
      this.ws.send(JSON.stringify(this.chatMessage))
    },
  }
}
</script>

<style scoped>
.ac-saber-menu-ladder-head {
  height: 20vh;
  width: 100%;
}

.ac-saber-menu-ladder-head-timer {
  background-repeat: no-repeat;
  background-image: url("https://cdn.acwing.com/static/web/img/file_system/file/application/ac_saber/others/timer.png");
  background-position: center;
  text-align: center;
  height: 20vh;
  width: 100%;
  line-height: 20vh;
  color: #e7d7d5;
  font-size: 3.5vh;
}

.ac-saber-menu-ladder-body {
  height: 45vh;
  width: 100%;
}

.ac-saber-menu-ladder-body-left {
  height: 45vh;
  width: 23%;
  display: inline-block;
  float: left;
  margin-left: 23vh;
}

.ac-saber-menu-ladder-body-scoreboard-photo {
  height: 29%;
  border: 1px solid rgb(247, 212, 43);
  width: 29%;
  margin-left: -11vh;
  margin-top: 3vh;
}

.ac-saber-menu-ladder-body-scoreboard-rank {
  font-size: 0.5vh;
  color: #95ccaf;
  text-align: center;
  font-weight: 400;
  cursor: pointer;
  width: 27vh;
  line-height: 5vh;
}

.ac-saber-menu-ladder-body-scoreboard-username {
  line-height: 5vh;
  color: black;
  font-weight: 700;
  text-align: center;
  margin-right: 11vh;

}

.ac-saber-menu-ladder-body-scoreboard-footer-text {
  line-height: 5vh;
  color: black;
  text-align: center;
  margin-right: 11vh;

  margin-top: 2vh;
}

.ac-saber-menu-ladder-body-left-scoreboard {
  background-repeat: no-repeat;
  background-image: url("https://cdn.acwing.com/static/web/img/file_system/file/application/ac_saber/others/board2.png");
  background-size: 28vh;
  height: 40vh;
  width: 100%;

}

.iconfont.icon-time {
  color: #2776df;
  margin: 0px 6px 0px 20px;
}

.ac-saber-menu-ladder-body-right-scoreboard-photo {
  height: 29%;
  border: 1px solid rgb(247, 212, 43);
  width: 20%;
  margin-left: -21vh;
  margin-top: 3vh;
}

.ac-saber-menu-ladder-body-right-scoreboard-rank {
  font-size: 0.5vh;
  color: #95ccaf;
  text-align: center;
  font-weight: 400;
  cursor: pointer;
  width: 27vh;
  line-height: 5vh;
}

.ac-saber-menu-ladder-body-right-scoreboard-username {
  line-height: 5vh;
  color: black;
  font-weight: 700;
  text-align: center;
  margin-right: 24vh;

}

.ac-saber-menu-ladder-body-right-scoreboard-footer-text {
  line-height: 5vh;
  color: black;
  text-align: center;
  margin-right: 24vh;

  margin-top: 2vh;
}

.ac-saber-menu-ladder-body-middle {
  height: 45vh;
  width: 28%;
  display: inline-block;
  float: left;
}

.iconfont.icon-time {
  color: #2776df;
  margin: 0px 6px 0px 20px;
}

.ac-saber-menu-ladder-body-right {
  height: 45vh;
  width: 30%;
  display: inline-block;
  float: left;
}

.ac-saber-menu-ladder-body-right-scoreboard {
  background-repeat: no-repeat;
  background-image: url("https://cdn.acwing.com/static/web/img/file_system/file/application/ac_saber/others/board2.png");
  background-size: 28vh;
  height: 40vh;
  width: 100%;

}

.ac-saber-menu-ladder-footer {
  height: 35vh;
  width: 100%;

}

.ac-saber-menu-ladder-footer-start {
  background-repeat: no-repeat;
  background-image: url("https://cdn.acwing.com/static/web/img/file_system/file/application/ac_saber/others/start_btn.png");
  background-position: center;
  background-size: 28vh;
  height: 17vh;
  width: 100%;
  color: white;
  font-weight: 400;
  font-size: 3.5vh;
  cursor: pointer;
  text-align: center;
  line-height: 16vh
}

.img-responsive {
  width: 37vh;
}

</style>

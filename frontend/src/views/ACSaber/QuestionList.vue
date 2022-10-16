<template>
  <div class="question-table">
    <el-button type="text" @click="Return()" style="margin-bottom: 10px ;float: left;"><span
        style="color: white">返回Esc</span>
    </el-button>
    <el-table
        :data="tableData"
        stripe
        style="width: 100%">
      <el-table-column
          prop="questionId"
          label="题号"
      >
      </el-table-column>
      <el-table-column
          prop="name"
          label="题目"
      >
        <template #default="scope">
          <el-button
              size="mini"
              type="text"
              @click="submitForm(scope.row.questionId)">{{ scope.row.name }}
          </el-button>
        </template>
      </el-table-column>
      <el-table-column
          prop="labels"
          label="题型"
      >
      </el-table-column>
      <el-table-column
          prop="difficulty"
          label="难度">
      </el-table-column>
      <!--      <el-table-column-->
      <!--          prop="isPass"-->
      <!--          label="是否完成">-->
      <!--      </el-table-column>-->
    </el-table>
    <div v-if="questionDialogVisible">
      <el-dialog
          v-model="questionDialogVisible"
          title="训练模式"
          width="100%"
          @opened="open"
          :destroy-on-close="true"
      >
        <div class='iconfont.icon-time' v-if='timeShow'>
          <div class='iconfont.icon-time'>倒计时 {{ minutes }}:{{ secondsCom }}</div>
        </div>
        <template #footer>
          <div>
            <QuestionAcForTrain :id="id" @sendWinMessage="sendWinMessage"></QuestionAcForTrain>
          </div>
        </template>
      </el-dialog>
    </div>

  </div>
</template>

<script>

import axios from "axios";
import QuestionAcForTrain from "./QuestionAcForTrain";
import {ElMessage} from "element-plus";

export default {
  name: "QuestionList",
  components: {
    QuestionAcForTrain
  },
  data() {
    return {
      questionDialogVisible: false,
      tableData: [
        // {
        //   id: '1',
        //   name: 'A + B',
        //   type: '无',
        //   difficulty: '简单',
        //   isPass: '否'
        // }, {
        //   id: '2',
        //   name: '\t01背包问题',
        //   type: '背包问题',
        //   difficulty: '简单',
        //   isPass: '是'
        // }, {
        //   id: '3',
        //   name: '多重背包问题 I',
        //   type: '背包问题',
        //   difficulty: '中等',
        //   isPass: '是'
        // }, {
        //   id: '4',
        //   name: '完全背包问题',
        //   type: '背包问题',
        //   difficulty: '困难',
        //   isPass: '是'
        // }
      ],
      minutes: 15,
      seconds: 10,
      timeShow: false,
      time: 20,
      id:1,
    }
  },
  computed:{
    secondsCom() {
      return this.seconds < 10 ? '0' + this.seconds : this.seconds
    },
  },
  created() {
    this.getQuestionList()
  },
  methods: {
    submitForm(id) {
      // console.log(id)
      // console.log('test')
      this.$message.success('3s后即将进入限时训练！')
      let thus = this;
      setTimeout(function () {
            let thus_ = thus
            thus.$nextTick(() => {
              // 打开做题窗口
              thus_.id = id
              thus_.questionDialogVisible = true
              // 计时器开始倒计时
              thus_.timeShow = true
              thus_.seconds=10
              thus_.minutes=15
              thus_.add();
            })
          }
          , 3000
      )
    },
    getQuestionList() {
      axios.post('/question/getQuestionByLabels?labels=' + this.$route.query.labels)
          .then(({data}) => {
            console.log(data)
            this.tableData = data.data
          })
          .catch((err) => {
            console.log(err)
          })
    },
    Return() {
      this.$router.replace('/acSaber/trainMode')
    },
    // 计时器报时结束时被调用
    showend() {
      this.$message.error('很遗憾，时间结束，您未能成功AC，游戏结束!')
      let thus = this
      setTimeout(function () {
            thus.$nextTick(() => {
              thus.questionDialogVisible = false
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
    // 推送输赢消息
    sendWinMessage() {
      ElMessage.success('恭喜你，成功AC！');
      // 令计时器停止
      this.timeShow=false
      this.seconds=10
      this.minutes=15
      let thus = this
      setTimeout(function () {
            thus.$nextTick(() => {
              thus.questionDialogVisible = false
            })
          }
          , 3000
      )
    },
  }
}
</script>

<style scoped>
.question-table {
  margin-top: 60px;
}
</style>
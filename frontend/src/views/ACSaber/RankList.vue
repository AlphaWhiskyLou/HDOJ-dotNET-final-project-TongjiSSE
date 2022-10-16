<template>
  <div class="ac-saber-menu-ladder-ranklist">
    <el-button type="text" @click="Return()" style="margin-bottom: 10px;    float: left;"><span style="color: white">返回Esc</span>
    </el-button>
    <div class="ac-saber-menu-ladder-head">
      <div class="ac-saber-menu-ladder-head-timer">
        <!--        <Timer :time="time" ref="headerChild"></Timer>-->
        排行榜
      </div>
    </div>
    <div style="float: right" >
      <el-tooltip
          effect="dark"
          content="赢一场获得3积分，输一场扣除1积分，任何一场比赛如果对战双方没有人在限时内成功AC，则双方的总场数和胜场数均不计入。好友模式的比赛不计入。"
          placement="bottom"
      >
        <el-button style="margin-bottom: 10px">竞技模式规则</el-button>
      </el-tooltip>
    </div>
    <el-tag size="medium" style="float:left;margin-bottom: 1vh">当前排名:{{ selfInfo.rank }}</el-tag>
    <el-tag size="medium" type="info" style="float:left;margin-bottom: 1vh">总场数:{{ selfInfo.totalCount }}</el-tag>
    <el-tag size="medium" type="success" style="float:left;margin-bottom: 1vh">胜场数:{{ selfInfo.passCount }}</el-tag>

    <el-table
        :data="tableData"
        stripe
        style="width: 100%">
      <el-table-column
          prop="ranking"
          label="排名"
          width="80">
      </el-table-column>
<!--      <el-table-column-->
<!--          prop="avatar"-->
<!--          label="头像"-->
<!--          width="280">-->
<!--      </el-table-column>-->
      <el-table-column  label="" width="120" >
        <template #default="scope">
          <img :src="'data:image/png;base64,'+scope.row.avatar" style="width: 120px;height:120px" alt="头像">
        </template>
      </el-table-column>
      <el-table-column
          prop="username"
          label="玩家名称"
          width="280">
      </el-table-column>
      <el-table-column
          prop="mark"
          label="积分"
          width="180">
      </el-table-column>
      <el-table-column
          prop="winRate"
          label="胜率"
          width="180">
      </el-table-column>
      <el-table-column
          prop="totalCount"
          label="总场数">
      </el-table-column>
      <el-table-column
          prop="passCount"
          label="胜场数">
      </el-table-column>
    </el-table>
  </div>
</template>

<script>
import axios from "axios";

export default {
  name: "RankList",
  data() {
    return {
      selfInfo: {
        userId: '',
        username: '',
        avatar: '',
        score: '',
        rank: '',
        totalCount: '',
        passCount: '',
      },
      tableData: []
    }
  },
  created() {
    this.getSelfInfo()
    this.getRankListInfo()
  },
  computed: {
    userId() {
      return JSON.parse(window.localStorage.getItem("token")).id
    },
  },
  methods: {
    submitForm() {
      console.log('test')

    },
    getSelfInfo() {
      let formData = new FormData();
      formData.append('userId',this.userId)
      axios.post('/solo/findUserSoloInfo', formData)
          .then((res) => {
            console.log('getSelfInfo', res)
            if (res.data.code==200) {
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
    getRankListInfo() {
      axios.post('/solo/findAllSoloInfo')
          .then((res) => {
            if (res.data.code==200) {
              console.log('getRankListInfo', res)
              this.tableData = res.data.data
            } else {
              this.$message.error('列表加载异常 :' + res.message)
            }
          })
          .catch((error) => {
            console.log(error)
          })
    },
    Return() {
      this.$router.replace('/acSaber/ladderMode')
    }
  }
}
</script>

<style scoped>
.ac-saber-menu-ladder-ranklist {
  margin-top: 60px;
}

.ac-saber-menu-ladder-head {
  height: 20vh;
  width: 100%;
}

.ac-saber-menu-ladder-head-timer {
  background-repeat: no-repeat;
  background-image: url("https://cdn.acwing.com/static/web/img/file_system/file/application/ac_saber/others/timer.png");
  background-position: center;
  text-align: center;
  margin-left: 6vh;
  height: 20vh;
  width: 100%;
  line-height: 20vh;
  color: #e7d7d5;
  font-size: 3.5vh;
}

</style>
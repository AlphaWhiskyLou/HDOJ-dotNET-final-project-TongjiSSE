<template>
  <div class="solution">
    <el-dialog :title="solutions[currentIndex].title" v-model="dialogSolutionVisible">
      <el-row>
        <el-col :span="8">
          <div class="grid-content ">
            作者:{{solutions[currentIndex].userName}}
          </div>
        </el-col>
        <el-col :span="8">
          <div class="grid-content">
            语言:{{solutions[currentIndex].language}}
          </div>
        </el-col>
        <el-col :span="8">
          <div class="grid-content ">
            时间:{{solutions[currentIndex].time}}
          </div>
        </el-col>
      </el-row>
      <el-collapse>
        <el-collapse-item title="思路" name="1">
          <div style="text-align: left;white-space:pre-line">{{solutions[currentIndex].content}}</div>
        </el-collapse-item>
        <el-collapse-item title="代码" name="2">
          <pre style="text-align: left">{{solutions[currentIndex].code}}</pre>
        </el-collapse-item>
        <el-collapse-item title="评论" name="3">
          <el-card style="background-color: #444444" class="box-card" v-for="item in comments" :key="item.id"
            @click="view(item)">
            <h5 style="text-align: left;font-size: larger;color:floralwhite;margin:10px 18px;">{{item.userName}}</h5>
            <div style="color:floralwhite;text-align: left;margin:0 18px;">
              <span>{{item.content}}</span>
            </div>
            <el-row :gutter="20">
              <el-col :span="4">
                <div class="grid-content" style="color:floralwhite">
                  <Star style="height: 15px;width: 15px" v-if="!item.isThumbed" @click="thumbComment(item)" />
                  <StarFilled style="height: 15px;width: 15px" v-if="item.isThumbed" />
                  点赞数:{{item.likes}}
                </div>
              </el-col>
              <el-col :span="8">
                <div class="grid-content" style="color:floralwhite">
                  时间:{{item.time}}
                </div>
              </el-col>
              <el-col :span="8">
                <el-button v-if="item.userId == this.userId" @click="deleteComment(item.id)">删除
                </el-button>
              </el-col>
            </el-row>
          </el-card>
        </el-collapse-item>
      </el-collapse>
      <div slot=" footer" class="dialog-footer">
        <el-button type="primary" @click="uploadComment" style="margin-top: 20px">发布评论</el-button>
        <el-button type="primary" @click="dialogSolutionVisible = false" style="margin-top: 20px">关闭
        </el-button>
        <el-button v-if="this.clickUserId == this.userId" type="danger" @click="deleteSolution"
          style="margin-top: 20px">删除</el-button>
      </div>
    </el-dialog>
    <div class="solutionHeader">

    </div>
    <el-card style="background-color: lightcyan" class="box-card" v-for="item in solutions" :key="item.id"
      @click="view(item)">
      <div slot="header" class="clearfix">
        <h5 style="font-size: larger;text-align: left">{{item.userName}}:{{item.title}}</h5>
      </div>
      <el-row :gutter="20">
        <el-col :span="4">
          <div class="grid-content ">
            <Star style="height: 15px;width: 15px" v-if="!item.isThumbed" @click="thumbSolution(item)" />
            <StarFilled style="height: 15px;width: 15px" v-if="item.isThumbed" />
            点赞数:{{item.likes}}
          </div>
        </el-col>
        <el-col :span="4">
          <div class="grid-content ">
            语言:{{item.language}}
          </div>
        </el-col>
        <el-col :span="16">
          <div class="grid-content ">
            时间:{{item.time}}
          </div>
        </el-col>
      </el-row>
    </el-card>
  </div>

</template>

<script>
import { Star, StarFilled } from '@element-plus/icons-vue'
import { ElMessageBox, ElMessage } from 'element-plus'
export default {
  components: {
    Star,
    StarFilled,
    ElMessageBox,
    ElMessage
  },
  data () {
    return {
      dialogSolutionVisible: false,

      comments: [{
        userId: '1',
        userName: '--',
        id: '1',
        sId: '1',
        content: '暂无数据',
        likes: '0',
        time: '--',
        isThumbed: false
      }],
      solutions: [{
        id: '0',
        userName: '未知',
        content: '暂无数据',
        userid: '1',
        qid: '1',
        time: '--',
        language: '--',
        title: '暂无数据',
        code: '--',
        likes: '0',
        isThumbed: false
      }],
      userId: '0',
      clickUserId: '0',
      currentIndex: '0'
    }
  },
  methods: {
    uploadComment () {
      ElMessageBox.prompt('请输入评论内容', 'Comment', {
        confirmButtonText: '上传',
        cancelButtonText: '取消',
        inputErrorMessage: '输入不能为空',
        inputValidator: (value) => {       // 点击按钮时，对文本框里面的值进行验证
          if (!value) {
            return '输入不能为空'
          }
        }
      })
        .then(({ value }) => {
          this.$http.post("/comment/addComment", {
            "userId": JSON.parse(window.localStorage.getItem("token")).id,
            "solutionId": this.solutions[this.currentIndex].id,
            'commentId': '',
            'content': value,
            'likes': 0,
            'commentTime': '',
          }).then(res => {
            if (res.data.code == 200)
              alert("上传成功")
            else
              alert("由于未知原因，上传失败")
            this.dialogNewVisible = false
          }).catch(err => {
            alert("由于未知原因，上传失败")
            console.log(err)
          })
          ElMessage({
            type: 'success',
            message: `上传成功`,
          })
        })
        .catch(() => {
          ElMessage({
            type: 'info',
            message: '取消操作',
          })
        })
    },
    thumbSolution (row) {
      row.isThumbed = true
      row.likes++
      this.$http.post("/solution/likesIncrement?solutionId="
        + row.id)
        .then(res => {
          console.log(res)
        })
        .catch(err => {
          console.log(err)
        })
    },
    thumbComment (row) {
      row.isThumbed = true
      row.likes++
      this.$http.post("/comment/likesIncrement?commentId="
        + row.id)
        .then(res => {
          console.log(res)
        })
        .catch(err => {
          console.log(err)
        })
    },
    view (row) {
      this.userId = JSON.parse(window.localStorage.getItem("token")).id
      this.dialogSolutionVisible = true
      for (let i = 0; i < this.solutions.length; i++) {
        if (row.id == this.solutions[i].id) {
          this.currentIndex = i
          break
        }
      }
      this.$http.get("/solution/getSolutionById?solutionId=" + this.solutions[this.currentIndex].id)
        .then(res => {
          this.clickUserId = res.data
        }).catch(err => {
          console.log(err)
        })
      this.$http.get("/comment/listComments?solutionId=" + this.solutions[this.currentIndex].id)
        .then(res => {
          console.log(res)
          this.comments.splice(0, 1)
          for (let i = 0; i < res.data.length; i++) {
            this.comments.push({
              'id': res.data[i].commentId,
              'userName': '',
              'content': res.data[i].content,
              'userId': res.data[i].userId,
              'sid': res.data[i].solutionId,
              'time': res.data[i].commentTime,
              'likes': res.data[i].likes,
              'isThumbed': false
            })
            this.$http.get("/user/getUserInfoUnique?userId="
              + this.comments[i].userId)
              .then(res => {
                this.comments[i].userName = res.data
              }).catch(err => {
                console.log(err)
              })
          }
        }).catch(err => {
          console.log(err)
        })
    },
    deleteSolution () {
      this.$http.get("/solution/deleteSolution?solutionId=" + this.solutions[this.currentIndex].id)
        .then(res => {

        }).catch(err => {
          console.log(err)
        })
    },
    deleteComment (commentId) {
      console.log(commentId)
      this.$http.get("/comment/deleteComment?commentId=" + commentId)
        .then(res => {

        }).catch(err => {
          console.log(err)
        })
    }
  },
  mounted () {
    this.$http.get("/solution/listSolutions?questionId=" + window.localStorage.getItem("currentQuestionId"))
      .then(res => {
        console.log(res)
        this.solutions.splice(0, this.solutions.length)
        for (let i = 0; i < res.data.length; i++) {
          this.solutions.push({
            'id': res.data[i].solutionId,
            'userName': '',
            'content': res.data[i].content,
            'userid': res.data[i].userId,
            'qid': res.data[i].questionId,
            'time': res.data[i].createdTime,
            'language': res.data[i].language,
            'title': res.data[i].title,
            'code': res.data[i].code,
            'likes': res.data[i].likes,
            'isThumbed': false
          })
          this.$http.get("/user/getUserInfoUnique?userId="
            + this.solutions[i].userid)
            .then(res => {
              console.log(res.data.message);
              this.solutions[i].userName = res.data;
            }).catch(err => {
              console.log(err)
            })
        }
      }).catch(err => {
        console.log(err)
      })
  }
}
</script>

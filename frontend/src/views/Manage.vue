<template>
  <div class="bg"></div>
  <div>
    <el-container style="width: 100%;height: 100%">
      <el-aside style="padding: 10px">
        <el-menu
            active-text-color="#ffd04b"
            style="background-color: rgba(0,178,238,0.5);border-radius: 5px;"
            default-active="1"
            text-color="#fff"
        >
          <el-menu-item index="1" @click="chooseTable=1">
            <span style="font-size: 15px;font-weight: bold">题目管理</span>
          </el-menu-item>
          <el-menu-item index="2" @click="chooseTable=2">
            <span style="font-size: 15px;font-weight: bold">用户管理</span>
          </el-menu-item>
        </el-menu>
      </el-aside>
      <el-main style="margin-top: 10px;margin-right: 10px;background-color: rgba(0,0,0,0.1);border-radius: 5px;">
        <div v-show="chooseTable===1">
          <el-button type="warning"
                     style="width: 20%;margin-bottom: 10px "
                     @click="UploadQ=true">点此上传新题目
          </el-button>
          <el-dialog
              v-model="UploadQ"
              title="题目上传"
              width="40%"
          >
            <el-form
                label-position="right"
                label-width="60px"
                :model="form"
                style="min-width: 30%"
            >
              <el-form-item label="题目"
                            :rules="{required: true, message: '不能为空', trigger: 'blur'}">
                <el-input placeholder="题目名称" v-model="form.name"/>
              </el-form-item>
              <el-form-item label="题干"
                            :rules="{required: true, message: '不能为空', trigger: 'blur'}">
                <el-input type="textarea"
                          placeholder="这里输入题目的详细描述"
                          rows="8"
                          v-model="form.description"/>
              </el-form-item>
              <div>
                <el-form-item label="标签"
                              :rules="{required: true, message: '标签名不能为空', trigger: 'blur'}">
                  <el-row :gutter="10">
                    <el-col :span="18">
                      <el-input v-model="form.labels"/>
                    </el-col>
                    <el-col :span="6">
                      <el-button @click="addItem" type="primary">增加</el-button>
                    </el-col>
                  </el-row>
                </el-form-item>
              </div>

              <div v-for="(item, index) in form.dynamicItem" :key="index">
                <el-form-item
                    label="标签"
                    :prop="'dynamicItem.' + index + '.content'"
                    :rules="{required: true, message: '标签名不能为空', trigger: 'blur'}"
                >
                  <el-row :gutter="10">
                    <el-col :span="18">
                      <el-input v-model="item.content">{{ item.content }}</el-input>
                    </el-col>
                    <el-col :span="6">
                      <el-button type="danger" @click="deleteItem(item, index)">删除</el-button>
                    </el-col>
                  </el-row>
                </el-form-item>
              </div>
              <el-form-item label="难度">
                <el-radio-group v-model="form.difficulty" size="large">
                  <el-radio-button label="简单" border>简单</el-radio-button>
                  <el-radio-button label="中等" border>中等</el-radio-button>
                  <el-radio-button label="困难" border>困难</el-radio-button>
                </el-radio-group>
              </el-form-item>
            </el-form>
            <template #footer>
      <span class="dialog-footer">
        <el-button @click="UploadQ = false">取消</el-button>
        <el-button type="primary" @click="confirmUpload()"
        >确认上传</el-button
        >
      </span>
            </template>
          </el-dialog>
          <el-table :data="tableData.slice((currentPage-1)*pageSize,currentPage*pageSize)"
                    stripe
                    style="width: 100%"
                    :default-sort="{ prop: 'questionId', order: 'ascending' }"
          >
            <el-table-column align="center" width="200px" prop="questionId" label="题号"/>
            <el-table-column align="center" width="auto" label="题目" prop="questionName"/>
            <el-table-column
                align="center"
                prop="difficulty"
                label="难度"
                sortable
                :sort-method="sortByDifficulty"
                width="150%">
              <template #default="scope">
                <span v-if="scope.row.difficulty == '简单'" style="color: lightgreen;">{{ scope.row.difficulty }}</span>
                <span v-else-if="scope.row.difficulty == '中等'"
                      style="color: lightsalmon;">{{ scope.row.difficulty }}</span>
                <span v-else-if="scope.row.difficulty == '困难'"
                      style="color: lightcoral;">{{ scope.row.difficulty }}</span>
                <span v-else>{{ scope.row.difficulty }}</span>
              </template>
            </el-table-column>
            <el-table-column align="right">
              <template #header>
                <el-input @keyup.enter.native="searchResource" v-model="keywords" size="small"
                          placeholder="搜索"></el-input>
              </template>
              <template #default="scope">
                <el-button
                    type="primary"
                    size="medium"
                    @click="goToRead(scope.row.questionId)"
                >查看详情
                </el-button>
                <el-button
                    type="danger"
                    size="medium"
                    @click="deleteQ(scope.row.questionId)"
                >删除
                </el-button>
              </template>
            </el-table-column>
          </el-table>
          <el-pagination
              hide-on-single-page
              class="fy"
              background
              layout="prev, pager, next, jumper"
              @current-change="handleCurrentChange"
              :page-size="pageSize"
              :total="tableData.length">
          </el-pagination>
        </div>
        <div v-show="chooseTable===2">
          <el-table :data="tableData1.slice((currentPage1-1)*pageSize1,currentPage1*pageSize1)"
                    stripe
                    style="width: 100%"
                    :default-sort="{ prop: 'userId', order: 'ascending' }"
          >
            <el-table-column align="center" width="200px" label="用户ID" prop="userId"/>
            <el-table-column align="center" width="auto" label="用户名" prop="userName"/>
            <el-table-column
                align="center"
                prop="email"
                label=""
                sortable
                width="150%">
            </el-table-column>
            <el-table-column align="right">
              <template #header>
                <el-input @keyup.enter.native="searchResource1" v-model="keywords1" size="small"
                          placeholder="搜索"></el-input>
              </template>
              <template #default="scope">
                <el-button
                    type="primary"
                    size="medium"
                    @click="startRevise(scope.row.userId)"
                >修改信息
                  <el-dialog
                      v-model="needRevise"
                      title="用户信息更新"
                      width="40%"
                  >
                    <el-form
                        label-position="right"
                        label-width="60px"
                        :model="userForm"
                        style="min-width: 30%"
                    >
                      <el-form-item label="修改"
                                    :rules="{required: true, message: '不能为空', trigger: 'blur'}">
                        <el-input placeholder="在此输入新用户名" v-model="userForm.newName"/>
                      </el-form-item>
                    </el-form>
                      <el-button @click="needRevise = false">取消</el-button>
                      <el-button type="primary" @click="goToRevise(userForm.userId)">确认更新</el-button>
                  </el-dialog>
                </el-button>
                <el-button
                    type="danger"
                    size="medium"
                    @click="deleteUser(scope.row.userId)"
                >删除
                </el-button>
              </template>
            </el-table-column>
          </el-table>
          <el-pagination
              hide-on-single-page
              class="fy"
              background
              layout="prev, pager, next, jumper"
              @current-change="handleCurrentChange1"
              :page-size="pageSize1"
              :total="tableData1.length">
          </el-pagination>
        </div>
      </el-main>
    </el-container>
  </div>
</template>

<script>

import {ElNotification} from "element-plus";

export default {
  name: "Manage",
  data() {
    return {
      chooseTable: 1,
      form: {
        dynamicItem: [{
          content: '',
        }],
        name: "",
        description: "",
        difficulty: "简单",
        labels: "",
      },
      userForm: {
        userId: "",
        newName: "",
      },
      needRevise: false,
      UploadQ: false,
      pageSize: 8,
      page: 1,
      currentPage: 1,
      keywords: '',
      filterDataShow: [],
      tableData: [],
      pageSize1: 8,
      page1: 1,
      currentPage1: 1,
      keywords1: '',
      filterDataShow1: [],
      tableData1: [],
    }
  },
  beforeRouteEnter: (to, from, next) => {
    if (window.sessionStorage.getItem("type") !== '0') {
      ElNotification({
        title: '失败',
        message: '权限错误！',
        type: 'warning',
      })
      this.$router.replace({path: '/hdoj/bank'})
    } else {
      next(vm => {
        vm.getAllQuestion();
        vm.getAllUser();
      })
    }
  },
  methods: {
    startRevise(val) {
      this.userForm.userId = val;
      this.needRevise = true;
    },
    confirmUpload() {
      this.UploadQ = false;
      for (let i = 0; i < this.form.dynamicItem.length; i++) {
        let newItem = this.form.dynamicItem[i].content;
        this.form.labels += "," + newItem;
      }
      this.$http.post("http://localhost:8081/question/addQuestion", this.form).then(res => {
        ElNotification({
          title: '成功',
          message: '题目已上传',
          type: 'success',
        })
      })
    },
    addItem() {
      this.form.dynamicItem.push({
        content: '',
      })
    },
    deleteItem(item, index) {
      this.form.dynamicItem.splice(index, 1)
    },
    getAllQuestion() {
      this.$http.post("http://localhost:8081/question/listQuestions")
          .then(res => {
            this.tableData = []
            for (let i = 0; i < res.data.data.length; i++) {
              this.tableData.push({
                questionId: res.data.data[i].questionId,
                questionName: res.data.data[i].name,
                difficulty: res.data.data[i].difficulty
              })
            }
            this.filterDataShow = this.tableData;
          }).catch(err => {
        console.log(err);
      })
    },
    getAllUser() {
      this.$http.post("http://localhost:8081/account/getAllAccounts")
          .then(res => {
            this.tableData1 = []
            for (let i = 0; i < res.data.data.length; i++) {
              this.tableData1.push({
                userId: res.data.data[i].userId,
                userName: res.data.data[i].username,
                email: res.data.data[i].email
              })
            }
            this.filterDataShow1 = this.tableData1;
          }).catch(err => {
        console.log(err);
      })
    },
    sortByDifficulty(obj1, obj2) {
      let val1;
      let val2;
      switch (obj1.difficulty) {
        case "困难":
          val1 = 3;
          break;
        case "中等":
          val1 = 2;
          break;
        case "简单":
          val1 = 1;
          break;
      }
      switch (obj2.difficulty) {
        case "困难":
          val2 = 3;
          break;
        case "中等":
          val2 = 2;
          break;
        case "简单":
          val2 = 1;
          break;
      }
      return val1 - val2;
    },
    goToRead(id) {
      window.localStorage.setItem("questionId", id);
      this.$router.push("/hdoj/bank/q/" + id);
    },
    goToRevise(userId) {
      console.log(userId)
      console.log(this.userForm.newName, "ohhh")
      this.needRevise = false;
      this.$http({
        method: 'post',
        url: "http://localhost:8081/account/updateInfo",
        data: {
          "userId": userId,
          "username": this.userForm.newName,
        }
      }).then(res => {
        ElNotification({
          title: '成功',
          message: '信息已更改',
          type: 'success',
        })
        this.getAllUser()
      })
    },
    searchResource() {
      this.currentPage = 1; //将当前页设置为1，确保每次都是从第一页开始搜
      let filterKeywords = this.keywords.trim();
      this.tableData = this.filterDataShow.filter(item => {
        if (item.questionName.includes(filterKeywords) || item.questionId.toString().includes(filterKeywords)) {
          return item
        }
      })
    },
    searchResource1() {
      this.currentPage1 = 1; //将当前页设置为1，确保每次都是从第一页开始搜
      let filterKeywords = this.keywords1.trim();
      this.tableData1 = this.filterDataShow1.filter(item => {
        if (item.userName.includes(filterKeywords) || item.userId.toString().includes(filterKeywords)) {
          return item
        }
      })
    },
    deleteQ(id) {
      this.$http.delete("http://localhost:8081/question/deleteQuestion/" + id).then(res => {
        ElNotification({
          title: '成功',
          message: '题目已删除',
          type: 'success',
        })
        this.getAllQuestion();
      })
    },
    deleteUser(id) {
      this.$http({
        method: "delete",
        url: "http://localhost:8081/account/deleteAccount",
        params: {"userId": id}
      }).then(res => {
        ElNotification({
          title: '成功',
          message: '用户已删除',
          type: 'success',
        })
        this.getAllUser();
      })
    },
    handleCurrentChange(val) {
      this.page = val;
      this.currentPage = val;
    },
    handleCurrentChange1(val) {
      this.page1 = val;
      this.currentPage1 = val;
    },
    indexMethod(index) {
      return (this.currentPage - 1) * this.pageSize + index + 1
    }
  }
}

</script>

<style scoped>

.bg {
  height: 100%;
  width: 100%;
  background: url("../assets/bg.jpg") center center no-repeat;
  background-size: cover;
  position: fixed;
  z-index: -100;
}

.fy {
  text-align: center;
  margin-top: 30px;
}

</style>
<template>
  <el-dialog title="输入参数" v-model="dialogVisible">
    <el-input v-model="input" autosize type="textarea" style="width: 70%" placeholder="请输入要测试的参数">
    </el-input>
    <div slot="footer" class="dialog-footer" style="padding-top: 30px">
      <el-button @click="dialogVisible = false">取 消</el-button>
      <el-button type="primary" @click="runCode">确 定</el-button>
    </div>
  </el-dialog>
  <el-container>
    <el-container>
      <el-aside width="50%">
        <el-tabs type="border-card" style="height: 100%" @tab-click="changeTabs">
          <el-tab-pane label="题目描述" style="text-align: left;">
            <div style="font-size:40px">
              {{this.id}}
              {{this.questionname}}
            </div>
            <el-divider></el-divider>
            {{this.questiondescription}}
            <el-divider></el-divider>
            题目难度：
            <el-tag v-for="tag in tags" :key="tag.name" :type="tag.type">
              {{tag.name}}
            </el-tag>
            <br><br>
            题目标签：
            <el-tag v-for="tag in tags0" :key="tag">
              {{tag}}
            </el-tag>
            <el-divider></el-divider>
            <div v-for="sample in samples">
              样例输入：<span style="color:orangered;width:300px; display:inline-block;">{{sample.input}}</span>
              输出：<span style="color:green; display:inline-block;">{{sample.output}}</span>
            </div>
          </el-tab-pane>
          <!--          <el-tab-pane label="题解">-->
          <!--            <Solution />-->
          <!--          </el-tab-pane>-->
          <el-tab-pane label="提交记录">
            <el-card style="">
              <el-card-content v-if="this.result!=''">
                <!-- 运行成功 -->
                <div v-if="this.result.code == 200">
                  执行结果： <span>通过</span><br>
                  执行用时： <span>{{this.result.data.time}}</span>ms<br>
                  通过测试用例： <span>{{this.result.data.allCases}}</span> / <span>{{this.result.data.allCases}}</span><br>
                </div>
                <!-- 运行失败 -->
                <div v-else>
                  执行结果： <span>未通过</span><br>
                  执行用时： <span>{{this.result.data.time}}</span><br>
                  通过测试用例： <span>{{this.result.data.testCases}}</span> / <span>{{this.result.data.allCases}}</span><br>
                  未通过用例： <span>{{this.result.data.input}}</span><br>
                  错误信息： <span>{{this.result.data.errorMessage}}</span><br>
                </div>
              </el-card-content>
              <el-card-content class="status">
                <el-table :data="tableData" style="width: 100%;left: 15%;" :row-class-name="tableRowClassName">
                  <el-table-column label="id" align="center" prop="id" v-if="false" />
                  <el-table-column prop="state" label="提交结果" width="100">
                  </el-table-column>
                  <el-table-column prop="language" label="语言" width="100">
                  </el-table-column>
                  <el-table-column prop="submitTime" label="提交时间" sortable width="150">
                  </el-table-column>
                </el-table>
              </el-card-content>
            </el-card>
          </el-tab-pane>
        </el-tabs>
      </el-aside>
      <el-main style="overflow:hidden">
        <MonacoEditor ref="monacoEditor" :myArgs="myArgs"/>
      </el-main>
    </el-container>
    <el-divider></el-divider>
    <el-footer>
      <el-row :gutter="20">
        <el-col :span="8">
          <div class="grid-content">
          </div>
        </el-col>
        <el-col :span="2" :offset="1">
          <div class="grid-content">
          </div>
        </el-col>
        <el-col :span="2">
          <div class="grid-content">
          </div>
        </el-col>
        <el-col :span="2">
          <div class="grid-content">
          </div>
        </el-col>
        <el-col :span="2" :offset="2">
          <div class="grid-content">
            <el-button size="middle" @click="dialogVisible=true">执行代码</el-button>
          </div>
        </el-col>
        <el-col :span="2">
          <div class="grid-content">
            <el-button size="middle" @click="submit">提交</el-button>
          </div>
        </el-col>
      </el-row>
    </el-footer>
  </el-container>

</template>

<script>

import MonacoEditor from '../../components/MonacoEditor'

export default {
  name:'Question',
  data () {
    return {
      myArgs: {},
      form: {},
      current: '',
      id: '',
      // nowPage: '1',
      maxPage: '200',
      tableData: [],
      code: '',
      input: '',
      dialogVisible: false,
      // dialogNewVisible: false,
      questionname: '',
      questiondescription: '',
      tags: [],
      tags0: [],
      samples: [],
      result: '',
      isPass: '通过'
    }
  },
  components: {
    MonacoEditor,
  },
  methods: {
    changeTabs (tab, e) {
      console.log(tab.index)
      if (tab.index == 1)
        this.current = true
      else
        this.current = false
      console.log(this.current)
    },

    submit () {
      var newcode=this.$refs.monacoEditor.code.replaceAll("\r", "\\r").replaceAll("\n", "\\n");
      this.language=this.$refs.monacoEditor.languageSelect;
      console.log({
        "code": this.code,
        'questionId': this.id,
      })
      if (this.code == null) {
        alert("请写点代码再传好吗")
        return
      }
      this.$http.post("/question/check", {
        "code": newcode,
        'questionId': this.id,
        "language": this.language,
      }).then(res => {
        console.log(res)
        document.getElementById("tab-2").click()
        this.result = res.data
        if (this.result.code == 200)
          this.isPass = '通过'
        else
          this.isPass = '未通过'
      }).catch(err => {
        console.log(err)
      })
    },
    runCode () {
      this.$refs.monacoEditor.ArgsIn=this.input.replaceAll("\r", "\\r").replaceAll("\n", "\\n");
      this.$refs.monacoEditor.getCompile();// 调用子组件的提交
      this.dialogVisible = false
    },
    handleCode (c) {
      this.code = c
    },
    questionquery () {
      var token = JSON.parse(localStorage.token)
      console.log("this.id", this.id)
      this.$http.post("/question/getQuestion?questionId=" + this.id)
          .then(res => {
            this.questionname = res.data.data.name
            this.questiondescription = res.data.data.description
            if (res.data.data.difficulty == "简单") this.tags[0] = { name: '简单', type: 'success' }
            else if (res.data.data.difficulty == "中等") this.tags[0] = { name: '中等', type: 'warning' }
            else if (res.data.data.difficulty == "困难") this.tags[0] = { name: '困难', type: 'danger' }
            this.tags0 = res.data.data.labels.split(",")
            this.myArgs = [res.data.data.returnType, res.data.data.argsType, token.id, this.id]
          })
    },
    samplequery () {
      this.$http.post("/sample/listSamples?questionId=" + this.id)
          .then(res => {
            this.samples = res.data.data
          })
    }
  },
  mounted () {
    // this.id = this.$route.params.id
    this.id=1
    // this.$refs.nowPage.$el.innerHTML = this.id
    // this.$refs.maxPage.$el.innerHTML = JSON.parse(window.localStorage.getItem("maxId"))
    // window.localStorage.setItem("currentQuestionId", this.id)
    // this.loaddata()
    this.questionquery()
    this.samplequery()
  }
}
</script>

<style>
.el-aside {
  height: auto;
}
.el-main {
  height: 77vh;
  /* background-color: lightgrey; */
}
.el-tag {
  margin-right: 10px;
  border-radius: 50%;
}
</style>

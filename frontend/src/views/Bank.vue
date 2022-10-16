<template>
  <div class="home1" >
        <div>
          <el-carousel indicator-position="outside" style="text-align: center;">
            <el-carousel-item v-for="item in imgList" :key="item.id">
              <img :src="item.idView">
            </el-carousel-item>
          </el-carousel>
        </div>

        <div class="questions">
          <el-row  style="margin-left: 65vh" >
<!--            <el-col :span="7"></el-col>-->
<!--            <el-col :span="2">-->
<div class="grid-content bg-purple">
                <el-select v-model="difficulty"  placeholder="难度" @change="dChange()" style=" width: 20vh;">
                  <el-option
                      v-for="item in difficultyOptions"
                      :key="item.value"
                      :label="item.label"
                      :value="item.value">
                  </el-option>
                </el-select>
            </div>
<!--    </el-col>-->
<!--            <el-col :span="2">-->
<div class="grid-content bg-purple">
                <el-select v-model="state" placeholder="状态" @change="sChange()"  style=" width: 20vh;">
                  <el-option
                      v-for="item in stateOptions"
                      :key="item.value"
                      :label="item.label"
                      :value="item.value">
                  </el-option>
                </el-select>
            </div>
<!--  </el-col>-->
<!--            <el-col :span="2">-->
              <div class="grid-content bg-purple">
              <el-select v-model="qTag" placeholder="标签" @change="tChange()"  style=" width: 20vh;">
                  <el-option
                      v-for="item in qTagOptions"
                      :key="item.value"
                      :label="item.label"
                      :value="item.value">
                  </el-option>
                </el-select>
            </div>
<!--            </el-col>-->
<!--            <el-col :span="4">-->
              <div class="grid-content bg-purple">
              <el-input
                  style=" width: 25vh;"
                  v-model="input"
                  placeholder="请输入内容"
                  clearable>
                <template #prefix>
                  <el-icon class="el-input__icon"><search /></el-icon>
                </template>
              </el-input>
            </div>
<!--            </el-col>-->
<!--            <el-col :span="4"></el-col>-->
          </el-row>
          <el-tag
              v-for="tag in tags"
              :key="tag.text"
              closable
              :type="tag.type"
              @close="handleClose(tag)">
            {{tag.text}}
          </el-tag>
        </div>


        <div class="question" >
            <el-table
                @row-click="openDetails"
                :data="tables.slice((this.currentPage - 1) * this.pageSize,this.currentPage * this.pageSize)"
                style="width: 100%; padding-left: 25%"
                :row-class-name="tableRowClassNameA"
                max-height="530px">
              <el-table-column label="id" align="center" prop="id" v-if="false" />
              <el-table-column label="labels" align="center" prop="labels" v-if="false"/>
              <el-table-column
                  prop="state"
                  label="状态"
                  width="100%">
              </el-table-column>
              <el-table-column
                  prop="name"
                  label="题目"
                  sortable
                  width="300%">
              </el-table-column>
              <el-table-column
                  prop="solution"
                  label="题解"
                  sortable
                  width="100%">
              </el-table-column>
              <el-table-column
                  prop="passingRate"
                  label="通过率"
                  sortable
                  width="100%">
              </el-table-column>
              <el-table-column
                  prop="difficulty"
                  label="难度"
                  sortable
                  :sort-method="sortByDifficulty"
                  width="150%">
                <template #default="scope">
                  <span v-if="scope.row.difficulty == '简单'" style="color: lightgreen;">{{scope.row.difficulty}}</span>
                  <span v-else-if="scope.row.difficulty == '中等'" style="color: lightsalmon;">{{scope.row.difficulty}}</span>
                  <span v-else-if="scope.row.difficulty == '困难'" style="color: lightcoral;">{{scope.row.difficulty}}</span>
                  <span v-else>{{scope.row.difficulty}}</span>
                </template>
              </el-table-column>
            </el-table>
          <el-pagination
              style="text-align: center"
              @size-change="handleSizeChange"
              @current-change="handleCurrentChange"
              :default-page-size="3"
              :current-page="currentPage"
              :page-size="pageSize"
              layout="total, prev, pager, next, jumper"
              :total="tables.length"
              >
          </el-pagination>
      </div>
  </div>
</template>

<script>
import {Search} from '@element-plus/icons-vue'
export default {
  components:{
    Search,
  },
  data() {
    return {
      // colorz:1,
      imgList: [
        {
          id:0,
          idView:require("@/assets/hdoj.png")
        },
        //TODO:添加其他编程学习平台或我们自己功能的广告画
      ],
      tags: [],
      month:'',
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
      state: '' ,
      input: '' ,      //     {
      //   id:'1',
      //   state:'未尝试(--`)',
      //   name:'大整数加法(迪，人)',
      //   solution:'7',
      //   passingRate:'50%',
      //   difficulty:'简单',
      //   labels:[
      //       "迪杰斯特拉",
      //       "人工智能"
      //     ]
      // },{
      //   id:'1',
      //   state:'在做了ing',
      //   name:'大整数加法(迪，最)',
      //   solution:'7',
      //   passingRate:'50%',
      //   difficulty:'中等',
      //   labels:[
      //     "迪杰斯特拉",
      //     "最短路径"
      //   ]
      // },{
      //   id:'1',
      //   state:'在做了ing',
      //   name:'大整数加法(迪)',
      //   solution:'1',
      //   passingRate:'50%',
      //   difficulty:'困难',
      //   labels:[
      //     "迪杰斯特拉",
      //   ]
      // },{
      //   id:'9',
      //   state:'已解答√',
      //   name:'两数之和(数，概)',
      //   solution:'1',
      //   passingRate:'60%',
      //   difficulty:'中等',
      //   labels:[
      //       "数学分析",
      //       "概率统计"
      //   ]
      // },
      tableData:[

      ],
      qTag:'',
      qTagOptions:[
        { value: '迪杰斯特拉', label: '迪杰斯特拉' },
        { value: '人工智能', label: '人工智能' },
        { value: '最短路径', label: '最短路径' },
        { value: '概率统计', label: '概率统计' },
        { value: '数学分析', label: '数学分析' },
        { value: '随便写个', label: '随便写个' },
      ],
      pageSize: 10, //每页多少条
      currentPage: 1, // 当前页
    };
  },
  computed:{
    tables(){
      const search=this.input;
      if(search||this.tags.length>0){
        let table=(this.tableData.filter(dataNews => {
          return Object.keys(dataNews).some(key => {
            return String(dataNews[key]).toLowerCase().indexOf(search) > -1
          })
        }))
        console.log(table);
        for(let i=0;i<table.length;i++){
          if(!this.filterTag(table[i])){
            table.splice(i,1);
            i--;
          }
        }
        return table;
      }
      return this.tableData;
    }
  },

  methods: {
    tableRowClassNameA({row, rowIndex}) {
      return '';
    },
    handleSizeChange(val){
      console.log(`${val} items per page`)
      this.pageSize=val;
    },
    handleCurrentChange(val){
      console.log(`current page: ${val}`)
      this.currentPage=val;
    },
    sortByDifficulty(obj1,obj2){
      let val1;
      let val2;
      switch (obj1.difficulty){
        case "困难":
          val1=3;
          break;
        case "中等":
          val1=2;
          break;
        case "简单":
          val1=1;
          break;
      }
      switch (obj2.difficulty){
        case "困难":
          val2=3;
          break;
        case "中等":
          val2=2;
          break;
        case "简单":
          val2=1;
          break;
      }
      return val1-val2;
    },
    goWeb(url){
      window.location.href=url;
    },
    toMaterial(val){
      if(val==0)
        this.$router.replace("/hdoj/material");
    },
    select(val){
      this.$refs.calendar.selectDate(val);
    },
    filterTag(row) {
      if(this.tags.length==0)
        return true;
      else{
        for(let i=0;i<this.tags.length;i++){
          switch(this.tags[i].value){
            case '状态':
              if(row.state!=this.tags[i].text&&row.state!="")
                return false;
              break;
            case '难度':
              if(row.difficulty!=this.tags[i].text)
                return false;
              break;
            case '标签':
              let t=0;
              for(let j=0;j<row.labels.length;j++) {
                if (row.labels[j] == this.tags[i].text){
                  t++;
                }
              }
              if(t==0)
                return false;
              break;
          }
        }
      }
      return true;
    },
    openDetails(row){
      window.localStorage.setItem("questionId",row.id);
      // window.localStorage.setItem("colorCount","1");
      this.$router.push("/hdoj/bank/q/"+row.id);
    },
    handleClose(tag) {
      this.tags.splice(this.tags.indexOf(tag), 1);
    },
    dChange(){
      for(let i=0;i<this.tags.length;i++){//先删除之前的标签
        if(this.tags[i].value=="难度"){
          this.tags.splice(i,1);
          break;
        }
      }
      switch (this.difficulty){
        case "简单":
          this.tags.push({text: '简单', value:'难度', type: 'success'});
          break;
        case "中等":
          this.tags.push({text: '中等', value:'难度', type: 'warning'});
          break;
        case "困难":
          this.tags.push({text: '困难', value:'难度', type: 'danger' });
          break;
      }
      this.difficulty=null;
    },
    sChange(){
      for(let i=0;i<this.tags.length;i++){//先删除之前的标签
        if(this.tags[i].value=="状态"){
          this.tags.splice(i,1);
          break;
        }
      }
      switch (this.state){
        case "未尝试(--`)":
          this.tags.push({text: '未尝试(--`)',value:'状态', type: 'info'});
          break;
        case "在做了ing":
          this.tags.push({text: '在做了ing',value:'状态', type: 'info'});
          break;
        case "已解答√":
          this.tags.push({text: '已解答√',value:'状态', type: 'info'});
          break;
      }
      this.state=null;
    },
    tChange(){
      this.tags.push({text: this.qTag, value:'标签', type: 'info'});
      this.qTag=null;
    },
    tableRowClassName({row, rowIndex}) {
      if (rowIndex%2 == 0)
        return 'success-row';
      else
        return 'warning-row';
    },
  },
  mounted() {
    this.$http.get("/question/listQuestions")
        .then(res =>{
          this.tableData.splice(0,this.tableData.length);
          const pass=[];
          console.log(res.data.length,"length here");
          window.localStorage.setItem("maxId",res.data[res.data.length-1].questionId);
          for(let i=0;i<res.data.length;i++){
            if(res.data[i].submission==0)
              pass[i]='--';
            else
              pass[i]=Math.trunc(res.data[i].pass/res.data[i].submission*100)+"%";
            this.tableData.push({
                'id':res.data[i].questionId,
                'state':'未尝试(--`)',
                'name':res.data[i].questionId+':'+res.data[i].name,
                'solution':0,
                'passingRate':pass[i],
                'difficulty':res.data[i].difficulty,
                'labels':res.data[i].labels.split(',')
            })
            this.$http.get("/code/getTheLatestCode?" +
                "userId="+JSON.parse(window.localStorage.getItem("token")).id
                + "&questionId=" +this.tableData[i].id)
            .then(res =>{
              if(res.data.data!=null){
                if(res.data.data.state==0)
                  this.tableData[i].state="在做了ing";
                else
                  this.tableData[i].state="已解答√";
              }
            }).catch(err => {

            })
          }
          this.$http.get("/solution/getSolutionCount")
              .then(res =>{
                for(let i=0;i<res.data.length;i++){
                  this.tableData[i].solution=res.data[i];
                }
              }).catch(err => {
            console.log(err);
          })
          console.log(this.tableData)
          this.qTagOptions.splice(0,this.qTagOptions.length)
          for(let i=0;i<this.tableData.length;i++){
            for(let j=0;j<this.tableData[i].labels.length;j++){
              this.qTagOptions.push({ value: this.tableData[i].labels[j], label: this.tableData[i].labels[j] })
            }
          }
          for (let i = 0; i < this.qTagOptions.length; i++) {//遍历删除重复标签
            for (let j = i+1; j < this.qTagOptions.length; j++) {
              if (this.qTagOptions[i].label == this.qTagOptions[j].label) {
                this.qTagOptions.splice(j,1);
                j--;
              }
            }
          }
          for (let i = 0; i < this.qTagOptions.length; i++) {//遍历删除重复标签
            if (this.qTagOptions[i].label == '') {
              this.qTagOptions.splice(i,1);
              break;
            }
          }
          console.log(this.qTagOptions)
        }).catch(err => {
          console.log(err);
        })
  },
  // beforeCreate() {
  //   let color=window.localStorage.getItem("colorCount")
  //   if(color==="1") {
  //     window.localStorage.setItem("colorCount", "0")
  //     location.reload();
  //   }
  // },
}
</script>

<style >


.el-table tbody tr:hover>td {
  background-color:lightblue!important
}

.el-table .success-row {
  background: white;
}

/*.el-col {*/
/*  border-radius: 4px;*/
/*}*/
.el-table__row:hover {
  cursor: pointer;
}

.bg-purple {
  /*background: #d3dce6;*/
}

.grid-content {
  margin:0px 2px;
  border-radius: 4px;
  min-height: 36px;
}

</style>

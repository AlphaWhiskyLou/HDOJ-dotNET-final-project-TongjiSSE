<template>
  <div class="carousel">
    <el-carousel :interval="4000" type="card" height="50vh">
      <el-carousel-item v-for="item in tableData" :key="item.id">
        <div @click="submitForm(item.labels)">
          <el-card class="box-card">
            <div slot="header" class="clearfix" >
              <span class="text_header">{{item.labels}}</span>
            </div>
            <div class="text item"  >
              {{item.counts}}
            </div>
          </el-card>
        </div>
      </el-carousel-item>
    </el-carousel>
  </div>
</template>

<script>

import axios from "axios";

export default {
  name: "TrainModel",
  data(){
    return{
      tableData:[
        // {
        //   id:1,
        //   name:'基础知识',
        //   currentPass:4,
        //   totalCount:11,
        // },
        // {
        //   id:2,
        //   name:'搜索',
        //   currentPass:0,
        //   totalCount:12,
        // },
        // {
        //   id:3,
        //   name:'数据结构',
        //   currentPass:9,
        //   totalCount:16,
        // },
        // {
        //   id:4,
        //   name:'图论',
        //   currentPass:0,
        //   totalCount:15,
        // },
        // {
        //   id:5,
        //   name:'数学知识',
        //   currentPass:0,
        //   totalCount:12,
        // },
      ]
    }
  },
  mounted(){
    this.getLabelsAndCount()
  },
  methods: {
    getLabelsAndCount(){
      axios.post('/question/getLabelsAndCount')
      .then(({data})=>{
        console.log('call getLabelsAndCount')
        console.log(data)
        this.tableData=data.data
      })
      .catch((err)=>{
        console.log(err)
      })
    },
    submitForm(labels) {
      console.log('test')
      this.$router.push({
        path:'/acSaber/questionList',
        query:{
          labels:labels,
        }
      })
    }
  }
}
</script>

<style scoped>
.carousel {
  width: 350px;
  margin: 69px auto
}

/*el-carousel {*/
/*  margin: 0 auto;*/
/*}*/

.el-carousel__item h3 {
  color: #475669;
  font-size: 14px;
  opacity: 0.75;
  margin: 0;
}

.el-carousel__item:nth-child(2n) {
  background-color: #99a9bf;
}

.el-carousel__item:nth-child(2n+1) {
  background-color: #d3dce6;
}

.text_header {
  font-size: 2.5vh;
  font-family: "Helvetica Neue", Helvetica, Arial, sans-serif;
  color: #FFFFFF;
}

.item {
  text-align: center;
  font-size: 2.5vh;
  color: #28354F;
  line-height: 108px;

}

.clearfix:before,
.clearfix:after {
  display: table;
  content: "";
}

.clearfix:after {
  clear: both
}

.clearfix {
  height: 108px;
  line-height: 108px;
}

.box-card {
  height: 100vh;
  cursor: pointer;
  text-align: center;
  background: #00cec9;
}
</style>
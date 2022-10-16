<template>
  <div class="mimgs">
    <el-header><el-row class="demo-autocomplete">
      <el-col :span="12" style="margin-left:50px;width: 650px !important;">
        <el-autocomplete
            class="inline-input"
            v-model="contentData"
            style="margin-left:450px;width: 500px !important;"
            :fetch-suggestions="querySearch"
            placeholder="请输入内容进行搜索"
        ></el-autocomplete>
      </el-col>
      <el-button @click="searchFile" style="height:40px; background-color: #b3c0d1;margin-left:220px; margin-top:10px; width:100px;">资料搜索</el-button>
    </el-row>
    </el-header>


    <el-carousel slot-scope="scope" :interval="4000" type="card" height="300px" style="margin-top:20px;" ref="carousel" >
    <el-carousel-item v-for="item in imgs" :key="item.url">
      <img :src="item.url" @click.native="linkTo(item.link,item.name)" style=";height:300px;"/>
    </el-carousel-item>
  </el-carousel>
    <div class="question" style="margin-top:40px;">
      <hr style="margin-top :-15px;">
      <el-table
          ref="singleTable"
          @row-click="openDetails"
          :data="tableData"
          style="width: 100%; padding-left: 20%;padding-right: 5%;border-radius: 20px;"
          :row-class-name="tableRowClassName"
      >
        <el-table-column label="referenceId" align="center" prop="id" v-if="false" />
        <el-table-column
            prop="name"
            label="资料名"
            sortable
            width="250">
        </el-table-column>
        <el-table-column
            prop="introduction"
            label="简介"
            sortable
            width="660">
        </el-table-column>

        <el-table-column
            slot-scope="scope"
            prop="download"
            label=""
            sortable
            width="160">
          <template #default="scope">
            <div @click="downloadFile(scope)" style="color: #3491FA;cursor: pointer;">点击下载</div>
          </template>
        </el-table-column>


      </el-table>
    </div>
  </div>
</template>

<script>
import axios from "axios";

export default {
  data() {
    return {
      contentData:'',
      tableData:[{
        referenceId:'7',
        name:'数据挖掘十大算法',
        introduction:'You can find everything you need to know about data mining',
        download:'点击下载',
        url:'http://myweb.sabanciuniv.edu/rdehkharghani/files/2016/02/The-Morgan-Kaufmann-Series-in-Data-Management-Systems-Jiawei-Han-Micheline-Kamber-Jian-Pei-Data-Mining.-Concepts-and-Techniques-3rd-Edition-Morgan-Kaufmann-2011.pdf'
      },{
        referenceId:'8',
        name:'Discrete Mathematics',
        introduction:'This text aims to give an introduction to select topics in discrete mathe- matics at a level appropriate for first or second year undergraduate math majors',
        download:'点击下载',
        url:'http://discrete.openmathbooks.org/pdfs/dmoi-tablet.pdf'

      },{
        referenceId:'4',
        name:'数据库原理与应用',
        introduction:'A database is a collection of information organized to provide efficient retrieval.',
        download:'点击下载',
        url:'http://index-of.es/Databases/Databases%20A%20Beginners%20Guide.pdf'

      },
        {
          id:'2',
          name:'软件工程',
          introduction:'Software Engineering encompasses a process, a collec- tion of methods (practice) and an array of tools that allow profession-\n' +
              'als to build high-quality computer software.',
          url:'http://engineering.futureuniversity.com/BOOKS%20FOR%20IT/Software-Engineering-9th-Edition-by-Ian-Sommerville.pdf'
        }],
      imgs:[{
          name:'数据挖掘十大算法',
          id:'1',
          url:'https://gimg2.baidu.com/image_search/src=http%3A%2F%2Fimgservice.suning.cn%2Fuimg1%2Fb2c%2Fimage%2FkVbS2xUghxr1pRttX-d8MA.jpg_800w_800h_4e&refer=http%3A%2F%2Fimgservice.suning.cn&app=2002&size=f9999,10000&q=a80&n=0&g=0n&fmt=jpeg?sec=1644046631&t=3509164239738935c80386b7eb7d665d',
          link:'http://myweb.sabanciuniv.edu/rdehkharghani/files/2016/02/The-Morgan-Kaufmann-Series-in-Data-Management-Systems-Jiawei-Han-Micheline-Kamber-Jian-Pei-Data-Mining.-Concepts-and-Techniques-3rd-Edition-Morgan-Kaufmann-2011.pdf'
        },
        {
          id:'1',
          name:'软件工程',
          url:'https://gimg2.baidu.com/image_search/src=http%3A%2F%2Fs.cmpedu.com%2Fimages%2Fupload%2F2019%2F4%2F7%2F1558374105335%28FM800%29.jpg&refer=http%3A%2F%2Fs.cmpedu.com&app=2002&size=f9999,10000&q=a80&n=0&g=0n&fmt=jpeg?sec=1644046970&t=25ffc308763d535d8c46c3f7b21360a3',
          link:'http://engineering.futureuniversity.com/BOOKS%20FOR%20IT/Software-Engineering-9th-Edition-by-Ian-Sommerville.pdf'
        },
        {
          id:'1',
          name:'数据库原理与应用',
          url:'https://img14.360buyimg.com/pop/g6/M03/07/00/rBEGDFD405kIAAAAAAmnYpMXTPIAABXjwBmwCAACad6712.jpg',
          link:'http://index-of.es/Databases/Databases%20A%20Beginners%20Guide.pdf'
        },
        {
          id:'1',
          name:'Discrete Math',
          url:'https://img14.360buyimg.com/pop/jfs/t1/4498/34/7790/57749/5ba70a66Ecbc4e5da/010868f8ff49ba52.jpg',
          link:'http://discrete.openmathbooks.org/pdfs/dmoi-tablet.pdf'
        }]
    };
  },
  methods: {



searchFile()
    {
      axios.post(
          "/reference/searchReference?searchKey="+this.contentData
      ).then(res => {
        this.tableData.splice(0,this.tableData.length)
        console.log(res)
        for(var i = 0;i<res.data.data.length;i++)
        {
          this.tableData.push(res.data.data[i])
        }

      })
    },
    downloadFile(i){
      // console.log(i.row.url)
      let url = i.row.url
      window.open(url)
      // this.linkTo(this.tableData[i].url,this.tableData[i].name)
    },
    linkTo(i,bookName){

      if(confirm("您要下载 "+bookName+" 吗？"))
      this.download(i,bookName)

    },
    download (src, fileName) {
      let x = new XMLHttpRequest();
      x.open("GET", src, true);
      x.responseType = 'blob';
      x.onload = function(e) {
        let url = window.URL.createObjectURL(x.response)
        let a = document.createElement('a');
        a.href = url
        a.download = fileName
        a.click()
      }
      x.send();
    }

  }
}
</script>

<style scoped>

.photo
{
  margin-bottom: 20px;
  height: 200px;
  margin-right: 80px;
}
#info
{

  text-align: left;
  height: 40px;
}
#p1
{
  height: 140px;
}
.el-header
{
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
  width:200px;
  border-radius: 30px;
  margin :20px;

}
.personalInfoPanel{


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

.personalInfoTitle{
  width:180px;font-size: 16px;line-height: 20px;
  flex-grow: 0;
  flex-shrink: 0;
  flex-basis: auto;
  display:inline;
  color: darkgrey;
  margin-right: 6px;
  margin-left: 6px;
}

.personalInfoContent{
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
  display:inline;
}
</style>
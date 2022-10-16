<template>
  <el-select v-model="languageSelect" size="mini" @change="changeCheckWay">
    <el-option
        v-for="item in options"
        :key="item.value"
        :label="item.label"
        :value="item.value">
    </el-option>
  </el-select>
<!--  <el-button type="primary" @click="getCompile" size="mini">立即提交</el-button>-->

  <MonacoEditor
      ref="monacoEditorIn"
      class="editor"
      v-if="fflush"
      v-model="code"

      @editorDidMount="editorDidMount"
      @change="change"
      @editorWillMount="editorWillMount"
      :language="languageSelect"
      :value="code"
  />


</template>

<script>
import MonacoEditor from 'vue-monaco'
import axios from "axios";

export default {
  components: {
    MonacoEditor
  },
  data() {
    return {
      options: [{
        value: 'java',
        label: 'Java'
      }, {
        value: 'c',
        label: 'C'
      }, {
        value: 'cpp',
        label: 'C++'
      }, {
        value: 'php',
        label: 'PHP'
      }, {
        value: 'perl',
        label: 'Perl'
      }, {
        value: 'python3',
        label: 'Python3'
      }],
     languageSelect: "cpp",
      code: '',
      fflush:true,
      ArgsIn:'',
    }
  },

  methods: {
    changeCheckWay(item){
      console.log(item);
      //location.reload();
      this.fflush = false
      this.$nextTick(() => {
        this.fflush = true
      })
      console.log("language:"+this.languageSelect);
    },
    fflushComponent(){
      this.fflush = false
      this.$nextTick(() => {
        this.fflush = true
      })
    },
    change(value, event) {
    //TODO:We need to find a way to dynamically change the editor language
    },
    editorDidMount(editor) {
      editor.onKeyUp(e => {
        this.code = editor.getValue()
        // console.log(this.code)
      })
    },
    getCompile(editor){
      const rightCode = this.code.replaceAll("\r", "\\r").replaceAll("\n","\\n");
      axios({
        url:'/compile/doCompile',
        method:'POST',
        data:{
          script : rightCode,
          language: this.languageSelect,
          versionIndex: "0",
          stdin: this.ArgsIn,
        }
      }).then(response => {
        this.$confirm(response.data, {
          confirmButtonText: '确定',
        });
            console.log(response.data)
          })
    }
  },
}
</script>

<style>
.editor {
  width: 800px;
  height: 1000px;
  /*width: 100%;*/
  /*height: 100%;*/
  text-align: left;
}
.monaco-editor{
  width: 800px;
  height: 1000px;
}
</style>

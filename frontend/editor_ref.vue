 <template>
   <el-select v-model="value" placeholder="Java" size="mini">
     <el-option
         v-for="item in options"
         :key="item.value"
         :label="item.label"
         :value="item.value">
     </el-option>
   </el-select>
  <div ref="container" class="container"></div>
</template>

 <script>
import * as monaco from "monaco-editor"
import { ref } from '@vue/reactivity'
import { onMounted, watch } from '@vue/runtime-core'
import axios from "axios"

export default {
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
      value: ''
    }
  },
  props: [
    "myArgs"
  ],
  setup (props, ctx) {
    const data = ref('')
    const container = ref(null)
    const myArgs = ref('')
    const content = ref('')
    onMounted(() => {
      var editor = monaco.editor.create(container.value, {
        language: 'java',
        minimap: {
          enabled: false
        },
        selectOnLineNumbers: true,
        roundedSelection: false,
        cursorStyle: 'line', // 光标样式
        automaticLayout: false, // 自动布局
        glyphMargin: true, // 字形边缘
        useTabStops: false,
        fontSize: 16, // 字体大小
        autoIndent: false, //自动布局
        wordWrap: 'on',
      })

      watch(
        () => props.myArgs,
        (val, pervVal) => {
          myArgs.value = val
          let type = myArgs.value
          var args = ''
          var returnT = type[0]
          if (type != null) {
            let t = type[1].split(",")
            if (t.length == 2)
              args = t[0] + " arg1," + t[1] + " arg2"
            else
              args = t[0] + " arg1"
          }
          content.value = "public class Run {\n"
            + "    public " + returnT + " run(" + args + ") {\n"
            + "        \n"
            + "    }\n"
            + "}"
          axios.post("/code/getTheLatestCode?userId=" + type[2] + "&questionId=" + type[3]) //TODO:NEED TO IMPLEMENT A NEW API HERE
            .then(res => {
              if (res.data.data.content != '') {
                content.value = res.data.data.content
                editor.setValue(content.value)
              }
            })
          editor.setValue(content.value)
        }
      )
      editor.onDidChangeModelContent(e => {
        const value = editor.getValue() //使value和其值保持一致
        if (value !== data.value) {
          data.value = value
          ctx.emit("data", data.value)
          // this.props.getValue && this.props.getValue(value)
        }
      })
    })

    return {
      myArgs,
      data,
      container,
      content
    }
  },
}
 </script>

<style>
.container {
  width: 800px;
  height: 600px;
  text-align: left;
}
</style>
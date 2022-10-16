<template>
  <div class="box" ref="box">
    <div class="right">
      <el-form ref="ruleFormsss" :model="ruleForm" label-width="120px" class="demo-ruleForm">
        <MonacoEditor :myArgs="myArgs" @data="data" :rightWidth="rightWidth" />
        <el-form-item prop="input">
          <el-button type="primary" @click="submit" :loading="isLoading">Submit</el-button>
        </el-form-item>
        <!-- <el-form-item style="display: inline;"> -->
        <!-- </el-form-item> -->
      </el-form>
    </div>
  </div>
</template>


<script>
import { reactive, ref, unref } from '@vue/reactivity'
import axios from 'axios'
import MonacoEditor from '../components/MonacoEditor'
import { onMounted } from 'vue'
import { watchEffect } from "@vue/runtime-core"
export default {
  name: "Submit",
  components: {
    MonacoEditor
  },
  props: ['myArgs'],
  setup (props, context) {
    const ruleFormsss = ref(null)
    const ruleForm = reactive({
      input: ''
    })
    const myArgs = ref('')
    console.log(myArgs.value)
    const isLoading = ref(false)
    const result = ref('')
    let code = {}
    const { input } = ruleForm
    const submit = async () => {
      isLoading.value = true
      const form = unref(ruleFormsss)
      if (!form) return
      try {
        await form.validate()

        //console.log(code)
        //var param = {
        //  "code": code,
        //  "input": input
        //}
        //console.log(param)
        axios.post(
          "/question/compile", { "code": code }
        ).then(res => {
          console.log(res)
          alert(res.data.data)
          result.value = res.data.data
          isLoading.value = false
        })
      } catch (error) {
        console.log(error)
      }
    }
    const data = (c) => {
      code = c
      context.emit('code', code)
    }
    const rightWidth = ref('')
    const dragControllerDiv = () => {
      var resize = document.getElementsByClassName('resize')
      var left = document.getElementsByClassName('left')
      var right = document.getElementsByClassName('right')
      var box = document.getElementsByClassName('box')
      for (let i = 0; i < resize.length; i++) {
        // 鼠标按下事件
        resize[i].onmousedown = function (e) {
          //颜色改变提醒
          resize[i].style.background = '#818181'
          var startX = e.clientX
          resize[i].left = resize[i].offsetLeft
          // 鼠标拖动事件
          document.onmousemove = function (e) {
            var endX = e.clientX
            var moveLen = resize[i].left + (endX - startX) // （endx-startx）=移动的距离。resize[i].left+移动的距离=左边区域最后的宽度
            var maxT = box[i].clientWidth - resize[i].offsetWidth // 容器宽度 - 左边区域的宽度 = 右边区域的宽度

            if (moveLen < 300) moveLen = 300 // 左边区域的最小宽度为32px
            if (moveLen > maxT - 400) moveLen = maxT - 400 //右边区域最小宽度为150px

            resize[i].style.left = moveLen // 设置左侧区域的宽度

            for (let j = 0; j < left.length; j++) {
              left[j].style.width = moveLen + 'px'
              right[j].style.width = (box[i].clientWidth - moveLen - 10) + 'px'
              rightWidth.value = right[j].style.width
              // document.getElementById('editor').style.width = right[j].style.width
            }
          }
          // 鼠标松开事件
          document.onmouseup = function (evt) {
            //颜色恢复
            resize[i].style.background = '#d6d6d6'
            document.onmousemove = null
            document.onmouseup = null
            resize[i].releaseCapture && resize[i].releaseCapture() //当你不在需要继续获得鼠标消息就要应该调用ReleaseCapture()释放掉
          }
          resize[i].setCapture && resize[i].setCapture() //该函数在属于当前线程的指定窗口里设置鼠标捕获
          return false
        }
      }
    }
    watchEffect(() => {
      myArgs.value = props.myArgs
    })
    onMounted(() => {
      dragControllerDiv()
    })
    return {
      myArgs,
      ruleForm,
      submit,
      ruleFormsss,
      result,
      isLoading,
      data,
      rightWidth,
      dragControllerDiv
    }
  }
}
</script>

<style scoped>
.demo-ruleForm {
  /* margin: 0 auto;
  max-width: 700px; */
}
.header {
  width: 100%;
  height: 5%;
}
.page {
  position: absolute;
  top: 0;
  bottom: 0;
  left: 0;
  right: 0;
}
.box {
  width: 100%;
  height: 92%;
  margin: 0 0px;
  overflow: hidden;
  /* box-shadow: -1px 9px 10px 3px rgba(0, 0, 0, 0.11); */
  display: inline-block;
}
/*左侧div样式*/

/*拖拽区div样式*/
.resize {
  cursor: col-resize;
  float: left;
  position: relative;
  /* top: 45%; */
  background-color: #d6d6d6;
  /* border-radius: 5px; */
  /* margin-top: -10px; */
  width: 10px;
  height: 100%;
  background-size: cover;
  background-position: center;
  /*z-index: 99999;*/
  font-size: 32px;
  color: white;
}
/*拖拽区鼠标悬停样式*/
.resize:hover {
  color: #444444;
}
/*右侧div'样式*/
.right {
  float: left;
  width: 55%; /*右侧初始化宽度*/
  height: 100%;
  background: #fff;
  /* box-shadow: -1px 4px 5px 3px rgba(0, 0, 0, 0.11); */
}
.el-form-item__content {
  margin: 0 !important;
}
.footer {
  width: 100%;
  height: 2%;
}
</style>
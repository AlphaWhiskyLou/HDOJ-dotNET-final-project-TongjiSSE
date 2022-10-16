<template>
  <div>
    <div>
      连接：
      <input type="text" v-model="userId" placeholder="用户编号"/>
      <button type="button" @click="setup">连接</button>
      <button type="button" @click="addUser">加入用户</button>
    </div>
    <div>
      随机匹配：
      <button type="button" @click="matchUser">随机匹配</button>
    </div>
    <div>
      取消匹配：
      <button type="button" @click="cancelMatch">取消匹配</button>
    </div>
    <div>
      实时更新分数：
      <input type="text" placeholder="新分数" v-model="newScoreInput"/>
      <button type="button" @click="userInPlay">实时更新分数</button>
    </div>
    <div>
      游戏结束：
      <button type="button" @click="gameover">游戏结束</button>
    </div>


  </div>
</template>

<script>
export default {
  name: "MatchGame",
  data(){
    return {
        userId:0,
        chatMessage:{
          sender:"",
          type:"",
          data:"",
        },
        newScoreInput:0,
    }
  },
  methods:{
    setup() {
      this.ws = new WebSocket("ws://101.132.172.189:1998/game/match/"+this.userId);
      let thus =this
      this.ws.onopen = function (e) {
        console.log("websocket 已打开 userId: "+thus.userId);
      }
      this.ws.onmessage = function (msg) {
        console.log( thus.userId+"收到服务端消息: " + msg.data)
      }
      this.ws.onclose = function (e) {
        console.log("websocket 已关闭 userId: " + thus.userId)
      }
      this.ws.onerror = function (e) {
        console.log("websocket 发生了错误 userId : " + thus.userId," 错误原因为 "+e.reason)
      }
    },
    disconnect(){
      this.ws.close()
    },
    // 用户加入
    addUser(){
      console.log("this.userId==",this.userId)
      this.chatMessage.sender=this.userId;
      this.chatMessage.type="ADD_USER";
      console.log("用户: "+this.userId+" 开始加入....");
      this.ws.send(JSON.stringify(this.chatMessage))
    },
    //随机匹配
    matchUser(){
      console.log("this.userId==",this.userId)
      this.chatMessage.sender=this.userId;
      this.chatMessage.type="MATCH_USER";
      console.log("用户: "+this.userId+"开始匹配....");
      this.ws.send(JSON.stringify(this.chatMessage))
    },
    // 取消匹配
    cancelMatch(){
      console.log("this.userId==",this.userId)
      this.chatMessage.sender=this.userId
      this.chatMessage.type="CANCEL_MATCH"
      console.log("用户:"+this.userId+"取消匹配....")
      this.ws.send(JSON.stringify(this.chatMessage))
    },
    // 游戏中
    userInPlay(){
      console.log("this.userId==",this.userId)
      this.chatMessage.sender=this.userId;
      this.chatMessage.type="PLAY_GAME";
      this.chatMessage.data=this.newScoreInput
      console.log("用户："+this.userId+"更新分数为"+this.chatMessage.data);
      this.ws.send(JSON.stringify(this.chatMessage))
    },
    // 游戏结束
    gameover(){
      console.log("this.userId==",this.userId)
      this.chatMessage.sender=this.userId;
      this.chatMessage.type="GAME_OVER";
      console.log("用户:"+this.userId+"结束游戏")
      this.ws.send(JSON.stringify(this.chatMessage))
    }
  }
}
</script>

<style scoped>

</style>
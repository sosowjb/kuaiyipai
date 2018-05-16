//app.js
App({
  onLaunch: function () {
    // 登录
    // 获取用户信息
    wx.getSetting({
      success: res => {
          // 已经授权，可以直接调用 getUserInfo 获取头像昵称，不会弹框
          wx.getUserInfo({
            withCredentials:true,
            success: res => {
              // 可以将 res 发送给后台解码出 unionId
              this.globalData.userInfo = res.userInfo
              if (!wx.getStorageSync("accessToken"))
              {
                wx.login({
                  success: res => {
                  this.Logins(res.code,null)
                  }
                })
              }
              if (this.userInfoReadyCallback) {
                this.userInfoReadyCallback(res)
              }
            }
          })
        
      }
    })
  },
  Logins:function(code,callback){
    wx.request({
      url: this.globalData.apiLink + '/api/TokenAuth/Authenticate',
      header: { 'Abp.TenantId': '1', 'Content-Type': 'application/json' },
      method: 'POST',
      dataType: 'json',
      responseType: 'text',
      success: function (res) {
        console.log("用户ID"+res.data.result.userId)
        wx.setStorage({
          key: "accessToken",
          data: res.data.result.accessToken
        });
          wx.setStorage({
            key: "userId",
            data: res.data.result.userId
          });
          if(callback)
          {
            callback();
          }
      },
      fail: function (res) { },
      complete: function (res) { },
    })
  },
  globalData: {
    userInfo: null,
    apiLink: "http://localhost:5000",//api链接
    imageLink: "http://images.kypwp.com"//图片链接
  }
})
//app.js
App({
  onLaunch: function () {
    var that = this;
    if (wx.getStorageSync("accessToken")) {
      wx.request({
        url: that.globalData.apiLink + '/api/TokenAuth/ValidateToken',
        method: "POST",
        header: {
          "Abp.TenantId": "1",
          "Authorization": "Bearer " + wx.getStorageSync("accessToken"),
          "Content-Type": "application/json"
        },
        success: function (res) {
          console.log(res);
          if (res.data.success) {
            if (!res.data.result) {
              wx.clearStorage("accessToken");
            }
          }
        }
      })
    }
  },
  Logins: function (nickname, avatarlink,code,callback){
    console.log(nickname);
    var that=this;
    wx.request({
      url: this.globalData.apiLink + '/api/TokenAuth/Authenticate',
      header: { 'Abp.TenantId': '1', 'Content-Type': 'application/json' },
      method: 'POST',
      data: {
        nickName: nickname,
        avatarLink: avatarlink,
        code: code
        },
      dataType: 'json',
      responseType: 'text',
      success: function (res) {
        console.log(res);
        if(res.data.success)
        {
            wx.showToast({
              title: '登陆成功...',
              icon: 'loading',
              mask: true,
              duration: 500
            })
        that.globalData.userInfo = 
        { 
         "avatar": avatarlink,
         "nickname": nickname
        };
        wx.setStorage({
          key: "accessToken",
          data: res.data.result.accessToken
        });
          wx.setStorage({
            key: "userId",
            data: res.data.result.userId
          });
          console.log(callback());
          typeof callback == "function" && callback()
        }
      },
      fail: function (res) {
        console.log(res);
       },
      complete: function (res) { },
    })
  },
  showModal(msg) {
    wx.showModal({
      content: msg,
      showCancel: false,
    })
  },
  openLoading: function () {
    wx.showToast({
      title: '数据加载中',
      icon: 'loading',
      duration: 3000
    });
  },
  openToast: function () {
    wx.showToast({
      title: '已完成',
      icon: 'success',
      duration: 3000
    });
  },
  globalData: {
    userInfo: null,
    apiLink: "https://api.kypwp.com", //api链接
    imageLink: "https://api.kypwp.com/pic"//图片链接

    //apiLink: "http://localhost:5000", //api链接
    //imageLink: "http://images.kypwp.com/"//图片链接
  }
})
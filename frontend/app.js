//app.js
App({
  onLaunch: function () {
  },
  Logins: function (nickname, avatarlink,code,callback){
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
          typeof callback == "function" && callback()
        }
      },
      fail: function (res) { },
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
    apiLink: "http://localhost:5000",//api链接
    imageLink: "http://images.kypwp.com"//图片链接
  }
})
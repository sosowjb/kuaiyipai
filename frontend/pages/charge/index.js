const app = getApp()
Page({
  onLoad() {
  },
  onShow() {
  },
  charge:function(e){
    var money = e.detail.value.chargemoney;
    if(money <= 0){
      wx.showModal({
        title: '提示',
        content: '充值金额需大于0',
        showCancel: false
      })
      return
    }
    var that = this;
    wx.request({
      url: app.globalData.apiLink + '/api/services/app/Balance/Charge', 
      method: "POST",
      header: {
        "Authorization": wx.getStorageSync("accessToken"),
        "Content-Type": "application/json"
      },
      data: {
        amount: e.detail.value.chargemoney
      },
      success: function (res) {
        if (res.data.success) {
          wx.showModal({
            title: '提示',
            content: '充值成功！',
            showCancel: false
          })
          wx.navigateBack;
        }
      }
    })
  }
})
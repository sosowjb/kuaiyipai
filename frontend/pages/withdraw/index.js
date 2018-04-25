const app = getApp()

Page({
  data: {
    withdrawmax: 0,
    times: 0
  },
  onLoad() {

  },
  onShow() {
  },
  widthdraw: function (cb) {
    var money = e.detail.value.withdrawmoney;

    if (money <= 0) {
      wx.showModal({
        title: '提示',
        content: '体现金额需大于0',
        showCancel: false
      })
      return
    }
    var that = this;
    wx.request({
      url: app.globalData.apiLink + '/api/services/app/Balance/Withdraw',
      method: "POST",
      header: {
        "Authorization": wx.getStorageSync("accessToken"),
        "Content-Type": "application/json"
      },
      data: {
        amount: money
      },
      success: function (res) {
        if (res.data.success) {
          wx.showModal({
            title: '提示',
            content: '提现成功！',
            showCancel: false
          })
          wx.navigateBack;
        }
      }
    })
  }
})
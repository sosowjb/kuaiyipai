const app = getApp()

Page({
  data: {
    withdrawmax: 0,
    times: 0
  },
  onLoad() {

  },
  onShow() {
    this.getUserInfo();
    this.setData({
      version: app.globalData.version
    });
  },
  getUserInfo: function (cb) {
    var that = this
    wx.login({
      success: function () {
        wx.getUserInfo({
          success: function (res) {
            that.setData({
              userInfo: res.userInfo
            });
          }
        })
      }
    })
  }
})
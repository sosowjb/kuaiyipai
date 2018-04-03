const app = getApp()

Page({
  data: {
    balance: 0,
    freeze: 0,
    obligation:0,
    waitsend: 0,
    waitget: 0
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
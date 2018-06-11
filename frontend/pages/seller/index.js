const app = getApp()

Page({
  data: {
    balance: 0,
    freeze: 0,
    score: 0,
    score_sign_continuous: 0,
    waitPay: 0,
    waitSend: 0,
    waitRevice: 0
  },
  onLoad() {

  },
  onShow() {
    this.getUserInfo();
    this.getUserOrder();
    this.getUserAmount();
  },
  getUserInfo: function (cb) {
    var that = this
    wx.login({
      success: function (loginRes) {
        //console.info(loginRes);
        // 获取用户基础信息
        wx.getUserInfo({
          success: function (res) {
            that.setData({
              userInfo: res.userInfo
            });
            if (!wx.getStorageInfoSync("accessToken")) {
              app.Logins(loginRes.code);
            }
          }
        })
      }
    })
  },
  // 获取用户账户信息
  getUserAmount: function () {
    var that = this;
    wx.request({
      url: app.globalData.apiLink + '/api/services/app/Balance/GetMyBalance',
      method: "POST",
      header: {
        "Authorization": wx.getStorageSync("accessToken"),
        "Content-Type": "application/json"
      },
      success: function (res) {
        console.info(res);
        if (res.data.code == 0) {
          that.setData({
            balance: res.available
          });
        }
      }
    })
  },
  // 获取用户订单情况
  getUserOrder: function () {
    var that = this;
    wx.request({
      url: app.globalData.apiLink + '/api/services/app/Order/GetEachTypeOrderCount', method: "POST",
      header: {
        "Authorization": wx.getStorageSync("accessToken"),
        "Content-Type": "application/json"
      },
      data: {
      },
      success: function (res) {
        if (res.data.success) {
          that.setData({
            waitPay: res.data.result.waitPay,
            waitSend: res.data.result.waitSend,
            waitReceive: res.data.result.waitReceive
          });
        }
      }
    })
  }
})
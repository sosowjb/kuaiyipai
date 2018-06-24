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
    this.getUserAmount();
    this.getUserOrder();
  },
  // 获取用户账户信息
  getUserAmount: function () {
    var that = this;
    wx.request({
      url: app.globalData.apiLink + '/api/services/app/Balance/GetMyBalance',
      method: "get",
      header: {
        "Abp.TenantId": "1",
        "Authorization": "Bearer " + wx.getStorageSync("accessToken"),
        "Content-Type": "application/json"
      },
      data:{

      },
      success: function (res) {
        console.info(res);
        if (res.data.code == 0) {
          that.setData({
            balance: res.available,
            freeze: res.frozen
          });
        }
      }
    })
  },
  // 获取用户订单情况
  getUserOrder: function () {
    var that = this;
    wx.request({
      url: app.globalData.apiLink + '/api/services/app/Order/GetEachTypeOrderCount', 
      method: "GET",
      header: {
        "Abp.TenantId": "1",
        "Authorization": "Bearer " + wx.getStorageSync("accessToken"),
        "Content-Type": "application/json"
      },
      data: {
        userType: 0
      },
      success: function (res) {
        console.info(res);
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
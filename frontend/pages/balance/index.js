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
        if (res.data.result) {
          that.setData({
            balance: res.data.result.total - res.data.result.frozen,
            freeze: res.data.result.frozen
          });
        }
      }
    })
  }
})
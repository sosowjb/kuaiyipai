const app = getApp()
var utils = require("../../content/utils/util.js");
Page({
  onLoad() {
  },
  onShow() {
  },
  createNonceStr: function () {
    return Math.random().toString(36).substr(2, 15)
  },
  createTimeStamp: function () {
    return parseInt(new Date().getTime() / 1000) + ''
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
        "Abp.TenantId": "1",
        "Authorization": "Bearer " + wx.getStorageSync("accessToken"),
        "Content-Type": "application/json"
      },
      data: {
        amount: e.detail.value.chargemoney
      },
      success: function (res) {
        console.log(res);
      /* wx.requestPayment(
          {
           'timeStamp': this.createTimeStamp(),
            'nonceStr': this.createNonceStr(),
            'package': '',
            'signType': 'MD5',
            'paySign': '',
            'success': function (res) { },
            'fail': function (res) { },
            'complete': function (res) { }
          })*/

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
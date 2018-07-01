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
        
        if (res.data.success)
        {
            wx.requestPayment(
            {
            'timeStamp':res.data.result.timeStamp,
            'nonceStr': res.data.result.nonceStr,
            'package': "prepay_id=" + res.data.result.prepayId,
              'signType': 'MD5',
              'paySign': res.data.result.sign,
              'success': function (res) {
                if (res.errMsg =="requestPayment:ok")
                {
                  this.CompleteCharge();
                }
              },
              'fail': function (res) { 
                if (res.errMsg=="requestPayment:fail cancel")
                {
                wx.showModal({
                  title: '提示',
                  content: '取消支付！',
                  showCancel: false
                })
                }
                if (res.errMsg =="requestPayment:fail")
                {
                  wx.showModal({
                    title: '提示',
                    content: res.err_desc,
                    showCancel: false
                  })
                }
              },
              'complete': function (res) {
                console.log(res);
              }
            })
        }
       /* if (res.data.success) {
          wx.showModal({
            title: '提示',
            content: '充值成功！',
            showCancel: false
          })
          wx.navigateBack;
       }*/
      },
      'fail': function (res) {
        console.log("请求签名错误"+res);
      },
      'complete': function (res) {
        console.log(res);
      }
    })
  },
  CompleteCharge:function(){

  }
})
const app = getApp()

Page({
	data: {
    imageLink: app.globalData.imageLink,
    userInfo:null,
    balance:0,
    freeze:0,
    score:0,
    score_sign_continuous:0,
    waitPay: 0,
    waitSend: 0,
    waitRevice: 0
  },
	onLoad() {
   // console.log(app.globalData.userInfo);
	},	
  onShow() {
    this.getUserInfo();
    this.getUserOrder();
    this.getUserAmount();
  },	
  getUserInfo: function (cb) {
    var that = this;
    console.log(wx.getStorageSync("accessToken"));
    if (!wx.getStorageSync("accessToken")) {
      wx.showToast({
        title: '正在登陆...',
        icon: 'loading',
        mask: true,
        duration: 100000
      })
        wx.login({
        success: function (loginRes) {
          //console.log("accessToken:" + wx.getStorageSync("accessToken"));
          app.Logins(loginRes.code, null);
        }
        })
      }else
      {
      //console.log(userInfo);
      //通过token获取用户信息
      }
  },
  // 获取用户账户信息
  getUserAmount: function () {
    var that = this;
    wx.request({
      url: app.globalData.apiLink +'/api/services/app/Balance/GetMyBalance',
      method: "GET",
      header: {
        "Abp.TenantId": "1",
        "Authorization": "Bearer "+wx.getStorageSync("accessToken"),
        "Content-Type": "application/json"
      },
      success: function (res) {
        console.info(res);
        if (res.data.success) {
          that.setData({
            balance: res.available
          });
        }
        else
        {
          wx.login({
            success: function (loginRes) {
              //console.log("accessToken:" + wx.getStorageSync("accessToken"));
              app.Logins(loginRes.code, null);
            }
          })
        }
      }
    })
  },
  // 获取用户订单情况
  getUserOrder:function(){
    var that = this;
    wx.request({
      url: app.globalData.apiLink + '/api/services/app/Order/GetEachTypeOrderCount',                       method: "Get",
      header: {
        "Abp.TenantId": "1",
        "Authorization": "Bearer "+wx.getStorageSync("accessToken"),
        "Content-Type": "application/json"
      },
      data: {
      },
      success: function (res) {
        if (res.data.success) {
          that.setData({
            waitPay: res.data.result.waitPay,
            waitSend:res.data.result.waitSend,
            waitReceive:res.data.result.waitReceive
          });
        }
      }
    })
  }
})
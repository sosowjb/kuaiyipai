const app = getApp()

Page({
  data: {
    imageLink: app.globalData.imageLink,
    userInfo: null,
    balance: 0,
    freeze: 0,
    score: 0,
    score_sign_continuous: 0,
    waitPay: 0,
    waitSend: 0,
    waitRevice: 0
  },
  onLoad() {
    this.login();
  },
  onShow() {
    var that = this;
    that.getUserOrder();
    that.getUserAmount();
    if (wx.getStorageSync("accessToken")) {
      if (!app.globalData.userInfo.userInfo) {
        that.getUserInfo();
      }
      else {
        console.log(app.globalData.userInfo);
        that.setData({
          userInfo: app.globalData.userInfo
        });
        if (!app.globalData.userInfo.phone) {
          that.setData({
            Deliveryhidden: false
          });
        }
      }
    }
    else {
      that.login();
    }
  },
  getUserInfo: function () {
    var that = this;
    wx.request({
      url: app.globalData.apiLink + '/api/services/app/UserInfo/GetUserInfo',
      header: {
        "Abp.TenantId": "1",
        "Authorization": "Bearer " + wx.getStorageSync("accessToken"),
        "Content-Type": "application/json"
      },
      method: 'GET',
      success: function (res) {
        if (res.data.success) {
          console.log(res.data.result);
          that.setData({
            userInfo: {
              "avatar": res.data.result.avatarUrl,
              "nickname": res.data.result.nickName,
              "phone": res.data.result.phone,
              "id": res.data.result.id
            }
          });
          app.globalData.userInfo = that.data.userInfo;
          if (!res.data.result.phone) {
            that.setData({
              Deliveryhidden: false
            });
          }
        }
      },
      fail: function (res) { },
      complete: function (res) { },
    })
  },
  login: function () {
    var that = this;
    console.log(wx.getStorageSync("accessToken"));
    if (wx.getStorageSync("accessToken")) {
      wx.request({
        url: app.globalData.apiLink + '/api/TokenAuth/ValidateToken',
        method: "POST",
        header: {
          "Abp.TenantId": "1",
          "Authorization": "Bearer " + wx.getStorageSync("accessToken"),
          "Content-Type": "application/json"
        },
        success: function (res) {
          console.log(res);
          if (res.data.success) {
            if (!res.data.result) {
              wx.navigateTo({
                url: '/pages/login/index'　　// 页面 A
              })
            }
          }
        }
      })
    } else {
      wx.navigateTo({
        url: '/pages/login/index'
      })
    }
  },
  // 获取用户账户信息
  getUserAmount: function () {
    var that = this;
    wx.request({
      url: app.globalData.apiLink + '/api/services/app/Balance/GetMyBalance',
      method: "GET",
      header: {
        "Abp.TenantId": "1",
        "Authorization": "Bearer " + wx.getStorageSync("accessToken"),
        "Content-Type": "application/json"
      },
      success: function (res) {
        console.info(res);
        if (res.data.success == 0) {
          that.setData({
            balance: res.data.result.available
          });
        }
      }
    })
  },
  // 获取用户订单情况
  getUserOrder: function () {
    var that = this;
    wx.request({
      url: app.globalData.apiLink + '/api/services/app/Order/GetEachTypeOrderCountAsSeller', method: "Get",
      header: {
        "Abp.TenantId": "1",
        "Authorization": "Bearer " + wx.getStorageSync("accessToken"),
        "Content-Type": "application/json"
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
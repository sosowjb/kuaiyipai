const app = getApp()
const APP_ID = 'wxbf4f4e48ba291877';
const APP_SECRET = 'ff172c069aaccbe4fa1d5743c4e89eba';

Page({
	data: {
    balance:0,
    freeze:0,
    score:0,
    score_sign_continuous:0,
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
        success: function (res) {
          console.info(res);
          // 访问后台进行登陆操作

          // 获取用户基础信息
          wx.getUserInfo({
            success: function (res) {
              that.setData({
                userInfo: res.userInfo
              });
            }
          })
        }
      })
  },
  // 获取用户账户信息
  getUserAmount: function () {
    var that = this;
    wx.request({
      url: 'https://api.it120.cc/' + app.globalData.subDomain + '/user/amount',
      data: {
        token: app.globalData.token
      },
      success: function (res) {
        if (res.data.code == 0) {
          that.setData({
            balance: res.data.data.balance,
            freeze: res.data.data.freeze,
            score: res.data.data.score
          });
        }
      }
    })
  },
  // 获取用户订单情况
  getUserOrder:function(){
    var that = this;
    wx.request({
      url: 'http://localhost:22742/api/services/app/Order/GetEachTypeOrderCount',
      data: {
        token: app.globalData.token,
        userId:0,
        userType:0
      },
      success: function (res) {
        if (res.data.success) {
          that.setData({
            waitPay: res.data.result.waitPay,
            waitSend:res.data.result.waitSend,
            waitReceive:res.data.result.waitReceive
          });
          //console.info(count_data);
        }
      }
    })

  },
  relogin:function(){
    var that = this;
    app.globalData.token = null;
    app.login();
    wx.showModal({
      title: '提示',
      content: '重新登陆成功',
      showCancel:false,
      success: function (res) {
        if (res.confirm) {
          that.onShow();
        }
      }
    })
  }
})
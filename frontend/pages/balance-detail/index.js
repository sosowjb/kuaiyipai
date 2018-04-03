const app = getApp()

Page({
  data: {
    balance: 0,
    freeze: 0,
    obligation: 0,
    waitsend: 0,
    waitget: 0,
    detaillist:  [
        {
          "type": "取款",
          "comment": "还剩1.0元  2018-11-22",
          "status":0,
          "balance":10
        },
        {
          "type": "取款",
          "comment": "还剩1.0元  2018-11-21",
          "status": 1,
          "balance": 10
        },
        {
          "type": "取款",
          "comment": "还剩1.0元  2018-10-22",
          "status": 0,
          "balance": 10
        },
        {
          "type": "取款",
          "comment": "还剩1.0元  2018-09-22",
          "status": 1,
          "balance": 10
        },
      ],
    showView:false
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
  },
  choosetype:function(){
    var that = this;
    that.setData({
      showView: (!that.data.showView)
    });
  }
})
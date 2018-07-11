const app = getApp()

Page({
  data: {
    height:0,
    balance: 0,
    freeze: 0,
    obligation: 0,
    waitsend: 0,
    waitget: 0,
    haveData: false,
    currentPage:0,
    pageSize:5,
    detaillist:null,
    showView:false
  },
  onLoad() {
    // 计算页面高度，用以加载列表
    wx.getSystemInfo({
      success: (res) => {
        this.setData({
          height: res.windowHeight,
        })
      }
    })
    this.getMyBalanceRecords();
  },
  onShow() {
  
  },
  more: function () {
    var that = this;
    if (!that.data.haveData) {
      that.data.currentPage++;
    }
    this.getMyBalanceRecords();
  },
  getMyBalanceRecords:function(){
    var that=this;
    wx.request({
      url: app.globalData.apiLink +"/api/services/app/Balance/GetMyBalanceRecords?skipCount=" + (that.data.currentPage * that.data.pageSize) + "&maxResultCount=" + that.data.pageSize,
      method: "get",
      header: {
        "Abp.TenantId": "1",
        "Authorization": "Bearer " + wx.getStorageSync("accessToken"),
        "Content-Type": "application/json"
      },
      success: function (res) {
        console.info(res);
        if(res.data.success)
        {
        if (res.data.result.items.length == 0){
          that.setData({ haveData: true });
          return;
        }
        if (that.data.currentPage == 0){
          console.info(123);
          that.setData({
            detaillist: res.data.result.items
          });
        }else{
          var tempList = that.data.detaillist;
          for (var i in res.data.result.items) {
            tempList.push(res.data.result.items[i]);
          }
          console.info(456);
          that.setData({
            detaillist: tempList
          });
        }
      }
    }
    })
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
  },
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
//index.js
//获取应用实例
var app = getApp()
Page({
  data: {
    addressList:[]
  },
  addAddress : function () {
    wx.navigateTo({
      url:"/pages/address-add/index"
    })
  },  
  editAddress: function (e) {
    wx.navigateTo({
      url: "/pages/address-add/index?id=" + e.currentTarget.dataset.id
    })
  },
  deleteAddress: function (e) {
    var that = this;
    var id = e.currentTarget.dataset.id;
    wx.showModal({
      title: '提示',
      content: '确定要删除该收货地址吗？',
      success: function (res) {
        if (res.confirm) {
          wx.request({
            url: 'http://localhost:22742/api/services/app/Address/DeleteAddress',
            data: {
              token: app.globalData.token,
              id: id
            },
            success: (res) => {
              wx.navigateBack({})
            }
          })
        } else if (res.cancel) {
          console.log('用户点击取消')
        }
      }
    })
    console.info(e.currentTarget.dataset.id);
  },
  setDefault:function(e){
    //console.info(e.currentTarget.dataset.id);
    wx.request({
      url: 'http://localhost:22742/api/services/app/Address/SetDefault',
      data: {
        token: app.globalData.token,
        id: e.currentTarget.dataset.id
      },
      success: function (res) {
        if (res.data.success) {
          that.setData({
            addressList: res.data.result
          });
        }
      }
    })
  } ,
  onLoad: function () {
    console.log('onLoad')   
  },
  onShow : function () {
    this.getUserAddressList();
  },
  // 获取用户地址列表
  getUserAddressList:function(){
    var that = this;
    wx.request({
      url: 'http://localhost:22742/api/services/app/Address/GetAddress',
      data: {
        token: app.globalData.token,
        userId: 0,
        userType: 0
      },
      success: function (res) {
        if (res.data.success) {
          that.setData({
            addressList: res.data.result
          });
        }
      }
    })
  }
})

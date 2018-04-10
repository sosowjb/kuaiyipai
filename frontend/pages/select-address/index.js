//index.js
//获取应用实例
var app = getApp()
Page({
  data: {
    addressList:[{
      id:'1',
      linkMan:"yets",
      mobile:'123333',
      address:'123123123123213213',
      isdefault:true
    },
    {
      id: '2',
      linkMan: "eee",
      mobile: '123333',
      address: '123123123123213213',
      isdefault: false
    },
    {
      id: '3',
      linkMan: "rrrr",
      mobile: '123333',
      address: '123123123123213213',
      isdefault: false
    },
    {
      id: '4',
      linkMan: "www",
      mobile: '6666666',
      address: 'dfsdfsfsd',
      isdefault: false
    },
    {
      id: '5',
      linkMan: "ggg",
      mobile: '55555',
      address: 'sadadsadassf',
      isdefault: false
    }
    ]
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
            url: 'https://api.it120.cc/' + app.globalData.subDomain + '/user/shipping-address/delete',
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
    console.info(e.currentTarget.dataset.id);
  } ,
  onLoad: function () {
    console.log('onLoad')   
  },
  onShow : function () {
  }
})

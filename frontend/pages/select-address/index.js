//index.js
//获取应用实例
var commonCityData = require('../../content/utils/city.js')
var app = getApp()
Page({
  data: {
    addressList:[],
    selectid:'',
    isSeller:0
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
            url: app.globalData.apiLink + '/api/services/app/Address/DeleteAddress', 
            method: "POST",
            header: {
              "Abp.TenantId": "1",
              "Authorization": "Bearer " + wx.getStorageSync("accessToken"),
              "Content-Type": "application/json"
            },
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
   // console.info(e.currentTarget.dataset.id);
  },
  setDefault:function(e){
    var that=this;
    //console.info(e.currentTarget.dataset.id);
    wx.request({
      url: app.globalData.apiLink + '/api/services/app/Address/SetDefault', 
      method: "POST",
      header: {
        "Abp.TenantId": "1",
        "Authorization": "Bearer " + wx.getStorageSync("accessToken"),
        "Content-Type": "application/json"
      },
      data: {
        id: e.currentTarget.dataset.id
      },
      success: function (res) {
        console.log(res);
        if (res.data.success) {
          that.getUserAddressList();
        }
      }
    })
  } ,
  onLoad: function (e) {
     
    if (e && e!=null)
    {
      console.log(e);
       this.setData({
         selectid: e.id,
         isSeller: e.isSeller
       });
    }
  },
  onShow : function () {
    this.getUserAddressList();
  },
  // 获取用户地址列表
  getUserAddressList:function(){
    var that = this;
    wx.request({
      url: app.globalData.apiLink + '/api/services/app/Address/GetAddress', 
      method: "get",
      header: {
        "Abp.TenantId": "1",
        "Authorization": "Bearer " + wx.getStorageSync("accessToken"),
        "Content-Type": "application/json"
      },
      success: function (res) {
        if (res.data.success) {
          for (var i = 0; i < res.data.result.items.length;i++)
          {
            res.data.result.items[i]["pcd"] = that.getCityName(commonCityData.cityData,res.data.result.items[i].province, res.data.result.items[i].city, res.data.result.items[i].district);// that.getCityName();
            console.log(res.data.result.items[i]);
          }
          that.setData({
            addressList: res.data.result.items
          });
        }
      }
    })
  },
  getCityName: function (cityData,p,c,s){
    var dizhi = "";
    for (var i = 0; i < cityData.length; i++) {
      if (cityData[i].id == p) {
        dizhi += cityData[i].name;
      }
      for (var j = 0; j < cityData[i].cityList.length; j++) {
        if (cityData[i].cityList[j].id == c) {
          dizhi +='-'+cityData[i].cityList[j].name;
        }
        for (var k = 0; k < cityData[i].cityList[j].districtList.length; k++) {
          if (cityData[i].cityList[j].districtList[k].id == s) {
            dizhi += '-' +cityData[i].cityList[j].districtList[k].name;
          }
        }
      }
    }
    return dizhi;
  },
  selectAddress:function(e){
    var that=this;
    console.log(e.currentTarget.dataset.id);
    console.log(that.data.selectid);
    if (that.data.selectid)
    {
    wx.navigateTo({
      url: "/pages/orderdetail/index?addressid=" + e.currentTarget.dataset.id + "&id=" + that.data.selectid +"&isSeller="+that.data.isSeller
    })
    }
  }
})

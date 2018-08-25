// pages/orderdetail/index.js
var commonCityData = require('../../content/utils/city.js')
const app = getApp()
Page({

  /**
   * 页面的初始数据
   */
  data: {
    id:"",
    orderStatus:1,//订单状态1表示
    statusTime:"",//订单时间
    address:"",//地址
    addressid:"",
    goodsName:"",
    dealPrice:0,
    expressCostAmount:0,
    dealTime:"",
    sellerTel:"",//卖家手机号
    auctionNum:"",//拍卖编号
    goodsPriceNum: "",//货款交易号
    goodspic:'',
    consigneeName:"",
    consigneeTel:"",
    deliveryType:"",
    deliveryId:"",
    status:"",
    isSeller:0,
    Deliveryhidden:true
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    var that = this;
    var id = options.id;
    var isSeller = options.isSeller;
    var addressid = options.addressid;
    wx.showLoading();
    that.loadData(isSeller,id);
    if (addressid){
      that.getAddressById(addressid);
    }
  },
  loadData: function (isSeller,id)
  {
    var that = this;
    wx.request({
      url: app.globalData.apiLink + '/api/services/app/Order/GetOrder?orderId=' + id,
      method: "GET",
      header: {
        "Abp.TenantId": "1",
        "Authorization": "Bearer " + wx.getStorageSync("accessToken"),
        "Content-Type": "application/json"
      },
      success: function (res) {
        wx.hideLoading();
        if (res.data.success) {
          console.log(res);
          that.setData({
            id: id,
            isSeller: isSeller,
            orderStatus: res.data.result.orderStatus, // 1,//订单状态1表示
            statusTime: res.data.result.orderTime,//"2018-04-03 23:23",//订单时间
            deliveryId: res.data.result.deliveryId,
            goodsName: res.data.result.goodsName,
            dealPrice: res.data.result.price,
            expressCostAmount: 0,
            // dealTime: .OrderTime,
            sellerTel: res.data.result.sellerTel,//卖家手机号
            auctionNum: res.data.result.auctionNum,//拍卖编号
            //  goodsPriceNum:res.data.result.code,//货款交易号
            goodspic: res.data.result.goodsPicture
          });
          if (that.data.address == '') {
            that.getdefaultaddress();
          }
        } else {
          wx.showModal({
            title: '提示',
            content: '无法获取数据',
            showCancel: false
          })
        }
      }
    })

  },
getAddressById(addressid){
  var that=this;
  wx.request({
    url: app.globalData.apiLink + '/api/services/app/Address/GetAddress?Id=' + addressid,
    method: "GET",
    header: {
      "Abp.TenantId": "1",
      "Authorization": "Bearer " + wx.getStorageSync("accessToken"),
      "Content-Type": "application/json"
    },
    success: function (res) {
      console.log(res);
      if (res.data.success) {
  that.setData({
    addressid: addressid,
    consigneeName: res.data.result.items[0].receiver,
    consigneeTel: res.data.result.items[0].contactPhoneNumber,
    address: that.getCityName(commonCityData.cityData, res.data.result.items[0].province, res.data.result.items[0].city, res.data.result.items[0].district) + " " + res.data.result.items[0].street,//地址
  })
      }
  }})
},
  /**
   * 生命周期函数--监听页面初次渲染完成
   */
  onReady: function () {
  
  },

  /**
   * 生命周期函数--监听页面显示
   */
  onShow: function () {
  
  },

  /**
   * 生命周期函数--监听页面隐藏
   */
  onHide: function () {
  
  },

  /**
   * 生命周期函数--监听页面卸载
   */
  onUnload: function () {
  
  },

  /**
   * 页面相关事件处理函数--监听用户下拉动作
   */
  onPullDownRefresh: function () {
  
  },

  /**
   * 页面上拉触底事件的处理函数
   */
  onReachBottom: function () {
  
  },

  /**
   * 用户点击右上角分享
   */
  onShareAppMessage: function () {
  
  },
  call:function(e){
    if (e.currentTarget.dataset.text)
    {
    wx.makePhoneCall({
      phoneNumber: e.currentTarget.dataset.text //仅为示例，并非真实的电话号码
    })
    }
  },
  copyNum:function(e){
    wx.setClipboardData({
      data: e.currentTarget.dataset.text,
      success: function (res) {
        wx.getClipboardData({
          success: function (res) {
            console.log("拷贝成功！") // data
          }
        })
      }
    })
  },
  getCityName: function (cityData, p, c, s) {
    var dizhi = "";
    for (var i = 0; i < cityData.length; i++) {
      if (cityData[i].id == p) {
        dizhi += cityData[i].name;
      }
      for (var j = 0; j < cityData[i].cityList.length; j++) {
        if (cityData[i].cityList[j].id == c) {
          dizhi += '-' + cityData[i].cityList[j].name;
        }
        for (var k = 0; k < cityData[i].cityList[j].districtList.length; k++) {
          if (cityData[i].cityList[j].districtList[k].id == s) {
            dizhi += '-' + cityData[i].cityList[j].districtList[k].name;
          }
        }
      }
    }
    return dizhi;
  },
  showModel: function (e) {
    var orderId = e.currentTarget.dataset.id;
    console.log(orderId);
    this.setData({
      Deliveryhidden: false,
      orderid: orderId
    });
  },
  hiddenModel: function (e) {
    this.setData({
      Deliveryhidden: true
    });
  },
  showModal: function (msg) {
    wx.showModal({
      content: msg,
      showCancel: false,
    })
  },
  toReceived: function (e) {//确认收货
    var orderId = e.currentTarget.dataset.id;
    if (orderId == "") {
      that.showModal("订单号不能为空");
      return;
    }
    wx.showLoading();
    wx.request({
      url: app.globalData.apiLink + "/api/services/app/OrderOps/Receive",
      method: "POST",
      header: {
        'Abp.TenantId': '1',
        'Content-Type': 'application/json',
        'Authorization': "Bearer " + wx.getStorageSync("accessToken")
      },
      data: {
        "orderId": orderId
      },
      success: (res) => {
        wx.hideLoading();
        if (res.data.success) {
          that.showModal("收货成功！");
        }
      }
    })
  },
  toEvaluation: function (e) {//立即评价

  },
  toSend: function (e) {
    var that = this;
    if (e.target.dataset.deliverid == "") {
      that.showModal("快递单号不能为空");
      return;
    }
    if (that.data.orderId == "") {
      that.showModal("订单号不能为空");
      return;
    }
    if (that.data.address == '')
    {
      that.showModal("地址不能为空");
      return;
    }
    wx.showLoading();
    wx.request({
      url: app.globalData.apiLink + "/api/services/app/OrderOps/Send",
      method: "POST",
      header: {
        'Abp.TenantId': '1',
        'Content-Type': 'application/json',
        'Authorization': "Bearer " + wx.getStorageSync("accessToken")
      },
      data: {
        "orderId": that.data.orderid,
        "deliveryId": e.target.dataset.deliverid
      },
      success: (res) => {
        wx.hideLoading();
        if (res.data.success) {
          that.showModal("发货成功！");
          that.loadData(that.data.isSeller,that.data.orderid);
        }
      }
    })
  },
  GetDeliveryId: function (e) {
    console.log(e);
    var that = this;
    that.setData({
      DeliveryId: e.detail.value
    });
  },
  toPayTap: function (e) {
    console.log(e);
    var orderId = e.currentTarget.dataset.id;
    var money = e.currentTarget.dataset.money;
    var that = this;
    if (that.data.addressid == "") {
      that.showModal("地址不能为空");
      return;
    }
    if (orderId == "") {
      that.showModal("订单号不能为空");
      return;
    }
    if (money<=0) {
      that.showModal("金额不能小于0元");
      return;
    }

    wx.showModal({
      title: '支付确认',
      content: '将扣除您' + money + '元 您确认支付吗？',
      confirmText: "确认",
      cancelText: "取消",
      success: function (res) {
        console.log(res);
        if (res.confirm) {
          wx.request({
            url: app.globalData.apiLink + "/api/services/app/OrderOps/Pay",
            method: "POST",
            header: {
              'Abp.TenantId': '1',
              'Content-Type': 'application/json',
              'Authorization': "Bearer " + wx.getStorageSync("accessToken")
            },
            data: {
              "orderId": orderId,
              "addressId": that.data.addressid
            },
            success: (res) => {
            
              if (res.data.success) {
                //console.log(res);
                that.loadData(that.data.isSeller,orderId);
              }
              else
              {
                //console.log(res);
                that.showModal(res.data.error.message);
              }
            }
          })
        } else {
          console.log("用户已经取消操作");
        }
      }
    });
  },
  selectAddress:function(){
    if (this.data.orderStatus=="1")
    {
    wx.navigateTo({
      url: "/pages/select-address/index?id=" + this.data.id + "&isSeller=" + this.data.isSeller
    })
    }
  },
  getdefaultaddress:function(){
    var that=this;
    wx.request({
      url: app.globalData.apiLink + "/api/services/app/Address/GetDefaultAddress",
      method: "GET",
      header: {
        'Abp.TenantId': '1',
        'Content-Type': 'application/json',
        'Authorization': "Bearer " + wx.getStorageSync("accessToken")
      },
      success: (res) => {
        console.log(res);
        if (res.data.success) {
          that.setData({
            addressid: res.data.result.id,
            consigneeName: res.data.result.receiver,
            consigneeTel: res.data.result.contactPhoneNumber,
            address: that.getCityName(commonCityData.cityData, res.data.result.province, res.data.result.city, res.data.result.district) + " " + res.data.result.street,//地址
          })
        }
      }
    })
  }
})
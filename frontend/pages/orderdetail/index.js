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
    goodsName:"",
    dealPrice:"",
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
    console.log(isSeller);

    wx.showLoading();
     wx.request({
       url: app.globalData.apiLink + '/api/services/app/Order/GetOrder?orderId=' + id,
       method: "GET",
       header: {
         "Abp.TenantId": "1",
         "Authorization": "Bearer " + wx.getStorageSync("accessToken"),
         "Content-Type": "application/json"
       },
       success: function (res) {
         console.log(res);
         wx.hideLoading();
         if (res.data.success) {
           that.setData({
             id: id,
             isSeller: isSeller,
             orderStatus:res.data.result.orderStatus, // 1,//订单状态1表示
             statusTime:res.data.result.orderTime,//"2018-04-03 23:23",//订单时间
             address: that.getCityName(commonCityData.cityData, res.data.result.provinceId, res.data.result.cityId, res.data.result.districtId) + " " + res.data.result.street,//地址
             goodsName:res.data.result.goodsName,
             dealPrice:res.data.result.price,
             expressCostAmount:0,
            // dealTime: .OrderTime,
             sellerTel: res.data.result.sellerTel,//卖家手机号
             auctionNum:res.data.result.auctionNum,//拍卖编号
           //  goodsPriceNum:res.data.result.code,//货款交易号
             goodspic:res.data.result.goodsPicture,
             consigneeName:res.data.result.buyerName,
             consigneeTel:res.data.result.buyerTel,
             deliveryType:res.data.result.deliveryType,
             deliveryId: res.data.result.deliveryId
           });
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
    // console.log(e.target.dataset.deliverid);
    // console.log(that.data.orderid);
    if (e.target.dataset.deliverid == "") {
      that.showModal("快递单号不能为空");
      return;
    }
    if (that.data.orderId == "") {
      that.showModal("订单号不能为空");
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
              "orderId": orderId
            },
            success: (res) => {
              console.log(res);

            }
          })
        } else {
          console.log("用户已经取消操作");
        }
      }
    });
  }
  
})
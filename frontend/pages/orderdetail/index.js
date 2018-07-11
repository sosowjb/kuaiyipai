// pages/orderdetail/index.js
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
    deliveryType:""
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    var id = options.id;
    var that=this;
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
             orderStatus:res.data.result.orderStatus, // 1,//订单状态1表示
             statusTime:res.data.result.orderTime,//"2018-04-03 23:23",//订单时间
             address: res.data.result.address,//地址
             goodsName:res.data.result.goodsName,
             dealPrice:res.data.result.price,
            // dealTime: .OrderTime,
             sellerTel: res.data.result.sellerTel,//卖家手机号
            // auctionNum:res.data.result.code,//拍卖编号
           //  goodsPriceNum:res.data.result.code,//货款交易号
             goodspic:res.data.result.goodsPicture,
             consigneeName:res.data.result.buyerName,
             consigneeTel:res.data.result.buyerTel,
             deliveryType:res.data.result.deliveryType
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
  
})
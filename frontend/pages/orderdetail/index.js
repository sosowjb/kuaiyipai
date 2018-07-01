// pages/orderdetail/index.js
const app = getApp()
Page({

  /**
   * 页面的初始数据
   */
  data: {
    id:"",
    orderStatus:1,//订单状态1表示
    statusTime:"2018-04-03 23:23",//订单时间
    address:"北京市北京市昌平区，霍营华龙苑中里7-2-501",//地址
    goodsName:"昭和时代手表",
    dealPrice:"1000",
    dealTime:"2018-04-03 23:23",
    sellerTel:"18500189360",//卖家手机号
    auctionNum:"412990",//拍卖编号
    goodsPriceNum: "198999er33443434",//货款交易号
    goodspic: app.globalData.imageLink + '/goods/1.jpg',
    consigneeName:"王嘉宾",
    consigneeTel:"18500189361",
    deliveryType:"快递"
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
     var id='';
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
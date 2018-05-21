var wxpay = require('../../content/utils/pay.js')
var app = getApp()
Page({
  data:{
    statusType: ["待付款", "待发货", "待收货", "已完成"],
    currentType:0,
    tabClass: ["", "", "", "", ""],
    height:'',
    currentPage:0,
    pageSize:3,
    status:3
  },
  statusTap:function(e){
     var curType =  e.currentTarget.dataset.index;
     this.data.currentType = curType
     this.setData({
       currentType:curType
     });
     this.onLoad(null);
  },
  orderDetail : function (e) {
    var orderId = e.currentTarget.dataset.id;
    wx.navigateTo({
      url: "/pages/orderdetail/index?id=" + orderId
    })
  },
  toPayTap:function(e){
    var orderId = e.currentTarget.dataset.id;
    var money = e.currentTarget.dataset.money;
    wxpay.wxpay(app, money, orderId, "/pages/orderlist/index");
  },  
  onLoad: function (e){
    var that = this;
    var tabClass = that.data.tabClass;

    // 标记颜色
    if (e.status) {
      that.setData({
        status: e.status,
      });
    }
    // 计算页面高度，用以加载列表
    wx.getSystemInfo({
      success: (res) => {
        this.setData({
          height: res.windowHeight,          
        })
      }
    })

    for(var i  in tabClass){
      if (i == that.data.status){
        tabClass[i] = "red-dot";
      } else {
        tabClass[i] = "";
      }
    }
    that.setData({
      tabClass: tabClass,
    });
    this.getData();  
  },
  // 加载更多订单
  more: function () {
    var that = this;
    that.data.currentPage++;
    this.getData();
  },
  getData:function(){

    var that = this;
    // 获取订单列表
    wx.showLoading();
    var status = that.data.status;

    var url = app.globalData.apiLink + '/api/services/app/Order/GetCompletedOrders';
    // 待付款
    if (status == 0) {
      url = app.globalData.apiLink + '/api/services/app/Order/GetWaitingForPaymentOrdersAsBuyer';
    } else if (status == 1) { // 待发货
      url = app.globalData.apiLink + '/api/services/app/Order/GetWaitingForSendingOrdersAsSeller';
    } else if (status == 2) { // 待收货
      url = app.globalData.apiLink + '/api/services/app/Order/GetWaitingForReceivingOrdersAsBuyer';
    }
    wx.request({
      url: url,
      method: "POST",
      header: {
        "Authorization": wx.getStorageSync("accessToken"),
        "Content-Type": "application/json"
      },
      data: {
        skipCount: that.data.currentPage * that.data.pageSize,
        maxResultCount: that.data.pageSize
      },
      success: (res) => {
        wx.hideLoading();
        if (res.data.code == 0) {
          that.setData({
            orderList: res.data.data.items
          });
        } else {
          this.setData({
            orderList: null
          });
        }
      }
    }) 
  },
  onReady:function(){
    // 生命周期函数--监听页面初次渲染完成
 
  },
  onShow:function(){
    
  },
  onHide:function(){
    // 生命周期函数--监听页面隐藏
 
  },
  onUnload:function(){
    // 生命周期函数--监听页面卸载
 
  },
  onPullDownRefresh: function() {
    // 页面相关事件处理函数--监听用户下拉动作
   
  },
  onReachBottom: function() {
    // 页面上拉触底事件的处理函数
  
  }
})
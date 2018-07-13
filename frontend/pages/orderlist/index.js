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
    status:2,
    isSeller:0,
    Deliveryhidden:true,
    DeliveryId:"",
    orderid:"",
    orderList:[],
    haveData:false
  },
  statusTap:function(e){
    var that = this;
     var curType =  e.currentTarget.dataset.index;
     //console.log(curType);
     that.setData({
       currentType:curType,
       status: curType,
       currentPage: 0,
       orderList: [],
       haveData:false
     });
     this.getData();
  },
  orderDetail : function (e) {
    var orderId = e.currentTarget.dataset.id;
    var that=this;
    wx.navigateTo({
      url: "/pages/orderdetail/index?id=" + orderId + '&isSeller=' + that.data.isSeller
    })
  },
  toPayTap:function(e){
    console.log(e);
    var orderId = e.currentTarget.dataset.id;
    var money = e.currentTarget.dataset.money;
    wx.showModal({
      title: '支付确认',
      content: '将扣除您' + money+'元 您确认支付吗？',
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
  },
  showModel:function(e){
    var orderId = e.currentTarget.dataset.id;
    console.log(orderId);
    this.setData({
      Deliveryhidden:false,
      orderid: orderId
    });
  },
  hiddenModel: function (e) {
    this.setData({
      Deliveryhidden: true
    });
  },
  showModal: function(msg) {
    wx.showModal({
      content: msg,
      showCancel: false,
    })
  },
  toReceived:function(e){//确认收货
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
  toSend:function(e){
    var that = this;
   // console.log(e.target.dataset.deliverid);
   // console.log(that.data.orderid);
    if(e.target.dataset.deliverid=="")
    {
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
        if (res.data.success)
        {
          that.showModal("发货成功！");
        }
      }
    }) 
  },
  GetDeliveryId:function(e){
    console.log(e);
    var that = this;
    that.setData({
      DeliveryId: e.detail.value
    });
  },
  onLoad: function (e){
    console.log(e);
    
    var that = this;
    var tabClass = that.data.tabClass;

    // 标记颜色
    if (e.status) {
      that.setData({
        status:e.status,
        currentType:e.status,
        isSeller:e.isSeller
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
    if (!that.data.haveData) {
      that.data.currentPage++;
    }
    this.getData();
  },
  getData:function(){

    var that = this;
    // 获取订单列表
    wx.showLoading();
    //console.log(that.data);
    var status = that.data.status;
    var isSeller = that.data.isSeller;
    var url = app.globalData.apiLink + '/api/services/app/Order/GetCompletedOrders';
    var urltype =""
    if (isSeller==0){
      urltype ="AsBuyer"
    }
    else{
      urltype = "AsSeller"
    }
    // 待付款
    if (status == 0) {
      url = app.globalData.apiLink + '/api/services/app/Order/GetWaitingForPaymentOrders'+urltype;
    } else if (status == 1) { // 待发货
      url = app.globalData.apiLink + '/api/services/app/Order/GetWaitingForSendingOrders' + urltype;
    } else if (status == 2) { // 待收货
      url = app.globalData.apiLink + '/api/services/app/Order/GetWaitingForReceivingOrders' + urltype;
    }
    wx.request({
      url: url + "?skipCount=" + (that.data.currentPage * that.data.pageSize) + "&maxResultCount=" + that.data.pageSize,
      method: "get",
      header: {
       'Abp.TenantId': '1',
       'Content-Type': 'application/json',
       'Authorization': "Bearer " + wx.getStorageSync("accessToken")
      },
      success: (res) => {
        //console.log(res);
        wx.hideLoading();
        if (res.data.success) {

          if (res.data.result.items.length == 0){
            that.setData({ haveData: true });
            return;
          }

          if (that.data.currentPage == 0){
            that.setData({
              orderList: res.data.result.items
            });
          }else{
            var tempList = that.data.orderList;
            for (var i in res.data.result.items) {
              tempList.push(res.data.result.items[i]);
            }
            that.setData({
              orderList: tempList
            });
          }
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
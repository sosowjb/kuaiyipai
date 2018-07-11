var app = getApp()
Page({
  data:{
    statusType: ["草稿箱", "拍卖中", "已结束", "历史"],
    currentType:0,
    tabClass: ["", "", "", "", ""],
    height:'',
    currentPage:0,
    pageSize:3,
    status:0,
    DeliveryId:"",
    containerid:"",
    containerList:[],
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
       containerList: [],
       haveData:false
     });
     this.getData();
  },
  containerDetail : function (e) {
    var containerId = e.currentTarget.dataset.id;
    wx.navigateTo({
      url: "/pages/containerdetail/index?id=" + containerId
    })
  },
  onLoad: function (e){
    //console.log(e);
    
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

    /*
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
    */
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
    var url = app.globalData.apiLink + '/api/services/app/Item/GetMyDraftingItems';

    // 拍卖中
    if (status == 1) {
      url = app.globalData.apiLink + '/api/services/app/Item/GetMyAuctionItems';
    } else if (status == 2) { // 已结束
      url = app.globalData.apiLink + '/api/services/app/Item/GetMyCompletedItems';
    } else if (status == 3) { // 历史
      url = app.globalData.apiLink + '/api/services/app/Item/GetMyTerminatedItems';
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
              containerList: res.data.result.items
            });
          }else{
            var tempList = that.data.containerList;
            for (var i in res.data.result.items) {
              tempList.push(res.data.result.items[i]);
            }
            that.setData({
              containerList: tempList
            });
          }
        }
      }
    }) 
  },
  addToAuction: function (e) {//加入到货柜
    var that = this;
    var containerId = e.currentTarget.dataset.id;
    wx.showModal({
      title: '提示',
      content: '确定要将当前货柜加入拍卖吗？',
      success: function (sm) {
        if (sm.confirm) {
          wx.showLoading();
          wx.request({
            url: app.globalData.apiLink + '/api/services/app/Item/StartAuction',
            method: "get",
            header: {
              'Abp.TenantId': '1',
              'Content-Type': 'application/json',
              'Authorization': "Bearer " + wx.getStorageSync("accessToken")
            },
            data: {
              id: containerId,
            },
            success: (res) => {
              wx.hideLoading();
              that.getData();
            }
          }) 
        }
      }
    })
  },
  deleteContainer: function (e) {//移除到货柜
    var that = this;
    var containerId = e.currentTarget.dataset.id;
    wx.showModal({
      title: '提示',
      content: '确定移除当前货柜吗？',
      success: function (sm) {
        if (sm.confirm) {
          wx.showLoading();
          wx.request({
            url: app.globalData.apiLink + '/api/services/app/Item/DeleteItem',
            method: "get",
            header: {
              'Abp.TenantId': '1',
              'Content-Type': 'application/json',
              'Authorization': "Bearer " + wx.getStorageSync("accessToken")
            },
            data: {
              id: containerId,
            },
            success: (res) => {
              wx.hideLoading();
              that.getData();
            }
          }) 
        }
      }
    })
  },
  addgoods:function(){
    wx.navigateTo({
      url:"/pages/publish/index"
    });
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
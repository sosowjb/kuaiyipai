var sliderWidth = 96; // 需要设置slider的宽度，用于计算中间位置

Page({
  data: {
    tabs: ["草稿箱", "拍卖中", "已结束","历史"],
    activeIndex: 1,
    sliderOffset: 0,
    sliderLeft: 0
  },
  onLoad: function () {
    var that = this;
    wx.getSystemInfo({
      success: function (res) {
        that.setData({
          sliderLeft: (res.windowWidth / that.data.tabs.length - sliderWidth) / 2,
          sliderOffset: res.windowWidth / that.data.tabs.length * that.data.activeIndex
        });
      }
    });
  },
  tabClick: function (e) {
    this.setData({
      sliderOffset: e.currentTarget.offsetLeft,
      activeIndex: e.currentTarget.id
    });
  },
  getMyDraftingItems:function(e){
wx.request({
  url: '/api/services/app/Item/GetMyDraftingItems',
  data: '',
  header: {},
  method: 'GET',
  dataType: 'json',
  responseType: 'text',
  success: function(res) {},
  fail: function(res) {},
  complete: function(res) {},
})
  }, //草稿
  getMyAuctionItems:function (e){

  }, //正在拍卖
  getMyCompletedItems:function (e){

  }, //结束的
  getMyTerminatedItems:function (e){

  } //历史
});
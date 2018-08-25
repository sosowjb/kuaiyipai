// pages/login/index.js
const app = getApp()
Page({

  /**
   * 页面的初始数据
   */
  data: {
    canIUse: wx.canIUse('button.open-type.getUserInfo'),
    historyurl:"pages/home/home"
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    var that=this;
    var url = "/pages/my/index";
    if (options.url)
    {
      var url = options.url;
    }
    that.setData({
      historyurl: url
    });
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
  bindGetUserInfo: function (e) {
    var that=this;
    wx.getUserInfo({
      success: function (resuserInfo) {
        //console.log(resuserInfo.userInfo)
        wx.login({
          success: res => {
            //console.log(resuserInfo.userInfo.nickName);
          app.Logins(resuserInfo.userInfo.nickName, resuserInfo.userInfo.avatarUrl, res.code,that.localhost)
          }
        })
      }
    })
  },
  localhost:function(){
    var that=this;
    console.log(that.data.historyurl);
    wx.switchTab({
      url: that.data.historyurl
    })
  },
  backhome:function(){
    wx.switchTab({
      url: '/pages/home/home'
    })
  }
});
// pages/home/home.js
const app = getApp()
var util =  require("../../content/utils/util.js");
Page({

    /**
     * 页面的初始数据
     */

    data: {
        imageLink: app.globalData.imageLink,
        inputShowed: false,
        inputVal: "",
        banner:[
          { title: "", link: "", picurl:"/goods/swiper-1.jpg"},
          { title: "", link: "", picurl:"/goods/swiper-2.jpg"},
          { title: "", link: "", picurl: "/goods/swiper-3.jpg" }
          ]
    },
    showInput: function () {
        this.setData({
            inputShowed: true
        });
    },
    hideInput: function () {
        this.setData({
            inputVal: "",
            inputShowed: false
        });
    },
    clearInput: function () {
        this.setData({
            inputVal: ""
        });
    },
    inputTyping: function (e) {
        this.setData({
            inputVal: e.detail.value
        });
    },
    confirmSearch: function (e) {
        wx.navigateTo({
            url: '/pages/list/list'
        })
    },

    /**
     * 生命周期函数--监听页面加载
     */
    onLoad: function (options) {

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
    getBannerData:function(){
        wx.request({
          url: app.globalData.apiLink+'/api/services/app/Item/GetAuctionItems?SkipCount=1&MaxResultCount=1&Sorting=Id',
          header: { 'Abp.TenantId': '1', 'Content-Type': 'application/json' },
          method: 'GET',
          dataType: 'json',
          responseType: 'text',
          success: function(res) {
            console.log(res)
          },
          fail: function(res) {},
          complete: function(res) {},
        })
    }
})
// pages/classify/index.js
const app = getApp()
Page({

  /**
   * 页面的初始数据
   */

  data: {
    imageLink: app.globalData.imageLink,
    list: ["list0", "list1", "list2", "list3", "list4", "list5", "list6", "list7", "list8", "list9", "list10", "list11", "list12", "list13", "list14", "list15", "list16", "list17", "list18", "list19", "list20", "list21", "list22", "list23", "list24", "list25", "list26", "list27", "list28", "list29"],

    toView: 'eeede'
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
  jumpTo : function (e) {

    // 获取标签元素上自定义的 data-opt 属性的值

    let target = e.currentTarget.dataset.opt;

    this.setData({

      toView: target

    })

  },
})
// pages/classify/index.js
const app = getApp()
Page({

  /**
   * 页面的初始数据
   */

  data: {
    imageLink: app.globalData.imageLink,
    getPillars:[],
    toView: 'inToView1'
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
    var that = this;
     wx.request({
       url: app.globalData.apiLink + '/api/services/app/Pillar/GetPillars?SkipCount=0&MaxResultCount=100',
       header: { 'Abp.TenantId': '1', 'Content-Type': 'application/json' },
       method: 'get',
       dataType: 'json',
       responseType: 'text',
       success: function (res) {
        if(res.data.success)
        {
          that.getCategory(res.data.result.items);
        }
       }
     });
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
  selectmenu: function (e) {
    console.log(e)
    var _id = e.target.dataset.text;
    this.setData({
      toView: 'inToView' + _id
    })
    console.log(this.data.toView)
  },
  /**
   * 用户点击右上角分享
   */
  onShareAppMessage: function () {
  },
  getCategory:function(Pillars){
    var that=this;
    wx.request({
      url: app.globalData.apiLink + '/api/services/app/Category/GetCategories?SkipCount=0&MaxResultCount=100',
      header: { 'Abp.TenantId': '1', 'Content-Type': 'application/json' },
      method: 'get',
      dataType: 'json',
      responseType: 'text',
      success: function (res) {
        if (res.data.success) {
          var categories= res.data.result.items
          var totals = []
          for (var i = 0; i < Pillars.length; i++) {
            var str = { 'id': Pillars[i].id, 'code': Pillars[i].code, 'name': Pillars[i].name, 'categories': [] }
            for (var j = 0; j < categories.length; j++) {
              if (categories[j].pillarId == Pillars[i].id) {
                var categorie = {
                  'id': categories[j].id,
                  'code': categories[j].code,
                  'name': categories[j].name,
                  'pillars': categories[j].pillarId
                }
                str.categories.push(categorie)
              }
            }
            totals.push(str);
          }
          that.setData({
             getPillars: totals
          })
          //console.log(JSON.stringify(that.data.getPillars));
        }
      }
    })
  },
  search: function () {
    wx.navigateTo({
      url: '/pages/search/index',
    })
  },
  search2:function(e){
    wx.navigateTo({
      url: '/pages/search/index?CategoryId=' + e.currentTarget.dataset.text,
    })
  }
})
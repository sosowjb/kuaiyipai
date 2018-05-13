// pages/classify/index.js
const app = getApp()
Page({

  /**
   * 页面的初始数据
   */

  data: {
    imageLink: app.globalData.imageLink,
    getPillars:[],
    GetCategories:[],
    total:[]
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
          that.setData({
            getPillars: res.data.result.items
          })
          wx.request({
            url: app.globalData.apiLink + '/api/services/app/Category/GetCategories?SkipCount=0&MaxResultCount=100',
            header: { 'Abp.TenantId': '1', 'Content-Type': 'application/json' },
            method: 'get',
            dataType: 'json',
            responseType: 'text',
            success: function (res) {
              if (res.data.success) {
                that.setData({
                  GetCategories: res.data.result.items
                });
                var totals=[]
                for (var i = 0; i <= this.data.getPillars.length;i++)
                {
                  var str = { 'id': this.data.getPillars[i].id, 'code': this.data.getPillars[i].code, 'name': this.data.getPillars[i].name, categories:[]}
                  for (var j = 0; j <= this.data.GetCategories.length; j++) 
                  {
                    if (this.data.GetCategories[j].PillarId == this.data.getPillars[i].id)
                    {
                      str.categories.push = {
                        'id': this.data.GetCategories[j].Id,
                        'code':this.data.GetCategories[j].Code,
                        'name': this.data.GetCategories[j].Name,
                        'pillars': this.data.GetCategories[j].PillarId
                    }
                    }
                  }
                  totals.push(str);
            
                }
                console.log(totals);
              }
            }
          })
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
    console.log(e);
  },
  /**
   * 用户点击右上角分享
   */
  onShareAppMessage: function () {
  },
  getName:function(id){
    var menu_node1 = this.data.getPillars.filter(function (e) { return e.id == id; });
    return menu_node1;
  }
})
// pages/publish/index.js
const app = getApp();
var utils=require("../../content/utils/util.js");
Page({

  /**
   * 页面的初始数据
   */
  data: {
    date: (new Date().format("yyyy-MM-dd")),
    time: new Date().format("hh:mm"),
    pillarId: 0,
    categoryId: 0,
    img_arr: [],
    formdata: {description:""},
    pictureList:[],
    getPillars: [],//分类
    pillars:[],
    categories:[],
    pillarsIndex:0,
    categoriesIndex:0
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    var that = this;
    wx.request({
      url: app.globalData.apiLink + '/api/services/app/Pillar/GetPillars?SkipCount=0&MaxResultCount=100',
      header: { 'Abp.TenantId': '1', 'Content-Type': 'application/json' },
      method: 'get',
      dataType: 'json',
      responseType: 'text',
      success: function (res) {
        if (res.data.success) {
          that.getCategory(res.data.result.items);
        }
      }
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
  bindDateChange: function (e){
    this.setData({
      date: e.detail.value
    })
  },
  bindTimeChange: function (e) {
    this.setData({
      time: e.detail.value
    })
  },
   chooseImage: function (e) {
    var that = this;
    wx.chooseImage({
      count:9,  //最多可以选择的图片总数
      sizeType: ['compressed'], // 可以指定是原图还是压缩图，默认二者都有
      sourceType: ['album', 'camera'], // 可以指定来源是相册还是相机，默认二者都有
      success: function (res) {
        // 返回选定照片的本地文件路径列表，tempFilePath可以作为img标签的src属性显示图片
        that.setData({
          img_arr: res.tempFilePaths
        });
        var tempFilePaths = res.tempFilePaths;
        //启动上传等待中...
        wx.showToast({
          title: '正在上传...',
          icon: 'loading',
          mask: true,
          duration: 10000
        })
        var uploadImgCount = 0;
        var pL=[];
        for (var i = 0, h = tempFilePaths.length; i < h; i++) {
          wx.uploadFile({
            url: app.globalData.apiLink+'/api/services/app/Item/UploadPicture',
            filePath: tempFilePaths[i],
            name: 'uploadfile_ant',
            formData: {
              'imgIndex': i
            },
            header: { 'Abp.TenantId': '1', 'Content-Type': 'application/json', 'Authorization': "Bearer " + wx.getStorageSync("accessToken") },
            success: function (res) {
             
              var data = JSON.parse(res.data);
     
              if (data.success)
             {
               var isco=false;
               if (uploadImgCount==0)
               {
                 isco=true;
               }

               var pic={
                 isCover: isco,
                 id: data.result.id
               }
               pL.push(pic);
               that.setData({
                 pictureList: pL
               });
             }
             console.log(pL);
              uploadImgCount++;
              //如果是最后一张,则隐藏等待中
              if (uploadImgCount == tempFilePaths.length) {
                wx.hideToast();
              }
            },
            fail: function (res) {
              wx.hideToast();
              wx.showModal({
                title: '错误提示',
                content: '上传图片失败',
                showCancel: false,
                success: function (res) { }
              })
            }
          });
        }
      }
    });
  },
  previewImage: function (e) {
    wx.previewImage({
      current: e.currentTarget.id, // 当前显示图片的http链接
      urls: this.data.files // 需要预览的图片http链接列表
    })
  },
  formSubmit: function (e) {
  /*  console.log(this.data.pictureList);
   console.log(this.data.getPillars[this.data.pillarsIndex]);
   console.log(this.data.getPillars[this.data.pillarsIndex].categories[this.data.categoriesIndex]);*/
    wx.request({
      url: app.globalData.apiLink + '/api/services/app/Item/CreateItem',
      data: { 
        pillarId: this.data.getPillars[this.data.pillarsIndex].id,
        categoryId: this.data.getPillars[this.data.pillarsIndex].categories[this.data.categoriesIndex].id,
        startPrice: e.detail.value.startPrice,
        stepPrice: e.detail.value.stepPrice,
        startTime: (new Date()).format("yyyy-MM-dd hh:mm:ss"),
        title: e.detail.value.title,
        deadline: this.data.date + " " + this.data.time,
        description: this.data.formdata.description,
        pictureList: this.data.pictureList
       },
      header: { 'Abp.TenantId': '1', 'Content-Type': 'application/json', 'Authorization':"Bearer "+wx.getStorageSync("accessToken") },
      method: 'POST',
      dataType: 'json',
      responseType: 'text',
      success: function (res) {
      console.log(res);
      }
      });
  },
  textchange:function(e){
   var that=this;
   that.setData({
      "formdata.description": e.detail.value
    });
    
  },
  getCategory: function (Pillars) {
    var that = this;
    wx.request({
      url: app.globalData.apiLink + '/api/services/app/Category/GetCategories?SkipCount=0&MaxResultCount=100',
      header: { 'Abp.TenantId': '1', 'Content-Type': 'application/json' },
      method: 'get',
      dataType: 'json',
      responseType: 'text',
      success: function (res) {
        if (res.data.success) {
          var categories = res.data.result.items
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
          var PobjectArray = that.data.getPillars
          var CobjectArray = PobjectArray[that.data.pillarsIndex].categories
          var p = []
          var c=[]
          for (var i = 0; i < PobjectArray.length; i++) {
            p.push(PobjectArray[i].name)
          }
          for (var i = 0; i < CobjectArray.length; i++) {
            c.push(CobjectArray[i].name)
          }
          that.setData({ pillars: p, categories: c})
        }
      }
    })
  },
  bindPickerChangeP:function(e){
    this.setData({ pillarsIndex: e.detail.value, categoriesIndex: 0 })
      var CobjectArray = this.data.getPillars[this.data.pillarsIndex].categories
      var c = []
      for (var i = 0; i < CobjectArray.length; i++) {
        c.push(CobjectArray[i].name)
      }
      this.setData({ categories:c})  
  },
  bindPickerChangeC:function(e){
    this.setData({
      categoriesIndex: e.detail.value
    })  
  }
})
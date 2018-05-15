// pages/publish/index.js
var utils=require("../../content/utils/util.js");
Page({

  /**
   * 页面的初始数据
   */
  data: {
    date: (new Date().format("yyyy-MM-dd")),
    time: new Date().format("hh:mm"),
    files: [],
    formdata: {description:""}
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
      count: 3,  //最多可以选择的图片总数
      sizeType: ['compressed'], // 可以指定是原图还是压缩图，默认二者都有
      sourceType: ['album', 'camera'], // 可以指定来源是相册还是相机，默认二者都有
      success: function (res) {
        // 返回选定照片的本地文件路径列表，tempFilePath可以作为img标签的src属性显示图片
        var tempFilePaths = res.tempFilePaths;
        //启动上传等待中...
        wx.showToast({
          title: '正在上传...',
          icon: 'loading',
          mask: true,
          duration: 10000
        })
        var uploadImgCount = 0;
        for (var i = 0, h = tempFilePaths.length; i < h; i++) {
          wx.uploadFile({
            url: util.getClientSetting().domainName + '/home/uploadfilenew',
            filePath: tempFilePaths[i],
            name: 'uploadfile_ant',
            formData: {
              'imgIndex': i
            },
            header: {
              "Content-Type": "multipart/form-data"
            },
            success: function (res) {
              uploadImgCount++;
              var data = JSON.parse(res.data);
              //服务器返回格式: { "Catalog": "testFolder", "FileName": "1.jpg", "Url": "https://test.com/1.jpg" }
              var productInfo = that.data.productInfo;
              if (productInfo.bannerInfo == null) {
                productInfo.bannerInfo = [];
              }
              productInfo.bannerInfo.push({
                "catalog": data.Catalog,
                "fileName": data.FileName,
                "url": data.Url
              });
              that.setData({
                productInfo: productInfo
              });

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
   /* console.log(e.detail.value.title);
    console.log(e.detail.value.startPrice);
    console.log(e.detail.value.stepPrice);
    console.log(e.detail.value.priceLimit);
    console.log((new Date()).format("yyyy-MM-dd hh:mm:ss"));
    console.log(this.data.date + " " + this.data.time);
    console.log(this.data.formdata.description);*/
    wx.request({
      url: this.globalData.apiLink + '/api/TokenAuth/Authenticate',
      data: { 
        title: e.detail.value.title,
        startPrice: e.detail.value.startPrice,
        stepPrice: e.detail.value.stepPrice,
        priceLimit: e.detail.value.priceLimit,
        startTime: (new Date()).format("yyyy-MM-dd hh:mm:ss"),
        deadline: this.data.date + " " + this.data.time,
        description: this.data.formdata.description
       },
      header: { 'Abp.TenantId': '1', 'Content-Type': 'application/json', 'Authorization':"Bearer "+wx.getStorageSync("accessToken") },
      method: 'POST',
      dataType: 'json',
      responseType: 'text',
      success: function (res) {
      
      }
      });
  },
  textchange:function(e){
   var that=this;
   that.setData({
      "formdata.description": e.detail.value
    });
    
  }
})
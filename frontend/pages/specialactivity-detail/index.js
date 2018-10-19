// pages/specialactivity-detail/index.js
const app = getApp();
var utils = require("../../content/utils/util.js");
let col1H = 0;
let col2H = 0;
Page({

  /**
   * 页面的初始数据
   */
  data: {
    said:'',
    coverPic:'',
    shuoshuo:'',
     imageLink: app.globalData.imageLink,
    inputShowed: false,
    inputVal: "",
    imgWidth: 0,
    scrollH: 0,
    loadingCount: 0,
    images: [],
    col1: [],
    col2: [],
    currentpage: 1,
    pageSize: 5,
  },
  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    var that = this;
    var statu = 0;
   // console.log(options.id);
    console.log(options);
    that.setData({
      said: options.id     
    });

    wx.request({
      url: app.globalData.apiLink + '/api/services/app/SpecialActivity/GetSpecialActivity?id=' + options.id,
      header: { 'Abp.TenantId': '1', 'Content-Type': 'application/json' },
      method: 'GET',
      dataType: 'json',
      responseType: 'text',
      success: function (res) {
        console.log(res);
        if (res.data.success) {
          that.setData({
            shuoshuo: res.data.result.name,
            coverPic: options.converpic
          });
         
        }
      }
    })




    wx.getSystemInfo({
      success: (res) => {
        let ww = res.windowWidth;
        let wh = res.windowHeight;
        let imgWidth = ww * 0.48;
        let scrollH = wh;

        this.setData({
          scrollH: scrollH - 48,
          imgWidth: imgWidth
        });
        //加载首组图片
        this.loadImages();
      }
    })
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
      onImageLoad: function (e) {
    let imageId = e.currentTarget.id;
    let oImgW = e.detail.width;         //图片原始宽度
    let oImgH = e.detail.height;        //图片原始高度
    let imgWidth = this.data.imgWidth;  //图片设置的宽度
    let scale = imgWidth / oImgW;        //比例计算
    let imgHeight = oImgH * scale;      //自适应高度

    let images = this.data.images;
    let imageObj = null;

    for (let i = 0; i < images.length; i++) {
      let img = images[i];
      if (img.id === imageId) {
        imageObj = img;
        break;
      }
    }
    imageObj.height = imgHeight;
    let loadingCount = this.data.loadingCount - 1;
    let col1 = this.data.col1;
    let col2 = this.data.col2;
    //判断当前图片添加到左列还是右列
    if (col1H <= col2H) {
      col1H += imgHeight;
      col1.push(imageObj);
    } else {
      col2H += imgHeight;
      col2.push(imageObj);
    }

    let data = {
      loadingCount: loadingCount,
      col1: col1,
      col2: col2
    };
    console.log(data);
    //当前这组图片已加载完毕，则清空图片临时加载区域的内容
    if (!loadingCount) {
      data.images = [];
    }
    this.setData(data);
  },
  loadImages: function () {
    wx.showLoading({
      title: '加载中...',
    })
    var that = this;
    var skincount = (this.data.currentpage - 1) * this.data.pageSize
    var cpage = this.data.currentpage
    wx.request({
      url: app.globalData.apiLink + '/api/services/app/Item/GetAuctionItems?SkipCount=' + skincount + '&MaxResultCount=' + this.data.pageSize + '&Sorting=Id&SpecialActivityId=' + that.data.said,
      header: { 'Abp.TenantId': '1', 'Content-Type': 'application/json' },
      method: 'GET',
      dataType: 'json',
      responseType: 'text',
      success: function (res) {
        console.log(res);
        if (res.data.success) {
          if (res.data.result.items.length > 0) {
            let images = res.data.result.items;
            cpage = cpage + 1
            let baseId = "img-" + (+new Date());
            that.setData({
              loadingCount: images.length,
              images: images,
              currentpage: cpage
            });
          }
        }
      },
      fail: function (res) { },
      complete: function (res) {
        wx.hideLoading();
      },
    })
  },
  openDetail: function (e) {
    var current = e.currentTarget.dataset.text;
    wx.navigateTo({
      url: '/pages/goodsdetail/index?id=' + current,
    })
  }
})
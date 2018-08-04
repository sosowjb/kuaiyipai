// pages/publish/index.js
import WxValidate from '../../content/utils/wxValidate'
const app = getApp();
var utils = require("../../content/utils/util.js");
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
    categoriesIndex:0,
    id:''
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    var that = this;
    var id = options.id;
if(id)
{
that.setData({
  id:id
});
}
    wx.request({
      url: app.globalData.apiLink + '/api/services/app/Pillar/GetPillars?SkipCount=0&MaxResultCount=100',
      header: { 'Abp.TenantId': '1', 'Content-Type': 'application/json' },
      method: 'get',
      dataType: 'json',
      responseType: 'text',
      success: function (res) {
        if (res.data.success) {
          that.getCategory(res.data.result.items,id,that.getinitData);
          
        }
      }
    });

  },
  showModal(msg) {
    wx.showModal({
      content: msg,
      showCancel: false,
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
      count: 9 - that.data.pictureList.length,  //最多可以选择的图片总数
      sizeType: ['compressed'], // 可以指定是原图还是压缩图，默认二者都有
      sourceType: ['album', 'camera'], // 可以指定来源是相册还是相机，默认二者都有
      success: function (res) {
        // 返回选定照片的本地文件路径列表，tempFilePath可以作为img标签的src属性显示图片
        var tempFilePaths = res.tempFilePaths;
      /*  console.log(res.tempFilePaths);
        var imgArr = that.data.img_arr;
        for (var i = 0, h = tempFilePaths.length; i < h; i++) {
          imgArr.push(tempFilePaths[i]);
        }
        console.log(imgArr);
        that.setData({
          img_arr: imgArr
        });*/
       
        //启动上传等待中...
        wx.showToast({
          title: '正在上传...',
          icon: 'loading',
          mask: true,
          duration: 10000
        })
        var uploadImgCount =0;
        if (that.data.pictureList.length)
        {
          uploadImgCount = that.data.pictureList.length;
        }
        var pL=that.data.pictureList;
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
              console.log(res);
              var data = JSON.parse(res.data);
              if (data.success)
             {
               var isco=false;
                console.log(uploadImgCount);
               if (uploadImgCount==0)
               {
                 isco=true;
               }
               var pic={
                 isCover: isco,
                 id: data.result.id,
                 url: data.result.url
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
      urls: this.data.img_arr // 需要预览的图片http链接列表
    })
  },
  formSubmit: function (e) {
  /*  console.log(this.data.pictureList);
   console.log(this.data.getPillars[this.data.pillarsIndex]);
   console.log(this.data.getPillars[this.data.pillarsIndex].categories[this.data.categoriesIndex]);*/
   var that=this;
    if (!e.detail.value.title)
    {
      this.showModal("请输入标题");
      return false
    }
    if (!e.detail.value.startPrice) {
      this.showModal("请输入起拍价");
      return false
    }
    if (isNaN(e.detail.value.startPrice)){
      this.showModal("起拍价必须是数字");
      return false
    }
    if (!e.detail.value.stepPrice) {
      this.showModal("请输入加价幅度");
      return false
    }
    if (isNaN(e.detail.value.stepPrice)) {
      this.showModal("加价幅度必须是数字");
      return false
    }
    var nowdate = new Date();
    var d1 = new Date((this.data.date + " " + this.data.time).replace(/\-/g, "\/"));
    if (nowdate >= d1) {
      this.showModal("结束日期不能等于小于当前日期");
      return false
    }
    if (!e.detail.value.description) {
      this.showModal("请输入描述");
      return false
    }
    if (this.data.pictureList.length<=0) {
      this.showModal("请至少上传一张图片");
      return false
    }

    wx.showLoading();
    var url = app.globalData.apiLink + '/api/services/app/Item/CreateItem'
    console.log(this.data.pictureList);
    if(that.data.id)
    {
      url = app.globalData.apiLink + '/api/services/app/Item/EditItem'
    }
  
    wx.request({
      url: url,
      data: { 
        pillarId: this.data.getPillars[this.data.pillarsIndex].id,
        categoryId: this.data.getPillars[this.data.pillarsIndex].categories[this.data.categoriesIndex].id,
        startPrice: e.detail.value.startPrice,
        stepPrice: e.detail.value.stepPrice,
        startTime: (new Date()).format("yyyy-MM-dd hh:mm:ss"),
        title: e.detail.value.title,
        deadline: this.data.date + " " + this.data.time,
        description: this.data.formdata.description,
        pictureList: this.data.pictureList,
        id:that.data.id
       },
      header: { 'Abp.TenantId': '1', 'Content-Type': 'application/json', 'Authorization':"Bearer "+wx.getStorageSync("accessToken") },
      method: 'POST',
      dataType: 'json',
      responseType: 'text',
      success: function (res) {
       // console.log(res);
        wx.hideLoading();
        wx.navigateTo({
          url: "/pages/container/index",
        })
      }
      });
  },
  textchange:function(e){
   var that=this;
   that.setData({
      "formdata.description": e.detail.value
    });
  },
  getCategory: function (Pillars,id,callback) {
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

          callback(id);
        }
      }
    })
  },
  getPillarsAndCategoriesIndex:function(id,cid)
  {
    var that=this;
    var Pillars = that.data.getPillars;
    console.log(Pillars); 
    var c = [];
    for (var i = 0; i < Pillars.length; i++) {
      if (Pillars[i].id == id)
      {
        that.setData({
          pillarsIndex: i
        });
        for (var j = 0; j < Pillars[i].categories.length; j++) {
          if (Pillars[i].categories[j].id==cid)
        {
          that.setData({
            categoriesIndex:j
          });
        }     
          c.push(Pillars[i].categories[j].name) 
      }
        that.setData({
          categories: c
        });
      }
    }
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
  },
  regValidator: function (source, regFormat) {
    var result = source.match(regFormat);
    if (result == null) return false;
    else return true;
  },
  getinitData:function(id){
    var that = this;
    if (id) {
      wx.request({
        url: app.globalData.apiLink + '/api/services/app/Item/GetItem?id=' + id,
        header: { 'Abp.TenantId': '1', 'Content-Type': 'application/json' },
        method: 'get',
        dataType: 'json',
        responseType: 'text',
        success: function (res) {
          if (res.data.success) {
            console.log(res);
            var dates = new Date(res.data.result.deadline);
           that.getPillarsAndCategoriesIndex(res.data.result.pillarId, res.data.result.categoryId)

            that.setData({
              'formdata.title': res.data.result.title,
              'formdata.startPrice': res.data.result.startPrice,
              'formdata.stepPrice': res.data.result.stepPrice,
              date: that.datetimefmt(dates, 'yyyy-MM-dd'),
              time: that.datetimefmt(dates, 'hh:mm'),
              'formdata.description': res.data.result.description
            });
          }
        }
      });
      wx.request({
        url: app.globalData.apiLink + '/api/services/app/Item/GetItemPictures?id=' + id,
        header: { 'Abp.TenantId': '1', 'Content-Type': 'application/json' },
        method: 'GET',
        dataType: 'json',
        responseType: 'text',
        success: function (res) {
          if (res.data.success) {
            that.setData({
              pictureList: res.data.result.items
            });
          }
        }
      });

    }
  },
  deleteimage:function(e){
    var that = this;
    console.log(that.data.pictureList);
    console.log(e.currentTarget.dataset.id);
   
    var piclist = that.data.pictureList;
    for (var i = 0; i < piclist.length; i++) {
      if (piclist[i].id == e.currentTarget.dataset.id)
      {
       // delete piclist[i];
        piclist.splice(i,1);
      }
    }
    that.setData({
      pictureList: piclist
    });
  },
 datetimefmt:function(time,fmt) { //author: meizz
    var o = {
      "M+": time.getMonth() + 1, //月份
      "d+": time.getDate(), //日
      "h+": time.getHours(), //小时
      "m+": time.getMinutes(), //分
      "s+": time.getSeconds(), //秒
      "q+": Math.floor((time.getMonth() + 3) / 3), //季度
      "S": time.getMilliseconds() //毫秒
    };
    if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (time.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
      if (new RegExp("(" + k + ")").test(fmt))
        fmt = fmt.replace(RegExp.$1, (RegExp.$1.length === 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
  }
})
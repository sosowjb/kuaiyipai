// pages/details/index.js
const app=getApp();
var utils = require("../../content/utils/util.js");
Page({
  /**
   * 页面的初始数据
   */
  data: {
    canIUse: wx.canIUse('button.open-type.getUserInfo'),
    imageLink: app.globalData.imageLink,
    djs:{//倒计时
      t: "",
      day: 0,
      h: 0,
      m: 0,
      s: 0,
      clock:""
    },
     fixednum:{
      is_open: 0,//是否打开窗口
      price: ""//出价
     },
    userInfo: {},
    vendor: { 
      id:123,//id
      nickname: "",//名称
      hpic: ""//头像
     // credit:4.8,//信用值
     // increase:true,//信用值是否增加
     // amountInAll:4000,
     // isAuthentication: true,//是否认证
     // isFollow:false,//是否关注
      },
    goodsInfo:{},
    goodsInfoPic:[],
    auctionInfo: []//拍卖情况
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    var that=this;
    var statu = 0;
    wx.request({
      url: app.globalData.apiLink + '/api/services/app/Item/GetItemPictures?id=' + options.id,
      header: { 'Abp.TenantId': '1', 'Content-Type': 'application/json' },
      method: 'GET',
      dataType: 'json',
      responseType: 'text',
      success: function (res) {
        if (res.data.success)
        {
          that.setData({
            goodsInfoPic:res.data.result.items
          });
        }
      }
    });
    

    wx.request({
      url: app.globalData.apiLink + '/api/services/app/Item/GetItem?id=' + options.id,
      header: { 'Abp.TenantId': '1', 'Content-Type': 'application/json' },
      method: 'GET',
      dataType: 'json',
      responseType: 'text',
      success: function (res) {
        if (res.data.success) {
          console.log(res.data.result);
          if (res.data.result.status == 'Auctioning') {
            statu = 1;
          }
          that.setData({
            goodsInfo: {
              goodsId: res.data.result.id,
             // rPrice: 78168,//参考价
              bPrice: res.data.result.startPrice,//起价
              addPrice: res.data.result.stepPrice,//加价幅度
              //pPrice: 100,//保证金
              desc: res.data.result.description,//描述
              status: statu,//拍卖状态【1，正在拍卖，0是还未开始,2结束】
              endTime: new Date(res.data.result.deadline).format("yyyy-MM-dd hh:mm"),
              goodsPic: [{ pic: "", smallpic: "" }],
              avator: res.data.result.avator,
              nikename: res.data.result.nikeName
            }
          });
          countdown(that);
        }
      }
    });
    that.GetBiddings(options.id);
  },
  GetBiddings:function(id){
    var that=this;
  wx.request({
    url: app.globalData.apiLink + '/api/services/app/Bidding/GetBiddings?ItemId=' + id + '&SkipCount=0&MaxResultCount=5&Sorting=Price',
    header: { 'Abp.TenantId': '1', 'Content-Type': 'application/json' },
    method: 'GET',
    dataType: 'json',
    responseType: 'text',
    success: function (res) {
      if (res.data.success) {
        console.log(res.data.result.items);
        that.setData({
          auctionInfo: res.data.result.items
        });
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
    clearTimeout(this.data.djs.t);
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
  getImageArry:function(){
    var arry=[];
    console.log(this.data.goodsInfoPic);
    for (var i = 0, len = this.data.goodsInfoPic.length; i < len;i++) {
      
      arry.push(this.data.goodsInfoPic[i].url);
   }
    return arry;
  },
  previewImage: function (e) {
    var current = e.currentTarget.dataset.text;
    console.log(current);
    var arry = this.getImageArry();
    wx.previewImage({
      current: current, // 当前显示图片的http链接  
      urls: arry// 需要预览的图片http链接列表  
    })
  },
  bid:function(){//出价
  var that=this;
    var suprice=0;
    if (that.data.auctionInfo.length>0)
   {
      suprice = that.data.auctionInfo[0].price;
   }
   console.log(suprice)
   that.setData({
      "fixednum.is_open": 1,
      "fixednum.price": parseFloat(suprice) + parseFloat(this.data.goodsInfo.addPrice)
   });
  },
  //出价手机键盘控件方法
closefixednum: function () {//关闭弹窗
    this.setData({
      "fixednum.is_open": 0
    });
  },
  clearPrice: function () {//清除加价
    this.setData({
      "fixednum.price": ""
    });
  },
  inputNum: function (e) {//输入数字
    console.log(this.data.fixednum.price);
    var priceTest = this.data.fixednum.price + e.currentTarget.dataset.text;
    this.setData({
      "fixednum.price": priceTest
    });
  },
  backspace: function () {//退格
    var priceTest = this.data.fixednum.price.toString().replace(/.$/, '');
    this.setData({
      "fixednum.price": priceTest
    });
  },
  submitPrice: function () {
    //提交加个，追标加价
    //验证价格是否合法
    var price = 0;
    if (this.data.auctionInfo.length > 0) {
      price = this.data.auctionInfo[0].price;
    }
   var nowPrice= this.data.fixednum.price
   //console.log("拍卖价格不能低于或等于当前价格1" + nowPrice);
   //console.log("拍卖价格不能低于或等于当前价格2" + price);
   if (nowPrice <= price)//如果小于或者等于当前最高价格，是不可以的
   {
   //console.log("拍卖价格不能低于或等于当前价格");
     show("拍卖价格不能低于或等于当前价格");
   }
   else if (price>0 && nowPrice%price!=0)//如果小于或者等于当前最高价格，是不可以的
   {
        //console.log("拍卖价格不能低于或等于当前价格");
        show("加价幅度错误");
   }
   else if (price <= 0 && nowPrice % this.data.goodsInfo.addPrice != 0)//如果小于或者等于当前最高价格，是不可以的
   {
     //console.log("拍卖价格不能低于或等于当前价格");
     show("加价幅度错误");
   }
   else
   {
    if(wx.getStorageSync("accessToken")) {
     this.submitdata();
    }
    else
    {
      wx.login({
        success: res => {
          app.Logins(res.code, this.submitdata)
        }
      })
    }
   }
  },
  submitdata:function(){
    if (wx.getStorageSync("accessToken"))
    {
    wx.request({
      url: app.globalData.apiLink + '/api/services/app/Bidding/Bid',
      data: { itemId: this.data.goodsInfo.goodsId, price: this.data.fixednum.price },
      header: { 'Abp.TenantId': '1', 'Content-Type': 'application/json', 'Authorization':"Bearer "+ wx.getStorageSync("accessToken") },
      method: 'POST',
      dataType: 'json',
      responseType: 'text',
      success: function (res) {
        if (res.data.success)
        {
          //提交获取成功
          show("恭喜您，竞拍成功");
          closefixednum();
        }
        else
        {
          show(res.data.error.message);
        }
      }
    })
    }
    else
    {
      wx.navigateTo({
        url: '/pages/login/index'
      })
    }
  },
  bindGetUserInfo: function (e) {
    console.log(e.detail.userInfo)
  }
})
function countdown(that) {
  var EndTime = that.data.goodsInfo.endTime || [];
  EndTime = new Date(EndTime);
  var NowTime = new Date().getTime();
  var total_micro_second = EndTime - NowTime || [];
  // 渲染倒计时时钟
  var second = Math.floor(total_micro_second / 1000);
  if(second<=0)
  {
    if (that.data.goodsInfo.status==1)//说明后台状态还未结束，需要修改状态
    {

    }

    that.setData({
    "djs.clock":"已经结束"
    });
    clearTimeout(that.data.djs.t);
  }
  else
  {
  that.setData({
    "djs.day": Math.floor(second / 3600 / 24),
    "djs.h": Math.floor(second / 3600 % 24),
    "djs.m": Math.floor(second / 60 % 60),
    "djs.s": Math.floor(second % 60)
  });
  that.setData({
    "djs.t":setTimeout(function () {
      total_micro_second -= 1000;
      countdown(that);
    }, 1000)
  });
  }
}

// 时间格式化输出，如11:03 25:19 每1s都会调用一次
function dateformat(that,micro_second) {
  // 总秒数

}
function show(content)
{
  wx.showModal({
    content: content,
    showCancel: false,
    success: function (res) {
      if (res.confirm) {
       
      }
    }
  });
}
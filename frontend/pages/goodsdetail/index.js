// pages/details/index.js
const app=getApp();
var utils = require("../../content/utils/util.js");
Page({
  /**
   * 页面的初始数据
   */

  data: {
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
      nickname: "天河珠宝",//名称
      hpic: app.globalData.imageLink+"/avatar/96.jpg",//头像
     // credit:4.8,//信用值
     // increase:true,//信用值是否增加
     // amountInAll:4000,
     // isAuthentication: true,//是否认证
     // isFollow:false,//是否关注
      },
    goodsInfo:{},
    auctionInfo:[//拍卖情况
      {
        id:"1",
        avatar: app.globalData.imageLink + "/avatar/96.jpg",
         nickname:"xiaoPeng",
         price:100,
         isBid:true,//是否中标
         bidTime:"2018/04/01 15:44"//中标时间
      },
      {
        id: "2",
        avatar: app.globalData.imageLink + "/avatar/96.jpg",
        nickname: "xiaoMing",
        price: 100,
        isBid: false,//是否中标
        bidTime: "2018/04/05 15:44"//投标时间
      }
      ]
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    var that=this;
    var statu = 0;
    wx.request({
      url: app.globalData.apiLink + '/api/services/app/Item/GetItem?id=' + options.id,
      header: { 'Abp.TenantId': '1', 'Content-Type': 'application/json' },
      method: 'GET',
      dataType: 'json',
      responseType: 'text',
      success: function (res) {
        console.log(new Date(res.data.result.deadline).format("yyyy-MM-dd hh:mm"));
        if (res.data.result.status =='Auctioning'){
          statu =1
        }
       
        that.setData({
          goodsInfo:{
            goodsId: res.data.result.id,
            rPrice: 78168,//参考价
            bPrice: res.data.result.startPrice,//起价
            addPrice: res.data.result.stepPrice,//加价幅度
            pPrice: 100,//保证金
            desc: res.data.result.description,//描述
            status: statu,//拍卖状态【1，正在拍卖，0是还未开始,2结束】
            endTime: new Date(res.data.result.deadline).format("yyyy-MM-dd hh:mm")
          }
        });
        countdown(that);
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
    for (var i = 0,len = this.data.goodsInfo.goodsPic.length; i < len;i++) {
      arry.push(this.data.goodsInfo.goodsPic[i].pic);
   }
    return arry;
  },
  previewImage: function (e) {
    var current = e.currentTarget.dataset.text;
    var arry = this.getImageArry();
    wx.previewImage({
      current: current, // 当前显示图片的http链接  
      urls: arry// 需要预览的图片http链接列表  
    })
  },
  bid:function(){//出价
  this.setData({
      "fixednum.is_open": 1
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
    var priceTest = this.data.fixednum.price.replace(/.$/, '');
    this.setData({
      "fixednum.price": priceTest
    });
  },
  submitPrice: function () {
    //提交加个，追标加价
  }
})
function countdown(that) {
  var EndTime = that.data.goodsInfo.endTime || [];
  EndTime = new Date(EndTime);
  var NowTime = new Date().getTime();
  var total_micro_second = EndTime - NowTime || [];
  // 渲染倒计时时钟
  var second = Math.floor(total_micro_second / 1000);
  console.log(second);
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
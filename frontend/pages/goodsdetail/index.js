// pages/details/index.js
const app=getApp()
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
    goodsInfo:{
      goodsId:"",
      rPrice: 78168,//参考价
      bPrice:0,//起价
      addPrice:100,//加价幅度
      pPrice:100,//保证金
      desc:"【帝舵\t间金原镶时刻钻\t男表】实体店现货\t九九新（二手）Tudor/帝舵\t九九新（二手）骏珏系列\t18K黄金/精钢\t自动机械\t男表\n品牌:Tudor/帝舵\n型号：56003\n编号：J835881\n附件：无附件（裸表）\n直径：39mm\n腕周：180mm\n表盘颜色:金色-时刻原镶钻\n表带:18K黄金/精钢\n表壳:18K黄金/精钢\n功能：日期显示\t星期显示\n公价：34600\n\n注：如果您对商品满意，好评截图返现，非常感谢您对我们店的支持！为了安全起见，部分贵重商品一律采用顺丰到付，您本人收货确认无误后，直接联系我们，运费返现+红包！\n\n本店所有商品，均为实体店现货，请您放心购买，为了回馈新老客户对我们的支持，即日起，出售的所有二手手表，在无损坏的情况下，均支持三月内8.5折，一年内7.5折回收服务！\n\n南京礼尚往来奢侈品1999年创办至今！回收销售世界名表、钻石、名包等一线奢侈品，只销售专柜正品，假一罚十，所有商品均为店内现货。全新表均享受全球联保服务。（99新为二手）\n\n大家请放心购买！里上往来飞格拉姆和南京礼尚往来\t保证金已缴纳23.8888万元\t。\n\n地址：南京市新街口繁华商圈淮海路2号（中央商场正对面）\t礼尚往来奢侈品鉴定评估中心",//描述
      goodsPic:[
        {
          smallpic: app.globalData.imageLink +"/goods/1.jpg",//缩略图链接
          pic: app.globalData.imageLink +"/goods/1.jpg"//原图链接
        },
        {
          smallpic: app.globalData.imageLink +"/goods/2.jpg",//缩略图链接
          pic: app.globalData.imageLink +"/goods/2.jpg"//原图链接
        }
        ,
        {
          smallpic: app.globalData.imageLink +"/goods/3.jpg",//缩略图链接
          pic: app.globalData.imageLink +"/goods/3.jpg"//原图链接
        }
        ,
        {
          smallpic: app.globalData.imageLink +"/goods/4.jpg",//缩略图链接
          pic: app.globalData.imageLink +"/goods/4.jpg"//原图链接
        }
        ,
        {
          smallpic: app.globalData.imageLink +"/goods/5.jpg",//缩略图链接
          pic: app.globalData.imageLink +"/goods/5.jpg"//原图链接
        }
        ,
        {
          smallpic: app.globalData.imageLink +"/goods/6.jpg",//缩略图链接
          pic: app.globalData.imageLink +"/goods/6.jpg"//原图链接
        }
        ,
        {
          smallpic: app.globalData.imageLink +"/goods/7.jpg",//缩略图链接
          pic: app.globalData.imageLink +"/goods/7.jpg"//原图链接
        }
        ,
        {
          smallpic: app.globalData.imageLink +"/goods/8.jpg",//缩略图链接
          pic: app.globalData.imageLink +"/goods/8.jpg"//原图链接
        },
        {
          smallpic: app.globalData.imageLink +"/goods/9.jpg",//缩略图链接
          pic: app.globalData.imageLink +"/goods/9.jpg"//原图链接
        }
      ],
      status:1,//拍卖状态【1，正在拍卖，0是还未开始,2结束】
      endTime:'2018-4-01 15:30'
    },

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
    
    countdown(this);
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
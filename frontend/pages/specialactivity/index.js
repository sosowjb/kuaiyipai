const app = getApp();
var page = 1;
var page_size = 5;
var sort = "last";
var is_easy = 0;
var lange_id = 0;
var pos_id = 0;
var unlearn = 0;
// 请求数据
var loadMore = function (that) {
  var skincount = (page - 1) * page_size;
  var cpage = page;
  that.setData({
    hidden: false
  });
  wx.request({
    url: app.globalData.apiLink + "/api/services/app/SpecialActivity/GetSpecialActivities?SkipCount=" + skincount +"&MaxResultCount="+cpage+"",
    success: function (res) {
      console.log(res.data.result.items);
      var list = that.data.list;
      for (var i = 0; i < res.data.result.items.length; i++) {
       list.push(res.data.result.items[i]);
      }
      that.setData({
        list: list
      });
      page++;
      that.setData({
        hidden: true
      });
    }
  });
}
Page({
  data: {
    hidden: true,
    list: [],
    scrollTop: 0,
    scrollHeight: 0
  },
  onLoad: function () {
    //   这里要注意，微信的scroll-view必须要设置高度才能监听滚动事件，所以，需要在页面的onLoad事件中给scroll-view的高度赋值
    var that = this;
    wx.getSystemInfo({
      success: function (res) {
        that.setData({
          scrollHeight: res.windowHeight
        });
      }
    });
    loadMore(that);
  },
  //页面滑动到底部
  bindDownLoad: function () {
    var that = this;
    loadMore(that);
    console.log("lower");
  },
  scroll: function (event) {
    //该方法绑定了页面滚动时的事件，我这里记录了当前的position.y的值,为了请求数据之后把页面定位到这里来。
    this.setData({
      scrollTop: event.detail.scrollTop
    });
  },
  topLoad: function (event) {
    //   该方法绑定了页面滑动到顶部的事件，然后做上拉刷新
    page = 0;
    this.setData({
      list: [],
      scrollTop: 0
    });
    loadMore(this);
    console.log("lower");
  },
  openDetail: function (e) {
    var that=this;
    var current = e.currentTarget.dataset.text;
    var coverpic = e.currentTarget.dataset.conver;
    wx.navigateTo({
      url: '/pages/specialactivity-detail/index?id=' + current + '&converpic=' + coverpic
    })
  }
})
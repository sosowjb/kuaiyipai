//index.js
var app = getApp()
Page({
  data: {
    nameModal:true,
    telModal:true,
  },
  onShow() {
    this.getUserInfo();
  },
  getUserInfo: function (cb) {
    var that = this
    wx.login({
      success: function () {
        wx.getUserInfo({
          success: function (res) {
            that.setData({
              userInfo: res.userInfo
            });
          }
        })
      }
    })
  },
  changePic: function () {
    let _this = this;
    wx.showActionSheet({
      itemList: ['从相册中选择', '拍照'],
      itemColor: "#f7982a",
      success: function (res) {
        if (!res.cancel) {
          if (res.tapIndex == 0) {
            _this.chooseWxImage('album')
          } else if (res.tapIndex == 1) {
            _this.chooseWxImage('camera')
          }
        }
      }
    })

  },
  chooseWxImage: function (type) {
    let _this = this;
    wx.chooseImage({
      sizeType: ['original', 'compressed'],
      sourceType: [type],
      success: function (res) {
        console.log(res);
        _this.setData({
          logo: res.tempFilePaths[0],
        })
      }
    })
  },
  nikNameModal:function(){
    this.setData({
      nameModal: !this.data.nameModal
    })
  },
  //取消按钮  
  nikCancel: function () {
    this.setData({
      nameModal: true
    });
  },
  //确认  
  nikConfirm: function () {
    this.setData({
      nameModal: true
    })
  },
  telephoneModal: function () {
    this.setData({
      telModal: !this.data.telModal
    })
  },
  //取消按钮  
  telCancel: function () {
    this.setData({
      telModal: true
    });
  },
  //确认  
  telConfirm: function () {
    this.setData({
      telModal: true
    })
  }  

})

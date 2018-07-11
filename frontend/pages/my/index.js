const app = getApp()

Page({
	data: {
    imageLink: app.globalData.imageLink,
    userInfo:null,
    balance:0,
    freeze:0,
    score:0,
    score_sign_continuous:0,
    waitPay: 0,
    waitSend: 0,
    waitRevice: 0,
    Deliveryhidden:true,
    acceptVal:100,
    identifyCode_btn: true,
    button_reqIdentifyCode: '获取验证码',
    identifyCode:''
  },
	onLoad() {
    this.login();
    this.setData({
      step: 1
    })
	},	
  onShow() {
    var that=this;
   
    if (wx.getStorageSync("accessToken"))
    {
      that.getUserInfo();
      that.getUserOrder();
      that.getUserAmount();
      /*else
      {
        that.setData({
          userInfo: app.globalData.userInfo
        });
        if (!app.globalData.userInfo.phone)
        {
          that.getUserInfo();
        }
      }*/
    }
    else
    {
      that.login();
    }
  },
  getUserInfo:function(){
   var that=this;
      wx.request({
        url: app.globalData.apiLink + '/api/services/app/UserInfo/GetUserInfo',
        header: {
          "Abp.TenantId": "1",
          "Authorization": "Bearer " + wx.getStorageSync("accessToken"),
          "Content-Type": "application/json"
        },
        method: 'GET',
        success: function(res) {
          if (res.data.success) {
            console.log(res.data.result);
            that.setData({
              userInfo: {
                "avatar": res.data.result.avatarUrl,
                "nickname": res.data.result.nickName,
                "phone": res.data.result.phone,
                "id": res.data.result.id
              }
            });
            app.globalData.userInfo=that.data.userInfo;
            if (!res.data.result.phone)
            {
              that.setData({
                Deliveryhidden: false
              });
            }
          }
        },
        fail: function(res) {},
        complete: function(res) {},
      })
  },
  login: function () {
    var that = this;
    if (wx.getStorageSync("accessToken")) {
           wx.request({
             url: app.globalData.apiLink + '/api/TokenAuth/ValidateToken',
             method: "POST",
             header: {
               "Abp.TenantId": "1",
               "Authorization": "Bearer " + wx.getStorageSync("accessToken"),
               "Content-Type": "application/json"
             },
             success: function (res) {
               console.log(res);
               if (res.data.success) {
                 if(!res.data.result)
                 {
                   wx.navigateTo({
                     url: '/pages/login/index'　　// 页面 A
                   })
                 }
               }
             }
           })
      }else
      {
        wx.navigateTo({
        url: '/pages/login/index'
      })
      }
  },
  // 获取用户账户信息
  getUserAmount: function () {
    var that = this;
    wx.request({
      url: app.globalData.apiLink +'/api/services/app/Balance/GetMyBalance',
      method: "GET",
      header: {
        "Abp.TenantId": "1",
        "Authorization": "Bearer "+wx.getStorageSync("accessToken"),
        "Content-Type": "application/json"
      },
      success: function (res) {
       console.log(res);
        if (res.data.success) {
          that.setData({
            balance: res.data.result.available
          });
        }
        else
        {
          wx.login({
            success: function (loginRes) {
              //console.log("accessToken:" + wx.getStorageSync("accessToken"));
             app.Logins(loginRes.code, null);
            }
          })
        }
      }
    })
  },
  // 获取用户订单情况
  getUserOrder:function(){
    var that = this;
    wx.request({
      url: app.globalData.apiLink + '/api/services/app/Order/GetEachTypeOrderCountAsBuyer',
      method: "Get",
      header: {
        "Abp.TenantId": "1",
        "Authorization": "Bearer "+wx.getStorageSync("accessToken"),
        "Content-Type": "application/json"
      },
      data: {
      },
      success: function (res) {
        if (res.data.success) {
          that.setData({
            waitPay: res.data.result.waitPay,
            waitSend:res.data.result.waitSend,
            waitReceive:res.data.result.waitReceive
          });
        }
      }
    })
  },
  input_acceptVal: function (e) {
    var acceptVal = e.detail.value;
    if (acceptVal!=null) {
      this.setData({
        acceptVal: acceptVal,
        identifyCode_btn: false
      })
    } else {
      this.setData({
        identifyCode_btn: true
      })
    }
  },
  input_identifyCode: function (e) {
    var identifyCode = e.detail.value;
    if (identifyCode != null) {
      this.setData({
        identifyCode: identifyCode
      })
    }
  },
  // 获取验证码按钮动作
  reqIdentifyCode: function (e) {
    var acceptVal = this.data.acceptVal;
    var that = this;
    var countdown = 60;
    var mobile = /^(13[0-9]|15[0-9]|17[0-9]|18[0-9]|19[0-9])\d{8}$/;
    // 校验手机或邮箱
    if (!mobile.test(acceptVal)) {
      app.showModal("请填写正确的手机号码或邮箱");
    } else {
      this.setData({
        button_reqIdentifyCode: '发送中'
      });
      // 获取验证码
      wx.request({
        url: this.globalData.apiLink + '/api/services/app/UserPhone/RequestCaptcha',
        header: {
          'Abp.TenantId': '1',
          'Content-Type': 'application/json',
          'Authorization': "Bearer " + wx.getStorageSync("accessToken")
        },
        method: 'POST',
        data: {
          accept: this.data.acceptVal
        },
        success: function (res) {
          console.log(res)
          // 结果不为空
          if (res!=null) {
            app.showModal(res.data.msg);
            if (res.data.code == 100 && countdown > 0) {
              interval = setInterval(function () {
                that.setData({
                  button_reqIdentifyCode: '重新获取(' + countdown + 's)'
                });
                countdown--;

                if (countdown <= 0) {
                  countdown = -1
                  that.setData({
                    button_reqIdentifyCode: '获取验证码'
                  });
                  clearInterval(interval)
                }
              }, 1000)
            } else {
              that.setData({
                button_reqIdentifyCode: '获取验证码'
              });
            }
          }
        },
        fail: function () {
          app.showModal("请求失败");
        }
      })
    }
  },
  bindPhone:function(e){
    var acceptVal = this.data.acceptVal;
    var identifyCode = this.data.identifyCode;
    var that = this;
    var mobile = /^(13[0-9]|15[0-9]|17[0-9]|18[0-9]|19[0-9])\d{8}$/;
    // 校验手机或邮箱
    if (!mobile.test(acceptVal)) {
      app.showModal("请填写正确的手机号码或邮箱");
      return;
    }
    if (identifyCode == null || identifyCode =="") {
      app.showModal("请填验证码");
      return;
    }
    wx.request({
      url: app.globalData.apiLink + "/api/services/app/UserPhone/BindPhone",
      method: "POST",
      header: {
        'Abp.TenantId': '1',
        'Content-Type': 'application/json',
        'Authorization': "Bearer " + wx.getStorageSync("accessToken")
      },
     data:{
       "phone": acceptVal,
       "captcha": identifyCode
      },
      success: (res) => {
        console.log(res);
       that.setData({
         Deliveryhidden:false
       }); 
      }
    }) 
  },
  hiddenModel:function(e){
    this.setData({
      Deliveryhidden: true
    }); 
  }
})
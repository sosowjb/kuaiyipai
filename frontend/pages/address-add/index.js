var commonCityData = require('../../content/utils/city.js')
//获取应用实例
var app = getApp()
Page({
  data: {
    provinces:[],
    citys:[],
    districts:[],
    selProvince:'请选择',
    selCity:'请选择',
    selDistrict:'请选择',
    selProvinceIndex:0,
    selCityIndex:0,
    selDistrictIndex:0,
    receiver:"",
    contactPhoneNumber:"",
    street:""
  },
  bindCancel:function () {
    wx.navigateBack({})
  },
  bindSave: function(e) {
    var that = this;
    var linkMan = e.detail.value.linkMan;
    var address = e.detail.value.address;
    var mobile = e.detail.value.mobile;
    var code = e.detail.value.code;
    if (linkMan == ""){
      wx.showModal({
        title: '提示',
        content: '请填写联系人姓名',
        showCancel:false
      })
      return
    }
    if (mobile == ""){
      wx.showModal({
        title: '提示',
        content: '请填写手机号码',
        showCancel:false
      })
      return
    }
    if (this.data.selProvince == "请选择"){
      wx.showModal({
        title: '提示',
        content: '请选择地区',
        showCancel:false
      })
      return
    }
    if (this.data.selCity == "请选择"){
      wx.showModal({
        title: '提示',
        content: '请选择地区',
        showCancel:false
      })
      return
    }
    var cityId = commonCityData.cityData[this.data.selProvinceIndex].cityList[this.data.selCityIndex].id;
    var districtId;
    if (this.data.selDistrict == "请选择" || !this.data.selDistrict){
      districtId = '';
    } else {
      districtId = commonCityData.cityData[this.data.selProvinceIndex].cityList[this.data.selCityIndex].districtList[this.data.selDistrictIndex].id;
    }
    if (address == ""){
      wx.showModal({
        title: '提示',
        content: '请填写详细地址',
        showCancel:false
      })
      return
    }
   /* console.log(that.data.id);
    console.log(commonCityData.cityData[this.data.selProvinceIndex].id);
    console.log(cityId);
    console.log(districtId);
    console.log(linkMan);
    console.log(address);
    console.log(mobile);*/
    var method='POST';
    var url = app.globalData.apiLink + "/api/services/app/Address/CreateAddress";
    if (that.data.id) {
      method='PUT'
      url = app.globalData.apiLink + "/api/services/app/Address/UpdateAddress";
    } 
    wx.request({
      url: url,
      method: method,
      header: {
        "Abp.TenantId": "1",
        "Authorization": "Bearer " + wx.getStorageSync("accessToken"),
        "Content-Type": "application/json"
      },
      data: {
        id: that.data.id,
        provinceId: commonCityData.cityData[this.data.selProvinceIndex].id,
        cityId: cityId,
        districtId: districtId == '' ? 0 : districtId,
        receiver:linkMan,
        street:address,
        contactPhoneNumber:mobile
      },
      success: function(res) {
        console.log(res);
        // 跳转到结算页面
        wx.navigateBack({})
      }
    })
  },
  initCityData:function(level, obj){
    if(level == 1){
      var pinkArray = [];
      for(var i = 0;i<commonCityData.cityData.length;i++){
        pinkArray.push(commonCityData.cityData[i].name);
      }
      this.setData({
        provinces:pinkArray
      });
    } else if (level == 2){
      var pinkArray = [];
      var dataArray = obj.cityList
      for(var i = 0;i<dataArray.length;i++){
        pinkArray.push(dataArray[i].name);
      }
      this.setData({
        citys:pinkArray
      });
    } else if (level == 3){
      var pinkArray = [];
      var dataArray = obj.districtList
      for(var i = 0;i<dataArray.length;i++){
        pinkArray.push(dataArray[i].name);
      }
      this.setData({
        districts:pinkArray
      });
    }
    
  },
  bindPickerProvinceChange:function(event){
  console.log(event);
    var selIterm = commonCityData.cityData[event.detail.value];
    console.log(selIterm);
    this.setData({
      selProvince:selIterm.name,
      selProvinceIndex:event.detail.value,
      citys: selIterm[event.detail.value].cityList,
      selCity:'请选择',
      selCityIndex:0,
      districts: selIterm[event.detail.value].cityList[0].districtList,
      selDistrict:'请选择',
      selDistrictIndex: 0
    })
    this.initCityData(2, selIterm)
  },
  bindPickerCityChange:function (event) {
    console.log(event.detail.value);
    var selIterm = commonCityData.cityData[this.data.selProvinceIndex].cityList[event.detail.value];
    this.setData({
      selCity:selIterm.name,
      selCityIndex:event.detail.value,

      districts: selIterm.districtList,
      selDistrict: '请选择',
      selDistrictIndex: 0
    })
    this.initCityData(3, selIterm)
  },
  bindPickerChange:function (event) {
    var selIterm = commonCityData.cityData[this.data.selProvinceIndex].cityList[this.data.selCityIndex].districtList[event.detail.value];
    if (selIterm && selIterm.name && event.detail.value) {
      this.setData({
        selDistrict: selIterm.name,
        selDistrictIndex: event.detail.value
      })
    }
  },
  onLoad: function (e) {
    var that = this;
   // this.initCityData(1)
    var id = e.id;
    if (id) {
      // 初始化原数据
      wx.showLoading();
      wx.request({
        url: app.globalData.apiLink + '/api/services/app/Address/GetAddress?Id='+id,
        method: "GET",
        header: {
          "Abp.TenantId": "1",
          "Authorization": "Bearer " + wx.getStorageSync("accessToken"),
          "Content-Type": "application/json"
        },
        success: function (res) {
          console.log(res);
          wx.hideLoading();
          if (res.data.success) {
            that.getPcsIndex(res.data.result.items[0].province, res.data.result.items[0].city, res.data.result.items[0].district);
            that.setData({
              id: res.data.result.items[0].id,
              street: res.data.result.items[0].street,
              selProvince: res.data.result.items[0].province,
              selCity: res.data.result.items[0].city,
              selDistrict: res.data.result.items[0].district,
              receiver: res.data.result.items[0].receiver,
              contactPhoneNumber: res.data.result.items[0].contactPhoneNumber
              });
            that.setDBSaveAddressId(res.data.result.items[0]);
            return;
          } else {
            wx.showModal({
              title: '提示',
              content: '无法获取快递地址数据',
              showCancel: false
            })
          }
        }
      })
    }
  },
  setDBSaveAddressId: function(data) {
    var retSelIdx = 0;
    for (var i = 0; i < commonCityData.cityData.length; i++) {
      if (data.province == commonCityData.cityData[i].id) {
        this.data.selProvinceIndex = i;
        for (var j = 0; j < commonCityData.cityData[i].cityList.length; j++) {
          if (data.city == commonCityData.cityData[i].cityList[j].id) {
            this.data.selCityIndex = j;
            for (var k = 0; k < commonCityData.cityData[i].cityList[j].districtList.length; k++) {
              if (data.district == commonCityData.cityData[i].cityList[j].districtList[k].id) {
                this.data.selDistrictIndex = k;
              }
            }
          }
        }
      }
    }
  },
  getCityName: function (cityData, p, c, s) {
    var dizhi = "";
    for (var i = 0; i < cityData.length; i++) {
      if (cityData[i].id == p) {
        dizhi += cityData[i].name;
      }
      for (var j = 0; j < cityData[i].cityList.length; j++) {
        if (cityData[i].cityList[j].id == c) {
          dizhi += '-' + cityData[i].cityList[j].name;
        }
        for (var k = 0; k < cityData[i].cityList[j].districtList.length; k++) {
          if (cityData[i].cityList[j].districtList[k].id == s) {
            dizhi += '-' + cityData[i].cityList[j].districtList[k].name;
          }
        }
      }
    }
    return dizhi;
  },
  getPcsIndex: function (id,cid,sid) {
    var that = this;
    var cityData = commonCityData.cityData;
   // console.log(cityData);
   // provinces: [],
     // citys: [],
     //   districts: [],


    var p=[];
    var c=[];
    var s=[];
    for (var i = 0; i < cityData.length; i++) {
      p.push(cityData[i].name);
      if (cityData[i].id == id) {
        that.setData({
          selProvinceIndex: i
        });
        for (var j = 0; j < cityData[i].cityList.length; j++) {
          c.push(cityData[i].cityList[j].name);
          if (cityData[i].cityList[j].id == cid) {
            console.log(cityData[i].cityList);
            that.setData({
              selCityIndex: j
            });
            for (var k = 0; k < cityData[i].cityList[j].districtList.length; k++) {
              s.push(cityData[i].cityList[j].districtList[k].name);
              if (cityData[i].cityList[j].districtList[k].id == sid) {
                console.log(cityData[i].cityList[j].districtList);
                that.setData({
                  selDistrictIndex: k
                });
              }
            }
          }
        
        }

      }
    }
       // provinces: [],
     // citys: [],
     //   districts: [],
    that.setData({
      provinces: p,
      citys:c,
      districts:s
    });
  }
})

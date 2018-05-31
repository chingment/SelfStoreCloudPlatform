/*
声明：本脚步是结合jquery和Lumos管理系统编写的
作者：邱庆文
创建日期：2014-05-13
版本号为:lumos.2.0.1
*/


(function ($) {
    $.lumos.common = {
        //-----------------------------------------
        getCurAgeByBirthday: function (strBirthday) {
            var returnAge;
            var strBirthdayArr = strBirthday.split("-");
            var birthYear = strBirthdayArr[0];
            var birthMonth = strBirthdayArr[1];
            var birthDay = strBirthdayArr[2];

            d = new Date();
            var nowYear = d.getFullYear();
            var nowMonth = d.getMonth() + 1;
            var nowDay = d.getDate();
            if (nowYear == birthYear) {
                returnAge = 0; //同年 则为0岁
            }
            else {
                var ageDiff = nowYear - birthYear; //年之差
                if (ageDiff > 0) {
                    if (nowMonth == birthMonth) {
                        var dayDiff = nowDay - birthDay; //日之差
                        if (dayDiff < 0) {
                            returnAge = ageDiff - 1;
                        }
                        else {
                            returnAge = ageDiff;
                        }
                    }
                    else {
                        var monthDiff = nowMonth - birthMonth; //月之差
                        if (monthDiff < 0) {
                            returnAge = ageDiff - 1;
                        }
                        else {
                            returnAge = ageDiff;
                        }
                    }
                }
                else {
                    returnAge = -1; //返回-1 表示出生日期输入错误 晚于今天
                }
            }

            return returnAge; //返回周岁年龄
        },
        //-----------------------------------------
        checkCardId: function (socialNo) {
            if (socialNo == "") {
                // alert("输入身份证号码不能为空!");
                return (false);
            }

            if (socialNo.length != 15 && socialNo.length != 18) {
                // alert("输入身份证号码格式不正确!");
                return (false);
            }

            var area = { 11: "北京", 12: "天津", 13: "河北", 14: "山西", 15: "内蒙古", 21: "辽宁", 22: "吉林", 23: "黑龙江", 31: "上海", 32: "江苏", 33: "浙江", 34: "安徽", 35: "福建", 36: "江西", 37: "山东", 41: "河南", 42: "湖北", 43: "湖南", 44: "广东", 45: "广西", 46: "海南", 50: "重庆", 51: "四川", 52: "贵州", 53: "云南", 54: "西藏", 61: "陕西", 62: "甘肃", 63: "青海", 64: "宁夏", 65: "新疆", 71: "台湾", 81: "香港", 82: "澳门", 91: "国外" };

            if (area[parseInt(socialNo.substr(0, 2))] == null) {
                //  alert("身份证号码不正确(地区非法)!");
                return (false);
            }

            if (socialNo.length == 15) {
                pattern = /^\d{15}$/;
                if (pattern.exec(socialNo) == null) {
                    // alert("15位身份证号码必须为数字！");
                    return (false);
                }
                var birth = parseInt("19" + socialNo.substr(6, 2));
                var month = socialNo.substr(8, 2);
                var day = parseInt(socialNo.substr(10, 2));
                switch (month) {
                    case '01':
                    case '03':
                    case '05':
                    case '07':
                    case '08':
                    case '10':
                    case '12':
                        if (day > 31) {
                            //  alert('输入身份证号码不格式正确!');
                            return false;
                        }
                        break;
                    case '04':
                    case '06':
                    case '09':
                    case '11':
                        if (day > 30) {
                            //   alert('输入身份证号码不格式正确!');
                            return false;
                        }
                        break;
                    case '02':
                        if ((birth % 4 == 0 && birth % 100 != 0) || birth % 400 == 0) {
                            if (day > 29) {
                                //  alert('输入身份证号码不格式正确!');
                                return false;
                            }
                        } else {
                            if (day > 28) {
                                //  alert('输入身份证号码不格式正确!');
                                return false;
                            }
                        }
                        break;
                    default:
                        //  alert('输入身份证号码不格式正确!');
                        return false;
                }
                var nowYear = new Date().getYear();
                if (nowYear - parseInt(birth) < 15 || nowYear - parseInt(birth) > 100) {
                    //   alert('输入身份证号码不格式正确!');
                    return false;
                }
                return (true);
            }

            var Wi = new Array(
	            7, 9, 10, 5, 8, 4, 2, 1, 6,
	            3, 7, 9, 10, 5, 8, 4, 2, 1
	            );
            var lSum = 0;
            var nNum = 0;
            var nCheckSum = 0;

            for (i = 0; i < 17; ++i) {


                if (socialNo.charAt(i) < '0' || socialNo.charAt(i) > '9') {
                    //    alert("输入身份证号码格式不正确!");
                    return (false);
                }
                else {
                    nNum = socialNo.charAt(i) - '0';
                }
                lSum += nNum * Wi[i];
            }


            if (socialNo.charAt(17) == 'X' || socialNo.charAt(17) == 'x') {
                lSum += 10 * Wi[17];
            }
            else if (socialNo.charAt(17) < '0' || socialNo.charAt(17) > '9') {
                //  alert("输入身份证号码格式不正确!");
                return (false);
            }
            else {
                lSum += (socialNo.charAt(17) - '0') * Wi[17];
            }



            if ((lSum % 11) == 1) {
                return true;
            }
            else {
                //   alert("输入身份证号码格式不正确!");
                return (false);
            }
        },
        //-----------------------------------------
        getDateDiff: function (startTime, endTime, diffType) {
            //将xxxx-xx-xx的时间格式，转换为 xxxx/xx/xx的格式 
            startTime = startTime.replace(/\-/g, "/");
            endTime = endTime.replace(/\-/g, "/");
            //将计算间隔类性字符转换为小写 
            diffType = diffType.toLowerCase();
            var sTime = new Date(startTime); //开始时间 
            var eTime = new Date(endTime); //结束时间 
            //作为除数的数字 
            var divNum = 1;
            switch (diffType) {
                case "second":
                    divNum = 1000;
                    break;
                case "minute":
                    divNum = 1000 * 60;
                    break;
                case "hour":
                    divNum = 1000 * 3600;
                    break;
                case "day":
                    divNum = 1000 * 3600 * 24;
                    break;
                default:
                    break;
            }
            return parseInt((eTime.getTime() - sTime.getTime()) / parseInt(divNum));
        },
        //-----------------------------------------
        htmlEnFilter: function (str) {

            str = str.replace(/&/g, '&amp;');
            str = str.replace(/</g, '&lt;');
            str = str.replace(/>/g, '&gt;');
            str = str.replace(/"/g, '&quot;');
            str = str.replace(/\\/g, '\\\\');
            return str;

        },
        //-----------------------------------------
        urlParamConvert: function (name, id, type) {
            if (typeof type == "undefined") {
                if ($("#" + id).length > 0) {
                    return "&" + name + "=" + $("#" + id).val();
                }
                else {
                    return "";
                }
            }
            else {
                if (id != null) {
                    return "&" + name + "=" + id;
                }
                else {
                    return "";
                }
            }
        },
        //-----------------------------------------
        isNullOrEmpty: function (obj) {
            if (obj == null) {
                return true;
            }
            else if (obj == "") {
                return true;
            }
            else {
                return false;
            }
        },
        //-----------------------------------------
        hideFromAsterisk: function (key, val, len) {
            $('body').data(key, val);
            if (val.length > 4) {
                val = val.substring(0, val.length - len) + "****";
            }
            return val;
        },
        //-----------------------------------------
        getHideFromAsterisk: function (key, obj) {
            var id = $(obj).attr("id");
            // if (!$.lumos.common.checkCardId($(obj).val())) { 
            if ($(obj).val().indexOf('*') > -1) {
                $('#hid' + id).val($('body').data(key));
            }
            else {
                if ($(obj).val() != $('body').data(key)) {
                    $('#hid' + id).val($(obj).val());
                }
            }
        },
        //-----------------------------------------
        getCardIdHideFromAsterisk: function (idtype, key, obj) {
            var id = $(obj).attr("id");
            if (idtype.indexOf("身份证") > -1) {
                if (!$.lumos.common.checkCardId($(obj).val())) {
                    $('#hid' + id).val($('body').data(key));
                }
                else {
                    if ($(obj).val() != $('body').data(key)) {
                        $('#hid' + id).val($(obj).val());
                    }
                }
            }
            else {
                $('#hid' + id).val($(obj).val());
            }
        },
        //-----------------------------------------
        getMobieNoHideFromAsterisk: function (key, obj) {
            var id = $(obj).attr("id");
            $('#hid' + id).val($(obj).val());

        },
        //-----------------------------------------
        getCurrentDate: function () {
            var nowYear = d.getFullYear();
            var nowMonth = d.getMonth() + 1;
            var nowDay = d.getDate();
            return nowYear + "-" + nowMonth + "-" + nowDay;
        },
        //-----------------------------------------
        isFloat: function (strVal) {
            if (strVal.toString() == "") return false;
            var chk = parseFloat(strVal);
            if (chk != strVal) {
                return false;
            }
            return true;
        },
        //-----------------------------------------
        isInt: function (strVal) {

            if (strVal.toString() == "") return false;
            var chk = parseInt(strVal);
            if (chk != strVal) {
                return false;
            }
            return true;
        },
        isDateTime: function (str, frmt) {
   
            var r;
            if (frmt == "yyyy-MM-dd") {
                r = str.match(/^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2})$/);
                if (r == null) return false;
                var d = new Date(r[1], r[3] - 1, r[4]);
                return (d.getFullYear() == r[1] && (d.getMonth() + 1) == r[3] && d.getDate() == r[4]);
            }
            else if (frmt == "yyyy-MM-dd HH:mm:ss") {
                r = str.match(/^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2}) (\d{1,2}):(\d{1,2}):(\d{1,2})$/);
                if (r == null) return false;
                var d = new Date(r[1], r[3] - 1, r[4], r[5], r[6], r[7]);
                return (d.getFullYear() == r[1] && (d.getMonth() + 1) == r[3] && d.getDate() == r[4] && d.getHours() == r[5] && d.getMinutes() == r[6] && d.getSeconds() == r[7]);
            }
            else if (frmt == "yyyy-MM-dd HH:mm") {
                r = str.match(/^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2}) (\d{1,2}):(\d{1,2})$/);
                if (r == null) return false;
                var d = new Date(r[1], r[3] - 1, r[4], r[5], r[6], "00");
                return (d.getFullYear() == r[1] && (d.getMonth() + 1) == r[3] && d.getDate() == r[4] && d.getHours() == r[5] && d.getMinutes() == r[6] && d.getSeconds() == "00");
            }
            else if (frmt == "HH:mm:ss") {
                r = str.match(/^(\d{1,2})(:)?(\d{1,2})\2(\d{1,2})$/);
                if (r == null) { return false; }
                if (r[1] > 24 || r[3] > 60 || r[4] > 60) {
                    return false
                }
                return true;
            }
            else {
                r = str.match(/^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2}) (\d{1,2}):(\d{1,2}):(\d{1,2})$/);
                if (r == null) return false;
                var d = new Date(r[1], r[3] - 1, r[4], r[5], r[6], r[7]);
                return (d.getFullYear() == r[1] && (d.getMonth() + 1) == r[3] && d.getDate() == r[4] && d.getHours() == r[5] && d.getMinutes() == r[6] && d.getSeconds() == r[7]);
            }


        },


        //-----------------------------------------
        convertDecimalPrec: function (floatvar, position) {
            var f_x = parseFloat(floatvar);
            if (isNaN(f_x)) {
                return 0;
            }
            var f_x = Math.round(floatvar * 100) / 100;
            var s_x = f_x.toString();
            var pos_decimal = s_x.indexOf('.');
            if (pos_decimal < 0) {
                pos_decimal = s_x.length;
                s_x += '.';
            }
            while (s_x.length <= pos_decimal + position) {
                s_x += '0';
            }
            return s_x;
        },

        convertToInt:function (val)  {

            if(typeof val=="boolean") {

                if(val)
                {
                    return 1;
                }
                else {
                    return 0;
                }
            }
            else
            {
                return parseInt(val);
            }
        },


        convertToDate: function (val) {
         
            if (val == null) {
                return "";
            }
            else {
                return val.toDate();
            }
        },

        convertToDateTime: function (val, nulstr) {

            if (val == null) {
                if (typeof nulstr == "undefined")
                    return "";
                else
                    return nulstr;
            }
            else {
                return val.toDateTime();
            }
        },

        //-----------------------------------------
        getRandom: function (n) {
            return Math.floor(Math.random() * n + 1)
        },
        //-----------------------------------------
        isPageError: function (data) {
            if (data != null || data != NaN) {
                if (data == null) {
                    return true;
                }
                else if (typeof (data.ErrorPage) != "undefined") {
                    window.location.href = data.ErrorPage;
                    return true;
                }
                else if (data.toString().indexOf("MessagesPage.aspx") > -1) {
                    var str = data;
                    str = str.replace(/"/g, '');
                    str = str.replace(/{/g, '');
                    str = str.replace(/}/g, '');
                    var arr = str.split(':')
                    if (typeof (arr[1]) != "undefined") {
                        window.location.href = arr[1];
                        return true;
                    }
                    else {
                        alert("请检查错误");
                        return true;
                    }
                }
                else {
                    return false;
                }
            }
            else {

                return false;
            }
        },
        //----------------------------------------------
        getUrlParamValue: function getUrlParam(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
            var r = window.location.search.substr(1).match(reg);  //匹配目标参数
            if (r != null) return unescape(r[2]); return null; //返回参数值

        },
        //----------------------------------------------
        loadingShow: function (msg) {
            //获取文件的绝对路径
            var uri = ""
            var url = window.top.location.href.replace("http://", "");
            var url1 = "";
            if (url.lastIndexOf("/") > 0) {
                url1 = url.substring(0, url.lastIndexOf("/"));
            } else {
                url1 = url
            }
            uri = "/Content/base/images/loading.gif";
            if ($('#lumos_loading').length == 0) {
                $("body").append("<div id='lumos_loading'   > <div class='lumos_loading_indicator' ><img src='" + uri + "' alt='' />当前系统<br /><span id='lumos_loading_msg'>正在初始化，请稍候......</span></div></div>");
                $("#lumos_loading_msg").text(msg);
                $("#lumos_loading").show();
            }
            else {
                $("#lumos_loading_msg").text(msg);
                $("#lumos_loading").show();
            }
        },
        //----------------------------------------------
        loadingHide: function () {
            $("#lumos_loading").hide();
        },
        setTabHeight: function (id, tabIndexVal) {
      
            var iframe = $('#' + id).find('.tab-content>.content').eq(tabIndexVal).find('.tabContent');
            if (iframe != null) {
                $(iframe).unbind("load");
                $(iframe).load(function () {
                   // window.top.document.body.scrollTop= 10;
                    var mainheight = $(this).contents().height();
                    // alert(mainheight) //.find(".gbr-mainbody")
                    //  setTimeout(function () { $('#' + id).find('.content').height(mainheight + "px"); }, 100)
                    $('#' + id).find('.content').height(mainheight + "px");
         
                });
            }
            //  document.getElementById(id).style.height = maxHeight + "px";
        },
        setIframeHeight: function () {
            var pfs = parent.frames;
            for (var i = 0; i < pfs.length; i++) {
                if (pfs[i] == window) {
                    var mainheight = $(pfs[i].document).contents().find("body").height();
                    var mainiframename = $(pfs[i]).attr("name");
                    $("iframe[name=" + mainiframename + "]", parent.document).parent().parent().height(mainheight + 30);
                }
            }
        },
        setIframeParentHeight: function () {
            var pfs = parent.frames;
            for (var i = 0; i < pfs.length; i++) {
                if (pfs[i] == window) {
                    var mainheight = $(pfs[i].document).contents().find("body").height();
                    var mainiframename = $(pfs[i]).attr("name");
                    $("iframe[name=" + mainiframename + "]", parent.document).parent().parent().height(mainheight + 30);
                }
            }
        },
        setIframeObjectHeight: function (id) {
            var pfs = parent.frames;
            for (var i = 0; i < pfs.length; i++) {
                if (pfs[i] == window) {
                    var mainheight = $(pfs[i]).height();
                    $("#" + id).height(mainheight - 48);
                }
            }
        },
        changedRadioChecked: function (name, val) {
            var rd_IsSetHome_op = document.getElementsByName(name);
            for (var j = 0; j < rd_IsSetHome_op.length; j++) {
                rd_IsSetHome_op[j].checked = false;
                if (rd_IsSetHome_op[j].value == val) {
                    rd_IsSetHome_op[j].checked = true;
                }
            }
        },
        singleImgSelectPreview: function(file, previewid, imgid, width, height) {
            var MAXWIDTH = width;
            var MAXHEIGHT = height;
            var div = document.getElementById(previewid);
            if (file.files && file.files[0]) {
                div.innerHTML = '<img id=' + imgid + '>';
                var img = document.getElementById(imgid);
                img.onload = function () {
                    var rect = $.lumos.common.clacImgZoomParam(MAXWIDTH, MAXHEIGHT, img.offsetWidth, img.offsetHeight);
                    img.width = rect.width;
                    img.height = rect.height;
                    img.style.marginTop = rect.top + 'px';
                }
                var reader = new FileReader();
                reader.onload = function (evt) { img.src = evt.target.result; }
                reader.readAsDataURL(file.files[0]);
            }
            else {
                var sFilter = 'filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod=scale,src="';
                file.select();
                var src = document.selection.createRange().text;
                div.innerHTML = '<img id=' + imgid + '>';
                var img = document.getElementById('imghead');
                img.filters.item('DXImageTransform.Microsoft.AlphaImageLoader').src = src;
                var rect = $.lumos.common.clacImgZoomParam(MAXWIDTH, MAXHEIGHT, img.offsetWidth, img.offsetHeight);
                status = ('rect:' + rect.top + ',' + rect.left + ',' + rect.width + ',' + rect.height);
                div.innerHTML = "<div id=divhead style='width:" + rect.width + "px;height:" + rect.height + "px;margin-top:" + rect.top + "px;margin-left:" + rect.left + "px;" + sFilter + src + "\"'></div>";
            }
        },
        clacImgZoomParam:function (maxWidth, maxHeight, width, height) {
            var param = { top: 0, left: 0, width: width, height: height };
            if (width > maxWidth || height > maxHeight) {
                rateWidth = width / maxWidth;
                rateHeight = height / maxHeight;

                if (rateWidth > rateHeight) {
                    param.width = maxWidth;
                    param.height = Math.round(height / rateWidth);
                } else {
                    param.width = Math.round(width / rateHeight);
                    param.height = maxHeight;
                }
            }

            param.left = Math.round((maxWidth - param.width) / 2);
            param.top = Math.round((maxHeight - param.height) / 2);
            return param;
        },

        singleImgDraw: function (previewid, ImgD, iwidth, iheight, srcurl) {
            var div = document.getElementById(previewid);
            div.innerHTML = '<img id=' + ImgD + '>';
            var img = document.getElementById(ImgD);
            img.onload = function () {
                var rect =$.lumos.common.clacImgZoomParam(iwidth, iheight, img.offsetWidth, img.offsetHeight);
                img.width = rect.width;
                img.height = rect.height;
                img.style.marginTop = rect.top + 'px';
            }
            img.src = srcurl;
        },
        //////////////////////////////
        getWorkDayCount:function (mode,Holiday,WeekendsOff, beginDay, endDay) {


            function nearlyWeeks(mode, weekcount, end) {

                /*
            
                功能：计算当前时间（或指定时间），向前推算周数(weekcount)，得到结果周的第一天的时期值；
            
                参数：
            
                mode -推算模式（'cn'表示国人习惯【周一至周日】；'en'表示国际习惯【周日至周一】）
            
                weekcount -表示周数（0-表示本周， 1-前一周，2-前两周，以此推算）；
            
                end -指定时间的字符串（未指定则取当前时间）；
            
                */



                if (mode == undefined) mode = "cn";

                if (weekcount == undefined) weekcount = 0;

                if (end != undefined)

                    end = new Date(new Date(end).toDateString());

                else

                    end = new Date(new Date().toDateString());



                var days = 0;

                if (mode == "cn")

                    days = (end.getDay() == 0 ? 7 : end.getDay()) - 1;

                else

                    days = end.getDay();



                return new Date(end.getTime() - (days + weekcount * 7) * 24 * 60 * 60 * 1000);

            }


            /*
        
            功能：计算一段时间内工作的天数。不包括周末和法定节假日，法定调休日为工作日，周末为周六、周日两天；
        
            参数：
        
            mode -推算模式（'cn'表示国人习惯【周一至周日】；'en'表示国际习惯【周日至周一】）
        
            beginDay -时间段开始日期；
        
            endDay -时间段结束日期；
        
            */

            var begin = new Date(beginDay.toDateString());

            var end = new Date(endDay.toDateString());



            //每天的毫秒总数，用于以下换算

            var daytime = 24 * 60 * 60 * 1000;

            //两个时间段相隔的总天数

            var days = (end - begin) / daytime + 1;
      
            //时间段起始时间所在周的第一天

            var beginWeekFirstDay = nearlyWeeks(mode, 0, beginDay.getTime()).getTime();


            //时间段结束时间所在周的最后天
            // alert(nearlyWeeks(mode, 0, endDay.getTime()))
            var endWeekOverDay = nearlyWeeks(mode, 0, endDay.getTime()).getTime() + 6 * daytime;



            //由beginWeekFirstDay和endWeekOverDay换算出，周末的天数

            // var weekEndCount = ((endWeekOverDay - beginWeekFirstDay) / daytime + 1) / 7 * 2;
            var weekEndCount=0;
            //根据参数mode，调整周末天数的值

            if (mode == "cn") {

                // if (endDay.getDay() > 0 && endDay.getDay() < 6)

                //  weekEndCount -= 2;

                //  else if (endDay.getDay() == 6)

                //  weekEndCount -= 1;



                // if (beginDay.getDay() == 0) weekEndCount -= 1;

            }

            else {

                if (endDay.getDay() < 6) weekEndCount -= 1;



                if (beginDay.getDay() > 0) weekEndCount -= 1;

            }


            //根据调休设置，调整周末天数（排除调休日）

            $.each(WeekendsOff, function (i, offitem) {

                var itemDay = new Date(offitem.split('-')[0] + "/" + offitem.split('-')[1] + "/" + offitem.split('-')[2]);

                //如果调休日在时间段区间内，且为周末时间（周六或周日），周末天数值-1

                if (itemDay.getTime() >= begin.getTime() && itemDay.getTime() <= end.getTime() && (itemDay.getDay() == 0 || itemDay.getDay() == 6))

                    weekEndCount -= 1;

            });

            //根据法定假日设置，计算时间段内周末的天数（包含法定假日）

            $.each(Holiday, function (i, itemHoliday) {

                var itemDay = new Date(itemHoliday.split('-')[0] + "/" + itemHoliday.split('-')[1] + "/" + itemHoliday.split('-')[2]);

                //如果法定假日在时间段区间内，且为工作日时间（周一至周五），周末天数值+1

                if (itemDay.getTime() >= begin.getTime() && itemDay.getTime() <= end.getTime() )

                    weekEndCount += 1;

            });



            //工作日 = 总天数 - 周末天数（包含法定假日并排除调休日）

            return days - weekEndCount;

        },
        getHolidayDayCount: function (mode, Holiday, WeekendsOff, beginDay, endDay) {


            function nearlyWeeks(mode, weekcount, end) {

                /*
            
                功能：计算当前时间（或指定时间），向前推算周数(weekcount)，得到结果周的第一天的时期值；
            
                参数：
            
                mode -推算模式（'cn'表示国人习惯【周一至周日】；'en'表示国际习惯【周日至周一】）
            
                weekcount -表示周数（0-表示本周， 1-前一周，2-前两周，以此推算）；
            
                end -指定时间的字符串（未指定则取当前时间）；
            
                */



                if (mode == undefined) mode = "cn";

                if (weekcount == undefined) weekcount = 0;

                if (end != undefined)

                    end = new Date(new Date(end).toDateString());

                else

                    end = new Date(new Date().toDateString());



                var days = 0;

                if (mode == "cn")

                    days = (end.getDay() == 0 ? 7 : end.getDay()) - 1;

                else

                    days = end.getDay();



                return new Date(end.getTime() - (days + weekcount * 7) * 24 * 60 * 60 * 1000);

            }


            /*
        
            功能：计算一段时间内工作的天数。不包括周末和法定节假日，法定调休日为工作日，周末为周六、周日两天；
        
            参数：
        
            mode -推算模式（'cn'表示国人习惯【周一至周日】；'en'表示国际习惯【周日至周一】）
        
            beginDay -时间段开始日期；
        
            endDay -时间段结束日期；
        
            */

            var begin = new Date(beginDay.toDateString());

            var end = new Date(endDay.toDateString());



            //每天的毫秒总数，用于以下换算

            var daytime = 24 * 60 * 60 * 1000;

            //两个时间段相隔的总天数

            var days = (end - begin) / daytime + 1;

            //时间段起始时间所在周的第一天

            var beginWeekFirstDay = nearlyWeeks(mode, 0, beginDay.getTime()).getTime();


            //时间段结束时间所在周的最后天
            // alert(nearlyWeeks(mode, 0, endDay.getTime()))
            var endWeekOverDay = nearlyWeeks(mode, 0, endDay.getTime()).getTime() + 6 * daytime;



            //由beginWeekFirstDay和endWeekOverDay换算出，周末的天数

            // var weekEndCount = ((endWeekOverDay - beginWeekFirstDay) / daytime + 1) / 7 * 2;
            var weekEndCount = 0;
            //根据参数mode，调整周末天数的值

            if (mode == "cn") {

                // if (endDay.getDay() > 0 && endDay.getDay() < 6)

                //  weekEndCount -= 2;

                //  else if (endDay.getDay() == 6)

                //  weekEndCount -= 1;



                // if (beginDay.getDay() == 0) weekEndCount -= 1;

            }

            else {

                if (endDay.getDay() < 6) weekEndCount -= 1;



                if (beginDay.getDay() > 0) weekEndCount -= 1;

            }


            //根据调休设置，调整周末天数（排除调休日）

            $.each(WeekendsOff, function (i, offitem) {

                var itemDay = new Date(offitem.split('-')[0] + "/" + offitem.split('-')[1] + "/" + offitem.split('-')[2]);

                //如果调休日在时间段区间内，且为周末时间（周六或周日），周末天数值-1

                if (itemDay.getTime() >= begin.getTime() && itemDay.getTime() <= end.getTime() && (itemDay.getDay() == 0 || itemDay.getDay() == 6))

                    weekEndCount -= 1;

            });

            //根据法定假日设置，计算时间段内周末的天数（包含法定假日）

            $.each(Holiday, function (i, itemHoliday) {

                var itemDay = new Date(itemHoliday.split('-')[0] + "/" + itemHoliday.split('-')[1] + "/" + itemHoliday.split('-')[2]);

                //如果法定假日在时间段区间内，且为工作日时间（周一至周五），周末天数值+1

                if (itemDay.getTime() >= begin.getTime() && itemDay.getTime() <= end.getTime())

                    weekEndCount += 1;

            });



            //工作日 = 总天数 - 周末天数（包含法定假日并排除调休日）

            return weekEndCount;

        },
        getSpanDayCount: function (mode, beginDay, endDay) {


          

            var begin = new Date(beginDay.toDateString());

            var end = new Date(endDay.toDateString());



            //每天的毫秒总数，用于以下换算

            var daytime = 24 * 60 * 60 * 1000;

            //两个时间段相隔的总天数

            var days = (end - begin) / daytime + 1;
            return days;


        },


        // 修复 IE 下 PNG 图片不能透明显示的问题
        fixPNG: function (myImage) {
            var arVersion = navigator.appVersion.split("MSIE");
            var version = parseFloat(arVersion[1]);
            if ((version >= 5.5) && (version < 7) && (document.body.filters)) {
                var imgID = (myImage.id) ? "id='" + myImage.id + "' " : "";
                var imgClass = (myImage.className) ? "class='" + myImage.className + "' " : "";
                var imgTitle = (myImage.title) ? "title='" + myImage.title + "' " : "title='" + myImage.alt + "' ";
                var imgStyle = "display:inline-block;" + myImage.style.cssText;
                var strNewHTML = "<span " + imgID + imgClass + imgTitle

              + " style=\"" + "width:" + myImage.width

              + "px; height:" + myImage.height

              + "px;" + imgStyle + ";"

              + "filter:progid:DXImageTransform.Microsoft.AlphaImageLoader"

              + "(src=\'" + myImage.src + "\', sizingMethod='scale');\"></span>";
                myImage.outerHTML = strNewHTML;
            }
        }

    }


})(jQuery);


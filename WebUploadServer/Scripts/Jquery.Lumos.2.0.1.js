/*
声明：本脚步是结合jquery和Lumos管理系统编写的
作者：邱庆文
创建日期：2014-05-13
版本号为:lumos.2.0.1
*/


(function ($) {
    $.lumos = {
        jsonHelper: {
            add: function (json, json2, key) {
                var isflag = false;
                if (json.length == 0) {
                    json.push(json2);
                    return true;
                } else {
                    $.each(json, function (p_index, p_row) {

                        if (json[p_index][key] == json2[key]) {
                            json[p_index] = json2;
                            isflag = true;
                        }
                    });
                    if (!isflag) {
                        json.push(json2);
                        return true;
                    }
                    else {
                        return false;
                    }
                }
            },
            edit: function (json, json2, key, value) {
                $.each(json, function (p_index, p_row) {
                    if (json[p_index][key] == value) {

                        $.each(json[p_index], function (x, y) {
                            json[p_index][x] = json2[x];
                        });
                    }
                });
            },
            del: function (json, key, value) {

                var index = -1;
                $.each(json, function (p_index, p_row) {
                    if (json[p_index][key] == value) {
                        index = p_index;
                    }
                });
                if (index != -1) {
                    json.splice(index, 1);
                }

            },
            getDetail: function (json, key, value) {
                var index = 0;
                $.each(json, function (p_index, p_row) {
                    if (json[p_index][key] == value) {
                        index = p_index;
                    }
                });
                return json[index];
            }
        },
        skin: {
            setSkin: function set(n) {
                var pfs = window.top.frames;
                for (var i = 0; i < pfs.length; i++) {
                    $(pfs[i].document).contents().find("#cssfile").attr("href", "/Content/themes/manager/" + n + "/Site.css");//设置页面样式
                    $(pfs[i].parent.document).contents().find("#cssfile").attr("href", "/Content/themes/manager/" + n + "/Site.css");//设置页面样式
                    // $(pfs[i].document).contents().find("#artDialog").attr("src", "/Scripts/ArtDialog/artDialog.source.js?skin=" + n);//设置页面样式
                    // $(pfs[i].parent.document).contents().find("#artDialog").attr("src", "/Scripts/ArtDialog/artDialog.source.js?skin=" + n);//设置页面样式
                }
                $.lumos.skin.setSkinCookie(n);//保存当前样式 
            },
            setSkinCookie: function (n) {

                var expires = new Date();
                expires.setTime(expires.getTime() + 24 * 60 * 60 * 365 * 1000);
                var flag = "Skin_Cookie=" + n;
                document.cookie = flag + ";path=/";
            },
            getSkinCookie: function () {
                var skin = "base";
                var mycookie = document.cookie;
                var name = "Skin_Cookie";
                var start1 = mycookie.indexOf(name + "=");
                if (start1 == -1) {
                    skin = "base";//如果没有设置则显示默认样式 
                }
                else {

                    var strCookie = document.cookie;
                    var arrCookie = strCookie.split("; ");
                    for (var i = 0; i < arrCookie.length; i++) {
                        var arr = arrCookie[i].split("=");
                        if (name == arr[0]) {
                            skin = arr[1];
                        }
                    }
                }
                return skin;
            }
        },
        subString2: function (str, i, l) {
            var t = str;
            if (str.length > i) {
                t = str.substring(0, i) + l;
            }
            return t;
        },
        dataParmsPush: function (json, key, value) {
            var isflag = true;
            $.each(json, function (i, field) {
                if (field.name == key) {
                    isflag = false;
                }
            });
            if (isflag) {
                json.push({ name: key, value: value });
            }
            else {
                $.each(json, function (i, field) {
                    if (field.name == key) {
                        field.value = value;
                    }
                });
            }
        },
        dataParmsCheck: function (data) {
            if (typeof data == "string") {
                if (!data) return;
                var paramsArr = data.split('&');
                var args = {}, argsStr = [], param, name, value;
                var str = "";
                for (var i = 0; i < paramsArr.length; i++) {
                    param = paramsArr[i].split('=');
                    name = param[0], value = param[1];
                    if (typeof value != "undefined") {
                        if (value != "null") {
                            str += "" + name + "=" + value + "&";
                        }
                    }
                }
                if (str.length > 0) {
                    str = str.substring(0, str.length - 1);
                }
                return str; //以json格式返回获取的所有参数 
            }
            else {

                for (var d in data) {
                    if (typeof data[d] == "undefined") {
                        delete data[d];
                    }
                    else {
                        if (data[d] == null) {
                            delete data[d];
                        }

                    }
                }
                for (var d in data) {
                    // alert(data[d]);
                }

                return data;
            }

        },
        getUrlParamValue: function getUrlParam(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
            var r = window.location.search.substr(1).match(reg);  //匹配目标参数
            if (r != null) return unescape(r[2]); return null; //返回参数值

        },
        getIframeName: function () {
            var pfs = window.parent.frames;
            var isW = false;
            var n = "";
            for (var i = 0; i < pfs.length; i++) {
                if (pfs[i] == window) {
                    n = $(pfs[i]).attr("name")
                }
            }
            return n;

        },
        initAge: function (id, sel) {
            for (var i = 0; i < 161; i++) {
                if (sel == i) {
                    $('#' + id).append("<option value='" + i + "' selected='selected'>" + i + "</option>");
                }
                else {
                    $('#' + id).append("<option value='" + i + "'>" + i + "</option>");
                }
            }
        },
        showTip: function (msg, type) {
            if (typeof type == "undefined") {
                alert(msg);
            }
            else {
                if (type == "1") {

                    $("#lumos_msg_tip").show(); $("#lumos_msg_tip").text(msg); setTimeout(function () { $("#lumos_msg_tip").hide() }, 10000)
                }
                else if (type == "2") {
                    art.dialog({
                        title: '提示',
                        time: 3,
                        content: msg
                    });

                }
                else {
                    alert(msg);
                }
            }
        },
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
        checkControlVisible: function () {


            $(".lumos_table").each(function () {


                var isShowListCheckbox = true;
                $(this).parent().prev().each(function () {

                    if ($(this).attr("class").indexOf("lumos_btnarea") > -1) {

                        var len = $(this).find(".lumos_opfun");
                        var list_checkbox_len = $(this).find(".list_checkbox");
                        if (len.length == 0) {
                            $(this).hide();
                        }
                        else {
                            $(".tb-operation").show();
                        }

                        if (list_checkbox_len.length > 0) {
                            isShowListCheckbox = true;
                        } else {
                            isShowListCheckbox = false;
                        }
                    }



                });



                var len = $(this).find(".lumos_opfun");
                if (len.length == 0) {
                    // $(".tb_operation").hide();
                }
                else {
                    // $(".tb_operation").show();
                }
                if (!isShowListCheckbox) {
                    $($(this).find("tr")).each(function () {

                        var thcbox = $(this).find("th").eq(0).find("input[type='checkbox']");
                        $(this).find("td").eq(0).find("input[type='checkbox']").hide();
                        if (thcbox.length > 0) {
                            $(thcbox).hide();
                            $(this).find("th").eq(0).text("序号");
                        }
                    });
                }
            });
        },
        isPad: function () {
            if ((navigator.userAgent.match(/(iPad)/i))) {
                return true;
            }
            else {
                return false;
            }
        },
        isPhone: function () {
            if ((navigator.userAgent.match(/(iPhone|iPod|Android|ios|SymbianOS)/i))) {
                return true;
            }
            else {
                return false;
            }
        },
        //----------------------------------------------
        openWin: function (obj, str, title, iwidth, iheight) {

            if ($.lumos.isPhone()) {
                $(obj).attr("href", str + "&optl=1");
            }
            else if ($.lumos.isPad()) {
                $(obj).attr("href", str + "&optl=1");
            }
            else {
                currentArtDialog = art.dialog.open(str, { title: title, width: iwidth, height: iheight });
                return false;
            }

        },
        openWinMax: function (obj, str, title) {
            if ($.lumos.isPhone()) {
                $(obj).attr("href", str + "&optl=1");
            }
            else if ($.lumos.isPad()) {
                $(obj).attr("href", str + "&optl=1");
            }
            else {
                currentArtDialog = art.dialog.open(str, { title: title, width: '100%', height: '100%', top: 75, fixed: true, drag: false });
                return false;
            }
        },
        //----------------------------------------------
        closeWin: function () {
            var win = art.dialog.open.origin;
            win.location.reload();
            art.dialog.close();
        },
        //----------------------------------------------
        alert: function (message) {
            art.dialog.alert(message)
        },
        //----------------------------------------------
        confirm: function (txt) {

        },
        //----------------------------------------------
        selTableCheckBox: function (chkVal, idVal) {
            var tableid = $('#' + idVal).parent().parent().parent().parent().attr("id");
            var objChecks = $('#' + tableid).find('input[type=checkbox]');
            var objCheckAll;
            for (var i = 0; i < objChecks.length; i++) {
                if (objChecks[i].name.indexOf('CheckOne') != -1) {
                    if (idVal.indexOf('CheckAll') != -1) {
                        objCheckAll = objChecks[i];
                        if (chkVal == true && !objChecks[i].disabled) {
                            objChecks[i].checked = true;
                        }
                        else {
                            objChecks[i].checked = false;
                        }
                    }
                    else if (idVal.indexOf('CheckOne') != -1) {
                        if (objChecks[i].checked == false) {

                            objChecks[i].checked = false;
                        }
                    }
                }
            }


            if ($('#' + idVal).attr("name").indexOf('CheckOne') != -1) {
                var ObjOneChecks = $('#' + tableid).find("input[name='list_table_CheckOne']");
                var cktruelength = 0;
                for (var i = 0; i < ObjOneChecks.length; i++) {
                    if (ObjOneChecks[i].checked == true) {
                        cktruelength += 1;
                    }
                }
                if (cktruelength == ObjOneChecks.length) {
                    $('#' + tableid).find("input[name='list_table_CheckAll']").attr("checked", true);
                }
                else {
                    $('#' + tableid).find("input[name='list_table_CheckAll']").attr("checked", false);
                }
            }
        },
        tabToTableGetTabContent: function (index, value) {
            //alert(index);
        },
        post: function (opts) {


            opts = $.extend({
                url: 'test.apsx',
                data: null,
                async:false,
                success: function () { return false; }
            }, opts || {});

            var token = $("input[name=__RequestVerificationToken]").val();
            var l_Data = null;
            if (typeof opts.data == "string") {
                l_Data = opts.data; //查询条件参数
                if (token != '') {
                    l_Data += "&token=" + token;
                }
            }
            else {
                l_Data = [];

                if (opts.data != null) {

                    if (typeof opts.data.length == "undefined") {
                        for (var p in opts.data) {
                            l_Data.push({ name: p, value: opts.data[p] });
                        }
                    }
                    else {
                        l_Data = opts.data;
                    }
                }

                if (token != '') {
                    l_Data.push({ name: "token", value: token });
                }

            }
            $.ajax({
                type: "Post",
                dataType: "json",
                async: opts.async,
                data: l_Data,
                url: opts.url,
                beforeSend: function (XMLHttpRequest) {
                    $.lumos.common.loadingShow("正在加载数据...");
                },
                complete: function (XMLHttpRequest, textStatus) {
                    $.lumos.common.loadingHide();
                }
            }).done(function (data) {
                if (!$.lumos.isPageError(data)) {
                    opts.success(data);
                }
            });
        },
        newGuid: function () {
            var guid = "";
            for (var i = 1; i <= 32; i++) {
                var n = Math.floor(Math.random() * 16.0).toString(16);
                guid += n;
                if ((i == 8) || (i == 12) || (i == 16) || (i == 20))
                    guid += "-";
            }
            return guid;
        }


    }


    $.fn.setMultiSelect = function () {

        $(this).click(function () {

            var v = $(this).attr("checked");
            var o = $(this).attr("object");
            if (typeof v == "undefined") {
                v = false;
            }
            else {
                v = true;
            }

            $("input[name='" + o + "']").attr("checked", v);

            if ($("#" + o + "0Tip").length > 0) {
                if (v) {
                    $("input[name='" + o + "']").defaultPassed();
                }
                else {
                    $("input[name='" + o + "']").defaultPassed("onShow");
                }
            }
        });

        var oname = $(this).attr("object");
        var allObj = this;
        $("input[name='" + oname + "']").click(function () {

            var ObjOneChecks = $("input[name='" + oname + "']");
            var cktruelength = 0;
            for (var i = 0; i < ObjOneChecks.length; i++) {
                if (ObjOneChecks[i].checked == true) {
                    cktruelength += 1;
                }
            }
            if (cktruelength == ObjOneChecks.length) {
                $(allObj).attr("checked", true);
            }
            else {
                $(allObj).attr("checked", false);
            }
        });
    }

    $.fn.getMultiSelectValues = function () {

        var checks = "";
        $(this).each(function () {
            if ($(this).attr("checked") == "checked") {
                checks += $(this).val() + ",";
            }
        })
        if (checks.length > 0) {
            checks = checks.substring(0, checks.length - 1);
        }
        return checks;
    }

    $.fn.getMultiSelectTexts = function (values) {
        var name = $(this).attr("name");
        var currencies = values.split(',');
        var currenciesText = "";
        for (var i = 0; i < currencies.length; i++) {
            var o = $('input[name=' + name + '][value=' + currencies[i] + ']');
            currenciesText += $(o).next().html() + ",";
        }
        if (currenciesText.length > 0) {
            currenciesText = currenciesText.substring(0, currenciesText.length - 1);
        }

        return currenciesText;
    }

    $.fn.bankInput = function (options) {
        var defaults = {
            min: 0, // 最少输入字数 
            max: 25, // 最多输入字数 
            deimiter: ' ', // 帐号分隔符 
            onlyNumber: true, // 只能输入数字 
            copy: true // 允许复制 
        };
        var opts = $.extend({}, defaults, options);
        var obj = $(this);
        // obj.css({ imeMode: 'Disabled', borderWidth: '1px', color: '#000', fontFamly: 'Times New Roman' }).attr('maxlength', opts.max);
        if (obj.val() != '') obj.val(obj.val().replace(/\s/g, '').replace(/(\d{4})(?=\d)/g, "$1" + opts.deimiter));
        obj.bind('keyup', function (event) {
            if (opts.onlyNumber) {
                if (!(event.keyCode >= 48 && event.keyCode <= 57)) {
                    this.value = this.value.replace(/\D/g, '');
                }
            }
            this.value = this.value.replace(/\s/g, '').replace(/(\d{4})(?=\d)/g, "$1" + opts.deimiter);
        }).bind('dragenter', function () {
            return false;
        }).bind('onpaste', function () {
            return !clipboardData.getData('text').match(/\D/);
        }).bind('blur', function () {
            this.value = this.value.replace(/\s/g, '').replace(/(\d{4})(?=\d)/g, "$1" + opts.deimiter);
            if (this.value.length < opts.min) {
                obj.focus();
            }
        })
    }

    $.fn.uploadImage = function (options) {

        var defaults = {
            form: "form1", //表单名
            url: "/Common/UploadImage",
            path: "",
            imgId: ""
        };
        var opts = $.extend({}, defaults, options);

        $(this).attr("accept", "image/gif, image/jpeg, image/png,image/x-ms-bmp");

        var fileInputName = $(this).attr("name");

        var fileInputFileName = fileInputName + "_Value";
        $("#" + opts.form).append(" <input name=\"" + fileInputFileName + "\" type=\"hidden\" id=\"" + fileInputFileName + "\"  />")

        if (opts.imgId == "") {
            opts.imgId = fileInputName + "_Img";
        }

        if (fileInputName != "") {
            $(this).change(function () {

                var fileName = $(this).val();
                if (fileName != "") {
                    if (!fileName.match(/.jpg|.gif|.png|.bmp/i)) {
                        art.dialog.alert('您上传的图片格式(.jpg|.gif|.png|.bmp)不正确，请重新选择！');
                        $(this).val("");
                        $("#" + opts.imgId).attr("src", "").hide();
                        $("#" + fileInputFileName).val("");
                        return;
                    }

                    var fileInputFileValue = $("#" + fileInputFileName).val();
                    var url = opts.url + "?fileInputName=" + fileInputName + "&path=" + opts.path;
                    if (fileInputFileValue != "") {
                        url += "&oldFileName=" + fileInputFileValue;
                    }


                    var file_size = 0;
                    var isFileSizeFlag = false;
                    if ($.browser.msie) {
                        var img = new Image();
                        img.src = fileName;
                        while (true) {
                            if (img.fileSize > 0) {
                                if (img.fileSize > 10 * 1024) {
                                    isFileSizeFlag = true;
                                }
                                break;
                            }
                        }
                    } else {
                        file_size = this.files[0].size;
                        var size = file_size / 1024;
                        if (size > 10 * 1024) {
                            isFileSizeFlag = true;
                        }
                    }

                    if (isFileSizeFlag) {
                        art.dialog.alert("请选择的图片不要超过10M");
                        if ($(this).val() != "") {
                            $(this).val("");
                            $("#" + opts.imgId).attr("src", "").hide();
                            $("#" + fileInputFileName).val("");
                        }
                        return;
                    }



                    var p_d = art.dialog({
                        title: '图片上传',
                        content: '正在上传图片...请稍后！',
                        cancel: false,
                        lock: true,
                        dblclickClose:false
                    });
                    $("#" + opts.form).ajaxSubmit({
                        type: "post",
                        url: url,
                        dataType: "json",
                        success: function (d) {
                            p_d.close();
                            if (d.result == resultType.success) {
                                var imgObject = d.data;
                                if (imgObject.OriginalPath.length > 0) {
                                    if ($("#" + opts.imgId).length > 0) {
                                        $("#" + opts.imgId).attr("src", imgObject.OriginalPath).show();
                                    }

                                    $("#" + fileInputFileName).val(imgObject.OriginalPath);
                                }
                            }
                            else {
                                $("#" + fileInputName).val("");
                                art.dialog.alert(d.message);
                            }
                            return false;
                        }
                    });
                }

            });
        }

    }

    $.fn.decimalInput = function (num) {

        //获取当前光标在文本框的位置
        function getCurPosition(domObj) {
            var position = 0;
            if (domObj.selectionStart || domObj.selectionStart == '0') {
                position = domObj.selectionStart;
            }
            else if (document.selection) { //for IE
                domObj.focus();
                var currentRange = document.selection.createRange();
                var workRange = currentRange.duplicate();
                domObj.select();
                var allRange = document.selection.createRange();
                while (workRange.compareEndPoints("StartToStart", allRange) > 0) {
                    workRange.moveStart("character", -1);
                    position++;
                }
                currentRange.select();
            }
            return position;
        }
        //获取当前文本框选中的文本
        function getSelectedText(domObj) {
            if (domObj.selectionStart || domObj.selectionStart == '0') {
                return domObj.value.substring(domObj.selectionStart, domObj.selectionEnd);
            }
            else if (document.selection) { //for IE
                domObj.focus();
                var sel = document.selection.createRange();
                return sel.text;
            }
            else return '';
        }

        $(this).css("ime-mode", "disabled");
        this.bind("keypress", function (e) {
            if (e.charCode === 0) return true;  //非字符键 for firefox
            var code = (e.keyCode ? e.keyCode : e.which);  //兼容火狐 IE
            if (code >= 48 && code <= 57) {
                var pos = getCurPosition(this);
                var selText = getSelectedText(this);
                var dotPos = this.value.indexOf(".");
                if (dotPos > 0 && pos > dotPos) {
                    if (pos > dotPos + 2) return false;
                    if (selText.length > 0 || this.value.substr(dotPos + 1).length < num)
                        return true;
                    else
                        return false;
                }
                return true;
            }
            //输入"."
            if (code == 46) {
                var selText = getSelectedText(this);
                if (selText.indexOf(".") > 0) return true; //选中文本包含"."
                else if (/^[0-9]+\.$/.test(this.value + String.fromCharCode(code)))
                    return true;
            }
            return false;
        });
        this.bind("blur", function () {
            if (this.value.lastIndexOf(".") == (this.value.length - 1)) {
                this.value = this.value.substr(0, this.value.length - 1);
            } else if (isNaN(this.value)) {
                this.value = "";
            }
            if (this.value)
                this.value = parseFloat(this.value).toFixed(2);
            $(this).trigger("input");
        });
        this.bind("paste", function () {
            if (window.clipboardData) {
                var s = clipboardData.getData('text');
                if (!isNaN(s)) {
                    value = parseFloat(s);
                    return true;
                }
            }
            return false;
        });
        this.bind("dragenter", function () {
            return false;
        });
        this.bind("keyup", function () {
        });
        this.bind("propertychange", function (e) {
            if (isNaN(this.value))
                this.value = this.value.replace(/[^0-9\.]/g, "");
        });
        this.bind("input", function (e) {
            if (isNaN(this.value))
                this.value = this.value.replace(/[^0-9\.]/g, "");
        });
    }

    $.fn.initAge = function (val) {
        var obj = $(this);
        for (var i = 0; i < 161; i++) {
            if (val == i) {
                $(obj).append("<option value='" + i + "' selected='selected'>" + i + "</option>");
            }
            else {
                $(obj).append("<option value='" + i + "'>" + i + "</option>");
            }
        }

    }

    $.fn.addTableStyle = function () {
        //-表格隔行,滑动,点击 变色
        $(this).find(' tbody tr:even ').addClass('trold');
        $(this).find(' tbody tr ').hover(
                  function () { $(this).addClass('trmouseover'); },
                 function () {
                     $(this).removeClass('trmouseover');
                 });

        $(this).find('tbody tr ').click(
                 function () {
                     $(this).toggleClass('trselected');
                 });
    }

    /*此方法已经过时 请查看$.fn.operateData*/
    $.fn.getListTableCheckBoxVal = function (obj, type, msgtip, msgsure) {
        var l_msgtip = "请您选择要删除的所选数据！";
        var l_msgsure = "确认要删除所选择的数据吗？";
        if (typeof msgtip != "undefined") {
            l_msgtip = msgtip;
        }
        if (typeof msgsure != "undefined") {
            l_msgsure = msgsure + "\r\n";
        }
        var l_DataTable = $(this);
        var selCheckedNum = 0;
        var sekKeys = "";
        var isSure = false;
        var selTexts = "";
        var headerTitleIndex = 1;
        if (typeof type == "number") {
            $(l_DataTable).find(" tbody tr").each(function () {
                $(this).find("input:checkbox").attr("checked")
                if ($(this).find("input:checkbox").attr("checked")) {
                    selCheckedNum += 1;
                    sekKeys += "" + $(this).find("input:checkbox").val() + ",";
                    selTexts += "" + $(this).find(".tb_CurSel").text().trim() + "\r\n";
                    headerTitleIndex = $(this).find(".tb_CurSel").index();
                }
            });
            if (sekKeys.split(',').length > 0) {
                sekKeys = sekKeys.substring(0, sekKeys.length - 1);
                selTexts = selTexts.substring(0, selTexts.length - 1);
            }
            selTexts = "选择的数据【" + $(l_DataTable).find("thead tr th").eq(headerTitleIndex).text().trim() + "】为:\r\n" + selTexts;
        }
        else {
            sekKeys = "" + type + "";
            selTexts += $(obj).parent().parent().find(".tb_CurSel").text().trim();
            selTexts = "选择的数据【" + $(l_DataTable).find("thead tr th").eq(headerTitleIndex).text().trim() + "】为:\r\n" + selTexts;
        }

        l_msgsure += selTexts;
        if (selCheckedNum > 0 || typeof type == "string") {


            art.dialog({
                id: "confirm",
                content: l_msgsure,
                ok: function () {
                    isSure = true;
                    return false;
                },
                cancelVal: '取消',
                cancel: function () {
                    sekKeys = "";
                } //为true等价于function(){}
            });

        }
        else {
            art.dialog(l_msgtip, true);
        }
        return sekKeys;
    }

    $.fn.operateData = function (opts) {

        opts = $.extend({
            object: '', //tabId
            multiSelect: true, //
            selectTip: "请您选择要删除的所选数据！",
            confirmTip: "确认要删除所选择的数据吗？\r\n",
            data: null,
            ok: function () { }
        }, opts || {});

        var l_DataTable = $(this);
        var selCheckedNum = 0;
        var sekKeys = "";
        var isSure = false;
        var selTexts = "";
        var headerTitleIndex = 1;


        if (opts.multiSelect) {
            $(l_DataTable).find(" tbody tr").each(function () {
                $(this).find("input:checkbox").attr("checked")
                if ($(this).find("input:checkbox").attr("checked")) {
                    selCheckedNum += 1;
                    sekKeys += "" + $(this).find("input:checkbox").val() + ",";
                    selTexts += "" + $(this).find(".tb_CurSel").text().trim() + "<br/>";
                    headerTitleIndex = $(this).find(".tb_CurSel").index();
                }
            });
            if (sekKeys.split(',').length > 0) {
                sekKeys = sekKeys.substring(0, sekKeys.length - 1);
                selTexts = selTexts.substring(0, selTexts.length - 1);
            }
            selTexts = "选择的数据【" + $(l_DataTable).find("thead tr th").eq(headerTitleIndex).text().trim() + "】为:<br/>" + selTexts;
        }
        else {
            if (opts.data != null) {
                sekKeys = "" + opts.data + "";
            }
            else {
                sekKeys = "" + $(opts.object).parent().parent().find(".hid_pkey").val() + "";
            }
            selTexts += $(opts.object).parent().parent().find(".tb_CurSel").text().trim();
            selTexts = "选择的数据【" + $(l_DataTable).find("thead tr th").eq(headerTitleIndex).text().trim() + "】为:<br/>" + selTexts;
            selCheckedNum = 1;
        }

        opts.confirmTip += selTexts;
        if (selCheckedNum > 0) {
            var p = art.dialog({
                id: "confirm",
                icon: 'question',
                content: opts.confirmTip,
                ok: function () {
                    p.close();
                    opts.ok(sekKeys);
                    return false;
                },
                cancelVal: '取消',
                cancel: function () {
                    sekKeys = "";
                } //为true等价于function(){}
            });
        }
        else {

            art.dialog.alert(opts.selectTip);
        }
        return sekKeys;
    }


    $.fn.tabToTableChangeTab = function (tabid, tabIndex) {

        var l_DataTable = $(this);
        var l_DataTableTabId = '#' + tabid;
        var curSelIndex;

        if (typeof tabIndex == "object") {
            curSelIndex = $(tabIndex).parent().index();
        }
        else {

            var l_i = $(l_DataTableTabId + ' .tab-item').find('.item').length;

            for (var i = 0; i < l_i; i++) {
                if ($($(l_DataTableTabId + ' .tab-item').find('.item').eq(tabIndex)).find('a').length == 0) {
                    tabIndex = tabIndex + 1;
                }
                else {
                    break;
                }
            }
            curSelIndex = tabIndex;
        }


        var tabLength = $(l_DataTableTabId).find('.tab-content .content').length;
        var curSelTabox;
        for (var i = 0; i < tabLength; i++) {
            if (curSelIndex == i) {
                $(l_DataTableTabId).find('.tab-item .item').eq(i).attr('class', 'item selected');
                curSelTabox = $(l_DataTableTabId).find('.tab-content .content').eq(i);
                $(curSelTabox).show();
            }
            else {
                $(l_DataTableTabId).find('.tab-content .content').eq(i).hide();
                $(l_DataTableTabId).find('.tab-item .item').eq(i).attr('class', 'item');
            }

        }

        $(l_DataTableTabId).data('selIndex', curSelIndex);
        var tabIndexVal = curSelIndex;
        return tabIndexVal;
    }

    $.fn.tabToTableSelTr = function (tabid, obj, tabContent, bFirstBox) {
        var l_DataTable = $(this);
        var l_DataTableTabId = '#' + tabid;
        $(l_DataTable).find(' tbody tr').removeClass('selected');
        if (obj != null) {
            $(obj).parent().parent().addClass('selected');
            var projectcode = $(obj).val();

            $(l_DataTable).data('selValue', projectcode); //保存当前选中的行表示
            $('#selname').remove();
            $(obj).parent().parent().find('.tb_CurSel').prepend('<span id="selname" class="listcursel">&nbsp;</span>'); //.tb_CurSel是要放勾选图标的位置,系统定义为td的class tb_CurSel
            $(l_DataTableTabId).show();
            var seltabindex = $(l_DataTableTabId).data('selIndex');
            if (typeof (seltabindex) == "undefined") {
                seltabindex = 0;
                $(l_DataTableTabId).data('selIndex', '0');
            }
            var seltabval = $(l_DataTable).data('selValue');

            if (bFirstBox == true) {
                seltabindex = $(l_DataTable).tabToTableChangeTab(tabid, 0);
            }
            else {

                seltabindex = $(l_DataTable).tabToTableChangeTab(tabid, seltabindex);

            }
            tabContent(seltabindex, seltabval);

        }
        else {
            $('#selname').remove();
            $(l_DataTableTabId).hide();
        }

    }

    $.fn.tabToTable = function (tabid, tabContent, bFirstBox) {
        if (typeof bFirstBox == 'undefined') {
            bFirstBox = false;
        }
        var l_DataTable = $(this);
        if ($('#' + tabid + ' .tab-item ul ').find('.item').length > 0) {
            $(l_DataTable).find(' tbody tr td').not('.tb-operation,.tb_cbox').unbind("click").click(function () {
                if ($(this).parent().hasClass('seltab')) {
                    $(this).parent().addClass('seltab');
                    var obj_hid = $(this).parent().find('.hid_pkey'); //.hid_pkey 作为每一行关键传递参数给tab的主标识, tab获取相关数据可以依据它
                    $(l_DataTable).tabToTableSelTr(tabid, obj_hid, tabContent, bFirstBox);
                }
                else {
                    $(this).parent().addClass('seltab');
                    var obj_hid = $(this).parent().find('.hid_pkey'); //.hid_pkey 作为每一行关键传递参数给tab的主标识, tab获取相关数据可以依据它
                    $(l_DataTable).tabToTableSelTr(tabid, obj_hid, tabContent, bFirstBox)

                }

            });

            $('#' + tabid + ' .tab-item ul .item a').unbind("click").click(function () {
                var selIndex = $(l_DataTable).tabToTableChangeTab(tabid, this);

                var seltabval = $(l_DataTable).data('selValue');
                if (typeof (seltabval) == "undefined") {
                    seltabval = 0;
                    $('#' + tabid).data('selValue', '0');
                }
                tabContent(selIndex, seltabval);
            });

        }
        if (bFirstBox == true) {
            $('#' + tabid).show();
        }
    }

    $.fn.mulitSelector = function (options) {
        var l_mulitSel = $(this);
        var l_mulitSelID = $(l_mulitSel).attr("id");

        var l_mulitSelContentID = l_mulitSelID + "_content";
        var l_mulitCheckName = l_mulitSelID + "_check";
        if ($("#" + l_mulitSelContentID).length != 0) {
            $("#" + l_mulitSelContentID).remove();
        }

        if (typeof $('body').data(l_mulitSelID + "_value") != "undefined") {

        }
        var $input = $(this);

        var ms_html;

        var settings =
		{
		    title: "请选择类别",
		    data: null
		};

        if (options) {
            jQuery.extend(settings, options);
        }

        function initialise() {
            initContent();
            initEvent();
        }

        function initEvent() {

            $("#" + l_mulitSelContentID).find(".ms_bt_ok").click(function () {
                var result = "";
                var value = "";
                var obj = $("#" + l_mulitSelContentID).find(".allItems1 input:checked");
                for (var i = 0; i < obj.length; i++) {
                    result += (i == 0 ? "" : "+") + obj[i].value.split("@")[1];
                    value += (i == 0 ? "" : ",") + "'" + obj[i].value.split("@")[0] + "'";
                }

                $input.val(result);
                $('body').data(l_mulitSelID + "_value", value)
                $("#" + l_mulitSelID + "Value").val(value);
                ms_html.remove();
            });

            $("#" + l_mulitSelContentID).find(".ms_bt_clear").click(function () {
                ms_html.remove();
                $input.val("");
                $("#" + l_mulitSelID + "Value").val("");
            });

            $("#" + l_mulitSelContentID).find(".ms_img_close").click(function () {
                ms_html.remove();
            });

        }

        function initContent() {

            var offset = $input.offset();
            var divtop = 1 + offset.top + document.getElementById($input.attr("id")).offsetHeight + 'px';
            var divleft = offset.left + 'px';
            var popmask = document.createElement('div');

            var html = [];

            html.push(' <div class="aui_outer" id="' + l_mulitSelContentID + '" style="display:block;top:' + divtop + ';left:' + divleft + ';  position: absolute; z-index: 1999; ">');
            html.push('    <table class="aui_border" >');
            html.push('    <tbody>');
            html.push('    <tr>');
            html.push('    <td class="aui_nw"></td>');
            html.push('    <td class="aui_n"></td>');
            html.push('    <td class="aui_ne"></td>');
            html.push('    </tr>');
            html.push('    <tr>');
            html.push('    <td class="aui_w"></td>');
            html.push('    <td class="aui_c">');
            html.push('    <div class="aui_inner">');
            html.push('    <table class="aui_dialog">');
            html.push('    <tbody>');
            html.push('    <tr>');
            html.push('    <td colspan="2" class="aui_header">');
            html.push('    <div class="aui_titleBar">');
            html.push('    <div class="aui_title">' + settings.title + '</div>');
            html.push('    <a href="javascript:void(0);" class="aui_close ms_img_close" ></a>');
            html.push('    </div>');
            html.push('    </td>');
            html.push('    </tr>');
            html.push('    <tr>');
            html.push('    <td class="aui_main" style="min-width:300px">');
            html.push('    <div class="aui_content" style="width:100%">');

            html.push('				<div id="divSelecting" style="text-align:right">');
            html.push('						<input  name="" class="form_btn ms_bt_ok" type="button" value="确定" />');
            html.push('						<input  name="" class="form_btn ms_bt_clear" type="button" value="清空" /></b>');
            html.push('				</div>');
            html.push('				<div  style="text-align:left" >  ');
            html.push('					<ol class="allItems1" style="margin-left:0px;">');

            var dataArray = settings.data;
            if (dataArray != null) {
                var len = dataArray.length;
                for (var i = 0; i < len; i++) {
                    var d = dataArray[i];
                    var status = findStatus(d.sncode);
                    html.push('						<li id=$' + d.sncode + ' name=100 class="nonelay">');
                    html.push('							<a href="###">');
                    html.push('							<label for="' + l_mulitCheckName + '_' + d.sncode + '">');
                    html.push('							<input id="' + l_mulitCheckName + '_' + d.sncode + '" name=' + l_mulitCheckName + ' type="checkbox" ' + status + ' value="' + (d.sncode + '@' + d.name) + '" />' + d.name + '</label>');
                    html.push('							</a>');
                    html.push('						</li>');
                }
            }

            html.push('					</ol>');
            html.push('				</div>');





            html.push('    </div>');
            html.push('    </td>');
            html.push('    </tr>');
            html.push('    <tr>');
            html.push('    <td colspan="2" class="aui_footer">');
            html.push('    <div class="aui_buttons"></div>');
            html.push('    </td>');
            html.push('    </tr>');
            html.push('    </tbody>');
            html.push('    </table>');
            html.push('   </div>');
            html.push('    </td>');
            html.push('    <td class="aui_e"></td>');
            html.push('    </tr>');
            html.push('    <tr>');
            html.push('    <td class="aui_sw"></td>');
            html.push('    <td class="aui_s"></td>');
            html.push('    <td class="aui_se"></td>');
            html.push('    </tr>');
            html.push('    </tbody>');
            html.push('    </table>');
            html.push('    </div>');


            ms_html = $(html.join("")).appendTo('body');
            $('#TextArea1').val($(ms_html).html());

        }

        function findStatus(d) {

            var content = $input.val();
            if (jQuery.trim(content) == "") {
                return "";
            }
            if (typeof $('body').data(l_mulitSelID + "_value") != "undefined") {
                var objval = $('body').data(l_mulitSelID + "_value");
                var obj = objval.split(",");
                for (var i = 0; i < obj.length; i++) {

                    if (obj[i] == "'" + d + "'") {
                        return "checked"
                    }
                }
            }

        }

        initialise();

    }

    $.fn.loadDataTable = function (opts) {
        //  url, pageIndex, dataParms, rowFun, tabid, selTabFun
        opts = $.extend({
            url: 'test.apsx', //获取数据URL路径
            pageIndex: 0, //当前页索引
            pageSize: 10,//每页多少条
            isShowPageBar: true,
            isUseEmptyRow: false,
            tempData: null,
            isUseTempData: false,
            dataParms: 11, //查询数据条件
            rowFun: function () { return false; }, //行数据构造
            tabid: '', //tabId
            selTabFun: function () { return false; }, //
            tabIsSelFirBox: false,           //是否选择第一个tabBox
            rowDbClickFun: function () { return false; },
            rowClickFun: function () { return false; },
            isReSetTabHeight: false,
            messageHandleFun: function (message) { return false; }
        }, opts || {});

        var l_DataTable = $(this); //当前绑定Table对象
        //判断是否pageIndex为数字类型,是就返回当前的PageIndex的索引
        if (typeof opts.pageIndex != "number") {
            opts.pageIndex = $(l_DataTable).data('cur_pg_index');
        }
        else {
            $(l_DataTable).data('cur_pg_index', opts.pageIndex);
        }
        var l_Data;
        if (typeof opts.dataParms != 'undefined') {

            if (typeof opts.dataParms == "string") {
                l_Data = "pageindex=" + opts.pageIndex + "&pageSize=" + opts.pageSize + "&IframeName=" + $.lumos.getIframeName() + "&"; //查询条件参数
                l_Data += opts.dataParms;
                l_Data = $.lumos.dataParmsCheck(l_Data);

            }
            else {
                l_Data = opts.dataParms;
                var isflag = true;
                $.each(l_Data, function (i, field) {
                    if (field.name == "pageindex") {
                        isflag = false;
                    }
                });
                if (isflag) {
                    l_Data.push({ name: "pageindex", value: opts.pageIndex });
                    l_Data.push({ name: "pageSize", value: opts.pageSize });
                    l_Data.push({ name: "IframeName", value: $.lumos.getIframeName() });
                }
                else {
                    $.each(l_Data, function (i, field) {
                        if (field.name == "pageindex") {
                            field.value = opts.pageIndex;
                        }
                        else if (field.name == "pageSize") {
                            field.value = opts.pageSize;
                        }
                        else if (field.name == "IframeName") {
                            field.value = $.lumos.getIframeName();
                        }
                    });
                }
            }
        }
        var l_StrRows = ""; //行数据



        function getData(data) {
            var l_DataTablePagerId = $(l_DataTable).attr("id") + "_pager";
            if (opts.isShowPageBar) {
                if ($('#' + l_DataTablePagerId).length <= 0) {
                    $(l_DataTable).after("<div id='" + l_DataTablePagerId + "' class=\"l-list-pager\"></div>"); //追加分页
                    $(l_DataTable).data('cur_pg_id', l_DataTablePagerId);
                }
                else {
                    $('#' + l_DataTablePagerId).html('');
                    //  $(l_DataTable).after("<div id='" + l_DataTablePagerId + "'></div>"); //追加分页
                    // $(l_DataTable).data('cur_pg_id', l_DataTablePagerId);
                    //  l_DataTablePagerId = $(l_DataTable).data('cur_pg_id');
                }
            }
            var l_DataTablePager = $('#' + l_DataTablePagerId);


            $(l_DataTable).find("tbody tr").remove(); //删除所有行
            var p_table = data;

            if (typeof p_table.Rows == "undefined") {
                $.each(p_table, function (p_index, p_row) {
                    l_StrRows += opts.rowFun(p_index, p_row); //加载行数据
                });
            }
            else {
                $.each(p_table.Rows, function (p_index, p_row) {
                    l_StrRows += opts.rowFun(p_index, p_row); //加载行数据
                });
            }

            if (opts.isUseEmptyRow) {
  
                if (p_table.Rows.length < opts.pageSize) {
                    var l_StrEmptyRow = "";
                    var l_StrEmptyTdCount = $(l_DataTable).find("thead").find("th").length;
                    for (var i = 0; i < opts.pageSize - p_table.Rows.length; i++) {
                        l_StrEmptyRow += "<tr>"
                        l_StrEmptyRow += "<td></td><td></td><td></td><td></td><td></td>"
                        l_StrEmptyRow += "</tr>"
                    }
                    l_StrRows += l_StrEmptyRow;
                }
            }






            $(l_DataTable).find("tbody").append(l_StrRows); //追加所有行


            //$(l_DataTable).find("tbody tr").dblclick(function () {
            //    var tdobj = $(this).find("td").eq("0");
            //    opts.rowDbClickFun(this, $(tdobj).attr("key"));

            //});

            //$(l_DataTable).find("tbody tr").click(function () {
            //    var tdobj = $(this).find("td").eq("0");
            //    opts.rowClickFun(this, $(tdobj).attr("key"));

            //});

            $(l_DataTable).addTableStyle();
            if ($(l_DataTable).find(" thead th ").eq(0).find("input[type=checkbox]").length > 0) {
                $(l_DataTable).find(" thead th ").eq(0).find("input[type=checkbox]").attr("checked", false);
                var trobj = $(l_DataTable).find("tbody tr");
                for (var i = 0; i < trobj.length; i++) {
                    var tdobj = $(trobj[i]).find("td").eq("0");
                    $(tdobj).html("<input type='checkbox' id='" + $(l_DataTable).attr("id") + "_" + i + "CheckOne' name='" + $(l_DataTable).attr("id") + "_CheckOne' onclick='javascript: return $.lumos.selTableCheckBox(this.checked, this.id);' value='" + $(tdobj).attr("key") + "' /><input  type=\"hidden\"  class=\"hid_pkey\"   value='" + $(tdobj).attr("key") + "' /><a style='padding-left:2px'>" + $(tdobj).text() + "</a>");
                }
            }
            else {
                if ($(l_DataTable).find(" thead tr").length < 2) {
                    $(l_DataTable).find(" thead th ").eq(0).text("序号");
                    var trobj = $(l_DataTable).find("tbody tr");
                    for (var i = 0; i < trobj.length; i++) {
                        var tdobj = $(trobj[i]).find("td").eq("0");
                        $(tdobj).html("<input  type=\"hidden\"  class=\"hid_pkey\"   value='" + $(tdobj).attr("key") + "' /><a style='padding-left:2px'>" + $(tdobj).text() + "</a>");
                    }
                }
            }


            $(l_DataTablePager).show(); //页面显示
            if (opts.isShowPageBar) {
                $(l_DataTablePager).pagination(p_table.TotalRecord, {
                    callback: function (page_id, jq) { $(l_DataTable).loadDataTable({ url: opts.url, pageIndex: page_id, pageSize: opts.pageSize, dataParms: opts.dataParms, rowFun: opts.rowFun, tabid: opts.tabid, selTabFun: opts.selTabFun,isUseEmptyRow:opts.isUseEmptyRow }); },
                    items_num: p_table.TotalRecord,
                    items_per_page: p_table.PageSize,
                    current_page: opts.pageIndex,
                    onSelClick: function (index) { $(l_DataTable).loadDataTable({ url: opts.url, pageIndex: index - 1, pageSize: opts.pageSize, dataParms: opts.dataParms, rowFun: opts.rowFun, tabid: opts.tabid, selTabFun: opts.selTabFun, isUseEmptyRow: opts.isUseEmptyRow }); }
                });
            }
            if (opts.tabid != '') {
                $('#' + opts.tabid).hide();

                //以下只作为出现选择行时出现的条件,如果不需要 可以删除其留下
                var pfs = window.parent.frames;
                var isW = false;

                for (var i = 0; i < pfs.length; i++) {
                    if (pfs[i] == window) {
                        if ($(pfs[i]).attr("name") == "page_maincontent" || $(pfs[i]).attr("name").indexOf('OpenartDialog') != -1) {
                            isW = true;
                        }
                    }
                }


                if (isW == true) {
                    //固定部分
                    $(l_DataTable).tabToTable(opts.tabid, function (tabIndexVal, tableSelVal) {
                        opts.selTabFun(tabIndexVal, tableSelVal);

                        if (opts.isReSetTabHeight) {
                            $.lumos.common.setTabHeight(opts.tabid, tabIndexVal);
                        }

                    }, opts.tabIsSelFirBox);
                    //固定部分
                }
                else {
                    var pfs = parent.frames;
                    for (var i = 0; i < pfs.length; i++) {
                        if (pfs[i] == window) {
                            var mainheight = $(pfs[i].document).contents().find("body").height();
                            var mainiframename = $(pfs[i]).attr("name");

                            // $("iframe[name=" + mainiframename + "]", parent.document).parent().parent().height(mainheight + 30);
                        }
                    }

                    $(l_DataTable).tabToTable(opts.tabid, function (tabIndexVal, tableSelVal) {
                        opts.selTabFun(tabIndexVal, tableSelVal);
                        // $.lumos.common.setTabHeight(opts.tabid, tabIndexVal);
                    }, opts.tabIsSelFirBox);
                }

            }
            else {

                //var pfs = parent.frames;
                //for (var i = 0; i < pfs.length; i++) {
                //    if (pfs[i] == window) {

                //        if ($(pfs[i]).attr("name") != "page_maincontent") {
                //            var mainheight = $(pfs[i].document).contents().find("body").height();
                //            var mainiframename = $(pfs[i]).attr("name");
                //            if (mainiframename.indexOf('tabIframe') > -1) {
                //                $("iframe[name=" + mainiframename + "]", parent.document).parent().parent().height(mainheight);
                //            }
                //        }
                //    }
                //}
            }
        }



        if (!opts.isUseTempData) {
            $.ajax({
                type: "post",
                url: opts.url,
                async: true,
                dataType: 'json',
                data: l_Data,
                beforeSend: function (XMLHttpRequest) {
                    $.lumos.common.loadingShow("正在加载数据...");
                },
                complete: function (XMLHttpRequest, textStatus) {
                    $.lumos.common.loadingHide();
                },
                success: function (d) {

                    if (!$.lumos.common.isPageError(d)) {
                        getData(d.data);

                        if (typeof d.message != undefined) {
                            opts.messageHandleFun(d.message)
                        }
                    }
                }
            });
        }
        else{
            getData(opts.tempData);
        }

    }

    $.fn.loadDataUl = function (opts) {
        //  url, pageIndex, dataParms, rowFun, tabid, selTabFun
        opts = $.extend({
            url: 'test.apsx', //获取数据URL路径
            pageIndex: 0, //当前页索引
            dataParms: 11, //查询数据条件
            rowFun: function () { return false; }, //行数据构造
            tabid: '', //tabId
            selTabFun: function () { return false; }, //
            tabIsSelFirBox: false,           //是否选择第一个tabBox
            rowDbClickFun: function () { return false; },
            rowClickFun: function () { return false; }
        }, opts || {});

        var l_DataTable = $(this); //当前绑定Table对象
        //判断是否pageIndex为数字类型,是就返回当前的PageIndex的索引
        if (typeof opts.pageIndex != "number") {
            opts.pageIndex = $(l_DataTable).data('cur_pg_index');
        }
        else {
            $(l_DataTable).data('cur_pg_index', opts.pageIndex);
        }
        var l_Data = "pageindex=" + opts.pageIndex + "&"; //查询条件参数
        if (typeof opts.dataParms != 'undefined') {
            l_Data += opts.dataParms;
        }
        var l_StrRows = ""; //行数据


        $.ajax({
            type: "post",
            url: opts.url,
            async: true,
            dataType: 'json',
            data: $.lumos.dataParmsCheck(l_Data),
            beforeSend: function (XMLHttpRequest) {
                $.lumos.common.loadingShow("正在加载数据...");

            },
            complete: function (XMLHttpRequest, textStatus) {
                $.lumos.common.loadingHide();
            },
            success: function (data) {
                if (!$.lumos.common.isPageError(data)) {
                    var l_DataTablePagerId = $(l_DataTable).attr("id") + "_pager";
                    if ($('#' + l_DataTablePagerId).length <= 0) {
                        $(l_DataTable).after("<div id='" + l_DataTablePagerId + "'></div>"); //追加分页
                        $(l_DataTable).data('cur_pg_id', l_DataTablePagerId);
                    }
                    else {
                        $('#' + l_DataTablePagerId).html('');
                        //  $(l_DataTable).after("<div id='" + l_DataTablePagerId + "'></div>"); //追加分页
                        // $(l_DataTable).data('cur_pg_id', l_DataTablePagerId);
                        //  l_DataTablePagerId = $(l_DataTable).data('cur_pg_id');
                    }
                    var l_DataTablePager = $('#' + l_DataTablePagerId);


                    $(l_DataTable).find("li").remove(); //删除所有行
                    var p_table = data;
                    $.each(p_table.Rows, function (p_index, p_row) {
                        l_StrRows += opts.rowFun(p_index, p_row); //加载行数据
                    });
                    $(l_DataTable).append(l_StrRows); //追加所有行



                    $(l_DataTablePager).show(); //页面显示

                    $(l_DataTablePager).pagination(p_table.TotalRecord, {
                        callback: function (page_id, jq) { $(l_DataTable).loadDataUl({ url: opts.url, pageIndex: page_id, dataParms: opts.dataParms, rowFun: opts.rowFun, tabid: opts.tabid, selTabFun: opts.selTabFun }); },
                        items_num: p_table.TotalRecord,
                        items_per_page: p_table.PageSize,
                        current_page: opts.pageIndex,
                        onSelClick: function (index) { $(l_DataTable).loadDataUl({ url: opts.url, pageIndex: index - 1, dataParms: opts.dataParms, rowFun: opts.rowFun, tabid: opts.tabid, selTabFun: opts.selTabFun }); }
                    });

                    if (opts.tabid != '') {
                        $('#' + opts.tabid).hide();

                        //以下只作为出现选择行时出现的条件,如果不需要 可以删除其留下
                        var pfs = window.parent.frames;
                        var isW = false;

                        for (var i = 0; i < pfs.length; i++) {
                            if (pfs[i] == window) {
                                if ($(pfs[i]).attr("name") == "page_maincontent" || $(pfs[i]).attr("name").indexOf('OpenartDialog') != -1) {
                                    isW = true;
                                }
                            }
                        }
                        if (isW == true) {
                            //固定部分
                            $(l_DataTable).tabToTable(opts.tabid, function (tabIndexVal, tableSelVal) {
                                opts.selTabFun(tabIndexVal, tableSelVal);

                                $.lumos.common.setTabHeight(opts.tabid, tabIndexVal);
                            }, opts.tabIsSelFirBox);
                            //固定部分
                        }
                        else {
                            var pfs = parent.frames;
                            for (var i = 0; i < pfs.length; i++) {
                                if (pfs[i] == window) {
                                    var mainheight = $(pfs[i].document).contents().find("body").height();
                                    var mainiframename = $(pfs[i]).attr("name");

                                    $("iframe[name=" + mainiframename + "]", parent.document).parent().parent().height(mainheight + 30);
                                }
                            }
                        }

                    }
                    else {
                        var pfs = parent.frames;
                        for (var i = 0; i < pfs.length; i++) {
                            if (pfs[i] == window) {
                                if ($(pfs[i]).attr("name") != "page_maincontent") {
                                    var mainheight = $(pfs[i].document).contents().find("body").height();
                                    var mainiframename = $(pfs[i]).attr("name");
                                    if (mainiframename.indexOf('tabIframe') > -1) {
                                        $("iframe[name=" + mainiframename + "]", parent.document).parent().parent().height(mainheight);
                                    }
                                }
                            }
                        }
                    }

                }
            }
        });

    }

    $.fn.tabToTree = function (opts) {
        opts = $.extend({
            tabid: '', //tabId
            selTabFun: function () { return false; }, //
            noSelectTip: "请在左边树形列表中选择节点！", //当没有在选择
            tabIsSelFirBox: false           //是否选择第一个tabBox
        }, opts || {});
        var l_DataTree = $(this); //当前绑定Tree对象
        var l_DataTreeId = $(l_DataTree).attr("id");
        var l_Tab = $("#" + opts.tabid);
        if (opts.tabIsSelFirBox) {
            $(l_Tab).find('.tab-content > .content').eq(0).show();
        }

        $("#" + opts.tabid + " .tab-item  > ul > .item a").unbind("click").click(function () {

            var tableSelVal = "";
            var treeSelObj = $.fn.zTree.getZTreeObj($(l_DataTree).attr("id")).getSelectedNodes();
            if (typeof treeSelObj[0] == "undefined") {

                opts.selTabFun(0, tableSelVal);

                art.dialog.alert(opts.noSelectTip);
                return;

            }
            else {
                tableSelVal = treeSelObj[0].id;
                var curSelIndex = $(this).parent().index();
                $("#" + l_DataTreeId).data("CurSelTabIndex", curSelIndex);
                $("#" + l_DataTreeId).data("TabId", opts.tabid);
                var tabLength = $("#" + opts.tabid).find('.tab-content > .content').length;
                var curSelTabox;
                for (var i = 0; i < tabLength; i++) {
                    if (curSelIndex == i) {
                        $("#" + opts.tabid).find(' .tab-item ul > .item').eq(i).attr('class', 'item selected');
                        curSelTabox = $("#" + opts.tabid).find('.tab-content >.content').eq(i);
                        $(curSelTabox).show();
                    }
                    else {
                        $("#" + opts.tabid).find('.tab-content >.content').eq(i).hide();
                        $("#" + opts.tabid).find('.tab-item ul >.item').eq(i).attr('class', 'item');
                    }

                }
                var tabIndexVal = curSelIndex;
                opts.selTabFun(tabIndexVal, tableSelVal);
            }


        });
    }

    $.fn.tabToTreeGetCurBox = function () {
        var l_DataTree = $(this); //当前绑定Tree对象
        var l_DataTreeID = $(l_DataTree).attr("id");
        var curSelTabox = 0;
        if (typeof $("#" + l_DataTreeID).data("CurSelTabIndex") != "undefined") {
            curSelTabox = $("#" + l_DataTreeID).data("CurSelTabIndex");
        }
        if (typeof $("#" + l_DataTreeID).data("TabId") != "undefined") {
            $("#" + $("#" + l_DataTreeID).data("TabId")).find('.tab-item ul>.item').eq(curSelTabox).find("a").click();
        }
    }


    $.fn.initNoticeBox = function () {
        var l_this = $(this);
        var inObjectDiv = $(this).find('.inObject div');
        var inObject = $(this).find('.inObject');
        var inputObject = $(this).find('.inputObject');
        inputObject.attr('maxlength', '11');//限制文本框11位

        var errtext = $('#errtext');

        $(this).find('#listSendPerson a').click(function () {

            var objid = $(this).text() + '<' + $(this).attr('value') + '>;';
            var objed = false;

            $('.inObject div').each(function () {
                if ($(this).text() == objid) {
                    objed = true;
                }
            });

            if (objed == false) {
                $('.inObject div').each(function () {
                    if ($(this).attr('clicked') == '1') {
                        //removeClicked($(this));
                    }
                });
                $(inObject).append('<div value=' + $(this).attr('value') + ' type="0" key=' + $(this).attr('key') + '>' + $(this).text() + '<span>&lt;' + $(this).attr('value') + '&gt;</span>;</div>');

                var newObj = $(inObject).find('div:last');

                var zTree = $.fn.zTree.getZTreeObj("treemenu");
                if (zTree != null) {
                    var node = zTree.getNodeByParam("id", $(this).attr('key'), null);
                    zTree.checkNode(node, true, true, true);
                }

                //alert(node.id)
                //while (newObj.height() > 20) { //IE中存在bug，浮动不足宽度自动换行，就判断，如果高度大于20，就宽度加5，持续到不换行
                //    newObj.css('width', newObj.width() + 5)
                //}
            } else {
                showMsg('已经添加！');
            }
        });

        function inputWidthSet() {
            var theLast = $(inObject).find('div:last');
            var last1 = theLast.width();
            var last2 = theLast.prev().width();
            var last3 = theLast.prev().prev().width();
            var outDivWidth = $(l_this).find('.outObject').width();
            var outDivHeight = $(l_this).find('.outObject').height();
            alert(outDivHeight)
            if (outDivWidth < (last3 + last2 + last1)) { }
            if (outDivWidth > (last1 + last2 + 80)) {
                inputObject.css('width', outDivWidth - last1 - last2 - 22);
            } else {

            }
        }

        ////live委托鼠标over
        inObjectDiv.live("mouseover", function () {
            if ($(this).attr('clicked') != '1') {
                if ($(this).attr('editing') != '1') {
                    $(this).css('backgroundColor', '#B8D8F8');
                };
            };
        });

        //live委托鼠标out
        inObjectDiv.live("mouseout", function () {
            if ($(this).attr('clicked') != '1') {
                if ($(this).attr('editing') != '1') {
                    $(this).css('backgroundColor', '#fff');
                }
            };
        });

        //定义2个function座位增加点击与取消点击的作用，可以调用。
        function addClicked(obj) {
            obj.css('backgroundColor', '#528bcb');
            var textObj = obj.text();

            obj.css('color', '#fff');

            obj.find('span').css('color', '#fff');
            obj.attr('clicked', '1');
        }

        function removeClicked(obj) {
            obj.css('backgroundColor', '#fff');
            var textObj = obj.text();

            obj.css('color', '#000');

            obj.find('span').css('color', '#999');
            obj.removeAttr('clicked');
            return false;
        }

        //live委托鼠标点击，如果别的已经被点击过，就取消前一个变色与属性clicked。
        inObjectDiv.live("click", function () {

            if ($(this).attr('clicked') != '1') {
                $('#inObject div').each(function () {
                    if ($(this).attr('clicked') == '1') {
                        removeClicked($(this));
                    }
                });
            }
            addClicked($(this));

        });

        //live委托键盘退格与del，删除div。
        inObjectDiv.live("keydown", function (e) {
            if (e.keyCode == 8 || e.keyCode == 46) {

                $('.inObject div').each(function () {
                    if ($(this).attr('clicked') == '1') {

                        //$("#inputObject").insetAfter($(this));
                        $(this).remove();
                        return false;
                    };
                });
            }
            return false;
        });

        //输出错误信息
        function showMsg(msg) {
            errtext.text(msg);
        }

        //分析字符串并拆解出号码，返回值.
        String.prototype.numStr = function () {
            return this.substring(this.indexOf('<') + 1, this.indexOf('>'));
        }

        //分析字符串并拆解出<前的字符，返回值.
        String.prototype.textStr = function () {
            return this.substring(0, this.indexOf('<'));
        }

        //判断是否是手机正则式
        String.prototype.isMobile = function () {
            return (this.length == 11) && /^((\(\d{3}\))|(\d{3}\-))?13[0-9]\d{8}|15[689]\d{8}|18[689]\d{8}/.test(this);
        }

        //检查是否是手机与长度是否是11位。
        function checkLength(obj) {
            return obj.numStr().isMobile();
        }

        //监视输入
        inputObject.live("keydown", function (e) {
            if ((e.keyCode == 32) || (e.keyCode == 13)) {
                tohtml(inputObject);
                return false;
            }
            else if (e.keyCode == 8) {
                if ($(this).attr("type") == "text") {
                    if ($(this).val() == "") {


                        if ($(this).prev().find("div:last").attr('clicked') == '1') {

                            $(this).prev().find("div:last").remove();
                        }
                        else {
                            $(this).prev().find("div:last").attr('clicked', "1");
                            addClicked($(this).prev().find("div:last"))
                        }
                        //$(this).prev().find("div:last").click();

                    }
                }

            }
            else if ((e.keyCode > 32 && e.keyCode < 48) || (e.keyCode > 57 && e.keyCode < 65) || (e.keyCode > 90 && e.keyCode < 97)) {
                showMsg('非数字，请输入数字！');
                return false;
            }
        });

        //当input失去焦点，判断是否正确后生成与产生错误并提示。
        inputObject.blur(function () {
            tohtml(inputObject);
        });

        //intohtml
        function tohtml(obj) {
            var inputtext = obj.attr('value');
            var textValue = obj.attr('textValue');
            var newObj = "";
            if (inputtext != "" && inputtext != null) { //防止空提交
                if (textValue == null) {
                    textValue = inputtext;
                    addText = '<div>' + textValue + '<span><' + inputtext + '></span>;</div>'
                    $(inObject).append(addText);
                    newObj = $(inObject).find('div:last');
                    showMsg('新增成功！');
                } else if (!isNaN(textValue)) { //判断是否是输入手机号形成的div,判断是否为数字。
                    newObj = obj.parent();
                    newObj.html(inputtext + '<span><' + inputtext + '></span>;');
                    obj.remove();
                    showMsg('编写后的编辑成功！');
                } else {
                    newObj = obj.parent();
                    newObj.html(textValue + '<span><' + inputtext + '></span>;');
                    obj.remove();
                    showMsg('修改成功！');
                }

                //if (inputtext.isMobile()) { //判断是否是手机加颜色。
                //    newObj.css('color', '#000');
                //} else {
                //    newObj.css('color', '#f00');
                //}

                //while (newObj.height() > 20) { //IE中存在bug，浮动不足宽度自动换行，就判断，如果高度大于20，就宽度加5，持续到不换行
                //    newObj.css('width', newObj.width() + 5)
                //}
            } else {
                if (textValue != null) {
                    obj.parent().remove();
                    showMsg('空值！删除');
                }
            }
            inputObject.attr('value', '');
        }

        //双击事件。
        //inObject.live("dblclick", function () {
        //    var thisObj = $(this);
        //    var thisWidth = thisObj.width();
        //    var text = thisObj.text();//提前取出内容，以为esc效果
        //    var numStr = text.numStr();//分析出电话
        //    var textStr = text.textStr();//分析出名称
        //    thisObj.removeAttr('clicked');//取消点击

        //    thisObj.css('backgroundColor', '#fff');//取消背景色
        //    if (thisObj.children("input").length > 0) {
        //        return false;//取消再次双击，防止为空
        //    };
        //    thisObj.empty();//清空内容

        //    var inputObj = $("<input type='text' onpaste='return false' maxlength='11'>").addClass("appendtext").attr("textValue", textStr).css('width', thisWidth).val(numStr).appendTo(thisObj);

        //    inputObj.trigger("focus").trigger("select");//获得焦点并全选

        //    inputObj.click(function () {
        //        return false;
        //    });

        //    inputObj.keydown(function (e) {
        //        //esc
        //        if (e.keyCode == 27) {
        //            thisObj.html(text);
        //            addClicked(thisObj);
        //            thisObj.removeAttr('editing');
        //        } else {
        //            if ((e.keyCode == 32) || (e.keyCode == 13)) {
        //                tohtml(inputObj);
        //                return false;
        //            } else if ((e.keyCode < 48 && e.keyCode != 8 && e.keyCode != 27 && e.keyCode != 46 && e.keyCode != 37 && e.keyCode != 39) || (e.keyCode > 57)) {
        //                showMsg('非数字，请输入数字！');
        //                return false;
        //            }
        //        }


        //    });

        //    inputObj.blur(function () {
        //        tohtml(inputObj);
        //    })

        //});

    }

    $.fn.setDisabled = function (disabled) {
        $(this).attr("disabled", disabled);

        if (disabled) {
            $(this).addClass("btn-disabled");
        }
        else {
            $(this).removeClass("btn-disabled");
        }
    }


})(jQuery);


var cont = null;

$(document).ready(function () {


    $(".onlyready").keydown(function () {

        if (window.event.keyCode == 8) {
            return;
            window.event.keyCode = 0;
        }
    });

    $('#btn_Cancle').click(function () {

        art.dialog.close();
    });

    $("input[readOnly]").keydown(function (e) {
        e.preventDefault();
    });

    if ($.browser.msie && ($.browser.version == "6.0") && !$.support.style) {
        $('.list_btnarea').find('.nav-button').css("margin-top", "-2px");
    }
    else if ($.browser.msie && ($.browser.version == "7.0") && !$.support.style) {
        $('.list_btnarea').find('.nav-button').css("margin-top", "-2px");
    }

    cont = $("#form1").attr("action")

    if (navigator.userAgent.toLowerCase().indexOf("chrome") != -1) {
        var selectors = $("input[clear=on]");
        for (var i = 0; i < selectors.length; i++) {

            var input = selectors[i];
            var inputName = selectors[i].name;
            var inputid = selectors[i].id;

            selectors[i].removeAttribute("name");
            selectors[i].removeAttribute("id");
            setTimeout(function () {
                input.setAttribute("name", inputName);
                input.setAttribute("id", inputid);
            }, 1)

        }
    }


  

    $(".open-bigimg").click(function () {
        var url = $(this).attr("src");

   
     //   window.top.art.dialog.open(url, { title: "100%", width: '100%', height: "100%" });
       
      //  var h = '<iframe name="" src="' + url + '" frameborder="0" style="border: 0px currentColor; border-image: none; width: 100%; height: 100%;" allowtransparency="true"></iframe>';
        window.top.art.dialog({
            isPicBox:true,
            id:"open-bigimg",
            padding: 0,
            top:"75px",
            title: '图片',
            width: '100%',
            height: '100%',
            content: '<img src="' + url + '">',
            lock: true
        });
    });


});



String.prototype.toDate = function () {

    if (this == "") {
        return "";
    }
    else {
        var val = this;
        var style = 'yyyy-MM-dd';
        if (navigator.userAgent.indexOf("Firefox") != -1) {
            var str = val.replaceAll("-", "/");
            str = str.replaceAll("T", " ");
            return new Date(str).format(style)
        }
        else if (navigator.userAgent.indexOf("Chrome") != -1) {
            var str = val.replaceAll("-", "/");
            str = str.replaceAll("T", " ");
            return new Date(str).format(style)
        }
        else if (navigator.userAgent.indexOf("Safari") != -1) {
            var str = val.replaceAll("-", "/");
            str = str.replaceAll("T", " ");
            return new Date(str).format(style)
        }
        else {
            var str = val.replaceAll("-", "/");
            return new Date(str).format(style)
        }
    }
}

String.prototype.replaceAll = function (oldStr, newStr) {
    return this.replace(new RegExp(oldStr, "gm"), newStr);
}


String.prototype.toDateTime = function (fmt) {
    var f = "yyyy-MM-dd hh:mm:ss";
    if (typeof fmt != "undefined") {
        f = fmt;
    }
    if (this == "") {
        return "";
    }
    else {
        var val = this;
        var style = f;
        var str = this.replaceAll("-", "/");
        if (navigator.userAgent.indexOf("Firefox") != -1) {
            var str = this.replaceAll("-", "/");
            return new Date(str).format(style)
        }
        else if (navigator.userAgent.indexOf("Chrome") != -1) {
            var str = val.replaceAll("-", "/");
            str = str.replaceAll("T", " ");
            return new Date(str).format(style)
        }
        else if (navigator.userAgent.indexOf("Safari") != -1) {
            var str = val.replaceAll("-", "/");
            str = str.replaceAll("T", " ");
            return new Date(str).format(style)
        }
        else {
            return new Date(str).format(style)
        }
    }
}

String.prototype.trim = function () {
    return this.replace(/^\s+|\s+$/g, "");
}

String.prototype.ltrim = function () {
    return this.replace(/^\s+/, "");
}

String.prototype.rtrim = function () {
    return this.replace(/\s+$/, "");
}

String.prototype.toBankAccountNo = function () {
    var s = this.replace(/\s/g, '').replace(/(\d{4})(?=\d)/g, "$1" + " ");
    return s;
}

Date.prototype.format = function (format) //author: meizz 
{
    var o = {
        "M+": this.getMonth() + 1, //month 
        "d+": this.getDate(),    //day 
        "h+": this.getHours() == 0 ? "23" : this.getHours() - 1,   //hour 
        "m+": this.getMinutes(), //minute 
        "s+": this.getSeconds(), //second 
        "q+": Math.floor((this.getMonth() + 3) / 3),  //quarter 
        "S": this.getMilliseconds() //millisecond 
    }

    if (navigator.userAgent.indexOf("Firefox") != -1 || navigator.userAgent.indexOf("Chrome") != -1 || navigator.userAgent.indexOf("Safari") != -1) {
        o = {
            "M+": this.getMonth() + 1, //month 
            "d+": this.getDate(),    //day 
            "h+": this.getHours(),   //hour 
            "m+": this.getMinutes(), //minute 
            "s+": this.getSeconds(), //second 
            "q+": Math.floor((this.getMonth() + 3) / 3),  //quarter 
            "S": this.getMilliseconds() //millisecond 
        }
    }







    if (/(y+)/.test(format)) format = format.replace(RegExp.$1,
      (this.getFullYear() + "").substr(4 - RegExp.$1.length));



    for (var k in o) {
        if (new RegExp("(" + k + ")").test(format)) {
            format = format.replace(RegExp.$1,
              RegExp.$1.length == 1 ? o[k] :
                ("00" + o[k]).substr(("" + o[k]).length));
        }
    }

    return format;
}




Date.prototype.addDays = function (d) {

    this.setDate(this.getDate() + parseInt(d));


};

Date.prototype.addWeeks = function (w) {
    this.addDays(w * 7);
};

Date.prototype.addMonths = function (m) {
    var d = this.getDate();
    var m1 = this.getMonth() + parseInt(m);
    this.setMonth(m1);
    if (this.getDate() < d)
        this.setDate(0);
};

Date.prototype.addYears = function (y) {
    var m = this.getMonth();
    this.setFullYear(this.getFullYear() + parseInt(y));

    if (m < this.getMonth()) {
        this.setDate(0);
    }
};
//测试 var now = new Date(); now.addDays(1);//加减日期操作 alert(now.Format("yyyy-MM-dd"));

Date.prototype.dateDiff = function (interval, endTime) {
    switch (interval) {
        case "s":   //計算秒差                        
            return parseInt((endTime - this) / 1000);
        case "n":   //計算分差                    
            return parseInt((endTime - this) / 60000);
        case "h":   //計算時差                        
            return parseInt((endTime - this) / 3600000);
        case "d":   //計算日差             
            return parseInt((endTime - this) / 86400000);
        case "w":   //計算週差            
            return parseInt((endTime - this) / (86400000 * 7));
        case "m":   //計算月差                 
            return (endTime.getMonth() + 1) + ((endTime.getFullYear() - this.getFullYear()) * 12) - (this.getMonth() + 1);
        case "y":   //計算年差                       
            return endTime.getFullYear() - this.getFullYear();
        default:    //輸入有誤                      
            return undefined;
    }
}

var currentArtDialog = null;
window.onload = function () {

    if (document.getElementsByTagName) {
        var s = document.getElementsByTagName("select");
        if ($.browser.msie && ($.browser.version == "6.0") && !$.support.style) {
            if (s.length > 0) {
                window.select_current = new Array();

                for (var i = 0, select; select = s[i]; i++) {
                    select.onfocus = function () { window.select_current[this.id] = this.selectedIndex; }
                    select.onchange = function () { restore(this); }
                    emulate(select);
                }
            }
        }
    }

}

function currentArtDialogClose() {

    if (currentArtDialog != null) {
        currentArtDialog.close();
    }
}

function restore(e) {
    if (e.options[e.selectedIndex].disabled) {
        e.selectedIndex = window.select_current[e.id];
    }
}

function emulate(e) {
    for (var i = 0, option; option = e.options[i]; i++) {
        if (option.disabled) {
            option.style.color = "graytext";
        }
        else {
            option.style.color = "menutext";
        }
    }
}


//window.alert = function (message) {
//    art.dialog.alert(message)
//}

//window.confirm = function (txt) {
//    art.dialog({
//        title: '提示',
//        content: '<p>' + txt + '</p>',
//        ok: function () {
//            return true;
//        },
//        okVal: '确认',
//        cancelVal: '关闭',
//        cancel: true
//    });
//}

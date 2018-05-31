(function ($) {
    $.lumos = lumos = {
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

        htmlEncode: function (str) {
            var s = "";
            if (str.length == 0) return "";
            s = str.replace(/&/g, "&amp;");
            s = s.replace(/</g, "&lt;");
            s = s.replace(/>/g, "&gt;");
            s = s.replace(/ /g, "&nbsp;");
            s = s.replace(/\'/g, "&#39;");
            s = s.replace(/\"/g, "&quot;");
            s = s.replace(/\n/g, "<br>");
            return s;
        },

        htmlDecode: function html_decode(str) {
            var s = "";

            if (typeof str == 'undefined')
                return "";

            if (str == null)
                return "";

            if (str.length == 0)
                return "";

            s = str.replace(/&amp;/g, "&");
            s = s.replace(/&lt;/g, "<");
            s = s.replace(/&gt;/g, ">");
            s = s.replace(/&nbsp;/g, " ");
            s = s.replace(/&#39;/g, "\'");
            s = s.replace(/&quot;/g, "\"");
            s = s.replace(/<br>/g, "\n");
            return s;
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
        },

        isFloat: function (strVal) {
            if (strVal.toString() == "") return false;
            var chk = parseFloat(strVal);
            if (chk != strVal) {
                return false;
            }
            return true;
        },

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

        setDigitalFormat: function (o, num) {
            if (o == null)
                return "";


            if (!$.lumos.isFloat(o))
                return "";

            var o = Number(o);
            return o.toFixed(num);

        },

        boolConvert: function (o) {
            if (o == null)
                return "否";

            if (o == true) {
                return "是";
            }
            else {
                return "否";
            }
        },

        // To set it up as a global function:
        convertMoney: function (number, places, symbol, thousand, decimal) {

            number = number || 0;
            places = !isNaN(places = Math.abs(places)) ? places : 2;



            symbol = symbol !== undefined ? symbol : "";

            thousand = thousand || ",";
            decimal = decimal || ".";
            var negative = number < 0 ? "-" : "",
                i = parseInt(number = Math.abs(+number || 0).toFixed(places), 10) + "",
                j = (j = i.length) > 3 ? j % 3 : 0;



            var amount = negative + (j ? i.substr(0, j) + thousand : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + thousand) + (places ? decimal + Math.abs(number - i).toFixed(places).slice(2) : "");

            return symbol + " " + amount;
        },

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
                var index = -1;
                $.each(json, function (p_index, p_row) {
                    if (json[p_index][key] == value) {
                        index = p_index;
                    }
                });
                return json[index];
            }
        },

        currentTime: function () {
            var now = new Date();

            var year = now.getFullYear();       //年
            var month = now.getMonth() + 1;     //月
            var day = now.getDate();            //日

            var hh = now.getHours();            //时
            var mm = now.getMinutes();          //分
            var ss = now.getSeconds();          //分

            var clock = year + "-";

            if (month < 10)
                clock += "0";

            clock += month + "-";

            if (day < 10)
                clock += "0";

            clock += day + " ";

            if (hh < 10)
                clock += "0";

            clock += hh + ":";

            if (mm < 10) clock += '0';
            clock += mm + ":";

            if (ss < 10) clock += '0';
            clock += ss;

            return (clock);
        },

        parseFormArray: function (obj) {

            var type = Object.prototype.toString.call(obj);
            var array = new Array();

            if (type == "[object Array]") {
                $.each(obj, function (i, n) {
                    $.each(n, function (x, j) {
                        array.push({ name: x, value: j })
                    })
                });

            }
            else if (type == "[object Object]") {
                $.each(obj, function (x, j) {
                    array.push({ name: x, value: j })
                })
            }

            return array;

        },

        messageBox: function (opts) {
            opts = $.extend({
                type: 'tip',
                title: '标题',
                content: '内容',
                isPopup: true,
                showBoxId: '',
                padding: "0px 0px",
                errorStackTrace: "",
                isTop: false
            }, opts || {});



            var _type = "warn";

            if (opts.type == "1") {
                _type = "warn";
            }
            else if (opts.type == "2") {
                _type = "success";
            }
            else if (opts.type == "3") {
                _type = "failure";
            }
            else if (opts.type == "4") {
                _type = "exception";
            }

            var _title = opts.title;
            var _content = opts.content;
            var _isPopup = opts.isPopup;
            var _showBoxId = opts.showBoxId;
            var _padding = opts.padding;
            var _errorStackTrace = opts.errorStackTrace;
            var _isTop = opts.isTop;



            var errorHtml = '  <div class="messagebox" style="height:300px;">';
            errorHtml += ' <div class="wrapper">';
            errorHtml += '   <div class="content">';
            errorHtml += '     <dl>';
            errorHtml += '      <dt class="' + _type + '" ></dt>';
            errorHtml += '    <dd >';
            errorHtml += '     <h1>' + _title + '</h1>';
            errorHtml += '      <p>' + lumos.htmlDecode(_content) + '</p>';
            errorHtml += '     </dd>';
            errorHtml += '   </dl>';


            if (_errorStackTrace != "" && _errorStackTrace != "null") {
                errorHtml += '<div class=\"errorstacktrace\">';
                errorHtml += _errorStackTrace;
                errorHtml += '</div>';
            }


            errorHtml += '  <div class="clear"></div>';
            errorHtml += ' </div>';
            errorHtml += ' </div>';
            errorHtml += '</div>';

            if (_isPopup) {
                window.top.art.dialog({
                    content: errorHtml,
                    cancelVal: '关闭',
                    title: '提示',
                    width: "500px",
                    height: "100px",
                    cancel: true,
                    padding: _padding,
                    lock: true
                });
            }
            else {

                if (_isTop) {

                    $('body', window.top.document).html(errorHtml);

                    var list = window.top.art.dialog.list;
                    for (var i in list) {
                        list[i].close();
                    };

                    art.dialog.close();
                }
                else {

                    if (_showBoxId != '') {
                        $("#" + _showBoxId).html(errorHtml)
                    }
                    else {
                        var list = art.dialog.list;
                        for (var i in list) {
                            list[i].close();
                        };

                        if ($("#gbr_main_content_right").length > 0) {
                            $('#gbr_main_content_right').html(errorHtml)
                        }
                        else if ($("#gbr_main_content").length > 0) {
                            $('#gbr_main_content').html(errorHtml)
                        }
                        else {
                            $('body').html(errorHtml)
                        }

                    }
                }
            }
        },

        openDialog: function (url, title, iwidth, iheight) {
            var dialogSymbol = "dialogtitle=" + escape(title);
            if (url.indexOf('?') > -1) {
                url += "&"
            }
            else {
                url += "?"
            }
            url += dialogSymbol;
            art.dialog.open(url, { id: "openDialog", title: title, width: iwidth, height: iheight, lock: true });
            return false;
        },

        closeDialog: function (message) {
            setTimeout(function () {
                var list = window.top.art.dialog.list;
                for (var i in list) {
                    list[i].close();
                }
                art.dialog.close();

            }, 100);

            if (typeof message != 'undefined') {
                window.top.tips(message);
            }
            return false;
        },

        tips: function (message, isCloseWindows) {

            if (typeof isCloseWindows != 'undefined') {
                if (isCloseWindows) {
                    setTimeout(function () {
                        var list = window.top.art.dialog.list;
                        for (var i in list) {
                            list[i].close();
                        }

                        if (typeof art != 'undefined') {
                            art.dialog.close();
                        }

                    }, 100);
                }
            }

            window.top.tips(message);

            return false;
        },

        parentDialog: function () {
            return art.dialog.open.origin;
        },

        getUrlParamValue: function getUrlParam(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
            var r = window.location.search.substr(1).match(reg);  //匹配目标参数
            if (r != null) return unescape(r[2]); return null; //返回参数值

        },

        postJson: function (opts) {

            opts = $.extend({
                isUseHandling: false,
                url: '',
                data: null,
                async: true,
                timeout: 0,
                beforeSend: function (XMLHttpRequest) {
                },
                complete: function (XMLHttpRequest, status) {
                    if (status == 'timeout') {
                        art.dialog.tips("网络请求超时,请重新打开页面");
                    }
                    else if (status == 'error') {
                        art.dialog.tips("网络请求失败,请检查网络是否已连接");
                    }
                },
                success: function () { }
            }, opts || {});

            var _url = opts.url;

            if (_url == '') {
                art.dialog.tips("请求失败,链接为空");
                return;
            }




            var _data = opts.data;
            var _async = opts.async;
            var _timeout = opts.timeout;
            var _success = opts.success;
            var _beforeSend = opts.beforeSend;
            var _complete = opts.complete;
            var _isUseHandling = opts.isUseHandling

            //判断__RequestVerificationToken是否存在
            var tokeName = "__RequestVerificationToken";
            var token = $("input[name=" + tokeName + "]");
            var tokenValue = "";
            if ($(token).length > 0) {
                tokenValue = $($(token)[0]).val();
                if (_data != null) {
                    var isExist = false;
                    var type = Object.prototype.toString.call(_data);
                    if (type == "[object Array]") {


                        $.each(_data, function (i, n) {
                            $.each(n, function (x, j) {
                                if (j == tokeName) {
                                    isExist = true;
                                    return;
                                }
                            })
                        });
                        if (!isExist) {
                            _data.push({ name: tokeName, value: tokenValue });
                        }
                    }
                    else if (type == "[object Object]") {
                        $.each(_data, function (x, j) {

                            if (x == tokeName) {
                                isExist = true;
                                return;
                            }
                        })

                        if (!isExist) {
                            _data.__RequestVerificationToken = tokenValue
                        }
                    }
                }
            }

            var handling;


            $.ajax({
                type: "Post",
                dataType: "json",
                async: _async,
                timeout: _timeout,
                data: _data,
                url: _url,
                beforeSend: function (XMLHttpRequest) {
                    if (_isUseHandling) {
                        handling = artDialog.loading2("正在处理");
                    }
                    else {
                        _beforeSend(XMLHttpRequest);
                    }
                },
                complete: function (XMLHttpRequest, status) {
                    _complete(XMLHttpRequest, status);
                }
            }).done(function (d) {

                if (_isUseHandling) {
                    handling.close();
                }

                if (d.result == resultType.exception) {
                    var messsage = d.data;
                    $.lumos.messageBox({ type: messsage.type, title: messsage.title, content: messsage.content, isPopup: messsage.isPopup, errorStackTrace: messsage.errorStackTrace, isTop: messsage.isTop })
                }
                else {
                    _success(d);
                }
            });
        },

        ajaxSubmit: function (opts) {

            opts = $.extend({
                formId: "form1",
                url: '',
                success: function () { return false; }
            }, opts || {});

            var _formId = opts.formId;
            var _url = opts.url;
            var _success = opts.success;

            if (_url == '') {
                art.dialog.tips("请求失败,链接为空");
                return;
            }


            $('#' + _formId).ajaxSubmit({
                type: "post",  //提交方式
                dataType: "json",
                url: _url,
                success: function (d) {

                    if (d.result == resultType.exception) {
                        var messsage = d.data;
                        $.lumos.messageBox({ type: "exception", title: messsage.Title, content: messsage.Content, errorStackTrace: messsage.errorStackTrace })
                    }
                    else {
                        _success(d);
                    }
                }
            });

        },

        download: function (url, data, method) {
            // Ajax 文件下载jQuery.download = function(url, data, method){    // 获得url和data

            if (url) {
                // data 是 string 或者 array/object
                var inputs = '';

                if (data != '') {
                    data = typeof data == 'string' ? data : jQuery.param(data);        // 把参数组装成 form的  input

                    jQuery.each(data.split('&'), function () {
                        var pair = this.split('=');
                        inputs += '<input type="hidden" name="' + pair[0] + '" value="' + pair[1] + '" />';
                    });


                }

                // request发送请求
                jQuery('<form action="' + url + '" method="' + (method || 'post') + '">' + inputs + '</form>')
                .appendTo('body').submit().remove();
            };
        },

        getFileSize: function (eleId) {
            var size = 0;
            try {
                if ($.browser.msie) {//ie旧版浏览器
                    var fileMgr = new ActiveXObject("Scripting.FileSystemObject");
                    var filePath = $('#' + eleId)[0].value;
                    var fileObj = fileMgr.getFile(filePath);
                    size = fileObj.size; //byte
                    size = size / 1024;//kb
                    size = size / 1024;//mb
                } else {//其它浏览器
                    size = $('#' + eleId)[0].files[0].size;//byte
                    size = size / 1024;//kb
                    size = size / 1024;//mb
                }

            } catch (e) {
                size = -1;
            }

            return size;
        },

        getImageUrlBySize: function (url, size) {
            var file_name = url;
            if (file_name == '')
                return;

            var index = file_name.lastIndexOf('\\');
            if (index > 0) {
                index = index + 1;
            }
            file_name = file_name.substring(index, file_name.length);

            index = file_name.lastIndexOf('.');

            return file_name;
        }
    }



    $.fn.loadDataTable = function (opts) {

        opts = $.extend({
            emptyTip: "暂时没有数据",//空数据提示
            url: 'test.apsx',//获取数据的URL
            searchButtonId: "btn_Search",//查询按钮ID
            searchParams: null,//查询的的参数
            rowDataCombie: function () { },//行数据组合
            operate: null,//操作方法，以元素operate="delete"为属性 过滤
            containerId: 'form1',//表单的容器
            success: function (data) { },
            refreshInterval: 0,
            isShowLoading: true,
            useEmptyRow: false
        }, opts || {});

        var _thisTable = $(this); //当前table
        var _url = opts.url;//访问的地址
        var _searchParams = opts.searchParams;//查询条件
        var _emptyTip = opts.emptyTip;
        var _container = $("#" + opts.containerId);
        var _success = opts.success;
        var _searchButtonId = opts.searchButtonId;
        var _refreshInterval = opts.refreshInterval;
        var _isShowLoading = opts.isShowLoading;
        var _useEmptyRow = opts.useEmptyRow;

        if (_searchParams == null) {
            _searchParams = new Array();
        }

        //构造分页
        function getPagination(totalrecord, pagesize, pageindex) {
            var l_pageIndex = parseInt(pageindex);
            var l_totalRecord = parseInt(totalrecord);
            var l_pageSize = parseInt(pagesize);
            var l_pagination = "";
            var l_paginationInfo = "<div class=\"pagination-info\"><span> 共有" + l_totalRecord + "条，每页显示：" + l_pageSize + "条 </span></div>";
            l_pagination += l_paginationInfo;

            var l_pageCount = Math.ceil(l_totalRecord / l_pageSize)//页数



            var l_paginationIndex = "";
            l_paginationIndex += ' <div class="pagination-index">';
            l_paginationIndex += '<ul>';

            if (l_pageSize > 1 && pageindex != 0) {
                l_paginationIndex += '<li page-index="0"><a>«</a></li>';
            }
            else {
                l_paginationIndex += '<li class="disabled" page-index="0"><a>«</a></li>';

            }

            if (pageindex > 0) {
                l_paginationIndex += '<li  page-index="' + (pageindex - 1) + '"><a>‹</a></li>';
            }
            else {
                l_paginationIndex += '<li class="disabled" page-index="0"><a>‹</a></li>';
            }

            var l_spitIndex = l_pageIndex - 2;
            if (l_spitIndex > 4) {
                l_paginationIndex += '<li page-index="0"><a>1</a></li>';
                l_paginationIndex += '<li page-index="' + (l_spitIndex - 2) + '"><a>...</a></li>';
            }
            else {
                for (var i = 0; i < l_spitIndex; i++) {
                    l_paginationIndex += '<li page-index="' + i + '"><a>' + (i + 1) + '</a></li>';
                }


            }

            for (var i = l_pageIndex - 2; i < l_pageIndex; i++) {
                if (i >= l_pageIndex || i < 0) {
                    continue;
                }
                l_paginationIndex += '<li page-index="' + i + '"><a>' + (i + 1) + '</a> </li>';
            }


            l_paginationIndex += ' <li class="active" ><span>' + (l_pageIndex + 1) + '</span></li>';




            for (var i = l_pageIndex + 1; i < l_pageCount; i++) {
                if (i >= l_pageIndex + 3) {
                    break;
                }
                l_paginationIndex += ' <li  page-index="' + i + '"><a>' + (i + 1) + '</a> </li>';
            }

            l_spitIndex = l_pageIndex + 3;


            if (l_pageCount - 4 > l_spitIndex) {
                l_paginationIndex += ' <li page-index="' + (l_spitIndex + 2) + '"><a >...</a> </li>';
                l_paginationIndex += ' <li page-index="' + (l_pageCount - 1) + '"><a>' + l_pageCount + '</a> </li>';
            }
            else {
                for (var i = l_spitIndex; i < l_pageCount; i++) {
                    l_paginationIndex += '   <li page-index="' + (i) + '"><a>' + (i + 1) + '</a> </li>';
                }
            }



            if (l_pageIndex != l_pageCount - 1 && l_pageCount > 0) {
                l_paginationIndex += '<li page-index="' + (l_pageIndex + 1) + '"><a>›</a></li>';
            }
            else {

                l_paginationIndex += '<li class="disabled" page-index="0"><a>›</a></li>';
            }



            if (l_pageSize > 1 && (l_pageIndex != l_pageCount - 1) && l_pageCount > 0) {
                l_paginationIndex += '<li  page-index="' + (l_pageCount - 1) + '"><a>»</a></li>';
            }
            else {
                l_paginationIndex += '<li class="disabled" page-index="0"><a>»</a></li>';
            }





            l_paginationIndex += '</ul>';
            l_paginationIndex += '</div>';

            l_pagination += l_paginationIndex;


            var l_paginationGoto = "<div class=\"pagination-goto\">  <input class=\"input-control input-go\"   type=\"text\"/>  <input class=\"btn btn-go\" type=\"button\"  pagecount=\"" + l_pageCount + "\" value=\"跳转\" /> </div>";
            l_pagination += l_paginationGoto;


            return l_pagination;
        }


        function htmlEncode(str) {
            var s = "";
            if (str.length == 0) {
                return "";
            }
            s = str.replace(/</g, "&lt;");
            //s = s.replace(/&/g, "&amp;");
            s = s.replace(/>/g, "&gt;");
            s = s.replace(/\'/g, "&#39;");
            s = s.replace(/\"/g, "&quot;");
            s = s.replace(/\n/g, "<br>");
            return s;
        }


        //加载数据
        function getList(currentPageIndex, searchparams, isShowLoading) {


            $(_thisTable).data('currentPageIndex', currentPageIndex);

            $.each(searchparams, function (i, field) {

                if (field.name == "PageIndex") {
                    field.value = currentPageIndex
                }
                else if (field.name == "PageSize") {
                    field.value = 10;
                }
                else {

                    field.value = $(_container).find("*[name='" + field.name + "']").val();
                    // alert(field.value)
                }
            });

            // alert(currentPageIndex)
            var loading;
            var loadingtip = $(_thisTable).find('.load-tip');
            var l_StrRows = ""; //行数据


            $.lumos.postJson({
                type: "post",
                url: _url,
                async: true,
                dataType: 'json',
                data: _searchParams,
                beforeSend: function (XMLHttpRequest) {
                    if (isShowLoading) {
                        $(loadingtip).show();
                        $(loadingtip).text("正在加载...")
                        // loading = art.dialog.loading("正在加载", 120000);
                    }
                },
                complete: function (XMLHttpRequest, status) {
                    if (isShowLoading) {
                        $(loadingtip).hide();
                        // loading.close();
                    }
                    if (status == 'timeout') {
                        art.dialog.tips("网络请求超时,请重新打开页面");
                    }
                    else if (status == 'error') {
                        art.dialog.tips("网络请求失败,请检查网络是否已连接");

                        if ($(_thisTable).find("tbody tr").length == 0) {
                            var headLen = $(_thisTable).find("thead tr th").length;
                            $(_thisTable).find("tbody").append("<tr><td colspan=\"" + headLen + "\" class=\"emptytip\">" + _emptyTip + "</td></tr>");
                        }
                    }
                    else if (status == 'parsererror') {
                        art.dialog.tips("网络请求发生错误");
                    }
                    else if (status == 'success') {
                        // loading.close();
                    }
                },
                success: function (d) {
                    if (isShowLoading) {
                        // loading.close();
                    }

                    // setTimeout(function () { loading.close() }, 500);
                    var dataContent = d.data;

                    _success(dataContent);

                    var list = dataContent;
                    var list_Data = null;
                    if (typeof list.rows != "undefined") {
                        list_Data = dataContent.rows
                    }






                    var tr_body = $(_thisTable).find("tbody");

                    $(tr_body).find("tr").remove(); //删除所有行
                    $.each(list_Data, function (p_index, p_row) {


                        for (p_row_d in p_row) {

                            if (p_row[p_row_d] == null) {
                                p_row[p_row_d] = "";
                            }
                            else if (typeof p_row[p_row_d] == 'string') {
                                if (p_row[p_row_d].indexOf('htmldecode') <= -1) {
                                    p_row[p_row_d] = htmlEncode(p_row[p_row_d]);
                                }
                            }
                        }

                        var row = opts.rowDataCombie(p_index, p_row); //加载行数据

                        var objRow = $(row).appendTo(tr_body); //追加行到tbody

                        $(objRow).data("keyval", p_row);
                        $(objRow).find('.keyval').data("keyval", p_row);
                    });


                    // $(_thisTable).find("tbody").append(l_StrRows); //追加所有行

                    $(_thisTable).addTableStyle();//追加样式



                    $(_thisTable).find(".pagination").html("");
                    pagination = getPagination(list.totalRecord, list.pageSize, currentPageIndex);
                    $(_thisTable).find(".pagination").append(pagination);

                    //处理每行的checkbox
                    var td_ckb = $(_thisTable).find(" thead th ").eq(0).find("input[type=checkbox]")[0];
                    if ($(td_ckb).length > 0) {
                        $(td_ckb).attr("checked", false);
                        var td_ckb_child_name = $(td_ckb).attr("childname")
                        var tr = $(_thisTable).find("tbody tr");
                        for (var i = 0; i < tr.length; i++) {
                            var tr_td_0 = $(tr[i]).find("td").eq("0");
                            $(tr_td_0).html("<input type='checkbox' name='" + td_ckb_child_name + "' />" + $(tr_td_0).text() + "");
                        }

                    }


                    if (_useEmptyRow) {
                        if (dataContent.rows.length != dataContent.pageSize) {

                            var trlength = dataContent.pageSize - dataContent.rows.length;
                            var tdlength = $(_thisTable).find('thead tr th').length;

                            for (var i = 0; i < trlength; i++) {
                                var emptyRowHtml = "<tr>"

                                for (var j = 0; j < tdlength; j++) {
                                    emptyRowHtml += "<td></td>";
                                }
                                emptyRowHtml += "</tr>";
                                $(tr_body).append(emptyRowHtml)
                            }
                        }
                    }
                    else {

                        //空数据提示
                        if (list_Data.length == 0) {
                            var headLen = $(_thisTable).find("thead tr th").length;
                            $(_thisTable).find("tbody").append("<tr><td colspan=\"" + headLen + "\" class=\"emptytip\">" + _emptyTip + "</td></tr>");
                        }
                    }
                }


            });
        }

        _searchParams.push({ name: "PageSize", value: 10 });
        _searchParams.push({ name: "PageIndex", value: 0 });


        getList(0, _searchParams, _isShowLoading);

        if (_refreshInterval != 0) {
            setInterval(function () { getList(0, _searchParams, false) }, _refreshInterval);
        }




        //处理分页
        $(_thisTable).find(".pagination-index li").live("click", function () {
            var currentPageIndex = $(this).attr("page-index");
            var classname = $(this).attr("class");
            if (classname != "active" && classname != "disabled") {
                getList(currentPageIndex, _searchParams, true);
            }
        });

        $(_thisTable).find(".pagination-goto .btn-go").live("click", function () {
            var index = $(_thisTable).find(".pagination-goto .input-go").val();
            var pagecount = parseInt($(this).attr("pagecount"));
            var regexp = /^[1-9]\d*$/;
            if (!regexp.test(index)) {
                art.dialog.tips("请输入大于0的正整数");
                return;
            }

            if (index > pagecount) {
                art.dialog.tips("请重新输入,不能超过" + pagecount);
                return;
            }

            index = index - 1;

            getList(index, _searchParams, true);

        });

        //处理查询按钮
        $("#" + _searchButtonId).live("click", function () {
            _searchParams = opts.searchParams;

            getList(0, _searchParams, true);
        });

        //处理所在区域的操作
        if (opts.operate != null) {
            $.each(opts.operate, function (btnclassname, f) {
                switch (btnclassname) {
                    case "delete":
                        $(_container).find("*[operate=" + btnclassname + "]").live("click", function () {

                            var multiple = $(this).attr("multiple");

                            var del_titleIndex = $(_thisTable).find("tbody tr:first .tipitem").index();
                            var del_titleValue = $(_thisTable).find("thead tr th").eq(del_titleIndex).text().trim();
                            var del_tips = "确认要删除以下数据【" + del_titleValue + "】为<br/>";
                            var keys = new Array();//todo
                            if (typeof multiple == "undefined") {
                                del_tips += $(this).parent().parent().find(".tipitem").text().trim();
                                key = $(this).parent().parent().data("keyval");
                                keys.push(key);
                            }
                            else {

                                var tr_Checked = $(_thisTable).find(" tbody tr input[checked=checked]");

                                if ($(tr_Checked).length <= 0) {
                                    art.dialog.tips("请选择要删除的数据");
                                    return
                                }

                                $(tr_Checked).each(function () {
                                    del_tips += "" + $(this).parent().parent().find(".tipitem").text().trim() + "<br/>";
                                    key = $(this).parent().parent().data("keyval");
                                    keys.push(key);
                                });
                                // keys = keys[0]
                            }

                            art.dialog.confirm(del_tips, function () {
                                f(keys)
                                return true;
                            },
                            function () { keys = null; })
                        });
                        break;
                    case "select":
                        $(_container).find("*[operate=" + btnclassname + "]").live("click", function () {

                            var multiple = $(this).attr("multiple");

                            var del_titleIndex = $(_thisTable).find("tbody tr:first .tipitem").index();
                            var del_titleValue = $(_thisTable).find("thead tr th").eq(del_titleIndex).text().trim();
                            var del_tips = "确认要选择以下数据【" + del_titleValue + "】为<br/>";
                            var keys = new Array();//todo
                            if (typeof multiple == "undefined") {
                                del_tips += $(this).parent().parent().find(".tipitem").text().trim();
                                var key = $(this).parent().parent().data("keyval");
                                keys.push(key);
                            }
                            else {

                                var tr_Checked = $(_thisTable).find(" tbody tr input[checked=checked]");

                                if ($(tr_Checked).length <= 0) {
                                    art.dialog.tips("请选择数据");
                                    return
                                }
                                $(tr_Checked).each(function () {
                                    del_tips += "" + $(this).parent().parent().find(".tipitem").text().trim() + "<br/>";
                                    key = $(this).parent().parent().data("keyval");
                                    keys.push(key);
                                });
                            }

                            art.dialog.confirm(del_tips, function () {
                                f(keys)
                                return true;
                            },
                            function () { keys = ""; })
                        });
                        break;
                    default:
                        $(_container).find("*[operate=" + btnclassname + "]").live("click", function () {

                            var keyval = $(this).parent().parent().data("keyval");

                            if ($(this).hasClass('keyval')) {
                                keyval = $(this).data("keyval");
                            }
                            f(keyval);
                        });
                        break;
                }
            });
        }

        this.loadData = function (index) {
            var pageIndex = $(_thisTable).data('currentPageIndex');
            if (typeof index != 'undefined') {
                pageIndex = index;
            }
            getList(pageIndex, _searchParams, true);

        }

        return this;

    }


    $.fn.initUploadImage = function (options) {
        var defaults = {
            url: "/Common/UploadImage",//调用的后台方法
            success: function (data) { }
        };
        var opts = $.extend({}, defaults, options);

        var _this = $(this);
        var _url = opts.url;
        var _success = opts.success

        var thisUpload = $(this);
        var inputName = $(thisUpload).attr("inputname");
        if (typeof inputName == undefined) {
            inputName = "";
        }

        $(".uploadImageForm").remove();

        var from_FileName = inputName + "_file"
        var formTemplate =
            '<form class=\"uploadImageForm\" enctype="multipart/form-data" style="position:absolute;z-index: 0;top:-20px;" >' +
            '<input type="hidden" name="valueinputname" value="' + inputName + '" />' +
            '<input type="hidden" name="fileinputname" value="' + from_FileName + '" />' +
            '<input type="hidden" name="path" value="" />' +
            '<input type="hidden" name="generatesize" value="true" />' +
            '<input type="file"  name="' + from_FileName + '" accept="image/gif, image/jpeg, image/png, image/x-ms-bmp, image/bmp"  style="left: 0px; top: 0px; width: 45px; height: 25px; cursor: pointer; z-index: 2;filter: alpha(opacity: 0); position: relative;text-align: left;opacity: 0; moz-opacity: 0;" /></form>';
        var form = $(formTemplate);

        $(form).data("comefrom", thisUpload)

        $('body').append(form);

        $('.uploadimgbox[upload=true]').live("mouseenter", function () {
            $(".uploadImageForm").show();

            var thisUpload = $(this);
            var left = $(this).offset().left;
            var top = $(this).offset().top;

            var width = $(this).width();
            var height = $(this).height();

            var btn_width = 35 / 2;
            var btn_height = 20 / 2;

            var f_left = width / 2 + left - btn_width;

            var f_top = height / 2 + top - btn_height;

            $(form).show();
            $(form).css("left", f_left);
            $(form).css("top", f_top);

            $(form).data("comefrom", thisUpload);
            // $(".uploadImageForm").find('input[type="file"]').show();
        });

        $(".uploadImageForm").find('input[type="file"]').live("mouseout", function () {
            $(".uploadImageForm").hide();
            //  $(".uploadImageForm").find('input[type="file"]').hide();
        });

        $(".uploadImageForm").find("input[type=file]").live("change", function () {

            var _this = $(this);
            var form = $(this).parent();
            var comefrom = $(form).data("comefrom");

            var _path = 'unknow';
            var _generatesize = "true";

            if (typeof $(comefrom).find('.uploadimg').attr('path') != 'undefined') {
                _path = $(comefrom).find('.uploadimg').attr('path');
            }

            if (typeof $(comefrom).find('.uploadimg').attr('generatesize') != 'undefined') {
                _path = $(comefrom).find('.uploadimg').attr('generatesize');
            }

            $(form).find('input[name=path]').val(_path);
            $(form).find('input[name=generatesize]').val(_generatesize);

            var fileName = $(this).val();//文件名


            if (fileName == '') {
                return;
            }

            if (fileName != "") {
                var extStart = fileName.lastIndexOf(".");
                var ext = fileName.substring(extStart, fileName.length).toUpperCase();
                if (ext != ".BMP" && ext != ".PNG" && ext != ".GIF" && ext != ".JPG" && ext != ".JPEG") {
                    art.dialog.tips('您上传的图片格式(.jpg|.jpeg|.gif|.png|.bmp)不正确，请重新选择！');
                    return;
                }
            }

            var size = 0;
            try {
                if ($.browser.msie) {//ie旧版浏览器
                    var fileMgr = new ActiveXObject("Scripting.FileSystemObject");
                    var filePath = $(_this)[0].value;
                    var fileObj = fileMgr.getFile(filePath);
                    size = fileObj.size; //byte
                    size = size / 1024;//kb
                    size = size / 1024;//mb
                } else {//其它浏览器
                    size = $(_this)[0].files[0].size;//byte
                    size = size / 1024;//kb
                    size = size / 1024;//mb
                }

            } catch (e) {
                size = -1;
            }


            if (size != -1) {
                if (size == 0) {
                    var file = $(_this)
                    file.after(file.clone().val(""));
                    file.remove();
                    art.dialog.tips('文件不存在或者文件的内容为空,请重新选择');
                    return;
                }
                else if (size > 10) {
                    var file = $(_this)
                    file.after(file.clone().val(""));
                    file.remove();
                    art.dialog.tips('图片大小不能超过10M,请重新选择');
                    return;
                }
            }



            function getNowFormatDate() {

                var date = new Date();

                var seperator1 = "-";

                var seperator2 = ":";

                var month = date.getMonth() + 1;

                var strDate = date.getDate();

                if (month >= 1 && month <= 9) {

                    month = "0" + month;

                }

                if (strDate >= 0 && strDate <= 9) {

                    strDate = "0" + strDate;

                }

                var currentdate = date.getFullYear() + seperator1 + month + seperator1 + strDate

                        + " " + date.getHours() + seperator2 + date.getMinutes()

                        + seperator2 + date.getSeconds();

                return currentdate;

            }


            var p_d = art.dialog({
                title: '图片上传',
                content: '正在上传图片...请稍后！',
                cancel: false,
                lock: true,
                drag: false,
                dblclickClose: false
            });

            $(form).ajaxSubmit({
                type: "post",
                url: _url + "?date=" + getNowFormatDate(),
                dataType: "json",
                success: function (d) {

                    p_d.close();
                    var file = $(_this)
                    file.after(file.clone().val(""));
                    file.remove();

                    if (d.result == resultType.success) {

                        var imgObject = d.data;


                        if (imgObject.originalPath.length > 0) {

                            if (imgObject.smallPath != null) {

                                $(comefrom).find('img').attr("src", imgObject.smallPath);
                            }
                            else {
                                $(comefrom).find('img').attr("src", imgObject.originalPath);
                            }

                            $(comefrom).find('img').attr("bigsrc", imgObject.originalPath);
                            $($(comefrom).find("input[type=hidden]")[0]).val(imgObject.originalPath);//上传成功后把新文件名保存到hidden控件中

                            _success(d);
                        }
                    }
                    else {
                        art.dialog.tips(d.message)
                    }
                }
            });

        });

        $(".uploadimg-view").live('click', function () {
            var img = $(this).parent().find('img');
            var url = $(img).attr("src");

            if (typeof url == 'undefined') {
                url = "";
            }

            if (url == "") {
                art.dialog.tips("没有图片可以预览");
                return;
            }

            if (typeof $(img).attr("bigsrc") != 'undefined') {
                url = $(img).attr("bigsrc");
            }

            window.top.art.dialog({
                isPicBox: true,
                id: "open-bigimg",
                padding: 0,
                title: '查看图片',
                width: '200px',
                height: '300px',
                content: '<div style="width:800px;height:600px;overflow:scroll"> <img src="' + url + '" style="width:100%;height:100%"> </div>',
                lock: true
            });
        });

        $(".uploadimg-delete").live('click', function () {

            //var _this = $(this);



            //art.dialog.confirm("确定要删除该图片？", function () {

            //    $(_this).parent().find('img').attr("src", "");
            //    $(_this).parent().find('input[type=hidden]').val("");

            //    return true;
            //});

        });

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

        function formatNumber(value, num) {
            var a, b, c, i;
            a = value.toString();
            b = a.indexOf(".");
            c = a.length;
            if (num == 0) {
                if (b != -1) {
                    a = a.substring(0, b);
                }
            } else {//如果没有小数点
                if (b == -1) {
                    a = a + ".";
                    for (i = 1; i <= num; i++) {
                        a = a + "0";
                    }
                } else {//有小数点，超出位数自动截取，否则补0
                    a = a.substring(0, b + num + 1);
                    for (i = c; i <= b + num; i++) {
                        a = a + "0";
                    }
                }
            }

            if (b == 0) {
                a = "0" + a;
            }

            return a;
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
                    if (pos > dotPos + num) return false;
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
            this.value = formatNumber(this.value, num);
            $(this).trigger("input");
        });
        this.bind("paste", function () {
            if (typeof window.clipboardData != undefined) {
                var s = clipboardData.getData('text');
                if (!isNaN(s)) {
                    value = formatNumber(s, num);
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

    $.fn.addTableStyle = function () {

        var _this = $(this)

        if (typeof $(_this).get(0) != "undefined") {
            var tagName = $(_this).get(0).tagName
            if (tagName == "TABLE") {

                //-表格隔行,滑动,点击 变色
                $(this).find(' tbody tr:even ').addClass('trold');
                $(this).find(' tbody tr ').hover(
                          function () { $(this).addClass('trmouseover'); },
                         function () {
                             $(this).removeClass('trmouseover');
                         });


            }
        }
    }

    $.fn.openDialog = function (url, title, iwidth, iheight) {

        var _this = $(this);
        $(_this).on("click", function () {
            lumos.openDialog(url, title, iwidth, iheight);
            return false;
        })
    }

    $.fn.multiSelect = function () {

        var _this = $(this);
        var _childname = $(_this).attr("childname");
        $(_this).live('click', function () {

            var isCheck = $(this).attr("checked");
            if (typeof isCheck == "undefined") {
                $('input[name=' + _childname + ']').removeAttr("checked");
            }
            else {
                $('input[name=' + _childname + ']').attr("checked", "checked");
            }

        });


        $('input[name=' + _childname + ']').live("click", function () {


            var isCheck = $(this).attr("checked");
            if (typeof isCheck == "undefined") {
                $(this).removeAttr("checked");
            }
            else {
                $(this).attr("checked", "checked");
            }
            var ckbLen = $('input[name=' + _childname + ']').length;
            var ckbCheckedLen = $('input[name=' + _childname + '][checked=checked]').length;
            if (ckbLen == ckbCheckedLen) {
                $(_this).attr("checked", "checked");
            }
            else {
                $(_this).removeAttr("checked");
            }



        });


    }

    $.fn.tab = function (opts) {


        opts = $.extend({
            click: function (index) { return true; }
        }, opts || {});

        var _click = opts.click;



        var _tabs = $(this);

        $(_tabs).each(function () {

            var _tab = $(this);
            var _tabitems = $(_tab).find(".item");
            var _tabcontents = $(_tab).find(".tab-content .subcontent");

            function showitem(index) {
                $(_tabitems).removeClass("selected");
                $(_tabitems).eq(index).addClass("selected");
                $(_tabcontents).hide();
                $(_tabcontents).eq(index).show();
                $(_tabs).attr("index", index);
            }

            $(_tabitems).unbind('click').click(function () {

                var index = $(this).index();
                var isflag = _click(index);
                if (!isflag)
                    return
                var disabled = $(this).hasClass("disabled");
                if (!disabled) {
                    showitem(index);
                }
            });

            $(_tabitems).each(function () {

                var isflag = $(this).hasClass("selected");
                if (isflag) {
                    var index = $(this).index();
                    showitem(index);
                }
            });




        });

        this.click = function () {
            var index = parseInt($(_tabs).attr("index"));
            _click(index);
        };

        return this;

    }

})(jQuery);


$(document).ready(function () {

    $(".open-bigimg").live('click', function () {
        var url = $(this).attr("src");

        if (typeof (url) == "undefined" || url == "") {
            return;
        }

        window.top.art.dialog({
            isPicBox: true,
            id: "open-bigimg",
            padding: 0,
            title: '查看图片',
            width: '200px',
            height: '300px',
            content: '<div style="width:800px;height:600px;overflow:scroll"> <img src="' + url + '" style="width:100%;height:100%"> </div>',
            lock: true
        });
    });

    $(".open-targetimg").live('click', function () {
        var url = $(this).attr("src");

        if (typeof (url) == "undefined" || url == "") {
            return;
        }
        window.open(url);
    });

    $('.markupload-file').live('change', function () {
        var file_name = $(this).val();

        if (file_name == '')
            return;

        var index = file_name.lastIndexOf('\\');
        if (index > 0) {
            index = index + 1;
        }
        file_name = file_name.substring(index, file_name.length);

        $(this).parent().parent().find('.file-name').text(file_name);
    });

    $("#btn_close,.btn-close,.dialog-close").on("click", function () {
        $.lumos.closeDialog();
    });


});


var operateType = {
    add: "1",
    update: "2",
    del: "3",
    save: "4",
    submit: "5",
    pass: "6",
    reject: "7",
    refuse: "8",
    cancle: "9",
    search: "101",
    exportExcel: "102"
}

var resultType = {
    unknown: 0,
    success: 1,
    failure: 2,
    exception: 3,
}

/*
 * 数字格式转换成千分位
 * param{Object}num，num必须是字符串型，不然.00会丢失精度
 */
function commafy(num) {
    num = num + "";
    num = num.replace(/[ ]/g, "");
    if (num == "") {
        return "";
    }

    var prefix = "";//币种符号或者负号前缀
    if (/^\W\d+(\.\d+)?$/.test(num)) {
        prefix = num.substring(0, 1);
        num = num.substring(1);
    }

    if (isNaN(num)) {
        return "";
    }

    var index = num.indexOf(".");
    if (index == -1) {//无小数点
        var reg = /(-?\d+)(\d{3})/;
        while (reg.test(num)) {
            num = num.replace(reg, "$1,$2");
        }
    } else {
        var intPart = num.substring(0, index);
        var pointPart = num.substring(index + 1, num.length);
        var reg = /(-?\d+)(\d{3})/;
        while (reg.test(intPart)) {
            intPart = intPart.replace(reg, "$1,$2");
        }
        num = intPart + "." + pointPart;
    }

    return prefix + num;
}


function toCurrencyDecimal(symbol, amount) {
    if (amount == "")
        return "-";
    else {

        var reg = /(-?\d+)(\d{3})/;
        while (reg.test(amount)) {
            amount = amount.replace(reg, "$1,$2");
        }

        return "<span class='currencysymbol'>" + symbol + "</span> <span class='amount'>" + amount + "</span>";
    }
}


/*
 * 去除千分位
 * param{Object}num
 */
function delcommafy(num) {
    if ((num + "").trim() == "") {
        return "";
    }
    num = num.replace(/,/gi, '');
    returnnum;
}

Date.prototype.format = function (format) //author: meizz 
{
    var o = {
        "M+": this.getMonth() + 1, //month 
        "d+": this.getDate(),    //day 
        "h+": this.getHours(),   //hour 
        "m+": this.getMinutes(), //minute 
        "s+": this.getSeconds(), //second 
        "q+": Math.floor((this.getMonth() + 3) / 3),  //quarter 
        "S": this.getMilliseconds() //millisecond 
    }
    if (/(y+)/.test(format)) format = format.replace(RegExp.$1,
      (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o) if (new RegExp("(" + k + ")").test(format))
        format = format.replace(RegExp.$1,
          RegExp.$1.length == 1 ? o[k] :
            ("00" + o[k]).substr(("" + o[k]).length));
    return format;
}

String.prototype.toDate = function () {

    if (this == "") {
        return "";
    }
    else {
        var style = 'yyyy-MM-dd';
        var str = this.replaceAll("-", "/");
        return new Date(str).format(style)

    }
}

String.prototype.replaceAll = function (oldStr, newStr) {
    return this.replace(new RegExp(oldStr, "gm"), newStr);
}
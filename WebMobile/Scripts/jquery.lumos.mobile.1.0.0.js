(function ($) {
    $.lumos = lumos = {

        writeCookie: function (name, value) {
            var expDate = new Date();

            expDate.setDate(expDate.getDate() + 1000);

            expDate = expDate.toGMTString();
            document.cookie = name + '=' + escape(value) + '; path=/; expires=' + expDate;
        },

        readCookie: function (name) {
            var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");
            if (arr = document.cookie.match(reg)) {
                return unescape(arr[2]);
            } else {
                return null;
            }
        },

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
            if (str.length == 0) return "";
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

        alert: function (options) {

            var defaults = {
                tip: '',
                confirmCallback: null
            };

            var settings = $.extend(defaults, options || {}),
                $this;

            function initialize() {
                var HTML = '<div style="background:#000;opacity:.5;position:fixed;z-index:99999;left:0px;top:0px;width:100%;height:100%;">'
                        + '</div><div style="background-color: #fff;width: 60%;margin: auto;position: fixed;left: 50%;top: 50%;-webkit-transform:translate(-50%,-50%);-moz-transform:translate(-50%,-50%);transform:translate(-50%,-50%);text-align: center;border-radius: 5px;z-index:100000;display:table;">'
                        + '<div class="bottombdColor" style="border-bottom: 1px solid #AB2B2B;display:table;width:100%;"><span class="fontColor" style="color: #AB2B2B;display:table-cell;line-height:20px;vertical-align:middle;text-align:center;font-size:16px;padding:10px;">'
                        + settings.tip + '</span></div><div style="display:table;width:100%;">'
                        + '<span class="fontColor" style="color: #AB2B2B;display:table-cell;height:50px;line-height:50px;vertical-align:middle;border-left:1px solid #EAEAEA;cursor: pointer;">确定</span></div></div>';
                $this = $(HTML).appendTo($('body'));
                var $btn = $this.children('div:eq(1)');
                $btn.children().eq(0).off('click', confirmBtnClickHandler).on('click', confirmBtnClickHandler);
            }

            function confirmBtnClickHandler() {
                $this.remove();
                if (settings.confirmCallback && typeof settings.confirmCallback == 'function') {
                    settings.confirmCallback();
                }
            }

            initialize();
        },

        tips: function (tip) {
            var dialogtips = $('.dialog-tips')
            $(dialogtips).show();
            $(dialogtips).find('span').text(tip);
            setTimeout(function () { $(dialogtips).hide(); }, 3000);
        },

        loading: {

            show: function () {
                $('.dialog-loading').show();
            },
            hide: function () {
                $('.dialog-loading').hide();
            }

        },

        tipsNum: function (options) {
            options = $.extend({
                obj: null,  //jq对象，要在那个html标签上显示
                str: "+1",  //字符串，要显示的内容;也可以传一段html，如: "<b style='font-family:Microsoft YaHei;'>+1</b>"
                startSize: "12px",  //动画开始的文字大小
                endSize: "30px",    //动画结束的文字大小
                interval: 600,  //动画时间间隔
                color: "red",    //文字颜色
                callback: function () { }    //回调函数
            }, options);
            $("body").append("<span class='num'>" + options.str + "</span>");
            var box = $(".num");
            var left = options.obj.offset().left + options.obj.width() / 2;
            var top = options.obj.offset().top - options.obj.height();
            box.css({
                "position": "absolute",
                "left": left + "px",
                "top": top + "px",
                "z-index": 9999,
                "font-size": options.startSize,
                "line-height": options.endSize,
                "color": options.color
            });
            box.animate({
                "font-size": options.endSize,
                "opacity": "0",
                "top": top - parseInt(options.endSize) + "px"
            }, options.interval, function () {
                box.remove();
                options.callback();
            });
        },

        resultType: {
            unknown: 0,
            success: 1,
            failure: 2,
            exception: 3,
            nologin: 4
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



            var errorHtml = '  <div class="messagebox">';
            errorHtml += ' <div class="wrapper">';
            errorHtml += '   <div class="content">';
            errorHtml += '     <dl>';
            errorHtml += '      <dt class="' + _type + '" ></dt>';
            errorHtml += '    <dd >';
            errorHtml += '     <h1>' + _title + '</h1>';
            errorHtml += '      <p>' + lumos.htmlDecode(_content) + '</p>';
            errorHtml += '     </dd>';
            errorHtml += '   </dl>';


            if (_errorStackTrace != "") {
                errorHtml += '<div class=\"errorstacktrace\">';
                errorHtml += _errorStackTrace;
                errorHtml += '</div>';
            }


            errorHtml += '  <div class="clear"></div>';
            errorHtml += ' </div>';
            errorHtml += ' </div>';
            errorHtml += '</div>';


            $('.body').html(errorHtml)

        },

        postJson: function (opts) {

            opts = $.extend({
                isShowLoading: false,
                url: '',
                data: null,
                async: true,
                timeout: 0,
                beforeSend: function (XMLHttpRequest) {
                },
                complete: function (XMLHttpRequest, status) {
                    if (status == 'timeout') {

                    }
                    else if (status == 'error') {

                    }
                },
                success: function () { }
            }, opts || {});

            var _url = opts.url;

            if (_url == '') {

                return;
            }




            var _data = opts.data;
            var _async = opts.async;
            var _timeout = opts.timeout;
            var _success = opts.success;
            var _beforeSend = opts.beforeSend;
            var _complete = opts.complete;
            var _isShowLoading = opts.isShowLoading

            var postStr = "";
            var obj = {};
            if (_data != null) {
                if (typeof (_data.length) != 'undefined') {
                    $.each(_data, function (i, v) {
                        obj[v.name] = v.value;
                    })

                    postStr = JSON.stringify(obj);
                }
                else {
                    postStr = JSON.stringify(_data);
                }
            }

            //获取防伪标记
            var token = $('[name=__RequestVerificationToken]').val();
            var headers = {};
            //防伪标记放入headers
            //也可以将防伪标记放入data
            headers["__RequestVerificationToken"] = token;

            var handling;

            $.ajax({
                type: "Post",
                dataType: "json",
                async: _async,
                headers: headers,
                timeout: _timeout,
                data: _data,
                url: _url,
                beforeSend: function (XMLHttpRequest) {
                    if (_isShowLoading) {
                        $.lumos.loading.show();
                    }
                    else {
                        _beforeSend(XMLHttpRequest);
                    }
                },
                complete: function (XMLHttpRequest, status) {
                    _complete(XMLHttpRequest, status);
                }
            }).done(function (d) {

                if (_isShowLoading) {
                    $.lumos.loading.hide();
                }

                if (d.result == $.lumos.resultType.exception) {
                    $.lumos.tips(d.message);
                }
                else if (d.result == $.lumos.resultType.nologin) {
                    var data = d.data;
                    window.location = data.loginPage;
                }
                else {
                    _success(d);
                }
            });
        }

    }


    $.fn.loadDataUl = function (opts) {

        opts = $.extend({
            emptyTip: "暂时没有数据",//空数据提示
            url: 'test.apsx',//获取数据的URL
            searchButtonId: "btn_Search",//查询按钮ID
            loadMoreButtonId: "btn_More",//查询按钮ID
            searchParams: null,//查询的的参数
            rowDataCombie: function () { },//行数据组合
            operate: null,//操作方法，以元素operate="delete"为属性 过滤
            containerId: 'form1',//表单的容器
            success: function (data) { },
            refreshInterval: 0,
            isShowLoading: true
        }, opts || {});

        var _thisTable = $(this); //当前table
        var _url = opts.url;//访问的地址
        var _searchParams = opts.searchParams;//查询条件
        var _emptyTip = opts.emptyTip;
        var _container = $("#" + opts.containerId);
        var _success = opts.success;
        var _searchButtonId = opts.searchButtonId;
        var _loadMoreButtonId = opts.loadMoreButtonId;
        var _refreshInterval = opts.refreshInterval;
        var _isShowLoading = opts.isShowLoading;


        if (_searchParams == null) {
            _searchParams = new Array();
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

                if (field.name == "Model.PageIndex") {
                    field.value = currentPageIndex
                }
                else if (field.name == "Model.PageSize") {
                    field.value = 10;
                }
                else {
                    field.value = $("*[name='" + field.name + "']").val();

                }
            });

            if (currentPageIndex == 0) {
                $(_thisTable).html('');
            }


            var l_StrRows = ""; //行数据

            $.lumos.postJson({
                type: "post",
                url: _url,
                async: true,
                dataType: 'json',
                data: _searchParams,
                isShowLoading: isShowLoading,
                beforeSend: function (XMLHttpRequest) {

                },
                complete: function (XMLHttpRequest, status) {

                },
                success: function (d) {

                    var dataContent = d.data;

                    var l_pageCount = Math.ceil(dataContent.totalRecord / dataContent.pageSize)//页数

                    $(_thisTable).data('pageCount', l_pageCount);


                    _success(dataContent);

                    var list = dataContent;
                    var list_Data = null;
                    if (typeof list.rows != "undefined") {
                        list_Data = dataContent.rows
                    }

                    var tr_body = $(_thisTable);

                    if (list_Data.length == 0) {
                        $(_thisTable).hide();
                    }
                    else {
                        $(_thisTable).show();
                    }

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
                }
            });
        }

        _searchParams.push({ name: "Model.PageSize", value: 10 });
        _searchParams.push({ name: "Model.PageIndex", value: 0 });

        getList(0, _searchParams, _isShowLoading);

        //处理查询按钮
        $("#" + _loadMoreButtonId).on("click", function () {

            _searchParams = opts.searchParams;
            var l_currentPageIndex = $(_thisTable).data('currentPageIndex')
            var currentPageIndex = parseInt(l_currentPageIndex) + 1;

            getList(currentPageIndex, _searchParams, true);
        });

        var isEnd = false;/*结束标志*/
        $(window).scroll(function () {
            if (isEnd == true) {
                return;
            }

            // 当滚动到最底部以上100像素时， 加载新内容
            // 核心代码
            if ($(document).height() - $(this).scrollTop() - $(this).height() < 100) {

                var l_pageCount = $(_thisTable).data('pageCount');
                var l_currentPageIndex = $(_thisTable).data('currentPageIndex')

                l_currentPageIndex++;

                _searchParams = opts.searchParams;

                if (l_currentPageIndex != l_pageCount) {
                    getList(l_currentPageIndex, _searchParams, true);
                }
                else {
                    $('.item-empty-tips span').text('~到底了哦~');
                    $('.item-empty-tips').show();
                }
            }
        });

        this.loadData = function (index) {
            var pageIndex = $(_thisTable).data('currentPageIndex');
            if (typeof index != 'undefined') {
                pageIndex = index;
            }
            getList(pageIndex, _searchParams, true);

        }

        return this;

    }

})(jQuery);

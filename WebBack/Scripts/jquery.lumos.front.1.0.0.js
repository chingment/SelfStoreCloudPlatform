(function ($) {
    $.lumos = lumos = {


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

        messageBox: function (opts) {
            opts = $.extend({
                type: 'tip',
                title: '标题',
                content: '内容',
                isPopup: true,
                showBoxId: '',
                padding: "0px 0px"
            }, opts || {});


            var _type = opts.type.toLowerCase();
            var _title = opts.title;
            var _content = opts.content;
            var _isPopup = opts.isPopup;
            var _showBoxId = opts.showBoxId;
            var _padding = opts.padding;

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
            errorHtml += '  <div class="clear"></div>';
            errorHtml += ' </div';
            errorHtml += ' </div';
            errorHtml += '</div>';




            if (_isPopup) {
                art.dialog({
                    content: errorHtml,
                    cancelVal: 'Close',
                    title: 'Tip',
                    width: "500px",
                    height: "100px",
                    cancel: true,
                    padding: _padding
                });
            }
            else {
                if (_showBoxId != '') {
                    $("#" + _showBoxId).html(errorHtml)
                }
                else {
                 
                    if ($("#gbr_main_content").length > 0) {
                        $('#gbr_main_content').html(errorHtml)
                    }
                    else {
                        $('body').html(errorHtml)
                    }

                }
            }




        },
        postJson: function (opts) {

            opts = $.extend({
                url: '',
                data: null,
                async: true,
                timeout: 0,
                beforeSend: function () { },
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

            $.ajax({
                type: "Post",
                dataType: "json",
                async: _async,
                timeout: _timeout,
                data: _data,
                url: _url,
                beforeSend: _beforeSend,
                complete: function (XMLHttpRequest, status) {
                    if (status == 'timeout') {
                        art.dialog.tips("网络请求超时,请重新打开页面");
                    }
                    else if (status == 'error') {
                        art.dialog.tips("网络请求失败,请检查网络是否已连接");
                    }
                }
            }).done(function (data) {
                if (data.result == "Exception") {
                    var messsage = data.content;
                    art.dialog.tips("").hide();
                    $.lumos.messageBox({ type: "exception", title: messsage.Title, content: messsage.Content })
                }
                else {
                    _success(data);
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
                success: function (data) {

                    if (data.result == "Exception") {
                        var messsage = data.content;
                        $.lumos.messageBox({ type: "exception", title: messsage.Title, content: messsage.Content })
                    }
                    else {
                        _success(data);
                    }
                }
            });

        }
    }


    $.fn.loadDataTable = function (opts) {

        opts = $.extend({
            emptyTip: "暂时没有数据",
            url: 'test.apsx',
            searchButtonId: "btn_Search",
            searchParams: null,
            rowDataCombie: function () { }
        }, opts || {});

        var _thisTable = $(this); //当前table
        var _url = opts.url;//访问的地址
        var _searchParams = opts.searchParams;//查询条件
        var _emptyTip = opts.emptyTip;

        //alert(Object.prototype.toString.call(opts.searchParams)  )

        var _searchButtonId = opts.searchButtonId;
        if (_searchParams == null) {
            _searchParams = new Array();
        }


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



            if (l_pageIndex != l_pageCount - 1) {
                l_paginationIndex += '<li page-index="' + (l_pageIndex + 1) + '"><a>›</a></li>';
            }
            else {

                l_paginationIndex += '<li class="disabled" page-index="0"><a>›</a></li>';
            }



            if (l_pageSize > 1 && (l_pageIndex != l_pageCount - 1)) {
                l_paginationIndex += '<li  page-index="' + (l_pageIndex + 1) + '"><a>»</a></li>';
            }
            else {
                l_paginationIndex += '<li class="disabled" page-index="0"><a>»</a></li>';
            }





            l_paginationIndex += '</ul>';
            l_paginationIndex += '</div>';

            l_pagination += l_paginationIndex;


            var l_paginationGoto = "<div class=\"pagination-goto\">  <input class=\"ipt input-go\"  type=\"text\"/>  <input class=\"btn btn-go\" type=\"button\"  pagecount=\"" + l_pageCount + "\" value=\"跳转\" /> </div>";
            l_pagination += l_paginationGoto;


            return l_pagination;
        }




        function getList(currentPageIndex, searchparams) {


            $.each(searchparams, function (i, field) {
                if (field.name == "PageIndex") {
                    field.value = currentPageIndex
                }
                else if (field.name == "PageSize") {
                    field.value = 10;
                }
                else {
                    field.value = $("*[name='" + field.name + "']").val();
                }
            });


            //alert(JSON.stringify(searchparams))
            // alert(currentPageIndex)

            var l_StrRows = ""; //行数据
            $.ajax({
                type: "post",
                url: _url,
                async: true,
                dataType: 'json',
                data: _searchParams,
                beforeSend: function (XMLHttpRequest) {
                },
                complete: function (XMLHttpRequest, textStatus) {
                }
            }).done(function (data) {

                var dataContent = data.content;



                var list = dataContent;
                var list_Data = null;
                if (typeof list.Rows != "undefined") {
                    list_Data = dataContent.Rows
                }



                $(_thisTable).find("tbody tr").remove(); //删除所有行
                $.each(list_Data, function (p_index, p_row) {

                    l_StrRows += opts.rowDataCombie(p_index, p_row); //加载行数据

                });
                $(_thisTable).find("tbody").append(l_StrRows); //追加所有行

                $(_thisTable).addTableStyle();

                if (list_Data.length == 0) {
                    var headLen = $(_thisTable).find("thead tr th").length;
                    $(_thisTable).find("tbody").append("<tr><td colspan=\"" + headLen + "\" class=\"emptytip\">" + _emptyTip + "</td></tr>");
                }


                $(_thisTable).find(".pagination").html("");
                pagination = getPagination(list.TotalRecord, list.PageSize, currentPageIndex);
                $(_thisTable).find(".pagination").append(pagination);


            });
        }

        _searchParams.push({ name: "PageSize", value: 10 });
        _searchParams.push({ name: "PageIndex", value: 0 });

        getList(0, _searchParams);


        $(_thisTable).find(".pagination-index li").live("click", function () {
            var currentPageIndex = $(this).attr("page-index");
            var classname = $(this).attr("class");
            if (classname != "active" && classname != "disabled") {
                getList(currentPageIndex, _searchParams);
            }
        });

        $(_thisTable).find(".pagination-goto .btn-go").live("click", function () {
            var index = $(_thisTable).find(".pagination-goto .input-go").val();
            var pagecount = parseInt($(this).attr("pagecount"));
            var regexp = /^[1-9]\d*$/;
            if (!regexp.test(index)) {
                art.dialog.tips("请输入正整数");
                return;
            }

            if (index > pagecount) {
                art.dialog.tips("请重新输入,不能超过" + pagecount);
                return;
            }

            index = index - 1;

            getList(index, _searchParams);

        });


        $("#" + _searchButtonId).live("click", function () {
            _searchParams = opts.searchParams;

            getList(0, _searchParams);
        });
    }


    $.fn.initUploadImageForm = function (options) {
        var defaults = {
            url: "/Common/UploadImage",//调用的后台方法
            path: ""//上传到里路径，如：/Fund
        };
        var opts = $.extend({}, defaults, options);

        var _template =
            '<input type="file" style="display:none;" class="if-fileinput" id="if-fileinput" name="if-fileinput" />' +
            '<img style="border:1px solid #A9A9A9;" />' +
            '<div style="display:none;position:absolute;top:0;"><span style="cursor:pointer;" class="if-uploadbutton">上传</span></div>' +
            '<input type="hidden" />';

        $(this).append(_template);

        $(this).mouseenter(function () {
            $(this).find("div").show();
        });

        $(this).mouseleave(function () {
            $(this).find("div").hide();
        });

        $(this).find(".if-uploadbutton").click(function () {
            $(this).parent().parent().find("input[type=file]").trigger("click");
        });

        $(this).find(".if-fileinput").attr("accept", "image/gif, image/jpeg, image/png, image/x-ms-bmp");
        $(this).find(".if-fileinput").change(function () {
            var imgId = $(this).parent().find("img");//显示图片的控件对象
            var fileName = $(this).val();//文件名
            var fileInputName = $(this).attr("name");//文件输入控件名称
            var form = $(this).parent();
            if (fileName != "") {
                if (!fileName.match(/.jpg|.gif|.png|.bmp/i)) {
                    art.dialog.alert('您上传的图片格式(.jpg|.gif|.png|.bmp)不正确，请重新选择！');
                    $(this).val("");
                    $(imgId).attr("src", "").hide();
                    //对应的hidden.val("")
                    return;
                }

                var fileInputFileValue = "";//旧文件名
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
                        $(imgId).attr("src", "").hide();
                        //$("#" + fileInputFileName).val("");//hidden控件
                    }
                    return;
                }

                var p_d = art.dialog({
                    title: '图片上传',
                    content: '正在上传图片...请稍后！',
                    cancel: false,
                    lock: true,
                    dblclickClose: false
                });

                $(form).ajaxSubmit({
                    type: "post",
                    url: url,
                    dataType: "json",
                    success: function (data) {
                        p_d.close();
                        if (data.result == "Success") {
                            var imgObject = data.content;
                            if (imgObject.OriginalPath.length > 0) {
                                if ($(imgId).length > 0) {
                                    $(imgId).attr("src", imgObject.OriginalPath).show();
                                }
                                $(form).find("input[type=hidden]").val(imgObject.OriginalPath);//上传成功后把新文件名保存到hidden控件中
                            }
                        }
                        else {
                            $("#" + fileInputName).val("");
                            art.dialog.alert(data.message)
                        }
                        return false;
                    }
                });
            }

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
            if (typeof window.clipboardData != undefined) {
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

    $.fn.setDisabled = function (disabled) {
        $(this).attr("disabled", disabled);

        if (disabled) {
            $(this).css("cursor", "default");
        }
        else {
            $(this).css("cursor", "pointer");
        }
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


})(jQuery);



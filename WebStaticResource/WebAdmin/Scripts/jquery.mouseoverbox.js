
(function ($) {

    $.fn.setMouseOverTipPos = function (opts) {

        opts = $.extend({
            tipBoxId: '',
            limitBoxId:''
        }, opts || {});



        var pThis = $(this);
        var tipBoxObj = $("#" + opts.tipBoxId);
        var limitBoxObj = $("#" + opts.limitBoxId);

        var pThisOffWidth = 130, pThisOffHeight = 130;


        var left = 0;
        var top = 0;


        var pThisOffSetLeft = pThis.offset().left;
        var pThisOffSetTop = pThis.offset().top, tipBoxOffHeight = tipBoxObj.outerHeight(true), tipBoxOffWidth = tipBoxObj.outerWidth(true);


        var arrow = "bottom";
        if (limitBoxObj) {

            var margin = 10;
            var limitBoxOffSetTop = limitBoxObj.offset().top;
            var limitBoxOffSetLeft = limitBoxObj.offset().left;
            var limitBoxOffSetRight = limitBoxOffSetLeft + limitBoxObj.width();
            var limitBoxOffSetBottom = limitBoxOffSetTop + limitBoxObj.height();
            var limitBoxHeight = limitBoxObj.height();
            var limitBoxWidth = limitBoxObj.width();
            left = ((pThisOffWidth / 2) + pThisOffSetLeft) - (tipBoxOffWidth / 2);//计算出提示框离左边的距离,以鼠标划过框为中心点
            top = pThisOffHeight + pThisOffSetTop - margin;


            if (left > limitBoxOffSetLeft && (left + tipBoxOffWidth) < limitBoxOffSetRight + 10) {
                if ((limitBoxOffSetBottom - pThisOffSetTop - pThisOffHeight) > tipBoxOffHeight) {
                    arrow = "bottom";
                }
                else {
                    if ((limitBoxOffSetBottom - pThisOffSetTop) > tipBoxOffHeight) {
                        if ((pThisOffSetLeft - limitBoxOffSetLeft) < tipBoxOffWidth) {

                            left = pThisOffSetLeft + pThisOffWidth - margin;
                            top = pThisOffSetTop;
                            arrow = "right";
                        }
                        else {
                            left = pThisOffSetLeft - tipBoxOffWidth + margin;
                            top = pThisOffSetTop;
                            arrow = "left";
                        }
                    }
                    else {
                        arrow = "top"
                        top = limitBoxOffSetBottom - (pThisOffHeight + tipBoxOffHeight) + margin;
                    }
                }
            }
            else {

                if (left < limitBoxOffSetLeft && (left + tipBoxOffWidth) < limitBoxOffSetRight) {
                    if ((limitBoxOffSetBottom - pThisOffSetTop - pThisOffHeight) > tipBoxOffHeight) {
                        left = limitBoxOffSetLeft;
                        arrow = "bottom_left";
                    }
                    else {

                        left = pThisOffSetLeft + pThisOffWidth - margin;
                        top = pThisOffSetTop;
                        arrow = "right";

                        if ((top + tipBoxOffHeight) > limitBoxOffSetBottom) {
                            left = limitBoxOffSetLeft;
                            top = limitBoxOffSetBottom - tipBoxOffHeight - pThisOffWidth + margin;
                            arrow = "top_left";
                        }
                    }
                }
                else {

                    if ((limitBoxOffSetBottom - pThisOffSetTop - pThisOffHeight) > tipBoxOffHeight) {
                        left = limitBoxOffSetRight - tipBoxOffWidth;
                        arrow = "bottom_right";
                    } else {

                        left = pThisOffSetLeft - tipBoxOffWidth;
                        top = pThisOffSetTop;
                        arrow = "left";
                        if ((top + tipBoxOffHeight) > limitBoxOffSetBottom) {
                            left = limitBoxOffSetRight - tipBoxOffWidth;
                            top = limitBoxOffSetBottom - (pThisOffHeight + tipBoxOffHeight) + margin;
                            arrow = "top_right";
                        }
                    }
                }
            }
        }

        $(tipBoxObj).css({ top: top + "px", left: left + "px" }).find("#tipArr").get(0).className = "arrow arrow_" + arrow;

        $(tipBoxObj).show();

   
        $(tipBoxObj).mouseenter(function () {
            $(this).show();
        });

        $(this).mouseleave(function () {
            $(tipBoxObj).hide();
        });

        $(tipBoxObj).mouseleave(function () {
            $(this).hide();
        });

        $(limitBoxObj).mouseleave(function () {
            $(tipBoxObj).hide();
        });

    }

})(jQuery);
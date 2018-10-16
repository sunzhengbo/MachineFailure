$(function () {
    //点击按钮显示信息
    var $span = $('.wrapper .bd .header .s-m-list span');
    var viewInfo = function () {
        $span.click(function () {
            $(this).html('v');
            $(this).siblings().show();
        });
        var $div = $('.wrapper .bd .header .s-m-list');
        var timer;
        var time1;
        var time2;
        $div.mouseleave(function () {
            time1 = new Date().getTime();
            timer = setTimeout(function () {
                $div.children('span').html('>');
                $div.children('.s-m-item').hide();
            }, 500);
        });
        $div.children('.s-m-item').mouseenter(function () {
            time2 = new Date().getTime();
            if (time2 - time1 < 200) {
                clearTimeout(timer);
            }
        });
    }();
});
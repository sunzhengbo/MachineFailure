$(function () {
    //点击过滤按钮弹出时间选择框
    var filterBth = $('.wrapper .bd .list .title .filter');
    filterBth.click(function () {
        //显示
        $('.wrapper .bd .time-filter').show();
        //绑定时间选择插件
        var date = new Date();
        //年月选择器
        laydate.render({
            elem: '#filterTime',
            type: 'month',
            min: '2013-9-1',
            max: date.getFullYear + date.getMonth,
            range: '~'
        });
        //点击重置
        var $input = $('.wrapper .bd .time-filter input');
        $('.wrapper .bd .time-filter .reset').click(function () {
            $input.val("");
        });
        //点击取消
        var $filter = $('.wrapper .bd .time-filter');
        $('.wrapper .bd .time-filter .cancel').click(function () {
            $filter.hide();
        });
        //点击确认
        $('.wrapper .bd .time-filter .confirm').click(function () {
            //限制输入框不能为空
            if (!$input.val() || !$input.val().replace(/(^\s*)|(\s*$)/g, "")) {
                alert("内容不能为空");
                return;
            }
            //TODO...提交信息
            var $list = $('.list .list-content');
            $.ajax({
                url: "/Records/GetRecordsByBLL",
                type:'post',
                datatype: 'json',
                data: { id: $list.data('id'), searchText: $list.data('name'), time: $input.val() },
                success: function (result) {
                    $list.children().remove();
                    if (result) {
                        var arr = JSON.parse(result);
                        for (var i = 0; i < arr.length; i++) {
                            $list.append('<li data-id=' + arr[i].Id + '>' + arr[i].Timestamp + '</li>')
                        }
                    }
                    //隐藏
                    $filter.hide();
                    $input.val(""); //置空
                    //调用显示图片的方法
                    viewPic();
                },
                error: function () {
                    console.log('The request failed');
                }
            });
        });
    });

    var $img = $('.detail .img-info .img-wrap');
    //点击记录显示图片和故障信息
    var viewPic = function () {
        //获取记录列表
        var recs = $('.list .list-content li');
        recs.click(function () {
            var $cont = $('.detail .text-info .content');
            $.ajax({
                url: '/Records/GetRecordsDetial',
                type: 'post',
                data: { id: $(this).data('id') },
                datatype: 'json',
                success: function (result) {
                    var rec = JSON.parse(result)[0];
                    $img.children().remove();
                    if (rec.FaultInfos == 'noPath')
                    {
                        alert('BinaryBase path is not found');
                    }
                    else if (rec.FaultInfos == 'noRecord')
                    {
                        alert('No record');
                    }
                    else {
                        var pics = rec.listPic;
                        for (var i = 0; i < pics.length; i++) {
                            if (pics[i] == 'noFile')
                            {
                                $img.append('<h3>Image does not exist or file path error</h3>');
                            }
                            else{
                                $img.append('<img src=' + pics[i] + '>');
                            }
                        }
                        $cont.html(rec.FaultInfos)
                        playImage();
                    }
                },
                error: function () {
                    console.log('The request failed');
                }
            });
        });
    }
    viewPic();

    var playImage = function () {
        //图片轮播
        if ($img.children('img')) {
            //获取图片存放的盒子的集合,遍历盒子
            for (let i = 0; i < $img.length; i++) {
                //当图片大于1时才轮播图片
                if ($($img[i]).children('img').length > 1) {
                    //调用图片轮播的方法
                    $($img[i]).shufflingFigure({ time: '3' })
                }
            }
        }
    }
});
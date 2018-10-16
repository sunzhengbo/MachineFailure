$(function () {
    //鼠标移入显示，并加载数据
    var $li = $('.top-nav-wrap .drop-down div>ul>li');
    //显示隐藏切换
    $li.hover(function () {
        $(this).children('ul').toggle();
        $(this).children('div').toggle();
    });
    //加载数据
    var $parent_li = $('.top-nav-wrap .drop-down .left-nav > ul > li')
    $parent_li.mouseenter(function () {
        var $index = $(this).data('id');
        if ($index == 0) {
            $.ajax({
                url: '/Home/GetCustomersByBLL',
                type: 'get',
                datatype: 'json',
                success: function (data) {
                    var arr = JSON.parse(data);
                    var $this = $parent_li.eq($index).children('ul');
                    $this.children().remove();
                    for (var i = 0; i < arr.length; i++) {
                        $this.append('<li data-id=' + arr[i].ID + '>' + arr[i].Name + '</li>');
                    }
                    changeContent();
                },
                error: function () {
                    console.log('The request failed');
                }
            });
        } else {
            var url = { 1: '/Sites/AjaxLoadSites', 2: '/Machines/AjaxLoadMachines', 3: '/FaultType/AjaxGetFaultTypes' }
            var value = $parent_li.eq($index-1).data('parent-id');
            $.ajax({
                url: url[$index],
                type: 'post',
                data: { ID: value },
                datatype: 'json',
                success: function (data) {
                    var arr = JSON.parse(data);
                    var $this = $parent_li.eq($index).children('ul');
                    var $ft = $parent_li.eq($index).children('div');
                    $this.children().remove();
                    $ft.children().remove();
                    for (var i = 0; i < arr.length; i++) {
                        if($index != 3){
                            $this.append('<li data-id=' + arr[i].ID + '>' + arr[i].Name + '</li>');
                        }
                        else
                        {
                            var siteID = $parent_li.eq(1).data('parent-id');
                            var mID = $parent_li.eq(2).data('parent-id');
                            var searchText = arr[i].Name;
                            var siteName = $parent_li.eq(1).children('span').html();
                            var machineName = $parent_li.eq(2).children('span').html();
                            var recURL = '/Records/ShowRecords?id=' + mID + '&searchText=' + searchText + '&siteName=' + siteName + '&machineName=' + machineName + '&siteID=' + siteID;
                            $ft.append('<a href="'+recURL+'">' + arr[i].Name + '</a>');
                        }
                    }
                    changeContent();
                },
                error: function () {
                    console.log('The request failed');
                }
            });
        }
    });
    //切换菜单内容
    var changeContent = function () {
        var $left_li = $('.top-nav-wrap .drop-down .left-nav .menu-list li');
        $left_li.click(function () {
            $(this).parent().siblings().html($(this).html());
            $(this).parent().parent().attr('data-parent-id', $(this).data('id'));
            var $index = $(this).parent().parent().data('id');
            //点击前一个按钮的话，那么后面的菜单内容恢复到默认的内容
            switch ($index) {
                case 0:
                    $parent_li.eq(1).children('span').html('站点名称');
                    $parent_li.eq(2).children('span').html('机组名称');
                    $parent_li.eq(3).children('span').html('故障类型');
                    $parent_li.eq(1).children('ul').children().remove();
                    $parent_li.eq(2).children('ul').children().remove();
                    $parent_li.eq(3).children('div').children().remove();
                    break;
                case 1:
                    $parent_li.eq(2).children('span').html('机组名称');
                    $parent_li.eq(3).children('span').html('故障类型');
                    $parent_li.eq(2).children('ul').children().remove();
                    $parent_li.eq(3).children('div').children().remove();
                    break;
                case 2:
                    $parent_li.eq(3).children('span').html('故障类型');
                    $parent_li.eq(3).children('div').children().remove();
                    break;
            }
        });
        var $left_a = $('.top-nav-wrap .drop-down .left-nav .menu-list a');
        $left_a.click(function () {
            $(this).parent().siblings().html($(this).html());
        });
    }


    // 搜索框的内容选择
    $('.top-nav-wrap .search .frm-sear .search-bd').hover(
        function () {
            //.top-nav-wrap .search .frm-sear .search-content 切换显示隐藏
            $(this).children('.search-content').toggle();
        }
    );

    //选择内容填充到输入框
    $('.top-nav-wrap .search .frm-sear .search-bd .search-content ul li').click(function () {
        $('.top-nav-wrap .search .frm-sear .search-bd .search-border').val($(this).html());
        $('.top-nav-wrap .search .frm-sear .search-bd .search-border').attr('placeholder', $(this).html());
    });
});
$(function () {
    var date = new Date();
    //年月选择器
    laydate.render({
        elem: '#laydate',
        type: 'month',
        min: '2013-9-1',
        max: date.getFullYear + date.getMonth
    });

    //添加结构信息
    var $add = $('.info-wrapper .record-infos .item .add-img');
    $add.click(function () {
        var $index = $(this).attr('data-index');
        var $infos = $('.info-wrapper');  //添加组态信息
        
        //点击添添加按钮显示相应的输入框
        //$form.hide().eq($index).show();  //这样写会重复提交，虽然是隐藏了，但是实际提交的过程中还是存在的
        var $formConfigure = { 0: '请输入合同主体名', 1:'请输入站点名', 2: '请输入机器名', 3: '请输入故障类型' }
        var $form = $infos.children('.configure-infos');
        if ($form) { //判断当前是否存在组态信息节点，存在就删除
            $form.remove();
        }
        $infos.append('<form action="#"  class="configure-infos">\
                            <span>' + $formConfigure[$index] + '</span>\
                            <input type="text" class="add-content">\
                            <div class="button">\
                                <input type="button" value="重置">\
                                <input type="button" value="取消">\
                                <input type="button" value="确定" class="sure">\
                            </div>\
                        </form>');

        //点击按钮操作相应的内容
        var $addBth = $('.info-wrapper .configure-infos .button input');
        $addBth.click(function () {
            var url_put = { 0: 'AddCustomer', 1: 'AddSite', 2: 'AddMachine', 3: 'AddFaultType' };  //设置url
            var $content = $(this).parent().siblings('.add-content');  //组态信息的输入框内容

            switch ($(this).index()) {
                case 0:  //清空
                    $content.val("");
                    break;
                case 1:  //取消
                    $infos.children('.configure-infos').remove();
                    break;
                case 2:  //提交
                    var $select = $('.info-wrapper .record-infos .item select');
                    //验证组态信息不能为空
                    var $input0 = $content.eq(0).val();
                    //将空格过滤掉
                    if (!$input0 || !$input0.replace(/(^\s*)|(\s*$)/g, "")) {
                        alert("内容不能为空");
                        return;
                    }
                    //准备提交数据
                    var data = {
                        0: { name: $content.val() },
                        1: {
                            customerID: $select.eq(0).children('option:selected').attr('value'),
                            name: $content.eq(0).val(),
                            addr: $content.eq(1).val()
                        },
                        2: {
                            siteID: $select.eq(1).children('option:selected').attr('value'),
                            customerID: $select.eq(0).children('option:selected').attr('value'),
                            name: $content.val(),
                        },
                        3: {
                            macID: $select.eq(2).children('option:selected').attr('value'),
                            siteID: $select.eq(1).children('option:selected').attr('value'),
                            customerID: $select.eq(0).children('option:selected').attr('value'),
                            name: $content.val(),
                        }
                    };
                    //提交数据
                    $.ajax({
                        url: url_put[$index],
                        type: 'post',
                        data: data[$index],
                        dataType: 'text',
                        success: function (msg) {
                            //数据添加成功后关闭组态窗口
                            $infos.children('.configure-infos').hide();
                            var url_get = { 0: 'GetCustomer', 1: 'GetSite', 2: 'GetMachine', 3: 'GetFaultType' };
                            var node = {
                                0: $('#customer'),
                                1: $('#site'),
                                2: $('#macs'),
                                3: $('#sens')
                                };
                            alert(msg);
                            if (msg && msg.indexOf('success') != -1) {  //添加数据成功就去刷新当前组态的列表
                                //清空内容
                                $content.val("");
                                //开始刷新当前组态的列表
                                if ($index == 0) {  //$index == 0是get请求
                                    $.ajax({
                                        url: url_get[$index],
                                        type: 'get',
                                        datatype: 'json',
                                        success: function (data) {
                                            //把字符串json转化json数组
                                            var arr = JSON.parse(data);
                                            if (arr instanceof Array)  //如果是数组就遍历
                                            {
                                                node[$index].children().remove();
                                                for (var i = 0; i < arr.length; i++) { //循环添加option
                                                    if (i == 0) {
                                                        node[$index].append('<option value="0">请输入...</option>');
                                                    }
                                                    node[$index].append('<option value=' + arr[i].ID + '>' + arr[i].Name + '</option>');
                                                }
                                            }
                                            if (data == null) {
                                                node[$index].append('<option value="0">无数据</option>');
                                            }
                                        },
                                        error: function () {
                                            console.log('The configure query failed');
                                        }
                                    });
                                } else {  //$index != 0是post请求
                                    $.ajax({
                                        url: url_get[$index],
                                        type: 'post',
                                        data: { id: $select.eq($index - 1).children('option:selected').attr('value') },
                                        datatype: 'json',
                                        success: function (data) {
                                            //把字符串json转化数组
                                            var arr = JSON.parse(data);
                                            if (arr instanceof Array)  //如果是数组就遍历
                                            {
                                                node[$index].children().remove();
                                                for (var i = 0; i < arr.length; i++) { //循环添加option
                                                    if (i == 0) {
                                                        node[$index].append('<option value="0">请输入...</option>');
                                                    }
                                                    node[$index].append('<option value=' + arr[i].ID + '>' + arr[i].Name + '</option>');
                                                }
                                            }
                                            if (data == null) {
                                                node[$index].append('<option value="0">无数据</option>');
                                            }
                                        },
                                        error: function () {
                                            console.log('The configure query failed');
                                        }
                                    });
                                }
                            }
                        },
                        error: function () {
                            console.log('submit fialed');
                        }
                    });
                    break;
            }
        });
    });

    //加载组态信息
    var ajaxInfo = function (params) {
        params.parent.change(function () {
            var id = $(this).children('option:selected').attr("value");
            if (id != 0) {
                $.ajax({
                    type: 'POST',
                    url: params.url,
                    data: { id: id },
                    dataType: 'json',
                    success: function (data) {
                        params.sub.children().remove();  //清空
                        if (data == "")
                        {
                            params.sub.append('<option value="0">无数据</option>');
                        }
                        else
                        {
                            //把字符串json转化json
                            var arr = JSON.parse(data);
                            if (arr instanceof Array)  //如果是数组就遍历
                            {
                                for (var i = 0; i < arr.length; i++) { //循环添加option
                                    if (i == 0) {
                                        params.sub.append('<option value="0">请输入...</option>');
                                    }
                                    params.sub.append('<option value=' + arr[i].ID + '>' + arr[i].Name + '</option>');
                                }
                            }
                        }
                    },
                    error: function () {
                        console.log('The request failed');
                    },
                });
            }
        });
    }

    //组态参数
    var params = {
        params_customer: {
            parent: $('#customer'),
            sub: $('#site'),
            url: 'GetSite'
        },
        params_site: {
            parent: $('#site'),
            sub: $('#macs'),
            url: 'GetMachine'
        },
        params_mac: {
            parent: $('#macs'),
            sub: $('#sens'),
            url: 'GetFaultType'
        }
    }
    //监听Customer
    ajaxInfo(params.params_customer)
    //监听Site
    ajaxInfo(params.params_site)
    //监听Machine
    ajaxInfo(params.params_mac)

    //获取图片
    var pics;
    $('.picture').change(function (e) {
        pics = [];
        //获取目标文件
        var fileRow = e.target.files || e.dataTransfer.files;
        //如果目标文件存在
        for (let i = 0; i < fileRow.length; i++) {
            //定义一个文件阅读器
            var reader = new FileReader();
            //装载文件
            reader.readAsDataURL(fileRow[i]);
            //文件装载后将其添加在数组中
            reader.onload = function () {
                pics.push(this.result);
            }
        }
    });

    //提交添加信息，并验证输入框不能为空
    $('#ok').click(function () {
        var customer = $('#customer').children('option:selected').attr('value');
        var sit = $('#site').children('option:selected').attr('value');
        var mac = $('#macs').children('option:selected').attr('value');
        var sen = $('#sens').children('option:selected').attr('value');
        var pic = $('.picture').val();
        var time = $('#laydate').val();
        var con = $('.conclusion').val();
        if (!customer) {
            alert("客户主体不能为空");
            return;
        }
        if(sit==0)
        {
            alert("站点不能为空");
            return;
        }
        if(mac==0){
            alert("机组不能为空");
            return;
        }
        if(sen==0)
        {
            alert("故障类型不能为空");
            return;
        }
        if (!pic) {
            alert("图片不能为空");
            return;
        }
        if(!time){
            alert("时间不能为空");
            return;
        }
        if(!con)
        {
            alert("故障信息不能为空");
            return;
        }

        $.ajax({
            url: 'AddRecord',
            type:'post',
            data: {
                customer: customer,
                site: sit,
                macs: mac,
                sens: sen,
                date:time,
                pictures: pics,
                conclusion: con
            },
            dataType:'text',
            success: function (flag) {
                alert(flag);
                window.location.reload();  //刷新当前页面
            },
            error: function (flag) {
                console.log('submit fialed');
            }
        });
    });
});
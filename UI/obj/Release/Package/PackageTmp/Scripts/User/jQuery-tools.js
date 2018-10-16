// 图片轮播功能
$.fn.shufflingFigure = function (parms) {
    var time = null;
    var subscript = null;
    var indexButton = null;
    var bgcClass = null;
    var button = null;
    var leftButton = null;
    var rightButton = null;

    //判断参数是否存在
    if (parms) {
        if (parms.time) {
            time = parms.time;
        }
        subscript = parms.subscript;
        if (subscript) {
            indexButton = subscript.indexButton;
            bgcClass = subscript.bgcClass;
        }
        button = parms.button;
        if (button) {
            leftButton = button.leftButton;
            rightButton = button.rightButton;
        }
    }
    //对于图片的对象集合进行遍历处理，即每个对象都按一下的逻辑处理
    //i是第几个集合对象，o是集合中的每个对象
    $(this).map(function (i, o) {
        var timer = 0;
        var index = 1;

        //获取图片集合
        var $img = $(o).children();
        // console.log($img)
        //判断是否有img标签，没有就抛出异常
        if ($img.length == 0) {
            throw "Don't have img tags";
        }
        //显示第一张
        $img.hide().eq(0).show();
        //判断时间是否存在，不存在就抛出异常
        var reg = /\D/;  //匹配非数字
        if (time == 0) {
            throw "Time is 0"
        } else if (time && reg.test(time)) {
            throw "Time is not number";
        }
        //当时间存在，采用传递的参数，否则默认赋值1000毫秒
        var t = time ? time * 1000 : 1000;
        //切换图片
        var changePicture = function () {
            //判断index的取值
            if (index >= $img.length) {
                index = 0;
            } else if (index < 0) {
                index = $img.length - 1;
            }
            //开始切换图片
            $img.fadeOut().eq(index).fadeIn();
            //切换下标按钮,当没有下标按钮的时候就不调用
            if (indexButtonFunc) {
                indexButtonFunc();
            }
            index++;
        }
        //调用图片切换
        timer = setInterval(changePicture, t);
        //鼠标移动上去停止轮播，离开继续轮播
        var $this = null;
        if (subscript || button) {
            $this = $(this).parent();
        } else {
            $this = $(this)
        }
        $this.hover(function () {
            ``
            clearInterval(timer);
        }, function () {
            timer = setInterval(changePicture, t);
        });

        //点击下标按钮，切换图片
        if (indexButton) {
            //每次都获取自己当前调用的那个下标按钮对象
            var $ul = $(indexButton[i]);
            //当下标按钮对象存在的时候就继续下一步操作
            if ($ul) {
                var $li = $ul.find('li');
                //绑定点击事件
                $li.click(function () {
                    index = $(this).index();
                    changePicture();
                });
                //循环切换下标按钮的颜色,并移除其他的兄弟的颜色
                var indexButtonFunc = function () {
                    $li.eq(index).addClass(bgcClass).siblings().removeClass(bgcClass);
                }
            }
        }

        //左右按钮制作
        if (leftButton && rightButton) {
            var $leftButton = $(leftButton[i]);
            var $rightButton = $(rightButton[i]);
            if ($leftButton && $rightButton) {
                $leftButton.click(function () {
                    index -= 2;
                    changePicture();
                });
                $rightButton.click(function () {
                    changePicture();
                });
            }
        }
    });
}
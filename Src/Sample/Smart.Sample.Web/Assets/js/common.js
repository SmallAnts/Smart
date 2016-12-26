var _loadingFlag = true;
$.ajaxSetup({
    beforeSend: function (XMLHttpRequest) {
        loading(true);
    },
    complete: function (XMLHttpRequest, textStatus) {
        _loadingFlag = true;
        loading(false);
    },
    error: function (xhr, status, e) {
        if (status == "abort") return;
        var reg = new RegExp("<title>(.*?)<\/title>", "i");
        var matchs = reg.exec(xhr.responseText);
        swal(status, matchs ? matchs[1] : xhr.responseText, "error");
        //$.gritter.add({ title: status, text: matchs[1], class_name: 'gritter-error' });
    }
});
function loading(status) {
    if (_loadingFlag && status) $("#loading").show();
    else $("#loading").hide();
}
// 验证表单数据
function validateForm(form) {
    if (!form) {
        form = $("form");
    } else if (typeof form == "string") {
        form = $("#" + form);
    }
    return form.validate ? form.validate().form() : true;
}
function clearValidationError() {
    $(".field-validation-error").empty().addClass("field-validation-valid").removeClass("field-validation-error");
}

// post 表单
// options={button:"#btnOk",url:"",begin:function(data){},end:function(ret){},error:function(ret){}}
function submitForm(button, url, end) {
    var options = {};
    if (typeof button == "string") {
        options = { button: button, url: url, end: end };
    } else {
        options = button;
    }
    $(options.button).on("click", function () {
        var $btn = $(this);
        var formselector = $btn.data("form");
        var $form = formselector ? $(formselector) : $(this).parents("form");
        if (!validateForm($form)) {
            return false;
        } else {
            $(this).button('loading');
            // 复选框处理
            var chksNames = [];
            $(":checkbox", $form).each(function () {
                var $this = $(this);
                var name = $this.attr("name");
                if (name === undefined) return true;
                if ($.inArray(name, chksNames) >= 0) return true;
                var $els = $("input[name='" + name + "']", $form);
                if ($els.length == 1) {
                    var val = $els.data("val");
                    if (val === true) {
                        $this.val($this.prop("checked"));
                    } else {
                        $this.val($this.prop("checked") ? 1 : 0);
                    }
                } else {
                    chksNames.push(name);
                }
            });
            var data = $form.serialize();
            $(':disabled[name]', $form).each(function () {
                $this = $(this);
                var tagName = $this[0].tagName;
                var type = $this.attr('type');
                if (tagName == 'INPUT') {
                    if (type == 'radio' || type == 'checkbox') {
                        if ($this.prop('checked')) data += "&" + this.name + "=" + $this.val();
                    } else {
                        data += "&" + this.name + "=" + $this.val();
                    }
                } else if (tagName == 'SELECT' || tagName == 'TEXTAREA') {
                    data += "&" + this.name + "=" + $this.val();
                }
            });
            $.each(chksNames, function () {
                var chkname = this;
                var rx = new RegExp("&?" + chkname + "=[^&]+", "gi");
                var m = data.match(rx);
                if (m) {
                    data = data.replace(rx, "");
                    var chkvalue = [];
                    $.each(m, function () {
                        chkvalue.push(this.split('=')[1]);
                    });
                    data += "&" + chkname + "=" + chkvalue.join(',');
                }
            });
            if ($.isFunction(options.begin)) {
                var newData = options.begin(data);
                if (newData === false) {
                    $btn.button("reset");
                    return false;
                } else if (newData) {
                    data = newData;
                }
            }
            $.post(options.url, data, function (ret) {
                if (ret.error) {
                    if ($.isFunction(options.error)) {
                        options.error(ret);
                    } else {
                        $.gritter.add({ class_name: 'gritter-error', text: ret.error });
                    }
                } else {
                    if ($.isFunction(options.end)) {
                        options.end(ret);
                    }
                }
                $btn.button("reset");
            }).error(function () {
                $btn.button("reset");
            });
        }
        return false;
    });
}
// 加载数据到表单
function loadData(obj) {
    var key, value, tagName, type, arr;
    for (x in obj) {
        key = x;
        value = obj[x];
        var $els = $("[name='" + key + "'],[name='" + key + "[]']");
        $els.each(function () {
            $this = $(this);
            tagName = $this[0].tagName;
            type = $this.attr('type');
            if (tagName == 'INPUT') {
                if (type == 'radio') {
                    $this.prop('checked', $this.val() == value);
                } else if (type == 'checkbox') {
                    if ($els.length == 1) {
                        var checked = value == 1 || value == "1" || value == true || value == "true" || value == "Yes";
                        $this.val(checked ? 1 : 0).prop('checked', checked);
                    } else {
                        arr = (value || 0).toString().split(',');
                        $this.prop('checked', false);
                        for (var i = 0; i < arr.length; i++) {
                            if ($this.val() == arr[i]) {
                                $this.prop('checked', true);
                                break;
                            }
                        }
                    }
                } else if (type == 'img') {
                    $this.attr("src", value);
                } else {
                    $this.val(value);
                }
            } else if (tagName == 'SELECT' || tagName == 'TEXTAREA') {
                $this.val(value);
            }
        });
    }
}
//初始化JQGRID
function initJqGrid(selector, options, fill) {
    try {
        var _options = {
            altRows: true,
            datatype: 'json',
            gridView: true,
            height: 400,
            mtype: 'POST',
            pager: selector + '_pager',
            rownumbers: true,
            rownumWidth: 25,
            shrinkToFit: false,
            sortable: false,
            viewrecords: true,
        };
        _options = $.extend(_options, options);
        var $grid = $(selector).jqGrid('GridDestroy').jqGrid(_options);
        if (fill != false) jqgrid_fill(selector);
        return $grid;
    } catch (e) {
        $.gritter.add({ title: "error", text: e.message || e, class_name: 'gritter-error' });
    }
}

var _$request;
function gotoUrl(href) {
    var url = href.replace(/\..*$/g, '').replace(/#/gi, '')
    $('.sidebar .active').removeClass('active open');
    $(".sidebar .submenu").removeClass("nav-show").addClass("nav-hide").css("display", "");
    var link = $('a[href="' + href + '"]').eq(0);
    link.parents('li').addClass('active open');
    var text = $.trim(link.find('.menu-text').text());
    if (text.length != 0) text = $.trim(link.text());
    document.title = text;

    var ret = scrollToTarget(href);
    if (ret === false) {
        $('#page-content').empty().html('<i class="ace-icon fa fa-spinner fa-spin blue fa-2x"></i>');
        if (_$request != null) {
            _$request.abort();
        }
        _$request = $.ajax({ url: url, data: { _: Math.random() } }).done(function (result) {
            $('#page-content').addClass('hidden').empty().html(result).removeClass('hidden');
            ret = scrollToTarget(href);
            //if (ret) document.title = ret;
        }).fail(function (e) {
            if (e.statusText == "abort") return;
            // alert(JSON.stringify(e))
            $('#page-content').empty().html('<div class="alert alert-danger"><i class="fa fa-warning"></i> 页面加载失败!</div>');
        });
        // breadcrumbs
        var $actives = $('.sidebar .active');
        var brHtml = '';
        for (var i = 0; i < $actives.size() ; i++) {
            brHtml += get_li($actives[i], i);
        }
        var bc = $("#breadcrumb");
        bc.html(brHtml).find("li:last").addClass("active").find("a").removeAttr("attr");
        bc.find("li:first").insertBefore($($actives[0]).find("i:first").html());
        function get_li(el, i) {
            var $a = $(el).find("a:first");
            var li = '<li>';
            if (i == 0) {
                li += $a.find("i").prop("outerHTML").replace("menu-icon", "ace-icon home-icon");
                li += " ";
            }
            li += '<a >' + $a.text() + '</a></li>';//href="' + $a.attr("href") + '"
            return li;
        }
    }
    //else if (ret) document.title = ret ;
    return true;
}
function getHref(href) {
    href = href && href.replace(/.*(?=#[^\s]*$)/, '') // 删除#号前面的字符
    href = $.trim(href);
    if (href.match(/[\#\/]$/i)) return false;
    return href;
}
function scrollToTarget(href) {
    var target = $('[data-id="' + href + '"]').eq(0);
    if (target.length == 1) {
        $('html,body').animate({ scrollTop: target.offset().top - 75 }, 300);
        return true;
    }
    return false; // 需要JAJX加载
}

function getNowDate() {
    var d = new Date();
    var month = d.getMonth() + 1;
    var day = d.getDate();
    var output = (month < 10 ? '0' : '') + month + '/' +
        (day < 10 ? '0' : '') + day + '/' +
        d.getFullYear();    //mm/dd/yyyy
    return output;
}

// sidebar 插件
(function ($, undefined) {
    function Ace_Sidebar(element, _options) {
        var options = $.extend({}, $.fn.sidebar.defaults, _options || {});
        var $element = $(element);
        var url = window.location.pathname.toLowerCase();

        $element.addClass(options.topMenu ? "sidebar h-sidebar navbar-collapse collapse" : "sidebar responsive");
        try { ace.settings.check('sidebar', 'fixed') } catch (e) { }
        if (options.shortcuts) {
            createShortcuts($element, options.shortcuts);
        }
        if (options.menus) {
            init(options.menus)
        }
        else {
            if (!options.url) return;
            $.post(options.url, options.postParam, function (data) {
                init(data);
            });
        }
        function init(data) {
            var $menus = $('<ul class="nav nav-list">');
            createMenus($menus, data);
            $element.append($menus);
            createToggleButton();
            $element.ace_sidebar();
            activeMenu();
            if (options.onComplete) {
                options.onComplete(data);
            }
        }
        function createShortcuts($element, data) {
            var shortcuts = '';
            shortcuts += '<div class="sidebar-shortcuts" id="sidebar-shortcuts">';
            // large
            shortcuts += '<div class="sidebar-shortcuts-large" id="sidebar-shortcuts-large">';
            for (var i = 0; i < data.length; i++) {
                shortcuts += '<button class="btn ' + data[i].buttonClass + '"><i class="ace-icon fa ' + data[i].iconClass + '"></i></button>';
            }
            shortcuts += '</div>';
            // mini
            shortcuts += '<div class="sidebar-shortcuts-mini" id="sidebar-shortcuts-mini">';
            for (var i = 0; i < data.length; i++) {
                shortcuts += '<span class="btn ' + data[i].buttonClass + '"></span>';
            }
            shortcuts += '</div>';
            // end
            shortcuts += '</div>';
            $element.append(shortcuts);
        }
        function createMenus($element, data) {
            $.each(data, function (i, item) {
                var li = $('<li></li>');
                var a = $('<a></a>');
                var icon = $('<i class="menu-icon fa"></i>').addClass(item.icon);
                var text = $('<span class="menu-text"></span>').text(item.text);
                a.append(icon).append(text);
                if (options.topMenu) li.addClass("hover");
                if (item.menus && item.menus.length > 0) {
                    a.attr('href', '#').addClass('dropdown-toggle');
                    var arrow = $('<b class="arrow fa fa-angle-down"></b>');
                    a.append(arrow);
                    li.append(a);
                    var menus = $('<ul class="submenu"></ul>');
                    createMenus(menus, item.menus);
                    li.append(menus);
                }
                else {
                    a.attr('href', item.url);
                    li.append(a);
                }
                li.append('<b class="arrow"></b>');
                $element.append(li);
                if (item.url && url.toLowerCase() == item.url.toLowerCase()) li.addClass("active");
            });
        }
        function createToggleButton() {
            $element.append('<div class="sidebar-toggle sidebar-collapse" id="sidebar-collapse">\
            <i class="ace-icon fa fa-angle-double-left" data-icon1="ace-icon fa fa-angle-double-left" data-icon2="ace-icon fa fa-angle-double-right"></i>\
        </div>');
            try { ace.settings.check('sidebar', 'collapsed') } catch (e) { }
        }
        function activeMenu() {
            $(".nav-list li.active").click();
        }
    }
    $.fn.sidebar = function (options) {
        var args = arguments;
        var flag = false;
        var val;
        var ret = this.each(function () {
            var $this = $(this);
            var data = $this.data('ace.sidebar')
            if (!data) $this.data('ace.sidebar', (data = new Ace_Sidebar(this, options)));
            if (typeof options == 'string') {
                flag = true;
                val = data[options].apply($this, Array.prototype.slice.call(args, 1));
            }
            return flag ? val : ret;
        });
    }

    $.fn.sidebar.defaults = {
        url: null,
        postParam: null,
        topMenu: false,
        shortcuts: null,
        menus: null,
        onComplete: null
    };
})(window.jQuery);
// 将表单序列化为对象
(function ($) {
    $.fn.serializeJson = function () {
        var serializeObj = {};
        var array = this.serializeArray();
        var str = this.serialize();
        $(array).each(function () {
            if (serializeObj[this.name]) {
                if ($.isArray(serializeObj[this.name])) {
                    serializeObj[this.name].push(this.value);
                } else {
                    serializeObj[this.name] = [serializeObj[this.name], this.value];
                }
            } else {
                serializeObj[this.name] = this.value;
            }
        });
        return serializeObj;
    };
})(jQuery);


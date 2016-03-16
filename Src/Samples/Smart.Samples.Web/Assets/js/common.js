var _loadingFlag = true;
$.ajaxSetup({
    beforeSend: function (XMLHttpRequest) { loading(true) },
    complete: function (XMLHttpRequest, textStatus) { _loadingFlag = true; loading(false); },
    error: function (xhr, status, e) {
        var reg = new RegExp("<title>(.*?)<\/title>", "i");
        var matchs = reg.exec(xhr.responseText);
        swal(status, matchs[1], "error");
    }
});
if (window.swal && swal.setDefaults) {
    swal.setDefaults({ cancelButtonText: '取消', confirmButtonText: "确定" });
}
function loading(status) {
    if (_loadingFlag && status) $("#loading").show();
    else $("#loading").hide();
}
function validateForm(form) {
    if (!form) {
        form = $("form");
    } else if (typeof form == "string") {
        form = $("#" + form);
    }
    return form.validate().form();
}
function defaultSubmit() {
    if (!validateForm($(this).parents("form"))) {
        return false;
    } else {
        $(this).button('loading');
        //this.disabled = true;
    }
}

function gotoUrl(href) {
    var url = href.replace(/\..*$/g, '').replace(/#/gi, '')
    $('.sidebar .active').removeClass('active');
    var link = $('a[href="' + href + '"]').eq(0);
    link.parents('li').addClass('active');

    var text = $.trim(link.find('.menu-text').text());
    if (text.length == 0) text = $.trim(link.text());
    document.title = text;

    var ret = scrollToTarget(href);
    if (ret === false) {
        $('.main-content-inner').empty().html('<div class="page-content"><i class="ace-icon fa fa-spinner fa-spin blue fa-2x"></i></div>');
        $.ajax({ url: url }).done(function (result) {
            $('.main-content-inner').addClass('hidden').empty().html(result).removeClass('hidden');
            ret = scrollToTarget(href);
            //if (ret) document.title = ret;
        }).fail(function (e) {
            //alert(e.responseText)
            $('.main-content-inner').empty().html('<div class="page-content"><div class="alert alert-danger"><i class="fa fa-warning"></i> 页面加载失败!</div></div>');
        });
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

// sidebar 
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
                init(data)
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
                if (item.url && url == item.url.toLowerCase()) li.addClass("active");
            });
        }
        function createToggleButton() {
            $element.append('<div class="sidebar-toggle sidebar-collapse" id="sidebar-collapse">\
            <i class="ace-icon fa fa-angle-double-left" data-icon1="ace-icon fa fa-angle-double-left" data-icon2="ace-icon fa fa-angle-double-right"></i>\
        </div>');
            try { ace.settings.check('sidebar', 'collapsed') } catch (e) { }
        }
        function activeMenu() {
            $(".nav-list li.active").parents(".nav-list li").addClass("active open");
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
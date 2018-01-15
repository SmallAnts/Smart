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

// ace_image 插件
(function ($, undefined) {
    function Ace_Image(element, _options) {
        var options = $.extend({}, $.fn.ace_image.defaults, _options || {});
        var $element = $(element);
        try {
            document.createElement('IMG').appendChild(document.createElement('B'));
        } catch (e) {
            Image.prototype.appendChild = function (el) { }
        }

        $element.editable({
            type: 'image',
            name: options.name,
            value: options.value,
            //onblur: 'ignore',  //don't reset or hide editable onblur?!
            image: {
                //specify ace file input plugin's options here
                btn_choose: options.btn_choose,
                droppable: true,
                maxSize: options.maxSize,
                //and a few extra ones here
                name: options.name, //put the field name here as well, will be used inside the custom plugin
                on_error: options.onError,
                on_success: options.onSuccess
            },
            url: function (params) {
                var submit_url = '/BasicData/UploadBrandLogo';
                var deferred = null;
                var value = $element.next().find('input[type=hidden]:eq(0)').val();
                if (!value || value.length == 0) {
                    deferred = new $.Deferred
                    deferred.resolve();
                    return deferred.promise();
                }

                var $form = $element.next().find('.editableform:eq(0)')
                var file_input = $form.find('input[type=file]:eq(0)');
                var ie_timeout = null

                if ("FormData" in window) {
                    var formData_object = new FormData();//create empty FormData object

                    //serialize our form (which excludes file inputs)
                    $.each($form.serializeArray(), function (i, item) {
                        //add them one by one to our FormData
                        formData_object.append(item.name, item.value);
                    });
                    //and then add files
                    $form.find('input[type=file]').each(function () {
                        var field_name = $(this).attr('name');
                        var files = $(this).data('ace_input_files');
                        if (files && files.length > 0) {
                            formData_object.append(field_name, files[0]);
                        }
                    });

                    //append primary key to our formData
                    //formData_object.append('pk', pk);

                    deferred = $.ajax({
                        url: options.url,
                        type: 'POST',
                        processData: false,//important
                        contentType: false,//important
                        dataType: 'json',//server response type
                        data: formData_object
                    })
                }
                else {
                    deferred = new $.Deferred

                    var temporary_iframe_id = 'temporary-iframe-' + (new Date()).getTime() + '-' + (parseInt(Math.random() * 1000));
                    var temp_iframe =
                            $('<iframe id="' + temporary_iframe_id + '" name="' + temporary_iframe_id + '" \
			frameborder="0" width="0" height="0" src="about:blank"\
			style="position:absolute; z-index:-1; visibility: hidden;"></iframe>')
                            .insertAfter($form);

                    $form.append('<input type="hidden" name="temporary-iframe-id" value="' + temporary_iframe_id + '" />');

                    //append primary key (pk) to our form
                    //$('<input type="hidden" name="pk" />').val(pk).appendTo($form);

                    temp_iframe.data('deferrer', deferred);
                    //we save the deferred object to the iframe and in our server side response
                    //we use "temporary-iframe-id" to access iframe and its deferred object

                    $form.attr({
                        action: options.url,
                        method: 'POST',
                        enctype: 'multipart/form-data',
                        target: temporary_iframe_id //important
                    });

                    $form.get(0).submit();

                    ie_timeout = setTimeout(function () {
                        ie_timeout = null;
                        temp_iframe.attr('src', 'about:blank').remove();
                        deferred.reject({ 'status': 'fail', 'message': 'Timeout!' });
                    }, 30000);
                }

                deferred
                .done(function (result) {//success
                    $element.get(0).src = result.url;
                    options.onDone(result.url);
                })
                .fail(function (result) {//failure
                    last_gritter = $.gritter.add({
                        title: 'error',
                        text: 'There was an error',
                        class_name: 'gritter-error gritter-center'
                    });
                })
                .always(function () {
                    if (ie_timeout) clearTimeout(ie_timeout)
                    ie_timeout = null;
                });

                return deferred.promise();
                // ***END OF UPDATE AVATAR HERE*** //
            },
            success: function (response, newValue) {
            }
        });
    }
    $.fn.ace_image = function (options) {
        var args = arguments;
        var flag = false;
        var val;
        var ret = this.each(function () {
            var $this = $(this);
            var data = $this.data('ace_image')
            if (!data) $this.data('ace_image', (data = new Ace_Image(this, options)));
            if (typeof options == 'string') {
                flag = true;
                val = data[options].apply($this, Array.prototype.slice.call(args, 1));
            }
            return flag ? val : ret;
        });
    }
    $.fn.ace_image.defaults = {
        url: null,
        name: "logo",
        value: null,
        maxSize: 1048576,//1M
        btn_choose: "Change Logo"
    };
})(window.jQuery);
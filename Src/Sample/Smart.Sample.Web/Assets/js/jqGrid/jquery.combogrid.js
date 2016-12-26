; (function ($) {
    'use strict';
    var combogridTmpl = '<div class="combobox-container">\
        <a class="combobox">\
            <span></span><abbr></abbr><div><b class="drop"></b></div>\
        </a>\
        <div class="combobox-drop">\
            <table id="${id}_grid"></table><div id="${id}_grid_pager"></div>\
        </div>\
    </div>';
    // { check:true, beforeClick:function(treeId,znode){}, beforeCheck:function(treeId,znode){},nodes:[] }
    var ComboGrid = function (el, options) {
        var self = this;
        self.element = $(el);
        self.options = $.extend({}, options);
        _init.call(self);
    };
    function _init() {
        try {
            var self = this;
            this.element.hide().after(combogridTmpl);
            var $combo = this.element.next().width(this.element.width());
            // 初始化下表格
            // TODO:
            // 下拉按钮
            $combo.find(".drop").on("click", function () {
                var $box = $combo.find(".combobox");
                $(this).addClass("down");
                $combo.find(".combobox-drop").css({ left: "0px", top: ($box.height() - 1) + "px" }).slideDown("fast");
                $("body").bind("mousedown", function (e) {
                    onBodyDown.call(self, e);
                });
            });
            // 清除按钮
            $combo.find("abbr").on("click", function () {
                self.setValue('', '');
                $(this).hide();
            }).hide();
            // 显示隐藏清除按钮
            $combo.on("mouseenter", function () {
                if (self.element.val().length > 0) {
                    $combo.find("abbr").show();
                }
            });
            $combo.on("mouseleave", function () {
                $combo.find("abbr").hide();
            });
        } catch (e) {
            console.log(e.message);
        }
    }
    // 公共方法
    ComboGrid.prototype = {
        setValue: function (value, text) {
            // set value
            this.element.val(value);
            // set text
            if (text == undefined) {
                var treeObj = $.fn.zTree.getZTreeObj(this.element.attr("id") + "_ztree");
                if (!treeObj) return;
                if (this.options.check) {
                    var ids = value.split(',');
                    var texts = [];
                    treeObj.checkAllNodes(false);
                    treeObj.getNodesByFilter(function (node) {
                        if ($.inArray(node.id.toString(), ids) >= 0) {
                            treeObj.checkNode(node, true, true);
                            texts.push(node.name);
                        }
                        return false;
                    });
                    text = texts.join(',');
                } else {
                    var node = treeObj.getNodeByParam("id", value, null);
                    if (node) {
                        text = node.name;
                        treeObj.selectNode(node);
                    }
                }
            }
            this.element.next().find(".combobox span").text(text);
        },
        getValue: function () {
            console.log(this.element.val());
            return this.element.val();
        },
        getText: function () {
            return this.element.next().find(".combobox span").text();
        }
    };
    $.fn.combogrid = function (options) {
        var args = arguments;
        var returnValue;
        var ret = this.each(function () {
            var $this = $(this);
            var data = $this.data('jquery.combogrid');
            if (!data) $this.data('jquery.combogrid', (data = new ComboGrid(this, options)));
            if (typeof options == 'string') {
                returnValue = data[options].apply(data, Array.prototype.slice.call(args, 1));
            }
        });
        return returnValue == undefined ? ret : returnValue;
    };
    $.fn.combogrid.Constructor = ComboGrid;
    function onRowClick(e, treeId, treeNode) {
        var treeObj = $.fn.zTree.getZTreeObj(treeId);
        var nodes = treeObj.getSelectedNodes();
        _setValueText.call(this, nodes);
        hideMenu.call(this);
    }
    function onRowCheck(event, treeId, treeNode) {
        var treeObj = $.fn.zTree.getZTreeObj(treeId);
        var nodes = treeObj.getCheckedNodes(true);
        _setValueText.call(this, nodes);
    }
    function _setValueText(nodes) {
        var text = [], value = [];
        //nodes.sort(function compare(a, b) { return a.id - b.id; });
        for (var i = 0, l = nodes.length; i < l; i++) {
            text.push(nodes[i].name);
            value.push(nodes[i].id);
        }
        this.setValue(value.join(","), text.join(","));
    }
    function hideMenu() {
        $(".combobox-drop").fadeOut("fast");
        this.element.next().find(".drop").removeClass("down");
        $("body").unbind("mousedown", onBodyDown);
    }
    function onBodyDown(event) {
        if (!(event.target.className == "drop" || event.target.className == "combobox-drop" || $(event.target).parents(".combobox-drop").length > 0)) {
            hideMenu.call(this);
        }
    }
})(jQuery);
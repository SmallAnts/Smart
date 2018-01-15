; (function ($) {
    'use strict';
    var combogridTmpl = '<div class="combobox-container">\
        <a class="combobox">\
            <span></span><abbr></abbr><div><b class="drop"></b></div>\
        </a>\
        <div class="combobox-drop" style="width:360px;">\
             <input type="text" class="input-search" style="width:100%;">\
             <table id="{id}_grid"></table><div id="{id}_grid_keynav"></div>\
        </div>\
    </div>';
    // { check:true, beforeClick:function(gridId,znode){}, beforeCheck:function(gridId,znode){},nodes:[] }
    var ComboGrid = function (el, options) {
        var self = this;
        self.element = $(el);
        self.options = $.extend({
            altRows: true,
            datatype: 'json',
            gridView: true,
            height: 200,
            width: 358,
            mtype: 'POST',
            pager: "#" + self.element.attr("id") + '_grid_keynav',
            rownumbers: true,
            rownumWidth: 25,
            shrinkToFit: true,
            sortable: false,
            viewrecords: true,
            valuefield: "",
            textfield: "",
        }, options);
        _init.call(self);
    };
    function _init() {
        try {
            var self = this;
            var id = self.element.attr("id");
            var gridId = id + "_grid";
            this.element.hide().after(combogridTmpl.replace(/\{id\}/gi, id));
            var $combo = this.element.next().width(this.element.width());
            // 初始化下拉表格
            self.options.ondblClickRow = function (rowid, iRow, iCol, e) {
                onRowClick.call(self, this, rowid);
            };
            if (self.options.multiselect) {
                //self.options.multiboxonly = true;
                self.options.onSelectAll = function (rowids, status) {
                    if (status) {
                        setByChecks.call(self, this, rowids);
                    } else {
                        self.setValue.call(self, "", "");
                    }
                }
                self.options.onSelectRow = function () {
                    setByChecks.call(self, this);
                }
            }

            $("#" + gridId)
                .jqGrid('GridDestroy')
                .jqGrid(self.options)
                .jqGrid('navGrid', "#" + gridId + "_keynav", { edit: false, add: false, del: false })
                .jqGrid('bindKeys', {
                    "onEnter": function (rowid) {
                        onRowClick.call(self, this, rowid);
                    }
                });

            //.jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });
            $("#" + gridId + "_keynav_left").hide();
            $("#" + gridId + "_keynav_center").width(240);
            self.element.next().find(".input-search")
                .on("keydown", function (e) {
                    $(this).data("oldval", this.value);
                })
                .on("keyup", function () {
                    if ($(this).data("oldval") != this.value) {
                        $("#" + gridId).jqGrid("setGridParam", { postData: { search: this.value }, page: 1 }).trigger("reloadGrid");
                    }
                });
            // 下拉按钮
            $combo.find(".drop").on("click", function () {
                var $box = $combo.find(".combobox");
                $(this).addClass("down");
                $combo.find(".combobox-drop").css({ left: "0px", top: ($box.height() - 1) + "px" }).slideDown("fast");
                $combo.find(".input-search").focus();
                $("body").bind("mousedown", function (e) {
                    onBodyDown.call(self, e);
                });

                var $grid = $("#" + gridId);
                var rowids = $grid.jqGrid('getGridParam', 'selarrrow');
                var ids = $.extend([], rowids);
                for (var i = 0; i < ids.length; i++) {
                    $grid.jqGrid('setSelection', ids[i], false);
                }
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
            this.element.val(value);
            this.element.next().find(".combobox span").text(text);
        },
        getValue: function () {
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
    function setByChecks(grid, rowids) {
        var value = [], text = [];
        var $grid = $(grid);
        if (!rowids) {
            rowids = $grid.jqGrid('getGridParam', 'selarrrow');
        }
        for (var i = 0; i < rowids.length; i++) {
            var rowdata = $grid.jqGrid("getRowData", rowids[i]);
            value.push(rowdata[this.options.valuefield]);
            text.push(rowdata[this.options.textfield]);
        }
        this.setValue.call(this, value.join(','), text.join(','));
    }
    function onRowClick(grid, rowid) {
        var $grid = $(grid);
        if (this.options.multiselect) {
            return;
        }
        var rowdata = $grid.jqGrid('getRowData', rowid);
        this.setValue.call(this, rowdata[this.options.valuefield], rowdata[this.options.textfield]);
        hideMenu.call(this);
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
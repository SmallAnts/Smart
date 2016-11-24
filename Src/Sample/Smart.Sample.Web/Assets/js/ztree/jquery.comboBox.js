; (function ($) {
    'use strict';
    var $tree;
    var ComboBox = function (el, options) {
        var self = this;
        self.element = $(el);
        self.options = $.extend({}, options);
    };
    function _init(el) {
        var dropdownId = this.element.attr("id") + '_tree';
        var dropdown =
'<div id="menuContent" class="menuContent" style="display:none; position: absolute;">\
    <ul id="' + dropdownId + '" class="ztree" style="margin-top:0; width:'
        + (this.options.dropDownWidth || this.element.width()) + 'px;"></ul>\
</div>';
        this.element.after(dropdown);
        $ztree = $.fn.zTree.init($("#" + dropdownId), setting, zNodes);
    }
    // 扩展方法
    ComboBox.prototype = {
        getZtree: function () {
            return $tree;
        },
    };
    $.fn.comboBox = function (options) {
        var args = arguments;
        return this.each(function () {
            var $this = $(this);
            var data = $this.data('jquery.comboBox');
            if (!data) $this.data('jquery.comboBox', (data = new ComboBox(this, options)));
            if (typeof options == 'string') data[options].apply(data, Array.prototype.slice.call(args, 1));
        });
    };
    $.fn.comboBox.Constructor = ComboBox;
    function onBodyDown(event) {
        if (!(event.target.id == "menuBtn" || event.target.id == "menuContent" || $(event.target).parents("#menuContent").length > 0)) {
            hideMenu();
        }
    }
})(jQuery);
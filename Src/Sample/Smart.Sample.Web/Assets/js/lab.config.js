var SUPPORT_JSON = (typeof (JSON) !== "undefined");
var IS_IE = window.navigator.msPointerEnabled || (document.all && document.querySelector); //8-11
var IS_OLD_IE = document.all && !document.addEventListener; //8 and below
var IS_CHROME = window.navigator.userAgent.indexOf("Chrome") !== -1;
var js = function (name) {
    var _scripts = {
        "autosize": "/assets/js/forms/jquery.autosize.min.js",
        "chosen": "/assets/js/forms/chosen.jquery.min.js",
        "colorpicker": "/assets/js/forms/bootstrap-colorpicker.min.js",
        "datepicker": "/assets/js/forms/date-time/bootstrap-datepicker.min.js",
        "editable": ["/assets/js/bootstrap/bootstrap-editable.min.js", "/assets/js/ace/ace-editable.min.js"],
        "datetimepicker": "/assets/js/forms/date-time/bootstrap-datetimepicker.min.js",
        "daterangepicker": "/assets/js/forms/date-time/daterangepicker.min.js",
        "dropzone": "/assets/js/forms/dropzone.min.js",
        "easypiechart": "/assets/js/chart/jquery.easypiechart.min.js",
        "hotkeys": "/assets/js/components/jquery.hotkeys.min.js",
        "flot": "/assets/js/chart/flot/jquery.flot.min.js",
        "flot-pie": "/assets/js/chart/flot/jquery.flot.pie.min.js",
        "flot-resize": "/assets/js/chart/flot/jquery.flot.resize.min.js",
        "fullcalendar": "/assets/js/fullcalendar/fullcalendar.min.js",
        "gritter": "/assets/js/components/jquery.gritter.min.js",
        "jqgrid": ["/assets/js/grid/jqgrid/grid.locale-cn.js", "/assets/js/grid/jqgrid/jquery.jqgrid.min.js", "/assets/js/grid/jqgrid/jquery.jqgrid.ext.js"],
        "jquerycui": "/assets/js/jqueryui/jquery-ui.custom.min.js",
        "knob": "/assets/js/forms/jquery.knob.min.js",
        "inputlimiter": "/assets/js/forms/jquery.inputlimiter.1.3.1.min.js",
        "maskedinput": "/assets/js/forms/jquery.maskedinput.min.js",
        "mobile": "/assets/js/mobile/jquery.mobile.custom.min.js",
        "moment": "/assets/js/forms/date-time/moment.min.js",
        "prettify": "/assets/js/tools/prettify.min.js",
        "select2": "/assets/js/forms/select2.min.js",
        "sparkline": "/assets/js/chart/jquery.sparkline.min.js",
        "spin": "/assets/js/components/spin.js",
        "spinner": "/assets/js/forms/fuelux/fuelux.spinner.min.js",
        "tag": "/assets/js/bootstrap/bootstrap-tag.min.js",
        "timepicker": "/assets/js/forms/date-time/bootstrap-timepicker.min.js",
        "tree": "/assets/js/forms/fuelux/fuelux.tree.min.js",
        "validate": ["/assets/js/validate/jquery.validate.min.js", "/assets/js/validate/additional-methods.min.js", "/assets/js/validate/jquery.validate.unobtrusive.min.js"],
        //"validate-unob": "/assets/js/validate/jquery.validate.unobtrusive.min.js",
        "wizard": "/assets/js/forms/fuelux/fuelux.wizard.min.js",
        "wysiwyg": "/assets/js/editor/bootstrap-wysiwyg.min.js",
        "ztree": "/assets/js/ztree/jquery.ztree.all.min.js",
    };
    var files = [];
    var rets = [];
    if (arguments.length > 1) {
        files = arguments;
    } else if (typeof name == "array") {
        files = name;
    } else if (name != undefined) {
        return _scripts[name];
    }
    for (var i = 0; i < files.length; i++) {
        var item = _scripts[files[i]];
        if (typeof item == "array") {
            for (var j = 0; j < item.length; j++) {
                rets.push(item[j]);
            }
        } else {
            rets.push(item);
        }
    }
    return rets;
};

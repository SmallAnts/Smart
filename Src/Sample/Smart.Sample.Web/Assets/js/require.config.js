var SUPPORT_JSON = (typeof (JSON) !== "undefined");
var IS_IE = window.navigator.msPointerEnabled || (document.all && document.querySelector); //8-11
var IS_OLD_IE = document.all && !document.addEventListener; //8 and below
var IS_CHROME = window.navigator.userAgent.indexOf("Chrome") !== -1
require.config({
    baseUrl: "/assets/js",
    //urlArgs: "bust=" + (new Date()).getTime(), //debug
    paths: {
        "ace": "ace/ace.min",
        "ace-editable": "ace/ace-editable.min",
        "ace-elements": "ace/ace-elements.min",
        "ace-extra": "ace/ace-extra.min",
        "bootstrap": "bootstrap/bootstrap.min",
        "chosen": "forms/chosen.jquery.min",
        "common": "common",
        "css": "require/css.min",
        "easypiechart": "chart/jquery.easypiechart.min",
        "mobile": "mobile/jquery.mobile.custom.min",
        "jquery": IS_IE || IS_OLD_IE ? "jquery1.min" : "jquery2.min",
        "signalr": "jquery.signalR-2.2.0.min",
        "validate": "validate/jquery.validate.min",
        "validate-unob": "validate/jquery.validate.unobtrusive.min",

        //"swal": IS_OLD_IE ? "dialog/swal-ie" : "dialog/sweetalert.min",
    },
    //在任何模块之前，都先载入这个模块
    map: {
        "*": {
            "css": "require/css"
        }
    },
    //依赖配置
    shim: {
        "ace": {
            deps: ["bootstrap"],
            exports: "ace"
        },
        "ace-el": ["ace"],
        "bootstrap": ["jquery"],
        "common": ["ace"],
        "easypiechart": {
            deps: ["jquery"],
        },
        "jquery.validate.min": ["jquery"],
        "jquery.validate.min": ["jquery"],
        "jquery.validate.unobtrusive.min": ["jquery.validate.min"],
        "signalr": ["jquery"],
        "/signalr/hubs": ["signalr"],
        "validate": ["jquery.validate.unobtrusive.min"],
    }
});

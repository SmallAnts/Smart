$(function () {
    $("body").on('expanded.pushMenu', set_sidebar_data).on("collapsed.pushMenu", set_sidebar_data);
    var sidebar_collapse = window.localStorage.getItem("sidebar-collapse");
    if (sidebar_collapse === "true") {
        $("body").addClass("sidebar-collapse");
    }
    function set_sidebar_data() {
        var data = $("body").hasClass("sidebar-collapse");
        window.localStorage.setItem("sidebar-collapse", data.toString());
    }

    if ($.jgrid && $.jgrid.defaults) {
        $.jgrid.defaults.width = "auto";
        $.jgrid.defaults.styleUI = 'Bootstrap';
    }
});
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
function formatDate(str) {
    if (!str) return "";
    return formatDateTime(str).substr(0, 10);
}
function formatDateTime(str) {
    if (!str) return "";
    var d = eval('new ' + str.substr(1, str.length - 2));
    var ar_date = [d.getFullYear(), d.getMonth() + 1, d.getDate(), d.getHours, d.getMinutes, d.getSeconds()];
    for (var i = 0; i < ar_date.length; i++) ar_date[i] = dFormat(ar_date[i]);
    return ar_date.slice(0, 3).join('-') + ' ' + ar_date.slice(3).join(':');
    function dFormat(i) { return i < 10 ? "0" + i.toString() : i; }
}
function validateForm() {
    return $("form").validate().form();
}
function defaultSubmit() {
    if (!validateForm()) {
        return false;
    } else {
        $(this).button('loading');
        //this.disabled = true;
    }
}
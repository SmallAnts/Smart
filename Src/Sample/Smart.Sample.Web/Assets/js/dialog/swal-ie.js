var sweetAlert = swal = function () {
    var options = arguments[0];
    if (typeof options == "string") { // Ex: swal("Hello", "Just testing", "info");
        alert(options + "\r\n" + (arguments[1] || ''));
    }
    else { // Ex: swal({ title:"Hello", text: "Just testing", type: "info" });
        var ret = true;
        var msg = options.title + "\r\n" + (options.text || '');
        if (options.showCancelButton) {
            ret = confirm(msg);
        } else {
            alert(msg);
        }
        if (arguments[1] && typeof arguments[1] == "function") {
            arguments[1](ret);
        }
    }
}
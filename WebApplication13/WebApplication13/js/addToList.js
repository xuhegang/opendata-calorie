$(document).ready(function () {
    $("#button1").click(function () {
        
        var rid = this.className;
        //window.location.href = "/Home/Add";
        htmlobj = $.ajax({ url: "/Home/Add?rid=" + rid, async: false });
        //this.attr('disabled', true);
    })
})
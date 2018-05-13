$(document).ready(function () {
    $(".rectype").click(function () {
        var buttonval = this.innerHTML;
        console.log(buttonval);
        $.ajaxSetup({
            async: false
        });
        $("#insertpoint").load("/Home/InitialRec?search=" + encodeURI(buttonval));
        var figuretype = document.getElementsByClassName("figuretype");
        var blanktype = document.getElementsByClassName("blanktype")
        var recipeswrapper = document.getElementsByClassName("rectype");
        var rid;
        for (let i = 0; i < figuretype.length; i++) {
            figuretype[i].onclick = function (event) {

                event.stopPropagation();
                event.preventDefault();
                rid = figuretype[i].nextElementSibling.firstElementChild.className;
                window.location.href = "/Home/Detail?rid=" + rid;
            }
        }
        for (let j = 0; j < blanktype.length; j++) {
            blanktype[j].onclick = function (event) {
                rid = blanktype[j].firstElementChild.className;
                event.stopPropagation();
                event.preventDefault();
                //alert("blank");
                window.location.href = "/Home/Detail?rid=" + rid;
            }
        }



    });
    $("#searchform").submit(function (event) {
        var event = event || window.event;
        event.preventDefault(); // 兼容标准浏览器
        window.event.returnValue = false; // 兼容IE6~8
        var inputval = $("#searchinput").val();
        console.log(inputval);
        $.ajaxSetup({
            async: false
        });
        $("#insertpoint").load("/Home/InitialRec?search=" + encodeURI(inputval));
        var figuretype = document.getElementsByClassName("figuretype");
        var blanktype = document.getElementsByClassName("blanktype")
        var recipeswrapper = document.getElementsByClassName("rectype");
        var rid;
        for (let i = 0; i < figuretype.length; i++) {
            figuretype[i].onclick = function (event) {

                event.stopPropagation();
                event.preventDefault();
                rid = figuretype[i].nextElementSibling.firstElementChild.className;
                window.location.href = "/Home/Detail?rid=" + rid;
            }
        }
        for (let j = 0; j < blanktype.length; j++) {
            blanktype[j].onclick = function (event) {
                rid = blanktype[j].firstElementChild.className;
                event.stopPropagation();
                event.preventDefault();
                //alert("blank");
                window.location.href = "/Home/Detail?rid=" + rid;
            }
        }

    });
});
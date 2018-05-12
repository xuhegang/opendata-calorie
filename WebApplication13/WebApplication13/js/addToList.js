$(document).ready(function () {
    var flag = false; //flag记录添加还是删除
    var total = 0;//总卡路里
    $("#check").click(function () {
        var calorie = $("#rList").attr("data-calorie");
        if (!flag) {//添加的情况
            var rid = $("#button1").attr("class"); //菜的id
            htmlobj = $.ajax({ url: "/Home/Add?rid=" + rid, async: false }); //发送到controller
            var spTitle = $("#spTitle").text();//菜名
            if ($("#rList").children().length == 0) {//list里没东西的话
                total += Number(calorie);
                $("<li class='Rli' id='rLi' data-title=" + spTitle +  "><a href='#'>" + spTitle + ": " + calorie + "KCal</a></li>").appendTo($("#rList"));
                $("<li class='Rli' id='rLi'><a href='#'>Total calorie is " + total + " KCal</a></li>").appendTo($("#rList"));
            }
            else {//list有菜
                var isExist = false;
                total = Number($("#rLi").attr("data-totalCalorie"));
                for (var i = 0; i < $("#rLi").children().length; i++) {
                    //console.log($("#rLi").attr("data-title"));
                    if (spTitle == $("#rLi").attr("data-title")) {
                        isExist = true;
                        break;
                    }
                }
                if (!isExist) {
                    total += Number(calorie);
                    $("<li class='Rli' id=" + spTitle + "><a href='#'>" + spTitle + ": " + calorie + "KCal</a></li>").insertBefore($("#rList").children().last());
                    var lastline = $("#rList").children().last().text("Total calorie is " + total + " KCal");
                }
            }
            flag = !flag;
        }
        else {
            total -= Number(calorie);
            if ($("#rList").children().length == 2) {//list只有两行
                $("#rList").children(0).remove();
                $("#rList").children(1).remove();
            }
            else {
                //var spTitle = $("#spTitle").text();
                $("#rList").children().last().prev().remove();
                $("#rList").children().last().text("Total calorie is " + total + " KCal");
            }
            var rid = $("#button1").attr("class");
            htmlobj = $.ajax({ url: "/Home/Remove?rid=" + rid, async: false });
            
            flag = !flag;
        }
       
        
    })
})
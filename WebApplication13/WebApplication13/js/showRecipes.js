window.onload = function () {
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
    for (let k = 0; k < recipeswrapper.length; k++) {
        recipeswrapper[k].onclick = function (event) {
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
        }
    }
}
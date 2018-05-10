window.onload = function () {
    var figuretype = document.getelementsbyclassname("figuretype");
    var blanktype = document.getelementsbyclassname("blanktype")
    var recipeswrapper = document.getelementsbyclassname("recipeswrapper");
    for (var i = 0; i < figuretype.length; i++) {
        figuretype[i].onclick = function (event) {
            event.stoppropagation();
            event.preventdefault();
        }
    }
    for (var j = 0; j < blanktype.length; j++) {
        blanktype[j].onclick = function (event) {
            event.stoppropagation();
            event.preventdefault();
            alert("blank")
        }
    }
    recipeswrapper.onclick = function (event) {
       console.log(event.target);
    }
    for (var i = 0; i < recipeswrapper.length; i++) {
        recipeswrapper[0].addeventlistener("click", function () {
            alert("hello");
        });
    }
    
}
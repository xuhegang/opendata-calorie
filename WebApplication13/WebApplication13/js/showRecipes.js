window.onload = function () {
    var figuretype = document.getElementsByClassName("figuretype");
    var blanktype = document.getElementsByClassName("blanktype")
    var recipeswrapper = document.getElementsByClassName("recipeswrapper");
    for (var i = 0; i < figuretype.length; i++) {
        figuretype[i].onclick = function (event) {
            event.stopPropagation();
            event.preventDefault();
        }
    }
    for (var j = 0; j < blanktype.length; j++) {
        blanktype[j].onclick = function (event) {
            event.stopPropagation();
            event.preventDefault();
            alert("blank")
        }
    }
  //  recipeswrapper.onclick = function (event) {
  //     console.log(event.target);
  //  }
    //for (var i = 0; i < recipeswrapper.length; i++) {
    //    recipeswrapper[0].addEventListener("click", function () {
    //        alert("hello");
   //     });
  //  }
    
}
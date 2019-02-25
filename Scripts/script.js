$(document).ready(function () {
    $('#buttonClick').click(function () {
        $('#formToShow').show();
        $('#tableToHide').hide();
        $('#buttonClick').hide();
        $('#profilClick').show()
    });
    $('#profilClick').click(function () {
        $('#formToShow').hide();
        $('#tableToHide').show();
        $('#profilClick').hide()
        $('#buttonClick').show();
    });
    $('#rdvShow').click(function () {
        $('#rdvToShow').show();
        $('#rdvHide').show(); 
    });
    $('#rdvHide').click(function () {
        $('#rdvToShow').hide();
        $('#rdvShow').show();
        $('#rdvHide').hide();
    });
    //var picked = $('#search').val();
    //$('#output').val(picked);
    //$('#search').focus(function () { });
    //$(function () {
    //    $('#search').keyup(function () {
    //        $('#searchBar').submit();
    //    });
    //    $('#search').focusin();
    //});
    
    // searchBar dynamique 
    //ne fonctionne pas correctement avec pagination car tri seulement la page visible
    //$("#search").keyup(function () {
    //    var value = $(this).val().toLowerCase();
    //    $(".tableFilter").filter(function () {
    //        $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
    //    });
    //});
});

//function pickSearching() {
//    if (sessionStorage.searching) {
//        if (document.getElementById('search').value != "") {
//            window.sessionStorage.searching = document.getElementById('search').value;
//            document.getElementById('searchBar').submit();
//        }
//        else {
//            //window.sessionStorage.searching = "";
//            document.getElementById('searchBar').submit();
//        }
//    }
//    else {
//        document.getElementById("search").value = "web storage don't work";
//    }
//    document.getElementById('search').focus();
//    document.getElementById("search").value = sessionStorage.search;
//}


function startTime() {
    var today = new Date();
    var h = today.getHours();
    var m = today.getMinutes();
    var s = today.getSeconds();
    m = checkTime(m); s = checkTime(s);
    document.getElementById('todayHour').innerHTML = h + ":" + m + ":" + s;
    var t = setTimeout(startTime, 500);
}
function checkTime(i) {
    if (i < 10) { i = "0" + i }; // add zero in front of numbers < 10
    return i;
}
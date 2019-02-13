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

});
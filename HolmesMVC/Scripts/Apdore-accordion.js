$(document).ready(function() {

    $("#accordion > li.li-bar").click(function() {
        var oldHtml = $(this).html();
        var newHtml = oldHtml.replace("▷", "|`|");
        newHtml = newHtml.replace("◢", "▷");
        newHtml = newHtml.replace("|`|", "◢");
        $(this).html(newHtml);
        $(this).next().slideToggle(150);
    });

    $("#accordion > li:first").click();

});
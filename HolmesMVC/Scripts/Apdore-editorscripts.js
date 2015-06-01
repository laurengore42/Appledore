$(document).ready(function () {
    // creates datepickers on pages that need datepickers (Edit and Create, mostly)
    $('.ui-datepicker').datepicker({
        dateFormat: "dd M yy", // don't change the date format, it breaks the datepicker
        showOtherMonths: false,
        autoSize: true,
        firstDay: 1
    });

    // updates season dropdown when adaptation changes
    $('#AdaptBox').on("change", function () {
        var url = $(this).data('url');
        var value = $(this).val();
        $('#SeasonOut').load(url, { AdaptId: value });
    });
});
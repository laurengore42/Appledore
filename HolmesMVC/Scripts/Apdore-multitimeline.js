function recalculateStartAndEndYears() {
    var startYear = 9999;
    var startMonth = 1;
    var endYear = 0;
    var endMonth = 12;

    $('canvas.timelineCanvas:visible').each(function () {
        var firstEpisode = this.attributes["data-firstepisode"].value;
        var lastEpisode = this.attributes["data-lastepisode"].value;
        if (firstEpisode < startYear) {
            startYear = firstEpisode;
        }
        if (lastEpisode > endYear) {
            endYear = lastEpisode;
        }
    });

    $('canvas.timelineCanvas').each(function () {
        this.attributes["data-startyear"].value = startYear;
        this.attributes["data-startmonth"].value = startMonth;
        this.attributes["data-endyear"].value = endYear;
        this.attributes["data-endmonth"].value = endMonth;
    });
}

$(document).ready(function () {
    $('select.adaptTimelineSelect').change(function () {
        $('div.timelineDiv').hide();
        $('canvas.timelineCanvas').hide();
        $('select.adaptTimelineSelect').each(function () {
            var adaptID = this.options[this.selectedIndex].value;
            if (adaptID > 0) {
                var divID = '#timelinediv' + adaptID;
                var canvasID = '#timeline' + adaptID;
                $(divID).show();
                $(canvasID).show();
                var currentStartYear = $(canvasID)[0].attributes["data-firstepisode"].value;
                var currentEndYear = $(canvasID)[0].attributes["data-lastepisode"].value;
            }
        });
        recalculateStartAndEndYears();
        redrawCanvases();
    });
});
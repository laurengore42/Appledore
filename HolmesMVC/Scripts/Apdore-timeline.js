function roundByFives(value, up) {
    if (value % 5 === 0) {
        return value;
    }
    if (up) {
        return (Number)(value) + (Number)(5 - value % 5);
    } else {
        return (Number)(value) - (Number)(value % 5);
    }
}

function drawEpLine(startRoundedDown, year, month, ctx, blockWidth, heightRatio, color) {
    ctx.fillStyle = color;
    ctx.fillRect(blockWidth * (year - startRoundedDown + (month - 1) / 12) / 5, 0, (1 / 60) * blockWidth, blockWidth * heightRatio);
}

function drawBox(startYear, startMonth, endYear, endMonth, ctx, blockWidth, heightRatio, color) {
    // takes an endYear which has ALREADY been ++
    // so for a show airing in 1989 only, this is '1989, 1990'

    var startRoundedDown = roundByFives(startYear, false);
    // padding
    if (startYear === startRoundedDown) {
        startRoundedDown -= 5;
    }

    // adjust for months
    startYear = (Number)(startYear) + (startMonth / 12);
    endYear = (Number)(endYear) - (1 - (endMonth / 12));

    // inclusive
    startYear -= 1 / 12;

    // draw adaptation duration box
    ctx.fillStyle = color;
    ctx.fillRect(blockWidth * (startYear - startRoundedDown) / 5, 0, ((endYear - startYear) / 5) * blockWidth, blockWidth * heightRatio);
}

function drawTimeline(startYear, startMonth, endYear, endMonth, canvas, ctx, episodes, spotty) {
    // because episodes in 1988, 1989 and 1990 is three years,
    // but 1990 - 1988 = 2
    endYear++;

    var startRoundedDown = roundByFives(startYear, false);
    var endRoundedUp = roundByFives(endYear, true);

    // padding
    if (endYear === endRoundedUp) {
        endRoundedUp += 5;
    }
    if (startYear === startRoundedDown) {
        startRoundedDown -= 5;
    }
    var outerBlocks = (endRoundedUp - startRoundedDown) / 5;

    var yearNow = (new Date()).getFullYear();
    var monthNow = (new Date()).getMonth();
    yearNow = (Number)(yearNow);
    monthNow = (Number)(monthNow + 1);
    if (startRoundedDown + (outerBlocks * 5) >= yearNow
        && endRoundedUp - 5 >= yearNow) { // there's a spare block in the Future
        outerBlocks--;
        endRoundedUp -= 5;
    }

    var markerHeight = 0.3125; // ratio to block height
    var markerWidth = 0.025; // ratio to block width

    canvas.width = 800; // for now
    var blockWidth = canvas.width / outerBlocks;

    // sanity check
    var heightLim = 15; // just over the height of a line of text, ideally
    var widthLim = 50 * 5; // eyeballing it
    if (blockWidth > widthLim) {
        blockWidth = widthLim;
        canvas.width = outerBlocks * blockWidth;
    }
    var heightRatio = heightLim / (0.3 * blockWidth);

    canvas.height = blockWidth * heightRatio * 1.3; // one row of blocks, and space for text

    // don't draw into future
    if (endRoundedUp > yearNow) {
        outerBlocks -= ((endRoundedUp - 1) - ((yearNow - 1) + monthNow/12)) / 5;
    }

    // draw outer timeline box
    ctx.strokeStyle = "#aaaaaa";
    ctx.lineWidth = 1;
    ctx.strokeRect(0, 0, outerBlocks * blockWidth, blockWidth * heightRatio);

    var color = 'green';
    if (spotty === true) {
        for (var j = 0; j < episodes.length; j += 2) {
            drawEpLine(startRoundedDown, episodes[j], episodes[j + 1], ctx, blockWidth, heightRatio, color);
        }
    } else {
        drawBox(startYear, startMonth, endYear, endMonth, ctx, blockWidth, heightRatio, color);
    }

    // shut up, Resharper
    var i;

    // little one-year markers
    ctx.fillStyle = "#888888";
    var newMarkerHeight = markerHeight * 0.3;
    var newMarkerWidth = markerWidth * 0.5;
    for (i = 0; i <= outerBlocks * 5; i++) {
        ctx.fillRect(i * (blockWidth / 5) - (blockWidth * newMarkerWidth / 2), blockWidth * heightRatio * (1 - newMarkerHeight), blockWidth * newMarkerWidth, blockWidth * heightRatio * newMarkerHeight);
    }

    // five-year markers over the top
    ctx.fillStyle = "#333333";
    for (i = 0; i <= outerBlocks; i++) {
        ctx.fillRect(i * blockWidth - (blockWidth * markerWidth) / 2, blockWidth * heightRatio * (1 - markerHeight), blockWidth * markerWidth, blockWidth * heightRatio * markerHeight);
    }

    // text
    ctx.fillStyle = "#333333";
    ctx.font = ".85em sans-serif";
    for (i = 0; i <= outerBlocks; i++) {
        var newYear = (Number)(startRoundedDown) + (Number)(i * 5);
        ctx.fillText(newYear, i * blockWidth, blockWidth * heightRatio * 1.3);
    }
}

function redrawCanvases() {
    $('canvas.timelineCanvas').each(function () {
        var canvas = this;
        var ctx = canvas.getContext('2d');
        var startYear = canvas.attributes["data-startyear"].value;
        var startMonth = canvas.attributes["data-startmonth"].value;
        var endYear = canvas.attributes["data-endyear"].value;
        var endMonth = canvas.attributes["data-endmonth"].value;
        var spottyValues = JSON.parse(canvas.attributes["data-spottyvalues"].value);

        var isSpotty = true;
        if (canvas.attributes["data-isspotty"]) {
            isSpotty = canvas.attributes["data-isspotty"].value === "true";
        }
        drawTimeline(startYear, startMonth, endYear, endMonth, canvas, ctx, spottyValues, isSpotty);
    });
}

$(document).ready(function () {
    redrawCanvases();
});
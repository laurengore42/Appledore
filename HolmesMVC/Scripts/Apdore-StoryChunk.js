$(document).ready(function () {
    $.ajax(
    {
        async: false,
        type: "GET",
        contentType: "application/xml",
        dataType: "text",
        url: "/Story/StoryXml",
        data: "storyCode=" + location.pathname.toLowerCase().replace("/story/", "").toUpperCase(),
        success: function (content) {
            $('#hiddenStory').html(content);
        },
        error: function (xhr, status, errorThrown) {
            $('#hiddenStory').html(xhr.responseText);
        }
    });

    var chunkSection = $('.storyChunk');
    var storySection = $('#hiddenStory');

    // This is the text and the tags
    var wholeStory = storySection.html();

    // Replace line-break tags with *¦
    // Kill all other tags
    var storyWithoutTags = htmlTrim(wholeStory);

    // Take chunk out of story
    var qd = {};
    location.search.substr(1).split("&").forEach(function (item) { var s = item.split("="), k = s[0], v = s[1] && decodeURIComponent(s[1]); (k in qd) ? qd[k].push(v) : qd[k] = [v] });
    var storyChunk = storyWithoutTags.substring(parseInt(qd["start"]), parseInt(qd["start"]) + parseInt(qd["length"]));

    // Put line breaks back in to chunk
    var lineBreaker = storyChunk;
    while (lineBreaker.indexOf("*¦") > -1) {
        lineBreaker = lineBreaker.replace("*¦", "</p><p>");
    }

    // Print chunk
    chunkSection.html("<p>" + lineBreaker + "</p>");
});
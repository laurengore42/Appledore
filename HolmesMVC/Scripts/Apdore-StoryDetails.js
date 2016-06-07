// thanks StackOverflow user karim79
function getSelectedText() {
    if (window.getSelection) {
        return window.getSelection().toString();
    } else if (document.selection) {
        return document.selection.createRange().text;
    }
    return '';
}

$(document).ready(function () {
    $.ajax(
    {
        async: true,
        type: "GET",
        contentType: "application/xml",
        dataType: "text",
        url: "/Story/StoryXml",
        data: "storyCode=" + location.pathname.toLowerCase().replace("/story/", "").toUpperCase(),
        success: function (content) {
            $('.story').html(content);
        },
        error: function (xhr, status, errorThrown) {
            $('.story').html(xhr.responseText);
        }
    });

    $('.story').mousemove(function (event) {
        var textChunk = getSelectedText();
        var button = $('.selButton');

        if (textChunk.length <= 0) {
            button.hide();
            return;
        }

        button.css({
            "position": "fixed",
            "left": "2em",
            "top": "2em"
        });

        button.show();
    });

    $('.selButton').click(function () {
        var storySection = $('.story');

        var textChunk = getSelectedText();
        if (textChunk.length <= 0) {
            return;
        }

        // Get whole story, full of misc tags
        var wholeStory = storySection.html();

        // Reduce story to text and *¦ coded line breaks
        var storyWithoutTags = htmlTrim(wholeStory);

        // Trim the chunk similarly
        textChunk = htmlTrim(textChunk);

        // Locate the chunk within the story
        var chunkStart = storyWithoutTags.indexOf(textChunk);
        var chunkLength = textChunk.length;

        // Go to chunk page
        var newUrl = location.pathname + "?start=" + chunkStart + "&length=" + chunkLength;
        if (chunkStart > -1) {
            window.open(newUrl);
        } else {
            alert("Chunk failed! Start " + chunkStart + " length " + chunkLength + ".");
        }
    });
});
// thanks StackOverflow user karim79
function getSelectedText() {
    if (window.getSelection) {
        return window.getSelection().toString();
    } else if (document.selection) {
        return document.selection.createRange().text;
    }
    return '';
}

function htmlTrim(wholeStory) {
    var tagless = wholeStory;

    // special case for line breaks
    var brRe = /<br.*?>/gi;
    tagless = tagless.replace(brRe, "*¦");
    var pRe = /<\/?p.*?>/gi;
    tagless = tagless.replace(pRe, "*¦");
    var divRe = /<\/?div.*?>/gi;
    tagless = tagless.replace(divRe, "*¦");

    // kill all tags
    var re = /<.*?>/gi;
    tagless = tagless.replace(re, "");

    // kill newlines
    tagless = tagless.replace(/\r/g, '');
    tagless = tagless.replace(/\n{2,}/g, '*¦');
    tagless = tagless.replace(/\n/g, '*¦');

    // tidy
    while (tagless.indexOf("*¦*¦") > -1) {
        tagless = tagless.replace("*¦*¦", "*¦");
    }
    while (tagless.indexOf("*¦ ") > -1) {
        tagless = tagless.replace("*¦ ", "*¦");
    }
    while (tagless.indexOf(" *¦") > -1) {
        tagless = tagless.replace(" *¦", "*¦");
    }

    // whitespace handling
    while (tagless.indexOf('  ') > -1) {
        tagless = tagless.replace('  ', ' ');
    }
    tagless = tagless.trim();

    while (tagless.indexOf('*¦') === 0) {
        // Sometimes this has to be done twice. Don't ask me why
        // If you don't trim this off, you get an off-by-2 error when chunking
        tagless = tagless.substring(2, tagless.length);
    }

    return tagless;
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
            $('#storyLoader').html(content);
        },
        error: function (xhr, status, errorThrown) {
            $('#storyLoader').html(xhr.responseText);
        }
        });

    $.ajax(
        {
            async: true,
            type: "GET",
            contentType: "application/xml",
            dataType: "text",
            url: "/Story/StoryAdapteds",
            data: "storyCode=" + location.pathname.toLowerCase().replace("/story/", "").toUpperCase(),
            success: function (content) {
                $('#adaptedsLoader').html(content);
            },
            error: function (xhr, status, errorThrown) {
                $('#adaptedsLoader').html(xhr.responseText);
            }
        }); 

    $('#storyLoader').mousemove(function (event) {
        var textChunk = getSelectedText();
        var button = $('#selButton');

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

    $('#selButton').click(function () {
        var storySection = $('#storyLoader');

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
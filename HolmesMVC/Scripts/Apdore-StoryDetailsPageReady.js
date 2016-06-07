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
});
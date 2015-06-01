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
    
    while (tagless.indexOf('*¦') == 0) {
        // Sometimes this has to be done twice. Don't ask me why
        // If you don't trim this off, you get an off-by-2 error when chunking
        tagless = tagless.substring(2, tagless.length);
    }

    return tagless;
}

// thanks StackOverflow user karim79
function getSelectedText() {
    if (window.getSelection) {
        return window.getSelection().toString();
    } else if (document.selection) {
        return document.selection.createRange().text;
    }
    return '';
}
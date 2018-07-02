function ToggleActorVisible() {
    $("#OldActor").toggle();
    $("#AddActor").toggle();
}

function ToggleCharVisible() {
    $("#OldChar").toggle();
    $("#AddChar").toggle();
}

function addParam(currentLocation, paramName, paramValue) {
    var currUrl = String(currentLocation);
    var gotQuery = currUrl.indexOf("?");
    var newUrl;
    if (gotQuery > -1) {
        if (paramName != "episode") {
            currUrl = killParam(currUrl, paramName);
        }
        newUrl = currUrl + "&" + paramName + "=" + paramValue;
    } else {
        newUrl = currUrl + "?" + paramName + "=" + paramValue;
    }
    return(newUrl);
}

function killParam(currentLocation, paramName) {
    var currUrl = String(location);
    var newUrl = currUrl;
    newUrl = newUrl.replace(paramName, "regexregex");
    var i = 0;
    while (newUrl.indexOf("regexregex") > 0 && i < 10) {
        var matchedParam = newUrl.match(/regexregex=\d+&?/i);
        newUrl = newUrl.replace(matchedParam, "");
        i++;
    }
    // trailing punctuation
    while (newUrl.substring(newUrl.length - 1, newUrl.length) == "?"
        || newUrl.substring(newUrl.length - 1, newUrl.length) == "&") {
        newUrl = newUrl.substring(0, newUrl.length - 1);
    }
    return (newUrl);
}

function SaveNewActor() {
    $("#actorsave").disabled = true;
    $("#actorsave").hide();
    if ($("#actorsurname") == "") {
        alert("Please enter a surname.");
        return;
    }

    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajaxPrefilter(function (options, originalOptions) {
        if (options.type.toUpperCase() == "POST") {
            options.data = $.param($.extend(originalOptions.data, { __RequestVerificationToken: token }));
        }
    });

    $.ajax({
        url: '/Actor/CreateShort',
        type: 'POST',
        data: {
            forename: $("#actorforename").val(),
            surname: $("#actorsurname").val(),
        },
        success: function (data) {
            location.replace(addParam(location, "actorid", data));
        },
        error: function (xhr, textStatus, errorThrown) {
            var sErrMsg = "";
            sErrMsg += "paramEdit form submit error ";
            sErrMsg += "\n\n" + " - textStatus :" + textStatus;
            sErrMsg += "\n\n" + " - Error Status :" + xhr.status;
            sErrMsg += "\n\n" + " - Error type :" + errorThrown;
            sErrMsg += "\n\n" + " - Error message :" + xhr.responseText;

            if (window.console) console.log(sErrMsg);
            alert(sErrMsg);
        }
    });
}

function SaveNewChar() {
    $("#charsave").disabled = true;
    $("#charsave").hide();
    if ($("#charsurname") == "") {
        alert("Please enter a surname.");
        return;
    }

    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajaxPrefilter(function (options, originalOptions) {
        if (options.type.toUpperCase() == "POST") {
            options.data = $.param($.extend(originalOptions.data, { __RequestVerificationToken: token }));
        }
    });

    $.ajax({
        url: '/Character/CreateShort',
        type: 'POST',
        data: {
            forename: $("#charforename").val(),
            surname: $("#charsurname").val(),
        },
        success: function (data) {
            location.replace(addParam(location, "characterid", data));
        },
        error: function (xhr, textStatus, errorThrown) {
            var sErrMsg = "";
            sErrMsg += "paramEdit form submit error ";
            sErrMsg += "\n\n" + " - textStatus :" + textStatus;
            sErrMsg += "\n\n" + " - Error Status :" + xhr.status;
            sErrMsg += "\n\n" + " - Error type :" + errorThrown;
            sErrMsg += "\n\n" + " - Error message :" + xhr.responseText;

            if (window.console) console.log(sErrMsg);
            alert(sErrMsg);
        }
    });
}
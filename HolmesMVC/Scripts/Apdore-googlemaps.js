var map;
var geocoder;
var pins = [];
var pinsChecked = false;

function placePins() {

    for (var i = 0; i < pins.length; i++) {
        pins[i].setMap(null);
    }
    pins = [];

    var setCenter = false;

    $('.actorPin').each(function () {
        var link = $(this).find('.actorLink').html();
        var birthplace = $(this).find('.actorBirthplace').text();

        // bias
        if (birthplace.indexOf(",") == -1) {
            birthplace = birthplace + ", UK";
        }

        geocoder.geocode({ 'address': birthplace }, function (results, status) {
            if (status == google.maps.GeocoderStatus.OK) {

                var pin = new google.maps.Marker({
                    position: results[0].geometry.location,
                    map: map
                });
                pins.push(pin);
                var infoWindow = new google.maps.InfoWindow({
                    content: link + ', ' + birthplace
                });
                google.maps.event.addListener(pin, "click", function () {
                    infoWindow.open(map, pin);
                });
                if (!setCenter) {
                    map.setCenter(results[0].geometry.location);
                    setCenter = true;
                }

            } else {
                alert("Geocode was not successful for the following reason: " + status);
            }
        });
    })

    var startLat = 51;
    var startLong = 0;
    var pinColor = "4aa3df";
    var pinImage = new google.maps.MarkerImage("http://chart.apis.google.com/chart?chst=d_map_pin_letter&chld=%E2%80%A2|" + pinColor,
        new google.maps.Size(21, 34),
        new google.maps.Point(0, 0),
        new google.maps.Point(10, 34));

    var startPos = new google.maps.LatLng(startLat, startLong);
    var pin = new google.maps.Marker({
        icon: pinImage,
        size: new google.maps.Size(28, 30),
        origin: new google.maps.Point(0, 0),
        anchor: new google.maps.Point(0, 15),
        draggable: false,
        position: startPos,
        map: map
    });
    pins.push(pin);
    map.setCenter(startPos);

    map.setZoom(16);
    if (pins.length > 0) {
        while (map.getZoom() > 5) {
            map.setZoom(map.getZoom() - 1);
            var bounds = map.getBounds();
            if (bounds != undefined) {
                if (bounds.contains(pins[0].getPosition())) {
                    break;
                }
            }
        }
    }
}

function initialize() {
    var latlng = new google.maps.LatLng(0, 0);
    var mapOptions = {
        zoom: 10,
        center: latlng
    };
    geocoder = new google.maps.Geocoder();
    map = new google.maps.Map(document.getElementById('map-canvas'), mapOptions);

    placePins();
}

$(document).ready(function () {
    initialize();
});
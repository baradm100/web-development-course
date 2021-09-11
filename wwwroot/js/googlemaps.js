let _markers = []
let _map


$(function initMap () {
    const myLatLng = { lat: 32.084694, lng: 34.7912818 };
    _map = new google.maps.Map(document.getElementById("google-map"), {
        zoom: 8,
        center: myLatLng,
    });

    _infoWindow = new google.maps.InfoWindow();
    $.ajax({
        url: "/Branches/GetJson",
    }).done(function (result) {
        $.each(result, function (key, value) {
            var title, lng, lat
            $.each(value, function (innerKey, innerValue) {
                if (innerKey === "address") {
                    $.each(innerValue, function (k, v) {
                        if (k === "latitude") {
                            lat = v
                        }
                        if (k === "longitude") {
                            lng = v
                        }
                    });
                }

                if (innerKey === "name") {
                    title = innerValue
                }

            });

            let marker = new google.maps.Marker({
                position: { lat: lat, lng: lng },
                title: title,
                map: _map
            });

            _markers.push(marker)
            marker.addListener("click", () => {
                showInfoWindow(marker);
            });
        });
    });
});

function showInfoWindow(marker) {
    _infoWindow.close();
    _infoWindow.setContent(marker.getTitle());
    _infoWindow.open(marker.getMap(), marker);
}

$('[marker-position-show]').click(function () {
    console.log("recoganize")
    var marker = _markers[parseInt($(this).attr('marker-position-show'))]
    _map.panTo(marker.getPosition())
    showInfoWindow(marker)
    _map.setZoom(14)
});
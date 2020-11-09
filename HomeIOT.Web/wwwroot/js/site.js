// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function dateToTicks(date) {
    var jsDate = moment(date, "DD.MM.YYYY hh:mm:ss").toDate();
    return ((jsDate.getTime() * 10000) + 621355968000000000) - (jsDate.getTimezoneOffset() * 600000000);
}

function reloadChart(sensorId) {
    $.ajax({
        url: '/Sensors/DataChartComponent?sensor=' + sensorId + '&from=' + dateToTicks($("#chartFrom" + sensorId).val()) + '&to=' + dateToTicks($("#chartTo" + sensorId).val())
    })
        .done(function (data) {
            $('#dataChart' + sensorId).html(data);
            drawChart(sensorId);
        });
}

function favoriteSensor(sensorId) {
    var favorite = $('#imgFavorite' + sensorId).attr("src") == "/images/favorite.png";
    $.ajax({
        url: '/sensors/favorite?sensorId=' + sensorId + "&favorite=" + favorite
    }).done(function (data) {
        if (!favorite) {
            $('#imgFavorite' + sensorId).attr("src", "/images/favorite.png");

            if (window.location.pathname == '/')
                $('#sensorDetail' + sensorId).remove()
        } else {
            $('#imgFavorite' + sensorId).attr("src", "/images/unfavorite.png");
        }
    });
}

    function changeScoreboard(days) {
            var date = days.getAttribute("data-dateTime");
            var track = $('#dropdownMenuButton').text();

            $.ajax({
            type: "GET",
                url: "/Home/ScoreboardByCondition",
                data: {date: date, track: track },
                async: true,
                success: function (data) {
                    $('#div_to_load_PartialView').html(data);
                },
            });
        }

        function changeTrackStatistic(days) {
            var date = days.getAttribute("data-dateTime");

            $.ajax({
            type: "GET",
                url: "/Manager/TracksByCondition",
                data: {date: date },
                async: true,
                success: function (data) {
                    $('#div_to_load_PartialViewManager').html(data);
                },
            });
        }

        function changeCardStatistic(days) {
            var date = days.getAttribute("data-dateTime");

            $.ajax({
            type: "GET",
                url: "/Manager/CardsByCondition",
                data: {date: date },
                async: true,
                success: function (data) {
                    $('#div_to_load_PartialViewManagerCard').html(data);
                },
            });
        }

        $(function () {

            $("#SelectedUsedChip").change(function () {

                var t = $(this).val();

                $.ajax({
                    type: "GET",
                    url: "/Salesman/GetPersonId",
                    data: { id: t },
                    async: true
                });

            });
        });


        function changeValue(value) {
            var track = null;
            track = value.getAttribute("data-dateTime");
            console.log(track);
            $('#dropdownMenuButton').text(track);
        }

        function changeMap(map) {
            var mapName = map.getAttribute("data-dateTime");

            $.ajax({
            type: "GET",
                url: "/Home/ShowMap",
                data: {name: mapName },
                async: true,
                success: function (data) {
                    $('#div_to_load_PartialViewMap').html(data);
                },
            });
        }

$(document).ready(function () {
    changeTracks();
});


function changeTracks(dateFilter) {
    var cardDate;

    if (dateFilter === null) {
        cardDate = "Idag";
    }
    else {
        cardDate = dateFilter.getAttribute("data-dateTime");
    }

    var jsonTracks;
    var trackNames = [];
    var trackCount = [];

    $.ajax({
        type: 'post',
        url: '/Manager/GetTracks',
        data: {date: cardDate},
        async: false,
        success: function (data) {
            var result = data;
            jsonTracks = JSON.parse(result);
        }
    })

    jsonTracks.forEach(function (item) {
        trackNames.push(item.Name);
        trackCount.push(item.Count);
    });


    var chartContent = document.getElementById('div_to_load_PartialViewManager');
    chartContent.innerHTML = '&nbsp;';
    $('#div_to_load_PartialViewManager').append('<canvas id="myChart"></canvas>');

    let myChart = document.getElementById('myChart').getContext('2d');

    let popularTracks = new Chart(myChart, {
        type: 'bar',
        data: {
            labels: trackNames,
            datasets: [{
                label: 'Spårnamn',
                data: trackCount,
                backgroundColor: [
                    '#09428f',
                    '#0c5ac0',
                    '#062d60'
                ],
            }]
        },
        options: {
            legend: {
                position: 'right',
                labels: {
                    fontColor: '#000'
                }
            },
            scales: {
                yAxes: [{
                    display: true,
                    ticks: {
                        beginAtZero: true
                    }
                }]
            }
        }

    });
}

function changeCards(dateFilter) {
    var date;

    if (dateFilter === null) {
        date = "Idag";
    }
    else {
        date = dateFilter.getAttribute("data-dateTime");
    }

    var jsonCards;
    var cardNames = [];
    var cardCount = [];

    $.ajax({
        type: 'post',
        url: '/Manager/GetCards',
        data: { date: date },
        async: false,
        success: function (data) {
            var result = data;
            jsonCards = JSON.parse(result);
        }
    })

    jsonCards.forEach(function (item) {
        cardNames.push(item.Name);
        cardCount.push(item.Count);
    });


    var chartContent = document.getElementById('div_to_load_PartialViewManagerCard');
    chartContent.innerHTML = '&nbsp;';
    $('#div_to_load_PartialViewManagerCard').append('<canvas id="myChartCards"></canvas>');

    let myChart = document.getElementById('myChartCards').getContext('2d');

    let popularTracks = new Chart(myChart, {
        type: 'bar',
        data: {
            labels: cardNames,
            datasets: [{
                label: 'Korttyp',
                data: cardCount,
                backgroundColor: [
                    '#09428f',
                    '#094490',
                    '#062d60',
                    '#0f71f0',
                    '#3f8df3',
                    '#0c5ac0'
                ],
            }]
        },
        options: {
            legend: {
                position: 'right',
                labels: {
                    fontColor: '#000'
                }
            },
            scales: {
                yAxes: [{
                    display: true,
                    ticks: {
                        beginAtZero: true
                    }
                }]
            }
        }

    });
}


$(document).ready(function () {
    $("#SearchBtn").click(function () {
        var SearchBy = $("#SearchBy").val();
        var SearchValue = $("#SearchInput").val();
        var SetData = $("#DataSearching");
        console.log(SearchValue);
        SetData.html(SearchBy);
        $.ajax({
            type: "post",
            url: "/Manager/GetSearchingData?SearchBy=" + SearchBy + "&SearchValue=" + SearchValue,
            contenType: "html",
            success: function (result) {
                $.each(result, function (index, value) {
                    var Data = "<tr>" +
                        "<th>" + value.Id + "</th>" +
                        "<th>" + value.Efternamn + "</th>" +
                        //"<th>" + value.Efternamn + "</th>" +
                        //"<th>" + value.Postnummer + "</th>" +
                        //"<th>" + value.E - postadress + "</th>" +
                        "</tr>";
                    SetData.append(Data);
                });
            }
        });
    });
});
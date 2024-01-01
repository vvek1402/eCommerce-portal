$(document).ready(function () {


    $.ajax({
        type: "POST",
        url: "/admin/Admin/countsoldcatagory",
        success: function (result) {
            var options = {
                series: result.countList,
                chart: {
                    width: 380,
                    type: 'pie',
                },
                labels: result.catList,
                responsive: [{
                    breakpoint: 480,
                    options: {
                        chart: {
                            width: 200
                        },
                        legend: {
                            position: 'bottom'
                        }
                    }
                }]
            };

            var chart = new ApexCharts(document.querySelector("#chart"), options);
            chart.render();
        }
    });
});


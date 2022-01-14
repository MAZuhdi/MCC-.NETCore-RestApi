var gender_chart;
var university_chart;
$(document).ready(function () {
    $.ajax({
        url: 'https://localhost:44367/Api/Employees/GenderStat'
    }).done((result) => {
        console.log(result.data[0]);

        var gender_chart_options = {
            chart: {
                type: 'donut',
                toolbar: {
                    show: true,
                    offsetX: 0,
                    offsetY: 0,
                    tools: {
                        download: true,
                        selection: true,
                        zoom: true,
                        zoomin: true,
                        zoomout: true,
                        pan: true,
                        reset: true | '<img src="/static/icons/reset.png" width="20">',
                        customIcons: []
                    },
                    export: {
                        csv: {
                            filename: undefined,
                            columnDelimiter: ',',
                            headerCategory: 'category',
                            headerValue: 'value',
                            dateFormatter(timestamp) {
                                return new Date(timestamp).toDateString()
                            }
                        },
                        svg: {
                            filename: undefined,
                        },
                        png: {
                            filename: undefined,
                        }
                    },
                    autoSelected: 'zoom'
                }
            },
            series: [result.data[0].count, result.data[1].count],
            labels: ['Male', 'Female']
        }

        gender_chart = new ApexCharts(document.querySelector("#gender-chart"), gender_chart_options);
        gender_chart.render();

    }).fail((error) => {
        console.log(error);
        gender_chart.html = error;
    })

    $.ajax({
        url: 'https://localhost:44367/Api/Universities/UniversityStat'
    }).done((result) => {
        let dataCount = new Array();
        let dataUniversityName = new Array();
        console.log("INI Univ")
        console.log(result);
        $.each(result.data, function (key, val) {
            console.log(val);
            dataCount.push(val.count);
            dataUniversityName.push(val.universityName);
        });
        console.log(dataCount);
        console.log(dataUniversityName);
        var university_chart_options = {
            chart: {
                type: 'bar',
                toolbar: {
                    show: true,
                    offsetX: 0,
                    offsetY: 0,
                    tools: {
                        download: true,
                        selection: true,
                        zoom: true,
                        zoomin: true,
                        zoomout: true,
                        pan: true,
                        reset: true | '<img src="/static/icons/reset.png" width="20">',
                        customIcons: []
                    },
                    export: {
                        csv: {
                            filename: undefined,
                            columnDelimiter: ',',
                            headerCategory: 'category',
                            headerValue: 'value',
                            dateFormatter(timestamp) {
                                return new Date(timestamp).toDateString()
                            }
                        },
                        svg: {
                            filename: undefined,
                        },
                        png: {
                            filename: undefined,
                        }
                    },
                    autoSelected: 'zoom'
                }
            },
            series: [{
                name: 'count',
                data: dataCount
            }],
            xaxis: {
                tickPlacement: 'on',
                categories: dataUniversityName
            }
        }
        university_chart = new ApexCharts(document.querySelector("#university-chart"), university_chart_options);

        university_chart.render();

    }).fail((error) => {
        console.log(error);
        university_chart.html = error;
    })
});






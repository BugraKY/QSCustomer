$(document).ready(function () {
    $(() => {
        $('#pie').dxPieChart({
            size: {
                width: 500,
            },
            palette: 'bright',
            dataSource,
            series: [
                {
                    argumentField: 'country',
                    valueField: 'area',
                    label: {
                        visible: true,
                        connector: {
                            visible: true,
                            width: 1,
                        },
                    },
                },
            ],
            title: 'Area of Countries',
            animation: {
                easing: "linear",
                duration: 0,
                maxPointCountSupported: 0
            },
            onPointClick(e) {
                const point = e.target;

                toggleVisibility(point);
            },
            onLegendClick(e) {
                const arg = e.target;

                toggleVisibility(this.getAllSeries()[0].getPointsByArg(arg)[0]);
            },
        });
        function toggleVisibility(item) {
            if (item.isVisible()) {
                item.hide();
            } else {
                item.show();
            }
        }
    });
    const dataSource = [{
        country: 'Russia',
        area: 12,
    }, {
        country: 'Canada',
        area: 7,
    }, {
        country: 'USA',
        area: 7,
    }, {
        country: 'China',
        area: 7,
    }, {
        country: 'Brazil',
        area: 6,
    }, {
        country: 'Australia',
        area: 5,
    }, {
        country: 'India',
        area: 2,
    }, {
        country: 'Others',
        area: 55,
    }];
});
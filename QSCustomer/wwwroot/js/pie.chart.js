
function getPie(setPieUrl, isPrint) {
    if (isPrint) {
        $.ajax({
            type: "GET",
            url: setPieUrl,
            success: function (dataSource) {
                console.log("piesource: " + dataSource);
                //dataSource = dataSource._ProjectTotalsOnebyDate;

                $(() => {
                    $('#pie').dxPieChart({
                        palette: 'bright',
                        dataSource,
                        title: 'Hatalar - Nok Qty.',
                        legend: {
                            orientation: 'horizontal',
                            itemTextPosition: 'right',
                            horizontalAlignment: 'center',
                            verticalAlignment: 'bottom',
                            columnCount: 4,
                        },
                        diameter:0.6,
                        /*
                        size: {
                            height: '15vh',
                            width: '15vw'
                        },*/
                        export: {
                            enabled: false,
                        },
                        animation: {
                            duration: 0,
                            maxPointCountSupported: 0
                        },
                        series: [{
                            argumentField: 'definition',
                            valueField: 'rate',
                            label: {
                                visible: true,
                                connector: {
                                    visible: true,
                                    width: 0.5
                                },
                                valueField: 'percent',
                                font: {
                                    size: 12,
                                },
                                format: 'fixedPoint',
                                //precision:2,
                                customizeText(point) {
                                    return `${point.argumentText}: ${point.valueText}%`;
                                },
                            },
                            position: 'columns',
                            sizeGroup: 'pies',
                            /*
                            smallValuesGrouping: {
                                mode: 'smallValueThreshold',
                                threshold: 0,
                            },*/
                        }],
                    });
                });
                setTimeout(function () {
                    $(document).trigger('printer');
                }, 100);
            }
        });
    } else {
        $.ajax({
            type: "GET",
            url: setPieUrl,
            success: function (dataSource) {
                console.log("piesource: " + dataSource);
                //dataSource = dataSource._ProjectTotalsOnebyDate;

                $(() => {
                    $('#pie').dxPieChart({
                        palette: 'bright',
                        dataSource,
                        title: 'Hatalar - Nok Qty.',
                        legend: {
                            orientation: 'horizontal',
                            itemTextPosition: 'right',
                            horizontalAlignment: 'center',
                            verticalAlignment: 'bottom',
                            columnCount: 20,
                        },
                        size: {
                            height: 600,
                            width: 800
                        },
                        export: {
                            enabled: false,
                        },
                        animation: {
                            duration: 0,
                            maxPointCountSupported: 0
                        },
                        series: [{
                            argumentField: 'definition',
                            valueField: 'rate',
                            label: {
                                visible: true,
                                connector: {
                                    visible: true,
                                    width: 0.5
                                },
                                valueField: 'percent',
                                font: {
                                    size: 12,
                                },
                                format: 'fixedPoint',
                                //precision:2,
                                customizeText(point) {
                                    return `${point.argumentText}: ${point.valueText}%`;
                                },
                            },
                            position: 'columns',
                            sizeGroup: 'pies',
                            /*
                            smallValuesGrouping: {
                                mode: 'smallValueThreshold',
                                threshold: 0,
                            },*/
                        }],
                    });
                });

            }
        });
    }

}









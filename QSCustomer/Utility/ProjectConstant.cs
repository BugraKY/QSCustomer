using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QSCustomer.Utility
{
    public static class ProjectConstant
    {
        public static class PDFString
        {
            public const string Before = @"
<!DOCTYPE html>
<html lang='en' xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <meta http-equiv='Content-Type' content='text/html; charset=UTF-8;' />
    <link rel='stylesheet' href='/mdb5/css/mdb.min.css' />
    <link rel='stylesheet' href='~/css/barchart.css' />
    <link rel='stylesheet' href='/css/site.css' type='text/css' media='screen' runat='server' />
    <link rel='stylesheet' href='https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.1/css/all.min.css' />
    <link rel='stylesheet' href='https://fonts.googleapis.com/css?family=Roboto:300,400,500,700&display=swap' />



    <!--TEST-->
    <!--Dev Extreme StyleSheet-->
    <!-- Diagram and Gantt stylesheets -->
    <link href='https://cdnjs.cloudflare.com/ajax/libs/devexpress-diagram/2.1.37/dx-diagram.min.css' rel='stylesheet'>
    <link href='https://cdnjs.cloudflare.com/ajax/libs/devexpress-gantt/3.1.24/dx-gantt.min.css' rel='stylesheet'>
    <!-- Theme stylesheets (reference only one of them) -->
    <link href='https://cdnjs.cloudflare.com/ajax/libs/devextreme/21.2.3/css/dx.light.css' rel='stylesheet'>
    <script src='~/lib/jquery/dist/jquery.min.js'></script>
    <script src='https://cdnjs.cloudflare.com/ajax/libs/jqueryui/1.12.1/jquery-ui.min.js'></script>
    <script src='https://cdn.datatables.net/1.10.16/js/jquery.dataTables.min.js'></script>
    <script src='~/lib/bootstrap/dist/js/bootstrap.bundle.min.js'></script>
    <script src='~/js/site.js' asp-append-version='true'></script>
    <!-- Diagram and Gantt development stylesheets -->
    <!--Dev Extreme StyleSheet End-->
    <!--Dev Extreme-->
    <!-- Diagram and Gantt -->
    <script src='https://cdnjs.cloudflare.com/ajax/libs/devexpress-diagram/2.1.37/dx-diagram.min.js'></script>
    <script src='https://cdnjs.cloudflare.com/ajax/libs/devexpress-gantt/3.1.24/dx-gantt.min.js'></script>

    <!-- DevExtreme Quill (required by the HtmlEditor UI component) -->
    <script src='https://cdnjs.cloudflare.com/ajax/libs/devextreme-quill/1.5.5/dx-quill.min.js'></script>

    <!-- DevExtreme library -->
    <script src='https://cdnjs.cloudflare.com/ajax/libs/devextreme/21.2.3/js/dx.all.js'></script>

    <!-- DevExpress.AspNet.Data -->
    <script src='https://cdnjs.cloudflare.com/ajax/libs/devextreme-aspnet-data/2.8.6/dx.aspnet.data.min.js'></script>

    <style>
        table {
            font-size: 8.5px !important;
            font-weight: 900 !important;
            color: black !important;
            width: 100% !important;
            text-align:center;
            border-color:black!important;
        }

        .table > :not(caption) > * > * {
            padding: 0.2rem 0.2rem !important;
        }
/*
            table.rotate-table-grid {
                box-sizing: border-box;
                border-collapse: collapse;
            }

            .rotate-table-grid tr, .rotate-table-grid td, .rotate-table-grid th {
                border: 1px solid #ddd;
                position: relative;
                padding: 10px;
            }

                .rotate-table-grid th span {
                    transform-origin: 0 50%;
                    -moz-transform-origin: 0 50%;
                    -webkit-transform-origin: 0 50%;
                    transform: rotate(-90deg);
                    -moz-transform: rotate(-90deg);
                    -webkit-transform: rotate(-90deg);
                    white-space: nowrap;
                    display: block;
                    position: absolute;
                    bottom: 0;
                    left: 50%;
                }*/
/*
            table.rotate-table-grid {
                box-sizing: border-box;
                border-collapse: collapse;
            }

            .rotate-table-grid tr, .rotate-table-grid td, .rotate-table-grid th {
                border: 1px solid #ddd;
                position: relative;
                padding: 10px;
            }

                .rotate-table-grid th span {
                    transform-origin: 0 50%;
                    -moz-transform-origin: 0 50%;
                    -webkit-transform-origin: 0 50%;
                    writing-mode: vertical-rl;
                    -moz-writing-mode: vertical-rl;
                    -webkit-writing-mode: vertical-rl;
                    white-space: nowrap;
                    display: block;
                    position: absolute;
                    bottom: 0;
                    left: 50%;
                }*/

        .verticalTableHeader span {
            -webkit-writing-mode: vertical-rl;
            writing-mode: vertical-rl;
            white-space:nowrap;
            text-align: left;
            height: 95px;
            overflow: hidden; 
        }
    </style>
</head>
<body>

    <section class='container' id='renderedSection'>
";
            public const string After = @"

    </section>

</body>
</html>
";
        }
    }
}

var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/getAllReports/{id}",
            "type": "get",
            "data": {id}
        },
        "columns": [
            { "data": "id", "width": "15%" },
            { "data": "idOprArea", "width": "15%" },
            { "data": "idMusteri", "width": "15%" },
            { "data": "idProjeDurumu", "width": "15%" },
            { "data": "projeCode", "width": "15%" },
            { "data": "baslangicTarihi", "width": "15%" },
            { "data": "bitisTarihi", "width": "15%" },
            { "data": "projeAcan", "width": "15%" },
            { "data": "projeAcan1", "width": "15%" },
            { "data": "projeOlusturmaTarihi", "width": "15%" },
            { "data": "onayTarih", "width": "15%" },
            { "data": "onayDurumu", "width": "15%" },
            { "data": "onaySonTarih", "width": "15%" },
            { "data": "onayToplamPiece", "width": "15%" },
            { "data": "onayToplamDk", "width": "15%" },
            { "data": "onayToplamCost", "width": "15%" },
            { "data": "onayLastNo", "width": "15%" },
            { "data": "fiyatIdKontrolTipi", "width": "15%" },
            { "data": "fiyatIdTipi", "width": "15%" },
            { "data": "fiyatSaatUcreti", "width": "15%" },
            { "data": "fiyatAnlasilanZaman", "width": "15%" },
            { "data": "fiyatIdParaBirimi", "width": "15%" },
            { "data": "fiyatYemek", "width": "15%" },
            { "data": "fiyatUlasim", "width": "15%" },
            { "data": "fiyatOfferSaatUcreti", "width": "15%" },
            { "data": "sikayetNo", "width": "15%" },
            { "data": "note", "width": "15%" },
            { "data": "materyel", "width": "15%" },
            { "data": "cls", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a href="urunler/veri/${data}" class="btn btn-success text-white" style="cursor:pointer" draggable="false">
                                    <i class="fas fa-edit"></i> 
                                </a>
                                <a onclick=Delete("/Admin/Product/Delete/${data}") 
                                    class="btn btn-danger text-white" style="cursor:pointer" draggable="false">
                                    <i class="fas fa-trash-alt"></i> 
                                </a>
                            </div>
                           `;
                }, "width": "40%"
            }
        ]
    });
}



function Delete(url) {
    swal({
        title: 'Adındaki Kategoriyi kaldırmak istediğinizden emin misiniz ?',
        text: "Bunu geri almak mümkün olmayacaktır!",
        icon: 'warning',
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}
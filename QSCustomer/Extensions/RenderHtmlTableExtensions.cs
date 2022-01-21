using QSCustomer.IMainRepository;
using QSCustomer.Models.DbModels;
using QSCustomer.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QSCustomer.Extensions
{
    public class RenderHtmlTableExtensions
    {
        private IEnumerable<qprojehataTanimi> ProjeHataTanim;
        bool secPage;
        int s = 0;
        int i = 0;
        public async Task<string> GetTableString(PdfReport _report, string MainTab_Chart)
        {

            string Table = "";
            await Task.Run(() =>
            {
                string SECTION_ONE = @"
<style>
        .smalltx{
            font-size:6px!important;
        }
</style>
<div>
    <table class='table table-bordered rotate-table-grid'>
        <thead>
            <tr>
                <th class='verticalTableHeader' style=' min-width: 1px; width: 1px; max-width: 10px;'><span>Control Date</span></th>
                <th class='verticalTableHeader' style=' min-width: 1px; width: 1px; max-width: 10px;'><span>Product Date</span></th>
                <th class='verticalTableHeader' style=' min-width: 1px; width: 1px; max-width: 10px;'><span>Ürün Referansları Prod</span></th>
                <th class='verticalTableHeader' style=' min-width: 1px; width: 1px; max-width: 10px;'><span>lotNo</span></th>
                <th class='verticalTableHeader' style=' min-width: 1px; width: 1px; max-width: 10px;'><span>Seri No Serial Number</span></th>
                <th class='verticalTableHeader' style=' min-width: 1px; width: 1px; max-width: 10px;'><span>Harcanan Süre Spent</span></th>
                <th class='verticalTableHeader' style=' min-width: 1px; width: 1px; max-width: 10px;'><span>%50 Fazla Mesai</span></th>
                <th class='verticalTableHeader' style=' min-width: 1px; width: 1px; max-width: 10px;'><span>%100 Fazla Mesai</span></th>
                <th class='verticalTableHeader' style=' min-width: 1px; width: 1px; max-width: 10px;'><span>Kontrol Adet Checked</span></th>
                <th class='verticalTableHeader' style=' min-width: 1px; width: 1px; max-width: 10px;'><span>Rötüş Adet Reworker</span></th>
                <th class='verticalTableHeader' style=' min-width: 1px; width: 1px; max-width: 10px;'><span>Hatalı Adet Nok</span></th>
";
                string SECTION_TWO_ProjeHataTanimString = "";
                foreach (var item in _report._ProjeHataTanim)
                {
                    if (s < 37)
                        SECTION_TWO_ProjeHataTanimString += "<th class='verticalTableHeader' style='min-width: 5px; width: 5px; max-width: 10px;' min-height: 10px;height:10px;max-height:150px;'><span>" + item.hataTanimi + "</span></th>";
                    else
                        break;
                    s++;
                }
                string SECTION_THREE = "</tr></thead><tbody>";
                string SECTION_FOUR = "";
                foreach (var item in _report._ProjectDetails)
                {
                    SECTION_FOUR += @"<tr>
                    <td class='text-center smalltx'>" + item.KontrolTarihi + @"</td>
                    <td class='text-center smalltx'>" + item.UretimTarihi + @"</td>
                    <td class='text-center smalltx'>" + item.PartNrTanimi + @"</td>
                    <td class='text-center smalltx'>" + item.IotNo + @"</td>
                    <td class='text-center smalltx'>" + item.SeriNo + @"</td>
                    <td class='text-center smalltx'>" + Math.Round(item.Harcanansaat, 2) + @"</td>
                    <td class='text-center smalltx'>" + item.Mesai50Hesapla + @"</td>
                    <td class='text-center smalltx'>" + item.Harcanangirilenmesai + @"</td>
                    <td class='text-center smalltx'>" + item.KontrolAdedi + @"</td>
                    <td class='text-center smalltx'>" + item.TamirAdedi + @"</td>
                    <td class='text-center smalltx'>" + item.HataAdeti + "</td>";
                    if (item.KontrolAdedi == 0)
                    {

                    }
                    if (item.Faults != null)
                    {
                        foreach (var FaultItem in item.Faults)
                        {
                            if (i < 37)
                            {
                                if (FaultItem == null)
                                {
                                    SECTION_FOUR += "<td style=' min-width: 1px; width: 1px; max-width: 10px;' min-height: 10px;height:10px;max-height:150px;'></td>";
                                }
                                else
                                {
                                    SECTION_FOUR += "<td class='text-center smalltx'>" + FaultItem.Adet + "</td>";
                                }
                            }
                            else
                                break;
                            i++;

                        }
                    }
                    else
                    {
                        foreach (var empty in _report._ProjeHataTanim)
                        {
                            SECTION_FOUR += "<td></td>";
                        }
                    }
                    SECTION_FOUR += "</tr>";
                    i = 0;
                }
                string SECTION_FIVE = "</tbody></table></div>";
                Table = MainTab_Chart + SECTION_ONE + SECTION_TWO_ProjeHataTanimString + SECTION_THREE + SECTION_FOUR + SECTION_FIVE;

                SECTION_TWO_ProjeHataTanimString = "";
                SECTION_THREE = "";
                SECTION_FOUR = "";
                SECTION_FIVE = "";







                if (_report._ProjeHataTanim.Count() >= 37)
                {
                    for (int s = 37; s < 74; s++)
                    {
                        if (_report._ProjeHataTanim.Count() < s)
                            SECTION_TWO_ProjeHataTanimString += "<th class='verticalTableHeader' style='min-width: 5px; width: 5px; max-width: 10px;' min-height: 10px;height:10px;max-height:150px;'><span>" + _report._ProjeHataTanim[s].hataTanimi + "</span></th>";
                        else
                            break;
                    }
                    SECTION_THREE = "</tr></thead><tbody>";
                    foreach (var item in _report._ProjectDetails)
                    {
                        SECTION_FOUR += @"<tr>
                    <td class='text-center smalltx'>" + item.KontrolTarihi + @"</td>
                    <td class='text-center smalltx'>" + item.UretimTarihi + @"</td>
                    <td class='text-center smalltx'>" + item.PartNrTanimi + @"</td>
                    <td class='text-center smalltx'>" + item.IotNo + @"</td>
                    <td class='text-center smalltx'>" + item.SeriNo + @"</td>
                    <td class='text-center smalltx'>" + Math.Round(item.Harcanansaat, 2) + @"</td>
                    <td class='text-center smalltx'>" + item.Mesai50Hesapla + @"</td>
                    <td class='text-center smalltx'>" + item.Harcanangirilenmesai + @"</td>
                    <td class='text-center smalltx'>" + item.KontrolAdedi + @"</td>
                    <td class='text-center smalltx'>" + item.TamirAdedi + @"</td>
                    <td class='text-center smalltx'>" + item.HataAdeti + "</td>";
                        if (item.KontrolAdedi == 0)
                        {

                        }
                        if (item.Faults != null)
                        {
                            for (int i = 37; i < 74; i++)
                            {
                                if (item.Faults.Count() < i)
                                {
                                    if (item.Faults[i] == null)
                                    {
                                        SECTION_FOUR += "<td style=' min-width: 1px; width: 1px; max-width: 10px;' min-height: 10px;height:10px;max-height:150px;'></td>";
                                    }
                                    else
                                    {
                                        SECTION_FOUR += "<td class='text-center smalltx'>" + item.Faults[i].Adet + "</td>";
                                    }
                                }
                                else
                                    break;

                            }
                        }
                        else
                        {
                            foreach (var empty in _report._ProjeHataTanim)
                            {
                                SECTION_FOUR += "<td></td>";
                            }
                        }
                        SECTION_FOUR += "</tr>";
                        i = 0;
                    }
                    SECTION_FIVE = "</tbody></table></div>";

                    Table += SECTION_ONE + SECTION_TWO_ProjeHataTanimString + SECTION_THREE + SECTION_FOUR + SECTION_FIVE;
                }

            });

            return Table;
        }
    }
}

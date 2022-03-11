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

            int productRef_count = 0, projectDetails_count = 0;
            double spent = (_report._SpentHours * 100) / 100;
            double OverTime100 = (_report._OverTime100Total * 100) / 100;
            double SpentHoursTotal = (_report._SpentHoursTotal * 100) / 100;
            DateTime begDate = Convert.ToDateTime(_report._SelectedProject.baslangicTarihi);
            List<string> CheckDate = new List<string>();
            List<long> ValTotal = new List<long>();
            long ValItem = 0;
            bool FirstAction = true, FirstDateAction = true;
            int DateCount = 0;
            string RenderedHtml = "";


            string Header_Table = "";
            string Details_Table = "";
            await Task.Run(() =>
            {
                string SecOne = @"
<style>
        .table-main > :not(caption) > * > * {
            padding: 0.5rem 0.5rem !important;
        }
</style>
        <section class='container' id='renderedSection'>
            <div class='pb-lg-5'>
                <table class='table table-bordered table-main border-expert'>
                    <thead>
                        <tr>
                            <th scope='col' class='mediumtx' style='width:90px;'><img src='/assets/images/resources/logo-3.png' style='width:80px;'></th>
                            <th scope='col' class='table-active text-center mediumtx' colspan='5'>KALITE KONTROL & ROTUS RAPORU / QUALITY CONTROL & REWORK REPORT</th>
                            <th scope='col' class='smalltx'>
                                OPR. NO<br />
                                Opr.Nr#
                            </th>
                            <th scope='col' class='mediumtx text-center' colspan='2'>" + _report._SelectedProject.projeCode + @"</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr class='table-active'>
                            <th scope='row' class='smalltx'>
                                Başlangıç Tarihi<br />
                                Start Date
                            </th>
                            <td class='mediumtx'>
                                <span id='beg_date'>
                                    " + begDate.ToString("dd/MM/yyyy") + @"
                                </span>
                            </td>
                            <td class='mediumtx' style='min-width:160px;'>
                                ÜRÜN REFERANS<br />
                                Product Ref #
                            </td>
                            <td class='mediumtx' style='min-width:160px;'>
                                HATA TANIMLARI<br />
                                Nok Descriptions
                            </td>
                            <td class='smalltx' style='min-width:160px;'>
                                ŞİKAYET NO<br />
                                Complaint Nr#
                            </td>
                            <td class='mediumtx'>
                                PPM

                            </td>
                            <td class='smalltx'>
                                KONTROL ADET<br />
                                Checked Qty

                            </td>
                            <td class='smalltx'>
                                HATALI ADET<br />
                                NOK Qty

                            </td>
                            <td class='smalltx'>
                                RÖTÜŞ ADET<br />
                                Reworked Qty
                            </td>
                        </tr>
                        <tr>
                            <th scope='row'>
                                MÜŞTERİ<br />
                                Customer
                            </th>
                            <td style='text-align:center; vertical-align: middle;'>
                                <span id='customer'>
                                    " + _report._Customer.musteriAdi + @"
                                </span>
                            </td>";

                string SecTwo = @"
                            <td rowspan='3' style='text-align:center;'>";
                if (_report._PartNrTanimlari.Count() > 21)
                {
                    for (int i = 0; i < 21; i++)
                    {
                        productRef_count++;
                        if (productRef_count % 3 == 0)
                        {
                            SecTwo += "<span>" + _report._PartNrTanimlari[i].partNrTanimi + ", </span><br/>";
                        }
                        else
                        {
                            SecTwo += "<span>" + _report._PartNrTanimlari[i].partNrTanimi + ", </span>";
                        }
                    }
                    SecTwo += "<span>...</span>";
                }
                else
                {
                    foreach (var item in _report._PartNrTanimlari)
                    {
                        productRef_count++;
                        if (productRef_count % 3 == 0)
                        {
                            SecTwo += "<span>" + item.partNrTanimi + ", </span><br/>";
                        }
                        else
                        {
                            SecTwo += "<span>" + item.partNrTanimi + ", </span>";
                        }
                    }
                }


                SecTwo += "</td>";

                string SecThree = @"
                            <td rowspan='3' style='text-align:center;'>";
                if (_report._ProjeHataTanim.Count() > 14)
                {
                    for (int i = 0; i < 20; i++)
                    {
                        productRef_count++;
                        if (productRef_count % 2 == 0)
                        {
                            SecThree += "<span>" + _report._ProjeHataTanim[i].hataTanimi + "</span><br/>";
                        }
                        else
                        {
                            SecThree += "<span>" + _report._ProjeHataTanim[i].hataTanimi + "</span>";
                        }
                    }
                    SecThree += "<span>...</span>";
                }
                else
                {
                    foreach (var item in _report._ProjeHataTanim)
                    {
                        productRef_count++;
                        if (productRef_count % 3 == 0)
                        {
                            SecThree += "<span>" + item.hataTanimi + "</span><br/>";
                        }
                        else
                        {
                            SecThree += "<span>" + item.hataTanimi + "</span>";
                        }
                    }
                }

                SecThree += "</td>";
                string SecFour = @"
                            <td rowspan='3' style='text-align:center;'>
                                <span id='comp_nr'>" + _report._SelectedProject.sikayetNo + @"</span>
                            </td>
                            <td style='text-align:center; vertical-align: middle;'>
                                <span id='ppm'>" + _report._PPMTotal + @"</span>
                            </td>
                            <td style='text-align:center; vertical-align: middle;'>
                                <span id='checked'>" + _report._CheckedTotal + @"</span>
                            </td>
                            <td style='text-align:center; vertical-align: middle;'>
                                <span id='nok'>" + _report._NokTotal + @"</span>
                            </td>
                            <td style='text-align:center; vertical-align: middle;'>
                                <span id='reworked'>" + _report._ReworkedTotal + @"</span>
                            </td>
                        </tr>
                        <tr>
                            <th scope='row'>
                                OPR. ALANI
                                Opr. Area
                            </th>
                            <td style='text-align:center; vertical-align: middle;'>
                                <span id='opr_area'>
                                    " + _report._Operation.fabrikaAdi + @"
                                </span>
                            </td>

                            <td style='text-align:center; vertical-align: middle;'>
                                HARCANAN SÜRE
                                Spent Hours
                            </td>
                            <td style='text-align:center; vertical-align: middle;'>
                                %50 FAZLA MESAI
                                %50 overtime
                            </td>
                            <td style='text-align:center; vertical-align: middle;'>
                                %100 FAZLA MESAI
                                %100 overtime
                            </td>

                            <td style='text-align:center; vertical-align: middle;'>
                                TOPLAM HARCANAN SÜRE
                                Total Spent Hours
                            </td>
                        </tr>
                        <tr>
                            <th scope='row'>
                                PARÇA TANIMI
                                Name of Product
                            </th>
                            <td style='text-align:center; vertical-align: middle;'>
                                <span id='nameofProduct'>" + _report._SelectedProject.materyel + @"</span>
                            </td>
                            <td style='text-align:center; vertical-align: middle;'>
                                <span id='spent_hr'>
                                    " + spent + @"
                                </span>
                            </td>
                            <td style='text-align:center; vertical-align: middle;'>
                                <span id='overtime50'></span>
                            </td>
                            <td style='text-align:center; vertical-align: middle;'>
                                <span id='overtime100'>" + OverTime100 + @"</span>
                            </td>
                            <td style='text-align:center; vertical-align: middle;'>
                                <span id='totalSpent_hr'>
                                    " + SpentHoursTotal + @"
                                </span>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </section>
";

                Header_Table = SecOne + SecTwo + SecThree + SecFour;

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
                Details_Table = MainTab_Chart + SECTION_ONE + SECTION_TWO_ProjeHataTanimString + SECTION_THREE + SECTION_FOUR + SECTION_FIVE;

                SECTION_TWO_ProjeHataTanimString = "";
                SECTION_THREE = "";
                SECTION_FOUR = "";
                SECTION_FIVE = "";







                if (_report._ProjeHataTanim.Count() >= 37)
                {
                    for (int s = 37; s < 74; s++)
                    {
                        if (s < _report._ProjeHataTanim.Count())
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
                                if (i < item.Faults.Count())
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

                    Details_Table += SECTION_ONE + SECTION_TWO_ProjeHataTanimString + SECTION_THREE + SECTION_FOUR + SECTION_FIVE;
                }

            });

            return Header_Table + Details_Table;
        }
    }
}

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Statistics.aspx.cs" Inherits="TayaIT.Enterprise.EMadbatah.Web.Statistics" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <form id="statsPageForm" action="" runat="server">
    <div class="MainContent_statistics">
        <h3>
            <asp:Label Text="أظهر وقت التحدث للأعضاء" runat="server" ID="lblShowTime" />
        </h3>
        <div class="rblMainChoices" id="rblMainChoices">
            <div class="option_container fr">
                <input type="radio" checked="checked" value="allSession" name="rblMainChoicesGroup"
                    id="rblMainChoicesAllSession" />
                <label for="rblMainChoicesAllSession">
                    كل الجلسة</label>
            </div>
            <div class="option_container fr">
                <input type="radio" value="agendaItem" name="rblMainChoicesGroup" id="rblMainChoicesAgendaItem" />
                <label for="rblMainChoicesAgendaItem">
                    لبند معين</label>
            </div>
            <div class="option_container fr">
                <asp:DropDownList ID="ddlAgendaItems" AutoPostBack="false" runat="server">
                </asp:DropDownList>
            </div>
            <div class="clear">
            </div>
        </div>
        <h3>
            <asp:Label Text="حالة الحضور" runat="server" ID="Label1" />
        </h3>
        <div class="rblSpeakerChoices" id="rblSpeakerChoices">
            <div class="option_container fr">
                <input type="radio" value="allSpeakers" name="rblSpeakerChoicesGroup" id="rblSpeakerChoicesAllSpeakers" />
                <label for="rblSpeakerChoicesAllSpeakers">
                    كل المتحدثن</label>
            </div>
            <div class="option_container fr">
                <input type="radio" checked="checked" value="filteredSpeakers" name="rblSpeakerChoicesGroup"
                    id="rblSpeakerChoicesFiltered" />
                <label for="rblSpeakerChoicesFiltered">
                    كل المتحدثين ما عدا الأعضاء الغياب و المعتذرين و الذين في مهمة رسمية</label>
            </div>
            <div class="clear">
            </div>
        </div>
    </div>
    <div class="srchInfo">
      <table border="0" cellspacing="0" cellpadding="0">
                <thead>
                    <tr>
                        <th class="widthnum1 basic_info">
                            اسم المستخدم
                        </th>
                        <th class="widthnum1 basic_info">
                           الوظيفة
                        </th>
                        <th class="widthnum1 basic_info">
                            الوقت بالدقائق
                        </th>
                        <th class="editbuttons">
                        </th>
                    </tr>
                </thead>
                <tbody id="usersStatistics" runat="server"></tbody>
         </table>
         </div>
         <div id="chart1" style="width:100%; height:250px;">

</div>
    </form>
    <script type="text/javascript">


        $(document).ready(function () {

            $("input[name*='rblMainChoicesGroup']").click(function () {
                $(this).each(function () {
                    if (this.checked == true) {

                        switch ($(this).val()) {
                            case 'allSession':
                                $("#MainContent_ddlAgendaItems").attr('disabled', 'disabled');
                                $("#MainContent_ddlAgendaItems").val(0);
                                // $("#MainContent_ddlAgendaSubItems").attr('disabled', 'disabled');



                                var speakersType = getSpeakersChoiceVal();
                                $("input[name*='rblSpeakerChoicesGroup']").each(function () {
                                    if (this.checked == true) {

                                        speakersType = $(this).val();
                                    }
                                });  //end each

                                getAttendanceStats(speakersType);



                                break;
                            case 'agendaItem':
                                $("#MainContent_ddlAgendaItems").removeAttr('disabled');
                                // $("#MainContent_ddlAgendaSubItems").attr('disabled');
                                break;
                        }
                    }
                }); //end each
            }); //end rblMainChoicesGroup

            $("input[name*='rblSpeakerChoicesGroup']").click(function () {
                $(this).each(function () {
                    if (this.checked == true) {

                        getAttendanceStats($(this).val());
                    }
                }); //end each
            }); //end rblMainChoicesGroup


            // cascading drop down lists, to get AgendaSubItems By AgendaItem ID
            $("#MainContent_ddlAgendaItems").change(function () {
                // $("#MainContent_ddlAgendaSubItems").html("");
                var AgendaItemID = $("#MainContent_ddlAgendaItems > option:selected").attr("value");
                if (AgendaItemID != 0) {

                    var speakersType = getSpeakersChoiceVal();
                    $("input[name*='rblSpeakerChoicesGroup']").each(function () {
                        if (this.checked == true) {

                            speakersType = $(this).val();
                        }
                    });  //end each

                    getAttendanceStats(speakersType);

                    // $('select#MainContent_ddlAgendaSubItems').attr('disabled', 'disabled')
                    //                jQuery.ajax({
                    //                    cache: false,
                    //                    type: 'post',
                    //                    url: 'EditSessionHandler.ashx',
                    //                    data: {
                    //                        funcname: 'GetAgendaSubItems',
                    //                        agendaid: AgendaItemID
                    //                    },
                    //                    //contentType: "application/json; charset=utf-8",
                    //                    dataType: 'json',
                    //                    success: function (subitems) {
                    //                        $.each(subitems, function () {
                    //                            $("#MainContent_ddlAgendaSubItems").append($("<option></option>").val(this['ID']).html(this['Text']));
                    //                        });
                    //                        if (subitems.length > 0) {
                    //                            $('select#MainContent_ddlAgendaSubItems').removeAttr('disabled')
                    //                        }


                    //                    },
                    //                    error: function () {

                    //                    }
                    //                });
                }
            }); //end dropdown cascading

            getAttendanceStats('filteredSpeakers');

        });  //end document.ready     



        function getSpeakersChoiceVal() {
            $("input[name*='rblSpeakerChoicesGroup']").each(function () {
                if (this.checked == true) {

                    return $(this).val();
                }
            });  //end each
        }


        function getAttendanceStats(speakersType) {
            var agendaItemID = $("#MainContent_ddlAgendaItems").val();
            //var agendaSubItemID = $("#MainContent_ddlSubAgendaItems").val();

            if (!agendaItemID)
                agendaItemID = 0;
            //          if (!agendaSubItemID)
            //              agendaSubItemID = 0;

            $('.absLoad.loading').show();

            jQuery.ajax({
                cache: false,
                type: 'post',
                url: 'SessionHandler.ashx',
                data: {
                    funcname: 'GetSpeakersStatistics',
                    agid: agendaItemID,
                    // AgendaSubItemID: agendaSubItemID,
                    speakerstype: speakersType,
                    sid: getParameterByName('sid')
                },
                //contentType: "application/json; charset=utf-8",
                dataType: 'json',
                success: function (data) {
                    var html = "";
                    $.each(data, function () {

                        html += "<tr><td>" + this['Name'] + "</td><td>" + this['JobTitle'] + "</td><td>" + (parseFloat(this['TotalSpeakTime']) / 60).toFixed(2) + "</td></tr>";


                    });

                    $("#MainContent_usersStatistics").html("");
                    $("#MainContent_usersStatistics").html(html);
                    $('.absLoad.loading').hide();
                },
                error: function () {
                    $('.absLoad.loading').hide();
                }
            });


        }




    </script>
</asp:Content>

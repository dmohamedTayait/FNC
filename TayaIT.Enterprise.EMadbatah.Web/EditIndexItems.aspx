<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="EditIndexItems.aspx.cs" Inherits="TayaIT.Enterprise.EMadbatah.Web.EditIndexItems" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

<div class="MainContent_EditIndx">
    <div id="divAllContent" class="divAllContent">
    </div>

    <div id="divEditTools" style="display:none" class="divEditTools">
        <input type="text" id="txtEditVal" name="txtEditVal" value=""  class="textfield" />
        <a href="#" id="aSave" class="Save">حفظ</a>
        <a href="#" id="aCancel">إلغاء</a>
    </div>
    </div>
<script type="text/javascript">


    $(document).ready(function () {

        $('#aCancel').click(function (event) {
            var divEditTools = $('#divEditTools');
            var txtEditVal = $('#txtEditVal');
            txtEditVal.val('');
            divEditTools.hide();
        });

        $('#aSave').click(function (event) {
            var divEditTools = $('#divEditTools');
            var agendaItemId = -1;
            var agendaSubItemId = -1;
            var text = $('#txtEditVal').val(); //currentEditItem.html();

            if (currentEditItem.attr('data-agendaitemid'))
                agendaItemId = currentEditItem.attr('data-agendaitemid');
            if (currentEditItem.attr('data-agendasubitemid'))
                agendaSubItemId = currentEditItem.attr('data-agendasubitemid');


            $('.absLoad.loading').show();
            jQuery.ajax({
                cache: false,
                type: 'post',
                url: 'SessionHandler.ashx',
                data: {
                    funcname: 'UpdateSessionAgendaItemValue',
                    itemname: text,
                    AgendaSubItemID: agendaSubItemId,
                    agid: agendaItemId
                },
                dataType: 'json',
                success: function (data) {
                    if (data == '-1') {
                        alert("لقد حدث خطأ");
                    }
                    divEditTools.hide();
                    $('.absLoad.loading').hide();
                },
                error: function () {
                    $('.absLoad.loading').hide();
                    divEditTools.hide();
                }
            }); //end ajax

            currentEditItem.html(text);
            $('#aCancel').click();
            currentEditItem = null;


        }); //end save



        $('.agendaItem').live("click", function (event) {
            if (event.type == "click") {
                // $(this).addClass('hover').find('.editbuttons').hide();
                currentEditItem = $(this);
                //var isCurrentItem
                var txtEditVal = $('#txtEditVal');
                var divEditTools = $('#divEditTools');
                txtEditVal.val(currentEditItem.html());
                divEditTools.show();
            }
        });



        getAgendaIndexItems();

    });

    var currentEditItem = null;




    function getAgendaIndexItems() {
        var sessionID = getParameterByName('sid');
        
        $('.absLoad.loading').show();
        jQuery.ajax({
            cache: false,
            type: 'post',
            url: 'SessionHandler.ashx',
            data: {
                funcname: 'GetSessionAgendaItemsIndex',
                sid: getParameterByName('sid')
            },
            dataType: 'json',
            success: function (data) {

                if (data && data != null && data.length > 0) {
                   // alert(data.length);
                    $.each(data, function () {
                        if (this.IsMainItem == true) {
                            var newDiv = $("<div></div>").html(this.Name).attr('data-agendaitemid', this.ID).attr('class', 'agendaItem')
                            $("#divAllContent").append(newDiv);
                        } else {
                            var newDiv = $("<div></div>").html(this.Name).attr('data-agendasubitemid', this.ID).attr('class', 'agendaItem subagendaItem')
                            $("#divAllContent").append(newDiv);
                        }
                    });
                }

                $('.absLoad.loading').hide();
            },
            error: function () {
                $('.absLoad.loading').hide();
            }
        });
    }

</script>

   
    
</asp:Content>


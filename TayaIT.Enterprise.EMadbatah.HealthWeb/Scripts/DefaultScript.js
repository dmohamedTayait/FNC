function fileUploader(ID) {
    new qq.FileUploader({
        template: '<div class="qq-uploader">' +
            '<div class="qq-upload-drop-area"><span>Drop files here to upload</span></div>' +
            '<div class="qq-upload-button">إضافة ملحق</div>' +
            '<ul class="qq-upload-list"></ul>' +
            '</div>',
        messages: {
            typeError: "فقط {extensions} مسموح بتحميل الملفات من نوع",
            sizeError: "{file} is too large, maximum file size is {sizeLimit}.",
            minSizeError: "{file} is too small, minimum file size is {minSizeLimit}.",
            emptyError: "{file} is empty, please select files again without it.",
            onLeave: "The files are being uploaded, if you leave now the upload will be cancelled."
        },

        // pass the dom node (ex. $(selector)[0] for jQuery users)
        element: $('#file-uploader' + ID)[0],
        listElement: $('#separate-list' + ID)[0],
        allowedExtensions: ['doc', 'docx', 'pdf'],
        // path to server-side upload script
        action: 'FileHandler.ashx',
        params: {
            funcname: 'Upload',
            sid: $('#file-uploader' + ID).attr('data-sid')
        },
        onComplete: function (id, fileName, responseJSON) {
            $('#separate-list' + ID + ' li:last').attr('data-id', responseJSON.id).append('<div class=\"upordown flnone\"><div class=\"up\"></div><div class=\"down\"></div></div><a class=\"Dellink\" href=\"#\">حذف</a>');
            // change the number
            var span = $('.column.column5 .number span', '.tbrowID' + ID);
            var spanHTML = span.html();
            var spanValue = spanHTML - 0 + 1;
            span.html(spanValue)
        }
        //onCancel: function(id, fileName){}
    });
}
$(document).ready(function () {
    $('.btnConfirm').click(function (e) {
        var sessionID = $(this).attr('data-sid');
        var select = $(this).parent('td').children('.selectVecsysFolders');
        var spnMessage = $(this).parent('td').children('span[name="spnWarn"]');
        if (select.val() == "") {
            spnMessage.text("من فضلك اختر المجلد المناسب");
            return;
        }
        var mp3FolderPath = select.val();
        var xmlFolderPath = select.children("option:selected").attr("data-xmlserverpath");
        //alert(xmlFolderPath);
        var errorMessage = " تستغرق هذه العملية عدة دقائق .. برجاء الانتظار حتى يتم نقل جميع الملفات للمضبطة الإلكترونية";
        var answer = confirm("هل أنت متأكد من أنك تريد ربط الملفات الموجوده في المجلد المختار بالجلسة الجديدة؟" + "\n" + errorMessage);
        //display confirm
        //if cancel return
        if (answer) {
            $('.btnConfirm').attr('disabled', 'disabled');
            $('.absLoad.loading').show();
            jQuery.ajax({
                cache: false,
                type: 'post',
                url: 'SessionHandler.ashx',
                data: {
                    funcname: 'ConfirmNewSession',
                    safp: mp3FolderPath,
                    sxfp: xmlFolderPath,
                    sid: sessionID
                },
                dataType: 'json',
                success: function (response) {
                    //html to display here (contain sessionFilesHTML or errorHTML)
                    if (response.Sucess == true)
                        location.reload();
                    else
                        spnMessage.text(response.Message);

                    $('.absLoad.loading').hide();
                },
                error: function () {
                    spnMessage.text("لقد حدث خطأ");
                    $('.absLoad.loading').hide();
                }
            });
        }
    })

    // on hover over the rows
    $('#maintable1 tr.tbrow').hover(function () {
        $(this).addClass('hover')
    }, function () {
        $(this).removeClass('hover')
    })
    // on click on the arrows
    $('#maintable1 .options .hoverArrow').click(function () {
        var curent = $(this);
        $('#maintable1 .options .hoverArrow').not(curent).removeClass('up').addClass('down');
        $(this).toggleClass('down up');
        var parentRow = $(this).parents('.tbrow');
        // remove opend class from other
        $('#maintable1 tr.tbrow').not(parentRow).removeClass('opend');
        parentRow.toggleClass('opend');

    })
    if ($('#MainContent_UserRuleHidden').val() == "Reviewer") {
        $('input[name=file]').css("display", "none");
        $('.qq-upload-button').css("display", "none");
    }
    // for the up and down
    function move(wh, ty) {
        var row = $(wh).parents(ty + ":first");
        // before move current index
        var beforeMoveIndex = row.index()
        var row2;
        if ($(wh).is(".up")) {
            row2 = row.prev()
            row.insertBefore(row2);
        } else {
            row2 = row.next()
            row.insertAfter(row2);
        }
        // send current index
        var afterMoveIndex = row.index()
        // return
        return { before: beforeMoveIndex, after: afterMoveIndex, tr1: row, tr2: row2 }
    }
    $('#maintable1 .ulist li,#maintable1 .smalltable tr').live('mouseover mouseout', function (event) {
        if (event.type == "mouseover") {
            // do something on mouseover
            $('.upordown', this).addClass('show')
        } else {
            // do something on mouseout
            $('.upordown', this).removeClass('show')
        }
    })
    $(".ulist li .upordown .up,.ulist li .upordown .down").live('click', function () {
        var pos = move(this, 'li')
        var sessionID = $(pos.tr1).attr('data-sessionid'); 
        reorderAttachment(sessionID,$(pos.tr1).attr('data-id'), pos.after, $(pos.tr2).attr('data-id'), pos.before); //attachement
    });
    $(".smalltable .upordown .up,.smalltable .upordown .down").live('click', function () {
        var pos = move(this, 'tr')
        var sessionID = $(pos.tr1).attr('data-sessionid'); 
        reorder(sessionID,$(pos.tr1).attr('data-id'), pos.after, $(pos.tr2).attr('data-id'), pos.before);
    });
    function reorder(sessionid,id1, neworder1, id2, neworder2) {
        $.ajax({
            cache: false,
            type: 'post',
            url: 'ReorderHandler.ashx',
            data: {
                funcname: 'ReorderSessionFiles',
                rid1: id1,
                order1: neworder1,
                rid2: id2,
                order2: neworder2,
                sid:sessionid
            },
            success: function (response) {
                if (response != '') {

                }
            },
            error: function () {

            }
        });
    }
    function reorderAttachment(sessionID,id1, neworder1, id2, neworder2) {
        $.ajax({
            cache: false,
            type: 'post',
            url: 'ReorderHandler.ashx',
            data: {
                funcname: 'ReOrderAttachment',
                rid1: id1,
                order1: neworder1,
                rid2: id2,
                order2: neworder2,
                sid: sessionID
            },
            success: function (response) {
                if (response != '') {

                }
            },
            error: function () {

            }
        });
    }

    $('.Dellink').live('click', function (e) {
        var button = $(this)
        var parentLI = button.parents('li')
        var answer = confirm("هل أنت متأكد أنك تريد الحذف ؟");

        var uid = parentLI.attr("data-id");
        if (answer) {
            $('.absLoad.loading').show();
            jQuery.ajax({
                cache: false,
                type: 'post',
                url: 'SessionHandler.ashx',
                data: {
                    funcname: 'RemoveAttachment',
                    attachid: uid
                },
                success: function (response) {

                    if (response != '1') {//| response != 'true')
                        alert('لقد حدث خطأ');
                    }
                    else {
                        var span = $('.column.column5 .number span', button.parents('tr.tbrow'));
                        var spanHTML = span.html();
                        var spanValue = spanHTML - 0 - 1;
                        span.html(spanValue)
                        parentLI.remove(); // ('display', 'none');
                    }
                    $('.absLoad.loading').hide();
                }
            });

        } //end of answer
        e.preventDefault();
    })




    $('.btnReloadSessionFiles').click(function (e) {
        var sessionID = $(this).attr('data-sid');


        $(this).attr('disabled', 'disabled');
        $('.absLoad.loading').show();
        jQuery.ajax({
            cache: false,
            type: 'post',
            url: 'SessionHandler.ashx',
            data: {
                funcname: 'ReloadSessionFiles',
                sid: sessionID
            },
            dataType: 'json',
            success: function (response) {
                //html to display here (contain sessionFilesHTML or errorHTML)
                if (response.Sucess == true) {
                    //alert('true');
                    location.reload();
                }
                else {
                    alert(response.Message);
                    //spnMessage.text(response.Message);
                }

                $('.absLoad.loading').hide();
                $('.btnReloadSessionFiles').removeAttr('disabled');
            },
            error: function () {
                spnMessage.text("لقد حدث خطأ");
                $('.absLoad.loading').hide();
            }
        });

    })


})
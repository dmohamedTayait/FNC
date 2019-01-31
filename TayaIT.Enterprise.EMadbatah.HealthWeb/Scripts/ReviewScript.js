function changeButtonsForSessionStatus() 
{

    var hdisCurrentUserFileRev = $('.isCurrentUserFileRev');
    if (hdisCurrentUserFileRev && hdisCurrentUserFileRev.val() != null) {
        if (hdisCurrentUserFileRev.val() == "true") {
            $('.btnApproveSession').css("display", "none");
            $('.btnFinalApproveSession').css("display", "none");
            return;
        }
    }

    jQuery.ajax({
        cache: false,
        type: 'post',
        url: 'ReviewerHandler.ashx',
        data: {
            funcname: 'GetSessionStatus',
            sid: $('#MainContent_SessionIDHidden').val()
        },
        success: function (response) {
            if (response == "-1") {
                return -1;
            }
            else {
                //new = 1
                //InProg = 2
                //completed = 3
                //Approv = 4
                //finalApprove = 5
                var sessionStatus = parseInt(response);

                if (sessionStatus == 1 || sessionStatus == 2 || sessionStatus == 5) {
                    $('.btnApproveSession').css("display", "none");
                    $('.btnFinalApproveSession').css("display", "none");
                }
                else if (sessionStatus == 3) {
                    $('.btnApproveSession').css("display", "inline");
                    $('.btnFinalApproveSession').css("display", "none");
                } else if (sessionStatus == 4) {
                    $('.btnApproveSession').css("display", "none");
                    $('.btnFinalApproveSession').css("display", "inline");
                }
                //    return parseInt(response);
            }
        },
        error: function () {
            //return -1;
        }
    });
}

$(document).ready(function () {


    $('.Edititem').hoverIntent(function () {

        $('#spnToolTipFileName').html($(this).attr('data-filename'));
        $('#spnToolTipUserName').html($(this).attr('data-username'));
        $('#spnToolTipFileRevName').html($(this).attr('data-filerevname'));
        $('#spnToolTipRevName').html($(this).attr('data-revname'));

        $('#divToolTip').fadeIn();
    },
     function () {
         $('#divToolTip').fadeOut();
     });




    // tinymce end
    var lastEditedDiv;
    $(".openeditem .Edititem").click(function () {
        lastEditedDiv = $(this);


        var id = $(this).attr('data-scid');
        var currentUserID = $('.currentUserID').val();
        var sessionFileID = $(this).attr('data-sfid');
        var segRevID = $(this).attr('data-filerevid');
        var ed = $('#SessionContentItemIDHidden');
        var note = $(this).attr('data-revnote');
        ed.val(id);


        var isCurrentUserFileRev = $('.isCurrentUserFileRev').val();

        if (isCurrentUserFileRev == 'true' && currentUserID != segRevID) {
            jQuery.ajax({
                cache: false,
                type: 'post',
                url: 'ReviewerHandler.ashx',
                data: {
                    funcname: 'IsSessionFileLockedByFileRev',
                    scid: id
                },
                success: function (response) {
                    // alert(response)
                    if (response == "true") {
                        //alert("yes locked");
                    }
                    else {
                        //alert("no");

                        var approve = confirm("لم يتم تخصيص الملف الخاص بهذا المقطع لمراجع ملف ... هل ترغب بأن تكون مراجع هذا الملف؟");
                        if (approve) {
                            //alert('sfid ' + sessionFileID + ' currentuserid ' + currentUserID);

                            jQuery.ajax({
                                cache: false,
                                type: 'post',
                                url: 'AdminHandler.ashx',
                                data: {
                                    funcname: 'AssignSessionFileReviewer',
                                    sfid: sessionFileID,
                                    uid: currentUserID,
                                    semail: 'false'
                                },
                                success: function (response) {
                                    // alert(response)
                                    if (response == "true") {
                                        //alert("yes assigned");
                                        // location.reload();
                                    }
                                    else {
                                        //alert("no not assigned");

                                        //                                         var approve = confirm("لم يتم تخصيص الملف الخاص بهذا المقطع لمراجع ملف ... هل ترغب بأن تكون مراجع هذا الملف؟");
                                        //                                         if (approve) {

                                        return false;



                                        //                                         }
                                    }
                                },
                                error: function () {
                                    //return -1;
                                }
                            });


                        }
                        else {
                            return false;
                        }
                    }
                },
                error: function () {
                    //return -1;
                }
            });

        } //end if rev
        else {
            var parent = $(this).parents('.openeditem');
            var MP3FilePath = parent.find('.MP3FilePath').val()
            var MP3FileStartTime = Math.floor(parent.find('.MP3FileStartTime').val())
            var MP3FileEndTime = Math.floor(parent.find('.MP3FileEndTime').val())
            var AudioPlayer = $("#jquery_jplayer_1");
            // jplayer
            AudioPlayer.jPlayer('destroy').jPlayer({
                swfPath: "/scripts/jPlayer/",
                wmode: "window",
                solution: 'html, flash',
                supplied: "mp3",
                preload: 'metadata',
                volume: 1,
                cssSelectorAncestor: '#jp_container_1',
                errorAlerts: false,
                warningAlerts: false,
                ready: function () {
                    // play the jplayer
                    $(this).jPlayer("setMedia", {
                        mp3: MP3FilePath // mp3 file path
                    }).jPlayer("play", MP3FileStartTime);
                },
                timeupdate: function (event) {
                    if (!$(this).data("jPlayer").status.paused) {
                        ed = $('textarea.tinymce').tinymce()
                        // all span segments
                        var all_spans_segments = $('span.segment', ed.contentDocument);
                        // remove all classes
                        all_spans_segments.removeClass('highlight editable');
                        // highlight the word by time
                        var playertime = event.jPlayer.status.currentTime;
                        if (Math.round(playertime) > MP3FileEndTime) {
                            AudioPlayer.jPlayer('pause', MP3FileStartTime)
                        } else if (Math.floor(playertime) < MP3FileStartTime) {
                            AudioPlayer.jPlayer('play', MP3FileStartTime)
                        } else {
                            //
                            var playerfixedTime = playertime.toFixed(2);
                            var playerfixedTimeString = playerfixedTime.toString();
                            var playerfixedTimeToArray = playerfixedTimeString.split('.');
                            // highlight the span
                            var highlight = all_spans_segments.filter('span.segment[data-stime^=' + playerfixedTimeToArray[0] + '\\.]');
                            if (highlight.length > 1) {
                                highlight = highlight.filter(function () {
                                    // get the nearest span
                                    var spanTime = $(this).attr('data-stime')
                                    var spanTimeToArray = spanTime.split('.');
                                    var spanfragment = spanTimeToArray[1];
                                    var playerfragment = playerfixedTimeToArray[1];
                                    if (playerfragment >= spanfragment) {
                                        return $(this);
                                    }
                                }).filter(':last')
                            }
                            // highlight
                            highlight.addClass('highlight')
                        }
                        if ($.browser.msie && $.browser.version == '9.0') {
                            if (Math.round(playertime) > MP3FileEndTime || Math.floor(playertime) < MP3FileStartTime) {
                                AudioPlayer.jPlayer('stop')
                            }
                        }
                    }
                }
            });


            if (lastEditedDiv.hasClass('reditem') || lastEditedDiv.hasClass('blueitem') || lastEditedDiv.hasClass('greenitem')) {
                $('#approve').show();
            }
            else {
                $('#approve').hide();
            }

            if (lastEditedDiv.hasClass('reditem')) {
                $('#reject').hide();
            }
            else {
                $('#reject').show();
            }


            //alert($(this).attr('data-sfid')); alert($(this).attr('data-scid'));


            $('#IsSessionStartHidden').val($(this).attr('data-isSessionStart'));

            if ($('#IsSessionStartHidden').val() == "0")
                $("#lnkMoreEditOptions").attr('style', 'display:block').attr('href', 'EditSessionFile.aspx?scid=' + $(this).attr('data-scid') + '&sfid=' + $(this).attr('data-sfid') + '&editmode=3&sid=' + getParameterByName('sid'));
            else
                $("#lnkMoreEditOptions").attr('style', 'display:none');

            $(".popupoverlay").show();
            $(".reviewpopup_cont").show();

            //koko2
            $(".datacontainer textarea").val($(this).html());
            $("#note").val(note);

            $(".datacontainer textarea").elastic();
            $(".datacontainer textarea").trigger('update');
        } //end else


    });

    $(".divcontent").scroll(function () {
        $(".popupoverlay").css({ top: $(this).scrollTop() })
    });

    $(".close_btn").click(function () {
        var AudioPlayer = $("#jquery_jplayer_1");
        // jplayer
        AudioPlayer.jPlayer('pause')
        $(".popupoverlay").hide();
        $(".reviewpopup_cont").hide();
    });

    // popup buttons actions
    $('#approve').click(function () {
        var ed = $('.reviewpopup_cont');

        lastEditedDiv.attr("data-revnote", $('#note').val());
        // Do you ajax call here, window.setTimeout fakes ajax call
        $('.absLoad.loading').show(); // Show progress
        var calledFuncName = 'ApproveSessionContentItem';
        var calledHandlerName = 'ReviewerHandler.ashx';
        if ($('#IsSessionStartHidden').val() == '1') {
            calledFuncName = 'ApproveSessionStart';
            calledHandlerName = 'SessionStartHandler.ashx';
        }
        jQuery.ajax({
            cache: false,
            type: 'post',
            url: calledHandlerName, //'ReviewerHandler.ashx',
            data: {
                funcname: calledFuncName,
                sid: $('#MainContent_SessionIDHidden').val(), //used for item 
                scid: $('#SessionContentItemIDHidden').val(), //used for item 
                sfid: $('#SessionContentItemIDHidden').val(), //used for stat this val contain fileID of start
                reviewernote: $('#note').val() //used for item and start
            },
            success: function (response) {

                if (response == '-2') {
                    alert('لا يجوز الموافقة علي نص مدرج تحت متحدث او بند غير معرف');
                    $('.absLoad.loading').hide();
                }
                else
                    if (response != '1') {
                        alert('لقد حدث خطأ');
                    }
                    else {

                        $('#note').val("");

                        if (lastEditedDiv.hasClass('reditem')) {
                            lastEditedDiv.removeClass("reditem");
                            $('#spnRejectCount').text(parseInt($('#spnRejectCount').text()) - 1);
                        } else if (lastEditedDiv.hasClass('greenitem')) {
                            $('#spnFixCount').text(parseInt($('#spnFixCount').text()) - 1);
                            lastEditedDiv.removeClass("greenitem");
                        } else if (lastEditedDiv.hasClass('blueitem')) {
                            $('#spnModAfterApprove').text(parseInt($('#spnModAfterApprove').text()) - 1);
                            lastEditedDiv.removeClass("blueitem");
                        }

                        if ((parseInt($('#spnRejectCount').text()) == 0) &&
                       (parseInt($('#spnFixCount').text()) == 0) &&
                       (parseInt($('#spnModAfterApprove').text()) == 0)) {

                            changeButtonsForSessionStatus();

                        }


                        //lastEditedDiv.closest("div").removeClass("reditem");
                        //koko

                        $('.absLoad.loading').hide();
                    }
            },
            error: function () {
                $('.absLoad.loading').hide();
            }
        });
        // close popup
        $.fancybox.close()
        $(".popupoverlay").hide();
        $(".reviewpopup_cont").hide();
    })



    // popup buttons actions
    $('.approveSessionFile').click(function () {

        var currFile = $(this);
        var fileID = currFile.attr('data-fileid');
        var fileName = currFile.text();


       if(confirm("هل انت متأكد انك ترغب في الموافقة على جميع المقاطع الخاصة بالملف" + fileName)) {
           $('.absLoad.loading').show();
            jQuery.ajax({
            cache: false,
            type: 'post',
            url: 'ReviewerHandler.ashx', //'ReviewerHandler.ashx',
            data: {
                funcname: 'ApproveRejectedItemsInFile',
                sfid:fileID
            },
            success: function (response) {
                    if (response != '1') {
                        alert('لقد حدث خطأ');
                    }
                    else {
                        alert('لقد تمت الموافقه على جميع مقاطع الملف ' + fileName);
                        $('.absLoad.loading').hide();
                        location.reload();
                    }
            },
                error: function () {
                    alert('لقد حدث خطأ');
                $('.absLoad.loading').hide();
            }
        });
       }

       
        // close popup
        $.fancybox.close()
        $(".popupoverlay").hide();
        $(".reviewpopup_cont").hide();
    })



    $('#reject').click(function () {
        //var ed = $('.reviewpopup_cont')
        // Do you ajax call here, window.setTimeout fakes ajax call
        $('.absLoad.loading').show();

        lastEditedDiv.attr("data-revnote", $('#note').val())

        var calledFuncName = 'RejectSessionContentItem';
        var calledHandlerName = 'ReviewerHandler.ashx';
        if ($('#IsSessionStartHidden').val() == '1') {
            calledFuncName = 'RejectSessionStart';
            calledHandlerName = 'SessionStartHandler.ashx';
        }
        jQuery.ajax({
            cache: false,
            type: 'post',
            url: calledHandlerName, //'ReviewerHandler.ashx',
            data: {
                funcname: calledFuncName,
                scid: $('#SessionContentItemIDHidden').val(), //used ofr item
                sid: $('#MainContent_SessionIDHidden').val(), //used ofr item
                sfid: $('#SessionContentItemIDHidden').val(), //used for stat this val contain fileID of start
                reviewernote: $('#note').val() //used for start and item
            },
            success: function (response) {
                if (response != '1') {
                    alert('لقد حدث خطأ');
                    //ed.setProgressState(0); // Hide progress
                    //ed.setContent(html);

                    //hide approve and final approve buttons
                    $('.btnApproveSession').css("display", "none");
                    $('.btnFinalApproveSession').css("display", "none");
                    changeButtonsForSessionStatus();
                    $('.absLoad.loading').hide();
                }
                else {
                    $('#note').val("");
                    lastEditedDiv.removeClass("greenitem");
                    lastEditedDiv.removeClass("blueitem");

                    lastEditedDiv.addClass("reditem"); //removeClass("myClass noClass").addClass("yourClass");
                    $('#spnRejectCount').text(parseInt($('#spnRejectCount').text()) + 1);

                    changeButtonsForSessionStatus();
                    $('.absLoad.loading').hide();

                }
                $('.absLoad.loading').hide();
            },
            error: function () {
                $('.absLoad.loading').hide();
            }
        });
        // close popup
        $.fancybox.close()
        $(".popupoverlay").hide();
        $(".reviewpopup_cont").hide();
    });

    function htmlEncode(value) {
        return $('<div/>').text(value).html();
    }

    $('#save').click(function () {
        $('.absLoad.loading').show();
        //var ed = $('.reviewpopup_cont')
        // Do you ajax call here, window.setTimeout fakes ajax call
        //ed.setProgressState(1); // Show progress
        lastEditedDiv.attr("data-revnote", $('#note').val())

        var calledFuncName = 'UpdateSessionContentItemText';
        var calledHandlerName = 'ReviewerHandler.ashx';
        if ($('#IsSessionStartHidden').val() == '1') {
            calledFuncName = 'SaveSessionStart';
            calledHandlerName = 'SessionStartHandler.ashx';
        }

        jQuery.ajax({
            cache: false,
            type: 'post',
            url: calledHandlerName, //'ReviewerHandler.ashx',
            data: {
                funcname: calledFuncName,
                scid: $('#SessionContentItemIDHidden').val(), //used ofr item,
                sid: $('#MainContent_SessionIDHidden').val(), //used ofr start
                sfid: $('#SessionContentItemIDHidden').val(), //used ofr start
                contentitemtext: htmlEncode($("textarea.tinymce").val()), //used ofr start and item
                reviewernote: $('#note').val()//used ofr start and item
            },
            success: function (response) {
                if (response != '1') {
                    alert('لقد حدث خطأ');
                    //ed.setProgressState(0); // Hide progress
                    //ed.setContent(html);
                } else {
                    $('#note').val("");
                    lastEditedDiv.html($("textarea.tinymce").html());
                    //lastEditedDiv.val($("textarea.tinymce").html());
                }
                $('.absLoad.loading').hide();
            },
            error: function () {
                $('.absLoad.loading').hide();
            }
        });
        // close popup
        $.fancybox.close()
        $(".popupoverlay").hide();
        $(".reviewpopup_cont").hide();
    });

    $('.btnApproveSession').click(function () {

        $('.absLoad.loading').show()
        $(this).attr('disabled', 'disabled');
        //var ed = $('.reviewpopup_cont')
        // Do you ajax call here, window.setTimeout fakes ajax call
        jQuery.ajax({
            cache: false,
            type: 'post',
            url: 'ReviewerHandler.ashx',
            data: {
                funcname: 'ApproveSession',
                sid: $('#MainContent_SessionIDHidden').val()//,
                //reviewernote: $('#note').val()
            },
            success: function (response) {

                if (response != 'true') {
                    alert('لقد حدث خطأ');
                    // Hide progress
                    //ed.setContent(html);
                }
                else {
                    changeButtonsForSessionStatus();
                    alert('يتم الآن تجهيز ملفات المضبطة.. هذه العملية تستغرق عدة دقائق .. سيقوم التطبيق بإرسال رسالة بريد إلكتروني إليكم بمجرد الإنتهاء من تجهيزها')
                    $('.btnFinalApproveSession').css("display", "none");

                }
                $('.absLoad.loading').hide();
            },
            error: function () {
                $('.absLoad.loading').hide();
            }
        });
        // close popup
        $.fancybox.close()
        $(".popupoverlay").hide();
        $(".reviewpopup_cont").hide();
    })
    $('.btnFinalApproveSession').click(function () {
        $('.absLoad.loading').show();
        $(this).attr('disabled', 'disabled');
        //var ed = $('.reviewpopup_cont')
        // Do you ajax call here, window.setTimeout fakes ajax call
        //ed.setProgressState(1); // Show progress
        jQuery.ajax({
            cache: false,
            type: 'post',
            url: 'ReviewerHandler.ashx',
            data: {
                funcname: 'FinalApproveSession',
                sid: $('#MainContent_SessionIDHidden').val(),
                reviewernote: $('#note').val()
            },
            success: function (response) {

                if (response != 'true') {
                    alert('لقد حدث خطأ');
                    //ed.setProgressState(0); // Hide progress
                    //ed.setContent(html);
                }
                else {
                    changeButtonsForSessionStatus();
                    alert('يتم الآن تجهيز الملفات النهائيه للمضبطة وارسالها الي البرلمان الالكتروني .. هذه العملية تستغرق عدة دقائق .. سيقوم التطبيق بإرسال رسالة بريد إلكتروني إليكم بمجرد الإنتهاء من تجهيزها')
                }
                $('.absLoad.loading').hide();
            },
            error: function () {
                $('.absLoad.loading').hide();
            }
        });
    })

    $('.gotofile').click(function (e) {
        var tlink = $(this)
        if (tlink.attr('href')) {
            var num = $(tlink.attr('href')).offset().top - $('#MainContent_pnlContent .divcontent').offset().top + $('#MainContent_pnlContent .divcontent').scrollTop() - 5;
            $('#MainContent_pnlContent .divcontent').stop().animate({
                scrollTop: num
            })
        }
        e.preventDefault()
    })

    $('.approveSessionFile').click(function (e) {
        var tlink = $(this);
        //alert(tlink.attr('href'));

        //usama march
        //here approve session file contents all session content items under it
        //call handler then update page (reload page)

        e.preventDefault();
    })

})
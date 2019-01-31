<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdminSecurity.aspx.cs" Inherits="TayaIT.Enterprise.EMadbatah.Web.AdminSecurity"
    MasterPageFile="~/Site.master" Title="المضبطة الإلكترونية - ادارة المستخدمين" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <link rel="stylesheet" type="text/css" href="styles/jquery.fancybox-1.3.4.css" />
    <script type="text/javascript" src="scripts/jquery.fancybox-1.3.4.pack.js"></script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="largerow addnewusercont">
        <!--old classes smallbtn , addnew-->
        <a href="#" class="actionbtnsmenu AddNewUserWin">إضافة مستخدم جديد</a>
    </div>
    <input id="EditableItemID" type="hidden" value="" />
    <input id="EditableItemRole" type="hidden" value="" />
    <input id="UserId" type="hidden" runat="server" value="" />
    <div id="datacont1" class="datacont1 largerow">
        <table border="0" cellspacing="0" cellpadding="0">
            <thead>
                <tr>
                    <th class="widthnum1 basic_info">
                        اسم المستخدم
                    </th>
                    <th class="widthnum1 basic_info">
                        اسم الدومين
                    </th>
                    <th class="widthnum1 basic_info">
                        البريد الالكترونى
                    </th>
                    <th class="widthnum2 basic_info">
                        المهمة
                    </th>
                    <th class="widthnum2 basic_info">
                        الحالة
                    </th>
                    <th class="editbuttons">
                    </th>
                </tr>
            </thead>
            <tbody id="db_users" runat="server">
                <!-- <tr>
                        <td>
                            محمد عبد اللطيف
                        </td>
                        <td>
                            develop/mabdellatief
                        </td>
                        <td>
                            mabdellatief@fnc.com
                        </td>
                        <td>
                            مصحح
                        </td>
                        <td>
                            active
                        </td>
                        <td class="editbuttons">
                            <a href="#" class="edit">[تعديل]</a> <a href="#" class="remove">[حذف]</a>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            sasas23 121
                        </td>
                        <td>
                            develop/dinad
                        </td>
                        <td>
                            asafe3@fnc.com
                        </td>
                        <td>
                            ادمين
                        </td>
                        <td class="inactive">
                            inactive
                        </td>
                        <td class="editbuttons">
                            <a href="#" class="edit">[تعديل]</a> <a href="#" class="remove">[حذف]</a>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            محمد عبد اللطيف
                        </td>
                        <td>
                            develop/mabdellatief
                        </td>
                        <td>
                            mabdellasdef@fnc.com
                        </td>
                        <td>
                            مصحح
                        </td>
                        <td>
                            active
                        </td>
                        <td class="editbuttons">
                            <a href="#" class="edit">[تعديل]</a> <a href="#" class="remove">[حذف]</a>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            ahmed saber
                        </td>
                        <td>
                            develop/saber
                        </td>
                        <td>
                            saber@gmail.com
                        </td>
                        <td>
                            ادمين
                        </td>
                        <td class="inactive">
                            inactive
                        </td>
                        <td class="editbuttons">
                            <a href="#" class="edit">[تعديل]</a> <a href="#" class="remove">[حذف]</a>
                        </td>
                    </tr>-->
            </tbody>
        </table>
    </div>
    <div class="drow">
        <div class="addnewusercont fr">
            <a href="#" class="actionbtnsmenu AddNewUserWin">إضافة مستخدم جديد</a>
        </div>
        <div class="arrowanchor fl">
        </div>
        <a class="fl anchor" href="AdminHome.aspx">عودة للصفحة الرئيسية</a>
        <!--<input type="button" class="btn fl" value="عودة للصفحة الرئيسية">-->
        <div class="clear">
        </div>
    </div>
    <div class="editingTools">
        <form id="adminform" action="dummy.php" method="post">
        <ul class="list">
            <li class="widthnum1">
                <div class="padd">
                    <input name="username" type="text" class="textfield" value="محمد عبد اللطيف" size="27" />
                </div>
            </li>
            <li class="widthnum1">
                <div class="padd">
                    <input disabled="disabled" name="domainName" type="text" class="textfield" value="develop/mabdellatief"
                        size="27" />
                </div>
            </li>
            <li class="widthnum1">
                <div class="padd">
                    <input name="email" type="text" class="textfield" value="mabdellatief@fnc.com" size="27" />
                </div>
            </li>
            <li class="widthnum2">
                <div class="padd">
                    <select name="task">
                        <option value="1">مدير النظام</option>
                        <option value="2">مصحح</option>
                        <option value="3">مراجع</option>
                        <option value="4">مراجع و مصحح</option>
                        <option value="5">مراجع ملف</option>
                    </select>
                </div>
            </li>
            <li class="widthnum2">
                <div class="padd">
                    <input name="active" type="checkbox" value="">
                    <label for="active">
                        نشط</label>
                </div>
            </li>
        </ul>
        <div class="tools">
            <a href="#" class="save">[حفظ]</a> <a href="#" class="cancel">[الغاء]</a>
        </div>
        </form>
    </div>
    <script type="text/javascript">
  
		$(document).ready(function(){
            // vars
			var mainWin = $('.editingTools');
            // cancel button
            $('.cancel',mainWin).click(function(event){
                $('.editingTools').hide();
                event.preventDefault();
            })
            // hover over rows
            $('#datacont1 tbody tr').live("mouseover mouseout", function(event) {
              if ( event.type == "mouseover" ) {
                if($(".editingTools").css("display")== "block" && $('#EditableItemID').val()== $(this).attr("data-id") )
            {
				$(this).addClass('hover').find('.editbuttons').hide();
            }
            else
            {$(this).addClass('hover').find('.editbuttons').show()}
              } else {
                $(this).removeClass('hover').find('.editbuttons').hide()
              }
            });
			// validation
			var v = $("#adminform").validate({
				rules: {
					username: "required",
					email: {
						required: true,
						email: true
					}
				},
				messages: {
					username: {
						required: "أدخل اسم المستخدم",
					},
					email: "أدخل البريد الإلكتروني"
				}
			});
			// new user window
			$(".AddNewUserWin").fancybox({
				type : 'inline',
				href : '#addNewUser',
				autoDimensions : false,
				padding: 20,
				width: 900
			});

            $(".AddNewUserWin").click(function () {
            $('.absLoad.loading').show();
				jQuery.ajax({
                    cache: false,
                    type: 'post',
                    url: 'AdminHandler.ashx',
                    data: {
                        funcname: 'GetAllUsersFromAD',
                    },
                    success: function (response) {
                        
                        if(response != '') {
                             var ed = $('#ad_users');
                             ed.html(response);
                        }
                        $('.absLoad.loading').hide();

                    },
                    error: function () {
                    $('.absLoad.loading').hide();
                    }
                });
			});
            // last clicked item
            var lastClickedparentTR;
			// click on the buttons edit & remove
			$('#datacont1 .editbuttons .edit').live('click',function(e){
				// reset form
				v.resetForm();
				//
				var theButton = $(this)
				var parentTR = lastClickedparentTR = theButton.parents('tr');
				// insert the data
				var td = $('td',parentTR).not('.editbuttons');

                // set the current editable id
                
                $('#EditableItemID').val(parentTR.attr("data-id"));
                $('#EditableItemRole').val(parentTR.attr("data-role"));
                
				// save the target row
				mainWin.data('storeROW',td);
				// loop throw the columns and update the inputs
				td.each(function(index){
					var text = $(this).text();
					switch(index){
						case 0:
							$('input[name=username]',mainWin).val(text)
						break;
						case 1:
							$('input[name=domainName]',mainWin).val(text)
						break;
						case 2:
							$('input[name=email]',mainWin).val(text)
						break;
						case 3:
                            //$('select[name=task] option',mainWin).val(parentTR.attr("data-role"));
							//$('select[name=task] option',mainWin).removeAttr('selected').filter(':contains('+text+')').attr('selected', 'selected');
                            $('select[name=task] option',mainWin).filter(':eq('+(parentTR.attr("data-role") - 1) +')').attr('selected', 'selected');
						break;
						case 4:
							if($.trim(text) == 'نشط'){
								$('input[name=active]',mainWin).attr('checked', 'checked');
							}else{
								$('input[name=active]',mainWin).removeAttr('checked');
							}
						break;
					}
				})
				// show the edit
				mainWin.css({top:parentTR.offset().top}).show();
				e.preventDefault()
			})
			$('.tools .save',mainWin).click(function(e){
				// collect text before updating
				var storeROW = mainWin.data('storeROW')
                $('.absLoad.loading').show();
				// success
				/*var ajaxOptions = {
					success:function(){
						// remove loading
						$('.tools',mainWin).removeClass('loading')*/
						// loop throw the columns and update them
                        
                        var UserName;
                        var domainName;
                        var email;
                        var role;
                        var roleStr ;
                        var active;
						storeROW.each(function(index){
							switch(index){
								case 0:
									UserName = $('input[name=username]',mainWin).val()
								break;
								case 1:
									domainName = $('input[name=domainName]',mainWin).val()
								break;
								case 2:
									email = $('input[name=email]',mainWin).val()
								break;
								case 3:
									role = $('select[name=task] option:selected',mainWin).val()
                                    roleStr = $('select[name=task] option:selected',mainWin).text()
                                    //role = $('select[name=task] option:selected',mainWin).attr('data-role')
								break;
								case 4:
									if($('input[name=active]',mainWin).is(':checked')){
										active = 'true'
										$(this).removeClass('inactive')
									}else{
										active = 'false'
										$(this).addClass('inactive')
									}
								break;
							}
							//$(this).text(text)
						})

                        jQuery.ajax({
                            cache: false,
                            type: 'post',
                            url: 'AdminHandler.ashx',
                            data: {
                                funcname: 'UpdateUser',
                                uid:  $('#EditableItemID').val(),
                                userdomainname:  $.trim(domainName),
                                useremail: $.trim(email),
                                userisactive: active,
                                userroleid: role,
                                username:$.trim(UserName)
                                
                            },
                            success: function (response) 
                            {
                        
                                if (response != 'true') {
                                    alert('لقد حدث خطأ');
                                }
                                else{
                                    lastClickedparentTR.attr('data-role', role);
                                    $('#EditableItemRole').val(role);
                                    
                                    var td = $('td',lastClickedparentTR).not('.editbuttons');
                                    td.each(function(index){
					                    
                                        
					                    switch(index){
						                    case 0:
							                    $(this).text($.trim(UserName));
						                    break;
						                    case 1:
							                    $(this).text($.trim(domainName));
						                    break;
						                    case 2:
							                    $(this).text($.trim(email));
						                    break;
						                    case 3:
                                                //$('select[name=task] option',mainWin).val(parentTR.attr("data-role"));
							                    //$('select[name=task] option',mainWin).removeAttr('selected').filter(':contains('+text+')').attr('selected', 'selected');
                                                 $(this).text(roleStr);
						                    break;
						                    case 4:
							                    if(active == 'true'){
								                    $(this).attr('checked', 'checked');
                                                    $(this).text('نشط');
							                    }else{
								                    $(this).removeAttr('checked');
                                                    $(this).text('غير نشط');
							                    }
						                    break;
					                    }
                                     });

                                }
$('.absLoad.loading').hide();
                            },
                            error: function () {
                            $('.absLoad.loading').hide();
                            }
                        });
						// hide the window
						mainWin.hide();
                e.preventDefault()
			})
        $("#addSeletedUsers").click(function(){
            $('.absLoad.loading').show();
            $('#ad_users input[type=checkbox]:checked').each(function() {
                var parentTR = $(this).parents('tr');
                // insert the data
                var UserName;
                var domainName;
                var email;
                var role;
                var active;
                var roleStr;
                var td = $('td',parentTR);
                td.each(function(index){
                    var text = $(this).text();
                    switch(index){
                        case 0:
                            UserName = $.trim(text);
                        break;   
                        case 1:
                            domainName = $.trim(text);
                        break;                   
                        case 2:
                            email = $.trim(text);
                        break;  
                        case 3:
                            role = $('select[name=task] option:selected',this).val();
                            roleStr= $('select[name=task] option:selected',this).text();
                        break;
                    }
                });
                active = false;
                jQuery.ajax({
                    cache: false,
                    type: 'post',
                    url: 'AdminHandler.ashx',
                    dataType:'json',
                    data: {
                        funcname: 'AddNewUser',
                        userdomainname: domainName,
                        useremail: email,
                        userisactive: active,
                        userroleid: role,
                        username:UserName
                    },
                    success:function(response){
                        //missing parameter
                        if(response == null){
                            alert('لقد حدث خطأ');
                            return;
                        }else{
                            if (response.Message == "fail" && response.IsDuplicate == "false") {
                                alert('لقد حدث خطأ');
                            }else if (response.Message == "fail" && response.IsDuplicate == "true") {
                                alert('المستخدم ' + UserName + ' موجود بالفعل');
                            }else{//undeleted or new
                                var newUserRow = "<tr data-role=\""+role+"\" data-id=\""+response.ID+"\" class=\"\">\n" +
                                "<td>"+UserName+"</td>\n" +
                                "<td>"+domainName+"</td>\n" +
                                "<td>"+email+"</td>\n" +
                                "<td>"+roleStr+"</td>\n" ;
                                if(active == 'true')
                                    newUserRow += "<td>نشط</td>\n";
                                else
                                    newUserRow += "<td class=\"inactive\">غير نشط</td>\n";
                                    newUserRow += "<td class=\"lastchild\"><div class=\"editbuttons\" style=\"display: none;\"><a class=\"edit\" href=\"\&quot;#\&quot;\">[تعديل]</a><a class=\"remove\" href=\"#inline2\">[حذف]</a></div></td>\n</tr>";
                                    $('#MainContent_db_users tr:last').after(newUserRow);
                            }
                        }
                    },error: function () {
                        alert('لقد حدث خطأ');
                    }
                });
            });
            // hide loading and close
            $('.absLoad.loading').hide();
            $.fancybox.close();
        });
        // remove button
 $('.editbuttons .remove').live('click',function(e){

            var answer = confirm("هل أنت متأكد أنك تريد الحذف ؟");
            var sameUser = false;
            var uid = $(this).parents('tr').attr("data-id");
            if(uid == $('#MainContent_UserId').val())
            {
                var answer2 = confirm("انت تحاول حذف حسابك، هل انت متأكد؟");
                if(answer2)
                    sameUser = true;
                else
                    answer=false;

            }
            if(answer)
            {
            $('.absLoad.loading').show();
             		jQuery.ajax({
                    cache: false,
                    type: 'post',
                    url: 'AdminHandler.ashx',
                    data: {
                        funcname: 'RemoveUser',
                        uid:  $(this).parents('tr').attr("data-id")
                    },
                    success: function (response) 
                    {
                        
                        if (response != '1')  {//| response != 'true')
                            alert('لقد حدث خطأ');
                                    }
                        else
                        {
                            if(sameUser)
                                window.location = "Default.aspx";
                        }
                        $('.absLoad.loading').hide();
                    }
                });   
                $(this).parents('tr').remove();  
			}
            });            
			
		})//end document.ready

    </script>
    <div class="displaynone">
        <div id="addNewUser">
            <div class="loading">
            </div>
            <div class="datacont2 largerow">
                <div style="height: 280px; width: 100%; font-size: 12px; overflow: auto;">
                    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="addnewuserstbl">
                        <!--caption>
                        إضافة مستخدم جديد
                    </caption-->
                        <thead>
                            <tr>
                                <th>
                                    إسم المستخدم
                                </th>
                                <th>
                                    إسم الدومين
                                </th>
                                <th>
                                    البريد الالكترونى
                                </th>
                                <th>
                                    مهمة المستخدم
                                </th>
                            </tr>
                        </thead>
                        <tbody id="ad_users">
                            <!-- new row-->
                            <!--<tr>
                        <td>
                            <input name="" type="checkbox" value="" />
                            محمد عبد اللطيف
                        </td>
                        <td>
                            develop/mabdellatief
                        </td>
                        <td>
                            asaber@gmail.com
                        </td>
                        <td>
                            <select class="fitAll" name="task">
                                <option value="1">مصحح</option>
                                <option value="2">ادمين</option>
                            </select>
                        </td>
                    </tr>-->
                            <!-- end of row-->
                        </tbody>
                    </table>
                </div>
            </div>
            <div class="tex_align_en addnewusercont">
                <!--old classes smallbtn addnew-->
                <input name="" type="button" id="addSeletedUsers" class="actionbtnsmenu" value="إضافة الإختيارات">
            </div>
        </div>
    </div>
</asp:Content>

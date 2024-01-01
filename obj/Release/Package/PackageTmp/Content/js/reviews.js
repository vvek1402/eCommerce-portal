$(document).ready(function () {


    $.ajax({
        type: "POST",
        url: "/admin/Admin/getreview",
        success: function (result) {
            var i = 0;
            var s = "";
            for (i = 0 ; i < result.review.length ; i++)
            {
                s+="<div class='customer-message align-items-center' style='padding:20px;'>"+ 
                        "<div class='text-truncate message-title'>"+
                           result.review[i]+
                            "</div>"+
                            "<div class='small message-time'>"+
                                 result.uname[i]+ " . <a href='#'>"+ result.pname[i]+"</a></div>"+
                            "</div>";
            }

            $('#reviews').html(s);
        }
    });
});


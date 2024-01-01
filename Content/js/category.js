$(document).ready(function () {

    $("#cat-btn").click(function () {
      
        var data = $("#cat-form").serialize();
        
        $.ajax({
            type: "POST",
            url: "/admin/Admin/addcategory",
            data: data,
            success: function (response) {
                $(".categoryclass").load("/admin/Admin/getcategory");
            }
        })

    })



})

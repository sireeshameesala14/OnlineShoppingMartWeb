// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function () {

    $.ajax({
        type: "get",
        url: $("#OsmApiUrl").val() + "Product/GetAllProductCategories",
        headers: {
            "Access-Control-Allow-Origin": "*"
        },
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "basic b3NtdXNlcjphcGlAMTIz");
        },
        success: function (result) {
            console.log("the result value is " + result);
            if (result) {
                $.each(result, function (value) {
                    console.log("the value is " + result[value].categoryName);
                    $("#ddlProductCatageory").append($("<option></option>").val(result[value].categoryId).html(result[value].categoryName));
                    /* $("ul.dropdown-menu").append("<li><a><span>" + result[value].catageoryName +"</span></a></li>");*/
                });
            }
        },
        error: function (result) {
            alert("error");
        }
    });
    $(".add-cart").on("click", function (e) {
        $("." + e.target.id).removeAttr("action");
        $("." + e.target.id).attr("action", "/Product/Cart");
        $("." + e.target.id).submit();
    });

    $(".product-deatil").on("click", function (e) {
        $("." + e.target.id).submit();
    });

    $(".remove-cart").on("click", function (e) {
        $("." + e.target.id).submit();
    });


    $(".product-info").on("click", function (e) {
        $("." + e.target.id).submit();
    });


    $("#register").attr("disabled", "disabled");
    $("#Email").focusout(function () {
        $.ajax({
            type: "get",
            url: $("#OsmApiUrl").val() + "User/IsUserExist?userName=" + $("#Email").val(),
            headers: {
                "Access-Control-Allow-Origin": "*"
            },
            beforeSend: function (xhr) {
                xhr.setRequestHeader("Authorization", "basic b3NtdXNlcjphcGlAMTIz");
            },
            success: function (result) {
                console.log("the result value " + result);
                if (!result) {
                    $("#register").removeAttr("disabled");
                    $("#Email").removeClass("invalidEmail");
                    $("#Email").addClass("validEmail");
                    $("#invalid-email-error").hide();
                }
                else {
                    $("#Email").removeClass("validEmail");
                    $("#Email").addClass("invalidEmail");
                    $("#register").attr("disabled", "disabled");
                    $("#invalid-email-error").show();
                }

            }
        });
    });
});
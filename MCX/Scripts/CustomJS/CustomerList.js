/// <reference path="../jquery-1.10.2.intellisense.js" />
/// <reference path="../jquery-1.10.2.min.js" />
//$(document).ready(function () {



//}); 
$("#gridBody").find('tr').hover(function () {
    $(this).addClass('onFoucsIn');
}, function () {
    $(this).removeClass('onFoucsIn');
});

$(document).on("click", "#gridBody tr", function (e) {
    $("#gridBody").find('tr').each(function () {
        $(this).removeClass('onTableTrClicked');
    });
    $(this).addClass('onTableTrClicked');

    console.log('vk');
    $('#RecordEdit').attr('href', "/Customers/Edit/" + (this.id));
    $('#RecordDetail').attr('href', "/Customers/Details/" + (this.id));
    $('#RecordDelete').attr('href', "/Customers/Delete/" + (this.id));

    //$("#gridBody").find('tr').each(function () {
    //    $(this).removeClass('onTableTrClicked');
    //});
});

$(document).on("click", "#RecordEdit,#RecordDetail,#RecordDelete", function () {
 
    if ($(this).attr("href") == "#") {
        alert("Please select any row!");
        return false;
    }
});
function ReBindGrid() {
    debugger;
    if ($("#searchFromGrid").val().trim().length > 0) {
        //$("#gridBody").html("");
        ajaxCallGet("/Customers/Index", "{'searchString':'" + $("#searchFromGrid").val() + "'}");
        //,'sortOrder':'Date','currentFilter':'','page':'1'
    }
}

function ajaxCallGet(url, parameter) {
    //JSON.stringify(parameter)
    $.ajax({ type: "GET", url: url, data: { searchString: $("#searchFromGrid").val() }, contentType: "application/Json", success: function () { console.log("Successfully bind fresh data to GRID"); } });
}
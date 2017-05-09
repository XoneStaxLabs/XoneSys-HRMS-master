
function GetProjectType() {

    var ddlBox = $("#ddlProjectTypeID");

    $.ajax({
        url: "/Project/GetProjectType",
        async: false
    }).done(function (data) {

        ddlBox.empty();
        ddlBox.append('<option value="0">Select</option>').selectpicker('refresh');
        $.each(data, function (i, item) {

            ddlBox.append('<option value=' + item.ProjTypID + '>' + item.ProjectType + '</option>').selectpicker('refresh');
        });
    });
}

function GetCompansationType() {

    var ddlBox = $("#ddlProcompTypID");

    $.ajax({
        url: "/Project/GetCompansationType",
        async: false
    }).done(function (data) {

        ddlBox.empty();
        ddlBox.append('<option value="0">Select</option>').selectpicker('refresh');

        $.each(data, function (i, item) {

            ddlBox.append('<option value=' + item.ProcompTypID + '>' + item.ProCompType + '</option>').selectpicker('refresh');
        });

    });
}

function GetGrade(DesignationID) {

    var ddlBox = $("#GradeID");

    $.ajax({
        url: "/Project/GetGrade",
        data: { DesigID: DesignationID },
        async: false
    }).done(function (data) {
        $("#GradeID").empty().selectpicker('refresh');
        ddlBox.append('<option value="0">--Select--</option>').selectpicker('refresh');
        $.each(data, function (i, item) {    
           
            ddlBox.append('<option value=' + item.GradeID + '>' + item.Gradename + '</option>').selectpicker('refresh');
                
        });

    });



}

function GetDesignation() {

    var ddlBox = $("#DesigID");

    $.ajax({
        url: "/Project/GetDesignation",
        async: false
    }).done(function (data) {

        ddlBox.empty();
        ddlBox.append('<option value="0">--Select--</option>').selectpicker('refresh');

        $.each(data, function (i, item) {

            ddlBox.append('<option value=' + item.DesigID + '>' + item.DesigName + '</option>').selectpicker('refresh');
        });

    });
}


$(document).ready(function () {

    GetProjectType();   
    GetCompansationType();
    GetDesignation();

    $.validator.addMethod("check_item_dropdown", function (value, element) {
        return this.optional(element) || value != 0;
    }, "Please select an item from the dropdown list.");

    $.validator.addMethod("dateformat", function (value, element) {
        return value.match(/^\d\d?\/\d\d?\/\d\d\d\d$/);
    }, "Please enter a date in the format dd/mm/yyyy.")
    $.validator.addMethod('time', function (value, element, param) {
        return value == '' || value.match(/^([01][0-9]|2[0-3]):[0-5][0-9]$/);
    }, 'Enter a valid time: hh:mm');
     

    $("#ddlProjectTypeID").change(function () {            
        if ($(this).val() != 0) {
            $("label[for='ddlProjectTypeID']").text('');
        }
        else {
            $("label[for='ddlProjectTypeID']").text('Please select an item from the dropdown list.');
        }
         
        
    })
    $("#DesigID").change(function () {            
        if ($(this).val() != 0) {
            $("label[for='DesigID']").text('');
        }
        else {
            $("label[for='DesigID']").text('Please select an item from the dropdown list.');
        }
    })
    $("#GradeID").change(function () {            
        if ($(this).val() != 0) {
            $("label[for='GradeID']").text('');
        }
        else {
            $("label[for='GradeID']").text('Please select an item from the dropdown list.');
        }
    })

    $("#ddlProcompTypID").change(function () {          
        if ($(this).val() != 0) {
            $("label[for='ddlProcompTypID']").text('');
        }
        else {
            $("label[for='ddlProcompTypID']").text('This field is required');
        }
    })

    $('#ProjFrom').change(function () {
        $(this).blur();
    })

    $('#ProjTo').change(function () {
        $(this).blur();
    })

    $("#ddlEmployeeList").change(function () {
             
        if ($(this).val() != null) {
            $("label[for='ddlEmployeeList']").text('');
        }
        else {
            $("label[for='ddlEmployeeList']").text('This field is required');
        }

            

    })     
        
     
        
    $("#ProjectForm").on("click", '.RemoveInterviewQus', function (e) {
        $(this).closest("div.shiftCreate").remove();
        e.preventDefault();
    });
          
    //$(document).on('blur', '.empnos', function () {
    //    //alert("Hai");
    //    var sum = 0;
    //    $('.empnos').each(function () {
    //        sum += Number($(this).val());
    //    });
    //    $("#employeeNoID").val(sum);

    //    $('#ddlEmployeeList').selectpicker({
    //        maxOptions: $("#employeeNoID").val()
    //    });

    //})

    $("#DesigID").change(function () {
       
        GetGrade($(this).val());

    })
})

 
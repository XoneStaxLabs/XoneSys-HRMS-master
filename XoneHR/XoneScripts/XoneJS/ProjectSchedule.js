function GetDesignation() {
    var ddlBox = $("#DesigID");

    $.ajax({
        url: "/Project/GetDesignation",
        async: false
    }).done(function (data) {
        ddlBox.empty();
        //ddlBox.append('<option value="0">--Select--</option>').selectpicker('refresh');
        $.each(data, function (i, item) {
            ddlBox.append('<option value=' + item.DesigID + '>' + item.DesigName + '</option>').selectpicker('refresh');
        });

        GetGrade($("#DesigID").val(), $("#HidProjID").val(), $("#AssignShiftID").val());
    });
}

function GetGrade(DesigID, ProjID, ShiftID) {
    var ddlBox = $("#GradeID");

    $.ajax({
        url: "/Project/GetEmpAssignGrade",
        data: { Desig_ID: DesigID, ProjID: ProjID, ShiftID: ShiftID },
        async: false
    }).done(function (data) {
        $("#GradeID").empty().selectpicker('refresh');
        ddlBox.append('<option value="0">--Select--</option>').selectpicker('refresh');
        $.each(data, function (i, item) {
            ddlBox.append('<option value=' + item.GradeID + ' data-GradeEmpNo=' + item.PrjGradeEmpNO + '>' + item.Gradename + '</option>').selectpicker('refresh');
        });
    });
}

function GetProjectEmployeeList(ID) {
    var ddlBox = $("#EmpID");

    $.ajax({
        url: "/Project/GetProjectEmployeeList",
        data: { GradeID: ID },
        async: false
    }).done(function (data) {
        ddlBox.empty();
        ddlBox.append('<option value="0">--Select--</option>').selectpicker('refresh');

        $.each(data, function (i, item) {
            var text;

            text = item.CandName + "( " + item.EmpSubType + " )";

            ddlBox.append('<option value=' + item.EmpID + '>' + text + '</option>').selectpicker('refresh');
        });
    });
}

function GetShift(ProjID) {
    var ddlBox = $("#ShiftID");
    var ddlBox1 = $("#AssignShiftID");

    $.ajax({
        url: "/Project/GetProjectShift",
        data: { ProjID: ProjID },
        async: false
    }).done(function (data) {
        ddlBox.empty().selectpicker('refresh');;
        ddlBox.append('<option value="0">--Select--</option>').selectpicker('refresh');
        $.each(data, function (i, item) {
            ddlBox.append('<option value=' + item.ShiftID + '>' + item.ShiftName + '</option>').selectpicker('refresh');
        });

        ddlBox1.empty().selectpicker('refresh');;
        // ddlBox1.append('<option value="0">--Select--</option>').selectpicker('refresh');
        $.each(data, function (i, item) {
            ddlBox1.append('<option value=' + item.ShiftID + '>' + item.ShiftName + '</option>').selectpicker('refresh');
        });

        GetSchedule($("#AssignShiftID").val());
    });
}

function GetSchedule(ShiftID) {
    $.ajax({
        method: "GET",
        url: "/Project/GetScheduleList",
        data: { ShiftID: ShiftID, ProjID: $("#HidProjID").val() }
    }).done(function (data) {
        var ddlBox = $("#SchID");
        ddlBox.empty().selectpicker('refresh');;
        // ddlBox.append('<option value="0">--Select--</option>').selectpicker('refresh');
        $.each(data, function (i, item) {
            ddlBox.append('<option value=' + item.SchID + '  data-startdate=' + item.StartDate + ' data-enddate= ' + item.EndDate + '>' + item.Schedule + '</option>').selectpicker('refresh');
        });

        GetGrade($("#DesigID").val(), $("#HidProjID").val(), ShiftID);
    });
}

$(document).ready(function () {
    GetShift($("#HidProjID").val());
    GetDesignation();
    $("#Schedule").modal('hide');
    $("#LeaveRelifEmp").modal('hide');

    $("#GradeID").change(function () {
        GetProjectEmployeeList($(this).val());
    });

    $("#DesigID").change(function () {
        GetGrade($(this).val(), $("#HidProjID").val(), $("#AssignShiftID").val());
    });

    $("#AssignShiftID").change(function () {
        GetSchedule($(this).val());
    })

    function MsgAlert(data, icon) {
        $.toast({
            text: data,
            position: 'top-right',
            hideAfter: 2000,
            showHideTransition: 'slide',
            loader: false,
            icon: icon
        })
    }

    $("#ShiftID").change(function () {
        if ($(this).val() != 0) {
            $("label[for='ShiftID']").text('');
        }
        else {
            $("label[for='ShiftID']").text('This field is required');
        }
    })

    $(document).on('change', '.RelifEmp', function () {
        if ($(this).val() != 0) {
            $("#Hidreliefempid").val($(this).val());
            $("#Empnames").text($(this).find('option:selected').text());
            $("#EmpTypes").text($(this).find('option:selected').attr("data-typeNam"));
            $("#EmpMobs").text($(this).find('option:selected').attr("data-mobile"));
            $("#HIdleavedate").text($(this).find('option:selected').attr("data-leavedate"));
            $("#HIdProjectID").val($(this).find('option:selected').attr("data-projectid"));

            $('#ReliefConfChk').attr('checked', false); // Unchecks it

            $("#Relifsmall").modal({ backdrop: 'static' });
            $("#Relifsmall").modal('show');
        }
    })

    $(document).on('click', '#ReliefEmpIDNO', function () {
        $("#Relifsmall").modal('hide');
        // $("#LeaveRelifEmp").modal('hide');

        $.ajax({
            url: "/Project/ProjectLeaveEmpDetails",
            data: { EmpID: $("#AbsEmpID").val(), SchFromDate: $("#HidStartdate").val(), SchToDate: $("#HidEnddate").val(), OffStatus: $("#HidOffStatus").val() }
        }).done(function (data) {
            $("#RelifeEmpList").html(data);
            $("#LeaveRelifEmp").modal('show');
        })
    });

    $(document).on('click', '#ReliefEmpIDYES', function () {
        var Chkvalue = 0;
        if ($("#ReliefConfChk").is(':checked')) {
            Chkvalue = 1;
        }

        $.ajax({
            type: "POST",
            url: "/Project/SaveReliefEmployee",
            data: { EmpLeaveDate: $("#HIdleavedate").text(), RelifeEmpName: $("#Hidreliefempid").val(), AbsEmpID: $("#AbsEmpID").val(), ProjID: $("#ProjID").val(), ReliefConfirmation: Chkvalue }
        }).done(function (data) {
            if (data > 0) {
                MsgAlert('Data Saved Successfully', 'success');
            }
        })

        $("#Relifsmall").modal('hide');
    });
    $(document).on('click', '#BtnCloseRelief', function () {
        $("#LeaveRelifEmp").modal({ backdrop: 'static' });
        $("#LeaveRelifEmp").modal('hide');
        //$("#Schedule").modal('show');
    });
})
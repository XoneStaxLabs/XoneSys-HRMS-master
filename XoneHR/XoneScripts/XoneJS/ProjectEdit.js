 
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


    
    function GetSelectedEmployee(ProjectID) {

      

        $.ajax({
            url: "/Project/GetSelectedEmployee",
            async: false,
            data: { ProjectID: ProjectID, DesigID: $("#HidDesigID").val() }
        }).done(function (data) {

            $("#GradeID").val(data);
            $('#GradeID').selectpicker('refresh');

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
            //ddlBox.append('<option value="0">Select</option>').selectpicker('refresh');
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
       // GetEmployees();
        GetCompansationType();
        GetDesignation();
        GetGrade($("#HidDesigID").val());
        
        $.validator.addMethod("check_item_dropdown", function (value, element) {
            return this.optional(element) || value != 0;
        }, "Please select an item from the dropdown list.");

        $.validator.addMethod("dateformat", function (value, element) {
            return value.match(/^\d\d?\/\d\d?\/\d\d\d\d$/);
        }, "Please enter a date in the format mm/dd/yyyy.")
        $.validator.addMethod('time', function (value, element, param) {
            return value == '' || value.match(/^([01][0-9]|2[0-3]):[0-5][0-9]$/);
        }, 'Enter a valid time: hh:mm');

        $("#btnProjectSubmit").click(function () {

            $("#ProjectForm").validate({
                rules: {
                    ProjFrom: {
                        required: true,
                      //  dateformat: true
                    },
                    ProjTo: {
                        required: true,
                      //  dateformat: true
                    },
                    projTypID: {
                        check_item_dropdown: true,
                        required: true
                    },
                    DesigID: {
                        check_item_dropdown: true,
                        required: true
                    },
                    GradeID: {
                        check_item_dropdown: true,
                        required: true
                    },                    
                    ProjCompAmount: {
                        required: true,
                        number: true
                    },
                    ProcompTypID: {
                        check_item_dropdown: true,
                        required: true
                    }

                }

            });
            if ($("#ProjectForm").valid()) {
                 
                var ProjFrom = $("#ProjFrom").val();
                var ProjTo = $("#ProjTo").val();
                if (ProjFrom <= ProjTo) {
                    $.ajax({

                        type: "POST",
                        url: "/Project/UpdateProject",
                        data: $("#ProjectForm").serialize()

                    }).done(function (data) {

                        if (data) {
                            $.toast({

                                text: "Saved Successfully",
                                position: 'top-right',
                                hideAfter: 3000,
                                showHideTransition: 'slide',
                                loader: false,
                            })

                            window.location.href = "/Project/Index";

                        }
                        else {
                            $.toast({

                                text: "Application Error Please Contact Administrator!",
                                position: 'top-right',
                                hideAfter: 3000,
                                showHideTransition: 'slide',
                                loader: false,
                            })
                        }

                    })
                }
                else {
                    $.toast({

                        text: "Please select a valid From & To Date ",
                        position: 'top-right',
                        hideAfter: 3000,
                        showHideTransition: 'slide',
                        loader: false,
                    })
                }

            }
        });
         
     
        $('select[name=projTypID]').val($("#HidProjecTypID").val());
        $('select[name=projTypID]').selectpicker('refresh');       

        $('select[name=ProcompTypID]').val($("#HidProcompTypID").val());
        $('select[name=ProcompTypID]').selectpicker('refresh');

        $('select[name=DesigID]').val($("#HidDesigID").val());
        $('select[name=DesigID]').selectpicker('refresh');
         

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

        $("#DesigID").change(function () {

            GetGrade($(this).val());

        })
        


    })
     
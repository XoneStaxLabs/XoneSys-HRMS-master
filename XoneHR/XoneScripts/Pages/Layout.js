function SessionExpiry() {
   
    $.ajax({

        type: "get",
        url: "/Login/LoginSession"


    }).done(function () {

    })


}

function GetMenuLists() {
  
    $.ajax({

        type: "get",
        url: "/Login/MenuLists"
    }).done(function (data) {

        $("#MenuLists").html(data);
    });

}

function PassportExpiry() {
    $.ajax({

        type: "get",
        url: "/DashBoard/GetPassportExpiryList"

    }).done(function (data) {

        $("#PassportNotification").html(data);

    });
}

function PLRDExpiry() {
    $.ajax({

        type: "get",
        url: "/DashBoard/GetPLRDExpiryList"

    }).done(function (data) {

        $("#PLRDNotification").html(data);

    });
}

function LeaveNotify() {
    
    $.ajax({
        type: "get",
        url: "/DashBoard/LeaveNotify"

    }).done(function (data) {
        $("#LeaveNotification").html(data);
    });
}


$(document).ready(function () {
   
    $('#reservation').daterangepicker();

    GetMenuLists();
    PassportExpiry();
    PLRDExpiry();
    LeaveNotify();
       
    setInterval(function () {
        SessionExpiry()
    }, 90000); //400000
    

    $("#SignOut").click(function () {
       
        $.ajax({

            type: "get",
            url: "/Login/LogoutSession"

        }).done(function (data) {

            window.location.href = "/Login/Index";

        });

    })
       
    //Initialize Select2 Elements
    //Initialize Select2 Elements
    $(".select2").select2();

    $('.daterangepicker-single').daterangepicker({
        "singleDatePicker": true,
        format: $("#HidDateFormat").val()
    });

    $('.daterangepicker2').daterangepicker();

    $('.daterangepicker2').daterangepicker({
        format: $("#HidDateFormat").val()
    });

    $('#datetimepicker10').datetimepicker({
        viewMode: 'years',
        format: $("#HidDateFormat").val()
    });

    $('.YearCal').datetimepicker({
      //  viewMode: 'years',
        format: $("#HidDateFormat").val()
    });

    $('.leavepicker,.leavepickertwo').datetimepicker({
        format: $("#HidDateFormat").val()
    });

    $('#datetimepicker9').datetimepicker({
        viewMode: 'years',
        format: $("#HidDateFormat").val()
    });
    //Timepicker
    $(".timepicker").timepicker({
        showInputs: false
    });

   

    //iCheck for checkbox and radio inputs
    $('input[type="checkbox"].minimal, input[type="radio"].minimal').iCheck({
        checkboxClass: 'icheckbox_minimal-blue',
        radioClass: 'iradio_minimal-blue'
    });

    //Red color scheme for iCheck
    $('input[type="checkbox"].minimal-red, input[type="radio"].minimal-red').iCheck({
        checkboxClass: 'icheckbox_minimal-red',
        radioClass: 'iradio_minimal-red'
    });

    //Flat red color scheme for iCheck
    $('input[type="checkbox"].flat-red, input[type="radio"].flat-red').iCheck({
        checkboxClass: 'icheckbox_flat-green',
        radioClass: 'iradio_flat-green'
    });


    //$("#MenuLists").on("click", ".MenuClick", function (e) {

    //    e.preventDefault(); // prevent default link button redirect behaviour
    //    var url = $(this).attr("name");
    //    alert(url);

    //    $('#LoadData').load(url);

    //});




});
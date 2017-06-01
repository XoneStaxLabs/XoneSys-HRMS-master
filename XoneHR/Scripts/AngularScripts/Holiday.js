var app = angular.module('HoliApp', ['datatables']);

app.controller('HoliCntrl', ['$scope', '$http', 'DTOptionsBuilder', 'DTColumnBuilder', '$compile', function ($scope, $http, DTOptionsBuilder, DTColumnBuilder, $compile) {

    var index = 1;
    countIndex = function () {
        return index++;
    }

    $scope.dtInstance = {};

    function callback(json) {
        console.log(json);
    }

    $scope.reloadData = function () {
        var resetPaging = false;
        $scope.dtInstance.reloadData(callback, resetPaging);
    };

    $scope.dtColumns = [
       DTColumnBuilder.newColumn(countIndex, "Sl No"),
       DTColumnBuilder.newColumn("Holiday", "Holiday").withOption('name', 'Name'),
       DTColumnBuilder.newColumn("HolidayDate").withTitle('Date').renderWith(function (data, type) {
          //return $filter('date')(data, 'dd/MM/yyyy');
          return (moment(data).format("DD/MM/YYYY"));
       }),
       DTColumnBuilder.newColumn("Description", "Description").withOption('name', 'Name'),
       DTColumnBuilder.newColumn(null).withTitle('Action').notSortable()
       .renderWith(function (data, type, full, meta) {
           return '<button type="button" class="btn btn-outline btn-circle btn-sm purple EditClick" ng-click="EditClick(' + data.HolidayID + ')">' +
               '   <i class="fa fa-edit"></i>' +
               '</button>&nbsp;' +

               '<button  type="button" class="btn btn-outline btn-circle dark btn-sm black BtnClick" ng-click="DeleteClick(' + data.HolidayID + ')">' +
               '   <i class="fa fa-trash-o"></i>' +
               '</button> ';
       }),
    ];
    $scope.dtOptions = DTOptionsBuilder.newOptions().withOption('ajax', {
        dataSrc: "data",
        url: "/MasterLists/HolidayMaster/ListHolidays",
        type: "POST"
    })
    .withOption('processing', true)
    .withOption('serverSide', true)
    .withPaginationType('full_numbers')
    .withDisplayLength(10)
    .withOption('aaSorting', [0, 'Desc'])
    .withOption('createdRow', function (row, data, dataIndex) {
        $compile(angular.element(row).contents())($scope);
    })
    .withOption('fnRowCallback', function (nRow, aData, iDisplayIndex) {
        $("td:first", nRow).html(iDisplayIndex + 1);
        return nRow;
    })

    $scope.AddNewBtn = function () {
        $("#Addnew").modal('show');
    }
   
    $scope.AddNewSave = function () {
                
        $("#HoliForm").validate();
        if($("#HoliForm").valid())
        {
            $scope.HolidayDate = $("#HolidayDate").val(); 
            $http({
                method: "POST",
                url: "/MasterLists/HolidayMaster/AddHolidays",
                data: {
                    Holiday: $scope.Holiday,
                    HolidayDate: $scope.HolidayDate,
                    Description: $scope.Description
                }
            }).success(function (response) {
                $.toast({
                    text: response.Message,
                    position: 'top-right',
                    hideAfter: 2000,
                    showHideTransition: 'slide',
                    loader: false,
                    icon: response.Icon
                })
                if (response.Result > 0)
                { window.location.href = "/MasterLists/HolidayMaster/Index"; }
            });
        }
    }

    $scope.DeleteClick = function (ID) {

        $scope.HolidayID_Dlt = ID;
        $http({
            method: "GET",
            url: "/MasterLists/HolidayMaster/GetHolidatyText",
            params: { HolidayID: $scope.HolidayID_Dlt }
        }).success(function (data) {
            $scope.DeleteHoliday = data;
        });
        $("#Delete").modal('show');
    }

    $scope.DeleteHoli = function () {
        $http({
            method: "POST",
            url: "/MasterLists/HolidayMaster/DeleteHoliday",
            params: {
                HolidayID: $scope.HolidayID_Dlt
            }
        }).success(function (response) {
            $.toast({
                text: response.Message,
                position: 'top-right',
                hideAfter: 2000,
                showHideTransition: 'slide',
                loader: false,
                icon: response.Icon
            })
            if (response.Result > 0)
            { window.location.href = "/MasterLists/HolidayMaster/Index"; }
        });
    }

    $('.daterangepicker-single').daterangepicker({
        "singleDatePicker": true
        //format: 'dd/mm/yyyy'
    });
}]);
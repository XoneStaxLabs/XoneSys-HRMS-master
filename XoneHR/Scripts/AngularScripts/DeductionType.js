var app = angular.module("MyApp", ['datatables']);
app.controller('MyCntrl', ['$scope', '$http', 'DTOptionsBuilder', 'DTColumnBuilder', '$compile', function ($scope, $http, DTOptionsBuilder, DTColumnBuilder, $compile) {

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
        DTColumnBuilder.newColumn("DeductionType", "Deduction Type").withOption('name', 'Name'),
        DTColumnBuilder.newColumn(null).withTitle('Action').notSortable()
        .renderWith(function (data, type, full, meta) {
            return '<button type="button" class="btn btn-outline btn-circle btn-sm purple EditClick" ng-click="EditClick(' + data.DeductTypeID + ')">' +
                '   <i class="fa fa-edit"></i>' +
                '</button>&nbsp;' +

                '<button  type="button" class="btn btn-outline btn-circle dark btn-sm black BtnClick" ng-click="DeleteClick(' + data.DeductTypeID + ')">' +
                '   <i class="fa fa-trash-o"></i>' +
                '</button> ';
        }),
    ];
    $scope.dtOptions = DTOptionsBuilder.newOptions().withOption('ajax', {
        dataSrc: "data",
        url: "/MasterLists/DeductionTypeMaster/ListDeductionType",
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

       $("#DeductForm").validate();
       if ($("#DeductForm").valid())
       {
           $http({
               method: "POST",
               url: "/MasterLists/DeductionTypeMaster/AddNewDeductionType",
               data: {
                   DeductionType: $scope.DeductionType
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
               if (response.Result > 0) {
                   window.location.href = "/MasterLists/DeductionTypeMaster/Index";
               }
           })
       }
       else
       {
           $("#Addnew").modal('show');
       }
       
    }

    $scope.EditClick = function (id) {

        $scope.DeductTypeID = id;
        $http({
            method: "GET",
            url: "/MasterLists/DeductionTypeMaster/GetDetailForEdit",
            params: {
                DeductTypeID: $scope.DeductTypeID
            }
        }).success(function (response) {
            $scope.DeductionType_edit = response.DeductionType;
        })

        $("#EditDeductType").modal('show');
    }

    $scope.EditDeductTypeBtn = function () {

        $("#DeductForm").validate();
        if($("#DeductForm").valid())
        {
            $http({
                method: "POST",
                url: "/MasterLists/DeductionTypeMaster/EditDeductionType",
                data: {
                    DeductTypeID: $scope.DeductTypeID,
                    DeductionType: $scope.DeductionType_edit
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
                if (response.Result > 0) {
                    window.location.href = "/MasterLists/DeductionTypeMaster/Index";
                }
            })
        }
        else
        {
            $("#EditDeductType").modal('show');
        }
    }

    $scope.DeleteClick = function (id) {

        $scope.DeductTypeID_Dlt = id;
        $http({
            method: "GET",
            url: "/MasterLists/DeductionTypeMaster/GetDeductionTypeText",
            params: {
                DeductTypeID: $scope.DeductTypeID_Dlt
            }
        }).success(function (data) {
            $scope.DeleteDeductType = data;
        });
        $("#Delete").modal('show');
    }

    $scope.DeleteDeduction = function () {

        $http({
            method: "GET",
            url: "/MasterLists/DeductionTypeMaster/DeleteDeductionType",
            params: {
                DeductTypeID: $scope.DeductTypeID_Dlt
            }
        }).success(function (data) {
            $.toast({
                text: data.Message,
                position: 'top-right',
                hideAfter: 2000,
                showHideTransition: 'slide',
                loader: false,
                icon: data.Icon
            })
            if (data.Result > 0)
            { window.location.href = "/MasterLists/DeductionTypeMaster/Index"; }
        });
    }

}]);
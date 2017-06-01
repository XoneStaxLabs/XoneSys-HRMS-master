var app = angular.module('DepartmentApp', ['datatables']);
app.controller('DepartmentCntrl', ['$scope', '$http', 'DTOptionsBuilder', 'DTColumnBuilder', '$compile', function ($scope, $http, DTOptionsBuilder, DTColumnBuilder, $compile) {

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
        DTColumnBuilder.newColumn("DeptName", "Name").withOption('name', 'Name'),
        DTColumnBuilder.newColumn("DeptCode", "Code").withOption('name', 'Name'),
        DTColumnBuilder.newColumn("UserType", "User Type").withOption('name', 'Name'),
        DTColumnBuilder.newColumn(null).withTitle('Action').notSortable()
        .renderWith(function (data, type, full, meta) {
            return '<button type="button" class="btn btn-outline btn-circle btn-sm purple EditClick" ng-click="EditClick(' + data.DeptID + ')">' +
                '   <i class="fa fa-edit"></i>' +
                '</button>&nbsp;' +

                '<button  type="button" class="btn btn-outline btn-circle dark btn-sm black BtnClick" ng-click="DeleteClick(' + data.DeptID + ')">' +
                '   <i class="fa fa-trash-o"></i>' +
                '</button> ';
        }),
    ];

    $scope.dtOptions = DTOptionsBuilder.newOptions().withOption('ajax', {
        dataSrc: "data",
        url: "/MasterLists/DepartmentMaster/DepartmentList",
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

    $http({
        method: "GET",
        url: "/MasterLists/DepartmentMaster/GetUserType"
    }).success(function (data) {
        $scope.UserTypeList = data;
    });

    $scope.AddNewSave=function()
    {
        $("#DepartmentForm").validate();
        if ($("#DepartmentForm").valid())
        {
            $http({
                method: "POST",
                url: "/MasterLists/DepartmentMaster/CreateDepartment",
                data: {
                    DeptName: $scope.DeptName,
                    DeptCode: $scope.DeptCode,
                    UserTypeId: $scope.UserTypeId
                }
            }).success(function (response) {

            });
        }
    }

    $scope.EditClick=function(id)
    {
        $scope.DeptID=id;
        $http({
            method: "GET",
            url: "/MasterLists/DepartmentMaster/GetDeptEditDetails",
            params: {
                DeptID: $scope.DeptID
            }
        }).success(function (response) {
            $scope.UserTypeId_edit = response.UserTypeId;
            $scope.DeptName_edit = response.DeptName;
            $scope.DeptCode_edit = response.DeptCode;
        });
        $("#EditDepartment").modal('show');
    }

    $scope.EditDeptBtn = function () {

        $("#DepartmentFormEdit").validate();
        if ($("#DepartmentFormEdit").valid())
        {
            $http({
                method: "POST",
                url: "/MasterLists/DepartmentMaster/EditDeparmentDetails",
                data: {
                    UserTypeId: $scope.UserTypeId_edit,
                    DeptName: $scope.DeptName_edit,
                    DeptCode: $scope.DeptCode_edit,
                    DeptID: $scope.DeptID
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
                { window.location.href = "/MasterLists/DepartmentMaster/Index"; }
            });
        }
        else
        {
            $("#EditDepartment").modal('show');
        }
        
    }

    $scope.DeleteClick = function (id) {

        $scope.DeptID_Dlt = id;
        //Not checked department any where used or not
        $http({
            method: "GET",
            url: "/MasterLists/DepartmentMaster/GetDepartmentname",
            params: { DeptID: $scope.DeptID_Dlt }
        }).success(function (data) {
            $scope.DeleteDept = data;
        });
        $("#Delete").modal('show');
    }

    $scope.DeleteDeptBtn = function () {
        $http({
            method: "POST",
            url: "/MasterLists/DepartmentMaster/DeleteDepartment",
            params: { DeptID: $scope.DeptID_Dlt }
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
            { window.location.href = "/MasterLists/DepartmentMaster/Index"; }
        });
    }

}]);
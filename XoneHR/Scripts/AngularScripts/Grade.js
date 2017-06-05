var app = angular.module('GradeApp', ['datatables']);
app.controller('GradeCntrl', ['$scope', '$http', 'DTOptionsBuilder', 'DTColumnBuilder', '$compile', function ($scope, $http, DTOptionsBuilder, DTColumnBuilder, $compile) {
    
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
        DTColumnBuilder.newColumn("Gradename", "Grade").withOption('name', 'Name'),
        DTColumnBuilder.newColumn("GradeCode", "Code").withOption('name', 'Name'),
        DTColumnBuilder.newColumn("DeptName", "Department").withOption('name', 'Name'),
        DTColumnBuilder.newColumn("DesignationName", "Designation").withOption('name', 'Name'),        
        DTColumnBuilder.newColumn(null).withTitle('Action').notSortable()
        .renderWith(function (data, type, full, meta) {
            return '<button type="button" class="btn btn-outline btn-circle btn-sm purple" ng-click="EditClick(' + data.GradeID + ',' + data.GradeDesignationId + ')">' +
                '   <i class="fa fa-edit"></i>' +
                '</button>&nbsp;' +

                '<button  type="button" class="btn btn-outline btn-circle dark btn-sm black" ng-click="DeleteClick(' + data.GradeID + ',' + data.GradeDesignationId + ')">' +
                '   <i class="fa fa-trash-o"></i>' +
                '</button> ';
        }),
    ];

    $scope.dtOptions = DTOptionsBuilder.newOptions().withOption('ajax', {
        dataSrc: "data",
        url: "/MasterLists/GradeMaster/ListGradeDetails",
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

    $http({
        method: "GET",
        url: "/MasterLists/GradeMaster/GetDepartment"
    }).success(function (response) {
        $scope.DeptList = response;
    });

    $scope.AddNewBtn = function () {
        $("#Addnew").modal('show');
    }

    $scope.DeptChange = function () {
        GetDesignation($scope.DeptID);
    }

    function GetDesignation(DeptID) {
        $http({
            method: "GET",
            url: "/MasterLists/GradeMaster/GetDesignation",
            params: { DeptID: DeptID }
        }).success(function (response) {
            $scope.DesigList = response;
        });
    }

    $scope.AddNewSave = function () {

        $("#GradeForm").validate();
        if ($("#GradeForm").valid())
        {
            $http({
                method: "POST",
                url: "/MasterLists/GradeMaster/AddNewGrade",
                data: {
                    DesignationID: $scope.DesignationID,
                    Gradename: $scope.Gradename,
                    GradeCode: $scope.GradeCode
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
                if (data.Result > 0) {
                    window.location.href = "/MasterLists/GradeMaster/Index";
                }
                else {
                    $("#Addnew").modal('show');
                }
            })
        }       
       
    }

    $scope.EditClick = function (id, GradeDesignationId) {
               
        $scope.GradeID = id;
        $scope.GradeDesignationId = GradeDesignationId;
        $http({
            method: "GET",
            url: "/MasterLists/GradeMaster/GetDetailsForEdit",
            params: {
                GradeID: $scope.GradeID,
                GradeDesignationId: $scope.GradeDesignationId
            }
        }).success(function (data) {
            GetDesignation(data.DeptID);
            $scope.DeptID_edit = data.DeptID;            
            $scope.Gradename_edit = data.Gradename;
            $scope.GradeCode_edit = data.GradeCode;            
            $scope.DesignationID_edit = data.DesignationID;
            $scope.isDisabled = true;
        })
        $("#EditGrade").modal('show');
    }

    $scope.EditGradeBtn = function () {
        $("#GradeFormEdit").validate();
        if($("#GradeFormEdit").valid())
        {
            $http({
                method: "POST",
                url: "/MasterLists/GradeMaster/EditGrade",
                data: {
                    GradeID: $scope.GradeID,
                    DesignationID: $scope.DesignationID_edit,
                    Gradename: $scope.Gradename_edit,
                    GradeCode: $scope.GradeCode_edit,
                    GradeDesignationId: $scope.GradeDesignationId
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
                if (data.Result > 0) {
                    window.location.href = "/MasterLists/GradeMaster/Index";
                }
                else {
                    $("#EditGrade").modal('show');
                }
            })
        }
    }

    $scope.DeleteClick=function(id, GradeDesignationId)
    {
        $scope.GradeID_Dlt = id;
        $scope.GradeDesignationId_Dlt = GradeDesignationId;
        $http({
            method: "GET",
            url: "/MasterLists/GradeMaster/GetGradeName",
            params: {
                GradeID: $scope.GradeID_Dlt
            }
        }).success(function (data) {
            $scope.DeleteGradeType = data;
            $("#Delete").modal('show');
        });
    }

    $scope.DeleteGradeBtn = function () {
        $http({
            method: "POST",
            url: "/MasterLists/GradeMaster/DeleteGrade",
            params: {
                GradeID: $scope.GradeID_Dlt,
                GradeDesignationId: $scope.GradeDesignationId_Dlt
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
            { window.location.href = "/MasterLists/GradeMaster/Index"; }
        });
    }

}]);
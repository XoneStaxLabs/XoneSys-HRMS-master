var app = angular.module('MyApp', ['datatables']);
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
        DTColumnBuilder.newColumn("CitizenName", "Citizen").withOption('name', 'Name'),
        DTColumnBuilder.newColumn("DesignationName", "Designation").withOption('name', 'Name'),
        DTColumnBuilder.newColumn("DocSubtypeName", "Document SubType").withOption('name', 'Name'),
        //DTColumnBuilder.newColumn("DeptName", "Department").withOption('name', 'Name'),
        DTColumnBuilder.newColumn(null).withTitle('Action').notSortable()
        .renderWith(function (data, type, full, meta) {
            return '<button type="button" class="btn btn-outline btn-circle dark btn-sm black BtnClick" ng-click="DeleteClick(' + data.ValidDocTypID + ')">' +
                '   <i class="fa fa-trash-o"></i>' +
                '</button>' ;
        }),
    ];

    $scope.dtOptions = DTOptionsBuilder.newOptions().withOption('ajax', {
        dataSrc: "data",
        url: "/MasterLists/DesigDocTypeMaster/ListDetails",
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
        url: "/MasterLists/DesigDocTypeMaster/GetCitizen"
    }).success(function (response) {
        $scope.CitizenList = response;
    });

    $http({
        method: "GET",
        url: "/MasterLists/DesigDocTypeMaster/GetDesignmation"
    }).success(function (response) {
        $scope.DesigList = response;
    });

    $http({
        method: "GET",
        url: "/MasterLists/DesigDocTypeMaster/GetDocType"
    }).success(function (response) {
        $scope.DocTypeList = response;
    });

    $scope.DocTypeChange = function () {
        $http({
            method: "GET",
            url: "/MasterLists/DesigDocTypeMaster/GetDocSubType",
            params: { DocTypeID: $scope.DocTypeID }
        }).success(function (response) {
            $scope.DocSubTypeList = response;
        });
    }

    
    $scope.NewAddBtn = function () {
        $("#Addnew").modal('show');
    }

    $scope.AddNewSave = function () {
        $("#DocForm").validate();
        if($("#DocForm").valid())
        {
            $http({
                method: "POST",
                url: "/MasterLists/DesigDocTypeMaster/AddNewDocTypes",
                params: {
                    CitizenID: $scope.CitizenID,
                    DesignationID: $scope.DesignationID,
                    DocTypeID: $scope.DocTypeID,
                    DocSubtypeID: $scope.DocSubtypeID
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
                    window.location.href = "/MasterLists/DesigDocTypeMaster/Index";
                }
            })
        }
        else
        {
            $("#Addnew").modal('show');
        }
    }

    $scope.DeleteClick = function (id) {
        $scope.ValidDocType_Dlt = id;        
        $("#Delete").modal('show');
    }

    $scope.DeleteDocType = function () {
        $http({
            method: "POST",
            url: "/MasterLists/DesigDocTypeMaster/DeleteDocType",
            params: {
                ValidDocTypID: $scope.ValidDocType_Dlt
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
            { window.location.href = "/MasterLists/DesigDocTypeMaster/Index"; }
        });
    }

}]);
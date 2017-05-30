var app = angular.module('DocumentApp', ['datatables']);
app.controller('DocumentCntrl', ['$scope', '$http', 'DTOptionsBuilder', 'DTColumnBuilder', '$compile', function ($scope, $http, DTOptionsBuilder, DTColumnBuilder, $compile) {

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
        DTColumnBuilder.newColumn("DocSubtypeName", "Name").withOption('name', 'Name'),
        DTColumnBuilder.newColumn("DocTypeName", "Document Type").withOption('name', 'Name'),
        DTColumnBuilder.newColumn(null).withTitle('Action').notSortable()
        .renderWith(function (data, type, full, meta) {
            return '<button type="button" class="btn btn-outline btn-circle btn-sm purple EditClick" ng-click="EditClick(' + data.DocSubtypeID + ')">' +
                '   <i class="fa fa-edit"></i>' +
                '</button>&nbsp;' +

                '<button  type="button" class="btn btn-outline btn-circle dark btn-sm black BtnClick" ng-click="DeleteClick(' + data.DocSubtypeID + ')">' +
                '   <i class="fa fa-trash-o"></i>' +
                '</button> ';
        }),
    ];

    $scope.dtOptions = DTOptionsBuilder.newOptions().withOption('ajax', {
        dataSrc: "data",
        url: "/MasterLists/DocumentSubTypeMaster/ListDocumentDetails",
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
        url: "/MasterLists/DocumentSubTypeMaster/GetDocTypes"
    }).success(function (response) {
        $scope.DoctypeList = response;
    });

    $scope.AddNewSave = function () {

        $("#DocumentForm").validate();
        if($("#DocumentForm").valid())
        {
            $http({
                method: "POST",
                url: "/MasterLists/DocumentSubTypeMaster/CreateDocumentSubType",
                data: {
                    DocTypeID: $scope.DocTypeID,
                    DocSubtypeName: $scope.DocSubtypeName
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
                    window.location.href = "/MasterLists/DocumentSubTypeMaster/Index";
                }
            })
        }
        else {
            $("#Addnew").modal('show');
        }
    }

    $scope.EditClick = function (id) {

        $scope.DocSubtypeID = id;
        $http({
            method: "GET",
            url: "/MasterLists/DocumentSubTypeMaster/GetDetailsForEdit",
            params: { DocSubtypeID: $scope.DocSubtypeID }
        }).success(function (response) {
            $scope.DocTypeID_edit = response.DocTypeID;
            $scope.DocSubtypeName_edit = response.DocSubtypeName;
        });

        $("#EditDocuments").modal('show');
    }

    $scope.EditDocBtn = function () {
        $("#DocumentFormEdit").validate();
        if($("#DocumentFormEdit").valid())
        {
            $http({
                method: "POST",
                url: "/MasterLists/DocumentSubTypeMaster/EditDocumentSubType",
                data: {
                    DocTypeID: $scope.DocTypeID_edit,
                    DocSubtypeID:$scope.DocSubtypeID,
                    DocSubtypeName: $scope.DocSubtypeName_edit
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
                    window.location.href = "/MasterLists/DocumentSubTypeMaster/Index";
                }
            })
        }
    }

    $scope.DeleteClick = function (id) {
        $scope.DocSubtypeID_Dlt = id;
        $http({
            method: "GET",
            url: "/MasterLists/DocumentSubTypeMaster/CheckDeletableStatus",
            params: { DocSubtypeID: $scope.DocSubtypeID_Dlt }
        }).success(function (response) {
            if (response) {
                $http({
                    method: "GET",
                    url: "/MasterLists/DocumentSubTypeMaster/GetDocumentName",
                    params: {
                        DocSubtypeID: $scope.DocSubtypeID_Dlt
                    }
                }).success(function (data) {

                    $scope.DeleteDocType = data;
                });

                $("#Delete").modal('show');
            }
            else {
                $.toast({
                    text: "This DocumentSubType Used For Cadidate Registration",
                    position: 'top-right',
                    showHideTransition: 'slide',
                    loader: false,
                    icon: "error"
                })
            }
        });

    }

    $scope.DeleteDocBtn = function () {
        $http({
            method: "POST",
            url: "/MasterLists/DocumentSubTypeMaster/DeleteDocType",
            params: {
                DocSubtypeID: $scope.DocSubtypeID_Dlt
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
            { window.location.href = "/MasterLists/DocumentSubTypeMaster/Index"; }
        });
    }

}]);
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
        DTColumnBuilder.newColumn("DocTypeName", "Name").withOption('name', 'Name'),
        DTColumnBuilder.newColumn(null).withTitle('Action').notSortable()
        .renderWith(function (data, type, full, meta) {
            return '<button type="button" class="btn btn-outline btn-circle btn-sm purple EditClick" ng-click="EditClick(' + data.DocTypeID + ')">' +
                '   <i class="fa fa-edit"></i>' +
                '</button>&nbsp;' +

                '<button  type="button" class="btn btn-outline btn-circle dark btn-sm black BtnClick" ng-click="DeleteClick(' + data.DocTypeID + ')">' +
                '   <i class="fa fa-trash-o"></i>' +
                '</button> ';
        }),
    ];

    $scope.dtOptions = DTOptionsBuilder.newOptions().withOption('ajax', {
        dataSrc: "data",
        url: "/MasterLists/DocumentTypeMaster/ListDocumentDetails",
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

        $("#DocumentForm").validate();
        if ($("#DocumentForm").valid()) {

            $http({
                method: "POST",
                url: "/MasterLists/DocumentTypeMaster/CreateDocumentType",
                data: {
                    DocTypeName: $scope.DocTypeName
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
                { window.location.href = "/MasterLists/DocumentTypeMaster/Index"; }
            });
        }
        else { $("#Addnew").modal('show'); }
    }

    $scope.EditClick = function (id) {
        $scope.DocTypeID = id;
        $http({
            method: "GET",
            url: "/MasterLists/DocumentTypeMaster/GetDocEditDetails",
            params: {
                DocTypeID: $scope.DocTypeID
            }
        }).success(function (response) {
            $scope.DocTypeName_edit = response.DocTypeName;
        });
        $("#EditDocuments").modal('show');
    }

    $scope.EditDocBtn = function () {

        $("#DocumentFormEdit").validate();
        if ($("#DocumentFormEdit").valid())
        {
            $http({
                method: "POST",
                url: "/MasterLists/DocumentTypeMaster/EditDocDetails",
                data: {
                    DocTypeID: $scope.DocTypeID,
                    DocTypeName: $scope.DocTypeName_edit
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
                    window.location.href = "/MasterLists/DocumentTypeMaster/Index";
                }
            });
        }
        else
        {
            $("#EditDocuments").modal('show');
        }
        
    }

    $scope.DeleteClick = function (id) {
        $scope.DocTypeID_Dlt = id;
        //$http({
        //    method: "GET",
        //    url: "/MasterLists/DocumentTypeMaster/CheckDeletableStatus",
        //    params: { DocTypeID: $scope.DocTypeID_Dlt }
        //}).success(function (response) {
        //    if (response) {
                
        //    }
        //    else {
        //        $.toast({
        //            text: "This DocumentType Used For Cadidate Registration",
        //            position: 'top-right',
        //            showHideTransition: 'slide',
        //            loader: false,
        //            icon: "error"
        //        })
        //    }
        //});

        $http({
            method: "GET",
            url: "/MasterLists/DocumentTypeMaster/GetDocumentName",
            params: {
                DocTypeID: $scope.DocTypeID_Dlt
            }
        }).success(function (data) {

            $scope.DeleteDocType = data;
            $("#Delete").modal('show');
        });

        $("#Delete").modal('show');
    }

    $scope.DeleteDocBtn = function () {
        $http({
            method: "POST",
            url: "/MasterLists/DocumentTypeMaster/DeleteDocType",
            params: {
                DocTypeID: $scope.DocTypeID_Dlt
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
            { window.location.href = "/MasterLists/DocumentTypeMaster/Index"; }
        });
    }


}]);
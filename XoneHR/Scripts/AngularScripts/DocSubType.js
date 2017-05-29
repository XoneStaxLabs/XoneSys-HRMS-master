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
            return '<button type="button" class="btn btn-outline btn-circle btn-sm purple EditClick" ng-click="EditClick(' + data.DocTypeID + ', ' + data.DocSubtypeID + ')">' +
                '   <i class="fa fa-edit"></i>' +
                '</button>&nbsp;' +

                '<button  type="button" class="btn btn-outline btn-circle dark btn-sm black BtnClick" ng-click="DeleteClick(' + data.DocTypeID + ',' + data.DocSubtypeID + ')">' +
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
    })

}]);
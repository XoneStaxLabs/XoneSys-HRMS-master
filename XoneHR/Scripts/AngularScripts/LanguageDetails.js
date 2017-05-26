var app = angular.module('LanguageApp', ['datatables']);
app.controller('LngCntrl', ['$scope', '$http', 'DTOptionsBuilder', 'DTColumnBuilder', '$compile', function ($scope, $http, DTOptionsBuilder, DTColumnBuilder, $compile) {

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
        DTColumnBuilder.newColumn(countIndex, 'Sl No'),
        DTColumnBuilder.newColumn("LanguageName", "Language").withOption('name', 'Name'),
        DTColumnBuilder.newColumn(null).withTitle('Action').notSortable()
        .renderWith(function (data, type, full, meta) {
            return '<button type="button" class="btn btn-outline btn-circle btn-sm purple EditClick" ng-click="EditClick(' + data.LanguageID + ')">' +
                ' <i class="fa fa-edit"></i>' +
                '</button>&nbsp;' +
                '<button  type="button" class="btn btn-outline btn-circle dark btn-sm black BtnClick" ng-click="DeleteClick(' + data.LanguageID + ')">' +
                '<i class="fa fa-trash-o"></i>' +
                '</button>';
        })
    ];
    $scope.dtOptions = DTOptionsBuilder.newOptions().withOption('ajax', {
        dataSrc: "data",
        url: "/MasterLists/LanguageMaster/ListLanguageDetails",
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

    $scope.NewAddBtn = function () {
        $("#Addnew").modal('show');
    }
    $scope.AddNewSave = function () {
        $("#LngForm").validate();
        if($("#LngForm").valid())
        {
            $http({
                method: "POST",
                url: "/MasterLists/LanguageMaster/AddNewLanguage",
                data: {
                    LanguageName: $scope.LanguageName
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
                    window.location.href = "/MasterLists/LanguageMaster/Index";
                }
            })
        }
        else {
            $("#Addnew").modal('show');
        }
    }
    $scope.EditClick = function (id) {

        $scope.LanguageID = id;
        $http({
            method: "GET",
            url: "/MasterLists/LanguageMaster/GetLangDetails",
            params: {
                LanguageID: $scope.LanguageID
            }
        }).success(function (response) {
            $scope.LanguageName_edit = response.LanguageName;            
        });
        $("#EditLng").modal('show');
    }
    $scope.EditLngBtn = function () {
        $("#LngFormEdit").validate();
        if($("#LngFormEdit").valid())
        {
            $http({
                method: "POST",
                url: "/MasterLists/LanguageMaster/EditLngDetails",
                data: {
                    LanguageID:$scope.LanguageID,
                    LanguageName: $scope.LanguageName_edit
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
                    window.location.href = "/MasterLists/LanguageMaster/Index";
                }
            })
        }
        else {
            $("#EditLng").modal('show');
        }
    }
    $scope.DeleteClick = function (id) {

        $scope.LanguageID_Dlt = id;
        $http({
            method: "GET",
            url: "/MasterLists/LanguageMaster/CheckDeletableStatus",
            params: { LanguageID: $scope.LanguageID_Dlt }
        }).success(function (response) {
            
            if(response)
            {
                $http({
                    method: "GET",
                    url: "/MasterLists/LanguageMaster/GetLanguageName",
                    params: {
                        LanguageID: $scope.LanguageID_Dlt
                    }
                }).success(function (response) {
                    $scope.DeleteLngName = response;
                });
                $("#Delete").modal('show');
            }
            else {
                $.toast({
                    text: "This Language Used For Cadidate Registration",
                    position: 'top-right',
                    showHideTransition: 'slide',
                    loader: false,
                    icon: "error"
                })
            }
        })
    }
    $scope.DeleteLng = function () {

        $http({
            method: "POST",
            url: "/MasterLists/LanguageMaster/DeleteLanguage",
            params: {
                LanguageID: $scope.LanguageID_Dlt
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
            { window.location.href = "/MasterLists/LanguageMaster/Index"; }
        })
    }

}]);
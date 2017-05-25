var app = angular.module('RaceApp', ['datatables']);
app.controller('RaceCntrl', ['$scope', '$http', 'DTOptionsBuilder', 'DTColumnBuilder', '$compile', function ($scope, $http, DTOptionsBuilder, DTColumnBuilder, $compile) {

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
        DTColumnBuilder.newColumn("RaceName", "Race Name").withOption('name', 'Name'),
        DTColumnBuilder.newColumn(null).withTitle('Action').notSortable()
        .renderWith(function (data, type, full, meta) {
            return '<button type="button" class="btn btn-outline btn-circle btn-sm purple EditClick" ng-click="EditClick(' + data.RaceID + ')">' +
                '   <i class="fa fa-edit"></i>' +
                '</button>&nbsp;' +

                '<button  type="button" class="btn btn-outline btn-circle dark btn-sm black BtnClick" ng-click="DeleteClick(' + data.RaceID + ')">' +
                '   <i class="fa fa-trash-o"></i>' +
                '</button> ';
        }),
    ];
    $scope.dtOptions = DTOptionsBuilder.newOptions().withOption('ajax', {
        dataSrc: "data",
        url: "/MasterLists/RaceMaster/ListRaceDetails",
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
        
        $("#RaceForm").validate({
            rules: {
                RaceName: {
                    required:true
                }
            }
        })
        if($("#RaceForm").valid())
        {
            $http(({
                method: "POST",
                url: "/MasterLists/RaceMaster/AddNewRace",
                data: {
                    RaceName: $scope.RaceName
                }
            })).success(function (reponse) {
                $.toast({
                    text: response.Message,
                    position: 'top-right',
                    hideAfter: 2000,
                    showHideTransition: 'slide',
                    loader: false,
                    icon: response.Icon
                })
                if (response.Result > 0)
                {
                    window.location.href = "/MasterLists/RaceMaster/Index";
                }
            });
        }
        else {
            $("#Addnew").modal('show');
        }

    }

    $scope.EditClick = function (id) {

        $scope.RaceID = id;
        $http({
            method: "GET",
            url: "/MasterLists/RaceMaster/GetDetailsForEdit",
            params: { RaceID: $scope.RaceID }
        }).success(function (response) {
            $scope.RaceName_edit = response.RaceName;
        });
        $("#EditRace").modal('show');
    }

    $scope.EditRaceBtn = function () {

        $("#RaceFormEdit").validate({
            rules: {
                RaceName_edit: {
                    required:true
                }
            }
        });
        if ($("#RaceFormEdit").valid()) {

            $http({
                method: "POST",
                url: "/MasterLists/RaceMaster/EditRaceDetails",
                data: {
                    RaceID: $scope.RaceID,
                    RaceName: $scope.RaceName_edit
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
                    window.location.href = "/MasterLists/RaceMaster/Index";
                }
            });
        }
        else {
            $("#EditRace").modal('show');
        }
    }

    $scope.DeleteClick = function (id) {
        $scope.RaceId_Dlt = id;

        $http({
            method: "GET",
            url: "/MasterLists/RaceMaster/CheckRaceDeletableStatus",
            params: { RaceId: $scope.RaceId_Dlt }
        }).success(function (response) {
            if (response) {
                $http({
                    method: "GET",
                    url: "/MasterLists/RaceMaster/GetRaceName",
                    params: {
                        RaceId: $scope.RaceId_Dlt
                    }
                }).success(function (response) {

                    $scope.DeleteRaceName = response;
                    $("#Delete").modal('show');
                });
            }
            else {
                $.toast({
                    text: "This Race Used For Cadidate Registration",
                    position: 'top-right',
                    showHideTransition: 'slide',
                    loader: false,
                    icon: "error"
                })
            }
        });
    }

    $scope.DeleteRace = function () {
        $http({
            method: "POST",
            url: "/MasterLists/RaceMaster/DeleteRace",
            params: {
                RaceId: $scope.RaceId_Dlt
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
            { window.location.href = "/MasterLists/RaceMaster/Index"; }
        });
    }

}]);
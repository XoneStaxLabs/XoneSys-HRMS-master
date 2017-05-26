var app = angular.module('SkillsetApp', ['datatables']);
app.controller('SkillsetCntrl', ['$scope', '$http', 'DTOptionsBuilder', 'DTColumnBuilder', '$compile', function ($scope, $http, DTOptionsBuilder, DTColumnBuilder, $compile) {

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
      DTColumnBuilder.newColumn("SkillName", "Name").withOption('name', 'Name'),
      DTColumnBuilder.newColumn("Description", "Description").withOption('name', 'Name'),
      DTColumnBuilder.newColumn(null).withTitle('Action').notSortable()
      .renderWith(function (data, type, full, meta) {
          return '<button type="button" class="btn btn-outline btn-circle btn-sm purple EditClick" ng-click="EditClick(' + data.SkillID + ')">' +
              ' <i class="fa fa-edit"></i>' +
              '</button>&nbsp;' +
              '<button  type="button" class="btn btn-outline btn-circle dark btn-sm black BtnClick" ng-click="DeleteClick(' + data.SkillID + ')">' +
              '<i class="fa fa-trash-o"></i>' +
              '</button>';
      })
    ];
    $scope.dtOptions = DTOptionsBuilder.newOptions().withOption('ajax', {
        dataSrc: "data",
        url: "/MasterLists/SkillsetMaster/ListSkillsetDetails",
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
        $("#skillForm").validate();
        if ($("#skillForm").valid()) {
            $http({
                method: "POST",
                url: "/MasterLists/SkillsetMaster/AddNewSkills",
                data: {
                    SkillName: $scope.SkillName
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
                    window.location.href = "/MasterLists/SkillsetMaster/Index";
                }
            })
        }
        else {
            $("#Addnew").modal('show');
        }
    }

    $scope.EditClick = function (id) {

        $scope.SkillID = id;
        $http({
            method: "GET",
            url: "/MasterLists/SkillsetMaster/GetEditSkillDetails",
            params: {
                SkillID: $scope.SkillID
            }
        }).success(function (response) {
            $scope.SkillName_edit=response.SkillName;
            $scope.Description_edit = response.Description;
        });
        $("#Editskillsets").modal('show');
    }

    $scope.EditSkillBtn = function () {
        $("#SkillFormEdit").validate();
        if ($("#SkillFormEdit").valid()) {
            $http({
                method: "POST",
                url: "/MasterLists/SkillsetMaster/EditSkillsets",
                data: {
                    SkillID: $scope.SkillID,
                    SkillName: $scope.SkillName_edit,
                    Description: $scope.Description_edit
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
                    window.location.href = "/MasterLists/SkillsetMaster/Index";
                }
            })
        }
        else {
            $("#Editskillsets").modal('show');
        }
    }

}]);
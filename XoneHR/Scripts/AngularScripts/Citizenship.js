var app = angular.module('CitizenApp', ['datatables']);

app.controller('CitizenListCntrl', ['$scope', '$http', 'DTOptionsBuilder', 'DTColumnBuilder', '$compile', function ($scope, $http, DTOptionsBuilder, DTColumnBuilder, $compile) {
   
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
        DTColumnBuilder.newColumn("CitizenName", "Citizen Name").withOption('name', 'Name'),
        DTColumnBuilder.newColumn("CitizenCode", "Citizen Code").withOption('name', 'Name'),
        DTColumnBuilder.newColumn("CitizenDesc", "Citizen Desc").withOption('name', 'Name'),
        DTColumnBuilder.newColumn(null).withTitle('Action').notSortable()
        .renderWith(function (data, type, full, meta) {
            return '<button type="button" class="btn btn-outline btn-circle btn-sm purple EditClick" ng-click="EditClick(' + data.CitizenID + ')">' +
                '   <i class="fa fa-edit"></i>' +
                '</button>&nbsp;' +

                '<button  type="button" class="btn btn-outline btn-circle dark btn-sm black BtnClick" ng-click="DeleteClick(' + data.CitizenID + ')">' +
                '   <i class="fa fa-trash-o"></i>' +
                '</button> ';
        }),
    ];
    $scope.dtOptions = DTOptionsBuilder.newOptions().withOption('ajax', {
        dataSrc: "data",
        url: "/MasterLists/CitizenMaster/ListCitizenDetails",
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
  
     $("#NewAddBtn").click(function(){
         $("#Addnew").modal('show');
     });
     
     $scope.AddNewSave = function () {
     
         $("#CitizenForm").validate({
             rules: {
                 CitizenName: {
                     required: true
                 }
             }
         })
         if ($("#CitizenForm").valid()) {
            
             $http({
                 method: "POST",
                 url: "/MasterLists/CitizenMaster/CreateCitizen",
                 data: {
                     CitizenName: $scope.CitizenName,
                     CitizenCode: $scope.CitizenCode,
                     CitizenDesc: $scope.CitizenDesc
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
                 if (response.Result>0)
                     window.location.href = "/MasterLists/CitizenMaster/Index";
             });
         }         
     }

     $scope.EditClick = function (id) {
        
         $scope.CitizenID_edit = id;
         $http({
             method: "GET",
             url: "/MasterLists/CitizenMaster/GetDetailsForEdit",
             params: { CitizenID: id }

         }).success(function (response) {
             $scope.CitizenName_edit = response.CitizenName;
             $scope.CitizenCode_edit = response.CitizenCode;
             $scope.CitizenDesc_edit = response.CitizenDesc;
         });
         $("#EditCitizen").modal('show');
     }
     
     $scope.EditCitizenBtn = function () {

         $("#CitizenForm").validate({
             rules: {
                 CitizenName: {
                     required: true
                 }
             }
         })

         if ($("#CitizenForm").valid()) {
             $http({
                 method: "POST",
                 url: "/MasterLists/CitizenMaster/EditCitizenDetails",
                 data: {
                     CitizenID: $scope.CitizenID_edit,
                     CitizenName: $scope.CitizenName_edit,
                     CitizenCode: $scope.CitizenCode_edit,
                     CitizenDesc: $scope.CitizenDesc_edit
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
                    window.location.href = "/MasterLists/CitizenMaster/Index";
                // $("#EditCitizen").modal('hide');
             });
         }
     }

     $scope.DeleteClick = function (id) {

         $scope.CitizenId_Dlt = id;
         $http({
             method: "GET",
             url: "/MasterLists/CitizenMaster/GetCitizenName",
             params: {
                 CitizenID: id
             }
         }).success(function (response) {
             alert(response);
             $scope.DeleteCitizenName = response;
             $("#Delete").modal('show');
         });
     }

     $scope.DeleteCitizen = function () {
         
         $http({
             method: "POST",
             url: "/MasterLists/CitizenMaster/DeleteCitizen",
             params: {
                 CitizenID: $scope.CitizenId_Dlt
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
                 window.location.href = "/MasterLists/CitizenMaster/Index";
         });
     }

}]);
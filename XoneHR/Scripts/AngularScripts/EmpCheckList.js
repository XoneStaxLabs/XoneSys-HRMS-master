var app = angular.module("MyApp", ['angularUtils.directives.dirPagination']);

app.service("myService", function ($http) {

    this.getListDetails = function () {
        return $http.get("/MasterLists/EmpCheckListMaster/getListDetails");
    };    
});

app.controller("MyCntrl", function ($scope, myService) {

    //For sorting according to columns
    $scope.sort = function (keyname) {
        $scope.sortKey = keyname;
        $scope.reverse = !$scope.reverse;
    }
    
    var getData = myService.getListDetails();

    getData.then(function (Obj) {       
        $scope.CheckLists = Obj.data;
    }, function (Obj) {
        alert("Records gathering failed!");
    });

    $scope.NewAddBtn = function () {
      
    }

});
